using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Enums;

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
