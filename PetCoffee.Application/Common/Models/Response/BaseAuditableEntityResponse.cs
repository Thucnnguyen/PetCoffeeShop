using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Common.Models.Response
{
    public class BaseAuditableEntityResponse
    {
        public DateTimeOffset CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public string? CreatedByUsername { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }

        public string? UpdatedByUsername { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }

        public string? DeletedByUsername { get; set; }

        public bool Deleted => DeletedAt != null;
    }
}
