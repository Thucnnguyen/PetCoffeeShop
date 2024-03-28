﻿using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
	public class RemoveProductInReservationHandler : IRequestHandler<RemoveProductInReservationCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;
		private readonly ICurrentAccountService _currentAccountService;

		public RemoveProductInReservationHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_currentAccountService = currentAccountService;
		}
		public async Task<bool> Handle(RemoveProductInReservationCommand request, CancellationToken cancellationToken)
		{
			var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
			if (currentAccount == null)
			{
				throw new ApiException(ResponseCode.AccountNotExist);
			}
			if (currentAccount.IsVerify)
			{
				throw new ApiException(ResponseCode.AccountNotActived);
			}
			var reservation = _unitOfWork.ReservationRepository.Get(
			 predicate: p => p.Id == request.ReservationId && p.CreatedById == currentAccount.Id && p.Status.Equals(OrderStatus.Success) && p.StartTime > DateTimeOffset.UtcNow,
			 includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Reservation, object>>>
			 {
					p => p.ReservationProducts
			 }
			 ).FirstOrDefault();
			if (reservation == null)
			{
				throw new ApiException(ResponseCode.ReservationNotExist);
			}
			var isExistProduct = reservation.ReservationProducts.Any(rp => rp.ProductId == request.ProductId);	
			if (!isExistProduct)
			{
				throw new ApiException(ResponseCode.ProductNotExistInReservation);
			}
		


			var product = _unitOfWork.ProductRepository.Get(rp => rp.Id == request.ProductId).Include(rp => rp.PetCoffeeShop).FirstOrDefault();
			decimal backMoney = 0;
          
				var pro =  _unitOfWork.ReservationProductRepository.Get(rp => rp.ProductId == request.ProductId && rp.ReservationId == request.ReservationId).FirstOrDefault();

				backMoney += pro.TotalProduct * pro.ProductPrice;
				_unitOfWork.ReservationProductRepository.DeleteAsync(pro);


       
			reservation.TotalPrice -= backMoney;
			
			_unitOfWork.ReservationRepository.UpdateAsync(reservation);

			
			var wallet = await _unitOfWork.WalletRepsitory.GetAsync(w => w.CreatedById == currentAccount.Id);
			if (!wallet.Any())
			{
				throw new ApiException(ResponseCode.NotEnoughBalance);
			}
		

			wallet.First().Balance += backMoney;
			await _unitOfWork.WalletRepsitory.UpdateAsync(wallet.First());

			var managerAccount = await _unitOfWork.AccountRepository
			.GetAsync(a => a.IsManager && a.AccountShops.Any(ac => ac.ShopId == product.PetCoffeeShopId));


			var managaerWallet = await _unitOfWork.WalletRepsitory
				.GetAsync(w => w.CreatedById == managerAccount.First().Id);
		

			if (managaerWallet == null)
			{

				var newWallet = new Wallet((decimal)backMoney);
				await _unitOfWork.WalletRepsitory.AddAsync(newWallet);
				await _unitOfWork.SaveChangesAsync();
				newWallet.CreatedById = managerAccount.First().Id;
				await _unitOfWork.WalletRepsitory.UpdateAsync(newWallet);
				await _unitOfWork.SaveChangesAsync();
			}
			else
			{
				managaerWallet.First().Balance -= backMoney;
				await _unitOfWork.WalletRepsitory.UpdateAsync(managaerWallet.First());
			}
			var newTransaction = new Domain.Entities.Transaction()
			{
				WalletId = wallet.First().Id,
				Amount = (decimal)backMoney,
				Content = "Xoá sản phẩm khỏi reservation booking",
				RemitterId = managaerWallet.First().Id,
				TransactionStatus = TransactionStatus.Done,
				ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
				TransactionType = TransactionType.RemoveProducts,
			};
			reservation.Transactions.Add(newTransaction);

			await _unitOfWork.SaveChangesAsync();

			return true;


		}
	}
}
