
using MediatR;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Features.Payments.Commands;

namespace PetCoffee.Application.Features.Payments.Handlers;

public class CallBackZaloPayHandler : IRequestHandler<CallbackZaloPayCommand>
{
    private readonly ILogger<CallBackZaloPayHandler> _logger;

    public CallBackZaloPayHandler(ILogger<CallBackZaloPayHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CallbackZaloPayCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Callback Zalopay");
        return Task.CompletedTask;
    }
}
