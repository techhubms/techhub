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

    // High-frequency tags excluded from all tag clouds.
    // These appear on most content items and don't provide filtering value.
    private static readonly string[] HighFrequencyExcludedTags = ["github", "copilot", "microsoft"];

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
    /// Converts configuration to Section models and applies ordering.
    /// </summary>
    private static ReadOnlyCollection<Section> InitializeSections(AppSettings settings)
    {
        // Define section display order (matches live site - starts with "all")
        var sectionOrder = new[]
        {
            "all", "github-copilot", "ai", "ml", "devops", "azure", "dotnet", "security"
        };

        // Convert configuration to Section models
        var sectionsDict = settings.Content.Sections
            .Select(kvp => ConvertToSection(kvp.Key, kvp.Value))
            .ToDictionary(s => s.Name);

        // Order sections according to defined order, then any remaining alphabetically
        return sectionOrder
            .Where(name => sectionsDict.ContainsKey(name))
            .Select(name => sectionsDict[name])
            .Concat(sectionsDict.Values.Where(s => !sectionOrder.Contains(s.Name)).OrderBy(s => s.Title))
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

        return new Section(sectionName, config.Title, config.Description, config.Url, config.Tag, ordered);
    }

    /// <summary>
    /// Get all sections defined in configuration.
    /// Sections are loaded lazily and cached in memory.
    /// </summary>
    public async Task<IReadOnlyList<Section>> GetAllSectionsAsync(CancellationToken ct = default)
    {
        return await Cache.GetOrCreateAsync("sections:all", entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
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
        return sections.FirstOrDefault(s => s.Name == name);
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

        var cacheKey = $"slug:{collectionName}:{slug}:{includeDraft}";
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
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
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await SearchInternalAsync(request, ct);
        }) ?? new SearchResults<ContentItem>
        {
            Items = [],
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
            entry.SetPriority(CacheItemPriority.NeverRemove);
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
            entry.SetPriority(CacheItemPriority.NeverRemove);
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
            entry.SetPriority(CacheItemPriority.NeverRemove);
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
        var excludeSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var sections = await GetAllSectionsAsync();

        foreach (var section in sections)
        {
            // Add section tag from configuration (e.g., "AI" for ai section, "All" for all section)
            if (!string.IsNullOrWhiteSpace(section.Tag))
            {
                excludeSet.Add(section.Tag);
            }

            // Add collection tags generated from collection names
            foreach (var collection in section.Collections)
            {
                // Generate tag from collection name (e.g., "blogs" -> "Blogs", "community" -> "Community")
                var collectionTag = Collection.GetTagFromName(collection.Name);
                if (!string.IsNullOrWhiteSpace(collectionTag))
                {
                    excludeSet.Add(collectionTag);
                }
            }
        }

        // Add high-frequency tags (already defined in HighFrequencyExcludedTags array)
        foreach (var tag in HighFrequencyExcludedTags)
        {
            excludeSet.Add(tag);
        }

        return excludeSet;
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

            facets["tags"] = [.. tags];
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

            facets["collections"] = [.. collections];
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

            facets["sections"] = [.. sections.Where(s => s.Count > 0)];
        }

        return new FacetResults
        {
            Facets = facets,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Get tag counts with aggregation from content_tags_expanded table.
    /// When TagsToCount is provided: returns counts for those specific tags (excluding structural tags)
    /// PLUS fills remaining slots (up to MaxTags) with popular tags.
    /// When TagsToCount is empty: returns top MaxTags popular tags (standard tag cloud).
    /// </summary>
    protected async Task<IReadOnlyList<TagWithCount>> GetTagCountsInternalAsync(
        TagCountsRequest request,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        var (filterClause, parameters) = BuildTagCountFilters(request, Dialect);
        var excludeSet = await GetExcludeTagsSetAsync();

        // Filter tagsToCount against exclusion set (section/collection titles, high-frequency terms)
        List<string>? filteredTagsToCount = null;
        if (request.TagsToCount is { Count: > 0 })
        {
            filteredTagsToCount = [.. request.TagsToCount
                .Where(t => !excludeSet.Contains(t))
                .Distinct(StringComparer.OrdinalIgnoreCase)];

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
            parameters.Add("collectionName", request.CollectionName);
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

            return [.. results.OrderByDescending(t => t.Count).ThenBy(t => t.Tag)];
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

            return [.. results];
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
                    sql.Append($@"
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
                parameters.Add("subcollection", request.Subcollection);
            }

            if (hasQuery)
            {
                sql.Append($@"
            AND {Dialect.GetFullTextWhereClause("query")}");
                parameters.Add("query", Dialect.TransformFullTextQuery(request.Query!));
                sql.Append($" ORDER BY {Dialect.GetFullTextOrderByClause("query")}");
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
                    sql.Append($@"
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
                parameters.Add("subcollection", request.Subcollection);
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
            sql.Append(hasQuery ? $" ORDER BY {Dialect.GetFullTextOrderByClause("query")}" : " ORDER BY c.date_epoch DESC");
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
                facetFields: ["tags", "collections", "sections"],
                tags: request.Tags!,
                sections: request.Sections!,
                collections: request.Collections!,
                dateFrom: request.DateFrom,
                dateTo: request.DateTo
            ), ct);
        }

        return new SearchResults<ContentItem>
        {
            Items = [.. items],
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
                AND c.draft = ").Append(Dialect.GetBooleanLiteral(false));

            if (hasQuery)
            {
                sql.Append(@"
                AND ").Append(Dialect.GetFullTextWhereClause("query"));
            }

            // Apply section/collection/date filters
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
                countSql.Append($" {ftsJoin}");
            }
        }

        var whereClauses = new List<string> { $"c.draft = {Dialect.GetBooleanLiteral(false)}" };

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

        if (request.DateFrom.HasValue)
        {
            sql.Append(" AND c.date_epoch >= @fromDate");
        }

        if (request.DateTo.HasValue)
        {
            sql.Append(" AND c.date_epoch <= @toDate");
        }
    }
}
