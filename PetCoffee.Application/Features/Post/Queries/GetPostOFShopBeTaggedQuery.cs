using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Post.Queries
{
	public class GetPostOFShopBeTaggedQuery : PaginationRequest<Domain.Entities.Post>, IRequest<PaginationResponse<Domain.Entities.Post, PostResponse>>
	{
		public long ShopId { get; init; }

		public override Expression<Func<Domain.Entities.Post, bool>> GetExpressions()
		{
			throw new NotImplementedException();
		}
	}
}
