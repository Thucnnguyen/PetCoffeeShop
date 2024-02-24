

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Comment.Commands;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Comment.Handlers;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public CreateCommentCommandHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<CommentResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
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
		//check post info
		var post = await _unitOfWork.PostRepository.GetByIdAsync(request.PostId);
		if (post == null)
		{
			throw new ApiException(ResponseCode.PetNotExisted);
		}
		
		var NewComment = _mapper.Map<Domain.Entities.Comment>(request);
		if(request.Image != null)
		{
			await _azureService.CreateBlob(request.Image.FileName, request.Image);
			NewComment.Image = await _azureService.GetBlob(request.Image.FileName);
		}
		await _unitOfWork.CommentRepository.AddAsync(NewComment);
		await _unitOfWork.SaveChangesAsync();

		var response = _mapper.Map<CommentResponse>(NewComment);
		response.Account = _mapper.Map<AccountForPostModel>(currentAccount);
		return response;
	}
}
