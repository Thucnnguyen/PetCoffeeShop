

using MediatR;

namespace PetCoffee.Application.Features.Moment.Commands;

public class DeleteMomentCommand : IRequest<bool>
{
	public long Id { get; set; }
}
