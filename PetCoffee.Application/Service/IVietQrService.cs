using PetCoffee.Application.Common.Models.Response;

namespace PetCoffee.Application.Service;

public interface IVietQrService
{
	public Task<TaxCodeResponse> CheckQrCode(string code);
}

