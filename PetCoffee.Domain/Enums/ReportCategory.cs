
using System.ComponentModel;

namespace PetCoffee.Domain.Enums;

public enum ReportCategory
{
	[Description("violence")]
	violence = 0,

	[Description("nude photos")]
	nudePhotos = 1,

	[Description("suicidal")]
	suicidal = 2,
}
