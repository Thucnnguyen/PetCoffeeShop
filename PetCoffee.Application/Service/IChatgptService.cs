
using OpenAI_API.Completions;

namespace PetCoffee.Application.Service;

public interface IChatgptService
{
    public Task<CompletionResult> SendRequest(string request);
}
