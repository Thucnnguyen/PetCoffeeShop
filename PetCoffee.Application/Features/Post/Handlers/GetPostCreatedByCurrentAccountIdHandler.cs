using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Post.Handlers;

public class GetPostCreatedByCurrentAccountIdHandler : IRequestHandler<GetPostCreatedByCurrentAccountIdQuery, IList<PostResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ICurrentAccountService _currentAccountService;

	public GetPostCreatedByCurrentAccountIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
	}

	public async Task<IList<PostResponse>> Handle(GetPostCreatedByCurrentAccountIdQuery request, CancellationToken cancellationToken)
	{
		//get Current account 
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}

		var Posts = _unitOfWork.PostRepository.Get(p => p.CreatedById == currentAccount.Id && p.Status == PostStatus.Active)
				.Include(p => p.PostCategories)
				.ThenInclude(c => c.Category)
				.Include(p => p.PostPetCoffeeShops)
				.ThenInclude(shop => shop.Shop)
				.Include(p => p.CreatedBy);

		if (Posts == null)
		{
			return new List<PostResponse>();
		}
		var response = Posts.Select(post => _mapper.Map<PostResponse>(post)).ToList();


		return response;
	}
}
