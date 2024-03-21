

using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Pet.Queries;

public class GetPetsByAreaIdQuery : PaginationRequest<Domain.Entities.Pet>, IRequest<PaginationResponse<Domain.Entities.Pet, PetResponse>>
{
	public long AreaId { get; set; }
	public TypeSpecies? TypeSpecies { get; set; }
	public PetType? PetType { get; set; }

	private string? _search;

	public string? Search
	{
		get => _search;
		set => _search = value?.Trim().ToLower();
	}
	public override Expression<Func<Domain.Entities.Pet, bool>> GetExpressions()
	{
		if (Search is not null)
		{
			Expression = Expression.And(p => p.Name != null && p.Name.ToLower().Contains(Search));
		}

		if (TypeSpecies is not null)
		{
			Expression = Expression.And(p => (p.TypeSpecies == TypeSpecies));
		}

		if (PetType is not null)
		{
			Expression = Expression.And(p => p.PetType == PetType);
		}

		Expression = Expression.And(p => p.PetAreas.Any(pa => pa.AreaId == AreaId && pa.EndTime == null) && !p.Deleted);


		return Expression;
	}
}
