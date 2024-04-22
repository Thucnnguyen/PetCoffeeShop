
using System.ComponentModel;

namespace PetCoffee.Domain.Enums;

public enum ReportCategory
{
	[Description("violence")]
	Violence = 0,

	[Description("nude photos")]
	NudePhotos = 1,

	[Description("suicidal")]
	Suicidal = 2,

	[Description("Appear Too Much")]
	AppearTooMuch = 3,

	[Description("Too private")]
	TooPrivate = 4,

	[Description("Sensitive content")]
	SensitiveContent = 5,

	[Description("Not Suitable")]
	NotSuitable = 6,

	[Description("Other")]
	other = 7,
}
