
using PetCoffee.Domain.Enums;

namespace PetCoffee.Domain.Entities;

public class Table : BaseAuditableEntity
{
	public long Id { get; set; }
	public string? Image {  get; set; }
	public TableType Type { get; set; }

	public long AreaId { get; set; }
	public Area Area { get; set; }

	public double PricePerHour { get; set; }
}
