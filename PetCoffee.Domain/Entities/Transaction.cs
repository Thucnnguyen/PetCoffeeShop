using LockerService.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

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
	public long RemitterId { get; set; }
	public Account Remitter {  get; set; }
	public string? Content { get; set; }
	public TransactionStatus TransactionStatus { get; set; }
}
