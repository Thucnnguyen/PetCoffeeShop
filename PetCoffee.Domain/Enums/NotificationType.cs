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
	[Description("Đơn hàng bị hủy do quán bị vô hiệu hóa")]
	ReturnOrder = 8,
	[Description("Sự kiện bạn tham gia bị hủy do không đủ số lượng")]
	CancelOrder = 9,
	[Description("Có shop mua package mới")]
	BuyNewPackage = 10,
	[Description("Có báo cáo mới về bình luận")]
	NewReportComment = 11,
	[Description("Có báo cáo mới về bình luận")]
	NewReportPost = 12,
	[Description("Đổi vị trí của thú cưng")]
	ChangePetArea = 13,

	[Description("Quán hiện tại chưa có thú cưng, cần thêm thú cưng vào quán")]
	RemindShopAboutNotHavingPet = 14,
	[Description("Có Một cửa hàng mới đăng ký")]
	NewShopRequest = 15,
	[Description("Sự kiện đã hủy")]
	CancelEvent = 16,
}
