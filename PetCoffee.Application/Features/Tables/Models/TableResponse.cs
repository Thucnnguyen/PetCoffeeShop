using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Tables.Models
{
    public class TableResponse : BaseAuditableEntityResponse
    {
        public long Id { get; set; }
        public string? Image { get; set; }
        public TableType Type { get; set; }

        public long AreaId { get; set; }
        //public Area Area { get; set; }

        public double PricePerHour { get; set; }
    }
}
