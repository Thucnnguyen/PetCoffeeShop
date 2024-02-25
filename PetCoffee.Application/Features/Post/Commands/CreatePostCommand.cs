
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Post.Command;

public class CreatePostCommand : IRequest<PostResponse>
{
	public string Content { get; set; }
	public IList<IFormFile>? Image {  get; set; }
	public IList<long>? CategoryIds { get; set; }
	public IList<long>? PetCafeShopIds { get; set; }
}
