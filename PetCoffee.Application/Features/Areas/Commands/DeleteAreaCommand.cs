using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Areas.Commands
{
    public class DeleteAreaCommand : IRequest<bool>
    {
        public long AreaId { get; set; }
    }
}
    