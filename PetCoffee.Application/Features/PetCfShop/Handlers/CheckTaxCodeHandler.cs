
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Pet.Queries;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class CheckTaxCodeHandler : IRequestHandler<CheckTaxCodeQuery, TaxCodeResponse>
{
	private readonly IVietQrService _vietQrService;

	public CheckTaxCodeHandler(IVietQrService vietQrService)
	{
		_vietQrService = vietQrService;
	}

	public async Task<TaxCodeResponse> Handle(CheckTaxCodeQuery request, CancellationToken cancellationToken)
	{
		return await _vietQrService.CheckQrCode(request.TaxCode);
	}
}
