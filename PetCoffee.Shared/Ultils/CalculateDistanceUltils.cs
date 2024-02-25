

namespace PetCoffee.Shared.Ultils;

public class CalculateDistanceUltils
{
	private static  double RADIUS = 6378.16;
	private static  double PI = Math.PI / 180;

	public static double CalculateDistance(double userLatitude, double userLongitude, double ShopLatitude, double ShopLongitude)
	{
		double dlon = Radians(ShopLongitude - userLongitude);
		double dlat = Radians(ShopLatitude - userLatitude);

		double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(userLatitude)) * Math.Cos(Radians(ShopLatitude)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
		double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		return angle * RADIUS;
	}

	private static double Radians(double x)
	{
		return x * PI / 180;
	}
}
