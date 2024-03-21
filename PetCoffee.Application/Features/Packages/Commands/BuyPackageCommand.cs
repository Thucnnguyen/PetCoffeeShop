
using MediatR;

namespace PetCoffee.Application.Features.Packages.Commands;

public class BuyPackageCommand : IRequest<bool>
{
	public long ShopId { get; set; }
	public long PackageId { get; set; }
}
