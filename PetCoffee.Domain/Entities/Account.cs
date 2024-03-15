using EntityFrameworkCore.Projectables;
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetCoffee.Domain.Entities;

[Table("Account")]
public class Account 
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
	public string? Address { get; set; }
    public LoginMethod LoginMethod { get; set; }
    public GenderAccount Gender { get; set; }

    public Role Role { get; set; }
	public AccountStatus Status { get; set; } = AccountStatus.Verifying;
	public string? OTP { get; set; }
	public DateTime? OTPExpired { get; set; }
	public DateTime? LastLogin { get; set; }
	public DateTime? EndTimeBlockPost { get; set; }
	public DateTime? EndTimeBlockComment { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

	public DateTime? DeletedAt { get; set; }

	[Projectable]
	public bool Deleted => DeletedAt != null;

	[InverseProperty(nameof(Post.CreatedBy))]
	public IList<Post> Posts { get; set; } = new List<Post>();
	[InverseProperty(nameof(Notification.Account))]
	public IList<Notification> Notifications { get; set; } = new List<Notification>();
	[InverseProperty(nameof(Like.CreatedBy))]
	public IList<Like> Likes { get; set; } = new List<Like>();	
	[InverseProperty(nameof(SubmittingEvent.CreatedBy))]
	public IList<SubmittingEvent> SubmittingEvents { get; set; } = new List<SubmittingEvent>();
	[InverseProperty(nameof(JoinEvent.CreatedBy))]
	public IList<JoinEvent> FollowEvents { get; set; } = new List<JoinEvent>();
	[InverseProperty(nameof(Reservation.CreatedBy))]
	public IList<Reservation> Reservations { get; set; } = new List<Reservation>();
	[InverseProperty(nameof(Comment.CreatedBy))]
	public IList<Comment> Comments { get; set; } = new List<Comment>();
	[InverseProperty(nameof(Report.CreatedBy))]
	public IList<Report> Reports { get; set; } = new List<Report>();
	[InverseProperty(nameof(AccountShop.Account))]
	public IList<AccountShop> AccountShops { get; set; } = new List<AccountShop>();
	[InverseProperty(nameof(RatePet.CreatedBy))]
	public IList<RatePet> PetRattings { get; set; } = new List<RatePet>();
	/*
	 * for staff, manager
	 */


	[Projectable]
	public bool IsActive => Equals(AccountStatus.Active, Status);
	[Projectable]
	public bool IsVerify => Equals(AccountStatus.Verifying, Status);
	[Projectable]
	public bool IsAdmin => Equals(Role, Role.Admin);

	[Projectable]
	public bool IsManager => Equals(Role, Role.Manager);

	[Projectable]
	public bool IsStaff => Equals(Role, Role.Staff);

	[Projectable]
	public bool IsCustomer => Equals(Role, Role.Customer);

	[Projectable]
	public bool IsBlockPost => EndTimeBlockPost != null && DateTime.UtcNow <= EndTimeBlockPost;
	[Projectable]
	public bool IsBlockComment => EndTimeBlockComment != null && DateTime.UtcNow <= EndTimeBlockComment;
	[Projectable]
	public bool IsOTPExpired => OTPExpired != null && DateTime.UtcNow >= OTPExpired;
}
