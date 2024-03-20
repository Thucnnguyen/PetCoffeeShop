using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Product.Models
{
    public class ProductResponse : BaseAuditableEntityResponse
    {

        public long Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public ProductStatus ProductStatus { get; set; }



    }
}
