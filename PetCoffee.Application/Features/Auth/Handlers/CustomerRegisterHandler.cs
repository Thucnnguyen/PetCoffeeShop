using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class CustomerRegisterHandler : IRequestHandler<CustomerRegisterCommand, string>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IJwtService _jwtService;
	private readonly ILogger<CustomerRegisterHandler> _logger;
	private readonly IMapper _mapper;

	public CustomerRegisterHandler(IUnitOfWork unitOfWork, ILogger<CustomerRegisterHandler> logger, IJwtService jwtService,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
		_jwtService = jwtService;
		_logger = logger;
		_mapper = mapper;
    }
    public async Task<string> Handle(CustomerRegisterCommand request, CancellationToken cancellationToken)
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

		var newAccount = await _unitOfWork.AccountRepository.AddAsync(account);
		await _unitOfWork.SaveChangesAsync();

		var resp = _jwtService.GenerateJwtToken(newAccount);
		return resp;
	}
}
