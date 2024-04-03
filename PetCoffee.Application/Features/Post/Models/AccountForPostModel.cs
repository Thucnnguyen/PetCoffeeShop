
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Post.Models;

public class AccountForPostModel
{
	public int Id { get; set; }
	public string FullName { get; set; }
	public string? Avatar { get; set; }	
	public Role Role { get; set; }
	public string Email { get; set; }
	public string? Address { get; set; }
	public string PhoneNumber { get; set; }
}
