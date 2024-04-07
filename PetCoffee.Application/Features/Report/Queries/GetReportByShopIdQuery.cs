

using LinqKit;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Report.Models;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace PetCoffee.Application.Features.Report.Queries;

public class GetReportByShopIdQuery : PaginationRequest<Domain.Entities.Report>, IRequest<PaginationResponse<Domain.Entities.Report, ReportResponse>>
{
	public DateTimeOffset? StartDate { get; set; }
	public DateTimeOffset? EndDate { get; set; }
	[JsonIgnore]
	[BindNever]
	public long ShopId { get; set; }
	public override Expression<Func<Domain.Entities.Report, bool>> GetExpressions()
	{
		if (StartDate == null)
		{
			Expression = Expression.And(e => e.CreatedAt >= StartDate);
		}

		if (EndDate == null)
		{
			Expression = Expression.And(e => e.CreatedAt <= EndDate);
		}

		Expression = Expression.And(e => (e.Post != null && e.Post.ShopId == ShopId )
									  ||(e.Comment != null && e.Comment.ShopId == ShopId));
		return Expression;

	}
}
