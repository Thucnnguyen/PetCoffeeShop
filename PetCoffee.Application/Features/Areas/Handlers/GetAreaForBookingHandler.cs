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

        
            var availableAreas = await _unitOfWork.AreaRepsitory.GetAsync(
                predicate: a => a.PetcoffeeShopId == shopId &&
                                 !a.Reservations.Any(r => !(r.StartTime >= request.EndTime || r.EndTime <= request.StartTime)));

    
            var response = _mapper.Map<List<AreaResponse>>(availableAreas);

            return new PaginationResponse<Domain.Entities.Area, AreaResponse>(
                response,
                response.Count(),
                request.PageNumber,
                request.PageSize);
        }
    }

}
