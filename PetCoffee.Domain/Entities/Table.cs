
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

public class Table : BaseAuditableEntity
{
	public long Id { get; set; }
	public string? Image {  get; set; }
	public TableType Type { get; set; }

	public long AreaId { get; set; }
	public Area Area { get; set; }

	public double PricePerHour { get; set; }


    [InverseProperty(nameof(ReservationTable.Reservation))]
    public IList<ReservationTable> ReservationTables { get; set; } = new List<ReservationTable>();
}
