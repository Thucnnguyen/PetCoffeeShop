

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class CreateAccountStaffPlaformHandler : IRequestHandler<CreateAccountStaffPlaformCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public CreateAccountStaffPlaformHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<bool> Handle(CreateAccountStaffPlaformCommand request, CancellationToken cancellationToken)
	{

		var checkEmail = await _unitOfWork.AccountRepository.GetAsync(a => a.Email == request.Email && !a.Deleted);
		if (checkEmail.Any())
		{
			throw new ApiException(ResponseCode.AccountIsExisted);
		}

		var newStaffPlatformAccount = _mapper.Map<Account>(request);
		newStaffPlatformAccount.Password = HashHelper.HashPassword(request.Password);
		newStaffPlatformAccount.Role = Role.PlatforStaff;
		newStaffPlatformAccount.LoginMethod = LoginMethod.UserNamePass;
		newStaffPlatformAccount.Status = AccountStatus.Active;
		newStaffPlatformAccount.PhoneNumber = "";

		await _unitOfWork.AccountRepository.AddAsync(newStaffPlatformAccount);
		await _unitOfWork.SaveChangesAsync();

		return true;
	}
}
