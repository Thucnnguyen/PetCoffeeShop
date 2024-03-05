
using LinqKit;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Notifications.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace PetCoffee.Application.Features.Notifications.Queries;

public class GetAllNotificationQuery : PaginationRequest<Notification>, IRequest<PaginationResponse<Notification, NotificationResponse>>
{
	public string? Search { get; set; }

	public NotificationType? Type { get; set; }

	public EntityType? EntityType { get; set; }

	public DateTime? From { get; set; }

	public DateTime? To { get; set; }

	public bool? IsRead { get; set; }

	public NotificationLevel? Level { get; set; }

	[BindNever]
	[JsonIgnore]
	public long? AccountId { get; set; }

	public override Expression<Func<Notification, bool>> GetExpressions()
	{
		if (Search != null)
		{
			Search = Search.Trim().ToLower();
		}

		Expression = Expression.And(notification => Search == null || notification.Content.ToLower().Contains(Search));

		Expression = Expression.And(notification => Type == null || Type.Equals(notification.Type));

		Expression = Expression.And(notification => EntityType == null || EntityType.Equals(notification.EntityType));

		Expression = Expression.And(notification => From == null || notification.CreatedAt >= From);

		Expression = Expression.And(notification => To == null || notification.CreatedAt <= To);

		Expression = Expression.And(notification => IsRead == null || notification.IsRead == IsRead);

		Expression = Expression.And(notification => AccountId == null || notification.AccountId == AccountId);

		Expression = Expression.And(notification => Level == null || notification.Level == Level);

		return Expression;
	}
}
