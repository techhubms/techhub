namespace TechHub.Web.Components;

/// <summary>One card of a <see cref="TimelineView"/>: a dated event, feature or release.</summary>
public sealed record TimelineViewEntry
{
    /// <summary>Unique within the timeline. Used in element ids, so keep it to letters, digits and dashes.</summary>
    public required string Id { get; init; }

    public required string Title { get; init; }

    /// <summary>The date the entry is placed under. <c>null</c> puts it in a "Date TBA" group at the end.</summary>
    public DateOnly? Date { get; init; }

    /// <summary>Optional text shown in the card header next to the title, for example the exact day or the quarter.</summary>
    public string? DateLabel { get; init; }

    /// <summary>Shown when the card is expanded.</summary>
    public string? Description { get; init; }

    public IReadOnlyList<TimelineViewBadge> Badges { get; init; } = [];
}

/// <summary>A small label in the header of a timeline card.</summary>
/// <param name="Text">The label.</param>
/// <param name="CssClass">One of the design system badge classes, for example <c>badge-info</c> or <c>badge-success</c>.</param>
public sealed record TimelineViewBadge(string Text, string CssClass = "badge-info");

/// <summary>A heading with a single link, shown above the first year of a <see cref="TimelineView"/>, for example "Future events" that are hidden.</summary>
/// <param name="Label">The heading, shown where a year is.</param>
/// <param name="LinkText">The text of the link, shown below the heading.</param>
/// <param name="Href">Where the link goes.</param>
public sealed record TimelineViewNotice(string Label, string LinkText, string Href);
