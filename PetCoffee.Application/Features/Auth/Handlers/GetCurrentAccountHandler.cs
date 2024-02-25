using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Auth.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class GetCurrentAccountHandler : IRequestHandler<GetCurrentAccountInfomationQuery, AccountResponse>
{
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public GetCurrentAccountHandler(IMapper mapper, ICurrentAccountService currentAccountService, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_currentAccountService = currentAccountService;
		_unitOfWork = unitOfWork;
	}

	public async Task<AccountResponse> Handle(GetCurrentAccountInfomationQuery request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.Unauthorized);
		}

		var response =  _mapper.Map<AccountResponse>(currentAccount);
		response.TotalIsFollowing = await _unitOfWork.FollowPetCfShopRepository.CountAsync(f => f.CreatedById == currentAccount.Id); ;
		return response;
	}
}
