using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Application.Features.Payments.Queries;
using PetCoffee.Application.Features.Transactions.Commands;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers;

[Route("api/v1")]
[ApiController]
public class TransactionController : ApiControllerBase
{
    [HttpPost("transactions/items")]
    [Authorize]
    public async Task<ActionResult<bool>> BuyItems([FromBody] BuyItemsCommand request)
    {
        var response = await Mediator.Send(request);
        return response;
    }

    [HttpGet("transactions/{TransactionId}")]
    public async Task<ActionResult<PaymentResponse>> GetDepositTransaction([FromRoute] GetTransactionByIdQuery request)
    {
        var response = await Mediator.Send(request);
        return response;
    }

    [HttpGet("transactions")]
    [Authorize]
    public async Task<ActionResult<PaginationResponse<Transaction, PaymentResponse>>> GetAllTransaction([FromQuery] GetAllTransactionQuery request)
    {
        var response = await Mediator.Send(request);
        return response;
    }
}
