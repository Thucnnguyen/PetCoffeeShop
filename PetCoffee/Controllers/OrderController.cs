﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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





		[HttpGet("order/{id:long}")]
		[Authorize]
		public async Task<ActionResult<ReservationDetailResponse>> GetOrder([FromRoute] long id)
		{
			var getOrderRequest = new GetReservationByIdQuery
			{
				Id = id
			};

			return await Mediator.Send(getOrderRequest);
		}



		[HttpPut("order/{id:long}")]
		[Authorize]
		public async Task<ActionResult<ReservationResponse>> UpdateOrder([FromRoute] long id, [FromBody] UpdateReservationCommand request)
		{
			request.Id = id;
			return await Mediator.Send(request);
		}

		// view order by speicific arear 
		[HttpGet("areas/{AreaId}/orders")]
		[Authorize]
		public async Task<ActionResult<IList<ReservationResponse>>> GetOrderByAreaId([FromRoute] GetReservationByAreaIdQuery request)
		{

			return Ok(await Mediator.Send(request));
		}

		[HttpPost("orders")]
		[Authorize]
		public async Task<ActionResult<ReservationResponse>> CreateOrder([FromForm] InitializeOrderCommand command)
		{
			return await Mediator.Send(command);
		}



		[HttpPut("order/{id:long}/return")]
		[Authorize]
		public async Task<ActionResult<ReservationResponse>> ReturnOrder([FromRoute] long id)
		{
			var command = new ReturnOrderCommand()
			{
				OrderId = id,
			};
			return await Mediator.Send(command);
		}

		[HttpPut("orders/{id:long}/invoice")]
		[Authorize]
		public async Task<ActionResult<bool>> AddMinusProductToBooking(
		[FromRoute] long id,
		[FromBody] UpdateProductOfBookingCommand command)
		{
			command.OrderId = id;
			return await Mediator.Send(command);
		}




		[HttpGet("accounts/orders")]
		[Authorize]
		public async Task<ActionResult<PaginationResponse<Reservation, ReservationResponse>>> GetOrdersByCurrentAccount(
  [FromQuery] GetAllReservationByAccountQuery request)
		{
			if (string.IsNullOrWhiteSpace(request.SortColumn))
			{
				request.SortColumn = "CreatedAt";
				request.SortDir = SortDirection.Desc;
			}
			return await Mediator.Send(request);
		}



		[HttpDelete("orders/{ReservationId:long}/reservation-product")]
		[Authorize]
		public async Task<ActionResult<bool>> RemoveProductInReservation(
		[FromRoute] RemoveProductInReservationCommand command)
		{

			return await Mediator.Send(command);

		}



		[HttpPut("order/{id:long}/rate")]
		[Authorize]
		public async Task<ActionResult<ReservationResponse>> RatingOrder([FromRoute] long id, [FromBody] RatingReservationCommand request)
		{
			request.Id = id;
			return await Mediator.Send(request);
		}


	}
}

