using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Packages.Models;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Transactions.Models
{
	public class TransactionResponse : BaseAuditableEntityResponse
	{

		public long Id { get; set; }

		public long WalletId { get; set; }
		public double Amount { get; set; }
		public long? RemitterId { get; set; }

		public long? ReservationId { get; set; }
		public AccountForPostModel? Creator {  get; set; }
		public string? Content { get; set; }

		public List<TransactionItemResponse> Items { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
		public TransactionType TransactionType { get; set; }
		public ShopResponseForAccount? Shop { get; set; }
		public PackageResponse? Package { get; set; }
	}
}
