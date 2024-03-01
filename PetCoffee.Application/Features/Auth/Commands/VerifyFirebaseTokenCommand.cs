
using MediatR;
using PetCoffee.Application.Features.Auth.Models;

namespace PetCoffee.Application.Features.Auth.Commands;

public class VerifyFirebaseTokenCommand : IRequest<AccessTokenResponse>
{
	public string FirebaseToken { get; set; }
}
