using MediatR;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Vaccination.Queries
{
    public class GetVaccinationsByPetIdQuery : IRequest<IList<VaccinationResponse>>
    {
        public long PetId { get; set; }
    }

}
