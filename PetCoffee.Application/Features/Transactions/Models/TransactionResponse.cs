﻿using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Transactions.Models
{
    public class TransactionResponse : BaseAuditableEntityResponse
    {

        public long Id { get; set; }

        public long WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public double Amount { get; set; }
        public long? RemitterId { get; set; }
        public Wallet? Remitter { get; set; }

        public long? ReservationId { get; set; }
        //public Reservation? Reservation { get; set; }
        public string? Content { get; set; }

        //// for donate
        //[InverseProperty(nameof(TransactionItem.Transaction))]
        //public IList<TransactionItem> Items { get; set; } = new List<TransactionItem>();

        public TransactionStatus TransactionStatus { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
