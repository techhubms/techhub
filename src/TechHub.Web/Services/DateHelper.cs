namespace TechHub.Web.Services;

/// <summary>
/// Helper for formatting Unix epoch timestamps as relative or absolute date strings,
/// using the Europe/Brussels timezone.
/// </summary>
public static class DateHelper
{
    private static readonly TimeZoneInfo _brusselsTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");

    /// <summary>
    /// Formats a Unix epoch timestamp as a relative date string:
    /// "Today", "Yesterday", "X days ago", "X weeks ago", or "MMM d, yyyy".
    /// </summary>
    public static string FormatDateRelative(long epochSeconds)
    {
        var dateTime = DateTimeOffset.FromUnixTimeSeconds(epochSeconds);
        var brusselsDate = TimeZoneInfo.ConvertTime(dateTime, _brusselsTimeZone).Date;
        var todayBrussels = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _brusselsTimeZone).Date;
        var daysDiff = (todayBrussels - brusselsDate).Days;

        if (daysDiff == 0)
        {
            return "Today";
        }

        if (daysDiff == 1)
        {
            return "Yesterday";
        }

        if (daysDiff < 7)
        {
            return $"{daysDiff} days ago";
        }

        if (daysDiff < 30)
        {
            return $"{daysDiff / 7} weeks ago";
        }

        return dateTime.ToString("MMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Formats a Unix epoch timestamp as a full date string, e.g. "March 23, 2026".
    /// </summary>
    public static string FormatDateFull(long epochSeconds) =>
        DateTimeOffset.FromUnixTimeSeconds(epochSeconds).ToString("MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>
    /// Formats a Unix epoch timestamp as an ISO date string, e.g. "2026-03-23".
    /// </summary>
    public static string FormatDateIso(long epochSeconds) =>
        DateTimeOffset.FromUnixTimeSeconds(epochSeconds).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>
    /// Formats a <see cref="DateTimeOffset"/> as "yyyy-MM-dd HH:mm:ss" in Europe/Brussels timezone.
    /// </summary>
    public static string FormatTimestamp(DateTimeOffset timestamp) =>
        TimeZoneInfo.ConvertTime(timestamp, _brusselsTimeZone).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
}
