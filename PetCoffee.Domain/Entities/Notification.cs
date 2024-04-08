using EntityFrameworkCore.Projectables;
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Notification")]
public class Notification : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string Title { get; set; }
	public string Content { get; set; }
	public string? Data { get; set; }
	public EntityType EntityType { get; set; }
	public NotificationType Type { get; set; }
	public string? ReferenceId { get; set; }
	public DateTimeOffset? ReadAt { get; set; }
	public NotificationLevel Level { get; set; } = NotificationLevel.Information;
	public long? ShopId { get; set; }
	public long AccountId { get; set; }
	public Account Account { get; set; }
	[Projectable]
	public bool IsRead => ReadAt != null;
	public Notification()
	{
	}
	public Notification(Account account, NotificationType type, EntityType entityType, object? data = null, long? shopId = null, bool ? saved = true)
	{
		Type = type;
		Account = account;
		AccountId = account.Id;
		EntityType = entityType;
		ShopId = shopId;

		switch (Type)
		{
			case NotificationType.LikePost:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var like = (Like)data;

					Title = $"Bài viết của bạn có thêm lượt thích";
					Content = $"{like.CreatedBy.FullName} đã thích bài viết của bạn";
					Level = NotificationLevel.Information;
					ReferenceId = like.PostId.ToString();
					break;
				}
			case NotificationType.CommentPost:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var comment = (Comment)data;

					Title = $"Bạn có bình luận mới";
					Content = $"{comment.CreatedBy.FullName} đã bình luận bài viết của bạn";
					Level = NotificationLevel.Information;
					ReferenceId = comment.PostId.ToString();

					break;
				}
			case NotificationType.ReplyComment:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var comment = (Comment)data;

					Title = $"Bạn có một bình luận mới";
					Content = $"{comment.CreatedBy.FullName} đã trả lời bình luận của bạn";
					Level = NotificationLevel.Information;
					ReferenceId = comment.PostId.ToString();

					break;
				}
			case NotificationType.NewPost:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var post = (Post)data;

					Title = $"Bài viết mới có thể bạn quan tâm";
					Content = $"{post.PetCoffeeShop.Name} đã đăng một bài viết mới";
					Level = NotificationLevel.Information;
					ReferenceId = post.Id.ToString();

					break;
				}
			case NotificationType.NewEvent:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var newEvent = (Event)data;

					Title = $"Sự kiện mới đang chờ bạn tham gia!";
					Content = $"{newEvent.PetCoffeeShop.Name} đã tạo một sự kiện mới";
					Level = NotificationLevel.Information;
					ReferenceId = newEvent.Id.ToString();

					break;
				}
			case NotificationType.JoinEvent:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var newEvent = (Event)data;

					Title = $"Có người mới tham gia sự kiện của bạn";
					Content = $"{newEvent.PetCoffeeShop.Name} đã tham gia {newEvent.Title} ";
					Level = NotificationLevel.Information;
					ReferenceId = newEvent.Id.ToString();

					break;
				}
			case NotificationType.Donation:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var transaction = (Transaction)data;

					Title = $"Thú cưng được tặng quà";
					Content = $"{transaction.Pet.Name} đã được tặng quà bời {transaction.CreatedBy.FullName}";
					Level = NotificationLevel.Information;
					ReferenceId = transaction.Id.ToString();

					break;
				}
			case NotificationType.NewFollower:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var follow = (FollowPetCfShop)data;

					Title = $"Quán của bạn có người theo dõi mới";
					Content = $"{follow.CreatedBy.FullName} đã theo dõi {follow.Shop.Name}";
					Level = NotificationLevel.Information;
					ReferenceId = follow.ShopId.ToString();
					break;
				}
			case NotificationType.ReturnOrder:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var reservation = (Reservation)data;

					Title = $"Đơn hàng của bạn đã được hoàn tiền";
					Content = $"{reservation.CreatedBy.FullName} đơn hàng {reservation.Code} của bạn đã bị hủy!";
					Level = NotificationLevel.Information;
					ReferenceId = reservation.Id.ToString();

					break;
				}
			case NotificationType.BuyNewPackage:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var transaction = (Transaction)data;

					Title = $"Có của hàng mới mua package";
					Content = $"{transaction.PetCoffeeShop.Name} đã mua gói {transaction.PackagePromotion.Description}.";
					Level = NotificationLevel.Information;
					ReferenceId = transaction.Id.ToString();

					break;
				}

			case NotificationType.NewReportComment:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var reportComment = (Report)data;

					Title = $"Có báo cáo mới về bình luận!";
					Content = $"{reportComment.CreatedBy.FullName} đã báo cáo  bình luận";
					Level = NotificationLevel.Information;
					ReferenceId = reportComment.Id.ToString();

					break;
				}
			case NotificationType.NewReportPost:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var reportComment = (Report)data;

					Title = $"Có báo cáo mới về bài viết!";
					Content = $"{reportComment.CreatedBy.FullName} đã báo cáo một bài viết";
					Level = NotificationLevel.Information;
					ReferenceId = reportComment.Id.ToString();

					break;
				}
			case NotificationType.ChangePetArea:
				{
					if (data == null)
					{
						throw new Exception($"[Notification] Data is required. Type: {Type}");
					}

					var reservation = (Reservation)data;

					Title = $"Thú cưng trong đơn hàng của bạn đổi tầng";
					Content = $"Thú cưng trong đơn hàng {reservation.Code} đã chuyển sang tầng khác bạn có thể hủy đơn hàng và hoàn về 100%";
					Level = NotificationLevel.Information;
					ReferenceId = reservation.Id.ToString();

					break;
				}
		}
	}
};
