

using MediatR;
using System.ComponentModel;

namespace PetCoffee.Application.Features.Post.Commands;

public class CreateLikePostCommand : IRequest<bool>
{
	public long PostId { get; set; }
}
