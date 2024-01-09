using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Infrastructure.Persistence.Context;
using PetCoffee.Infrastructure.Persistence.Repository;
using PetCoffee.Infrastructure.Services;
using System.Reflection;
using System.Text;

namespace PetCoffee.Infrastructure;

public static class ConfigureService
{
	public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
		IConfiguration _configuration)
	{
		services.AddHttpContextAccessor();
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			var con = _configuration.GetConnectionString("MyDb");
			options.UseMySql(con,
						new MySqlServerVersion(new Version(8, 0, 30)),
						 builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)); 
			options.UseProjectables();
		});

		services.AddScoped<IJwtService, JwtService>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		//config jwt
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
			.AddJwtBearer(options =>
			{
				using var sc = services.BuildServiceProvider().CreateScope();

				// Validate JWT Token
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Token:secret").Value)),
				};
			});
		return services;
	}
}
