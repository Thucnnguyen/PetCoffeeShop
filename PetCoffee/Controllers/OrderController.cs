using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Features.PostCategory.Queries;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class OrderController : ApiControllerBase
    {

        [HttpGet("orders")]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<Reservation, ReservationResponse>>> GetOrders(
      [FromQuery] GetAllReservationQuery request)
        {
            if (string.IsNullOrWhiteSpace(request.SortColumn))
            {
                request.SortColumn = "CreatedAt";
                request.SortDir = SortDirection.Desc;
            }
            return await Mediator.Send(request);
        }



        [HttpGet("{id:long}")]
        [Authorize]
        public async Task<ActionResult<ReservationDetailResponse>> GetOrder([FromRoute] long id)
        {
            var getOrderRequest = new GetReservationQuery
            {
                Id = id
            };

            return await Mediator.Send(getOrderRequest);
        }



        [HttpPut("{id:long}")]
        [Authorize]
        public async Task<ActionResult<ReservationResponse>> UpdateOrder([FromRoute] long id, [FromBody] UpdateReservationCommand request)
        {
            request.Id = id;
            return await Mediator.Send(request);
        }

        // view order by speicific arear 
        [HttpGet("areas/{AreaId}/orders")]
        [Authorize]
        public async Task<ActionResult<IList<ReservationResponse>>> GetOrderByAreaId([FromQuery] GetReservationByAreaIdQuery request)
        {
    
            return Ok(await Mediator.Send(request));
        }

    }
}
