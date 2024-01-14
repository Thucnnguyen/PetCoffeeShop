
using FluentValidation;

namespace PetCoffee.Application.Features.Auth.Commands;

public class ShopManagerRegisterCommandValidation : AbstractValidator<ShopManagerRegisterCommand>
{

}

public class ShopManagerRegisterCommand : CustomerRegisterCommand
{
	public string ShopName { get; set; }
	public string? Description { get; set; }
	public string? WebsiteUrl { get; set; }
	public string? FbUrl { get; set; }
	public string? InstagramUrl { get; set; }
	public string? ShopAvatarUrl { get; set; }
	public string Phone { get; set; }
	public string Email { get; set; }
	public string Location { get; set; }
	public double? Latitude { get; set; }
	public double? Longitude { get; set; }
}
