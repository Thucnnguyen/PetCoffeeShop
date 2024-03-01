
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class VerifyFirebaseTokenHandler : IRequestHandler<VerifyFirebaseTokenCommand, AccessTokenResponse>
{
	private readonly IFirebaseService _firebaseService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IJwtService _jwtService;

	public VerifyFirebaseTokenHandler(IFirebaseService firebaseService, IUnitOfWork unitOfWork, IJwtService jwtService)
	{
		_firebaseService = firebaseService;
		_unitOfWork = unitOfWork;
		_jwtService = jwtService;
	}

	public async Task<AccessTokenResponse> Handle(VerifyFirebaseTokenCommand request, CancellationToken cancellationToken)
	{
		var userRecord = await _firebaseService.VerifyFirebaseToken(request.FirebaseToken);

		if(userRecord == null)
		{
			throw new ApiException(ResponseCode.FirebaseTokenNotValid);
		}

		var ExistedAccount = await _unitOfWork.AccountRepository
			.Get(a => a.Email == userRecord.Email && a.LoginMethod == LoginMethod.FirebaseEmail)
			.FirstOrDefaultAsync();

			if(ExistedAccount != null)
			{
				var resp = new AccessTokenResponse(_jwtService.GenerateJwtToken(ExistedAccount));
				return resp;
			}

			var NewAccount = new Account() 
			{
				FullName = userRecord.DisplayName, 
				Email = userRecord.Email, 
				PhoneNumber = string.IsNullOrEmpty(userRecord.PhoneNumber) ? "" : userRecord.PhoneNumber,
				Avatar = userRecord.PhotoUrl,
				Password = "",
				LoginMethod = LoginMethod.FirebaseEmail,
				
			};

			await _unitOfWork.AccountRepository.AddAsync(NewAccount);
			await _unitOfWork.SaveChangesAsync();

			var response = new AccessTokenResponse(_jwtService.GenerateJwtToken(NewAccount));
			return response;

	}
}
