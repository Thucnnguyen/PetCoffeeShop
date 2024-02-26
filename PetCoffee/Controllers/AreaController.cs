using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Commands;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Features.Comment.Queries;
using PetCoffee.Application.Features.PostCategory.Commands;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Features.PostCategory.Queries;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class AreaController : ApiControllerBase
    {

        [HttpGet("Areas/{ShopId}")]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<Area, AreaResponse>>> GetAreaById([FromRoute] GetAreaByPetCfShopIdQuery request)
        {
            var response = await Mediator.Send(request);
            return response;
        }


        [HttpPost("Areas")]
        [Authorize]

        public async Task<ActionResult<AreaResponse>> Post([FromForm] CreateAreaCommand request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("Areas")]
        [Authorize]

        public async Task<ActionResult<AreaResponse>> Put([FromForm] UpdateAreaCommand request)
        {
            return await Mediator.Send(request);
        }


    }
}
