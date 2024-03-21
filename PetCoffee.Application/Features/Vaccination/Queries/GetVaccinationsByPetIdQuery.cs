using MediatR;
using PetCoffee.Application.Features.Vaccination.Models;

namespace PetCoffee.Application.Features.Vaccination.Queries
{
    public class GetVaccinationsByPetIdQuery : IRequest<IList<VaccinationResponse>>
    {
        public long PetId { get; set; }
    }

}
