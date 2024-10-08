﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Enums;
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
	public string? WebsiteUrl { get; set; }
	public string? FbUrl { get; set; }
	public string? InstagramUrl { get; set; }
	public string Location { get; set; }
	public string TaxCode { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public string? StartTime { get; set; }
	public string? EndTime { get; set; }
	public ShopType Type { get; set; }
}
