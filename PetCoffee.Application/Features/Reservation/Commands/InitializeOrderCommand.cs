using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.Areas.Commands;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }

        public int TotalSeat { get; set; }
    }


}
