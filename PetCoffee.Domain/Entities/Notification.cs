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
    public string Data { get; set; }
    public EntityType EntityType { get; set; }
    public NotificationType Type { get; set; }
    public string? ReferenceId { get; set; }
    public DateTimeOffset? ReadAt { get; set; }
    public NotificationLevel Level { get; set; } = NotificationLevel.Information;

    public long AccountId { get; set; }
    public Account Account { get; set; }
    [Projectable]
    public bool IsRead => ReadAt != null || Deleted;
    public Notification()
    {
    }
    public Notification(Account account, NotificationType type, EntityType entityType, object? data = null, bool? saved = true)
    {
        Type = type;
        Account = account;
        AccountId = account.Id;
        EntityType = entityType;
        switch (Type)
        {
            case NotificationType.LikePost:
                {
                    if (data == null)
                    {
                        throw new Exception($"[Notification] Data is required. Type: {Type}");
                    }

                    var like = (Like)data;

                    Title = $"Bạn có một lượt thích bải viết mới";
                    Content = $"{like.CreatedBy.FullName} đã thích bài viết của bạn";
                    Level = NotificationLevel.Information;

                    EntityType = EntityType.Post;
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

                    Title = $" Bạn có một bình luận mới";
                    Content = $"{comment.CreatedBy.FullName} đã bình luận bài viết của bạn";
                    Level = NotificationLevel.Information;
                    ReferenceId = comment.Id.ToString();

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
                    Content = $"{comment.CreatedBy.FullName} đã trả lời bình luận bài viết của bạn";
                    Level = NotificationLevel.Information;
                    ReferenceId = comment.Id.ToString();

                    break;
                }
            case NotificationType.NewPost:
                {
                    if (data == null)
                    {
                        throw new Exception($"[Notification] Data is required. Type: {Type}");
                    }

                    var post = (Post)data;

                    Title = $"Bài viết mới của quán bạn theo dõi";
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

                    Title = $"Sự kiện mới của quán bạn theo dõi";
                    //Content = $"{newEvent.PetCoffeeShop.Name} đã tạo một sự kiện mới";
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

                    Title = $"Sự kiện mới của quán bạn theo dõi";
                    Content = $"{newEvent.PetCoffeeShop.Name} đã tạo một sự kiện mới";
                    Level = NotificationLevel.Information;
                    ReferenceId = newEvent.Id.ToString();

                    break;
                }
        }
    }
};
