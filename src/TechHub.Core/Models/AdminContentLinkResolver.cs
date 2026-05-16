namespace TechHub.Core.Models;

public static class AdminContentLinkResolver
{
    public static bool LinksExternally(string collectionName)
    {
        ArgumentNullException.ThrowIfNull(collectionName);
        return collectionName.ToLowerInvariant() is "news" or "blogs" or "community";
    }

    public static string GetHref(string collectionName, string slug, string? externalUrl, string? primarySectionName = null)
    {
        ArgumentNullException.ThrowIfNull(collectionName);
        ArgumentNullException.ThrowIfNull(slug);

        var normalizedCollection = collectionName.ToLowerInvariant();
        var normalizedSlug = slug.ToLowerInvariant();
        var normalizedExternalUrl = externalUrl ?? string.Empty;

        if (normalizedCollection is "news" or "blogs" or "community")
        {
            return normalizedExternalUrl;
        }

        if (normalizedCollection == "roundups")
        {
            return $"/all/roundups/{normalizedSlug}";
        }

        if (!string.IsNullOrWhiteSpace(primarySectionName))
        {
            return $"/{primarySectionName.ToLowerInvariant()}/{normalizedCollection}/{normalizedSlug}";
        }

        return normalizedExternalUrl;
    }
}
