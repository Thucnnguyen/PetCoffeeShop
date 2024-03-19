

using PetCoffee.Application.Features.Post.Models;

namespace PetCoffee.Application.Features.RatePets.Models;

public class RatePetResponseForCus
{
	public bool IsRate { get; set; }
	public List<RatePetResponse>? RatePets { get; set; }
}

public class RatePetResponse
{
	public long PetId { get; set; }
	public long Rate { get; set; }
	public string? Comment { get; set; }
	public AccountForPostModel? Account { get; set; }
}