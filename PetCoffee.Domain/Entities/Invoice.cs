using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Domain.Entities
{
    [Table("Invoie")]
    public class Invoice : BaseAuditableEntity
    {

        [Key]
        public long Id { get; set; }



        public long? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public decimal TotalAmount { get; set; }


            
        [InverseProperty(nameof(InvoiceProduct.Invoice))]
        public ICollection<InvoiceProduct> Products { get; set; }


        




    }
}
