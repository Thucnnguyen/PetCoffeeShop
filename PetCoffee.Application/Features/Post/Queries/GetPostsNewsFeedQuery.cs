
using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Post.Model;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Post.Queries;

public class GetPostsNewsFeedQuery : PaginationRequest<Domain.Entities.Post>, IRequest<PaginationResponse<Domain.Entities.Post, PostResponse>>
{
    private string? _search;

    public string? Search
    {
        get => _search;
        set => _search = value?.Trim().ToLower();
    }
    public long CategoryId { get; set; } = 0;

    public override Expression<Func<Domain.Entities.Post, bool>> GetExpressions()
    {
        if (Search is not null)
        {
            if (Search is not null)
            {
                Expression = Expression.And(e => e.Content.ToLower().Contains(Search));
            }
        }
        if (CategoryId > 0)
        {
            Expression = Expression.And(e => e.PostCategories.Where(p => p.CategoryId == CategoryId).Any());
        }
        return Expression;
    }
}
