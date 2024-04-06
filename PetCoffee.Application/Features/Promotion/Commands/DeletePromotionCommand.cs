using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Promotion.Commands
{
	public class DeletePromotionCommand : IRequest<bool>
	{
		public long Id { get; set; }
	}
	
}
