using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Domain.Entities
{
    public class InvoiceProduct
    {
      
        public long InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    
        public long ProductId { get; set; }
        public Product Product { get; set; }

        public int TotalProduct { get; set; }




    }
}
