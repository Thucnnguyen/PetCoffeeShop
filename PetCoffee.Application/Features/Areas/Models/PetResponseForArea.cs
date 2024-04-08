using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Areas.Models;

public class PetResponseForArea
{
	public long Id { get; set; }
	public string Name { get; set; }
	public int? BirthYear { get; set; }
	public double? Weight { get; set; }
	public decimal? Rate { get; set; } = 0;
	public string? Avatar { get; set; }
	public string? Backgrounds { get; set; }
	public string? Description { get; set; }
	public PetStatus PetStatus { get; set; }
	public PetType PetType { get; set; }
	public PetGender Gender { get; set; }
	public TypeSpecies TypeSpecies { get; set; }
	public bool Spayed { get; set; } = false;

}
