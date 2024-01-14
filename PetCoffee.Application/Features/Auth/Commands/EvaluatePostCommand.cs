
using FluentValidation;
using MediatR;
using OpenAI_API.Completions;

namespace PetCoffee.Application.Features.Auth.Commands;
public class EvaluatePostCommandValidation : AbstractValidator<EvaluatePostCommand>
{
    public EvaluatePostCommandValidation()
    {
		RuleFor(model => model.Prompt).NotEmpty();
	}
}
public class EvaluatePostCommand: IRequest<bool>
{
	public string Prompt { get; set; }
}
