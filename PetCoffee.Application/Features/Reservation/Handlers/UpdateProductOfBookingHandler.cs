using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
	public class UpdateProductOfBookingHandler : IRequestHandler<UpdateProductOfBookingCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;
		private readonly ICurrentAccountService _currentAccountService;

		public UpdateProductOfBookingHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_currentAccountService = currentAccountService;
		}
		public async Task<bool> Handle(UpdateProductOfBookingCommand request, CancellationToken cancellationToken)
		{
			var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
			if (currentAccount == null)
			{
<<<<<<< HEAD
				WalletId = wallet.First().Id,
				Amount = (decimal)totalPrice,
				Content = "Đặt đồ uống",
				RemitterId = managaerWallet.Id,
                PetCoffeeShopId = products.FirstOrDefault().Value.PetCoffeeShopId,
				TransactionStatus = TransactionStatus.Done,
				ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
				TransactionType = TransactionType.AddProducts,
			};
            reservation.Transactions.Add(newTransaction);
			await _unitOfWork.ReservationRepository.UpdateAsync(reservation);
=======
				throw new ApiException(ResponseCode.AccountNotExist);
			}
			if (currentAccount.IsVerify)
			{
				throw new ApiException(ResponseCode.AccountNotActived);
			}
>>>>>>> feat/bookingV03

			var reservation = _unitOfWork.ReservationRepository.Get(
				predicate: p => p.Id == request.OrderId && p.CreatedById == currentAccount.Id && p.Status.Equals(OrderStatus.Success) && p.StartTime > DateTimeOffset.UtcNow,
				includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Reservation, object>>>
				{
					p => p.ReservationProducts
				}
				).FirstOrDefault();
			if (reservation == null)
			{
				throw new ApiException(ResponseCode.ReservationNotExist);
			}
			decimal totalPrice = 0;
			Dictionary<long, Domain.Entities.Product> products = new();
			foreach (var pro in request.Products)
			{

				var p = (await _unitOfWork.ProductRepository.GetAsync(pr => pr.Id == pro.ProductId && !pr.Deleted)).FirstOrDefault();
				if (p == null)
				{
					throw new ApiException(ResponseCode.ProductNotExist);
				}
				products.Add(p.Id, p);
				totalPrice += (decimal)p.Price * pro.Quantity;

			}

			var wallet = await _unitOfWork.WalletRepsitory.GetAsync(w => w.CreatedById == currentAccount.Id);
			if (!wallet.Any())
			{
				throw new ApiException(ResponseCode.NotEnoughBalance);
			}
			if (wallet.First().Balance < totalPrice)
			{
				throw new ApiException(ResponseCode.NotEnoughBalance);
			}


			foreach (var pro in request.Products)
			{
				var existingProduct = reservation.ReservationProducts
					.FirstOrDefault(rp => rp.ProductId == pro.ProductId && rp.ProductPrice == products[pro.ProductId].Price);

				if (existingProduct != null)
				{
					if (pro.Quantity > existingProduct.TotalProduct)
					{
						var quantityDifference = pro.Quantity - existingProduct.TotalProduct;
						var priceDifference = quantityDifference * products[pro.ProductId].Price;

						wallet.First().Balance -= priceDifference;
						var managerAccount = await _unitOfWork.AccountRepository
							.GetAsync(a => a.IsManager && a.AccountShops.Any(ac => ac.ShopId == products[pro.ProductId].PetCoffeeShopId));
						if (!managerAccount.Any())
						{
							throw new ApiException(ResponseCode.AccountNotExist);
						}
						var managerWallet = await _unitOfWork.WalletRepsitory
							.GetAsync(w => w.CreatedById == managerAccount.First().Id);
						if (!managerWallet.Any())
						{
							var newManagerWallet = new Wallet(priceDifference);
							newManagerWallet.CreatedById = managerAccount.First().Id;
							await _unitOfWork.WalletRepsitory.AddAsync(newManagerWallet);
						}
						else
						{
							managerWallet.First().Balance += priceDifference;
						}

						var newTransaction = new Domain.Entities.Transaction()
						{
							WalletId = wallet.First().Id,
							Amount = (decimal)priceDifference,
							Content = "Thay đổi số lượng đồ uống",
							RemitterId = managerWallet.FirstOrDefault().Id,
							PetCoffeeShopId = products.FirstOrDefault().Value.PetCoffeeShopId,
							TransactionStatus = TransactionStatus.Done,
							ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
							TransactionType = TransactionType.AddProducts,
						};
						reservation.Transactions.Add(newTransaction);
						existingProduct.TotalProduct = pro.Quantity;
					}
					else if (pro.Quantity < existingProduct.TotalProduct)
					{
						var quantityDifference = existingProduct.TotalProduct - pro.Quantity;
						var priceDifference = quantityDifference * products[pro.ProductId].Price;

						wallet.First().Balance += priceDifference;
						var managerAccount = await _unitOfWork.AccountRepository
							.GetAsync(a => a.IsManager && a.AccountShops.Any(ac => ac.ShopId == products[pro.ProductId].PetCoffeeShopId));
						if (!managerAccount.Any())
						{
							throw new ApiException(ResponseCode.AccountNotExist);
						}
						var managerWallet = await _unitOfWork.WalletRepsitory
							.GetAsync(w => w.CreatedById == managerAccount.First().Id);
						if (!managerWallet.Any())
						{
							var newManagerWallet = new Wallet(priceDifference);
							newManagerWallet.CreatedById = managerAccount.First().Id;
							await _unitOfWork.WalletRepsitory.AddAsync(newManagerWallet);
						}
						else
						{
							managerWallet.First().Balance -= priceDifference;
						}

						var newTransaction = new Domain.Entities.Transaction()
						{
							WalletId = wallet.First().Id,
							Amount = (decimal)priceDifference,
							Content = "Thay đổi số lượng đồ uống",
							RemitterId = managerWallet.FirstOrDefault().Id,
							PetCoffeeShopId = products.FirstOrDefault().Value.PetCoffeeShopId,
							TransactionStatus = TransactionStatus.Done,
							ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
							TransactionType = TransactionType.MinusProducts,
						};
						reservation.Transactions.Add(newTransaction);

						existingProduct.TotalProduct = pro.Quantity;
					}
				}
				else
				{
					var newReservationProduct = new ReservationProduct
					{
						ProductId = pro.ProductId,
						TotalProduct = pro.Quantity,
						ProductPrice = products[pro.ProductId].Price,
					};
					reservation.ReservationProducts.Add(newReservationProduct);
				}
			}

			reservation.TotalPrice = totalPrice;

			await _unitOfWork.WalletRepsitory.UpdateAsync(wallet.First());

			await _unitOfWork.ReservationRepository.UpdateAsync(reservation);
			await _unitOfWork.SaveChangesAsync();

			return true;


		}
	}

}

