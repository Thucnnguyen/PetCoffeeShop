using Microsoft.AspNetCore.Http;

namespace PetCoffee.Application.Service;

public interface IAzureService
{
    public Task<bool> HasBadWords(string content);
    public Task<string> Translate(string content, string to);
    public Task<string> CreateBlob(string name, IFormFile file);
    public Task<string> GetBlob(string name);
	public Task<string> UpdateloadImages(IList<IFormFile>? images);

	public Task<bool> SendEmail(string to, string content,string subject);
}
