using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PostCategory.Commands;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Persistence.Repository;
using TmsApi.Common;

namespace PetCoffee.Application.Features.PostCategory.Handlers;

public class UpdatePostCategoryHandler : IRequestHandler<UpdatePostCategoryCommand, PostCategoryResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public UpdatePostCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<PostCategoryResponse> Handle(UpdatePostCategoryCommand request, CancellationToken cancellationToken)
	{
		var ExistedPostCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id);
		if (ExistedPostCategory == null)
		{
			throw new ApiException(ResponseCode.PostCategoryNotExisted);
		}
		Assign.Partial(request, ExistedPostCategory);
		await _unitOfWork.CategoryRepository.UpdateAsync(ExistedPostCategory);
		await _unitOfWork.SaveChangesAsync();
		return _mapper.Map<PostCategoryResponse>(ExistedPostCategory);
	}
}
