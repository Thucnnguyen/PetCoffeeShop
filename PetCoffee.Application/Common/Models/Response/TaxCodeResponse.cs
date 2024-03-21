
namespace PetCoffee.Application.Common.Models.Response;

public class TaxCodeResponse
{
    public string Code { get; set; }
    public string Desc { get; set; }
    public Data Data { get; set; }
}

public class Data
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}