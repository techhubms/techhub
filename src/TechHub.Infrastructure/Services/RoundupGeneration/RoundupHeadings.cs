using System.Text;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Extracts the topic headings of a previous roundup. Giving the writer only the headings is enough for an
/// occasional "following last week's ..." sentence and costs far less than the whole previous roundup.
/// </summary>
internal static class RoundupHeadings
{
    private const int MaxHeadings = 30;

    /// <summary>Returns the <c>##</c> and <c>###</c> headings of <paramref name="roundupContent"/>, one per line, or an empty string.</summary>
    public static string Extract(string? roundupContent)
    {
        if (string.IsNullOrWhiteSpace(roundupContent))
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        var count = 0;
        var inCodeFence = false;

        foreach (var rawLine in roundupContent.Split('\n'))
        {
            var line = rawLine.TrimEnd('\r');

            if (line.StartsWith("```", StringComparison.Ordinal))
            {
                inCodeFence = !inCodeFence;
                continue;
            }

            if (inCodeFence || !IsTopicHeading(line))
            {
                continue;
            }

            sb.AppendLine(line);
            if (++count >= MaxHeadings)
            {
                break;
            }
        }

        return sb.ToString().TrimEnd();
    }

    private static bool IsTopicHeading(string line)
    {
        if (!line.StartsWith("## ", StringComparison.Ordinal) && !line.StartsWith("### ", StringComparison.Ordinal))
        {
            return false;
        }

        var title = line.TrimStart('#').Trim();
        return !title.Equals("This Week's Overview", StringComparison.OrdinalIgnoreCase)
            && !title.Equals("Also relevant this week", StringComparison.OrdinalIgnoreCase)
            && !title.Equals("Dates to know", StringComparison.OrdinalIgnoreCase);
    }
}
