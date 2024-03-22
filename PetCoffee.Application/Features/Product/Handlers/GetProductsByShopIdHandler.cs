using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Product.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

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
            var Products = _unitOfWork.ProductRepository
                        .Get(predicate: request.GetExpressions())
                        //.Include(p => p.PetAreas.Where(pa => pa.EndTime == null))
                        //.ThenInclude(pa => pa.Area)
                        //.Include(p => p.PetRattings)
                        .AsQueryable();

            return new PaginationResponse<Domain.Entities.Product, ProductResponse>(
                    Products,
                    request.PageNumber,
                    request.PageSize,
                    product => _mapper.Map<ProductResponse>(product));
        }
	}

}
