
using MediatR;
using PetCoffee.Application.Common.Models.Response;

namespace PetCoffee.Application.Features.Pet.Queries;

public class CheckTaxCodeQuery : IRequest<TaxCodeResponse>
{
	public string TaxCode { get; set; }
}
