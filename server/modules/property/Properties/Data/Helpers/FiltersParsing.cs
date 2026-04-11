using System.Globalization;

namespace Properties.Data.Helpers;
public static class FilterParsing
{
    public static bool TryParseEnumIgnoreCase<TEnum>(string? value, out TEnum result)
        where TEnum : struct, Enum
        => Enum.TryParse(value, ignoreCase: true, out result);

    // Supports "3" or "3+"
    public static bool TryParseMinInt(string? value, out int? min)
    {
        min = null;
        if (string.IsNullOrWhiteSpace(value)) return true;

        var trimmed = value.Trim();
        var plus = trimmed.EndsWith("+");
        var numberPart = plus ? trimmed[..^1] : trimmed;

        if (!int.TryParse(numberPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n))
            return false;

        min = n;
        return true;
    }

    // Supports "1" or "1.5" or "2+"
    public static bool TryParseMinDouble(string? value, out double? min)
    {
        min = null;
        if (string.IsNullOrWhiteSpace(value)) return true;

        var trimmed = value.Trim();
        var plus = trimmed.EndsWith("+");
        var numberPart = plus ? trimmed[..^1] : trimmed;

        if (!double.TryParse(numberPart, NumberStyles.Float, CultureInfo.InvariantCulture, out var n))
            return false;

        min = n;
        return true;
    }

    public static bool TryParseDate(string? value, out DateTime? date)
    {
        date = null;
        if (string.IsNullOrWhiteSpace(value)) return true;

        if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dt))
            return false;

        date = dt.ToUniversalTime();
        return true;
    }
}