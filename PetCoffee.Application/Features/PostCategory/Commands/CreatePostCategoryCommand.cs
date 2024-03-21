
using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.PostCategory.Models;

namespace PetCoffee.Application.Features.PostCategory.Commands;


public class CreatePostCategoryValidation : AbstractValidator<CreatePostCategoryCommand>
{
	public CreatePostCategoryValidation()
	{
		RuleFor(model => model.Name).NotEmpty();
	}
}
public class CreatePostCategoryCommand : IRequest<PostCategoryResponse>
{
	public string Name { get; set; }
}
