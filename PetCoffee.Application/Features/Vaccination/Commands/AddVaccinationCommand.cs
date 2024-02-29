using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Vaccination.Models;
using PetCoffee.Domain.Enums;



namespace PetCoffee.Application.Features.Vaccination.Commands
{

    public class AddVaccinationCommandValidator : AbstractValidator<AddVaccinationCommand>
    {
        public AddVaccinationCommandValidator()
        {
            RuleFor(command => command.VaccinationDate)
			.Must((command, VacciniationDate) => VacciniationDate < command.ExpireTime)
            .WithMessage("Vaccination date must be smaller than Expire time.");
        }
    }
    public class AddVaccinationCommand : IRequest<VaccinationResponse>
    {

        //[SwaggerSchema(Format = "date-time")]
        public DateTime VaccinationDate { get; set; }
        //[SwaggerSchema(Format = "date-time")]
        public DateTime ExpireTime { get; set; }
        public VaccinationType VaccinationType { get; set; }
        public IFormFile? PhotoEvidence { get; set; }
        public long PetId { get; set; }
        //public Domain.Entities.Pet Pet { get; set; }
    }
}
