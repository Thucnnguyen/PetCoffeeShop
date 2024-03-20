

using PetCoffee.Application.Features.Post.Models;

namespace PetCoffee.Application.Features.RatePets.Models;



public class RatePetResponse
{
	public long PetId { get; set; }
	public long Rate { get; set; }
	public string? Comment { get; set; }
	public AccountForPostModel? Account { get; set; }
	public DateTimeOffset? CreatedAt { get; set; }
}