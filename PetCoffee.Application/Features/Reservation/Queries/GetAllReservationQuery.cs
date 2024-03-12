using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Queries
{
    public class GetAllReservationQuery : PaginationRequest<Domain.Entities.Reservation>, IRequest<PaginationResponse<PetCoffee.Domain.Entities.Reservation, ReservationResponse>>
    {
        private string? _search;

        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }
        public OrderStatus? Status { get; set; }
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public long? CustomerId { get; set; }



        public override Expression<Func<Domain.Entities.Reservation, bool>> GetExpressions()
        {
            //if (Search is not null)
            //{
            //    Expression = Expression.And(post => (post.Content != null && post.Content.ToLower().Contains(Search)));
            //}
            Expression = Expression.And(order => Status == null || order.Status == Status);

            Expression = Expression.And(order => From == null || order.CreatedAt.Date >= From);

            Expression = Expression.And(order => To == null || order.CreatedAt.Date <= To);
            Expression = Expression.And(order => CustomerId == null
                                         || (order.CreatedById != null && CustomerId == order.CreatedById));
                                         

            return Expression;
        }
    }
}
