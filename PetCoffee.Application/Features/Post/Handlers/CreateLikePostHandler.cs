

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Post.Handlers;

public class CreateLikePostHandler : IRequestHandler<CreateLikePostCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ICurrentAccountService _currentAccountService;

	public CreateLikePostHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
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

		var post = await _unitOfWork.PostRepository.GetByIdAsync(request.PostId);
		if (post == null)
		{
			throw new ApiException(ResponseCode.PostNotExisted);
		}

		var LikePost = await _unitOfWork.LikeRepository.GetAsync(l => l.PostId == request.PostId && l.CreatedById == curAccount.Id);
		if(LikePost.Any())
		{
			return false;
		}
		var newLikePost = _mapper.Map<Like>(request);
		await _unitOfWork.LikeRepository.AddAsync(newLikePost);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
