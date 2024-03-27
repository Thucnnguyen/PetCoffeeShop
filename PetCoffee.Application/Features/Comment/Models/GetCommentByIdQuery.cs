

using MediatR;

namespace PetCoffee.Application.Features.Comment.Models;

public class GetCommentByIdQuery : IRequest<CommentResponse>
{
	public long Id { get; set; }
}
