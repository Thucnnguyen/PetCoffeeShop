using MediatR;
using PetCoffee.Application.Features.Payments.Models;

namespace PetCoffee.Application.Features.Payments.Commands;

public class CreatePaymentCommand : IRequest<PaymentResponse>
{
    public string PaymentContent { get; set; } = string.Empty;
    public string PaymentCurrency { get; set; } = string.Empty;
    public long RequiredAmount { get; set; }
}
