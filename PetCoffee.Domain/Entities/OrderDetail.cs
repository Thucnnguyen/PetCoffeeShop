using LockerService.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("OrderDetail")]
public class OrderDetail : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public long ServiceId { get; set; }
	public Service Service { get; set; }
	public long OrderId {  get; set; }
	public Order Order { get; set; }
	public double Price { get; set; }
	public decimal Reviews { get; set;}
	public int Quantiy { get; set; }

}
