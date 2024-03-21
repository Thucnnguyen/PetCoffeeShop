
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Comment.Models;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Comment.Queries;

public class GetSubCommentByCommentIdQuery : PaginationRequest<Domain.Entities.Comment>, IRequest<PaginationResponse<Domain.Entities.Comment, CommentResponse>>
{
    public long CommentId { get; set; }
    public override Expression<Func<Domain.Entities.Comment, bool>> GetExpressions()
    {
        return Expression;
    }
}