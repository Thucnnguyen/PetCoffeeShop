using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
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

            // check exist area
            var area = (await _unitOfWork.AreaRepsitory.GetAsync(a => !a.Deleted && a.Id == request.AreaId)).FirstOrDefault();

            if (area == null)
            {
                throw new ApiException(ResponseCode.AreaNotExist);
            }

            // check seat is ok ?
            var isSeat =  IsAreaAvailable(request.AreaId, request.StartTime, request.EndTime, request.TotalSeatBook);

            if (!isSeat)
            {
                throw new ApiException(ResponseCode.AreaInsufficientSeating);
            }

            var order = new Domain.Entities.Reservation
            {

                Status = OrderStatus.Processing,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Note = request.Note,
                AreaId = request.AreaId,
                TotalPrice = 0, //
                Discount = 0, //
                Deposit = 0, //
                Code = "test", //
                CreatedById = currentAccount.Id,
                CreatedAt = DateTime.Now,

            };

            await _unitOfWork.ReservationRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();



            // minus money in wallet for booking



            return _mapper.Map<ReservationResponse>(order);


        }



        public  bool IsAreaAvailable(long areaId, DateTime startTime, DateTime endTime, int requestedSeats)
        {
           
          
            var existingReservations =  _unitOfWork.ReservationRepository
                .Get(r => r.AreaId == areaId && (r.Status == OrderStatus.Success || r.Status != OrderStatus.Processing)  &&
                            ((startTime >= r.StartTime && startTime < r.EndTime) ||
                             (endTime > r.StartTime && endTime <= r.EndTime) ||
                             (startTime <= r.StartTime && endTime >= r.EndTime)))
                .ToList();

          
            existingReservations = existingReservations
                .Where(r => r.Status == OrderStatus.Success)
                .ToList();

          
            var totalSeatsBooked = existingReservations.Sum(r => r.TotalSeatBook);

         
            var area =  _unitOfWork.AreaRepsitory.Get(a => a.Id == areaId).FirstOrDefault();

         
            

        
            return true;
        }



    }
}
