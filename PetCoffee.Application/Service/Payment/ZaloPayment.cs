

namespace PetCoffee.Application.Service.Payment;

public class ZaloPayment
{
	public string PaymentReferenceId { get; set; } = default!;

	public long Amount { get; set; }

	public string? Info { get; set; }
}
