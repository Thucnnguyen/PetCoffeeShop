
using MediatR;
using PetCoffee.Application.Features.Post.Model;

namespace PetCoffee.Application.Features.Post.Queries;

public class GetPostCreatedByCurrentAccountIdQuery : IRequest<IList<PostResponse>>
{
}
