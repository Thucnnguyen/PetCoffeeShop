using Newtonsoft.Json;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Service;
using PetCoffee.Infrastructure.Settings;

namespace PetCoffee.Infrastructure.Services;

public class VietQrService : IVietQrService
{
	private readonly VietQrSettings _settings;

	public VietQrService(VietQrSettings settings)
	{
		_settings = settings;
	}

	public async Task<TaxCodeResponse> CheckQrCode(string Code)
	{
		// init http client
		var httpClient = new HttpClient();
		// set up url
		httpClient.BaseAddress = new Uri(_settings.Url);
		//send and get response
		var response = await httpClient.GetAsync(Code);

		var taxResponse = JsonConvert.DeserializeObject<TaxCodeResponse>(await response.Content.ReadAsStringAsync());
		return taxResponse;
	}
}
