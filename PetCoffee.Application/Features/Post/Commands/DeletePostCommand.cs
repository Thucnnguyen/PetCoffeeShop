

using MediatR;

namespace PetCoffee.Application.Features.Post.Commands;

public class DeletePostCommand : IRequest<bool>
{
	public long Id { get; set; }
}
