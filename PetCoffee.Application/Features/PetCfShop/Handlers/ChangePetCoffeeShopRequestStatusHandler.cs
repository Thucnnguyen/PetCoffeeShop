

using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Constantsl;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PetCfShop.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class ChangePetCoffeeShopRequestStatusHandler : IRequestHandler<ChangePetCoffeeShopRequestStatusCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;

	public ChangePetCoffeeShopRequestStatusHandler(IUnitOfWork unitOfWork, IAzureService azureService)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
	}

	public async Task<bool> Handle(ChangePetCoffeeShopRequestStatusCommand request, CancellationToken cancellationToken)
	{
		var PetCoffShopChangeStatus =await _unitOfWork.PetCoffeeShopRepository.Get(shop => shop.Id == request.ShopId && shop.Status == ShopStatus.Processing)
																			 .FirstOrDefaultAsync();
		if(PetCoffShopChangeStatus == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}
		
		if(request.Status == ShopStatus.Active)
		{
			PetCoffShopChangeStatus.Status = ShopStatus.Active;
			PetCoffShopChangeStatus.EndTimePackage = DateTime.UtcNow.AddMonths(3);
			await _unitOfWork.PetCoffeeShopRepository.UpdateAsync(PetCoffShopChangeStatus);
			var createdBy = await _unitOfWork.AccountRepository.GetByIdAsync(PetCoffShopChangeStatus.CreatedById);
			if (createdBy.IsCustomer)
			{
				createdBy.Role = Role.Manager;
				await _unitOfWork.AccountRepository.UpdateAsync(createdBy);
			}
			await _unitOfWork.SaveChangesAsync();
			
			var EmailContent = string.Format(EmailConstant.AcceptShop, createdBy.FullName);
			await _azureService.SendEmail(createdBy.Email, EmailContent, EmailConstant.EmailSubject);
			return true;
		}

		if(request.Status == ShopStatus.Cancel)
		{
			PetCoffShopChangeStatus.Status = request.Status;

			await _unitOfWork.SaveChangesAsync();
			var EmailContent = string.Format(EmailConstant.RejectShop, PetCoffShopChangeStatus.CreatedBy.FullName);
			await _azureService.SendEmail(PetCoffShopChangeStatus.CreatedBy.Email, EmailContent, EmailConstant.EmailSubject);
			return true;
		}

		PetCoffShopChangeStatus.Status = request.Status;

		await _unitOfWork.SaveChangesAsync();

		return true;
	}
}
