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
        var Comments =  _unitOfWork.CommentRepository.Get(c => c.PostId == request.PostId,disableTracking:true)
                                                    .Include(c => c.CreatedBy)        
                                                    .ToList();

        var ShowComments = Comments.Where(c => c.ParentCommentId != null);
        var response = new List<CommentResponse>();
        foreach (var comment in Comments)
        {
            var commentResponse = _mapper.Map<CommentResponse>(comment);
            commentResponse.TotalSubComments = Comments.Count(c => c.ParentCommentId == comment.Id);
            response.Add(commentResponse);
        }
        return new PaginationResponse<Domain.Entities.Comment, CommentResponse>(
        response,
        response.Count(),
        request.PageNumber,
        request.PageSize);
    }
}
