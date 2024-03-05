using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Report.Commands
{

    public class CreateReportPostCommandValidation : AbstractValidator<CreateReportPostCommand>
    {
        public CreateReportPostCommandValidation()
        {
            RuleFor(model => model.ReportCategory)
                .IsInEnum()
                .NotNull();
        }
    }
    public class CreateReportPostCommand : IRequest<bool>
    {
        [JsonIgnore]
        public long postId { get; set; }

        public ReportCategory ReportCategory { get; set; }
    }
}
