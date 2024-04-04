
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Wallets.Models;

public class GetTransactionAmountResponse 
{
	public decimal Balance { get; set; }
	public decimal Percent {  get; set; }
	public bool IsUp { get; set; } = false;

	public List<TransactionAmountResponse> Transactions { get; set; }
}

public class TransactionAmountResponse
{
	public decimal Amount { get; set; }	
	public TransactionType TransactionTypes { get; set; }
}