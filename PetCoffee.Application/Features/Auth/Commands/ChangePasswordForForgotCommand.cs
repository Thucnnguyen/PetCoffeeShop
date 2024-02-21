
using MediatR;

namespace PetCoffee.Application.Features.Auth.Commands;

public class ChangePasswordForForgotCommand : IRequest<bool>
{
	public string Email { get; set; }
	public string NewPassword { get; set; }
}
