
using FluentValidation;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Common.Models.Response;
using System.Net;
using System.Text.Json;

namespace PetCoffee.Application.Common.Exceptions;

public class ExceptionMidleware
{
    private readonly RequestDelegate _next;
    public ExceptionMidleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiException exception)
        {
            await HandleApiExceptionAsync(context, exception);
        }
        catch (ValidationException exception)
        {
            await HandleValidationExceptionASync(context, exception);
        }
        catch (Exception exception)
        {
            await HandleInternalServerExceptionAsync(context, exception);
        }
    }

    private async Task HandleInternalServerExceptionAsync(HttpContext context, Exception exception)
    {
        await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, new ExceptionResponse(exception));
    }

    private async Task HandleValidationExceptionASync(HttpContext context, ValidationException exception)
    {
        await HandleExceptionAsync(context, HttpStatusCode.BadRequest, new ExceptionResponse(exception));
    }

    private async Task HandleApiExceptionAsync(HttpContext context, ApiException exception)
    {
        await HandleExceptionAsync(context, HttpStatusCode.BadRequest, new ExceptionResponse(exception));
    }

    private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, ExceptionResponse response)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
