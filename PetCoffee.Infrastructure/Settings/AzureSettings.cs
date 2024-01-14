
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Infrastructure.Settings;

public class AzureSettings
{
	public static readonly string ConfigSection = "Azure";

	[Required]
	public string KeyContentModerator { get; set; }
	[Required]
	public string UrlConetentModerator { get; set; }
	
	[Required]
	public string KeyTranslator { get; set; }
	[Required]
	public string UrlTranslator { get; set; }
	[Required]
	public string LocationTranslator { get; set; }
	
	[Required]
	public string BlobConnectionString { get; set; }
}
