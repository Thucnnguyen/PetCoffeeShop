using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;


namespace PetCoffee.Domain.Entities;
[Table("Order")]
public class Reservation : BaseAuditableEntity
{
	[Key] 
	public long Id { get; set; }
	public decimal TotalPrice { get; set; }
	public OrderStatus Status { get; set; }	
	public decimal Discount { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public string? Note { get; set; }
	public decimal Deposit { get; set; }
	public string Code { get; set; }
	public string? Rate { get; set; }
	public string? Comment { get; set; }

	public long? AreaId { get; set; }
	public Area? Area { get; set; }

    public IList<Transaction> Transactions { get; set; } = new List<Transaction>();


   

}
