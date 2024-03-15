

using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Transactions.Models;

namespace PetCoffee.Application.Features.Wallets.Models;

public class WalletResponse
{
	public long Id { get; set; }
	public decimal Balance { get; set; }
	public List<ItemWalletResponse> Items { get; set; }
	public List<TransactionResponse> Transactions { get; set; }
}
