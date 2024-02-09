
using System.Security.Claims;

namespace PetCoffee.Application.Service;

public interface ICurrentPrincipalService
{
	public string? CurrentPrincipal { get; }

	public long? CurrentSubjectId { get; }

}
