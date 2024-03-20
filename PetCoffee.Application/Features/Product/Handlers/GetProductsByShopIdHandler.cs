using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Pet.Queries;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Product.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Product.Handlers
{

    public class GetProductsByShopIdHandler : IRequestHandler<GetProductsByShopIdQuery, PaginationResponse<Domain.Entities.Product, ProductResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentAccountService _currentAccountService;
        private readonly IMapper _mapper;

        public GetProductsByShopIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentAccountService = currentAccountService;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<Domain.Entities.Product, ProductResponse>> Handle(GetProductsByShopIdQuery request, CancellationToken cancellationToken)
        {
            var currentAccount = await _currentAccountService.GetCurrentAccount();
            if (currentAccount == null)
            {
                throw new ApiException(ResponseCode.AccountNotExist);
            }
            if (currentAccount.IsVerify)
            {
                throw new ApiException(ResponseCode.AccountNotActived);
            }
            var PetCoffeeShop = await _unitOfWork.PetCoffeeShopRepository.GetAsync(s => s.Id == request.ShopId);
            if (!PetCoffeeShop.Any())
            {
                throw new ApiException(ResponseCode.ShopNotExisted);
            }
            //var Products = await _unitOfWork.ProductRepository
            //            .GetAsync(
            //                    predicate: p => p.pe == request.ShopId && !p.Deleted,
            //                    includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Product, object>>>
            //                    {
            //                    p => p.Area
            //                    });
            //return new PaginationResponse<Domain.Entities.Product, ProductResponse>(
            //        Products,
            //        request.PageNumber,
            //        request.PageSize,
            //        pet => _mapper.Map<ProductResponse>(pet));
            return null;
        }
    }

}
