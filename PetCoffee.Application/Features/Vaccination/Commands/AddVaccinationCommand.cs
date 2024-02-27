using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.SqlServer.Server;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Vaccination.Models;

using PetCoffee.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PetCoffee.Application.Features.Vaccination.Commands
{

    public class AddVaccinationCommandValidator : AbstractValidator<AddVaccinationCommand>
    {
        public AddVaccinationCommandValidator()
        {
            RuleFor(command => command.VacciniationDate)
            .Must((command, VacciniationDate) => VacciniationDate < command.ExpireTime)
            .WithMessage("Vaccination date must be smaller than Expire time.");
        }
    }
    public class AddVaccinationCommand : IRequest<VaccinationResponse>
    {

        //[SwaggerSchema(Format = "date-time")]
        public DateTime VacciniationDate { get; set; }
        //[SwaggerSchema(Format = "date-time")]
        public DateTime ExpireTime { get; set; }
        public VaccinationType VaccinationType { get; set; }
        public IFormFile? PhotoEvidence { get; set; }
        public long PetId { get; set; }
        //public Domain.Entities.Pet Pet { get; set; }
    }
}
