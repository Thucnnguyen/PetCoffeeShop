
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Pet.Models;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Pet.Queries;

public class GetPetsByShopIdQuery : PaginationRequest<Domain.Entities.Pet>, IRequest<PaginationResponse<Domain.Entities.Pet, PetResponse>>
{
	public long ShopId { get; set; }

	public override Expression<Func<Domain.Entities.Pet, bool>> GetExpressions()
	{
		return Expression;
	}
}
