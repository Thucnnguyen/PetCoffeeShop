using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Pet.Queries;

namespace PetCoffee.API.Controllers;

[Route("api/v1")]
[ApiController]
public class PetController : ApiControllerBase
{
    [HttpPost("pets")]
    [Authorize]
    public async Task<ActionResult<PetResponse>> CreatePet([FromForm] CreatePetCommand request)
    {
        var response = await Mediator.Send(request);
        return response;
    }
    [HttpGet("petCoffeeShops/pets")]
    [Authorize]
    public async Task<ActionResult<PaginationResponse<Domain.Entities.Pet, PetResponse>>> GetPetByShopId([FromQuery] GetPetsByShopIdQuery request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("area/pets")]
    [Authorize]
    public async Task<ActionResult<PaginationResponse<Domain.Entities.Pet, PetResponse>>> GetPetByAreaId([FromQuery] GetPetsByAreaIdQuery request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("pets/{Id}")]
    [Authorize]
    public async Task<ActionResult<PetResponse>> GetPetById([FromRoute] GetPetByIdQuery request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }
    [HttpPut("pets")]
    [Authorize]
    public async Task<ActionResult<PetResponse>> UpdatePet([FromForm] UpdatePetCommand request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }

    [HttpPut("shops/pets")]
    [Authorize]
    public async Task<ActionResult<bool>> UpdateShopIdForPet([FromBody] UpdateShopIdOfPetCommand request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }

    [HttpPut("areas/pets")]
    [Authorize]
    public async Task<ActionResult<bool>> UpdatePetArea([FromBody] UpdatePetAreaCommand request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete("Pets/{Id}")]
    [Authorize]
    public async Task<ActionResult<bool>> UpdatePet([FromRoute] DeletePetCommand request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }

}
