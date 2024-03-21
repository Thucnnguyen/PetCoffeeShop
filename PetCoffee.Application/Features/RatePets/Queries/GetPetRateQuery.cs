using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.RatePets.Models;
using PetCoffee.Domain.Entities;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.RatePets.Queries;

public class GetPetRateQuery : PaginationRequest<RatePet>, IRequest<PaginationResponse<RatePet, RatePetResponse>>
{
	public long PetId { get; set; }
	public override Expression<Func<RatePet, bool>> GetExpressions()
	{
		Expression = Expression.And(rp => rp.PetId == PetId);
		return Expression;
	}
}
