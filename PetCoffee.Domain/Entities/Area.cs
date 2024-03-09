
using EntityFrameworkCore.Projectables;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

public class Area : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string? Description { get; set; }
	public string? Image { get; set; }
	public int TotalSeat { get; set; }
    public int? TotalSeatAvailable { get; set; }
    public int Order {  get; set; } 
    public long PetcoffeeShopId { get; set; }
    public PetCoffeeShop PetCoffeeShop { get; set; }


	[Projectable]
	public bool Deleted => DeletedAt != null;

	[InverseProperty(nameof(Reservation.Area))]
	public IList<Reservation> Reservations { get; set; } = new List<Reservation>();
	[InverseProperty(nameof(Pet.Area))]
	public IList<Pet> Pets { get; set; } = new List<Pet>();
}
