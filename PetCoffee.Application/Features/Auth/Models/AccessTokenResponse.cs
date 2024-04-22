using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Auth.Models;

public class AccessTokenResponse
{
	public string AccessToken { get; private set; }
	public AccountStatus Status { get; private set; }
	public AccessTokenResponse(string accessToken, AccountStatus status)
	{
		AccessToken = accessToken;
		Status = status;
	}
}