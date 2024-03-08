
using MediatR;
using PetCoffee.Application.Features.Payments.Models;

namespace PetCoffee.Application.Features.Payments.Commands;

public class ProcessVnpayPaymentIpnCommand : VnPaymentCallback, IRequest
{
}
