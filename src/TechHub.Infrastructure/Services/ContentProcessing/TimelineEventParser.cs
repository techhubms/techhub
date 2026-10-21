using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TechHub.Core.Logging;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Turns the <c>timeline_events</c> array of the classification response into <see cref="TimelineEvent"/> values.
/// Dates for events that happen "now" are never taken from the model: code uses the article's publication date.
/// </summary>
internal static class TimelineEventParser
{
    private const int MaxEvents = 10;

    private static readonly TimeZoneInfo _brusselsTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");

    /// <summary>Publication date of the article in Europe/Brussels.</summary>
    public static DateOnly GetBrusselsDate(DateTimeOffset publishedAt)
        => DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(publishedAt, _brusselsTimeZone).DateTime);

    /// <summary>
    /// Parses the events. Invalid events are skipped and logged; they never fail the article.
    /// </summary>
    public static IReadOnlyList<TimelineEvent> Parse(JsonElement eventsArray, DateOnly publicationDate, ILogger logger, string url)
    {
        if (eventsArray.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var events = new List<TimelineEvent>();
        var seen = new HashSet<(string Kind, string Product, string Feature)>();

        foreach (var element in eventsArray.EnumerateArray().Take(MaxEvents))
        {
            var timelineEvent = ParseEvent(element, publicationDate, logger, url);
            if (timelineEvent is null)
            {
                continue;
            }

            var key = (timelineEvent.Kind, timelineEvent.Product.ToUpperInvariant(), (timelineEvent.Feature ?? string.Empty).ToUpperInvariant());
            if (seen.Add(key))
            {
                events.Add(timelineEvent);
            }
        }

        return events;
    }

    private static TimelineEvent? ParseEvent(JsonElement element, DateOnly publicationDate, ILogger logger, string url)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        var kind = GetString(element, "kind");
        var product = GetString(element, "product");
        var description = GetString(element, "description");
        var dateSource = GetString(element, "date_source");

        if (kind is null || !TimelineEvent.ValidKinds.Contains(kind))
        {
            logger.LogWarning("Skipping timeline event with unknown kind '{Kind}' for {Url}", kind.Sanitize(), url.Sanitize());
            return null;
        }

        if (product is null || description is null)
        {
            logger.LogWarning("Skipping timeline event without product or description for {Url}", url.Sanitize());
            return null;
        }

        DateOnly date;
        string precision;

        if (dateSource is TimelineEvent.DateSourcePublication)
        {
            date = publicationDate;
            precision = "day";
        }
        else if (dateSource is TimelineEvent.DateSourceExplicit)
        {
            precision = GetString(element, "date_precision") ?? "day";
            if (!TimelineEvent.ValidPrecisions.Contains(precision))
            {
                logger.LogWarning("Skipping timeline event with unknown date precision '{Precision}' for {Url}", precision.Sanitize(), url.Sanitize());
                return null;
            }

            var dateText = GetString(element, "date");
            if (dateText is null || !DateOnly.TryParseExact(dateText, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                logger.LogWarning("Skipping timeline event with unparseable date '{Date}' for {Url}", dateText.Sanitize(), url.Sanitize());
                return null;
            }

            date = Normalize(date, precision);
        }
        else
        {
            logger.LogWarning("Skipping timeline event with unknown date source '{DateSource}' for {Url}", dateSource.Sanitize(), url.Sanitize());
            return null;
        }

        return new TimelineEvent
        {
            Kind = kind,
            Product = product,
            Feature = GetString(element, "feature"),
            Description = description,
            Date = date,
            DatePrecision = precision,
            DateSource = dateSource,
            Quote = GetString(element, "quote")
        };
    }

    private static DateOnly Normalize(DateOnly date, string precision) => precision switch
    {
        "month" => new DateOnly(date.Year, date.Month, 1),
        "quarter" => new DateOnly(date.Year, ((date.Month - 1) / 3 * 3) + 1, 1),
        _ => date
    };

    private static string? GetString(JsonElement element, string name)
    {
        if (element.TryGetProperty(name, out var property) && property.ValueKind == JsonValueKind.String)
        {
            var value = property.GetString();
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        return null;
    }
}
