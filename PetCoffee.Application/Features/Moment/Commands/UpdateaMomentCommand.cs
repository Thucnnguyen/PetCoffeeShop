
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Moment.Commands;

public class UpdateaMomentCommand : IRequest<MomentResponse>
{
	public string? Content { get; set; }
	public string? ImageUrl { get; set; }
	public IList<IFormFile>? NewImages { get; set; }
	public MomentType MomentType { get; set; }
	public bool IsPublic { get; set; } = true;
}
