using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PostCategory.Commands;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.PostCategory.Handlers;

public class CreatePostCategoryHandler : IRequestHandler<CreatePostCategoryCommand, PostCategoryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreatePostCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PostCategoryResponse> Handle(CreatePostCategoryCommand request, CancellationToken cancellationToken)
    {
        var CategoryExisted = _unitOfWork.CategoryRepository.IsExisted(c => c.Name == request.Name);
        if (CategoryExisted)
        {
            throw new ApiException(ResponseCode.PostCategoryIsExisted);
        }
        var NewCategory = await _unitOfWork.CategoryRepository.AddAsync(_mapper.Map<Category>(request));
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PostCategoryResponse>(NewCategory);
    }
}
