
using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.Auth.Models;

namespace PetCoffee.Application.Features.Auth.Commands;

public class LoginUserNamePassValidation : AbstractValidator<LoginUsernamePassCommand>
{
    public LoginUserNamePassValidation()
    {
        RuleFor(model => model.Username).NotEmpty();
        RuleFor(model => model.Password).NotEmpty();

    }
}

public class LoginUsernamePassCommand : IRequest<AccessTokenResponse>
{
	public string Username { get; set; }	
	public string Password { get; set; }
}
