using EntityFrameworkCore.Projectables;
using LockerService.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetCoffee.Domain.Entities;

[Table("Account")]
public class Account : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string? FullName { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	[JsonIgnore]
	public string Password { get; set; }
	public string? Avatar { get; set; }
	public string? Background { get; set; }
	public string? Description { get; set; }
	public string Address { get; set; }
    public LoginMethod LoginMethod { get; set; }

    public Role Role { get; set; }
	public AccountStatus Status { get; set; } = AccountStatus.Verifying;
	public string? OTP { get; set; }
	public DateTime? LastLogin { get; set; }
	public DateTime? StartTimePackage { get; set; }
	public DateTime? EndTimePackage { get; set; }
	public DateTime? EndTimeBlockPost { get; set; }
	public DateTime? EndTimeBlockComment { get; set; }

	/*
	 * for staff, manager
	 */
	public long? PetCafeShopId { get; set; }
	public PetCoffeeShop? PetCafeShop { get; set; }

	[Projectable]
	public bool IsActive => Equals(AccountStatus.Active, Status);
	[Projectable]
	public bool IsAdmin => Equals(Role, Role.Admin);

	[Projectable]
	public bool IsManager => Equals(Role, Role.Manager);

	[Projectable]
	public bool IsStaff => Equals(Role, Role.staff);

	[Projectable]
	public bool IsCustomer => Equals(Role, Role.Customer);

	[Projectable]
	public bool IsFullService => DateTimeOffset.Now <= EndTimePackage;
	[Projectable]
	public bool IsBlockPost => EndTimeBlockPost != null && DateTime.Now <= EndTimeBlockPost;
	[Projectable]
	public bool IsBlockComment => EndTimeBlockComment != null && DateTime.Now <= EndTimeBlockComment;
}
