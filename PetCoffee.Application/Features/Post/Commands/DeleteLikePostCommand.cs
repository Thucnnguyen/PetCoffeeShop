

using MediatR;

namespace PetCoffee.Application.Features.Post.Commands;

public class DeleteLikePostCommand : IRequest<bool>
{
    public long PostId { get; set; }
}