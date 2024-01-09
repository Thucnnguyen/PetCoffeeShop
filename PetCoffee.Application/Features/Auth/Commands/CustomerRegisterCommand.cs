
using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.Auth.Models;

namespace PetCoffee.Application.Features.Auth.Commands;


public class CustomerRegisterCommandValidator : AbstractValidator<CustomerRegisterCommand>
{
    public CustomerRegisterCommandValidator()
    {
		RuleFor(model => model.Password).NotEmpty();
		RuleFor(model => model.Email).NotEmpty();

		RuleFor(model => model.PhoneNumber)
		.NotEmpty()
		.Matches("^\\d{10}$")  
		.WithMessage("Hãy nhập đúng số điện thoại gồm 10 chữ số");
	}
}

public class CustomerRegisterCommand : IRequest<string>
{
	public string? FullName { get; set; }
	public string? PhoneNumber { get; set; }
	public string Password { get; set; }
	public string Email { get; set; }
	public string? Avatar { get; set; }
}
