using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Pet.Commands;

public class CreatePetCommand : IRequest<PetResponse>
{
	public string Name { get; set; }
	public int? BirthYear { get; set; }
	public double? Weight { get; set; }
	public string? Description { get; set; }
	public PetType PetType { get; set; }
	public PetGender Gender { get; set; }
	public IFormFile? Avatar { get; set; }
	public IFormFile? Backgound { get; set; }
}
