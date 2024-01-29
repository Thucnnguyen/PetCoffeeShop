using EntityFrameworkCore.Projectables;
using LockerService.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("PetCoffeeShop")]
public class PetCoffeeShop : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string Name { get; set; }
	public string? Description { get; set; }
	public string? WebsiteUrl { get; set; }
	public string? FbUrl { get; set; }
	public string? InstagramUrl { get; set; }
	public string? AvatarUrl { get; set; }
	public string? BackgroundUrl { get; set; }

	public string Phone {  get; set; }
	public string Email { get; set; }
	public string Location { get; set; }
	public double? Latitude { get; set; }
	public double? Longitude { get; set; }
	public ShopStatus Status { get; set; } = ShopStatus.Active;
	public DateTimeOffset? StartTime { get; set; }
	public DateTimeOffset? EndTime { get;set; }

	public IList<Account> Staffs { get; set; } = new List<Account>();
	public IList<Service> Services { get; set; } = new List<Service>();
	public IList<Pet> Pets { get; set; } = new List<Pet>();
	public IList<Event> Events { get; set; } = new List<Event>();

}
