using System.Globalization;
using System.Text.RegularExpressions;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Safety net for the timeline extraction of the classification step. A deadline (retirement, deprecation, end of support, ...)
/// is the one event a reader cannot afford to miss, and the classification covers many tasks in one answer. When the text
/// names a future date next to a deadline phrase and no extracted event has that date, the article gets a second, focused
/// extraction.
/// </summary>
internal static partial class TimelineDeadlineGuard
{
    private const int WindowLength = 150;
    private const int MaxYearsAhead = 5;
    private const string MonthNames = "January|February|March|April|May|June|July|August|September|October|November|December";
    private static readonly HashSet<string> _deadlineKinds = new(StringComparer.Ordinal)
    {
        "deprecation", "retirement", "end-of-support", "breaking-change"
    };

    /// <summary>
    /// Whether <paramref name="text"/> names a date after <paramref name="publicationDate"/> in the neighborhood of a deadline phrase
    /// that none of <paramref name="events"/> covers.
    /// </summary>
    public static bool MissesDeadline(string text, IReadOnlyList<TimelineEvent> events, DateOnly publicationDate)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(events);

        var latest = publicationDate.AddYears(MaxYearsAhead);

        foreach (var (date, index, length) in FindDates(text))
        {
            if (date <= publicationDate || date > latest || IsCovered(date, events))
            {
                continue;
            }

            var start = Math.Max(0, index - WindowLength);
            var end = Math.Min(text.Length, index + length + WindowLength);
            if (DeadlinePhrase().IsMatch(text.AsSpan(start, end - start)))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsCovered(DateOnly date, IReadOnlyList<TimelineEvent> events) => events.Any(e =>
        _deadlineKinds.Contains(e.Kind)
        && (e.Date == date || (e.DatePrecision != "day" && e.Date.Year == date.Year && e.Date.Month == date.Month)));

    private static IEnumerable<(DateOnly Date, int Index, int Length)> FindDates(string text)
    {
        var textualDates = MonthDayYear().Matches(text).Cast<Match>()
            .Concat(DayMonthYear().Matches(text))
            .Select(match =>
            {
                var isValid = TryBuild(match.Groups["y"].Value, match.Groups["m"].Value, match.Groups["d"].Value, out var date);
                return (Match: match, IsValid: isValid, Date: date);
            })
            .Where(parsed => parsed.IsValid);

        foreach (var parsed in textualDates)
        {
            yield return (parsed.Date, parsed.Match.Index, parsed.Match.Length);
        }

        var isoDates = IsoDate().Matches(text).Cast<Match>()
            .Select(match =>
            {
                var isValid = DateOnly.TryParseExact(match.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
                return (Match: match, IsValid: isValid, Date: date);
            })
            .Where(parsed => parsed.IsValid);

        foreach (var parsed in isoDates)
        {
            yield return (parsed.Date, parsed.Match.Index, parsed.Match.Length);
        }
    }

    private static bool TryBuild(string year, string month, string day, out DateOnly date)
    {
        date = default;

        if (!DateTime.TryParseExact(month, "MMMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var monthDate)
            || !int.TryParse(year, CultureInfo.InvariantCulture, out var y)
            || !int.TryParse(day, CultureInfo.InvariantCulture, out var d)
            || y is < 2000 or > 2100
            || d < 1
            || d > DateTime.DaysInMonth(y, monthDate.Month))
        {
            return false;
        }

        date = new DateOnly(y, monthDate.Month, d);
        return true;
    }

    [GeneratedRegex($@"\b(?<m>{MonthNames})\s+(?<d>\d{{1,2}})(?:st|nd|rd|th)?,?\s+(?<y>\d{{4}})\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 1000)]
    private static partial Regex MonthDayYear();

    [GeneratedRegex($@"\b(?<d>\d{{1,2}})(?:st|nd|rd|th)?\s+(?<m>{MonthNames})\s+(?<y>\d{{4}})\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 1000)]
    private static partial Regex DayMonthYear();

    [GeneratedRegex(@"\b\d{4}-\d{2}-\d{2}\b", RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 1000)]
    private static partial Regex IsoDate();

    [GeneratedRegex(@"retir|deprecat|end[\s-]of[\s-](?:support|life)|sunset|no longer (?:be )?supported|support ends?|will be removed|breaking change|discontinu|shut(?:s|ting)? down", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 1000)]
    private static partial Regex DeadlinePhrase();
}
