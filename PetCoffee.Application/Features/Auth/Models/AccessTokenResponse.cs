namespace PetCoffee.Application.Features.Auth.Models;

public class AccessTokenResponse
{
	public string AccessToken { get; private set; }

	public AccessTokenResponse(string accessToken)
	{
		AccessToken = accessToken;
	}
}