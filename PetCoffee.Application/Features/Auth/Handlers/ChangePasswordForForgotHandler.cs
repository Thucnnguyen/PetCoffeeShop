using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class ChangePasswordForForgotHandler : IRequestHandler<ChangePasswordForForgotCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public ChangePasswordForForgotHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<bool> Handle(ChangePasswordForForgotCommand request, CancellationToken cancellationToken)
	{
		var AccountIsExist = await _unitOfWork.AccountRepository.GetAsync(a => a.Email == request.Email);

		if (AccountIsExist == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}

		var account = AccountIsExist.First();
		account.Password = HashHelper.HashPassword(request.NewPassword);
		await _unitOfWork.AccountRepository.UpdateAsync(account);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
