namespace TechHub.Web.Components.Layout;

/// <summary>One page in the admin navigation.</summary>
/// <param name="Label">Text of the link.</param>
/// <param name="Href">Path of the page.</param>
/// <param name="Exact">When true only <paramref name="Href"/> itself is this page; otherwise every path below it is too.</param>
/// <param name="Also">Other path prefixes that belong to this page (for example the content preview of the content page).</param>
/// <param name="Except">Path prefixes below <paramref name="Href"/> that belong to another page.</param>
public sealed record AdminNavItem(
    string Label,
    string Href,
    bool Exact = false,
    IReadOnlyList<string>? Also = null,
    IReadOnlyList<string>? Except = null)
{
    /// <summary>Whether the (normalized, lowercase, without query) <paramref name="path"/> is this page.</summary>
    public bool Matches(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (Except?.Any(e => IsPathOrBelow(path, e)) == true)
        {
            return false;
        }

        if (string.Equals(path, Href, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return (!Exact && IsPathOrBelow(path, Href)) || (Also?.Any(a => IsPathOrBelow(path, a)) ?? false);
    }

    private static bool IsPathOrBelow(string path, string prefix) =>
        string.Equals(path, prefix, StringComparison.OrdinalIgnoreCase)
        || path.StartsWith(prefix + "/", StringComparison.OrdinalIgnoreCase);
}

/// <summary>A top-level admin menu item with the pages in its sub menu.</summary>
/// <param name="Label">Text of the top-level link.</param>
/// <param name="Items">The pages of the group; the first one is where the top-level link goes.</param>
public sealed record AdminNavGroup(string Label, IReadOnlyList<AdminNavItem> Items)
{
    /// <summary>Where the top-level link goes.</summary>
    public string Href => Items[0].Href;

    /// <summary>The page of this group that <paramref name="path"/> belongs to, or <c>null</c>.</summary>
    public AdminNavItem? FindItem(string path) => Items.FirstOrDefault(i => i.Matches(path));
}

/// <summary>The structure of the admin menu: top-level groups and the pages in their sub menus.</summary>
public static class AdminNavigation
{
    public static IReadOnlyList<AdminNavGroup> Groups { get; } =
    [
        new("Automatic Processing",
        [
            new("Dashboard", "/admin", Exact: true),
            new("RSS Feeds", "/admin/feeds"),
            new("Processed", "/admin/processed-urls")
        ]),
        new("Content Management",
        [
            new("Content Items", "/admin/content-items", Also: ["/admin/content-preview"]),
            new("Products", "/admin/products"),
            new("Visual Studio Code Updates", "/admin/vscode-updates"),
            new("Custom Pages", "/admin/custom-pages")
        ]),
        new("Reviews", [new("Reviews", "/admin/reviews")]),
        new("Newsletter",
        [
            new("Subscribers", "/admin/newsletter", Exact: true),
            new("Send log", "/admin/newsletter/send-log")
        ]),
        new("Stats", [new("Stats", "/admin/statistics")]),
        new("Settings",
        [
            new("Settings", "/admin/settings"),
            new("Feature toggles", "/admin/feature-toggles")
        ])
    ];

    /// <summary>Turns an absolute or relative URL into the path used for matching: no query or fragment, no trailing slash.</summary>
    public static string NormalizePath(string baseRelativePath)
    {
        ArgumentNullException.ThrowIfNull(baseRelativePath);

        var path = baseRelativePath;
        var end = path.IndexOfAny(['?', '#']);
        if (end >= 0)
        {
            path = path[..end];
        }

        return "/" + path.Trim('/');
    }

    /// <summary>The group that <paramref name="path"/> belongs to, or <c>null</c> for a path outside the menu.</summary>
    public static AdminNavGroup? FindGroup(string path) => Groups.FirstOrDefault(g => g.FindItem(path) is not null);
}
