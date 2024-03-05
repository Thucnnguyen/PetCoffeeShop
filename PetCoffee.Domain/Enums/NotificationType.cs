

using System.ComponentModel;

namespace PetCoffee.Domain.Enums;

public enum NotificationType 
{
	[Description("Like post")]
	LikePost = 0,
	[Description("Comment post")]
	CommentPost = 1,
	[Description("Reply comment")]
	ReplyComment = 2,
	[Description("New Post")]
	NewPost = 3,
	[Description("New Event")]
	NewEvent = 4,
	[Description("Join Event")]
	JoinEvent = 5,
}
