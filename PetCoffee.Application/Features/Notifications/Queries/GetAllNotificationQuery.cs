
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

		if(Search is not null)
		{
			Expression = Expression.And(notification =>  notification.Content.ToLower().Contains(Search));
		}
		if(Type is not null)
		{
			Expression = Expression.And(notification => Type.Equals(notification.Type));
		}
		if(EntityType is not null)
		{
			Expression = Expression.And(notification =>  EntityType.Equals(notification.EntityType));
		}
		if(From is not null)
		{
			Expression = Expression.And(notification => notification.CreatedAt >= From);
		}
		if(To is not null)
		{
			Expression = Expression.And(notification => notification.CreatedAt <= To);
		}
		if(IsRead is not null)
		{
			Expression = Expression.And(notification =>  notification.IsRead == IsRead);
		}
		if(AccountId is not null)
		{
			Expression = Expression.And(notification => notification.AccountId == AccountId);
		}
		if(Level is not null)
		{
			Expression = Expression.And(notification => notification.Level == Level);
		}

		return Expression;
	}
}
