using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.PetCfShop.Commands;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Vaccination.Commands;
using PetCoffee.Application.Features.Vaccination.Handlers;
using PetCoffee.Application.Features.Vaccination.Models;
using PetCoffee.Application.Features.Vaccination.Queries;

namespace PetCoffee.API.Controllers
{
    [Route("/api/v1")]
    [ApiController]
    public class VaccinationController : ApiControllerBase
    {


        [HttpGet("Pets/{PetId}/Vaccinations")]
        [Authorize]
        public async Task<ActionResult<IList<VaccinationResponse>>> GetVaccinationsByPetId([FromRoute] GetVaccinationsByPetIdQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

		[HttpGet("Vaccinations/{Id}")]
		[Authorize]
		public async Task<ActionResult<IList<VaccinationResponse>>> GetVaccinationsById([FromRoute] GetVaccinationByIdQuery request)
		{
			var response = await Mediator.Send(request);
			return Ok(response);
		}

		//create vaccation for specific pet

		[HttpPost("Vaccination")]
        [Authorize]
        public async Task<ActionResult<VaccinationResponse>> AddVacction([FromForm] AddVaccinationCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("Vaccination")]
        [Authorize]
        public async Task<ActionResult<VaccinationResponse>> UpdateVaccination(
            [FromForm] UpdateVaccinationCommand request)
        {
            return await Mediator.Send(request);
        }

        [HttpDelete("Vaccination/{VaccinationId}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteVaccination([FromRoute] DeleteVaccinationCommand request)
        {
            var response = await Mediator.Send(request);
            return response;
        }

    }
}
