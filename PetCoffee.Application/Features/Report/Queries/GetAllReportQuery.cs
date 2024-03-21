
using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Report.Models;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Report.Queries;

public class GetAllReportQuery : PaginationRequest<Domain.Entities.Report>, IRequest<PaginationResponse<Domain.Entities.Report, ReportResponse>>
{
	public bool IsComment { get; set; }
	public bool IsPost { get; set; }
	public ReportStatus? Status { get; set; }
	public override Expression<Func<Domain.Entities.Report, bool>> GetExpressions()
	{
		if (IsComment && !IsPost)
		{
			Expression = Expression.And(r => r.CommentId != null);
		}
		if (!IsComment && IsPost)
		{
			Expression = Expression.And(r => r.PostID != null);
		}
		if (Status != null)
		{
			Expression = Expression.And(r => r.Status == Status);

		}
		return Expression;
	}
}
