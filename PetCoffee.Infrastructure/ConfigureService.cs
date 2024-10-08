﻿using Azure.Storage.Blobs;
using FirebaseAdmin;
using FluentValidation;
using Google.Apis.Auth.OAuth2;
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
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Application.Service.Notifications.Models;
using PetCoffee.Application.Service.Payment;
using PetCoffee.Infrastructure.Persistence.Context;
using PetCoffee.Infrastructure.Persistence.Interceptors;
using PetCoffee.Infrastructure.Persistence.Repository;
using PetCoffee.Infrastructure.Scheduler;
using PetCoffee.Infrastructure.Services;
using PetCoffee.Infrastructure.Services.Notifications;
using PetCoffee.Infrastructure.Services.Notifications.Website.SignalR;
using PetCoffee.Infrastructure.Services.Payment.VnPay;
using PetCoffee.Infrastructure.Services.Payment.ZaloPay;
using PetCoffee.Infrastructure.Settings;
using PetCoffee.Infrastructure.SinalR;
using PetCoffee.Infrastructure.SinalR.Notifications;
using Quartz;
using Quartz.AspNetCore;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

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
		// config Quartz
		services.AddQuartz(q =>
		{
			q.UseMicrosoftDependencyInjectionJobFactory();

			// Cron jobs for order overtime jobs every hours 
			var remindShopNotHavePet = new JobKey(CheckShopHasPetJob.CheckShopHasPetJobKey);
			q.AddJob<CheckShopHasPetJob>(options => options.WithIdentity(remindShopNotHavePet));

			q.AddTrigger(options =>
				options.ForJob(remindShopNotHavePet)
					.WithIdentity($"{CheckShopHasPetJob.CheckShopHasPetJobKey}-trigger")
					.WithCronSchedule("0 0 0/3 ? * * *", x => x.InTimeZone(TimeZoneInfo.Utc))
			);
		});
		services.AddQuartzServer(options =>
		{
			// options.WaitForJobsToComplete = true;
		});
		//add scoped
		services.AddScoped<ICurrentPrincipalService, CurrentPrincipleService>();
		services.AddScoped<ICurrentAccountService, CurrentAccountService>();
		services.AddScoped<IChatgptService, ChatgptService>();
		services.AddScoped<IAzureService, AzureService>();
		services.AddScoped<ISchedulerService, SchdulerService>();
		services.AddScoped<IJwtService, JwtService>();
		services.AddScoped<IVietQrService, VietQrService>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IFirebaseService, FirebaseService>();
		services.AddScoped<AuditableEntitySaveChangesInterceptor>();
		services.AddSingleton<INotificationProvider, NotificationProvider>();
		services.AddSingleton<INotificationAdapter, NotificationAdapter>();
		services.AddSingleton<INotifier, Notifier>();
		services.AddSingleton<NotificationConnectionManager>();
		services.AddSingleton<IWebNotificationService, WebNotificationService>();
		services.AddSingleton<IZaloPayService, ZaloPayService>();
		services.AddSingleton<IVnPayService, VnPayService>();
		services.AddSingleton<NotificationConnectionManager>();
		services.AddSingleton<ConnectionManagerServiceResolver>(serviceProvider => type =>
		{
			return type switch
			{
				Type _ when type == typeof(NotificationConnectionManager)
					=> serviceProvider.GetRequiredService<NotificationConnectionManager>(),
				_ => throw new KeyNotFoundException()
			};
		});
		//add signalR
		services.AddSignalR()
			.AddJsonProtocol(options =>
			{
				options.PayloadSerializerOptions.Converters
					.Add(new JsonStringEnumConverter());
			});

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		//config VietQr
		services.AddOptions<VietQrSettings>()
			.BindConfiguration(VietQrSettings.ConfigSection)
			.ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddSingleton(sp => sp.GetRequiredService<IOptions<VietQrSettings>>().Value);
		//redis 
		services.AddOptions<RedisSettings>()
			.BindConfiguration(RedisSettings.ConfigSection)
			.ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddSingleton(sp => sp.GetRequiredService<IOptions<RedisSettings>>().Value);

		services.AddSingleton<ICacheService, CacheService>();
		services.AddStackExchangeRedisCache(redisOptions =>
		{
			using var sc = services.BuildServiceProvider().CreateScope();
			var settings = sc.ServiceProvider.GetRequiredService<RedisSettings>();
			var connection = $"{settings.Host}:{settings.Port},password={settings.Password}";
			redisOptions.Configuration = connection;
		});
		//config ZaloPay
		services.AddOptions<ZaloPaySettings>()
			.BindConfiguration(ZaloPaySettings.ConfigSection)
			.ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddSingleton(sp => sp.GetRequiredService<IOptions<ZaloPaySettings>>().Value);
		//config vnpay
		services.AddOptions<VnPaySettings>()
			.BindConfiguration(VnPaySettings.ConfigSection)
			.ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddSingleton(sp => sp.GetRequiredService<IOptions<VnPaySettings>>().Value);
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
		services.AddSingleton<IContentModeratorClient>(u =>
		{
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

		FirebaseApp.Create(new AppOptions
		{
			Credential = GoogleCredential.FromFile("petcoffeeshop.json"),
		});
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
