﻿

using System.ComponentModel;

namespace PetCoffee.Domain.Enums;

public enum NotificationType
{
	[Description("Thích bài viết")]
	LikePost = 0,
	[Description("Bình Luận bài viết")]
	CommentPost = 1,
	[Description("Phàn hồi bình luận")]
	ReplyComment = 2,
	[Description("bài đăng mới")]
	NewPost = 3,
	[Description("Sự kiện mới")]
	NewEvent = 4,
	[Description("Tham gia sự kiện")]
	JoinEvent = 5,
	[Description("Tặng quà")]
	Donation = 6,
	[Description("Người Follow Mới")]
	NewFollower = 7,

}
