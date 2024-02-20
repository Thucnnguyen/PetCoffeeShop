using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Command;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Post.Handler;

public class CreatePostHandler : IRequestHandler<CreatePostCommand, PostResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;
	public CreatePostHandler(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
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
		var newPost = _mapper.Map<Domain.Entities.Post>(request);
		newPost.Image = await _azureService.UpdateloadImages(request.Image);
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
		if (request.PetCafeShopIds != null && request.PetCafeShopIds.Any())
		{
			foreach (var shopId in request.PetCafeShopIds)
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
		var response = _mapper.Map<PostResponse>(newPost);
		response.CreatedById = curAccount.CreatedById;
		response.Account = _mapper.Map<AccountForPostModel>(curAccount);

		if(listShop.Count > 0)
		{
			response.PetCoffeeShops = listShop.Select(s => _mapper.Map<CoffeeshopForPostModel>(s)).ToList();	
		}
		if (listCategory.Count > 0)
		{
			response.Categories = listCategory.Select(s => _mapper.Map<CategoryForPostModel>(s)).ToList();
		}

		return response;
	}
}
