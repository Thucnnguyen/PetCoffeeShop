using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Comment.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Comment.Handlers;

public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public DeleteCommentHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
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
		if(currentAccount.IsCustomer)
		{
			var comment = await _unitOfWork.CommentRepository.GetByIdAsync(request.CommentId);

			if (comment == null)
			{
				return false;
			}

			if (comment.CreatedById != currentAccount.Id)
			{
				throw new ApiException(ResponseCode.PermissionDenied);
			}
			var SubComments = await _unitOfWork.CommentRepository.GetAsync(s => s.ParentCommentId == comment.Id);
			if (SubComments.Any())
			{
				await _unitOfWork.CommentRepository.DeleteRange(SubComments);
			}
			await _unitOfWork.CommentRepository.DeleteAsync(comment);
		}

		if (currentAccount.IsCustomer)
		{
			var comment = await _unitOfWork.CommentRepository.GetByIdAsync(request.CommentId);

			if (comment == null)
			{
				return false;
			}

			if (comment.CreatedById != currentAccount.Id)
			{
				throw new ApiException(ResponseCode.PermissionDenied);
			}
			var SubComments = await _unitOfWork.CommentRepository.GetAsync(s => s.ParentCommentId == comment.Id);
			if (SubComments.Any())
			{
				foreach ( var subComment in SubComments)
				{
					subComment.DeletedAt = DateTimeOffset.UtcNow;
					await _unitOfWork.CommentRepository.UpdateAsync(subComment);
				}
			}
			comment.DeletedAt = DateTimeOffset.UtcNow;
			await _unitOfWork.CommentRepository.UpdateAsync(comment);
		}

		if (currentAccount.IsStaff && currentAccount.IsManager)
		{
			var ShopIds = currentAccount.AccountShops.Select(acs => acs.ShopId);
			var comment = await _unitOfWork.CommentRepository.GetByIdAsync(request.CommentId);

			if (comment == null)
			{
				return false;
			}

			if (comment.CreatedById != currentAccount.Id)
			{
				throw new ApiException(ResponseCode.PermissionDenied);
			}
			var SubComments = await _unitOfWork.CommentRepository.GetAsync(s => s.ParentCommentId == comment.Id);
			if (SubComments.Any())
			{
				foreach (var subComment in SubComments)
				{
					subComment.DeletedAt = DateTimeOffset.UtcNow;
					await _unitOfWork.CommentRepository.UpdateAsync(subComment);
				}
			}
			comment.DeletedAt = DateTimeOffset.UtcNow;
			await _unitOfWork.CommentRepository.UpdateAsync(comment);
		}

		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
