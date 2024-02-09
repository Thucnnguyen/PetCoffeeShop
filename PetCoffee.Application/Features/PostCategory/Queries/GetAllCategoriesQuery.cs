

using MediatR;
using PetCoffee.Application.Features.PostCategory.Models;

namespace PetCoffee.Application.Features.PostCategory.Queries;

public class GetAllCategoriesQuery : IRequest<IList<PostCategoryResponse>>
{
	private string? _search;

	public string? Search
	{
		get => _search;
		set => _search = value?.Trim().ToLower();
	}
}
