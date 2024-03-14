
namespace PetCoffee.Domain.Entities;

public class ReservationTable
{
	public long TableId { get; set; }
	public long ReservationId { get; set;}

	public Table Table { get; set; }
	public Reservation Reservation { get; set; }
}
