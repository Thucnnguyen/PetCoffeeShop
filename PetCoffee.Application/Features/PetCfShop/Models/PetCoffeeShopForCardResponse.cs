using PetCoffee.Domain.Enums;
using System.Text.Json.Serialization;

namespace PetCoffee.Application.Features.PetCfShop.Models;

public class PetCoffeeShopForCardResponse
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string? Description { get; set; }
	public string? AvatarUrl { get; set; }
	public string? BackgroundUrl { get; set; }
	public double? Distance { get; set; } = 0;
	public double? TotalFollow { get; set; }
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ShopType Type { get; set; }
	public ShopStatus Status { get; set; }
	public DateTimeOffset CreatedAt { get; set; }
	public double? Latitude { get; set; }
	public double? Longitude { get; set; }
}
