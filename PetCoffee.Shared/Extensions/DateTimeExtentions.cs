
namespace PetCoffee.Shared.Extensions;

public static class DateTimeExtentions
{
    public static long GetTimeStamp(this DateTime date)
    {
        return (long)(date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
    }
}
