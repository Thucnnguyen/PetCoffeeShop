
using MediatR;
using PetCoffee.Application.Features.Vaccination.Models;

namespace PetCoffee.Application.Features.Vaccination.Queries;

public class GetVaccinationByIdQuery : IRequest<VaccinationResponse>
{
	public long Id { get; set; }
}
