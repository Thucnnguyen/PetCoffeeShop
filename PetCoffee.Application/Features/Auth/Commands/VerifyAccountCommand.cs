
using MediatR;

namespace PetCoffee.Application.Features.Auth.Commands;

public class VerifyAccountCommand : IRequest<bool>
{
	public string OTP { get; set; }
}
