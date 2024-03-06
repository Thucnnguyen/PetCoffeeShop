using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Report.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Report.Queries
{
    public class GetAllReportRequestQuery : PaginationRequest<Domain.Entities.Report>, IRequest<PaginationResponse<Domain.Entities.Report, ReportResponse>>
    {
        public ReportStatus? ReportStatus { get; set; }
        public override Expression<Func<Domain.Entities.Report, bool>> GetExpressions()
        {
            if (ReportStatus is not null)
            {
                Expression = Expression.And(report => Equals(ReportStatus, report.Status));
            }

            return Expression;
        }
    }
}
