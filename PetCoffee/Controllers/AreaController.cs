using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Commands;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Areas.Queries;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class AreaController : ApiControllerBase
    {

        [HttpGet("petcoffeeshops/{ShopId}/areas")]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<Area, AreaResponse>>> GetAreaById([FromRoute] GetAreaByPetCfShopIdQuery request)
        {
            var response = await Mediator.Send(request);
            return response;
        }

        [HttpGet("areas/{AreaId}")]
        [Authorize]
        public async Task<ActionResult<AreaResponse>> GetAreaById([FromRoute] GetAreaByIdQuery request)
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


        [HttpDelete("Area/{AreaId}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteArea([FromRoute] DeleteAreaCommand request)
        {
            var response = await Mediator.Send(request);
            return response;
        }


        // view property area spcificshop at timerange and people go
        [HttpGet("petcoffeeshops/booking/areas")]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<Area, AreaResponse>>> GetAreaForBooking([FromQuery] GetAreaForBookingQuery request)
        {

            var response = await Mediator.Send(request);
            return response;
        }


    }
}
