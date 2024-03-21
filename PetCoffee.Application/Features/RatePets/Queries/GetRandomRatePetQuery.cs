
using MediatR;
using PetCoffee.Application.Features.RatePets.Models;

namespace PetCoffee.Application.Features.RatePets.Queries;

public class GetRandomRatePetQuery : IRequest<RatePetResponse>
{
	public long PetId { get; set; }
}
