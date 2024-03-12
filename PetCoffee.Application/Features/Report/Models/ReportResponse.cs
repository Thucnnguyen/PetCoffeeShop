using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Report.Models
{
    public class ReportResponse: BaseAuditableEntityResponse
    {
        public long Id { get; set; }
        public long? CommentId { get; set; }
        public long? PostID { get; set; }
        public string? Reason { get; set; }
		public ReportStatus Status { get; set; }
	}
}
