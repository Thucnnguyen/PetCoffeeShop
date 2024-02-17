﻿

namespace PetCoffee.Application.Common.Models.Response
{
    public class BaseAuditableEntityResponse
    {
        public DateTime CreatedAt { get; set; }

        public long? CreatedById { get; set; }
        public DateTime UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }

        public bool Deleted => DeletedAt != null;
    }
}
