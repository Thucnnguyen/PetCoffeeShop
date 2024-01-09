using EntityFrameworkCore.Projectables;
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("PetCafeShop")]
public class PetCafeShop
{
	[Key]
	public long Id { get; set; }
	public string Name { get; set; }
	public string? Description { get; set; }
	public string? WebsiteUrl { get; set; }
	public string? FbUrl { get; set; }
	public string? InstagramUrl { get; set; }
	public string? AvatarUrl { get; set; }
	public string Phone {  get; set; }
	public string Email { get; set; }
	public string Location { get; set; }
	public ShopStatus status { get; set; }
	public DateTimeOffset? StartTime { get; set; }
	public DateTimeOffset? EndTime { get;set; }

	// for holiday
	public DateTimeOffset? SpecialStartTime { get; set; }
	public DateTimeOffset? SpecialEndTime { get; set; }



	public IList<Account> Staffs { get; set; } = new List<Account>();
	public IList<Service> Services { get; set; } = new List<Service>();
	public IList<Pet> Pets { get; set; } = new List<Pet>();
	public IList<Event> Events { get; set; } = new List<Event>();

}
