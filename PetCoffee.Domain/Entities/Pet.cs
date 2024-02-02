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
	public PetCoffeeShop PetCoffeeShop { get; set; }
	public long? AreaId { get; set; }
	public Area? Area { get; set; }
	[InverseProperty(nameof(Vaccination.Pet))]
	public IList<Vaccination> Comments { get; set; } = new List<Vaccination>();
	[InverseProperty(nameof(Diary.Pet))]
	public IList<Diary> Diaries { get; set; } = new List<Diary>();
}
