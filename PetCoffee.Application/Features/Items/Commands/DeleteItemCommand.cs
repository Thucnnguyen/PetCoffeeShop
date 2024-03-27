
using MediatR;

namespace PetCoffee.Application.Features.Items.Commands;

public class DeleteItemCommand : IRequest<bool>
{
	public long Id { get; set; }
}
