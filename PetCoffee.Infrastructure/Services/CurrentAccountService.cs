
using Microsoft.Extensions.DependencyInjection;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Infrastructure.Services;

public class CurrentAccountService : ICurrentAccountService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly ICurrentPrincipalService _currentPrincipalService;

	public CurrentAccountService(IServiceScopeFactory serviceScopeFactory,
		ICurrentPrincipalService currentPrincipalService)
	{
		_serviceScopeFactory = serviceScopeFactory;
		_currentPrincipalService = currentPrincipalService;
	}

	public async Task<Account?> GetCurrentAccount()
	{
		using var scope = _serviceScopeFactory.CreateScope();
		var serviceProvider = scope.ServiceProvider;
		var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
		var currentAccountId = _currentPrincipalService.CurrentSubjectId;
		if (currentAccountId == null)
		{
			return null;
		}
		return await unitOfWork.AccountRepository.GetByIdAsync(currentAccountId);
	}

	public async Task<Account> GetRequiredCurrentAccount()
	{
		return await GetCurrentAccount() ?? throw new ApiException(ResponseCode.Unauthorized);
	}
}
