using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.PostCategory.Commands;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Features.Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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


        public string?  Comment { get; set; }
    }
}
    