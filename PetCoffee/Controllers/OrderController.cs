using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Domain.Entities;

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
    }
}
