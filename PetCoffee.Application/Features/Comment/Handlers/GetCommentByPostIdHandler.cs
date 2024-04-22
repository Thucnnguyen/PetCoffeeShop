using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Features.Comment.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Comment.Handlers;

public class GetCommentByPostIdHandler : IRequestHandler<GetCommentByPostIdQuery, PaginationResponse<Domain.Entities.Comment, CommentResponse>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetCommentByPostIdHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<PaginationResponse<Domain.Entities.Comment, CommentResponse>> Handle(GetCommentByPostIdQuery request, CancellationToken cancellationToken)
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

		var Comments = _unitOfWork.CommentRepository.Get(c => c.PostId == request.PostId && !c.Deleted && !reportedCommentIds.Contains(c.Id), disableTracking: true)
													.Include(c => c.CreatedBy)
													.Include(c => c.PetCoffeeShop)
													.OrderByDescending(c => c.CreatedAt)
													.ToList();
		if(!Comments.Any())
		{
			return new PaginationResponse<Domain.Entities.Comment, CommentResponse>(
				new List<CommentResponse>(),
				0,
				request.PageNumber,
				request.PageSize);
		}
		var ShowComments = Comments.Where(c => c.ParentCommentId == null)
							.Skip((request.PageNumber - 1) * request.PageSize)
							.Take(request.PageSize);

		var response = new List<CommentResponse>();
		foreach (var comment in ShowComments)
		{
			var commentResponse = _mapper.Map<CommentResponse>(comment);
			commentResponse.TotalSubComments = Comments.Count(c => c.ParentCommentId == comment.Id);
			response.Add(commentResponse);
		}

		return new PaginationResponse<Domain.Entities.Comment, CommentResponse>(
		response,
		Comments.Count(),
		request.PageNumber,
		request.PageSize);
	}
}
