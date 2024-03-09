namespace PetCoffee.Infrastructure.Settings;

public class ZaloPaySettings
{
	public static readonly string ConfigSection = "ZaloPay";

	public string AppUser { get; set; }
	public string PaymentUrl { get; set; }
	public string RedirectUrl { get; set; }
	public string CallBackUrl { get; set; }
	public string HashSecret { get; set; }
	public string IpUrl { get; set; }
	public int AppId { get; set; }
	public string Key1 { get; set; }
	public string Key2 { get; set; }
}
