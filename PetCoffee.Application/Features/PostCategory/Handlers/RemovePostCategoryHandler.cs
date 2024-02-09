
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PostCategory.Commands;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.PostCategory.Handlers;

public class RemovePostCategoryHandler : IRequestHandler<RemovePostCategoryCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public RemovePostCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<bool> Handle(RemovePostCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id);
		if (category == null)
		{
			throw new ApiException(ResponseCode.PostCategoryNotExisted);
		}

		await _unitOfWork.CategoryRepository.DeleteAsync(category);
		await _unitOfWork.SaveChangesAsync();

		return true;
	}
}
