using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Domain.Entities
{
    public class PetCoffeeProduct
    {

        public long PetCoffeeShopId { get; set; }
        public long ProductId { get; set; }

        public Product Product { get; set; }
        public PetCoffeeShop PetCoffeeShop { get; set; }
    }
}
