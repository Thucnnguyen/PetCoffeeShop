
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
		var Comments = _unitOfWork.CommentRepository.Get(c => c.ParentCommentId == request.CommentId).Include(c =>c.CreatedBy);
		var response = Comments.Select(c => _mapper.Map<CommentResponse>(c)).ToList();
		return new PaginationResponse<Domain.Entities.Comment, CommentResponse>(
			   response,
			   response.Count(),
			   request.PageNumber,
			   request.PageSize);
	}
}
