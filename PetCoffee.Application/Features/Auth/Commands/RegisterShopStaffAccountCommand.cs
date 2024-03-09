
using MediatR;

namespace PetCoffee.Application.Features.Auth.Commands;

public class RegisterShopStaffAccountCommand : IRequest<bool>
{
	public long ShopId { get; set; }
	public string FullName { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
}
