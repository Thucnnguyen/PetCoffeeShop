using FluentValidation;
using MediatR;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Report.Commands
{
    public class CreateReportCommentCommandValidation : AbstractValidator<CreateReportPostCommand>
    {
        public CreateReportCommentCommandValidation()
        {
            RuleFor(model => model.ReportCategory)
                .IsInEnum()
                .NotNull();
        }
    }
    public class CreateReportCommentCommand : IRequest<bool>
    {
        [JsonIgnore]
        public long Id { get; set; }

        public ReportCategory ReportCategory { get; set; }
    }
}
