using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Domain.Entities;
using System.Linq.Expressions;


namespace PetCoffee.Application.Features.Areas.Commands
{
    public class GetAreaByPetCfShopIdQuery : PaginationRequest<Domain.Entities.Area>, IRequest<PaginationResponse<Domain.Entities.Area, AreaResponse>>
    {
        public long ShopId { get; set; }
        public override Expression<Func<Area, bool>> GetExpressions()
        {
            return Expression;
        }
    }
}
