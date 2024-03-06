﻿using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Models
{
    public class ReservationResponse : BaseAuditableEntityResponse
    {
        public long Id { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Discount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }
        public decimal Deposit { get; set; }
        public string Code { get; set; }
        public string? Rate { get; set; }
        public string? Comment { get; set; }

        public long? FloorId { get; set; }
        //public Area? Area { get; set; }
    }
}
