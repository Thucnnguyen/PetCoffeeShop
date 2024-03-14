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
	public int? BirthYear { get; set; }
	public double? Weight { get; set; }
	public PetGender? Gender { get; set; }
	public string? Avatar {  get; set; }
	public string? Backgound { get; set; }
	public string? Description { get; set; }
	public PetType PetType { get; set; }
	public PetStatus PetStatus { get; set; } = PetStatus.Active;
	public bool Spayed { get; set; } = false;
	public long PetCoffeeShopId { get; set; }
	public PetCoffeeShop PetCoffeeShop { get; set; }
	public long? AreaId { get; set; }
	public Area? Area { get; set; }
	[InverseProperty(nameof(Vaccination.Pet))]
	public IList<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
	[InverseProperty(nameof(Moment.Pet))]
	public IList<Moment> Moments { get; set; } = new List<Moment>();
	[InverseProperty(nameof(Transaction.Pet))]
	public IList<Transaction> Transactions { get; set; } = new List<Transaction>();
	[InverseProperty(nameof(RatePet.Pet))]
	public IList<RatePet> PetRattings { get; set; } = new List<RatePet>();
}
