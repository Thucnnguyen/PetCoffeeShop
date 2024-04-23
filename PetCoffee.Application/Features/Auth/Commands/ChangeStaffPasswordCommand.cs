

using MediatR;

namespace PetCoffee.Application.Features.Auth.Commands;

public class ChangeStaffPasswordCommand : IRequest<bool>
{
	public long Id { get; set; }
	public string NewPassword { get; set; }
}
