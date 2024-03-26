using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
	public class ReturnOrderHandler : IRequestHandler<ReturnOrderCommand, ReservationResponse>
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentAccountService _currentAccountService;

		public ReturnOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentAccountService = currentAccountService;
		}

		public async Task<ReservationResponse> Handle(ReturnOrderCommand request, CancellationToken cancellationToken)
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

			var reservation = _unitOfWork.ReservationRepository.Get(p => p.Id == request.OrderId && p.CreatedById == currentAccount.Id && p.Status.Equals(OrderStatus.Success))
				.Include(r => r.Transactions)
				.ThenInclude(t => t.Wallet)
                .Include(r => r.Transactions)
                .ThenInclude(t => t.Remitter)
                .FirstOrDefault();
			if (reservation == null)
			{
				throw new ApiException(ResponseCode.ReservationNotExistOrIsRefunded);
			}

			// check time 
			if(reservation.StartTime.UtcDateTime < DateTimeOffset.UtcNow) 
			{
                throw new ApiException(ResponseCode.ExpiredReservation);
            }


			decimal amountRefund = 0;

			if (reservation.IsTotallyRefund)
			{
				var transaction = reservation.Transactions.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Done && t.TransactionType == TransactionType.Reserve);
				if(transaction == null)
				{
					throw new ApiException(ResponseCode.TransactionNotFound);
				}


				transaction.Wallet.Balance += reservation.TotalPrice;
                transaction.Remitter.Balance -= reservation.TotalPrice;

                var newRefundTransaction = new Transaction()
                {
                    WalletId = transaction.Wallet.Id,
                    Amount = (decimal)reservation.TotalPrice,
                    Content = "Hoàn tiền đặt chỗ",
                    TransactionStatus = TransactionStatus.Done,
                    ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
                    TransactionType = TransactionType.Refund,
                };

                reservation.Transactions.Add(newRefundTransaction);
				amountRefund = newRefundTransaction.Amount;

			}
			else
			{
                var transaction = reservation.Transactions.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Done && t.TransactionType == TransactionType.Reserve);
                transaction.Wallet.Balance +=  (reservation.TotalPrice * 60)/100;
                transaction.Remitter.Balance -= (reservation.TotalPrice * 60) / 100;

                var newRefundTransaction = new Transaction()
                {
                    WalletId = transaction.Wallet.Id,
                    Amount = (decimal)(reservation.TotalPrice * 60) / 100,
                    Content = "Hoàn tiền đặt chỗ",
                    TransactionStatus = TransactionStatus.Done,
                    ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
                    TransactionType = TransactionType.Refund,
                };

                reservation.Transactions.Add(newRefundTransaction);
				amountRefund = newRefundTransaction.Amount;
			}

			reservation.Status = OrderStatus.Returned;

			var response = _mapper.Map<ReservationResponse>(reservation);
			response.AmountRefund = amountRefund;



			await _unitOfWork.ReservationRepository.UpdateAsync(reservation);
			await _unitOfWork.SaveChangesAsync();

			return response;
		}
	}
}
