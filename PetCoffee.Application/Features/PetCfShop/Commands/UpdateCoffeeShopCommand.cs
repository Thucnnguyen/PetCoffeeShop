using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.PetCfShop.Models;
using System.ComponentModel.DataAnnotations;


namespace PetCoffee.Application.Features.PetCfShop.Commands;
public class UpdateCoffeeShopValidation : AbstractValidator<UpdateCoffeeShopCommand>
{
	public UpdateCoffeeShopValidation()
	{
		RuleFor(model => model.Email).NotEmpty();

		RuleFor(model => model.Phone)
		.NotEmpty()
		.Matches("^\\d{10}$")
		.WithMessage("Hãy nhập đúng số điện thoại gồm 10 chữ số");
	}
}
public class UpdateCoffeeShopCommand : IRequest<PetCoffeeShopResponse>
{
	public long Id { get; set; }
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
	public double Latitude { get; set; }
	public double Longitude { get; set; }
}
