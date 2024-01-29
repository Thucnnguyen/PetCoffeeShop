
namespace PetCoffee.Domain.Entities;

public class SubmittingEvent
{
	public long Id { get; set; }
	public long EventId { get; set; }
	public long SenderId { get; set; }
	public string? Description { get; set; }

	public Account Sender {  get; set; }
	public Event Event { get; set; }
}
