using MediatR;
using PetCoffee.Application.Features.Memory.Models;

namespace PetCoffee.Application.Features.Moment.Queries;

public class GetMomentByPetIdQuery : IRequest<IList<MomentResponse>>
{
    public long Id { get; set; }
}
