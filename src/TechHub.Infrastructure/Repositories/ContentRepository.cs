using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Text;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Data;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Database-backed content repository using PostgreSQL.
/// Uses ISqlDialect abstraction for database-specific syntax.
/// Provides caching logic and markdown rendering for all content operations.
/// Supports optional query logging when DatabaseOptions.EnableQueryLogging is enabled.
/// </summary>
public class ContentRepository : IContentRepository
{
    private static readonly string[] _searchFacetFields = ["tags", "collections", "sections"];
    /// <summary>
    /// Column selection for list views - excludes content column for performance.
    /// Maps to ContentItem record which doesn't have a Content property.
    /// </summary>
    protected const string ListViewColumns = @"
                c.slug AS Slug,
                c.title AS Title,
                c.author AS Author,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.feed_name AS FeedName,
                c.primary_section_name AS PrimarySectionName,
                c.excerpt AS Excerpt,
                c.external_url AS ExternalUrl,
                c.draft AS Draft,
                c.subcollection_name AS SubcollectionName,
                c.plans AS Plans,
                c.ghes_support AS GhesSupport,
                c.tags_csv AS TagsCsv,
                c.is_ai AS IsAi,
                c.is_azure AS IsAzure,
                c.is_dotnet AS IsDotNet,
                c.is_devops AS IsDevOps,
                c.is_github_copilot AS IsGitHubCopilot,
                c.is_ml AS IsMl,
                c.is_security AS IsSecurity";

    /// <summary>
    /// Column selection for detail views - includes content column for markdown rendering.
    /// Maps to ContentItemDetail record which extends ContentItem with the Content property.
    /// </summary>
    protected const string DetailViewColumns = @"
                c.slug AS Slug,
                c.title AS Title,
                c.author AS Author,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.feed_name AS FeedName,
                c.primary_section_name AS PrimarySectionName,
                c.excerpt AS Excerpt,
                c.external_url AS ExternalUrl,
                c.draft AS Draft,
                c.content AS Content,
                c.subcollection_name AS SubcollectionName,
                c.plans AS Plans,
                c.ghes_support AS GhesSupport,
                c.tags_csv AS TagsCsv,
                c.is_ai AS IsAi,
                c.is_azure AS IsAzure,
                c.is_dotnet AS IsDotNet,
                c.is_devops AS IsDevOps,
                c.is_github_copilot AS IsGitHubCopilot,
                c.is_ml AS IsMl,
                c.is_security AS IsSecurity";

    protected IDbConnection Connection { get; }
    protected ISqlDialect Dialect { get; }
    protected IMemoryCache Cache { get; }
    protected IMarkdownService MarkdownService { get; }

    private readonly AppSettings _settings;
    private readonly ILogger<ContentRepository>? _logger;
    private readonly bool _enableQueryLogging;

    // ── Cache TTLs ───────────────────────────────────────────────────────────
    // Content-derived caches use TTLs as a safety net. Event-driven invalidation
    // (via InvalidateCachedData) provides immediate freshness after writes;
    // TTLs ensure self-healing if an invalidation path is missed.
    private static readonly TimeSpan _searchCacheTtl = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan _slugCacheTtl = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan _tagCacheTtl = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan _sitemapCacheTtl = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan _authorCacheTtl = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan _configCacheTtl = TimeSpan.FromHours(1);

    static ContentRepository()
    {
        // Register custom type handler for safe boolean conversion
        SqlMapper.AddTypeHandler(new BooleanTypeHandler());
    }

    /// <summary>
    /// Type handler for safe boolean conversion from database values.
    /// Handles both native bool and int representations.
    /// </summary>
    private sealed class BooleanTypeHandler : SqlMapper.TypeHandler<bool>
    {
        public override bool Parse(object value)
        {
            return value switch
            {
                long l => l != 0,
                int i => i != 0,
                bool b => b,
                _ => throw new ArgumentException($"Cannot convert {value.GetType()} to bool")
            };
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, bool value)
        {
            parameter.Value = value ? 1 : 0;
        }
    }

    public ContentRepository(
        IDbConnection connection,
        ISqlDialect dialect,
        IMemoryCache cache,
        IMarkdownService markdownService,
        IOptions<AppSettings> settings,
        ILogger<ContentRepository>? logger = null,
        IOptions<DatabaseOptions>? databaseOptions = null)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(dialect);
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(markdownService);
        ArgumentNullException.ThrowIfNull(settings);

        Connection = connection;
        Dialect = dialect;
        Cache = cache;
        MarkdownService = markdownService;
        _settings = settings.Value;
        _logger = logger;
        _enableQueryLogging = databaseOptions?.Value.EnableQueryLogging ?? false;
    }

    // ==================== Section Methods ====================

    /// <summary>
    /// Initialize sections from configuration.
    /// Converts configuration to Section models and applies ordering using <see cref="SectionConfig.Order"/>.
    /// </summary>
    private static ReadOnlyCollection<Section> InitializeSections(AppSettings settings)
    {
        // Convert configuration to Section models, ordered by the configured Order property.
        // "all" has Order=0 (default) so it comes first, then sections by their explicit Order value.
        return settings.Content.Sections
            .OrderBy(kvp => kvp.Value.Order)
            .Select(kvp => ConvertToSection(kvp.Key, kvp.Value))
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Convert SectionConfig from appsettings.json to Section model.
    /// </summary>
    private static Section ConvertToSection(string sectionName, SectionConfig config)
    {
        // Define collection display order (non-custom collections shown after "All" in SubNav/NavHeader)
        var collectionOrder = new[] { "roundups", "news", "blogs", "videos", "community" };

        var collectionsDict = config.Collections
            .Select(kvp =>
            {
                // Use GetTagFromName for display name (e.g., "blogs" -> "Blogs", "vscode-updates" -> "Vscode Updates")
                var displayName = Collection.GetTagFromName(kvp.Key);
                return new Collection(
                    kvp.Key,
                    kvp.Value.Title,
                    kvp.Value.Url,
                    kvp.Value.Description,
                    displayName,
                    kvp.Value.Custom,
                    kvp.Value.Order);
            })
            .ToDictionary(c => c.Name);

        // Order: known collections first in defined order, then any remaining non-custom alphabetically, then custom pages by Order/Title
        var ordered = collectionOrder
            .Where(name => collectionsDict.ContainsKey(name))
            .Select(name => collectionsDict[name])
            .Concat(collectionsDict.Values
                .Where(c => !c.IsCustom && !collectionOrder.Contains(c.Name))
                .OrderBy(c => c.Title))
            .Concat(collectionsDict.Values
                .Where(c => c.IsCustom)
                .OrderBy(c => c.Order)
                .ThenBy(c => c.Title))
            .ToList();

        return new Section(sectionName, config.Title, config.Description, config.Url, config.Tag, ordered, config.HideCollectionPages, config.ShowHeroBanner);
    }

    /// <summary>
    /// Get all sections defined in configuration.
    /// Sections are loaded lazily and cached in memory.
    /// </summary>
    public async Task<IReadOnlyList<Section>> GetAllSectionsAsync(CancellationToken ct = default)
    {
        return await Cache.GetOrCreateAsync("sections:all", entry =>
        {
            entry.SetAbsoluteExpiration(_configCacheTtl);
            return Task.FromResult(InitializeSections(_settings));
        }) ?? [];
    }

    /// <summary>
    /// Get a single section by name.
    /// Sections are loaded lazily and cached in memory.
    /// </summary>
    public async Task<Section?> GetSectionByNameAsync(string name, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        var sections = await GetAllSectionsAsync(ct);
        return sections.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    // ==================== Sitemap Methods ====================

    /// <summary>
    /// Gets all published content items that have a real detail page on the site.
    /// Excludes news/blogs/community items (they link externally to the original source).
    /// Results are cached — sitemap generation can call this freely.
    /// </summary>
    public async Task<IReadOnlyList<SitemapItem>> GetSitemapItemsAsync(CancellationToken ct = default)
    {
        return await Cache.GetOrCreateAsync("sitemap:items", async entry =>
        {
            entry.SetAbsoluteExpiration(_sitemapCacheTtl);
            return await GetSitemapItemsInternalAsync(ct);
        }) ?? [];
    }

    private async Task<IReadOnlyList<SitemapItem>> GetSitemapItemsInternalAsync(CancellationToken ct)
    {
        var sql = $@"
            SELECT
                c.slug               AS Slug,
                c.primary_section_name AS PrimarySectionName,
                c.collection_name    AS CollectionName,
                c.date_epoch         AS DateEpoch
            FROM content_items c
            WHERE c.draft = {Dialect.GetBooleanLiteral(false)}
              AND c.collection_name NOT IN ('news', 'blogs', 'community')
            ORDER BY c.date_epoch DESC";

        var results = await Connection.QueryAsync<SitemapItem>(new CommandDefinition(sql, cancellationToken: ct));
        return results.ToArray();
    }

    // ==================== Author Methods ====================

    /// <summary>
    /// Get all known authors with their published content item counts.
    /// Returns authors sorted alphabetically by name.
    /// Results are cached for the lifetime of the application.
    /// </summary>
    public async Task<IReadOnlyList<AuthorSummary>> GetAuthorsAsync(CancellationToken ct = default)
    {
        return await Cache.GetOrCreateAsync("authors:all", async entry =>
        {
            entry.SetAbsoluteExpiration(_authorCacheTtl);
            return await GetAuthorsInternalAsync(ct);
        }) ?? [];
    }

    private async Task<IReadOnlyList<AuthorSummary>> GetAuthorsInternalAsync(CancellationToken ct)
    {
        var sql = $@"
            SELECT
                c.author   AS Name,
                COUNT(*)   AS ItemCount
            FROM content_items c
            WHERE c.draft = {Dialect.GetBooleanLiteral(false)}
              AND c.author IS NOT NULL
              AND c.author <> ''
            GROUP BY c.author
            ORDER BY LOWER(c.author), c.author";

        var results = await Connection.QueryWithLoggingAsync<AuthorSummary>(
            new CommandDefinition(sql, cancellationToken: ct),
            _logger,
            _enableQueryLogging);

        return results.ToArray();
    }

    // ==================== Markdown Rendering ====================

    /// <summary>
    /// Renders the raw markdown content to HTML if Content is present and RenderedHtml is not already set.
    /// After rendering, clears the raw Content to save memory since it's no longer needed.
    /// </summary>
    protected ContentItemDetail RenderHtmlIfNeeded(ContentItemDetail item)
    {
        ArgumentNullException.ThrowIfNull(item);

        // If RenderedHtml is already set, return as-is (Content already nulled)
        if (item.RenderedHtml != null)
        {
            return item;
        }

        // If no raw content to render, something is wrong - item should have either RenderedHtml or Content
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new InvalidOperationException(
                $"ContentItemDetail '{item.Slug}' in collection '{item.CollectionName}' has no Content to render and no RenderedHtml. " +
                "Items must have either pre-rendered HTML or raw markdown content.");
        }

        // Render the markdown to HTML
        var processedMarkdown = MarkdownService.ProcessYouTubeEmbeds(item.Content);
        var renderedHtml = MarkdownService.RenderToHtml(processedMarkdown);

        // Set rendered HTML (this also clears Content to save memory)
        item.SetRenderedHtml(renderedHtml);
        return item;
    }

    // ==================== Public Methods with Caching ====================

    /// <summary>
    /// Get a single content item by slug and collection.
    /// Results are cached in memory. Renders markdown to HTML if needed.
    /// Returns ContentItemDetail which includes the full markdown content and rendered HTML.
    /// </summary>
    public async Task<ContentItemDetail?> GetBySlugAsync(
        string collectionName,
        string slug,
        bool includeDraft = false,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(collectionName);
        ArgumentNullException.ThrowIfNull(slug);

        // Lowercase before cache lookup and DB query so that URL casing never causes a miss.
        collectionName = collectionName.ToLowerInvariant();
        slug = slug.ToLowerInvariant();

        var cacheKey = $"slug:{collectionName}:{slug}:{includeDraft}";
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetAbsoluteExpiration(_slugCacheTtl);
            var item = await GetBySlugInternalAsync(collectionName, slug, includeDraft, ct);
            return item != null ? RenderHtmlIfNeeded(item) : null;
        });
    }

    /// <summary>
    /// Search content with filters, facets, and pagination.
    /// Results are cached in memory based on search parameters.
    /// </summary>
    public async Task<SearchResults<ContentItem>> SearchAsync(SearchRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cacheKey = request.GetCacheKey();
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetAbsoluteExpiration(_searchCacheTtl);
            return await SearchInternalAsync(request, ct);
        }) ?? new SearchResults<ContentItem>
        {
            Items = Array.Empty<ContentItem>(),
            TotalCount = 0,
            Facets = new FacetResults { Facets = new Dictionary<string, IReadOnlyList<FacetValue>>(), TotalCount = 0 }
        };
    }

    /// <summary>
    /// Get facet counts for tags, collections, and sections.
    /// Results are cached in memory based on facet request parameters.
    /// </summary>
    public async Task<FacetResults> GetFacetsAsync(FacetRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cacheKey = request.GetCacheKey();
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetAbsoluteExpiration(_searchCacheTtl);
            return await GetFacetsInternalAsync(request, ct);
        }) ?? new FacetResults { Facets = new Dictionary<string, IReadOnlyList<FacetValue>>(), TotalCount = 0 };
    }

    /// <summary>
    /// Get tag counts with optional filtering.
    /// Returns top N tags (sorted by count descending) above minUses threshold.
    /// Results are cached - very fast for repeated calls with same filters.
    /// </summary>
    public async Task<IReadOnlyList<TagWithCount>> GetTagCountsAsync(
        TagCountsRequest request,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cacheKey = request.GetCacheKey();
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetAbsoluteExpiration(_tagCacheTtl);
            return await GetTagCountsInternalAsync(request, ct);
        }) ?? [];
    }

    // ==================== Protected Helper Methods ====================

    /// <summary>
    /// Get the cached set of tags to exclude from tag clouds.
    /// Cached for the lifetime of the application since it's based on static configuration.
    /// Returns a case-insensitive HashSet for efficient contains checks and SQL filtering.
    /// Includes: section tags, collection tags, and high-frequency terms (github, copilot, microsoft).
    /// </summary>
    protected async Task<HashSet<string>> GetExcludeTagsSetAsync()
    {
        return await Cache.GetOrCreateAsync("excludetags:set", async entry =>
        {
            entry.SetAbsoluteExpiration(_configCacheTtl);
            return await BuildExcludeTagsSetAsync();
        }) ?? [];
    }

    /// <summary>
    /// Build a set of section and collection tags to exclude from tag clouds.
    /// Uses section Tags from configuration and programmatically generated collection tags.
    /// These are structural tags added by ContentFixer for search purposes,
    /// but shouldn't appear in tag clouds as they're redundant (users already filter by section/collection).
    /// </summary>
    private async Task<HashSet<string>> BuildExcludeTagsSetAsync()
    {
        var sections = await GetAllSectionsAsync();

        var sectionTags = sections
            .Select(s => s.Tag)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t!);

        var collectionTags = sections
            .SelectMany(s => s.Collections)
            .Select(c => Collection.GetTagFromName(c.Name))
            .Where(t => !string.IsNullOrWhiteSpace(t));

        return TagExclusions.BuildExcludeSet(sectionTags, collectionTags);
    }

    // ==================== Internal Data Access Methods ====================

    /// <summary>
    /// Internal implementation for getting content by slug.
    /// Uses SQL to query the database. Includes content column for detail view rendering.
    /// Returns ContentItemDetail which includes the markdown content.
    /// </summary>
    protected async Task<ContentItemDetail?> GetBySlugInternalAsync(
        string collectionName,
        string slug,
        bool includeDraft,
        CancellationToken ct)
    {
        // Build WHERE clause conditionally to allow index usage
        var draftFilter = includeDraft ? "" : $"AND c.draft = {Dialect.GetBooleanLiteral(false)}";

        var sql = $@"
            SELECT {DetailViewColumns}
            FROM content_items c
            WHERE c.collection_name = @collectionName
              AND c.slug = @slug
              {draftFilter}";

        var item = await Connection.QuerySingleOrDefaultAsync<ContentItemDetail>(
            new CommandDefinition(sql, new { collectionName, slug }, cancellationToken: ct));

        return item;
    }

    /// <summary>
    /// Calculate bitmask value for section filtering.
    /// Bit 0 (1) = AI, Bit 1 (2) = Azure, Bit 2 (4) = .NET, Bit 3 (8) = DevOps,
    /// Bit 4 (16) = GitHub Copilot, Bit 5 (32) = ML, Bit 6 (64) = Security.
    /// </summary>
    /// <param name="sections">Collection of section names to include in bitmask</param>
    /// <returns>Integer bitmask with bits set for each matching section</returns>
    protected static int CalculateSectionBitmask(IEnumerable<string> sections)
    {
        ArgumentNullException.ThrowIfNull(sections);

        var bitmask = 0;
        foreach (var section in sections)
        {
            bitmask |= CalculateSectionBitmask(section);
        }

        return bitmask;
    }

    /// <summary>
    /// Calculate bitmask value for a single section.
    /// </summary>
    /// <param name="section">Section name</param>
    /// <returns>Integer bitmask value for the section (0 if unknown)</returns>
    protected static int CalculateSectionBitmask(string section)
    {
        ArgumentNullException.ThrowIfNull(section);

        var sectionNormalized = section.ToLowerInvariant().Trim();
        return sectionNormalized switch
        {
            "ai" => 1,
            "azure" => 2,
            "dotnet" => 4,
            "devops" => 8,
            "github-copilot" => 16,
            "ml" => 32,
            "security" => 64,
            _ => 0
        };
    }

    /// <summary>
    /// Get facet counts for tags, collections, and sections within the filtered scope.
    /// Uses standard SQL.
    /// </summary>
    protected async Task<FacetResults> GetFacetsInternalAsync(FacetRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var parameters = new DynamicParameters();
        var facets = new Dictionary<string, IReadOnlyList<FacetValue>>();

        // Build WHERE clause for filtering
        var whereClauses = BuildFilterWhereClauses(request, parameters);
        var whereClause = whereClauses.Count > 0 ? "WHERE " + string.Join(" AND ", whereClauses) : "";

        // Get total count
        var countSql = $"SELECT COUNT(DISTINCT c.slug) FROM content_items c {whereClause}";
        var totalCount = await Connection.ExecuteScalarAsync<long>(
            new CommandDefinition(countSql, parameters, cancellationToken: ct));

        // Get tag facets if requested
        if (request.FacetFields.Contains("tags"))
        {
            var tagsSql = $@"
                SELECT cte.tag_word AS Value, COUNT(DISTINCT c.slug) AS Count
                FROM content_tags_expanded cte
                INNER JOIN content_items c ON cte.slug = c.slug AND cte.collection_name = c.collection_name
                {whereClause}
                GROUP BY cte.tag_word
                ORDER BY Count DESC, cte.tag_word
                LIMIT @maxFacetValues";

            parameters.Add("maxFacetValues", request.MaxFacetValues);

            var tags = await Connection.QueryWithLoggingAsync<FacetValue>(
                new CommandDefinition(tagsSql, parameters, cancellationToken: ct),
                _logger,
                _enableQueryLogging);

            facets["tags"] = tags.ToList();
        }

        // Get collection facets if requested
        if (request.FacetFields.Contains("collections"))
        {
            var collectionsSql = $@"
                SELECT c.collection_name AS Value, COUNT(*) AS Count
                FROM content_items c
                {whereClause}
                GROUP BY c.collection_name
                ORDER BY Count DESC, c.collection_name";

            var collections = await Connection.QueryWithLoggingAsync<FacetValue>(
                new CommandDefinition(collectionsSql, parameters, cancellationToken: ct),
                _logger,
                _enableQueryLogging);

            facets["collections"] = collections.ToList();
        }

        // Get section facets if requested
        if (request.FacetFields.Contains("sections"))
        {
            // Use UNION ALL to count each section from bitmask column
            var sectionsSql = $@"
                SELECT 'ai' AS Value, COUNT(*) AS Count FROM content_items c {whereClause} AND (c.sections_bitmask & 1) > 0
                UNION ALL
                SELECT 'azure', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 2) > 0
                UNION ALL
                SELECT 'dotnet', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 4) > 0
                UNION ALL
                SELECT 'devops', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 8) > 0
                UNION ALL
                SELECT 'github-copilot', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 16) > 0
                UNION ALL
                SELECT 'ml', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 32) > 0
                UNION ALL
                SELECT 'security', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 64) > 0
                ORDER BY Count DESC, Value";

            var sections = await Connection.QueryWithLoggingAsync<FacetValue>(
                new CommandDefinition(sectionsSql, parameters, cancellationToken: ct),
                _logger,
                _enableQueryLogging);

            facets["sections"] = sections.Where(s => s.Count > 0).ToList();
        }

        return new FacetResults
        {
            Facets = facets,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Get tag counts with aggregation from content_tags_expanded table.
    /// When TagsToCount is provided: returns counts for those specific tags (including structural tags
    /// so users can see and deselect them) PLUS fills remaining slots (up to MaxTags) with popular tags
    /// (which DO exclude structural tags).
    /// When TagsToCount is empty: returns top MaxTags popular tags (standard tag cloud).
    /// </summary>
    protected async Task<IReadOnlyList<TagWithCount>> GetTagCountsInternalAsync(
        TagCountsRequest request,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        var (filterClause, parameters) = BuildTagCountFilters(request, Dialect);
        var excludeSet = await GetExcludeTagsSetAsync();

        // Pass all tagsToCount through without filtering against excludeSet.
        // Selected tags must always appear in the tag cloud so users can deselect them,
        // even if they match section/collection titles (e.g., "News" on /all/news?tags=news).
        // The excludeSet is still applied to the popular fill portion of the query.
        List<string>? filteredTagsToCount = null;
        if (request.TagsToCount is { Count: > 0 })
        {
            filteredTagsToCount = request.TagsToCount
                .Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            if (filteredTagsToCount.Count == 0)
            {
                filteredTagsToCount = null;
            }
        }

        return await ExecuteTagCountQueryAsync(request, filteredTagsToCount, excludeSet, filterClause, parameters, ct);
    }

    /// <summary>
    /// Build WHERE clause filters for tag count queries.
    /// Shared by both TagsToCount and TopN query paths.
    /// Requires ISqlDialect for database-specific full-text search syntax.
    /// </summary>
    private static (string filterClause, DynamicParameters parameters) BuildTagCountFilters(TagCountsRequest request, ISqlDialect dialect)
    {
        var parameters = new DynamicParameters();
        var filters = new List<string>();

        // Section filter (bitmask)
        if (!string.IsNullOrWhiteSpace(request.SectionName) &&
            !request.SectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            var bitmask = CalculateSectionBitmask(request.SectionName);
            if (bitmask > 0)
            {
                filters.Add($"(sections_bitmask & {bitmask}) > 0");
            }
        }

        // Collection filter
        if (!string.IsNullOrWhiteSpace(request.CollectionName) &&
            !request.CollectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            filters.Add("collection_name = @collectionName");
            parameters.Add("collectionName", request.CollectionName.ToLowerInvariant());
        }

        // Date range filters
        if (request.DateFrom.HasValue)
        {
            filters.Add("date_epoch >= @dateFrom");
            parameters.Add("dateFrom", request.DateFrom.Value.ToUnixTimeSeconds());
        }

        if (request.DateTo.HasValue)
        {
            filters.Add("date_epoch <= @dateTo");
            parameters.Add("dateTo", request.DateTo.Value.ToUnixTimeSeconds());
        }

        // Tag intersection filter (for dynamic counts when tags are selected)
        if (request.Tags != null && request.Tags.Count > 0)
        {
            var tagParams = string.Join(", ", request.Tags.Select((_, i) => $"@filterTag{i}"));
            for (int i = 0; i < request.Tags.Count; i++)
            {
                parameters.Add($"filterTag{i}", request.Tags[i].ToLowerInvariant().Trim());
            }

            filters.Add($@"(collection_name, slug) IN (
                SELECT collection_name, slug FROM content_tags_expanded
                WHERE tag_word IN ({tagParams})
                GROUP BY collection_name, slug
                HAVING COUNT(*) = {request.Tags.Count})");
        }

        // Full-text search filter (restricts to content items matching search query)
        if (!string.IsNullOrWhiteSpace(request.SearchQuery) && dialect.SupportsFullTextSearch)
        {
            var ftsJoin = dialect.GetFullTextJoinClause();
            var ftsJoinClause = string.IsNullOrEmpty(ftsJoin) ? "" : $" {ftsJoin}";
            var ftsWhere = dialect.GetFullTextWhereClause("searchQuery");

            filters.Add($@"(collection_name, slug) IN (
                SELECT c.collection_name, c.slug FROM content_items c{ftsJoinClause}
                WHERE {ftsWhere})");
            parameters.Add("searchQuery", dialect.TransformFullTextQuery(request.SearchQuery!));
        }

        var filterClause = filters.Count > 0 ? string.Join(" AND ", filters) : "";
        return (filterClause, parameters);
    }

    /// <summary>
    /// Unified tag count query. Returns proper display names via MAX(tag_display).
    /// Filters ALL excluded tags (sections, collections, high-frequency) in SQL.
    ///
    /// When tagsToCount is provided: UNION of specific tag counts + popular tags filling to MaxTags.
    /// When tagsToCount is null: standard top-N popular tags.
    ///
    /// Query optimization: single-pass GROUP BY with filtering on is_full_tag.
    /// - tag_display is stored for ALL rows (full tags and word expansions) with original casing.
    /// - MAX(tag_display) picks the best display name (full-tag display wins over word expansion).
    /// - For popular tags: WHERE is_full_tag = true ensures only real tags appear.
    /// - For tagsToCount: all rows are counted (word-level matching), display name always available.
    /// - PK constraint (collection_name, slug, tag_word) guarantees each item is counted once.
    /// </summary>
    private async Task<IReadOnlyList<TagWithCount>> ExecuteTagCountQueryAsync(
        TagCountsRequest request,
        List<string>? tagsToCount,
        HashSet<string> excludeSet,
        string filterClause,
        DynamicParameters parameters,
        CancellationToken ct)
    {
        var filterSql = string.IsNullOrEmpty(filterClause) ? "" : $"AND {filterClause}";

        if (tagsToCount != null && tagsToCount.Count > 0)
        {
            // --- UNION: specific tags + popular fill ---

            // Build params for specific tags
            var tcParams = string.Join(", ", tagsToCount.Select((_, i) => $"@tcTag{i}"));
            for (int i = 0; i < tagsToCount.Count; i++)
            {
                parameters.Add($"tcTag{i}", tagsToCount[i].ToLowerInvariant());
            }

            // Build NOT IN params: excludeSet + tagsToCount (to prevent duplicates in UNION)
            var allExcluded = new HashSet<string>(excludeSet, StringComparer.OrdinalIgnoreCase);
            foreach (var tag in tagsToCount)
            {
                allExcluded.Add(tag);
            }

            var excludeParams = string.Join(", ", allExcluded.Select((_, i) => $"@excl{i}"));
            int idx = 0;
            foreach (var tag in allExcluded)
            {
                parameters.Add($"excl{idx}", tag.ToLowerInvariant());
                idx++;
            }

            // Calculate how many popular tags to add
            var fillLimit = Math.Max(0, request.MaxTags - tagsToCount.Count);

            // Build HAVING for popular tags part:
            // - Only show tags that exist as full tags (not just word expansions)
            // - COUNT(*) still counts ALL matching rows (full + expansion) for accurate click-through counts
            var popularHavingParts = new List<string>
            {
                "MAX(CASE WHEN is_full_tag THEN 1 ELSE 0 END) = 1"
            };
            if (request.MinUses > 1)
            {
                popularHavingParts.Add($"COUNT(*) >= {request.MinUses}");
            }

            var popularHaving = $"HAVING {string.Join(" AND ", popularHavingParts)}";

            string sql;
            if (fillLimit > 0)
            {
                sql = $@"
                    (SELECT MAX(tag_display) AS Tag, COUNT(*) AS Count
                     FROM content_tags_expanded
                     WHERE tag_word IN ({tcParams})
                       {filterSql}
                     GROUP BY tag_word)
                    UNION ALL
                    (SELECT MAX(tag_display) AS Tag, COUNT(*) AS Count
                     FROM content_tags_expanded
                     WHERE tag_word NOT IN ({excludeParams})
                       {filterSql}
                     GROUP BY tag_word
                     {popularHaving}
                     ORDER BY Count DESC, Tag
                     LIMIT {fillLimit})";
            }
            else
            {
                // All slots used by tagsToCount — no room for popular tags
                sql = $@"
                    SELECT MAX(tag_display) AS Tag, COUNT(*) AS Count
                    FROM content_tags_expanded
                    WHERE tag_word IN ({tcParams})
                      {filterSql}
                    GROUP BY tag_word";
            }

            var results = (await Connection.QueryWithLoggingAsync<TagWithCount>(
                new CommandDefinition(sql, parameters, cancellationToken: ct),
                _logger,
                _enableQueryLogging)).ToList();

            // Add zero-count for tagsToCount tags not found in results (zero matches in current filters)
            var resultTagNames = new HashSet<string>(results.Select(r => r.Tag), StringComparer.OrdinalIgnoreCase);
            foreach (var tag in tagsToCount)
            {
                if (!resultTagNames.Contains(tag))
                {
                    results.Add(new TagWithCount { Tag = tag, Count = 0 });
                }
            }

            return results.OrderByDescending(t => t.Count).ThenBy(t => t.Tag).ToList();
        }
        else
        {
            // --- Standard top-N popular tags ---

            var excludeParams = string.Join(", ", excludeSet.Select((_, i) => $"@excl{i}"));
            int idx = 0;
            foreach (var tag in excludeSet)
            {
                parameters.Add($"excl{idx}", tag.ToLowerInvariant());
                idx++;
            }

            // Only show tags that exist as full tags (not just word expansions),
            // but COUNT(*) counts ALL matching rows for accurate click-through counts
            var havingConditions = new List<string>
            {
                "MAX(CASE WHEN is_full_tag THEN 1 ELSE 0 END) = 1"
            };
            if (request.MinUses > 1)
            {
                havingConditions.Add($"COUNT(*) >= {request.MinUses}");
            }

            var havingClause = $"HAVING {string.Join(" AND ", havingConditions)}";

            var sql = $@"
                SELECT MAX(tag_display) AS Tag, COUNT(*) AS Count
                FROM content_tags_expanded
                WHERE tag_word NOT IN ({excludeParams})
                  {filterSql}
                GROUP BY tag_word
                {havingClause}
                ORDER BY Count DESC, Tag
                LIMIT {request.MaxTags}";

            var results = await Connection.QueryWithLoggingAsync<TagWithCount>(
                new CommandDefinition(sql, parameters, cancellationToken: ct),
                _logger,
                _enableQueryLogging);

            return results.ToList();
        }
    }

    /// <summary>
    /// Build WHERE clauses for filtering content items.
    /// Reusable helper for facets, search, and other queries.
    /// </summary>
    protected List<string> BuildFilterWhereClauses(FacetRequest request, DynamicParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(parameters);

        var whereClauses = new List<string> { $"c.draft = {Dialect.GetBooleanLiteral(false)}" };

        if (request.Tags != null && request.Tags.Count > 0)
        {
            for (int i = 0; i < request.Tags.Count; i++)
            {
                whereClauses.Add($@"
                    EXISTS (
                        SELECT 1 FROM content_tags_expanded cte{i}
                        WHERE cte{i}.slug = c.slug
                        AND cte{i}.collection_name = c.collection_name
                        AND cte{i}.tag_word = @tag{i}
                    )");
                parameters.Add($"tag{i}", request.Tags[i].ToLowerInvariant().Trim());
            }
        }

        if (request.Sections != null && request.Sections.Count > 0)
        {
            var sectionBitmask = CalculateSectionBitmask(request.Sections);
            if (sectionBitmask > 0)
            {
                whereClauses.Add($"(c.sections_bitmask & {sectionBitmask}) > 0");
            }
        }

        if (request.Collections != null && request.Collections.Count > 0)
        {
            // Optimization: Use equality for single collection, dialect-specific clause for multiple
            if (request.Collections.Count == 1)
            {
                whereClauses.Add("c.collection_name = @collection");
                parameters.Add("collection", request.Collections[0].ToLowerInvariant().Trim());
            }
            else
            {
                whereClauses.Add($"c.collection_name {Dialect.GetListFilterClause("collections", request.Collections.Count)}");
                var normalizedCollections = request.Collections.Select(c => c.ToLowerInvariant().Trim());
                parameters.Add("collections", Dialect.ConvertListParameter(normalizedCollections));
            }
        }

        if (request.DateFrom.HasValue)
        {
            whereClauses.Add("c.date_epoch >= @fromDate");
            parameters.Add("fromDate", ((DateTimeOffset)request.DateFrom.Value).ToUnixTimeSeconds());
        }

        if (request.DateTo.HasValue)
        {
            whereClauses.Add("c.date_epoch <= @toDate");
            parameters.Add("toDate", ((DateTimeOffset)request.DateTo.Value).ToUnixTimeSeconds());
        }

        return whereClauses;
    }

    /// <summary>
    /// Build optimized subquery for tag filtering using content_tags_expanded.
    /// Uses GROUP BY + HAVING COUNT(*) = N for multi-tag AND logic.
    /// Returns (collection_name, slug) pairs.
    /// </summary>
    protected string BuildTagsTableQuery(SearchRequest request, DynamicParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(parameters);

        var sql = new StringBuilder("SELECT collection_name, slug FROM content_tags_expanded");

        var whereClauses = new List<string>();

        // Tag filtering - exact match on tag_word (no splitting)
        // If user searches "azure ai foundry", we look for exact match "azure ai foundry" in tag_word
        // If user searches "ai", we look for exact match "ai" in tag_word
        // Tags are pre-split during storage, so this provides word-level matching
        // tag_word is stored lowercase, so direct comparison is efficient
        var normalizedTags = request.Tags!.Select(t => t.ToLowerInvariant().Trim());
        whereClauses.Add($"tag_word {Dialect.GetListFilterClause("tags", request.Tags.Count)}");
        parameters.Add("tags", Dialect.ConvertListParameter(normalizedTags));

        // Section filtering using bitmask ("all" means no filter)
        if (request.Sections != null && request.Sections.Count > 0 &&
            !request.Sections.Any(s => s.Equals("all", StringComparison.OrdinalIgnoreCase)))
        {
            var sectionBitmask = CalculateSectionBitmask(request.Sections);
            if (sectionBitmask > 0)
            {
                whereClauses.Add($"(sections_bitmask & {sectionBitmask}) > 0");
            }
        }

        // Collection filtering ("all" means no filter)
        if (request.Collections != null && request.Collections.Count > 0 &&
            !request.Collections.Any(c => c.Equals("all", StringComparison.OrdinalIgnoreCase)))
        {
            // Optimization: Use equality for single collection, dialect-specific clause for multiple
            if (request.Collections.Count == 1)
            {
                whereClauses.Add("collection_name = @collection");
                parameters.Add("collection", request.Collections[0].ToLowerInvariant().Trim());
            }
            else
            {
                whereClauses.Add($"collection_name {Dialect.GetListFilterClause("collections", request.Collections.Count)}");
                var normalizedCollections = request.Collections.Select(c => c.ToLowerInvariant().Trim());
                parameters.Add("collections", Dialect.ConvertListParameter(normalizedCollections));
            }
        }

        // Date filtering
        if (request.DateFrom.HasValue)
        {
            whereClauses.Add("date_epoch >= @fromDate");
            parameters.Add("fromDate", ((DateTimeOffset)request.DateFrom.Value).ToUnixTimeSeconds());
        }

        if (request.DateTo.HasValue)
        {
            whereClauses.Add("date_epoch <= @toDate");
            parameters.Add("toDate", ((DateTimeOffset)request.DateTo.Value).ToUnixTimeSeconds());
        }

        sql.Append(" WHERE ").Append(string.Join(" AND ", whereClauses));

        // GROUP BY to prevent duplicates when item matches multiple tags
        sql.Append(" GROUP BY collection_name, slug");

        // HAVING COUNT(*) = @tagCount ensures ALL tags must match (AND logic)
        // For single tag: COUNT = 1
        // For multiple tags (e.g., tags=ai,azure): COUNT = 2 (item must have both)
        sql.Append(" HAVING COUNT(*) = @tagCount");
        parameters.Add("tagCount", request.Tags!.Count);

        // PERFORMANCE OPTIMIZATION: Apply ORDER BY + LIMIT in subquery
        // Since content_tags_expanded has date_epoch, we can sort and limit HERE
        // This reduces outer query from processing 100s of matches to just the page size (e.g., 20)
        // Result: 40-50% faster queries (348ms → 199ms in benchmarks)
        //
        // CRITICAL: Only apply LIMIT here when NO search query is present.
        // When a search query IS present, the outer query applies an additional FTS filter
        // that further reduces results. If we LIMIT here, we may exclude items that would
        // match the FTS filter, causing the combined tags+search query to incorrectly
        // return zero results (the FTS-matching items get pruned before FTS runs).
        sql.Append(" ORDER BY MAX(date_epoch) DESC");
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            sql.Append(" LIMIT @take OFFSET @skip");
            // Note: @take and @skip are added by caller
        }

        return sql.ToString();
    }

    /// <summary>
    /// Full-text search implementation using database-specific FTS via ISqlDialect.
    /// Full-text search implementation using PostgreSQL tsvector through dialect abstraction.
    /// </summary>
    protected async Task<SearchResults<ContentItem>> SearchInternalAsync(SearchRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var sql = new StringBuilder();
        var parameters = new DynamicParameters();

        var hasQuery = !string.IsNullOrWhiteSpace(request.Query);
        var hasTags = request.Tags != null && request.Tags.Count > 0;
        var hasSections = request.Sections != null && request.Sections.Count > 0 &&
                          !request.Sections.Any(s => s.Equals("all", StringComparison.OrdinalIgnoreCase));
        var hasCollections = request.Collections != null && request.Collections.Count > 0 &&
                             !request.Collections.Any(c => c.Equals("all", StringComparison.OrdinalIgnoreCase));
        var hasAuthor = !string.IsNullOrWhiteSpace(request.Author);

        // OPTIMIZATION: When filtering by tags, pre-filter using tags table
        // This reduces FTS search from 4000+ items to potentially just 10-20
        if (hasTags)
        {
            // PERFORMANCE: Add take/skip BEFORE building subquery so they're applied there
            parameters.Add("take", request.Take);
            parameters.Add("skip", request.Skip);

            var tagsQuery = BuildTagsTableQuery(request, parameters);

            sql.Append($@"
            SELECT {ListViewColumns}
            FROM content_items c");

            // Join with FTS if query is provided (now operating on pre-filtered subset!)
            if (hasQuery)
            {
                var ftsJoin = Dialect.GetFullTextJoinClause();
                if (!string.IsNullOrEmpty(ftsJoin))
                {
                    sql.Append(CultureInfo.InvariantCulture, $@"
            {ftsJoin}");
                }
            }

            sql.Append(CultureInfo.InvariantCulture, $@"
            WHERE (c.collection_name, c.slug) IN (
                {tagsQuery}
            )
            AND c.draft = {(request.IncludeDraft ? $"{Dialect.GetBooleanLiteral(false)} OR c.draft = {Dialect.GetBooleanLiteral(true)}" : Dialect.GetBooleanLiteral(false))}");

            if (!string.IsNullOrWhiteSpace(request.Subcollection) &&
                !request.Subcollection.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                sql.Append(" AND c.subcollection_name = @subcollection");
                parameters.Add("subcollection", request.Subcollection.ToLowerInvariant());
            }

            if (hasAuthor)
            {
                sql.Append(" AND c.author = @author");
                parameters.Add("author", request.Author);
            }

            if (hasQuery)
            {
                sql.Append(CultureInfo.InvariantCulture, $@"
            AND {Dialect.GetFullTextWhereClause("query")}");
                parameters.Add("query", Dialect.TransformFullTextQuery(request.Query!));
                sql.Append(CultureInfo.InvariantCulture, $@"
            ORDER BY {Dialect.GetFullTextOrderByClause("query")}, c.date_epoch DESC");
                // LIMIT is applied here because the tag subquery skips it when search is present
                // (to avoid pruning items before FTS filter runs)
                sql.Append(" LIMIT @take OFFSET @skip");
            }
            else
            {
                sql.Append(" ORDER BY c.date_epoch DESC");
            }

            // Note: When no search query, LIMIT is already applied in tag subquery for better performance
        }
        else
        {
            // Non-tags path: standard query from content_items
            sql.Append($@"
            SELECT {ListViewColumns}
            FROM content_items c");

            if (hasQuery)
            {
                var ftsJoin = Dialect.GetFullTextJoinClause();
                if (!string.IsNullOrEmpty(ftsJoin))
                {
                    sql.Append(CultureInfo.InvariantCulture, $@"
            {ftsJoin}");
                }
            }

            // Build WHERE clause
            var whereClauses = new List<string>();

            if (!request.IncludeDraft)
            {
                whereClauses.Add($"c.draft = {Dialect.GetBooleanLiteral(false)}");
            }

            if (hasQuery)
            {
                whereClauses.Add(Dialect.GetFullTextWhereClause("query"));
                parameters.Add("query", Dialect.TransformFullTextQuery(request.Query!));
            }

            if (hasSections)
            {
                var sectionBitmask = CalculateSectionBitmask(request.Sections!);
                if (sectionBitmask > 0)
                {
                    whereClauses.Add($"(c.sections_bitmask & {sectionBitmask}) > 0");
                }
            }

            if (hasCollections)
            {
                // Optimization: Use equality for single collection, dialect-specific clause for multiple
                if (request.Collections!.Count == 1)
                {
                    whereClauses.Add("c.collection_name = @collection");
                    parameters.Add("collection", request.Collections[0].ToLowerInvariant().Trim());
                }
                else
                {
                    whereClauses.Add($"c.collection_name {Dialect.GetListFilterClause("collections", request.Collections.Count)}");
                    var normalizedCollections = request.Collections.Select(c => c.ToLowerInvariant().Trim());
                    parameters.Add("collections", Dialect.ConvertListParameter(normalizedCollections));
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Subcollection) &&
                !request.Subcollection.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                whereClauses.Add("c.subcollection_name = @subcollection");
                parameters.Add("subcollection", request.Subcollection.ToLowerInvariant());
            }

            if (hasAuthor)
            {
                whereClauses.Add("c.author = @author");
                parameters.Add("author", request.Author);
            }

            if (request.DateFrom.HasValue)
            {
                whereClauses.Add("c.date_epoch >= @fromDate");
                parameters.Add("fromDate", ((DateTimeOffset)request.DateFrom.Value).ToUnixTimeSeconds());
            }

            if (request.DateTo.HasValue)
            {
                whereClauses.Add("c.date_epoch <= @toDate");
                parameters.Add("toDate", ((DateTimeOffset)request.DateTo.Value).ToUnixTimeSeconds());
            }

            sql.Append(" WHERE ").Append(string.Join(" AND ", whereClauses));

            // When a search query is provided, order by relevance (ts_rank) first, then date
            // This ensures title matches rank higher than content-only matches
            if (hasQuery)
            {
                sql.Append(CultureInfo.InvariantCulture, $" ORDER BY {Dialect.GetFullTextOrderByClause("query")}, c.date_epoch DESC");
            }
            else
            {
                sql.Append(" ORDER BY c.date_epoch DESC");
            }

            sql.Append(" LIMIT @take OFFSET @skip");
            parameters.Add("take", request.Take);
            parameters.Add("skip", request.Skip);
        }

        // Get items and count in a single round-trip
        var combinedSql = sql.ToString() + ";\n" + BuildCountSql(request, parameters);

        using var multi = await Connection.QueryMultipleWithLoggingAsync(
            new CommandDefinition(combinedSql, parameters, cancellationToken: ct),
            _logger,
            _enableQueryLogging);

        var items = await multi.ReadAsync<ContentItem>();
        var totalCount = await multi.ReadSingleAsync<int>();

        // Compute facets if requested
        FacetResults? facets = null;
        if (request.IncludeFacets)
        {
            facets = await GetFacetsAsync(new FacetRequest(
                facetFields: _searchFacetFields,
                tags: request.Tags!,
                sections: request.Sections!,
                collections: request.Collections!,
                dateFrom: request.DateFrom,
                dateTo: request.DateTo
            ), ct);
        }

        return new SearchResults<ContentItem>
        {
            Items = items.ToList(),
            TotalCount = totalCount,
            Facets = facets,
            ContinuationToken = null
        };
    }

    /// <summary>
    /// Build SQL for counting total results using database-specific FTS via ISqlDialect.
    /// Build SQL for counting total results using PostgreSQL tsvector for FTS.
    /// </summary>
    private string BuildCountSql(SearchRequest request, DynamicParameters parameters)
    {
        var hasTags = request.Tags != null && request.Tags.Count > 0;
        var hasSections = request.Sections != null && request.Sections.Count > 0;
        var hasQuery = !string.IsNullOrWhiteSpace(request.Query);

        if (hasTags)
        {
            var sql = new StringBuilder();

            // PostgreSQL INTERSECT approach for tag filtering
            sql.Append(@"
                SELECT COUNT(DISTINCT c.slug)
                FROM content_items c
                WHERE (c.collection_name, c.slug) IN (
                    SELECT DISTINCT collection_name, slug
                    FROM (");

            for (int i = 0; i < request.Tags!.Count; i++)
            {
                if (i > 0)
                {
                    sql.Append(" INTERSECT ");
                }

                var paramName = $"tag{i}";
                parameters.Add(paramName, request.Tags[i].ToLowerInvariant().Trim());

                sql.Append(@"
                        SELECT collection_name, slug 
                        FROM content_tags_expanded 
                        WHERE tag_word = @").Append(paramName);
            }

            sql.Append(@"
                    ) AS tag_results
                )
                AND c.draft = ").Append(request.IncludeDraft
                    ? $"{Dialect.GetBooleanLiteral(false)} OR c.draft = {Dialect.GetBooleanLiteral(true)}"
                    : Dialect.GetBooleanLiteral(false).ToString());

            if (hasQuery)
            {
                sql.Append(@"
                AND ").Append(Dialect.GetFullTextWhereClause("query"));
            }

            // Apply section/collection/date/subcollection filters
            AppendContentItemFilters(sql, request, hasSections);

            return sql.ToString();
        }

        // Standard count query
        var countSql = new StringBuilder($"SELECT COUNT(*) FROM content_items c");

        if (hasQuery)
        {
            var ftsJoin = Dialect.GetFullTextJoinClause();
            if (!string.IsNullOrEmpty(ftsJoin))
            {
                countSql.Append(CultureInfo.InvariantCulture, $" {ftsJoin}");
            }
        }

        var whereClauses = new List<string>
        {
            request.IncludeDraft
                ? $"(c.draft = {Dialect.GetBooleanLiteral(false)} OR c.draft = {Dialect.GetBooleanLiteral(true)})"
                : $"c.draft = {Dialect.GetBooleanLiteral(false)}"
        };

        if (hasQuery)
        {
            whereClauses.Add(Dialect.GetFullTextWhereClause("query"));
        }

        if (hasSections)
        {
            var sectionBitmask = CalculateSectionBitmask(request.Sections!);
            if (sectionBitmask > 0)
            {
                whereClauses.Add($"(c.sections_bitmask & {sectionBitmask}) > 0");
            }
        }

        if (request.Collections != null && request.Collections.Count > 0 &&
            !request.Collections.Any(c => c.Equals("all", StringComparison.OrdinalIgnoreCase)))
        {
            // Match parameter naming from BuildTagsTableQuery and SearchInternalAsync
            if (request.Collections.Count == 1)
            {
                whereClauses.Add("c.collection_name = @collection");
            }
            else
            {
                whereClauses.Add($"c.collection_name {Dialect.GetListFilterClause("collections", request.Collections.Count)}");
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Subcollection) &&
            !request.Subcollection.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            whereClauses.Add("c.subcollection_name = @subcollection");
        }

        if (!string.IsNullOrWhiteSpace(request.Author))
        {
            whereClauses.Add("c.author = @author");
        }

        if (request.DateFrom.HasValue)
        {
            whereClauses.Add("c.date_epoch >= @fromDate");
        }

        if (request.DateTo.HasValue)
        {
            whereClauses.Add("c.date_epoch <= @toDate");
        }

        countSql.Append(" WHERE ").Append(string.Join(" AND ", whereClauses));

        return countSql.ToString();
    }

    /// <summary>
    /// Append section, collection, and date filters to a count query operating on content_items c.
    /// Used by PostgreSQL INTERSECT count paths that query content_items directly
    /// (parameters are already populated by BuildTagsTableQuery in the main query).
    /// </summary>
    private static void AppendContentItemFilters(StringBuilder sql, SearchRequest request, bool hasSections)
    {
        if (hasSections)
        {
            var sectionBitmask = CalculateSectionBitmask(request.Sections!);
            if (sectionBitmask > 0)
            {
                sql.Append(CultureInfo.InvariantCulture, $" AND (c.sections_bitmask & {sectionBitmask}) > 0");
            }
        }

        if (request.Collections != null && request.Collections.Count > 0 &&
            !request.Collections.Any(c => c.Equals("all", StringComparison.OrdinalIgnoreCase)))
        {
            if (request.Collections.Count == 1)
            {
                sql.Append(" AND c.collection_name = @collection");
            }
            else
            {
                // Note: Uses same parameter names as BuildTagsTableQuery
                sql.Append(" AND c.collection_name = ANY(@collections)");
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Subcollection) &&
            !request.Subcollection.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            sql.Append(" AND c.subcollection_name = @subcollection");
        }

        if (!string.IsNullOrWhiteSpace(request.Author))
        {
            sql.Append(" AND c.author = @author");
        }

        if (request.DateFrom.HasValue)
        {
            sql.Append(" AND c.date_epoch >= @fromDate");
        }

        if (request.DateTo.HasValue)
        {
            sql.Append(" AND c.date_epoch <= @toDate");
        }
    }

    // ==================== Admin Methods ====================

    /// <inheritdoc/>
    public async Task<TechHub.Core.Models.Admin.ContentItemAiMetadataResult?> GetAiMetadataAsync(
        string collectionName,
        string slug,
        CancellationToken ct = default)
    {
        const string Sql = @"
SELECT collection_name AS CollectionName, slug AS Slug, ai_metadata::text AS AiMetadata
FROM content_items
WHERE collection_name = @CollectionName AND slug = @Slug
LIMIT 1";

        return await Connection.QueryFirstOrDefaultAsync<TechHub.Core.Models.Admin.ContentItemAiMetadataResult>(
            new CommandDefinition(Sql, new { CollectionName = collectionName, Slug = slug }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAiMetadataAsync(
        string collectionName,
        string slug,
        string aiMetadata,
        CancellationToken ct = default)
    {
        const string Sql = @"
UPDATE content_items
SET ai_metadata = @AiMetadata::jsonb, updated_at = NOW()
WHERE collection_name = @CollectionName AND slug = @Slug";

        var rows = await Connection.ExecuteAsync(
            new CommandDefinition(Sql, new { CollectionName = collectionName, Slug = slug, AiMetadata = aiMetadata }, cancellationToken: ct));
        return rows > 0;
    }

    /// <inheritdoc/>
    public async Task<TechHub.Core.Models.Admin.ContentItemEditData?> GetEditDataAsync(
        string collectionName,
        string slug,
        CancellationToken ct = default)
    {
        const string Sql = @"
SELECT collection_name, slug, date_epoch, title, author, excerpt, content, primary_section_name,
       feed_name, tags_csv, ai_metadata::text AS ai_metadata,
       is_ai, is_azure, is_dotnet, is_devops, is_github_copilot, is_ml, is_security
FROM content_items
WHERE collection_name = @CollectionName AND slug = @Slug
LIMIT 1";

        var row = await Connection.QueryFirstOrDefaultAsync(
            new CommandDefinition(Sql, new { CollectionName = collectionName, Slug = slug }, cancellationToken: ct));

        if (row is null)
        {
            return null;
        }

        var sections = new List<string>();
        if ((bool)row.is_ai)
        {
            sections.Add("ai");
        }

        if ((bool)row.is_azure)
        {
            sections.Add("azure");
        }

        if ((bool)row.is_dotnet)
        {
            sections.Add("dotnet");
        }

        if ((bool)row.is_devops)
        {
            sections.Add("devops");
        }

        if ((bool)row.is_github_copilot)
        {
            sections.Add("github-copilot");
        }

        if ((bool)row.is_ml)
        {
            sections.Add("ml");
        }

        if ((bool)row.is_security)
        {
            sections.Add("security");
        }

        var tagsCsv = (string)row.tags_csv;
        var tags = string.IsNullOrWhiteSpace(tagsCsv)
            ? []
            : tagsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return new TechHub.Core.Models.Admin.ContentItemEditData
        {
            CollectionName = (string)row.collection_name,
            Slug = (string)row.slug,
            DateEpoch = (long)row.date_epoch,
            Title = (string)row.title,
            Author = (string)row.author,
            Excerpt = (string)row.excerpt,
            Content = (string)row.content,
            PrimarySectionName = (string)row.primary_section_name,
            FeedName = (string?)row.feed_name,
            Tags = tags,
            Sections = sections,
            AiMetadata = (string?)row.ai_metadata
        };
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateEditDataAsync(
        string collectionName,
        string slug,
        TechHub.Core.Models.Admin.ContentItemEditData editData,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(editData);

        var tagsCsv = editData.Tags.Count > 0
            ? $",{string.Join(",", editData.Tags)},"
            : string.Empty;

        var isAi = editData.Sections.Contains("ai");
        var isAzure = editData.Sections.Contains("azure");
        var isDotnet = editData.Sections.Contains("dotnet");
        var isDevops = editData.Sections.Contains("devops");
        var isGhc = editData.Sections.Contains("github-copilot");
        var isMl = editData.Sections.Contains("ml");
        var isSecurity = editData.Sections.Contains("security");
        var bitmask = CalculateSectionBitmask(editData.Sections);

        const string ItemSql = @"
UPDATE content_items
SET title                = @Title,
    author               = @Author,
    excerpt              = @Excerpt,
    content              = @Content,
    date_epoch           = @DateEpoch,
    primary_section_name = @PrimarySectionName,
    feed_name            = COALESCE(@FeedName, feed_name),
    tags_csv             = @TagsCsv,
    is_ai                = @IsAi,
    is_azure             = @IsAzure,
    is_dotnet            = @IsDotnet,
    is_devops            = @IsDevops,
    is_github_copilot    = @IsGhc,
    is_ml                = @IsMl,
    is_security          = @IsSecurity,
    sections_bitmask     = @Bitmask,
    ai_metadata          = @AiMetadata::jsonb,
    updated_at           = NOW()
WHERE collection_name = @CollectionName AND slug = @Slug";

        var parameters = new
        {
            Title = editData.Title,
            Author = editData.Author,
            Excerpt = editData.Excerpt,
            Content = editData.Content,
            DateEpoch = editData.DateEpoch,
            PrimarySectionName = editData.PrimarySectionName,
            FeedName = editData.FeedName,
            TagsCsv = tagsCsv,
            IsAi = isAi,
            IsAzure = isAzure,
            IsDotnet = isDotnet,
            IsDevops = isDevops,
            IsGhc = isGhc,
            IsMl = isMl,
            IsSecurity = isSecurity,
            Bitmask = bitmask,
            AiMetadata = editData.AiMetadata,
            CollectionName = collectionName,
            Slug = slug
        };

        using var transaction = Connection.BeginTransaction();
        try
        {
            var rows = await Connection.ExecuteAsync(
                new CommandDefinition(ItemSql, parameters, transaction: transaction, cancellationToken: ct));

            if (rows > 0)
            {
                // Sync all denormalized fields in content_tags_expanded atomically.
                // Admin edits can change date_epoch, section booleans, and sections_bitmask —
                // all are denormalized here and must stay in sync with content_items.
                await Connection.ExecuteAsync(
                    new CommandDefinition(
                        @"UPDATE content_tags_expanded
                          SET date_epoch        = @DateEpoch,
                              is_ai             = @IsAi,
                              is_azure          = @IsAzure,
                              is_dotnet         = @IsDotnet,
                              is_devops         = @IsDevops,
                              is_github_copilot = @IsGhc,
                              is_ml             = @IsMl,
                              is_security       = @IsSecurity,
                              sections_bitmask  = @Bitmask
                          WHERE collection_name = @CollectionName AND slug = @Slug",
                        parameters,
                        transaction: transaction,
                        cancellationToken: ct));

                // Keep processed_urls.feed_name in sync with content_items.feed_name
                // so the admin processed URLs listing reflects the updated feed.
                await Connection.ExecuteAsync(
                    new CommandDefinition(
                        @"UPDATE processed_urls
                          SET feed_name  = COALESCE(@FeedName, feed_name),
                              updated_at = NOW()
                          WHERE collection_name = @CollectionName AND slug = @Slug",
                        parameters,
                        transaction: transaction,
                        cancellationToken: ct));
            }

            transaction.Commit();
            return rows > 0;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // ==================== Admin Content Items ====================

    /// <inheritdoc/>
    public async Task<PagedResult<TechHub.Core.Models.Admin.ContentItemListItem>> GetContentItemsPagedAsync(
        int offset,
        int limit,
        string? search = null,
        string? collectionName = null,
        string? feedName = null,
        string? subcollectionName = null,
        CancellationToken ct = default)
    {
        var whereClauses = new List<string>();
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(search))
        {
            whereClauses.Add("(ci.title ILIKE @Search OR ci.slug ILIKE @Search OR ci.external_url ILIKE @Search)");
            parameters.Add("Search", $"%{search}%");
        }

        if (!string.IsNullOrWhiteSpace(collectionName))
        {
            whereClauses.Add("ci.collection_name = @CollectionName");
            parameters.Add("CollectionName", collectionName);
        }

        if (!string.IsNullOrWhiteSpace(feedName))
        {
            whereClauses.Add("ci.feed_name = @FeedName");
            parameters.Add("FeedName", feedName);
        }

        if (!string.IsNullOrWhiteSpace(subcollectionName))
        {
            whereClauses.Add("ci.subcollection_name = @SubcollectionName");
            parameters.Add("SubcollectionName", subcollectionName);
        }

        var whereStr = whereClauses.Count > 0
            ? "WHERE " + string.Join(" AND ", whereClauses)
            : string.Empty;

        var countSql = $"SELECT COUNT(*) FROM content_items ci {whereStr}";
        var totalCount = await Connection.ExecuteScalarAsync<int>(
            new CommandDefinition(countSql, parameters, cancellationToken: ct));

        var dataSql = $@"
SELECT ci.slug           AS Slug,
       ci.collection_name AS CollectionName,
       ci.title           AS Title,
       ci.author          AS Author,
       ci.feed_name       AS FeedName,
       ci.external_url    AS ExternalUrl,
       ci.primary_section_name AS PrimarySectionName,
       NULLIF(CONCAT_WS(', ',
           CASE WHEN ci.is_ai THEN 'ai' END,
           CASE WHEN ci.is_azure THEN 'azure' END,
           CASE WHEN ci.is_dotnet THEN 'dotnet' END,
           CASE WHEN ci.is_devops THEN 'devops' END,
           CASE WHEN ci.is_github_copilot THEN 'github-copilot' END,
           CASE WHEN ci.is_ml THEN 'ml' END,
           CASE WHEN ci.is_security THEN 'security' END
       ), '') AS AllSections,
       ci.date_epoch      AS DateEpoch,
       ci.created_at      AS CreatedAt,
       (pu.external_url IS NOT NULL) AS HasProcessedUrl
FROM content_items ci
LEFT JOIN processed_urls pu
    ON pu.collection_name = ci.collection_name AND pu.slug = ci.slug
{whereStr}
ORDER BY ci.created_at DESC
LIMIT @Limit OFFSET @Offset";

        parameters.Add("Limit", limit);
        parameters.Add("Offset", offset);

        var items = await Connection.QueryAsync<TechHub.Core.Models.Admin.ContentItemListItem>(
            new CommandDefinition(dataSql, parameters, cancellationToken: ct));

        return new PagedResult<TechHub.Core.Models.Admin.ContentItemListItem>
        {
            Items = items.AsList(),
            TotalCount = totalCount
        };
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteContentItemAsync(string collectionName, string slug, CancellationToken ct = default)
    {
        const string Sql = "DELETE FROM content_items WHERE collection_name = @CollectionName AND slug = @Slug";
        var rows = await Connection.ExecuteAsync(
            new CommandDefinition(Sql, new { CollectionName = collectionName, Slug = slug }, cancellationToken: ct));
        return rows > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateGhcFeaturePlansAsync(
        string slug,
        IReadOnlyList<string> plans,
        bool ghesSupport,
        bool draft,
        CancellationToken ct = default)
    {
        var plansCsv = string.Join(",", plans);

        const string Sql = @"
UPDATE content_items
SET plans        = @Plans,
    ghes_support = @GhesSupport,
    draft        = @Draft,
    updated_at   = NOW()
WHERE slug = @Slug
  AND collection_name = 'videos'
  AND subcollection_name = 'ghc-features'";

        var rows = await Connection.ExecuteAsync(
            new CommandDefinition(Sql, new { Plans = plansCsv, GhesSupport = ghesSupport, Draft = draft, Slug = slug }, cancellationToken: ct));
        return rows > 0;
    }

    // ==================== Cache Invalidation ====================

    /// <inheritdoc/>
    public void InvalidateCachedData()
    {
        if (Cache is MemoryCache memoryCache)
        {
            memoryCache.Clear();
            _logger?.LogInformation("Content cache invalidated");
        }
    }

    // ==================== Legacy Redirect ====================

    private sealed record LegacySlugRow(string PrimarySectionName, string CollectionName, string Slug, string? ExternalUrl);

    /// <inheritdoc/>
    public async Task<LegacyRedirectResult?> FindByLegacySlugAsync(
        string slug,
        string? sectionHint = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(slug);

        // Normalize: lowercase and strip .html extension
        var normalizedSlug = slug.ToLowerInvariant().Trim('/');
        if (normalizedSlug.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
        {
            normalizedSlug = normalizedSlug[..^5];
        }

        // Also prepare a date-prefix-stripped variant (YYYY-MM-DD-slug → slug)
        var strippedSlug = System.Text.RegularExpressions.Regex.Replace(
            normalizedSlug,
            @"^\d{4}-\d{2}-\d{2}-",
            string.Empty);

        // Lowercase section hint for case-insensitive comparison.
        var normalizedHint = sectionHint?.ToLowerInvariant() ?? string.Empty;
        var hasSectionHint = !string.IsNullOrEmpty(normalizedHint);

        var sql = $"""
            SELECT
                primary_section_name AS PrimarySectionName,
                collection_name      AS CollectionName,
                slug                 AS Slug,
                external_url         AS ExternalUrl
            FROM content_items
            WHERE (slug = @NormalizedSlug OR slug = @StrippedSlug)
              AND draft = {Dialect.GetBooleanLiteral(false)}
            ORDER BY
                CASE WHEN @HasSectionHint AND LOWER(primary_section_name) = @SectionHint THEN 0 ELSE 1 END,
                date_epoch DESC
            LIMIT 1
            """;

        var row = await Connection.QueryFirstOrDefaultAsync<LegacySlugRow>(
            new CommandDefinition(
                sql,
                new
                {
                    NormalizedSlug = normalizedSlug,
                    StrippedSlug = strippedSlug,
                    HasSectionHint = hasSectionHint,
                    SectionHint = normalizedHint,
                },
                cancellationToken: ct));

        if (row == null)
        {
            return null;
        }

        // Items in externally-linking collections (news, blogs, community) don't have
        // their own detail page — redirect straight to the source URL when available.
        // Validate that the external URL is an absolute http/https URL to prevent open-redirect
        // or unsafe-scheme vulnerabilities (e.g. javascript: or malformed relative URLs).
        var isExternalCollection = row.CollectionName is "news" or "blogs" or "community";
        var hasValidExternalUrl = isExternalCollection
            && !string.IsNullOrEmpty(row.ExternalUrl)
            && Uri.TryCreate(row.ExternalUrl, UriKind.Absolute, out var parsedUri)
            && (parsedUri.Scheme == Uri.UriSchemeHttp || parsedUri.Scheme == Uri.UriSchemeHttps);

        // row.ExternalUrl is guaranteed non-null here because hasValidExternalUrl requires
        // !IsNullOrEmpty(row.ExternalUrl) (line above). The null-forgiving operator is safe.
        string redirectUrl;
        if (hasValidExternalUrl)
        {
            redirectUrl = row.ExternalUrl!;
        }
        else if (row.CollectionName == "roundups")
        {
            // Roundups are only accessible via /all/roundups/ — they don't exist under
            // individual section paths (mirrors ContentItem.GetHref() logic).
            redirectUrl = $"/all/roundups/{row.Slug}";
        }
        else
        {
            redirectUrl = $"/{row.PrimarySectionName}/{row.CollectionName}/{row.Slug}";
        }

        return new LegacyRedirectResult(redirectUrl);
    }
}
