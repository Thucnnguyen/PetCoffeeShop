using FluentValidation;
using MediatR;
using PetCoffee.Domain.Enums;
using System.Text.Json.Serialization;

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
