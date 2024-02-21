
using MediatR;

namespace PetCoffee.Application.Features.Auth.Queries;

public class CheckEmailExistQuery : IRequest<bool>
{
	public string Email { get; set; }
}
