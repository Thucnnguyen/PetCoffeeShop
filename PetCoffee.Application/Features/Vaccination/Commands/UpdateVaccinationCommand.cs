using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Vaccination.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Vaccination.Commands
{
	public class UpdateVaccinationCommandValidator : AbstractValidator<UpdateVaccinationCommand>
	{
		public UpdateVaccinationCommandValidator()
		{
			RuleFor(model => model.Id).NotEmpty();
			//RuleFor(command => command.VacciniationDate)
			//.Must((command, VacciniationDate) => VacciniationDate < command.ExpireTime)
			//.WithMessage("Vaccination date must be smaller than Expire time.");
		}
	}
	public class UpdateVaccinationCommand : IRequest<VaccinationResponse>
	{

		public long Id { get; set; }
		public DateTime? VaccinationDate { get; set; }

		public DateTime? ExpireTime { get; set; }
		public VaccinationType? VaccinationType { get; set; }
		public IFormFile? NewPhotoEvidence { get; set; }


	}

}
