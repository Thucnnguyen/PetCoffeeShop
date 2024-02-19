using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.PetCfShop.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Application.Features.PetCfShop.Commands;

public class CreatePetcfShopValidation : AbstractValidator<CreatePetCfShopCommand>
{
    public CreatePetcfShopValidation()
    {
		RuleFor(model => model.Email).NotEmpty();
		RuleFor(model => model.TaxCode)
			.MinimumLength(10)
			.WithMessage("Mã số thuế có độ dài tối thiểu là 10");

		RuleFor(model => model.Phone)
		.NotEmpty()
		.Matches("^\\d{10}$")
		.WithMessage("Hãy nhập đúng số điện thoại gồm 10 chữ số");
	}
}

public class CreatePetCfShopCommand : IRequest<PetCoffeeShopResponse>
{
	//Shop Information
	public string Name { get; set; }
	[EmailAddress]
	public string Email { get; set; }
	public string Phone { get; set; }
	public IFormFile? Avatar { get; set; }
	public IFormFile? Background { get; set; }
	public string? WebUrl { get; set; }
	public string? FbUrl { get; set; }
	public string? InstagramUrl { get; set; }
	public string Location { get; set; }
	public string TaxCode { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
}
