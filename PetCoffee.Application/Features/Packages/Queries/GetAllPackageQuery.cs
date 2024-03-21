

using MediatR;
using PetCoffee.Application.Features.Packages.Models;

namespace PetCoffee.Application.Features.Packages.Queries;

public class GetAllPackageQuery : IRequest<List<PackageResponse>>
{
}
