

using MediatR;
using PetCoffee.Application.Features.Report.Models;

namespace PetCoffee.Application.Features.Report.Queries;

public class GetReportByIdQuery : IRequest<ReportResponse>
{
	public long Id { get; set; }
}
