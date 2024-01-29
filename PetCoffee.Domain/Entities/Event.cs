
using LockerService.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Event")]
public class Event : BaseAuditableEntity
{
	public int Id { get; set; }
	public string? Title { get; set; }
	public string? Content { get; set; }
	public string? Image {  get; set; }
	public string? Description { get; set; }	
	public DateTimeOffset StartTime { get; set; }
	public DateTimeOffset EndTime { get; set; }
	public string? Location { get; set; }

	public long PetCafeShopId { get; set; }
	public PetCoffeeShop PetCafeShop { get; set; }

	
}
