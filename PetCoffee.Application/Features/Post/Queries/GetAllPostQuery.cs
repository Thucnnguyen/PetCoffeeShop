using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Post.Queries
{
    public class GetAllPostQuery : PaginationRequest<PetCoffee.Domain.Entities.Post>, IRequest<PaginationResponse<PetCoffee.Domain.Entities.Post, PostResponse>>
    {
        private string? _search;

        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }
        public override Expression<Func<PetCoffee.Domain.Entities.Post, bool>> GetExpressions()
        {
            if (Search is not null)
            {
                Expression = Expression.And(post => (post.Title != null && post.Title.ToLower().Contains(Search)));
            }

            return Expression;
        }
    }
}
