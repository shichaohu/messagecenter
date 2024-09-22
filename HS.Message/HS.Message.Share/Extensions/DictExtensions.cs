namespace HS.Message.Share.Extensions;

public static class DictExtensions
{
    public static T? GetValue<T>(this Dictionary<string, T> dic, string key)
    {
        if (!dic.ContainsKey(key)) return default;

        return dic[key];
    }

    public static Guid? Id(this Dictionary<string, object> dic, string key)
    {
        if (!dic.ContainsKey(key)) return null;

        return Guid.Parse(dic[key].ToString());
    }

    public static string Str(this Dictionary<string, object> dic, string key)
    {
        if (!dic.ContainsKey(key)) return null;

        return dic[key].ToString();
    }

    public static int? Int(this Dictionary<string, object> dic, string key)
    {
        if (!dic.ContainsKey(key)) return null;

        bool ok = int.TryParse(dic[key].ToString(), out int num);
        return ok ? num : null;
    }
    public static decimal? Decimal(this Dictionary<string, object> dic, string key)
    {
        if (!dic.ContainsKey(key)) return null;

        bool ok = decimal.TryParse(dic[key].ToString(), out decimal num);
        return ok ? num : null;
    }
}