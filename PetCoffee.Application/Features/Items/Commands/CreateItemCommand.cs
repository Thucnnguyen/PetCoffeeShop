using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Areas.Commands;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Items.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Items.Commands
{
    public class CreateItemValidation : AbstractValidator<CreateItemCommand>
    {
        public CreateItemValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Price).NotEmpty().GreaterThan(0);
        }
    }
    public class CreateItemCommand : IRequest<ItemResponse>
    {
      
    
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
