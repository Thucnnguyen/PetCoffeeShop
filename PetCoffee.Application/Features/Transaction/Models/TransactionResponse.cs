using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCoffee.Application.Common.Models.Response;

namespace PetCoffee.Application.Features.Transaction.Models
{
    public class TransactionResponse: BaseAuditableEntityResponse
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
