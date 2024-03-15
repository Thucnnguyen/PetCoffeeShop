using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Pet.Commands;

public class UpdatePetCommand : IRequest<PetResponse>
{
	public long Id { get; set; }
	public string Name { get; set; }
	public int? BirthYear { get; set; }
	public double? Weight { get; set; }
	public string? Description { get; set; }
	public PetType PetType { get; set; }
	public TypeSpecies TypeSpecies { get; set; }
	public PetGender Gender { get; set; }
	public PetStatus PetStatus { get; set; } 
	public IFormFile? NewAvatar { get; set; }
	public IList<IFormFile>? NewBackgrounds { get; set; }
}
