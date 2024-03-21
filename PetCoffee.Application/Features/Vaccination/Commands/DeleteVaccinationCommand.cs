using MediatR;

namespace PetCoffee.Application.Features.Vaccination.Commands
{
	public class DeleteVaccinationCommand : IRequest<bool>
	{
		public long VaccinationId { get; set; }
	}
}
