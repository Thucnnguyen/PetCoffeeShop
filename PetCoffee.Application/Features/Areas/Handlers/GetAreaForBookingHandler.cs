using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Areas.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Areas.Handlers
{
    public class GetAreaForBookingHandler : IRequestHandler<GetAreaForBookingQuery, PaginationResponse<Domain.Entities.Area, AreaResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentAccountService _currentAccountService;

        public GetAreaForBookingHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentAccountService = currentAccountService;
        }
        public async Task<PaginationResponse<Area, AreaResponse>> Handle(GetAreaForBookingQuery request, CancellationToken cancellationToken)
        {
            var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
            if (currentAccount == null)
            {
                throw new ApiException(ResponseCode.AccountNotExist);
            }

    
            var shopId = request.ShopId;

        
            var availableAreas =  _unitOfWork.AreaRepsitory.Get(
                predicate: a => a.PetcoffeeShopId == shopId &&
                                 !a.Reservations.Any(r => !(r.StartTime >= request.EndTime || r.EndTime <= request.StartTime))).ToList();


            var response = new List<AreaResponse>();

            foreach (var area in availableAreas)
            {
                var existingReservations = await _unitOfWork.ReservationRepository
                    .GetAsync(r => r.AreaId == area.Id && (r.Status == OrderStatus.Success || r.Status == OrderStatus.Processing));

                //var totalSeatsBooked = existingReservations.Sum(r => r.TotalSeatBook);

                var availableSeat = area.TotalSeat - existingReservations.Count();

                var areaResponse = _mapper.Map<AreaResponse>(area);
                areaResponse.AvailableSeat = availableSeat;
                response.Add(areaResponse);
            }

            return new PaginationResponse<Domain.Entities.Area, AreaResponse>(
                response,
                response.Count(),
                request.PageNumber,
                request.PageSize);
        }
    }

}


//
//List<AreaResponse> response = new List<AreaResponse>(); 

//foreach(var  area in availableAreas)
//{
//    var existingReservations = await _unitOfWork.ReservationRepository
//.GetAsync(r => r.AreaId == area.Id && (r.Status == OrderStatus.Success || r.Status != OrderStatus.Processing));


//    //existingReservations = existingReservations
//    //    .Where(r => r.Status == OrderStatus.Success)
//    //    .ToList();

//    var avaliableSeat = area.TotalSeat - existingReservations.Count();
//    var rs = _mapper.Map<AreaResponse>(area);
//    rs.AvailableSeat = avaliableSeat;
//    response.Add(rs);
//}

//
//List<AreaResponse> response = new List<AreaResponse>();

//foreach (var area in availableAreas)
//{
//    var existingReservations =  _unitOfWork.ReservationRepository
//        .Get(r => r.AreaId == area.Id && (r.Status == OrderStatus.Success || r.Status == OrderStatus.Processing)).ToList();

//    var avaliableSeat = area.TotalSeat - existingReservations.Count();

//    // You may want to filter out "Processing" reservations here:
//    // existingReservations = existingReservations.Where(r => r.Status == OrderStatus.Success || r.Status == OrderStatus.Cancelled).ToList();

//    var rs = _mapper.Map<AreaResponse>(area);
//    rs.AvailableSeat = avaliableSeat;
//    response.Add(rs);
//}
//