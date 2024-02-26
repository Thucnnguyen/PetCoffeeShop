using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Vaccination.Queries;

namespace PetCoffee.API.Controllers
{
    [Route("/api/v1")]
    [ApiController]
    public class VaccinationController : ApiControllerBase
    {

        [HttpGet("Pets/{PetId}/Vaccinations")]
        //[Authorize]
        public async Task<ActionResult<IList<PetResponse>>> GetVaccinationsByPetId([FromRoute] GetVaccinationsByPetIdQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
