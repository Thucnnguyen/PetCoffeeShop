using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.Reservation.Models;

namespace PetCoffee.Application.Features.Reservation.Commands
{
	public class InitializeOrderValidation : AbstractValidator<InitializeOrderCommand>
	{
		public InitializeOrderValidation()
		{
			RuleFor(model => model.TotalSeat).NotEmpty();
			RuleFor(command => command.EndTime)
			.GreaterThan(command => command.StartTime)
			.WithMessage("EndTime must be greater than StartTime");
		}
	}

	public class InitializeOrderCommand : IRequest<ReservationResponse>
	{
		public long AreaId { get; set; }
		public DateTimeOffset StartTime { get; set; }
		public DateTimeOffset EndTime { get; set; }
		public string? Note { get; set; }

		public int TotalSeat { get; set; }
	}


}
