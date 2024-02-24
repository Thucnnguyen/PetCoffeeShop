using Azure.Storage.Blobs;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenAI_API;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Infrastructure.Persistence.Context;
using PetCoffee.Infrastructure.Persistence.Interceptors;
using PetCoffee.Infrastructure.Persistence.Repository;
using PetCoffee.Infrastructure.Services;
using PetCoffee.Infrastructure.Settings;
using System.Reflection;
using System.Text;

namespace PetCoffee.Infrastructure;

public static class ConfigureService
{
	public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
		IConfiguration _configuration)
	{
		services.AddHttpContextAccessor();

		//set up dbconnection
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			var con = _configuration.GetConnectionString("MyDb");
			options.UseMySql(con,
						new MySqlServerVersion(new Version(8, 0, 30)),
						 builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)); 
			options.UseProjectables();
		});

		//add scoped
		services.AddScoped<ICurrentPrincipalService, CurrentPrincipleService>();
		services.AddScoped<ICurrentAccountService, CurrentAccountService>();
		services.AddScoped<IChatgptService, ChatgptService>();
		services.AddScoped<IAzureService, AzureService>();
		services.AddScoped<IJwtService, JwtService>();
		services.AddScoped<IVietQrService, VietQrService>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		//config VietQr
		services.AddOptions<VietQrSettings>()
			.BindConfiguration(VietQrSettings.ConfigSection)
			.ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddSingleton(sp => sp.GetRequiredService<IOptions<VietQrSettings>>().Value);
		//config azure settings
		services.AddOptions<AzureSettings>()
			.BindConfiguration(AzureSettings.ConfigSection)
			.ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddSingleton(sp => sp.GetRequiredService<IOptions<AzureSettings>>().Value);
		// get azureSetting
		using var sc = services.BuildServiceProvider().CreateScope();
		var _azureSettings = sc.ServiceProvider.GetRequiredService<AzureSettings>();
		// config azure blob
		services.AddSingleton(u => new BlobServiceClient(_azureSettings.BlobConnectionString));
		// config azure contentmoderator
		services.AddSingleton<IContentModeratorClient>(u => {
				var client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(_azureSettings.KeyContentModerator));
				client.Endpoint = _azureSettings.UrlConetentModerator;
				return client;
			});


		//config chatgpt
		services.AddOptions<ChatgptSettings>()
			.BindConfiguration(ChatgptSettings.ConfigSection)
			.ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddSingleton(sp => sp.GetRequiredService<IOptions<ChatgptSettings>>().Value);
		services.AddScoped<IOpenAIAPI>(serviceProvider =>
		{
			var apiKey = sc.ServiceProvider.GetRequiredService<ChatgptSettings>();
			return new OpenAIAPI(apiKey.Key);
		});
		//config firebase
		services.AddOptions<FirebaseSettings>()
			.BindConfiguration(FirebaseSettings.ConfigSection)
			.ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddSingleton(sp => sp.GetRequiredService<IOptions<FirebaseSettings>>().Value);
	
		//config jwt
		services.AddOptions<JwtSettings>()
			.BindConfiguration(JwtSettings.ConfigSection)
			.ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				using var sc = services.BuildServiceProvider().CreateScope();
				var settings = sc.ServiceProvider.GetRequiredService<JwtSettings>();

				// Validate JWT Token
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
					ValidateIssuer = false,
					ValidateAudience = false,
				};
			});
		return services;
	}
}
