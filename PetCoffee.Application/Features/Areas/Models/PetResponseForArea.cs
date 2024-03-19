

using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Areas.Models;

public class PetResponseForArea
{
	public long Id { get; set; }
	public string Name { get; set; }

	public string? Avatar { get; set; }
	public double? Rate { get; set; }
	public PetType PetType { get; set; }

}
