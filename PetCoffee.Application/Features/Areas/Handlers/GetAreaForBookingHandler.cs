using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Areas.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

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


            var availableAreas = _unitOfWork.AreaRepsitory.Get(
                predicate: a => a.PetcoffeeShopId == shopId && !a.Deleted).ToList();

			var areaResponses = availableAreas
						 .Skip((request.PageNumber - 1) * request.PageSize)
						 .Take(request.PageSize);

			var response = new List<AreaResponse>();

            foreach (var area in areaResponses)
            {
                var existingReservations = await _unitOfWork.ReservationRepository
                    .GetAsync(r => r.AreaId == area.Id && (r.Status == OrderStatus.Success)
                    && (r.StartTime <= request.EndTime || r.EndTime >= request.StartTime) && r.StartTime.Date == request.StartTime.Date);

                var test = await _unitOfWork.ReservationRepository
                    .GetAsync(r => r.AreaId == area.Id);

                var totalSeated = existingReservations.Sum(x => x.BookingSeat);

                var availableSeat = area.TotalSeat - totalSeated < 0 ? 0 : area.TotalSeat - totalSeated;

                var areaResponse = _mapper.Map<AreaResponse>(area);
                areaResponse.AvailableSeat = availableSeat;
				var pets = await _unitOfWork.PetRepository
					.Get(p => p.PetAreas.Any(pa => pa.AreaId == area.Id && pa.EndTime == null) && !p.Deleted)
					.Include(p => p.PetAreas)
					.ThenInclude(pa => pa.Area)
					.Select(p => _mapper.Map<PetResponseForArea>(p))
					.ToListAsync();
                areaResponse.Pets = pets;
				response.Add(areaResponse);
            }

            return new PaginationResponse<Domain.Entities.Area, AreaResponse>(
                response,
				availableAreas.Count(),
                request.PageNumber,
                request.PageSize);
        }
    }

}


