using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class CustomerRegisterHandler : IRequestHandler<CustomerRegisterCommand, AccessTokenResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IJwtService _jwtService;
	private readonly IMapper _mapper;

	public CustomerRegisterHandler(IUnitOfWork unitOfWork, IJwtService jwtService,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
		_jwtService = jwtService;
		_mapper = mapper;
    }
    public async Task<AccessTokenResponse> Handle(CustomerRegisterCommand request, CancellationToken cancellationToken)
	{
		var isExisted = _unitOfWork.AccountRepository.IsExisted(a => a.Email.Equals(request.Email));
		if(isExisted)
		{
			throw new ApiException(ResponseCode.AccountIsExisted);
		}
		// hash password
		var hasPassword = HashHelper.HashPassword(request.Password);
		var account = _mapper.Map<Account>(request);

		account.Password = hasPassword;
		account.Role = Role.Customer;
		var newAccount = await _unitOfWork.AccountRepository.AddAsync(account);
		await _unitOfWork.SaveChangesAsync();

		var resp = new AccessTokenResponse(_jwtService.GenerateJwtToken(newAccount));
		return resp;
	}
}
