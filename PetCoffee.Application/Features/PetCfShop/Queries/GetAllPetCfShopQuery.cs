using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace PetCoffee.Application.Features.PetCfShop.Queries
{
    public class GetAllPetCfShopQuery : PaginationRequest<PetCoffeeShop>, IRequest<PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>>
    {
        private string? _search;

        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }

        public ShopStatus? Status { get; set; }

        public string? Address { get; set; }
        public override Expression<Func<PetCoffeeShop, bool>> GetExpressions()
        {
            if (Search is not null)
            {
                Expression = Expression.And(store => (store.Location != null && store.Location.ToLower().Contains(Search)) ||
                                                     store.Name.ToLower().Contains(Search));
            }

            if (Status is not null)
            {
                Expression = Expression.And(store => Equals(Status, store.Status));
            }

            if (Address is not null)
            {
                Expression = Expression.And(store => store.Location != null && store.Location.ToLower().Contains(Address.ToLower().Trim()));
            }


            return Expression;
        }
    }
}
    