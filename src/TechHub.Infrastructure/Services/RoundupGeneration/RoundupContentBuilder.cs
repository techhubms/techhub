using System.Globalization;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Step 8: Builds the final roundup content including table of contents.
/// </summary>
internal static class RoundupContentBuilder
{
    /// <summary>
    /// Builds the table of contents from markdown section/subsection headers.
    /// </summary>
    public static string BuildTableOfContents(string content)
    {
        var lines = content.Split('\n');
        var tocLines = new List<string>();

        foreach (var line in lines)
        {
            if (line.StartsWith("## ", StringComparison.Ordinal))
            {
                var sectionTitle = line[3..].Trim();
                var anchor = BuildAnchor(sectionTitle);
                tocLines.Add(string.Create(CultureInfo.InvariantCulture, $"- [{sectionTitle}](#{anchor})"));
            }
            else if (line.StartsWith("### ", StringComparison.Ordinal))
            {
                var subsectionTitle = line[4..].Trim();
                var anchor = BuildAnchor(subsectionTitle);
                tocLines.Add(string.Create(CultureInfo.InvariantCulture, $"  - [{subsectionTitle}](#{anchor})"));
            }
        }

        return string.Join("\n", tocLines);
    }

    /// <summary>
    /// Assembles the final roundup content with introduction, TOC, and section content.
    /// </summary>
    public static string BuildFullContent(string sectionContent, string introduction, string tableOfContents) =>
        $"{introduction}\n\n<!--excerpt_end-->\n\n## This Week's Overview\n\n{tableOfContents}\n\n{sectionContent}";

    /// <summary>
    /// Generates the roundup slug from the publish date.
    /// </summary>
    public static string BuildSlug(DateOnly publishDate) =>
        publishDate.ToString("'weekly-ai-and-tech-news-roundup-'yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static string BuildAnchor(string title)
    {
        // Matches Kramdown anchor generation: remove non-alphanumeric-space-dash, lowercase, replace non-alnum with dash.
        var clean = new string(title.Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-').ToArray());
        return new string(clean.ToLowerInvariant().Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray());
    }
}
