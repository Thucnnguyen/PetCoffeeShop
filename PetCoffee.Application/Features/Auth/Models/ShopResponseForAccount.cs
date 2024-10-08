﻿

using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Auth.Models;

public class ShopResponseForAccount
{
	public long Id { get; set; }
	public string? AvatarUrl { get; set; }
	public string? Name { get; set; }
	public string Email { get; set; }
	public string Location { get; set; }

	public ShopStatus Status { get; set; }
	public ShopType ShopType { get; set; }
	public string Phone { get; set; }
}
