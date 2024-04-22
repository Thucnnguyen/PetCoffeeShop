using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PetCfShop.Commands;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class CreatePetCfShopHandler : IRequestHandler<CreatePetCfShopCommand, PetCoffeeShopResponse>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IAzureService _azureService;
	private readonly IVietQrService _vietQrService;
	private readonly INotifier _notifier;
	public CreatePetCfShopHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IAzureService azureService, IVietQrService vietQrService, INotifier notifier)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_azureService = azureService;
		_vietQrService = vietQrService;
		_notifier = notifier;
	}

	public async Task<PetCoffeeShopResponse> Handle(CreatePetCfShopCommand request, CancellationToken cancellationToken)
	{
		var CurrentUser = await _currentAccountService.GetCurrentAccount();
		if (CurrentUser == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (CurrentUser.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		var isHasRequest = await _unitOfWork.PetCoffeeShopRepository.Get(p => p.CreatedById == CurrentUser.Id && p.Status == ShopStatus.Processing)
								.AnyAsync();
		if (isHasRequest)
		{
			throw new ApiException(ResponseCode.HasShopRequest);
		}
		var NewPetCoffeeShop = _mapper.Map<PetCoffeeShop>(request);
		NewPetCoffeeShop.Status = ShopStatus.Processing;
		NewPetCoffeeShop.OpeningTime = request.StartTime;
		NewPetCoffeeShop.ClosedTime = request.EndTime;
		//check TaxCode 
		var TaxCodeResponse = await _vietQrService.CheckQrCode(request.TaxCode);

		if (TaxCodeResponse == null || TaxCodeResponse.Code == "51")
		{
			throw new ApiException(ResponseCode.TaxCodeNotExisted);
		}

		//upload avatar
		if (request.Avatar != null)
		{
			await _azureService.CreateBlob(request.Avatar.FileName, request.Avatar);
			NewPetCoffeeShop.AvatarUrl = await _azureService.GetBlob(request.Avatar.FileName);
		}
		//upload background
		if (request.Background != null)
		{
			await _azureService.CreateBlob(request.Background.FileName, request.Background);
			NewPetCoffeeShop.BackgroundUrl = await _azureService.GetBlob(request.Background.FileName);
		}


		var NewAccountShop = new AccountShop()
		{
			AccountId = CurrentUser.Id,
			ShopId = NewPetCoffeeShop.Id
		};
		NewPetCoffeeShop.AccountShops.Add(NewAccountShop);
		await _unitOfWork.PetCoffeeShopRepository.AddAsync(NewPetCoffeeShop);
		await _unitOfWork.SaveChangesAsync();
		var adminAndStaffPlat = await _unitOfWork.AccountRepository
								.Get(a => (a.IsPlatformStaff ||a.IsAdmin) && !a.Deleted && a.Status == AccountStatus.Active)
								.ToListAsync();
		NewPetCoffeeShop.CreatedBy = CurrentUser;
		if (adminAndStaffPlat.Any())
		{
			foreach (var account in adminAndStaffPlat)
			{
				var notificationForReply = new Notification(
					account: account,
					type: NotificationType.NewShopRequest,
					entityType: EntityType.Shop,
					data: NewPetCoffeeShop,
					shopId: NewPetCoffeeShop.Id
				);
				await _notifier.NotifyAsync(notificationForReply, true);
			}
		}

		var response = _mapper.Map<PetCoffeeShopResponse>(NewPetCoffeeShop);
		response.CreatedById = CurrentUser.Id;
		return response;
	}
}
