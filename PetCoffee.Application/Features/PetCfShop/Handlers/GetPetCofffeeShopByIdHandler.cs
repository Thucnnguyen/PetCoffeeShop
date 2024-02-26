
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Shared.Ultils;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetPetCofffeeShopByIdHandler : IRequestHandler<GetPetCoffeeShopByIdQuery, PetCoffeeShopResponse>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _accountService;

	public GetPetCofffeeShopByIdHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService accountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_accountService = accountService;
	}

	public async Task<PetCoffeeShopResponse> Handle(GetPetCoffeeShopByIdQuery request, CancellationToken cancellationToken)
	{
		var CurrentUser = await _accountService.GetCurrentAccount();
		if (CurrentUser == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (CurrentUser.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}
		var CurrentShop = (await _unitOfWork.PetCoffeeShopRepository.GetAsync(
			predicate: p => p.Id == request.Id,
			includes:  new List<Expression<Func<PetCoffeeShop, object>>>()
				{
					shop => shop.CreatedBy
				},
			disableTracking: true
			)).FirstOrDefault();

		if (CurrentShop == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}
		var response = _mapper.Map<PetCoffeeShopResponse>(CurrentShop);
		if(request.Longitude != 0 && request.Latitude != 0)
		{
			response.Distance = CalculateDistanceUltils.CalculateDistance(request.Latitude, request.Longitude, CurrentShop.Latitude, CurrentShop.Longitude);
		}
		response.TotalFollow = await _unitOfWork.FollowPetCfShopRepository.CountAsync(f => f.ShopId == request.Id);
		response.IsFollow = (await _unitOfWork.FollowPetCfShopRepository.GetAsync(s => s.CreatedById == CurrentUser.Id && s.ShopId == CurrentShop.Id)).Any();
		response.CreatedBy = _mapper.Map<AccountForPostModel>(CurrentShop.CreatedBy);
		return response;
	}
}
