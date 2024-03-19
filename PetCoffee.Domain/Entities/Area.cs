
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
    public int Order {  get; set; } 
    public long PetcoffeeShopId { get; set; }
    public PetCoffeeShop PetCoffeeShop { get; set; }


	[Projectable]
	public bool Deleted => DeletedAt != null;

	//[InverseProperty(nameof(Reservation.Area))]
	//public IList<Reservation> Reservations { get; set; } = new List<Reservation>();
	[InverseProperty(nameof(PetArea.Area))]
	public IList<PetArea> PetAreas { get; set; } = new List<PetArea>();

	public long PricePerHour { get; set; }
	public int TotalSeat { get; set; }


    [InverseProperty(nameof(Reservation.Area))]
    public IList<Reservation> Reservations { get; set; } = new List<Reservation>();

}
