

using System.ComponentModel;

namespace PetCoffee.Domain.Enums;

public enum TypeSpecies 
{
	//cat
	[Description("Mèo Ba Tư")]
	Persian,
	[Description("Mèo Xiêm")]
	Siamese,
	[Description("Mèo Maine Coon")]
	MaineCoon,
	[Description("Mèo Sphynx")]
	Sphynx,
	[Description("Mèo Anh lông ngắn")]
	BritishShorthair,
	[Description("Mèo Abyssinian")]
	Abyssinian,
	[Description("Mèo Tai Cụp")]
	ScottishFold,

	//dog
	[Description("Chó Golden Retriever")]
	GoldenRetriever,
	[Description("Chó săn cừu Đức")]
	GermanShepherd,
	[Description("Chó Bulldog Pháp")]
	FrenchBulldog,
	[Description("Chó Poodle")]
	Poodle,
	[Description("Chó Labrador Retriever")]
	LabradorRetriever,
	[Description("Chó Beagle")]
	Beagle,
	[Description("Chó Dachshund")]
	Dachshund,

	//other
	[Description("Loài khác")]
	Others,

}
