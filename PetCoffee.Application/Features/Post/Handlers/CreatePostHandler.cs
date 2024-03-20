using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Command;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Post.Handler;

public class CreatePostHandler : IRequestHandler<CreatePostCommand, PostResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly INotifier _notifier;

	public CreatePostHandler(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService, ICurrentAccountService currentAccountService, INotifier notifier)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
		_notifier = notifier;
	}

	public async Task<PostResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
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
		if(request.ShopId != null)
		{
			var shop = await _unitOfWork.PetCoffeeShopRepository
						.Get(s => s.Id == request.ShopId && !s.Deleted && s.IsBuyPackage)
						.FirstOrDefaultAsync();
			if(shop == null)
			{
				throw new ApiException(ResponseCode.ShopIsExpired);
			}
		}
		
		var newPost = _mapper.Map<Domain.Entities.Post>(request);
		if (request.Image != null)
		{
			newPost.Image = await _azureService.UpdateloadImages(request.Image);
		}
		await _unitOfWork.PostRepository.AddAsync(newPost);
		await _unitOfWork.SaveChangesAsync();

		//add category relationship
		var listCategory = new List<Category>();
		if (request.CategoryIds != null && request.CategoryIds.Any())
		{
			foreach (var categoryId in request.CategoryIds)
			{
				var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
				if (category == null) continue;
				listCategory.Add(category);
				await _unitOfWork.PostCategoryRepository.AddAsync(new Domain.Entities.PostCategory()
				{
					CategoryId = categoryId,
					PostId = newPost.Id,
				});
			}
		}

		//add shop relationship
		var listShop = new List<PetCoffeeShop>();
		if (request.PetCafeShopTagIds != null && request.PetCafeShopTagIds.Any())
		{
			foreach (var shopId in request.PetCafeShopTagIds)
			{
				var shop = await _unitOfWork.PetCoffeeShopRepository.GetByIdAsync(shopId);
				if (shop == null) continue;
				try
				{
					listShop.Add(shop);
					await _unitOfWork.PostCoffeeShopRepository.AddAsync(new PostPetCoffeeShop()
					{
						ShopId = shopId,
						PostId = newPost.Id,
					});
				}
				catch (Exception ex)
				{
					continue;

				}
			}
		}

		await _unitOfWork.SaveChangesAsync();
		var newPostData = (await _unitOfWork.PostRepository
				.GetAsync(
					predicate: p => p.Id == newPost.Id,
					includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Post, object>>>
					{
						p => p.CreatedBy,
						p => p.PetCoffeeShop,
					})).FirstOrDefault();
		var response = _mapper.Map<PostResponse>(newPostData);


		if (listShop.Count > 0)
		{
			response.PetCoffeeShops = listShop.Select(s => _mapper.Map<CoffeeshopForPostModel>(s)).ToList();
		}
		if (listCategory.Count > 0)
		{
			response.Categories = listCategory.Select(s => _mapper.Map<CategoryForPostModel>(s)).ToList();
		}

		if (newPostData != null)
		{
			var follows = await _unitOfWork.FollowPetCfShopRepository.Get(a => a.ShopId == newPostData.ShopId)
																		.Include(a => a.CreatedBy)
																		.ToListAsync();
			foreach (var acc in follows)
			{
				var notification = new Notification(
					account: acc.CreatedBy,
					type: NotificationType.NewPost,
					entityType: EntityType.Post,
					data: newPostData
				);
				await _notifier.NotifyAsync(notification, true);
			}
		}

		return response;
	}
}
