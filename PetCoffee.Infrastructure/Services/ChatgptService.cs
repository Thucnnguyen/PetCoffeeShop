using OpenAI_API;
using OpenAI_API.Completions;
using PetCoffee.Application.Service;
using PetCoffee.Infrastructure.Common.Constant;

namespace PetCoffee.Infrastructure.Services;

public class ChatgptService : IChatgptService
{
    private readonly IOpenAIAPI _openAIAPI;
    public ChatgptService(IOpenAIAPI openAIAPI)
    {

        _openAIAPI = openAIAPI;
    }
    public async Task<CompletionResult> SendRequest(string request)
    {
        request += PromptConstants.Prompt;

        CompletionRequest completionRequest = new CompletionRequest();
        completionRequest.Model = "gpt-3.5-turbo-instruct";
        completionRequest.MaxTokens = 1500;
        completionRequest.Prompt = request;
        completionRequest.NumChoicesPerPrompt = 1;
        var result = await _openAIAPI.Completions.CreateCompletionAsync(completionRequest);
        return result;
    }
}
