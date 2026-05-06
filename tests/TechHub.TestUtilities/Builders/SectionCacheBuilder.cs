using TechHub.Core.Models;

namespace TechHub.TestUtilities.Builders;

/// <summary>
/// Builder for <see cref="SectionCache"/> instances in tests.
///
/// Defaults to a production-like ready cache with all 7 real sections
/// (ai, ml, devops, azure, dotnet, security, github-copilot) each containing
/// all 5 standard collections (news, videos, community, blogs, roundups).
///
/// Usage:
/// <code>
/// // Production-like ready cache (most tests)
/// var cache = A.SectionCache.Build();
///
/// // Specific sections and/or collections
/// var cache = A.SectionCache.WithSections("ai", "dotnet").WithCollections("videos", "blogs").Build();
///
/// // Empty (not ready) cache — for testing !IsReady behaviour
/// var cache = A.SectionCache.Empty();
/// </code>
/// </summary>
public class SectionCacheBuilder
{
    private static readonly string[] _defaultSections =
        ["ai", "ml", "devops", "azure", "dotnet", "security", "github-copilot"];

    private static readonly string[] _defaultCollections =
        ["news", "videos", "community", "blogs", "roundups"];

    private string[] _sections = _defaultSections;
    private string[] _collections = _defaultCollections;

    public SectionCacheBuilder WithSections(params string[] sections)
    {
        _sections = sections;
        return this;
    }

    public SectionCacheBuilder WithCollections(params string[] collections)
    {
        _collections = collections;
        return this;
    }

    public SectionCache Build()
    {
        var cache = new SectionCache();
        var sections = _sections
            .Select(name => new Section(
                name, name, "desc", $"/{name}", name,
                _collections.Select(c => new Collection(c, c, $"/{name}/{c}", c, c)).ToList()))
            .ToList();
        cache.Initialize(sections);
        return cache;
    }

    /// <summary>Returns an uninitialised cache (<see cref="SectionCache.IsReady"/> is false).</summary>
    public static SectionCache Empty() => new();
}
