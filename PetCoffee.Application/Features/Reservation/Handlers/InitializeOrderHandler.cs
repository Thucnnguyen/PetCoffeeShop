using AutoMapper;
using MediatR;
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
	public class InitializeOrderHandler : IRequestHandler<InitializeOrderCommand, ReservationResponse>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentAccountService _currentAccountService;

		public InitializeOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentAccountService = currentAccountService;
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

			//cal price in order
			TimeSpan duration = request.EndTime - request.StartTime;
			decimal durationInHours = (decimal)duration.TotalHours;
			var totalPrice = (durationInHours * area.PricePerHour) * request.TotalSeat;

			// check wallet
			var customerWallet = (await _unitOfWork.WalletRepsitory.GetAsync(w => w.CreatedById == currentAccount.Id)).FirstOrDefault();



			if (customerWallet == null)
			{
				var newWalet = new Wallet();
				await _unitOfWork.WalletRepsitory.AddAsync(newWalet);
				customerWallet = newWalet;
			}
			//check balance
			var isEnoughMoney = customerWallet.Balance >= totalPrice;
			if (customerWallet.Balance < totalPrice)
			{
				var processingOrder = new Domain.Entities.Reservation
				{
					Status = OrderStatus.Processing,
					StartTime = request.StartTime,
					EndTime = request.EndTime,
					Note = request.Note,
					AreaId = request.AreaId,
					TotalPrice = totalPrice,
					Discount = 0,
					Code = TokenUltils.GenerateOTPCode(6),
					BookingSeat = request.TotalSeat
				};
				throw new ApiException(ResponseCode.NotEnoughBalance);
			}

			var order = new Domain.Entities.Reservation
			{

				Status = OrderStatus.Success,
				StartTime = request.StartTime,
				EndTime = request.EndTime,
				Note = request.Note,
				AreaId = request.AreaId,
				TotalPrice = totalPrice, //  
				Discount = 0, //
				Code = TokenUltils.GenerateOTPCode(6), //
				BookingSeat = request.TotalSeat
			};


			//add value to invoice table
			var invoice = new Invoice
			{
				ReservationId = order.Id,
				CreatedAt = DateTimeOffset.UtcNow,
				TotalAmount = totalPrice
			};

			order.Invoices.Add(invoice);


			//await _unitOfWork.InvoiceRepository.AddAsync(invoice);
			//await _unitOfWork.SaveChangesAsync();

			//add value to transaction table 

			var newTransaction = new Domain.Entities.Transaction()
			{
				WalletId = currentAccount.Id,
				Amount = (decimal)totalPrice,
				Content = "Đặt chỗ đơn hàng",
				RemitterId = customerWallet.Id,
				TransactionStatus = TransactionStatus.Done,
				ReferenceTransactionId = Guid.NewGuid().ToString(),
				TransactionType = TransactionType.Reserve,
				ReservationId = order.Id,
			};

			order.Transactions.Add(newTransaction);
			//await _unitOfWork.TransactionRepository.AddAsync(newTransaction);
			//await _unitOfWork.SaveChangesAsync();


			// minus money in wallet for booking

			customerWallet.Balance -= (decimal)totalPrice;
			await _unitOfWork.WalletRepsitory.UpdateAsync(customerWallet);


			await _unitOfWork.ReservationRepository.AddAsync(order);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<ReservationResponse>(order);




		}



		public bool IsAreaAvailable(long areaId, DateTimeOffset startTime, DateTimeOffset endTime, int requestedSeats, Area area)
		{


			//var existingReservations = _unitOfWork.ReservationRepository
			//    .Get(r => r.AreaId == areaId && (r.Status == OrderStatus.Success || r.Status != OrderStatus.Processing) &&
			//                ((startTime >= r.StartTime && startTime < r.EndTime) ||
			//                 (endTime > r.StartTime && endTime <= r.EndTime) ||
			//                 (startTime <= r.StartTime && endTime >= r.EndTime)))
			//    .ToList();



			var existingReservations = _unitOfWork.ReservationRepository
				 .Get(r => r.AreaId == areaId && (r.Status == OrderStatus.Success || r.Status == OrderStatus.Processing)
				 && (r.StartTime <= endTime || r.EndTime >= startTime) && r.StartTime == startTime.Date);


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

