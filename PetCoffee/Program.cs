using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PetCoffee.Application;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Infrastructure;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		Description = "Standard Authorization header using the Bearer scheme (example: bearer {Token})",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
	});
	options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.ConfigureApplicationServices(builder.Configuration);
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(builder =>
{
	builder.AllowAnyHeader();
	builder.AllowAnyMethod();
	builder.AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMidleware>();

app.Run();
