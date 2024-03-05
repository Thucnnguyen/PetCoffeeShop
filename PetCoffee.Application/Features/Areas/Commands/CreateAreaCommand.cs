using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Areas.Models;


namespace PetCoffee.Application.Features.Areas.Commands
{
    public class CreateAreatValidation : AbstractValidator<CreateAreaCommand>
    {
        public CreateAreatValidation()
        {
            RuleFor(model => model.TotalSeat).NotEmpty();
            RuleFor(command => command)
               .Custom((command, context) =>
               {
                   if (command.Description == null && command.Image == null)
                   {
                       context.AddFailure("Có ít nhất một nội dung hoặc ảnh");
                   }
               });
        }
    }
    public class CreateAreaCommand : IRequest<AreaResponse>
    {
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public int TotalSeat { get; set; }
        public int Order { get; set; }
        public long PetcoffeeShopId { get; set; }
    }
}
