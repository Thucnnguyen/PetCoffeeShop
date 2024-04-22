using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Comment.Commands;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Comment.Handlers;

public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;
	private readonly INotifier _notifier;

	public CreateCommentHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper, INotifier notifier)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
		_notifier = notifier;
	}

	public async Task<CommentResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
	{
		//get Current account 
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		// check is shop expired
		if (request.ShopId != null)
		{
			var shop = await _unitOfWork.PetCoffeeShopRepository
						.Get(s => s.Id == request.ShopId && !s.Deleted && s.IsBuyPackage)
						.FirstOrDefaultAsync();
			if (shop == null)
			{
				throw new ApiException(ResponseCode.ShopIsExpired);
			}
		}

		//check post info
		var post = await _unitOfWork.PostRepository.Get(po => po.Id == request.PostId)
													.Include(po => po.CreatedBy)
													.FirstOrDefaultAsync();
		if (post == null)
		{
			throw new ApiException(ResponseCode.PetNotExisted);
		}

		var NewComment = _mapper.Map<Domain.Entities.Comment>(request);
		if (request.Image != null)
		{
			await _azureService.CreateBlob(request.Image.FileName, request.Image);
			NewComment.Image = await _azureService.GetBlob(request.Image.FileName);
		}
		await _unitOfWork.CommentRepository.AddAsync(NewComment);
		await _unitOfWork.SaveChangesAsync();

		var NewCommentData = (await _unitOfWork.CommentRepository
								.GetAsync(
									predicate: c => c.Id == NewComment.Id,
									includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Comment, object>>>
									{
										c => c.CreatedBy,
										c => c.PetCoffeeShop
									})).FirstOrDefault();

		var response = _mapper.Map<CommentResponse>(NewCommentData);
		//for reply
		if (NewCommentData.ParentCommentId != null)
		{

			var parrentComment = await _unitOfWork.CommentRepository.Get(pa => pa.Id == NewCommentData.ParentCommentId)
												.Include(pa => pa.CreatedBy)
												.FirstOrDefaultAsync();
			if (parrentComment != null && parrentComment.CreatedById != currentAccount.Id)
			{
				var notificationForComment = new Notification(
					account: parrentComment.CreatedBy,
					type: NotificationType.CommentPost,
					entityType: EntityType.Post,
					data: NewCommentData,
					post.ShopId
				);
				await _notifier.NotifyAsync(notificationForComment, true);
			}
		}
		// notification for poster
		if(currentAccount.Id != post.CreatedById)
		{
			var notificationForPoster = new Notification(
					account: post.CreatedBy,
					type: NotificationType.ReplyComment,
					entityType: EntityType.Post,
					data: NewCommentData,
					post.ShopId
				);
			await _notifier.NotifyAsync(notificationForPoster, true);
		}

		return response;

	}
}
