
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Application.Features.Moment.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using TmsApi.Common;

namespace PetCoffee.Application.Features.Moment.Handlers;

public class UpdateMomentHandler : IRequestHandler<UpdateMomentCommand, MomentResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public UpdateMomentHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<MomentResponse> Handle(UpdateMomentCommand request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
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
		//check moment info
		var UpdateMoment = await _unitOfWork.MomentRepository.Get(m => m.Id == request.Id)
								.Include(m => m.Pet)
								.FirstOrDefaultAsync();
		if (UpdateMoment == null)
		{
			throw new ApiException(ResponseCode.MomentNotExisted);
		}

		//check permission
		if (!currentAccount.AccountShops.Any(a => a.ShopId == UpdateMoment.Pet.PetCoffeeShopId))
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}

		Assign.Partial<UpdateMomentCommand,Domain.Entities.Moment>(request, UpdateMoment);

		if(request.NewImages != null)
		{
			UpdateMoment.Image = await _azureService.UpdateloadImages(request.NewImages);
		}

		await _unitOfWork.MomentRepository.UpdateAsync(UpdateMoment);
		await _unitOfWork.SaveChangesAsync();
		return _mapper.Map<MomentResponse>(UpdateMoment);
	}
}
