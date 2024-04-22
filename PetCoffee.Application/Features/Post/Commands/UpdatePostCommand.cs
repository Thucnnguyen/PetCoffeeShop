
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Post.Model;

namespace PetCoffee.Application.Features.Post.Commands;

public class UpdatePostCommand : IRequest<PostResponse>
{
	public long PostId { get; set; }
	public string Content { get; set; }
	public IList<IFormFile>? Image { get; set; }
}
