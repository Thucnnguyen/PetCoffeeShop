

using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Entities;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.PetCfShop.Queries;

public class GetMostPopularPetcfShopQuery : PaginationRequest<PetCoffeeShop>, IRequest<PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>>
{


	public override Expression<Func<PetCoffeeShop, bool>> GetExpressions()
	{
		return Expression;
	}
}
