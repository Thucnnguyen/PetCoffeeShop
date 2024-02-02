
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Infrastructure.Settings;

public class FirebaseSettings
{
	public static string ConfigSection = "Fcm";

	[Required]
	public string ProjectId { get; set; } = default!;

	[Required]
	public string PrivateKey { get; set; } = default!;

	[Required]
	public string ClientEmail { get; set; } = default!;

	[Required]
	public string TokenUri { get; set; } = default!;
}
