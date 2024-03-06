using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Report.Models
{
    public class ReportResponse: BaseAuditableEntityResponse
    {
        public long Id { get; set; }
        public long? CommentId { get; set; }
        public long? PostID { get; set; }
        public string? Reason { get; set; }
        public ReportStatus Status { get; set; }
        public ReportCategory ReportCategory { get; set; }
    }
}
