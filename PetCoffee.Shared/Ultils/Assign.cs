namespace TmsApi.Common;

public class Assign
{
    public static void Partial<T, U>(T source, U dest, Func<string, object, bool> ignoreCallback = null)
    {
        foreach (var item in source.GetType().GetProperties())
        {
            var property = dest.GetType().GetProperty(item.Name);
            if (property == null)
            {
                continue;
            }
            if (item.GetValue(source) == null) continue;
            var value = item.GetValue(source, null);
            if (ignoreCallback != null && ignoreCallback(property.Name, value))
            {
                continue;
            }

            property.SetValue(dest, value);
        }
    }
    
}