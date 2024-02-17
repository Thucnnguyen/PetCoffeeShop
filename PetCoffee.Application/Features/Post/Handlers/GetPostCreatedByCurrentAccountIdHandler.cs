

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Application.Features.Post.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System.Linq.Expressions;

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

		var Posts = _unitOfWork.PostRepository.Get(p => p.CreatedById == currentAccount.Id)
				.Include(p => p.Comments)
				.ThenInclude(com => com.CreatedBy)
				.Include(p => p.PostCategories)
				.ThenInclude(c => c.Category)
				.Include(p => p.PostPetCoffeeShops)
				.ThenInclude(shop => shop.Shop)
				.Include(p => p.CreatedBy);

		if (Posts == null)
		{
			return new List<PostResponse>();
		}
		var response = new List<PostResponse>();

		foreach (var post in Posts)
		{
			var postResponse = _mapper.Map<PostResponse>(post);
			
			if (post.Comments != null )
			{
				postResponse.Comments = post.Comments.ToList().Select(c => _mapper.Map<CommentForPost>(c)).ToList();
			}

			if (post.PostCategories != null)
			{
				postResponse.Categories = post.PostCategories.Select(c => _mapper.Map<CategoryForPostModel>(c.Category)).ToList();
			}

			if (post.PostPetCoffeeShops != null)
			{
				postResponse.PetCoffeeShops = post.PostPetCoffeeShops.Select(c => _mapper.Map<CoffeeshopForPostModel>(c.Shop)).ToList();
			}
			if(post.CreatedBy != null)
			{
				postResponse.Account = _mapper.Map<AccountForPostModel>(post.CreatedBy);
			}
			response.Add(postResponse);
		}
		return response;
	}
}
