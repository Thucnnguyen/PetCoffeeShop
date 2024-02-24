using EntityFrameworkCore.Projectables;
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
	public string TaxCode { get; set; }
	public string Location { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public ShopStatus Status { get; set; } = ShopStatus.Active;
	public ShopType Type { get; set; }
	public DateTime? StartTime { get; set; }
	public DateTime? EndTime { get;set; }
	[InverseProperty(nameof(Account.PetCoffeeShop))]
	public IList<Account> Staffs { get; set; } = new List<Account>();
	[InverseProperty(nameof(PostPetCoffeeShop.Shop))]
	public IList<PostPetCoffeeShop> PostPetCoffeeShops { get; set; } = new List<PostPetCoffeeShop>();

	[InverseProperty(nameof(Pet.PetCoffeeShop))]
	public IList<Pet> Pets { get; set; } = new List<Pet>();
	[InverseProperty(nameof(Event.PetCoffeeShop))]
	public IList<Event> Events { get; set; } = new List<Event>();
	[InverseProperty(nameof(Area.PetCoffeeShop))]
	public IList<Area> Areas { get; set; } = new List<Area>();

	[Projectable]
	public bool IsBuyPackage =>  DateTime.Now <= EndTime;
}
