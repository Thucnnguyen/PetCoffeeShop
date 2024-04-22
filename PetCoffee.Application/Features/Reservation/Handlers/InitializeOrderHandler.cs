using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
	public class InitializeOrderHandler : IRequestHandler<InitializeOrderCommand, ReservationResponse>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentAccountService _currentAccountService;
		private readonly ISchedulerService _schedulerService;

		public InitializeOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService, ISchedulerService schedulerService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentAccountService = currentAccountService;
			_schedulerService = schedulerService;
		}
		public async Task<ReservationResponse> Handle(InitializeOrderCommand request, CancellationToken cancellationToken)
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
			if (!currentAccount.Role.Equals(Role.Customer))
			{
				throw new ApiException(ResponseCode.PermissionDenied);
			}


			// check wallet
			var customerWallet = (await _unitOfWork.WalletRepsitory.GetAsync(w => w.CreatedById == currentAccount.Id)).FirstOrDefault();

			if (customerWallet == null)
			{
				throw new ApiException(ResponseCode.NotEnoughBalance);
			}

			//check exist area
			var area = (await _unitOfWork.AreaRepsitory.GetAsync(a => !a.Deleted && a.Id == request.AreaId)).FirstOrDefault();

			if (area == null)
			{
				throw new ApiException(ResponseCode.AreaNotExist);
			}

			if (area.TotalSeat < request.TotalSeat)
			{
				throw new ApiException(ResponseCode.AreaInsufficientSeating);
			}


			//check seat is ok ?
			var isSeat = IsAreaAvailable(request.AreaId, request.StartTime, request.EndTime, request.TotalSeat, area);

			if (!isSeat)
			{
				throw new ApiException(ResponseCode.AreaInsufficientSeating);
			}




			// check voucher is valid 
			var promotion = new Domain.Entities.Promotion();

			if (request.PromotionId != null)
			{
				var pro = (await _unitOfWork.PromotionRepository
					.GetAsync(
						predicate: p => !p.Deleted && p.Id == request.PromotionId
									&& p.PetCoffeeShopId == area.PetcoffeeShopId
									&& p.To >= DateTimeOffset.Now && p.From <= DateTimeOffset.Now,
						includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Promotion, object>>>()
						{
							p => p.AccountPromotions.Where(ap => ap.AccountId == currentAccount.Id),
						}
					))
					.FirstOrDefault();
				if (pro == null)
				{
					throw new ApiException(ResponseCode.PromotionNotExisted);
				}

				if (pro.AccountPromotions.Any())
				{
					throw new ApiException(ResponseCode.PromotionWasUsed);
				}

				promotion = pro;
			}



			//calculate price in order
			TimeSpan duration = request.EndTime - request.StartTime;
			decimal durationInHours = (decimal)duration.TotalHours;
			var totalPrice = Math.Round((durationInHours * area.PricePerHour) * request.TotalSeat, 2, MidpointRounding.AwayFromZero);


			//check balance
			//var isEnoughMoney = customerWallet.Balance >= totalPrice;
			if (customerWallet.Balance < totalPrice)
			{
				throw new ApiException(ResponseCode.NotEnoughBalance);
			}

			var order = new Domain.Entities.Reservation
			{
				Status = OrderStatus.Success,
				StartTime = request.StartTime,
				EndTime = request.EndTime,
				Note = request.Note,
				AreaId = request.AreaId,
				TotalPrice = request.PromotionId == null ? totalPrice : totalPrice - (totalPrice * promotion.Percent) / 100, //  
				Discount = request.PromotionId == null ? 0 : (totalPrice * promotion.Percent) / 100, //
				Code = TokenUltils.GenerateCodeForOrder(), // code to search
				BookingSeat = request.TotalSeat,
				PromotionId = request.PromotionId != null ? promotion.Id : null,
			};

			//Get ManagerAccount
			var managerAccount = await _unitOfWork.AccountRepository
			.Get(a => a.IsManager && a.AccountShops.Any(ac => ac.ShopId == area.PetcoffeeShopId))
			.FirstOrDefaultAsync();

			var managaerWallet = await _unitOfWork.WalletRepsitory
			.Get(w => w.CreatedById == managerAccount.Id)
			.FirstOrDefaultAsync();
			if (managaerWallet == null)
			{
				//add new Wallet
				var newManagerWallet = new Wallet();
				await _unitOfWork.WalletRepsitory.AddAsync(newManagerWallet);
				await _unitOfWork.SaveChangesAsync();
				newManagerWallet.CreatedById = managerAccount.Id;
				await _unitOfWork.WalletRepsitory.UpdateAsync(newManagerWallet);
				await _unitOfWork.SaveChangesAsync();
				managaerWallet = newManagerWallet;
			}
			//add value to transaction table 

			var newTransaction = new Domain.Entities.Transaction()
			{
				WalletId = customerWallet.Id,
				Amount = (decimal)order.TotalPrice,
				Content = "Đặt chỗ",
				RemitterId = managaerWallet.Id,
				PetCoffeeShopId = area.PetcoffeeShopId,
				TransactionStatus = TransactionStatus.Done,
				ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
				TransactionType = TransactionType.Reserve,
			};

			order.Transactions.Add(newTransaction);

			// minus money in wallet for booking

			customerWallet.Balance -= (decimal)order.TotalPrice;
			managaerWallet.Balance += (decimal)order.TotalPrice;
			await _unitOfWork.WalletRepsitory.UpdateAsync(customerWallet);
			await _unitOfWork.WalletRepsitory.UpdateAsync(managaerWallet);


			await _unitOfWork.ReservationRepository.AddAsync(order);

			// save accountPromotion
			if (request.PromotionId != null)
			{
				AccountPromotion accountPromotion = new AccountPromotion
				{
					AccountId = currentAccount.Id,
					PromotionId = promotion.Id,
				};
				await _unitOfWork.AccountPromotionRepository.AddAsync(accountPromotion);
			}


			await _unitOfWork.SaveChangesAsync();
			await _schedulerService.SetReservationToOvertime(order.Id, order.EndTime.UtcDateTime.AddMinutes(1));
			// get order just inserted
			var reservations = await _unitOfWork.ReservationRepository.Get(
					 predicate: o => o.Id == order.Id,
					 includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Reservation, object>>>
					 {
								 p => p.CreatedBy,
								 o => o.Area,
								 o => o.Area.PetCoffeeShop
					 },
					 disableTracking: true)
				.FirstOrDefaultAsync();

			var response = _mapper.Map<ReservationResponse>(reservations);
			response.AreaResponse = _mapper.Map<AreaResponse>(area);
			response.AccountForReservation = _mapper.Map<AccountForReservation>(reservations.CreatedBy);
			response.PetCoffeeShopResponse = _mapper.Map<PetCoffeeShopResponse>(reservations.Area.PetCoffeeShop);
			return response;
		}


		public bool IsAreaAvailable(long areaId, DateTimeOffset startTime, DateTimeOffset endTime, int requestedSeats, Area area)
		{

			var existingReservations = _unitOfWork.ReservationRepository
				 .Get(r => r.AreaId == areaId && (r.Status == OrderStatus.Success)
				 && (r.StartTime <= endTime || r.EndTime >= startTime) && r.StartTime.Date == startTime.Date);


			var totalSeatsBooked = existingReservations.Sum(r => r.BookingSeat);

			bool res = requestedSeats <= area.TotalSeat - totalSeatsBooked;
			if (!res)
			{
				return false;
			}

			return true;
		}
	}
}

