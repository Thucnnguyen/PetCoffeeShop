
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Features.Comment.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Comment.Handlers;

public class GetSubCommentByCommentIdHandler : IRequestHandler<GetSubCommentByCommentIdQuery, PaginationResponse<Domain.Entities.Comment, CommentResponse>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetSubCommentByCommentIdHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<PaginationResponse<Domain.Entities.Comment, CommentResponse>> Handle(GetSubCommentByCommentIdQuery request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		// get Report post
		var reportedCommentIds = (await _unitOfWork.ReportRepository
			.GetAsync(r => r.CreatedById == currentAccount.Id && r.CommentId != null && r.Status != ReportStatus.Reject))
			.Select(r => r.CommentId)
			.ToList();

		var Comments = await _unitOfWork.CommentRepository
			.GetAsync(
				predicate: c => c.ParentCommentId == request.CommentId && !reportedCommentIds.Contains(c.Id),
				includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Comment, object>>>
				{
					c =>c.CreatedBy,
					c =>c.PetCoffeeShop
				});

		return new PaginationResponse<Domain.Entities.Comment, CommentResponse>(
			   Comments,
			   request.PageNumber,
			   request.PageSize,
			   comment => _mapper.Map<CommentResponse>(comment));
	}
}
