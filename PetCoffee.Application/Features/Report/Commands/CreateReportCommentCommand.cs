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
				.NotNull();
		}
	}
	public class CreateReportCommentCommand : IRequest<bool>
	{
		[JsonIgnore]
		public long CommentID { get; set; }

		public ReportCategory ReportCategory { get; set; }
	}
}
