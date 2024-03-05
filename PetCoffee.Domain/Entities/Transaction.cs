using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Transaction")]
public class Transaction : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	[ForeignKey("Wallet")]
	public long WalletId { get; set; }
	public Wallet Wallet { get; set; }
	public double Amount { get; set; }
	public long? RemitterId { get; set; }
	public Wallet? Remitter {  get; set; }

	public long? ReservationId { get; set; }
	public Reservation? Reservation { get; set; }
	public string? Content { get; set; }

	// for donate
	[InverseProperty(nameof(TransactionItem.Transaction))]
	public IList<TransactionItem> Items { get; set; } = new List<TransactionItem>();

	public TransactionStatus TransactionStatus { get; set; }
	public TransactionType TransactionType { get; set; }
}
