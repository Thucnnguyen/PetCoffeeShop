using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace PetCoffee.Application.Features.PetCfShop.Handlers
{
    public class GetAllPetCfShopHandler : IRequestHandler<GetAllPetCfShopQuery, PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetAllPetCfShopHandler(
        IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>> Handle(GetAllPetCfShopQuery request,
            CancellationToken cancellationToken)
        {
            var stores = await _unitOfWork.PetCoffeeShopRepository.GetAsync(
                predicate: request.GetExpressions(),
                orderBy: request.GetOrder(),
                includes: new List<Expression<Func<PetCoffeeShop, object>>>()
                {
                //store => store.Location,
                //    store => store.Location.Province,
                //store => store.Location.District,
                //store => store.Location.Ward,
                },
                disableTracking: true
            );

            return new PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>(
                stores,
                request.PageNumber,
                request.PageSize,
                entity => _mapper.Map<PetCoffeeShopResponse>(entity));
        }
    }
}
