using MediatR;

namespace PetCoffee.Application.Features.PostCategory.Commands;

public class RemovePostCategoryCommand : IRequest<bool>
{
	public long Id { get; set; }
}
