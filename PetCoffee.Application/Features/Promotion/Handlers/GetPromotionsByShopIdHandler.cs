using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Promotion.Models;
using PetCoffee.Application.Features.Promotion.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Promotion.Handlers
{

	public class GetPromotionsByShopIdHandler : IRequestHandler<GetPromotionsByShopIdQuery, PaginationResponse<Domain.Entities.Promotion, PromotionResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentAccountService _currentAccountService;
		private readonly IMapper _mapper;

		public GetPromotionsByShopIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_currentAccountService = currentAccountService;
			_mapper = mapper;
		}
		public async Task<PaginationResponse<Domain.Entities.Promotion, PromotionResponse>> Handle(GetPromotionsByShopIdQuery request, CancellationToken cancellationToken)
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

			var Promotions = _unitOfWork.PromotionRepository
						.Get(predicate: request.GetExpressions())
						.Include(pr => pr.AccountPromotions)
						.AsQueryable();

			return new PaginationResponse<Domain.Entities.Promotion, PromotionResponse>(
					Promotions,
					request.PageNumber,
					request.PageSize,
					promotion => _mapper.Map<PromotionResponse>(promotion));
		}
	}
}
