using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.Reservation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Commands
{


	public class RatingReservationCommand : IRequest<ReservationResponse>
	{

		[JsonIgnore]
		public long Id { get; set; }


		public string? Note { get; set; }

		[Range(1, 5)]
		public long Rate { get; set; }

		public string? Comment { get; set; }

	}
	
	
}
