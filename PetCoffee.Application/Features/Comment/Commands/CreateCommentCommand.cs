
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Comment.Models;

namespace PetCoffee.Application.Features.Comment.Commands;

public class CreateCommentValidation : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentValidation()
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

public class CreateCommentCommand : IRequest<CommentResponse>
{
	public string? Content { get; set; }
	public IFormFile? Image { get; set; }
	public long PostId { get; set; }
	public long? ParentCommentId { get; set; }
}
