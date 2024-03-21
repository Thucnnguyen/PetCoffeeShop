using MediatR;
using PetCoffee.Application.Features.Report.Models;

namespace PetCoffee.Application.Features.Report.Queries
{
	public class GetAllReportSpeicificPostQuery : IRequest<IList<ReportResponse>>
	{
		public long postId { get; set; }
	}
}
