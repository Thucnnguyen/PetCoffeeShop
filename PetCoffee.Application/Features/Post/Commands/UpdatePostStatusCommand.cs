using FluentValidation;
using MediatR;
//using Newtonsoft.Json;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
