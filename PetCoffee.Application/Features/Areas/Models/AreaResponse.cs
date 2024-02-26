using PetCoffee.Application.Common.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Areas.Models
{
    public class AreaResponse : BaseAuditableEntityResponse
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int TotalSeat { get; set; }

        public long PetcoffeeShopId { get; set; }
    }
}
