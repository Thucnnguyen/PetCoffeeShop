using MediatR;
using Newtonsoft.Json;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Areas.Queries
{
    public class GetAreaForBookingQuery : PaginationRequest<Area>, IRequest<PaginationResponse<Area, AreaResponse>>
    {
        
        public long ShopId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int? TotalSeat { get; set; }
        public override Expression<Func<Area, bool>> GetExpressions()
        {
            return Expression;
        }
    }
}
