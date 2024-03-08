
using MediatR;
using PetCoffee.Application.Features.Items.Models;

namespace PetCoffee.Application.Features.Items.Queries;

public class GetAllItemInWalletQuery : IRequest<List<ItemWalletResponse>>
{
}
