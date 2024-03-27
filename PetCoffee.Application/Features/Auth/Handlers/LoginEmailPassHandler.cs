
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class LoginEmailPassHandler : IRequestHandler<LoginEmailPassCommand, AccessTokenResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IJwtService _jwtService;
	private readonly IMapper _mapper;

	public LoginEmailPassHandler(IUnitOfWork unitOfWork, IJwtService jwtService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_jwtService = jwtService;
		_mapper = mapper;
	}

	public async Task<AccessTokenResponse> Handle(LoginEmailPassCommand request, CancellationToken cancellationToken)
	{
		var isExisted = await _unitOfWork.AccountRepository.GetUserByUserNameAndPassword(request.Email, request.Password);
		if (isExisted == null)
		{
			throw new ApiException(ResponseCode.LoginFailed);
		}
		if (!HashHelper.CheckHashPwd(request.Password, isExisted.Password))
		{
			throw new ApiException(ResponseCode.LoginFailed);
		}
		var resp = new AccessTokenResponse(_jwtService.GenerateJwtToken(isExisted));
		return resp;
	}
}
