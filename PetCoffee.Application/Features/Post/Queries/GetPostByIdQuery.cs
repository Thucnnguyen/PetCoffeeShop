using MediatR;
using PetCoffee.Application.Features.Post.Model;

namespace PetCoffee.Application.Features.Post.Queries
{
	public class GetPostByIdQuery : IRequest<PostResponse>
	{
		public long Id { get; init; }
	}
}
