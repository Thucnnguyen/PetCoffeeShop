using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Payments.Models;

public class PaymentResponse
{
    public long Id { get; set; }
    public double Amount { get; set; }
    public string? Content { get; set; }
    public string? ReferenceTransactionId { get; set; }
    public string? Url { get; set; }
    public long? ReservationId { get; set; }
    public long? PetId { get; set; }
    public string? PetName { get; set; }
    public string? ShopName { get; set; }
    public List<TransactionItemResponse> TransactionItems { get; set; }
    public TransactionType Type { get; set; }
}
