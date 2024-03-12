

using MediatR;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Report.Commands;

public class UpdateReportStatuscommand : IRequest<bool>
{
	public long ReportId { get; set; }
	public ReportStatus Status { get; set; }
}
