

using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class PetArea
{
	[Key]
	public long Id { get; set; }
	public long PetId { get; set; }
	public long AreaId { get; set; }
	public DateTimeOffset StartTime { get; set; }
	public DateTimeOffset? EndTime { get; set; }

	public Pet Pet { get; set; }
	public Area Area { get; set; }
}
