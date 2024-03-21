

using MediatR;
using PetCoffee.Application.Features.Packages.Models;

namespace PetCoffee.Application.Features.Packages.Commands;

public class CreatePackageCommand : IRequest<PackageResponse>
{
	public string? Description { get; set; }
	public int Duration { get; set; }
	public decimal PromotionAmount { get; set; }
	public decimal? PromotionDiscount { get; set; }
}
