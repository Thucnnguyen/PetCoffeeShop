
using MediatR;

namespace PetCoffee.Application.Features.Comment.Commands;

public class DeleteCommentCommand : IRequest<bool>
{
    public long CommentId { get; set; }
}
