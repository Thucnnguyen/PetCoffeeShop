
using LockerService.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class Item : BaseAuditableEntity
{
	[Key]
	public long ItemId {  get; set; }
	public string Name { get; set; }	
	public double Price { get; set; }
	public string Description { get; set; }

}
