using FluentValidation;
using MediatR;
using PetCoffee.Application.Features.PostCategory.Models;

namespace PetCoffee.Application.Features.PostCategory.Commands;

public class UpdatePostCategoryValidation : AbstractValidator<UpdatePostCategoryCommand>
{
    public UpdatePostCategoryValidation()
    {
        RuleFor(model => model.Id).NotEmpty();
        RuleFor(model => model.Name).NotEmpty();
    }
}

public class UpdatePostCategoryCommand : IRequest<PostCategoryResponse>
{
	public long Id { get; set; }
	public string Name { get; set; }
}
