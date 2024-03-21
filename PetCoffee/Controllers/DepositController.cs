using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Payments.Commands;
using PetCoffee.Application.Features.Payments.Models;

namespace PetCoffee.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class DepositController : ApiControllerBase
    {
        [HttpPost("recharges")]
        [Authorize]
        public async Task<ActionResult<PaymentResponse>> CreateDepositTransaction([FromBody] CreatePaymentCommand request)
        {
            var response = await Mediator.Send(request);
            return response;
        }


        [HttpGet("payments/callback/vnpay/{referenceId}")]
        public async Task<ActionResult<PaymentResponse>> CallbackVNPay([FromRoute] string referenceId, [FromQuery] ProcessVnpayPaymentIpnCommand request)
        {
            request.ReferenceId = referenceId;
            await Mediator.Send(request);
            return Ok();
        }
    }
}
