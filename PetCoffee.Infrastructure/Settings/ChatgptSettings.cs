
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Infrastructure.Settings;

public class ChatgptSettings
{
	public static readonly string ConfigSection = "Chatgpt";

	[Required]
	public string Key { get; set; }
	[Required]
	public string Url { get; set; }
}
