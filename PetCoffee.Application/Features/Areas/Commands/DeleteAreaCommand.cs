using MediatR;

namespace PetCoffee.Application.Features.Areas.Commands
{
	public class DeleteAreaCommand : IRequest<bool>
	{
		public long AreaId { get; set; }
	}
}
