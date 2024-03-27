
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Auth.Models;

public class AccountForRecord
{
	public long Id { get; set; }
	public string Email { get; set; }
	public string? Avatar { get; set; }
	public Role Role { get; set; }
	public AccountStatus Status { get; set; }

}
