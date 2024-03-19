
using MediatR;
using PetCoffee.Application.Features.RatePets.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Application.Features.RatePets.Commands;

public class CreatePetRateCommand : IRequest<RatePetResponse>
{
	public long PetId { get; set; }
	[Range(1, 5)]
	public long Rate { get; set; }
	public string? Comment { get; set; }
}

