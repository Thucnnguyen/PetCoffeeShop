using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.PetCfShop.Commands;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Features.PostCategory.Queries;
using PetCoffee.Application.Features.Report.Commands;

namespace PetCoffee.API.Controllers
{
    [Route("/api/v1")]
    [ApiController]
    public class ReportController : ApiControllerBase
    {
        [HttpPost("/posts/{postId}/report")]
        [Authorize]
        public async Task<ActionResult<bool>> CreateReportPostByCurrentAccount(
      [FromRoute] long postId, [FromBody] CreateReportPostCommand request)
        {

            request.postId = postId;
            var response = await Mediator.Send(request);
            return response;
        }

        // view report of specific post  - role admin
        [HttpGet("/posts/{postId}/report")]
        [Authorize]
        public async Task<ActionResult<IList<PostCategoryResponse>>> Get([FromQuery] GetAllCategoriesQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

    }
}
