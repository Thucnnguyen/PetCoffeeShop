

using MediatR;
using PetCoffee.Application.Features.Packages.Models;

namespace PetCoffee.Application.Features.Packages.Queries;

public class GetPackageByIdQuery : IRequest<PackageResponse>
{
	public long Id { get; set; }
}
