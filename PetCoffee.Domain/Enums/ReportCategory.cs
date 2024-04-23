
using System.ComponentModel;

namespace PetCoffee.Domain.Enums;

public enum ReportCategory
{
	[Description("Violence")]
	Violence = 0,

	[Description("NudePhotos")]
	NudePhotos = 1,

	[Description("Suicidal")]
	Suicidal = 2,

	[Description("AppearTooMuch")]
	AppearTooMuch = 3,

	[Description("TooPrivate")]
	TooPrivate = 4,

	[Description("SensitiveContent")]
	SensitiveContent = 5,

	[Description("NotSuitable")]
	NotSuitable = 6,

	[Description("Other")]
	other = 7,
}
