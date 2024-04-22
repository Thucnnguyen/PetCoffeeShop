using MediatR;
using PetCoffee.Application.Features.Reservation.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetCoffee.Application.Features.Reservation.Commands;



public class RatingReservationCommand : IRequest<ReservationResponse>
{

	[JsonIgnore]
	public long Id { get; set; }

	[Range(1, 5)]
	public long Rate { get; set; }

	public string? Comment { get; set; }

}
