using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetCoffee.Infrastructure.Services;

public class JwtService : IJwtService
{
	private readonly JwtSettings _jwtSettings;
	public JwtService(JwtSettings jwtSettings)
	{
		_jwtSettings = jwtSettings;
	}
	private string GenerateJwtToken(Account account, int expireInDays)
	{
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
			new Claim(ClaimTypes.Name, account.FullName ?? string.Empty),
			new Claim(ClaimTypes.Role, account.Role.ToString())
		};

		var token = new JwtSecurityToken(
			claims: claims,
			expires: DateTime.UtcNow.AddDays(expireInDays),
			signingCredentials: credentials
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
	public string GenerateJwtToken(Account account)
	{
		return GenerateJwtToken(account,_jwtSettings.TokenExpire);
	}
}
