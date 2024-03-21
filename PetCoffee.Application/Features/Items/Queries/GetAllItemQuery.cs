using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Domain.Entities;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Items.Queries
{
	public class GetAllItemQuery : PaginationRequest<Item>, IRequest<PaginationResponse<Item, ItemResponse>>
	{

		public override Expression<Func<Item, bool>> GetExpressions()
		{
			throw new NotImplementedException();
		}
	}
}
