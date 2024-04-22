using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;


namespace PetCoffee.Application.Features.Post.Handlers;

public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, PostResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;

	public UpdatePostHandler(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
	}

	public async Task<PostResponse> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
	{
		var curAccount = await _currentAccountService.GetCurrentAccount();
		if (curAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (curAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}
		// check is shop expired
		var updatePost = await _unitOfWork.PostRepository
								.Get(p => p.Id == request.PostId && !p.Deleted && p.CreatedById == curAccount.Id)
								.Include(p => p.Comments)
			.Include(p => p.Likes)
			.Include(p => p.PostCategories)
				.ThenInclude(c => c.Category)
			.Include(p => p.PostPetCoffeeShops)
				.ThenInclude(shop => shop.Shop)
			.Include(p => p.CreatedBy)
			.Include(p => p.PetCoffeeShop)
			.FirstOrDefaultAsync();

		if (updatePost == null)
		{
			throw new ApiException(ResponseCode.PostNotExisted);
		}

		updatePost.Content = request.Content;
		if (request.Image != null)
		{
			updatePost.Image = await _azureService.UpdateloadImages(request.Image);
		}
		await _unitOfWork.PostRepository.UpdateAsync(updatePost);
		await _unitOfWork.SaveChangesAsync();


		var postResponse = _mapper.Map<PostResponse>(updatePost);
		postResponse.TotalComment = updatePost.Comments.Count();
		postResponse.TotalLike = updatePost.Likes.Count();
		postResponse.IsLiked = updatePost.Likes.Any(l => l.CreatedById == curAccount.Id);
		return postResponse;
	}
}
