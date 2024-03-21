
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using TmsApi.Common;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, AccountResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly IMapper _mapper;
	private readonly ICurrentAccountService _currentAccountService;

	public UpdateAccountHandler(IUnitOfWork unitOfWork, IAzureService azureService, IMapper mapper, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
	}

	public async Task<AccountResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
	{
		var CurrentAccount = await _currentAccountService.GetCurrentAccount();
		if (CurrentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (CurrentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		Assign.Partial<UpdateAccountCommand, Account>(request, CurrentAccount);

		if (request.AvatarFile != null)
		{
			await _azureService.CreateBlob(request.AvatarFile.FileName, request.AvatarFile);
			CurrentAccount.Avatar = await _azureService.GetBlob(request.AvatarFile.FileName);
		}
		if (request.BackgroundFile != null)
		{
			await _azureService.CreateBlob(request.BackgroundFile.FileName, request.BackgroundFile);
			CurrentAccount.Background = await _azureService.GetBlob(request.BackgroundFile.FileName);
		}

		await _unitOfWork.AccountRepository.UpdateAsync(CurrentAccount);
		await _unitOfWork.SaveChangesAsync();

		return _mapper.Map<AccountResponse>(CurrentAccount);
	}
}
