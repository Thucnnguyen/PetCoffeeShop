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
                throw new ApiException(ResponseCode.AccountNotExist);
            }
            if (currentAccount.IsVerify)
            {
                throw new ApiException(ResponseCode.AccountNotActived);
            }

            var reservation = _unitOfWork.ReservationRepository.Get(
                predicate: p => p.Id == request.OrderId && p.CreatedById == currentAccount.Id && p.Status.Equals(OrderStatus.Success) && p.StartTime > DateTimeOffset.UtcNow,
                includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Reservation, object>>>
                {
                    p => p.ReservationProducts,

                }
                ).FirstOrDefault();
            if (reservation == null)
            {
                throw new ApiException(ResponseCode.ReservationNotExist);
            }
            decimal totalPrice = 0;
            Dictionary<long, Domain.Entities.Product> products = new();
            //new code here
			var productIds = request.Products.Select(p => p.ProductId);
			var productsDic = await _unitOfWork.ProductRepository
				.Get(pr => productIds.Contains(pr.Id) && !pr.Deleted)
				.ToDictionaryAsync(p => p.Id);
			foreach (var pro in request.Products)
            {

                var p = productsDic.ContainsKey(pro.ProductId);
                if (!p)
                {
                    throw new ApiException(ResponseCode.ProductNotExist);
                }
                products.Add(pro.ProductId, productsDic[pro.ProductId]);
                totalPrice += (decimal)productsDic[pro.ProductId].Price * pro.Quantity;
            }

            //
            var wallet = await _unitOfWork.WalletRepsitory.GetAsync(w => w.CreatedById == currentAccount.Id);
            if (!wallet.Any())
            {
                throw new ApiException(ResponseCode.NotEnoughBalance);
            }
            if (wallet.First().Balance < totalPrice)
            {
                throw new ApiException(ResponseCode.NotEnoughBalance);
            }



            //
            List<TransactionProduct> TransactionProducts = new();
            foreach (var pro in request.Products)
            {
                var existingProduct = reservation.ReservationProducts
                  .FirstOrDefault(rp => rp.ProductId == pro.ProductId &&  rp.ProductPrice == products[pro.ProductId].Price);

                //
                if (existingProduct != null)
                {
                    existingProduct.TotalProduct += pro.Quantity;
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

                TransactionProducts.Add(new TransactionProduct
                {
                    Price = products[pro.ProductId].Price,
                    ProductId = pro.ProductId,
                    ProductImage = products[pro.ProductId].Image,
					ProductName = products[pro.ProductId].Name,
					Quantity = pro.Quantity,
                });

			}
			await _unitOfWork.ReservationRepository.UpdateAsync(reservation);


			wallet.First().Balance -= totalPrice;
            await _unitOfWork.WalletRepsitory.UpdateAsync(wallet.First());


            //get manager account 
            var managerAccount = await _unitOfWork.AccountRepository
                .Get(a => a.IsManager && a.AccountShops.Any(ac => ac.ShopId == products.First().Value.PetCoffeeShopId))
                .FirstOrDefaultAsync();

            var managaerWallet = await _unitOfWork.WalletRepsitory
                .Get(w => w.CreatedById == managerAccount.Id)
                .FirstOrDefaultAsync();
            //
            
            if(managaerWallet == null)
            {
                
                var newWallet = new Wallet((decimal)totalPrice);
                await _unitOfWork.WalletRepsitory.AddAsync(newWallet);
                await _unitOfWork.SaveChangesAsync();
                newWallet.CreatedById = managerAccount.Id;
                await _unitOfWork.WalletRepsitory.UpdateAsync(newWallet);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                managaerWallet.Balance += totalPrice;
                await _unitOfWork.WalletRepsitory.UpdateAsync(managaerWallet);
            }

            reservation.TotalPrice += totalPrice;

			var newTransaction = new Domain.Entities.Transaction()
			{
				WalletId = wallet.First().Id,
				Amount = (decimal)totalPrice,
				Content = "Đặt đồ uống",
				RemitterId = managaerWallet.Id,
                PetCoffeeShopId = products.FirstOrDefault().Value.PetCoffeeShopId,
				TransactionStatus = TransactionStatus.Done,
				ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
				TransactionType = TransactionType.AddProducts,
			};
            newTransaction.TransactionProducts = TransactionProducts;

			reservation.Transactions.Add(newTransaction);
			await _unitOfWork.ReservationRepository.UpdateAsync(reservation);

			await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

}
