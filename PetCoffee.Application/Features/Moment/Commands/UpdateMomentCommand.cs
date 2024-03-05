
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Moment.Commands;

public class UpdateMomentCommand : IRequest<MomentResponse>
{
	public long Id { get; set; }
	public string? Content { get; set; }
	public IList<IFormFile>? NewImages { get; set; }
	public MomentType? MomentType { get; set; }
	public bool? IsPublic { get; set; }
}
