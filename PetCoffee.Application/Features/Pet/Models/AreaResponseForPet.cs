

namespace PetCoffee.Application.Features.Pet.Models;

public class AreaResponseForPet
{
	public long Id { get; set; }
	public long Order { get; set; }
	public DateTimeOffset? StartTime { get; set; }
	public DateTimeOffset? EndTime { get; set; }
}
