using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Wallets.Models;
using PetCoffee.Application.Features.Wallets.Queries;

namespace PetCoffee.API.Controllers;

[Route("api/v1")]
[ApiController]
public class WalletController : ApiControllerBase
{
	[HttpGet("account/wallet")]
	[Authorize]
	public async Task<ActionResult<WalletResponse>> GetAreaById([FromQuery] GetWalletForCurrentAccountQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}

	[HttpGet("account/wallet/income")]
	[Authorize(Roles ="Manager")]
	public async Task<ActionResult<IncomeForShopResponse>> GetIncomeHandler([FromQuery] GetIncomeQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}

	[HttpGet("account/wallet/outcome")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult<IncomeForShopResponse>> GetOutCome([FromQuery] GetOutcomeQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}

	[HttpGet("account/wallet/month/income")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult<List<GetTransactionAmountResponse>>> GetOutCome([FromQuery] GetIncomeTransactionForShopInMonthQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	[HttpGet("account/wallet/month/outcome")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult<List<GetTransactionAmountResponse>>> GetOutCome([FromQuery] GetOutcomeTransactionForShopInMonthQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}

	[HttpGet("account/wallet/platform/income")]
	[Authorize(Roles = "Admin")]
	public async Task<ActionResult<IncomeResponse>> GetIncomePlatform([FromQuery] GetIncomePlatformQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	[HttpGet("account/wallet/platform/month/income")]
	[Authorize(Roles = "Admin")]
	public async Task<ActionResult<GetTransactionAmountResponse>> GetIncomePlatformInMonth([FromQuery] GetIncomePlatFormnInMonthQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	[HttpGet("account/wallet/shop/income")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult<IncomeResponse>> GetIncomeByShopId([FromQuery] GetShopIncomeQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}

	[HttpGet("account/wallet/shop/outcome")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult<IncomeResponse>> GetOutcomeByShopId([FromQuery] GetShopOutComeQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}
