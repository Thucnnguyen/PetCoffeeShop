using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.Reservation.Models;
using System.Text.Json.Serialization;

namespace PetCoffee.Application.Features.Reservation.Commands
{
	public class UpdateReservationValidation : AbstractValidator<UpdateReservationCommand>
	{
		public UpdateReservationValidation()
		{
			//RuleFor(model => model.Comment).NotEmpty();
		}
	}
	public class UpdateReservationCommand : IRequest<ReservationResponse>
	{

		[JsonIgnore]
		public long Id { get; set; }


		public string? Comment { get; set; }
	}
}
