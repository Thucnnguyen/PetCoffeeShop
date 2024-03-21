

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

namespace PetCoffee.Application.Features.Comment.Handlers;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAzureService _azureService;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IMapper _mapper;
    private readonly INotifier _notifier;

    public CreateCommentCommandHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper, INotifier notifier)
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
        //if (NewCommentData.ParentCommentId == null)
        //{
        //	var notificationForComment = new Notification(
        //			account: post.CreatedBy,
        //			type: NotificationType.CommentPost,
        //			entityType: EntityType.Like,
        //			data: NewCommentData
        //		);
        //	await _notifier.NotifyAsync(notificationForComment, true);
        //	return response;
        //}

        //var notificationForReply = new Notification(
        //			account: post.CreatedBy,
        //			type: NotificationType.ReplyComment,
        //			entityType: EntityType.Like,
        //			data: NewCommentData
        //		);
        //await _notifier.NotifyAsync(notificationForReply, true);
        return response;


    }
}
