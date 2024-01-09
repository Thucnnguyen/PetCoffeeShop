using LockerService.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Pet")]
public class Pet : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string Name { get; set; }
	public int Age { get; set; }
	public string? Image {  get; set; }
	public PetStatus PetStatus { get; set; } = PetStatus.Active;
	public long PetCafeShopId { get; set; }
	public PetCafeShop PetCafeShop { get; set; }
}
