using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Areas.Queries;
using PetCoffee.Application.Features.Tables.Models;
using PetCoffee.Application.Features.Tables.Queries;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers
{
    [Route("/api/v1")]
    [ApiController]
    public class TableController : ApiControllerBase
    {
        [HttpGet("petcoffeeshops/{ShopId}/areas/{AreaId}/tables")]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<Table, TableResponse>>> GetTableByAreaId([FromRoute] GetTableByAreaIdQuery request)
        {
            var response = await Mediator.Send(request);
            return response;
        }


    }
}
