using MediatR;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class EvalutePostHandler : IRequestHandler<EvaluatePostCommand, bool>
{
	private readonly IChatgptService _chatgptService;
	private readonly IAzureService _auzeService;
	public EvalutePostHandler(IChatgptService chatgptService, IAzureService auzeService)
	{
		_chatgptService = chatgptService;
		_auzeService = auzeService;
	}
	public async Task<bool> Handle(EvaluatePostCommand request, CancellationToken cancellationToken)
	{
		var conent = await _auzeService.Translate(request.Prompt, "en");
        var check = await _auzeService.HasBadWords(conent);
        return check;
	}
}
