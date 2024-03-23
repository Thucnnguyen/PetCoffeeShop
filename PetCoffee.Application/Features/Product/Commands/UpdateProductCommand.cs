using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Product.Commands
{
   

    public class UpdateProductCommand : IRequest<ProductResponse>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public IList<IFormFile>? Image { get; set; }
    }

  

}
