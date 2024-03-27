
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Comment.Handlers;

public class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, CommentResponse>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetCommentByIdHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<CommentResponse> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
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

		var comment = await _unitOfWork.CommentRepository.Get(c => c.Id == request.Id, disableTracking: true)
													.Include(c => c.CreatedBy)
													.Include(c => c.PetCoffeeShop)
													.FirstOrDefaultAsync();
		if (comment == null)
		{
			throw new ApiException(ResponseCode.CommentNotExist);
		}

		return _mapper.Map<CommentResponse>(comment);
	}
}
