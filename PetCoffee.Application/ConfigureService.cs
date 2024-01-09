using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCoffee.Application.Common.Behaviours;
using PetCoffee.Application.Common.Mapping;
using System.Reflection;


namespace PetCoffee.Application;

public static class ConfigureService 
{
	public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services,
	IConfiguration configuration)
	{
		// Auto mapper
		services.AddAutoMapper(typeof(MappingProfile));

		// FluentAPI validation
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		// MediatR
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationErrorBehaviour<,>));
		});

		return services;
	}
}
