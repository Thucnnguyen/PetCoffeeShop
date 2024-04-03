using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.Payments.Models;

namespace PetCoffee.Application.Features.Payments.Commands;

public class CreatePaymentValidation : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentValidation()
    {
        RuleFor(src => src.RequiredAmount ).Must(amount => amount >10.000).WithMessage("Vui lòng nhập số tiền lớn hơn 10.000 VND");
    }
}

public class CreatePaymentCommand : IRequest<PaymentResponse>
{
	public string PaymentContent { get; set; } = string.Empty;
	public long RequiredAmount { get; set; }
}
