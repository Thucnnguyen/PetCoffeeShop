

namespace PetCoffee.Application.Features.Wallets.Models;

public class IncomeForShopResponse
{
	public decimal Balanace { get; set; }
	public List<ShopIncomeResponse> MonthAmounts { get; set; }
}

public class ShopIncomeResponse
{
	public string ShopName { get; set; }
	public LinkedList<decimal> Amounts { get; set; }
}