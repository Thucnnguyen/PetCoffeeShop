
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Auth.Models;

public class AccountForRecord
{
	public long Id { get; set; }
	public string Email { get; set; }
	public string? Avatar { get; set; }
	public string PhoneNumber { get; set; }
	public string Phone { get; set; }
	public Role Role { get; set; }
	public AccountStatus Status { get; set; }

}
