

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Post.Handlers;

public class CreateLikePostHandler : IRequestHandler<CreateLikePostCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly INotifier _notifier;


	public CreateLikePostHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService, INotifier notifier)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
		_notifier = notifier;
	}

	public async Task<bool> Handle(CreateLikePostCommand request, CancellationToken cancellationToken)
	{
		var curAccount = await _currentAccountService.GetCurrentAccount();
		if (curAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (curAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		var post = await _unitOfWork.PostRepository.Get(p => p.Id == request.PostId)
													.Include(p => p.CreatedBy)
													.FirstOrDefaultAsync();
		if (post == null)
		{
			throw new ApiException(ResponseCode.PostNotExisted);
		}

		var LikePost = await _unitOfWork.LikeRepository.GetAsync(l => l.PostId == request.PostId && l.CreatedById == curAccount.Id);
		if (LikePost.Any())
		{
			return false;
		}
		var newLikePost = _mapper.Map<Like>(request);
		await _unitOfWork.LikeRepository.AddAsync(newLikePost);
		await _unitOfWork.SaveChangesAsync();
		newLikePost.CreatedBy = curAccount;
		if (post.ShopId != null)
		{
			var listOfStaffsAndManagers = await _unitOfWork.AccountRepository
				.Get(a => a.AccountShops.Any(a => a.ShopId == post.ShopId))
				.Include(a => a.AccountShops)
				.ToListAsync();

			foreach (var staff in listOfStaffsAndManagers)
			{
				var notification = new Notification(
					account: staff,
					type: NotificationType.LikePost,
					entityType: EntityType.Like,
					data: newLikePost,
					shopId: post.ShopId
				);

				await _notifier.NotifyAsync(notification, true);
			}
		}
		else
		{
			if (post.CreatedById != curAccount.Id)
			{
				var notification = new Notification(
					account: post.CreatedBy,
					type: NotificationType.LikePost,
					entityType: EntityType.Like,
					data: newLikePost
				);

				await _notifier.NotifyAsync(notification, true);
			}

		}
		return true;
	}
}
