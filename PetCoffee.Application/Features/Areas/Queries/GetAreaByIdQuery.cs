

using MediatR;
using PetCoffee.Application.Features.Areas.Models;

namespace PetCoffee.Application.Features.Areas.Queries;

public class GetAreaByIdQuery : IRequest<AreaResponse>
{
	public long AreaId { get; set; }
}
