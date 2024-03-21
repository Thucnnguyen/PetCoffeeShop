
using MediatR;
using PetCoffee.Application.Features.Memory.Models;

namespace PetCoffee.Application.Features.Moment.Queries;

public class GetMomentByIdQuery : IRequest<MomentResponse>
{
	public long MomentId { get; set; }
}
