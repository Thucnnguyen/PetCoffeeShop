
using MediatR;

namespace PetCoffee.Application.Features.Auth.Commands;

public class CreateAccountStaffPlaformCommand : IRequest<bool>
{
	public string FullName { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
}
