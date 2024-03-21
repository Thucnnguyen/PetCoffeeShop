using MediatR;
using PetCoffee.Application.Features.Auth.Models;

namespace PetCoffee.Application.Features.Auth.Commands;

public class UpdatePasswordCommand : IRequest<AccountResponse>
{
	public string CurrentPassword { get; set; }
	public string NewPassword { get; set; }
}
