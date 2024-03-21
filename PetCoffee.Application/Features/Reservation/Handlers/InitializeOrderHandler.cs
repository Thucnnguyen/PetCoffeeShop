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


            //check seat is ok ?
            var isSeat = IsAreaAvailable(request.AreaId, request.StartTime, request.EndTime, request.TotalSeat);

            if (!isSeat)
            {
                throw new ApiException(ResponseCode.AreaInsufficientSeating);
            }

            //cal price in order
            TimeSpan duration = request.EndTime - request.StartTime;
            decimal durationInHours = (decimal)duration.TotalHours;
            var totalPrice = durationInHours * area.PricePerHour;

            // check wallet
            var customerWallet = (await _unitOfWork.WalletRepsitory.GetAsync(w => w.CreatedById == currentAccount.Id)).FirstOrDefault();



            if (customerWallet == null)
            {
                throw new ApiException(ResponseCode.WalletNotAvailable);
            }
            var isEnoughMoney = customerWallet.Balance >= totalPrice;
            if (!isEnoughMoney)
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
                TotalPrice = totalPrice, //  
                Discount = 0, //
                Deposit = 0, //
                Code = "test", //
                CreatedById = currentAccount.Id,
                BookingSeat = request.TotalSeat


                //CreatedAt = DateTime.Now


            };

            //await _unitOfWork.ReservationRepository.AddAsync(order);
            //await _unitOfWork.SaveChangesAsync();

            //add value to invoice table
            var invoice = new Invoice
            {
                ReservationId = order.Id,
                CreatedAt = DateTime.UtcNow,
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



        public  bool IsAreaAvailable(long areaId, DateTimeOffset startTime, DateTimeOffset endTime, int requestedSeats)
        {


            //var existingReservations = _unitOfWork.ReservationRepository
            //    .Get(r => r.AreaId == areaId && (r.Status == OrderStatus.Success || r.Status != OrderStatus.Processing) &&
            //                ((startTime >= r.StartTime && startTime < r.EndTime) ||
            //                 (endTime > r.StartTime && endTime <= r.EndTime) ||
            //                 (startTime <= r.StartTime && endTime >= r.EndTime)))
            //    .ToList();

            var existingReservations =  _unitOfWork.ReservationRepository
                 .Get(r => r.AreaId == areaId && (r.Status == OrderStatus.Success || r.Status == OrderStatus.Processing)
                 && (r.StartTime <= endTime || r.EndTime >= startTime));



        
            //


            var totalSeatsBooked = existingReservations.Sum(r => r.BookingSeat);



            var area = _unitOfWork.AreaRepsitory.Get(a => a.Id == areaId).FirstOrDefault();
            bool res = area.TotalSeat > existingReservations.Sum(r => r.BookingSeat);
            if (!res)
            {
                return false;

            }




            return true;
        }




    }
}
