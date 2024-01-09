

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Wallet")]
public class Wallet
{
	[Key]
	public long Id { get; set; }
	public decimal Balance { get; set; }
	public long AccountId { get; set; }
	public Account Account { get; set; }

	public IList<Transaction> Transactions { get; set; } = new List<Transaction>();

	public Wallet()
	{
		Balance = 0;
	}

	public Wallet(decimal initialBalance)
	{
		Balance = initialBalance;
	}
}
