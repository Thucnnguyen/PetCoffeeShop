﻿using FluentValidation;
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
    public class UpdateReportStatusCommandValidation : AbstractValidator<UpdateReportStatusCommand>
    {
        public UpdateReportStatusCommandValidation()
        {
            RuleFor(model => model.Status)
                .IsInEnum()
                .NotNull();
        }
    }
    public class UpdateReportStatusCommand : IRequest<bool>
    {
        
        public long Id { get; set; }

        public ReportStatus Status { get; set; }

   
    }
}
