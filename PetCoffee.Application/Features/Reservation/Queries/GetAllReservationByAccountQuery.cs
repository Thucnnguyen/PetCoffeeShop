﻿using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;


namespace PetCoffee.Application.Features.Reservation.Queries
{

    public class GetAllReservationByAccountQuery : PaginationRequest<Domain.Entities.Reservation>, IRequest<PaginationResponse<PetCoffee.Domain.Entities.Reservation, ReservationResponse>>
    {
        private string? _search;

        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }
        public OrderStatus? Status { get; set; }
        public DateTimeOffset? From { get; set; }

        public DateTimeOffset? To { get; set; }

        public override Expression<Func<Domain.Entities.Reservation, bool>> GetExpressions()
        {
            if (Status is not null)
            {
                Expression = Expression.And(order => Status == null || order.Status == Status);
            }
            if (From is not null)
            {
                Expression = Expression.And(order => From == null || order.CreatedAt.Date >= From);
            }
            if (To is not null)
            {
                Expression = Expression.And(order => To == null || order.CreatedAt.Date <= To);

            }

          

            return Expression;
        }
    }
}
