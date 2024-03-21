

using MediatR;

namespace PetCoffee.Application.Features.Packages.Commands;

public class DeletePackageCommand : IRequest<bool>
{
	public long Id { get; set; }
}
