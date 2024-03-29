using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Packages.Models;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Payments.Models;

public class PaymentResponse : BaseAuditableEntityResponse
{
	public long Id { get; set; }
	public double Amount { get; set; }
	public string? Content { get; set; }
	public string? ReferenceTransactionId { get; set; }
	public string? Url { get; set; }
	public long? ReservationId { get; set; }
	public long? PetId { get; set; }
	public string? PetName { get; set; }
	public List<TransactionItemResponse> TransactionItems { get; set; }
	public TransactionType TransactionType { get; set; }
	public ShopResponseForAccount? Shop { get; set; }
	public PackageResponse? Package { get; set; }
	public AccountForPostModel? Creator { get; set; }

}
