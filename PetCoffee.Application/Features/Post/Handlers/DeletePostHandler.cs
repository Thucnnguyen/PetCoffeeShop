

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Post.Handlers;

public class DeletePostHandler : IRequestHandler<DeletePostCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ICurrentAccountService _currentAccountService;

	public DeletePostHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
	}

	public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
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
		if(curAccount.IsCustomer)
		{
			var post = await _unitOfWork.PostRepository
					.Get(p => p.Id == request.Id && p.CreatedById == curAccount.Id)
					.FirstOrDefaultAsync();

			if (post == null)
			{
				throw new ApiException(ResponseCode.PostNotExisted);
			}

			post.DeletedAt = DateTimeOffset.UtcNow;

			await _unitOfWork.PostRepository.UpdateAsync(post);
		}

		if(curAccount.IsStaff || curAccount.IsManager)
		{
			var ShopIds = curAccount.AccountShops.Select(acs => acs.ShopId);

			var post = await _unitOfWork.PostRepository
					.Get(p => p.Id == request.Id && ShopIds.Any(Id => Id == p.ShopId))
					.FirstOrDefaultAsync();

			if (post == null)
			{
				throw new ApiException(ResponseCode.PostNotExisted);
			}

			post.DeletedAt = DateTimeOffset.UtcNow;

			await _unitOfWork.PostRepository.UpdateAsync(post);
		}

		await _unitOfWork.SaveChangesAsync();
		
		return true;
	}
}
