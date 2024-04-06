using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Product.Commands;
using PetCoffee.Application.Features.Promotion.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Promotion.Handlers
{
	public class DeletePromotionHandler : IRequestHandler<DeletePromotionCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentAccountService _currentAccountService;
		private readonly ICacheService _cacheService;

		public DeletePromotionHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService, ICacheService cacheService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentAccountService = currentAccountService;
			_cacheService = cacheService;
		}

		public async Task<bool> Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
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

			if (currentAccount.IsCustomer)
			{
				throw new ApiException(ResponseCode.PermissionDenied);
			};

			var promotion = (await _unitOfWork.PromotionRepository.GetAsync(p => p.Id == request.Id && !p.Deleted)).FirstOrDefault();

			if (promotion == null)
			{
				throw new ApiException(ResponseCode.PromotionNotExisted);
			}

			if (!currentAccount.AccountShops.Any(a => a.ShopId == promotion.PetCoffeeShopId))
			{
				throw new ApiException(ResponseCode.PermissionDenied);
			}

			promotion.DeletedAt = DateTimeOffset.UtcNow;
			//promotion.ProductStatus = Domain.Enums.ProductStatus.Inactive;
			await _unitOfWork.PromotionRepository.UpdateAsync(promotion);
			await _unitOfWork.SaveChangesAsync();

			return true;
		}
	}

}
