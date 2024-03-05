using MediatR;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Features.Report.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Report.Queries
{
    public class GetAllReportSpeicificPostQuery : IRequest<IList<ReportResponse>>
    {
        
        public long postId { get; set; }



    }
}
