﻿using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Domain.Entities
{
    [Table("Product")]
    public class Product: BaseAuditableEntity
    {

        [Key]
        public long Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }

        public ProductStatus ProductStatus { get; set; } = ProductStatus.Active;

        public string Image { get; set; }



        [InverseProperty(nameof(InvoiceProduct.Product))]
        public IList<InvoiceProduct> Invoices  { get; set; } = new List<InvoiceProduct>();



        public long PetCoffeeShopId { get; set; }
        public PetCoffeeShop PetCoffeeShop { get; set; }




    }
}
