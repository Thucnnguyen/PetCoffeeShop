
using LockerService.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class Category : BaseAuditableEntity
{
	[Key]
	public int Id { get; set; }
	public string Name { get; set; }
}
