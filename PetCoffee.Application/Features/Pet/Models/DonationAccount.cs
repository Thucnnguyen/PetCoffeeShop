

namespace PetCoffee.Application.Features.Pet.Models;

public class DonationAccount
{
	public long Id { get; set; }
	public string? Name { get; set; }
	public string? AvatarUrl { get; set; }
	public decimal TotalDonate { get; set; } = 0;
}
