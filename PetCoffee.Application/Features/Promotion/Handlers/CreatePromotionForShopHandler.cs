using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Product.Commands;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Promotion.Commands;
using PetCoffee.Application.Features.Promotion.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Promotion.Handlers
{
	public class CreatePromotionForShopHandler : IRequestHandler<CreatePromotionForShopCommand, PromotionResponse>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAzureService _azureService;
		private readonly ICurrentAccountService _currentAccountService;
		private readonly IMapper _mapper;

		public CreatePromotionForShopHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_azureService = azureService;
			_currentAccountService = currentAccountService;
			_mapper = mapper;
		}
		public async Task<PromotionResponse> Handle(CreatePromotionForShopCommand request, CancellationToken cancellationToken)
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

			if (!currentAccount.AccountShops.Any(a => a.ShopId == request.PetCoffeeShopId))  
			{
				throw new ApiException(ResponseCode.PermissionDenied);
			};

			var PetCoffeeShop = await _unitOfWork.PetCoffeeShopRepository.GetAsync(s => s.Id == request.PetCoffeeShopId && s.Status == ShopStatus.Active);
			if (PetCoffeeShop == null)
			{
				throw new ApiException(ResponseCode.ShopNotExisted);
			}

			var NewPromotion = _mapper.Map<Domain.Entities.Promotion>(request);

			NewPromotion.Code = TokenUltils.GenerateOTPCode(8);

			await _unitOfWork.PromotionRepository.AddAsync(NewPromotion);
			await _unitOfWork.SaveChangesAsync();
			var response = _mapper.Map<PromotionResponse>(NewPromotion);

			response.CreatedById = currentAccount.Id;

			return response;

		}
	}
}
