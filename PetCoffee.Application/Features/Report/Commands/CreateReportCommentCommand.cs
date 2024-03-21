using FluentValidation;
using MediatR;
using PetCoffee.Domain.Enums;
using System.Text.Json.Serialization;

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
