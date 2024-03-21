

namespace PetCoffee.Infrastructure.Services.Payment.ZaloPay;

public class ZalopayResponse
{
    public int ReturnCode { get; set; }
    public string ReturnMessage { get; set; } = string.Empty;
    public string OrderUrl { get; set; } = string.Empty;
}
