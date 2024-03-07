

using MediatR;
using PetCoffee.Application.Features.Payments.Commands;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Application.Service.Payment;

namespace PetCoffee.Application.Features.Payments.Handlers;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, PaymentResponse>
{
	private readonly IZaloPayService _payService;

	public CreatePaymentHandler(IZaloPayService payService)
	{
		_payService = payService;
	}


    public async Task<PaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
	{
		var newZaloPayemnt = new ZaloPayment()
		{
			Amount = request.RequiredAmount,
			Info = request.PaymentContent
		};
		var transaction = await _payService.CreatePayment(newZaloPayemnt);

		return new PaymentResponse()
		{
			PaymentId = transaction.ReferenceTransactionId,
			PaymentUrl = transaction.Url,
		};
	}
}
