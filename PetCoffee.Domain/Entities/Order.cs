using LockerService.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PetCoffee.Domain.Entities;
[Table("Order")]
public class Order : BaseAuditableEntity
{
	[Key] 
	public long Id { get; set; }
	public decimal TotalPrice { get; set; }
	public OrderStatus Status { get; set; }	
	public decimal Discount { get; set; }
	public DateTimeOffset ReceiveAt { get; set; }
	public string? Note { get; set; }
	public decimal Deposit { get; set; }
	public string Code { get; set; }
	public long CafeShopId { get; set; }
	public PetCafeShop CafeShop { get; set; }
}
