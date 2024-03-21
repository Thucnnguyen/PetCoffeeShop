using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Comment.Commands;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Comment.Handlers;

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAzureService _azureService;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IMapper _mapper;

    public UpdateCommentCommandHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _azureService = azureService;
        _currentAccountService = currentAccountService;
        _mapper = mapper;
    }

    public async Task<CommentResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
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


        var comment = (await _unitOfWork.CommentRepository.GetAsync(
            predicate: c => c.Id == request.Id,
            includes: new List<Expression<Func<Domain.Entities.Comment, object>>>()
            {
                c => c.CreatedBy,
            })).First();
        if (comment == null)
        {
            throw new ApiException(ResponseCode.CommentNotExisted);
        }
        if (comment.CreatedById != currentAccount.Id)
        {
            throw new ApiException(ResponseCode.PermissionDenied);
        }

        comment.Content = request.Content;
        if (request.NewImage != null)
        {
            await _azureService.CreateBlob(request.NewImage.FileName, request.NewImage);
            comment.Image = await _azureService.GetBlob(request.NewImage.FileName);
        }
        await _unitOfWork.CommentRepository.UpdateAsync(comment);
        await _unitOfWork.SaveChangesAsync();
        var response = _mapper.Map<CommentResponse>(comment);
        return response;
    }
}
