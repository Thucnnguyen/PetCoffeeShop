using MediatR;
using PetCoffee.Application.Features.Auth.Models;

namespace PetCoffee.Application.Features.Auth.Queries;

public class GetCurrentAccountInfomationQuery : IRequest<AccountResponse>
{
}
