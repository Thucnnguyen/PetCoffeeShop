using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Report.Commands;
using PetCoffee.Application.Features.Report.Handlers;
using PetCoffee.Application.Features.Report.Models;
using PetCoffee.Application.Features.Report.Queries;

namespace PetCoffee.API.Controllers
{
    [Route("/api/v1")]
    [ApiController]
    public class ReportController : ApiControllerBase
    {
        [HttpPost("posts/{postId}/reports")]
        [Authorize]
        public async Task<ActionResult<bool>> CreateReportPostByCurrentAccount(
      [FromRoute] long postId, [FromBody] CreateReportPostCommand request)
        {

            request.postId = postId;
            var response = await Mediator.Send(request);
            return response;
        }

        // view report of specific post  - role admin
        [HttpGet("posts/{postId}/report")]
        [Authorize]
        public async Task<ActionResult<IList<ReportResponse>>> Get([FromRoute] GetAllReportSpeicificPostQuery request)
        {

            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("reports")]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<Domain.Entities.Report, ReportResponse>>> GetAll([FromQuery] GetAllReportQuery request)
        {

            var response = await Mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("reports/{id}")]
        [Authorize]
        public async Task<ActionResult<ReportResponse>> GetById([FromRoute] GetReportByIdHandler request)
        {

            var response = await Mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("comments/{Id}/reports")]
        [Authorize]
        public async Task<ActionResult<bool>> CreateReportCommentByCurrentAccount(
    [FromRoute] long Id, [FromBody] CreateReportCommentCommand request)
        {
            request.Id = Id;
            var response = await Mediator.Send(request);
            return response;
        }

        [HttpPut("reports/status")]
        [Authorize]
        public async Task<ActionResult<bool>> UpdateReportStatus([FromBody] UpdateReportStatuscommand request)
        {
            var response = await Mediator.Send(request);
            return response;
        }
    }
}
