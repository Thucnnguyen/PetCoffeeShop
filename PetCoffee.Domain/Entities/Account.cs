using EntityFrameworkCore.Projectables;
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
	public string? Address { get; set; }
    public LoginMethod LoginMethod { get; set; }

    public Role Role { get; set; }
	public AccountStatus Status { get; set; } = AccountStatus.Verifying;
	public string? OTP { get; set; }
	public DateTime? OTPExpired { get; set; }
	public DateTime? LastLogin { get; set; }
	public DateTime? EndTimeBlockPost { get; set; }
	public DateTime? EndTimeBlockComment { get; set; }

	[InverseProperty(nameof(Post.CreatedBy))]
	public IList<Post> Posts { get; set; } = new List<Post>();
	[InverseProperty(nameof(Notification.Account))]
	public IList<Notification> Notifications { get; set; } = new List<Notification>();
	[InverseProperty(nameof(Like.CreatedBy))]
	public IList<Like> Likes { get; set; } = new List<Like>();	
	[InverseProperty(nameof(SubmittingEvent.CreatedBy))]
	public IList<SubmittingEvent> SubmittingEvents { get; set; } = new List<SubmittingEvent>();
	[InverseProperty(nameof(FollowEvent.CreatedBy))]
	public IList<FollowEvent> FollowEvents { get; set; } = new List<FollowEvent>();
	[InverseProperty(nameof(Reservation.CreatedBy))]
	public IList<Reservation> Reservations { get; set; } = new List<Reservation>();
	[InverseProperty(nameof(Comment.CreatedBy))]
	public IList<Comment> Comments { get; set; } = new List<Comment>();
	[InverseProperty(nameof(Report.CreatedBy))]
	public IList<Report> Reports { get; set; } = new List<Report>();
	/*
	 * for staff, manager
	 */
	public long? PetCoffeeShopId { get; set; }
	public PetCoffeeShop? PetCoffeeShop { get; set; }

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
	public bool IsBlockPost => EndTimeBlockPost != null && DateTime.Now <= EndTimeBlockPost;
	[Projectable]
	public bool IsBlockComment => EndTimeBlockComment != null && DateTime.Now <= EndTimeBlockComment;
	[Projectable]
	public bool IsOTPExpired => OTPExpired != null && DateTime.Now >= OTPExpired;
}
