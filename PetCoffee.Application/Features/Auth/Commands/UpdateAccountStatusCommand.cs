

using MediatR;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Auth.Commands;

public class UpdateAccountStatusCommand : IRequest<bool>
{
	public long Id { get; set; }
	public AccountStatus AccountStatus { get; set; }
}
