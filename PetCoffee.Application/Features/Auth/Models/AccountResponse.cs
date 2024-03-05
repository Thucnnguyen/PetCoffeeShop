
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Auth.Models;

public class AccountResponse 
{
	public long Id { get; set; }
	public string? FullName { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	public string? Avatar { get; set; }
	public string? Background { get; set; }
	public string? Description { get; set; }
	public string? Address { get; set; }
	public Role Role { get; set; }
	public AccountStatus Status { get; set; }
	public DateTime? EndTimeBlockPost { get; set; }
	public DateTime? EndTimeBlockComment { get; set; } 
	public long TotalIsFollowing { get; set; } = 0;
	public IList<ShopResponseForAccount>? ShopResponses { get; set; }	
}
