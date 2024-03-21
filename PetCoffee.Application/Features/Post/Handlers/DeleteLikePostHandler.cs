
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Post.Handlers;

public class DeleteLikePostHandler : IRequestHandler<DeleteLikePostCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentAccountService _currentAccountService;

    public DeleteLikePostHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentAccountService = currentAccountService;
    }

    public async Task<bool> Handle(DeleteLikePostCommand request, CancellationToken cancellationToken)
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

        var LikePost = await _unitOfWork.LikeRepository.GetAsync(l => l.PostId == request.PostId && l.CreatedById == curAccount.Id);
        if (!LikePost.Any())
        {
            return false;
        }

        await _unitOfWork.LikeRepository.DeleteAsync(new Like { PostId = request.PostId, CreatedById = curAccount.Id });
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
