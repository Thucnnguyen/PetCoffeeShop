using Microsoft.Azure.CognitiveServices.ContentModerator;
using PetCoffee.Application.Service;
using PetCoffee.Infrastructure.Settings;
using System.Text;
using Newtonsoft.Json;
using PetCoffee.Infrastructure.Common.Constant;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using OpenAI_API.Moderation;
using System.Net;

namespace PetCoffee.Infrastructure.Services;

public class AzureService : IAzureService
{
	private readonly AzureSettings _settings;
	private readonly IContentModeratorClient _clientModerator;
	private readonly BlobServiceClient _blobClient;

	public AzureService(AzureSettings languageAISettings, IContentModeratorClient contentModeratorClient, BlobServiceClient blobServiceClient)
	{
		_settings = languageAISettings;
		_clientModerator = contentModeratorClient;
		_blobClient = blobServiceClient;
	}

	public async Task<string> CreateBlob(string name, IFormFile file)
	{
		BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(AzureConstant.ContainerName);
		var blobClient = blobContainerClient.GetBlobClient(name);

		var httpHeaders = new BlobHttpHeaders()
		{
			ContentType = file.ContentType,
		};
		var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
		return result.ToString();
	}

	public async Task<string> GetBlob(string name)
	{
		BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(AzureConstant.ContainerName);
		var blobClient = blobContainerClient.GetBlobClient(name);
		return blobClient.Uri.AbsoluteUri;
	}

	public async Task<bool> HasBadWords(string content)
	{
		
		//convert text to byte
		byte[] textBytes = Encoding.UTF8.GetBytes(content);
		MemoryStream stream = new MemoryStream(textBytes);

		//send request
		var result = _clientModerator.TextModeration.ScreenText("text/plain", stream, "eng", true, true, null, true);
		if(result.Terms == null || result.Terms.Count() == 0)
		{
			return false;
		}
		return true;
	}

	public async Task<bool> SendEmail(string to, string content, string subject)
	{
		
		var JsonData = JsonConvert.SerializeObject(new
		{
			to = to,
			content = content,
			subject = subject
		});
		string result;
		using (var client = new HttpClient())
		using (var request = new HttpRequestMessage())
		{
			// Build the request.
			request.Method = HttpMethod.Post;
			request.RequestUri = new Uri(_settings.LogicAppUrl);
			request.Content = new StringContent(JsonData, Encoding.UTF8, "application/json");

			// Send the request and get response.
			HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(true);
			// Read response as a string.
			var statusCode = response.StatusCode;
			if (statusCode == HttpStatusCode.Accepted)
			{
				return true;
			}
		}
		return false;
	}

	public async Task<string> Translate(string content, string to)
	{

		object[] body = new object[] { new { Text = content } };
		var requestBody = JsonConvert.SerializeObject(body);
		string result;
		using (var client = new HttpClient())
		using (var request = new HttpRequestMessage())
		{
			// Build the request.
			request.Method = HttpMethod.Post;
			request.RequestUri = new Uri(_settings.UrlTranslator + UrlConstant.Url+to);
			request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
			request.Headers.Add("Ocp-Apim-Subscription-Key", _settings.KeyTranslator);
			request.Headers.Add("Ocp-Apim-Subscription-Region", _settings.LocationTranslator);

			// Send the request and get response.
			HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(true);
			// Read response as a string.
			result = await response.Content.ReadAsStringAsync();
			dynamic jsonResponse = JsonConvert.DeserializeObject(result);
			result = jsonResponse[0].translations[0].text.ToString();

		}
		return result;
	}
}
