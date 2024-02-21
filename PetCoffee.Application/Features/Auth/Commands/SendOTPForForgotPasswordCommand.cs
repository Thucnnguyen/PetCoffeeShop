using MediatR;

namespace PetCoffee.Application.Features.Auth.Commands;

public class SendOTPForForgotPasswordCommand : IRequest<bool>
{
	public string Email { get; set; }
}
