using AutoMapper;
using MediatR;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Features.PostCategory.Queries;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.PostCategory.Handlers;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IList<PostCategoryResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public GetAllCategoriesHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	async Task<IList<PostCategoryResponse>> IRequestHandler<GetAllCategoriesQuery, IList<PostCategoryResponse>>.Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
	{
		if (string.IsNullOrEmpty(request.Search))
		{
			request.Search = "";
		}
		var Categories = await _unitOfWork.CategoryRepository.GetAsync(c => c.Name.Contains(request.Search));
		var Response = Categories.Select(c => _mapper.Map<PostCategoryResponse>(c)).ToList();
		return Response;
	}
}
