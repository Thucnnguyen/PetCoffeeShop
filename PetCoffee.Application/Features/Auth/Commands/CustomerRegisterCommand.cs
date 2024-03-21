using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Auth.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Application.Features.Auth.Commands;

public class CustomerRegisterCommandValidator : AbstractValidator<CustomerRegisterCommand>
{
    public CustomerRegisterCommandValidator()
    {
        RuleFor(model => model.Password).NotEmpty().WithMessage("Password không được để trống");
        RuleFor(model => model.Password).MinimumLength(6).WithMessage("Password có độ dài tối thiểu là 6");

        RuleFor(model => model.Email).NotEmpty();

        RuleFor(model => model.PhoneNumber)
        .NotEmpty()
        .Matches("^\\d{10}$")
        .WithMessage("Hãy nhập đúng số điện thoại gồm 10 chữ số");
    }
}

public class CustomerRegisterCommand : IRequest<AccessTokenResponse>
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string Password { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string? Address { get; set; }
    public IFormFile? Avatar { get; set; }
    public IFormFile? Background { get; set; }
}
