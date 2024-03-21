

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Wallet")]
public class Wallet : BaseAuditableEntity
{
    [Key]
    public long Id { get; set; }
    public decimal Balance { get; set; }

    [InverseProperty(nameof(Transaction.Wallet))]
    public IList<Transaction> Transactions { get; set; } = new List<Transaction>();

    [InverseProperty(nameof(WalletItem.Wallet))]
    public IList<WalletItem> Items { get; set; } = new List<WalletItem>();


    public Wallet()
    {
        Balance = 0;
    }

    public Wallet(decimal initialBalance)
    {
        Balance = initialBalance;
    }
}
