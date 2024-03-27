
using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.Auth.Models;

namespace PetCoffee.Application.Features.Auth.Commands;

public class LoginUserNamePassValidation : AbstractValidator<LoginEmailPassCommand>
{
	public LoginUserNamePassValidation()
	{
		RuleFor(model => model.Email).NotEmpty();
		RuleFor(model => model.Password).NotEmpty();

	}
}

public class LoginEmailPassCommand : IRequest<AccessTokenResponse>
{
	public string Email { get; set; }
	public string Password { get; set; }
}
