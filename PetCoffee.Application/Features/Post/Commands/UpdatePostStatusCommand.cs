using FluentValidation;
using MediatR;
//using Newtonsoft.Json;
using PetCoffee.Domain.Enums;

using System.Text.Json.Serialization;


namespace PetCoffee.Application.Features.Post.Commands
{
    public class UpdateCustomerStatusCommandValidation : AbstractValidator<UpdatePostStatusCommand>
    {
        public UpdateCustomerStatusCommandValidation()
        {
            RuleFor(model => model.Status)
                .IsInEnum()
                .NotNull();
        }
    }
    public class UpdatePostStatusCommand : IRequest<bool>
    {
        [JsonIgnore]
        public long Id { get; set; }

        public PostStatus Status { get; set; }
    }
}
