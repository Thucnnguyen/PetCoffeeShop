
namespace PetCoffee.Application.Service.Payment;

public class VnPayPayment
{
	public string PaymentReferenceId { get; set; } = default!;

	public long Amount { get; set; }

	public string? Info { get; set; }

	public string OrderType { get; set; } = default!;

	public DateTimeOffset Time { get; set; }
}
