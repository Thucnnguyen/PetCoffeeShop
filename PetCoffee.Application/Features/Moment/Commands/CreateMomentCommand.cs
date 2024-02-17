using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Memory.Commands;

public class CreatememoryValidation : AbstractValidator<CreateMomentCommand>
{
	public CreatememoryValidation()
	{
		RuleFor(command => command)
		   .Custom((command, context) =>
		   {
			   if (command.Content == null && command.Image == null)
			   {
				   context.AddFailure("Có ít một nhất nội dung hoặc ảnh");
			   }
		   });
	}
}

public class CreateMomentCommand : IRequest<MomentResponse>
{
	public string? Content { get; set; }
	public IList<IFormFile>? Image { get; set; }
	public MomentType MomentType { get; set; }
	public bool IsPublic { get; set; } = true;

	public long PetId { get; set; }
}
