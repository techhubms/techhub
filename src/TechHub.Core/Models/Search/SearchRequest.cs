namespace TechHub.Core.Models;

/// <summary>
/// Search request parameters for filtering, searching, and pagination.
/// </summary>
public class SearchRequest
{
    public string? Query { get; }
    public IReadOnlyList<string> Tags { get; }
    public IReadOnlyList<string> Sections { get; }
    public IReadOnlyList<string> Collections { get; }
    public string? Subcollection { get; }
    public DateTimeOffset? DateFrom { get; }
    public DateTimeOffset? DateTo { get; }
    public bool IncludeDraft { get; }
    public bool UseSemanticSearch { get; }
    public int Take { get; }
    public int Skip { get; }
    public string OrderBy { get; }
    public string? ContinuationToken { get; }
    public bool IncludeFacets { get; }

    public SearchRequest(
        int take,
        IReadOnlyList<string> sections,
        IReadOnlyList<string> collections,
        IReadOnlyList<string> tags,
        int skip = 0,
        string? query = null,
        string? subcollection = null,
        DateTimeOffset? dateFrom = null,
        DateTimeOffset? dateTo = null,
        bool includeDraft = false,
        bool useSemanticSearch = false,
        string orderBy = "date_desc",
        string? continuationToken = null,
        bool includeFacets = false)
    {
        ArgumentNullException.ThrowIfNull(sections);
        ArgumentNullException.ThrowIfNull(collections);
        ArgumentNullException.ThrowIfNull(tags);

        if (take <= 0)
        {
            throw new ArgumentException($"Take must be greater than 0, got {take}", nameof(take));
        }

        if (take > 50)
        {
            throw new ArgumentException($"Take cannot exceed 50, got {take}", nameof(take));
        }

        if (skip < 0)
        {
            throw new ArgumentException($"Skip cannot be negative, got {skip}", nameof(skip));
        }

        if (sections.Count == 0)
        {
            throw new ArgumentException("Sections cannot be empty - provide specific section names or 'all'", nameof(sections));
        }

        if (collections.Count == 0)
        {
            throw new ArgumentException("Collections cannot be empty - provide specific collection names or 'all'", nameof(collections));
        }

        if (dateFrom.HasValue && dateTo.HasValue && dateFrom > dateTo)
        {
            throw new ArgumentException("DateFrom cannot be after DateTo", nameof(dateFrom));
        }

        // Validate special "all" value usage
        if (sections.Count > 1 && sections.Any(s => s.Equals("all", StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("Cannot combine 'all' with specific section names", nameof(sections));
        }

        if (collections.Count > 1 && collections.Any(c => c.Equals("all", StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("Cannot combine 'all' with specific collection names", nameof(collections));
        }

        Take = take;
        Skip = skip;
        Query = query;
        Tags = tags;
        Sections = sections;
        Collections = collections;
        Subcollection = subcollection;
        DateFrom = dateFrom;
        DateTo = dateTo;
        IncludeDraft = includeDraft;
        UseSemanticSearch = useSemanticSearch;
        OrderBy = orderBy;
        ContinuationToken = continuationToken;
        IncludeFacets = includeFacets;
    }

    /// <summary>
    /// Build cache key for this search request based on all query parameters.
    /// </summary>
    public string GetCacheKey()
    {
        // Optimization: Use StringBuilder to reduce allocations
        var sb = new System.Text.StringBuilder("search");

        if (!string.IsNullOrWhiteSpace(Query))
        {
            sb.Append("|q:").Append(Query);
        }

        if (Tags.Count > 0)
        {
            sb.Append("|t:");
            var sortedTags = (System.Collections.Generic.IEnumerable<string>)(Tags.Count == 1 ? Tags : Tags.OrderBy(x => x));
            sb.AppendJoin(',', sortedTags);
        }

        if (Sections.Count > 0)
        {
            sb.Append("|s:");
            var sortedSections = (System.Collections.Generic.IEnumerable<string>)(Sections.Count == 1 ? Sections : Sections.OrderBy(x => x));
            sb.AppendJoin(',', sortedSections);
        }

        if (Collections.Count > 0)
        {
            sb.Append("|c:");
            var sortedCollections = (System.Collections.Generic.IEnumerable<string>)(Collections.Count == 1 ? Collections : Collections.OrderBy(x => x));
            sb.AppendJoin(',', sortedCollections);
        }

        if (!string.IsNullOrWhiteSpace(Subcollection))
        {
            sb.Append("|sc:").Append(Subcollection);
        }

        if (DateFrom.HasValue)
        {
            sb.Append("|df:").Append(DateFrom.Value.ToUnixTimeSeconds());
        }

        if (DateTo.HasValue)
        {
            sb.Append("|dt:").Append(DateTo.Value.ToUnixTimeSeconds());
        }

        if (IncludeDraft)
        {
            sb.Append("|draft:1");
        }

        sb.Append("|take:").Append(Take);
        sb.Append("|skip:").Append(Skip);

        if (!string.IsNullOrWhiteSpace(ContinuationToken))
        {
            sb.Append("|ct:").Append(ContinuationToken);
        }

        return sb.ToString();
    }
}
