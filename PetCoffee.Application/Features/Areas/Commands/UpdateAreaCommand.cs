using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Areas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Areas.Commands
{
    public class UpdateAreaValidation : AbstractValidator<UpdateAreaCommand>
    {
        public UpdateAreaValidation()
        {
            RuleFor(model => model.TotalSeat).NotEmpty();
            RuleFor(command => command)
               .Custom((command, context) =>
               {
                   if (command.Description == null && command.Image == null)
                   {
                       context.AddFailure("Có ít một nhất nội dung hoặc ảnh");
                   }
               });
        }
    }
    public class UpdateAreaCommand : IRequest<AreaResponse>
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public int TotalSeat { get; set; }
        public int Order { get; set; }
    }
}
