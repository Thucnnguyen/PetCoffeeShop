

using MediatR;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Areas.Queries;

public class GetAreaByIdQuery : IRequest<AreaResponse>
{
	public long AreaId { get; set; }

	private string? _search;

	public string? Search
	{
		get => _search;
		set => _search = value?.Trim().ToLower();
	}
}
