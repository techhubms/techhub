using System.Data;
using System.Text.RegularExpressions;
using Dapper;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.Extensions.Logging;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Validates and repairs content items in the database.
/// Used both inline during content creation (per-item via <see cref="RepairMarkdown"/>)
/// and in bulk via admin triggers (<see cref="FixAllAsync"/>).
/// </summary>
public sealed class ContentFixerService : IContentFixerService
{
    private readonly IDbConnection _connection;
    private readonly IContentReviewRepository? _reviewRepo;
    private readonly ILogger<ContentFixerService> _logger;

    /// <summary>
    /// Deprecated tag names that should be removed from content items.
    /// Tags with useful replacements are handled by TagNormalizer._tagMappings
    /// (e.g. "dotnet" → ".NET", "Machine Learning" → "ML").
    /// </summary>
    private static readonly HashSet<string> _deprecatedTags = new(StringComparer.OrdinalIgnoreCase)
    {
        "Cloud",
        "Coding"
    };

    public ContentFixerService(
        IDbConnection connection,
        IContentReviewRepository? reviewRepo,
        ILogger<ContentFixerService> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _reviewRepo = reviewRepo;
        _logger = logger;
    }

    /// <inheritdoc />
    public string RepairMarkdown(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return content;
        }

        // Normalize line endings
        var text = content.Replace("\r\n", "\n", StringComparison.Ordinal).Replace("\r", "\n", StringComparison.Ordinal);
        var lines = text.Split('\n');
        var result = new List<string>(lines.Length + 20);

        var inCodeBlock = false;

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var prevLine = result.Count > 0 ? result[^1] : string.Empty;

            // Track code fences
            if (Regex.IsMatch(line, @"^\s*```"))
            {
                if (!inCodeBlock)
                {
                    // Opening fence — MD031: ensure blank line before (if prev is non-empty)
                    if (result.Count > 0 && prevLine.Trim().Length > 0)
                    {
                        result.Add(string.Empty);
                    }

                    result.Add(line.TrimEnd());
                    inCodeBlock = true;

                    // Remove blank lines immediately after opening fence
                    while (i + 1 < lines.Length && lines[i + 1].Trim().Length == 0)
                    {
                        i++;
                    }
                }
                else
                {
                    // Closing fence — remove blank lines immediately before it
                    while (result.Count > 0 && result[^1].Trim().Length == 0)
                    {
                        result.RemoveAt(result.Count - 1);
                    }

                    result.Add(line.TrimEnd());
                    inCodeBlock = false;

                    // MD031: ensure blank line after closing fence (if next is non-empty)
                    if (i + 1 < lines.Length && lines[i + 1].Trim().Length > 0)
                    {
                        result.Add(string.Empty);
                    }
                }

                continue;
            }

            // Inside code blocks — preserve content as-is (only trim trailing whitespace)
            if (inCodeBlock)
            {
                result.Add(line.TrimEnd());
                continue;
            }

            // MD009: Remove trailing whitespace
            line = line.TrimEnd();

            // MD018: Ensure space after # in headings
            var headingNoSpace = Regex.Match(line, @"^(#{1,6})([^#\s].*)");
            if (headingNoSpace.Success)
            {
                line = headingNoSpace.Groups[1].Value + " " + headingNoSpace.Groups[2].Value;
            }

            // Remove trailing colons from headings
            if (Regex.IsMatch(line, @"^#{1,6}\s+.*:$"))
            {
                line = line[..^1];
            }

            // Fix numbered list items missing space after period ("1.Item" → "1. Item")
            var numberedNoSpace = Regex.Match(line, @"^(\s*)(\d+)\.([^\s].*)");
            if (numberedNoSpace.Success)
            {
                line = numberedNoSpace.Groups[1].Value + numberedNoSpace.Groups[2].Value + ". " + numberedNoSpace.Groups[3].Value;
            }

            // Detect if current line is a heading
            var isHeading = Regex.IsMatch(line, @"^#{1,6}\s+");

            if (isHeading)
            {
                // MD022: Ensure blank line before heading (if prev is non-empty)
                if (result.Count > 0 && result[^1].Trim().Length > 0)
                {
                    result.Add(string.Empty);
                }

                result.Add(line);

                // MD022: Ensure blank line after heading (if next is non-empty and not heading)
                if (i + 1 < lines.Length && lines[i + 1].Trim().Length > 0 && !Regex.IsMatch(lines[i + 1], @"^#{1,6}\s+"))
                {
                    result.Add(string.Empty);
                }

                continue;
            }

            result.Add(line);
        }

        // Collapse multiple consecutive blank lines into one
        var collapsed = new List<string>(result.Count);
        var blankCount = 0;
        foreach (var line in result)
        {
            if (line.Trim().Length == 0)
            {
                blankCount++;
                if (blankCount <= 1)
                {
                    collapsed.Add(string.Empty);
                }
            }
            else
            {
                blankCount = 0;
                collapsed.Add(line);
            }
        }

        // Remove trailing blank lines, ensure exactly one trailing newline
        while (collapsed.Count > 0 && collapsed[^1].Trim().Length == 0)
        {
            collapsed.RemoveAt(collapsed.Count - 1);
        }

        var final = string.Join("\n", collapsed);
        if (final.Length > 0 && !final.EndsWith('\n'))
        {
            final += "\n";
        }

        return final;
    }

    /// <inheritdoc />
    public async Task<(int Scanned, int Fixed)> NormalizeTagsAsync(string? collectionFilter = null, CancellationToken ct = default)
    {
        var collectionClause = collectionFilter != null
            ? " WHERE collection_name = @CollectionFilter"
            : string.Empty;

        var sql = $@"
SELECT slug, collection_name AS CollectionName, tags_csv AS TagsCsv,
       date_epoch AS DateEpoch, sections_bitmask AS SectionsBitmask,
       is_ai AS IsAi, is_azure AS IsAzure, is_dotnet AS IsDotnet,
       is_devops AS IsDevops, is_github_copilot AS IsGithubCopilot,
       is_ml AS IsMl, is_security AS IsSecurity
FROM content_items
{collectionClause}
ORDER BY date_epoch DESC";

        var items = (await _connection.QueryAsync<TagNormalizationRow>(
            new CommandDefinition(sql, new { CollectionFilter = collectionFilter }, cancellationToken: ct))).ToList();

        var fixedCount = 0;

        foreach (var item in items)
        {
            var tags = ParseTagsCsv(item.TagsCsv);
            var originalTags = new List<string>(tags);

            // 1. Remove deprecated tags
            tags.RemoveAll(t => _deprecatedTags.Contains(t));

            // 2. Add section-derived tags and collection tag
            var sections = GetSectionSlugs(item);
            tags = TagNormalizer.EnsureSectionTags(tags, sections);

            // Add collection tag
            var collectionTag = char.ToUpperInvariant(item.CollectionName[0]) + item.CollectionName[1..];
            if (!tags.Any(t => t.Equals(collectionTag, StringComparison.OrdinalIgnoreCase)))
            {
                tags.Add(collectionTag);
            }

            // 3. Full normalization: casing, noise removal, dedup (same pipeline as ingestion)
            tags = TagNormalizer.NormalizeTags(tags);

            if (tags.SequenceEqual(originalTags, StringComparer.Ordinal))
            {
                continue;
            }

            var newTagsCsv = tags.Count > 0 ? $",{string.Join(",", tags)}," : string.Empty;

            if (_reviewRepo != null && _activeJobId.HasValue)
            {
                await _reviewRepo.CreateAsync(item.Slug, item.CollectionName,
                    ContentReviewChangeType.Tags, item.TagsCsv ?? string.Empty, newTagsCsv,
                    _activeJobId, ct);
            }
            else
            {
                await _connection.ExecuteAsync(new CommandDefinition(
                    @"UPDATE content_items
                      SET tags_csv = @TagsCsv, updated_at = NOW()
                      WHERE collection_name = @Collection AND slug = @Slug",
                    new { TagsCsv = newTagsCsv, Collection = item.CollectionName, Slug = item.Slug },
                    cancellationToken: ct));

                // Rebuild content_tags_expanded to match updated tags
                await _connection.ExecuteAsync(new CommandDefinition(
                    "DELETE FROM content_tags_expanded WHERE collection_name = @Collection AND slug = @Slug",
                    new { Collection = item.CollectionName, Slug = item.Slug },
                    cancellationToken: ct));

                if (tags.Count > 0)
                {
                    var tagRows = ContentItemWriteRepository.BuildTagWords(
                        tags, item.CollectionName, item.Slug, item.DateEpoch,
                        item.IsAi, item.IsAzure, item.IsDotnet, item.IsDevops,
                        item.IsGithubCopilot, item.IsMl, item.IsSecurity, item.SectionsBitmask);

                    foreach (var row in tagRows)
                    {
                        await _connection.ExecuteAsync(new CommandDefinition(
                            @"INSERT INTO content_tags_expanded
                                (collection_name, slug, tag_word, tag_display, is_full_tag,
                                 date_epoch, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
                                 is_ml, is_security, sections_bitmask)
                              VALUES
                                (@CollectionName, @Slug, @TagWord, @TagDisplay, @IsFullTag,
                                 @DateEpoch, @IsAi, @IsAzure, @IsDotnet, @IsDevops, @IsGhc,
                                 @IsMl, @IsSecurity, @Bitmask)
                              ON CONFLICT DO NOTHING",
                            row,
                            cancellationToken: ct));
                    }
                }
            }

            fixedCount++;
        }

        _logger.LogInformation("Tag normalization: fixed {Fixed} items out of {Total}", fixedCount, items.Count);
        return (items.Count, fixedCount);
    }

    /// <inheritdoc />
    public async Task<int> NormalizeAuthorsAsync(string? collectionFilter = null, CancellationToken ct = default)
    {
        if (_reviewRepo != null && _activeJobId.HasValue)
        {
            // In review mode, query for affected items and create review records
            var items = await _connection.QueryAsync<AuthorRow>(new CommandDefinition(
                @"SELECT slug, collection_name AS CollectionName, author
                  FROM content_items WHERE author = 'Tech Hub Team'",
                cancellationToken: ct));

            var count = 0;
            foreach (var item in items)
            {
                await _reviewRepo.CreateAsync(item.Slug, item.CollectionName,
                    ContentReviewChangeType.Author, item.Author, "TechHub",
                    _activeJobId, ct);
                count++;
            }

            _logger.LogInformation("Author normalization: queued {Fixed} items for review ('Tech Hub Team' → 'TechHub')", count);
            return count;
        }

        var rows = await _connection.ExecuteAsync(new CommandDefinition(
            @"UPDATE content_items SET author = 'TechHub', updated_at = NOW()
              WHERE author = 'Tech Hub Team'",
            cancellationToken: ct));

        _logger.LogInformation("Author normalization: fixed {Fixed} items ('Tech Hub Team' → 'TechHub')", rows);
        return rows;
    }

    /// <inheritdoc />
    public async Task<(int Scanned, int Fixed)> FixMarkdownAsync(string? collectionFilter = null, CancellationToken ct = default)
    {
        var collectionClause = collectionFilter != null
            ? " WHERE collection_name = @CollectionFilter"
            : string.Empty;

        var sql = $@"
SELECT slug, collection_name AS CollectionName, content
FROM content_items
{collectionClause}
ORDER BY date_epoch DESC";

        var items = (await _connection.QueryAsync<MarkdownRow>(
            new CommandDefinition(sql, new { CollectionFilter = collectionFilter }, cancellationToken: ct))).ToList();

        var fixedCount = 0;

        foreach (var item in items)
        {
            ct.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(item.Content))
            {
                continue;
            }

            var repaired = RepairMarkdown(item.Content);

            if (string.Equals(repaired, item.Content, StringComparison.Ordinal))
            {
                continue;
            }

            if (_reviewRepo != null && _activeJobId.HasValue)
            {
                await _reviewRepo.CreateAsync(item.Slug, item.CollectionName,
                    ContentReviewChangeType.Markdown, item.Content, repaired,
                    _activeJobId, ct);
            }
            else
            {
                await _connection.ExecuteAsync(new CommandDefinition(
                    @"UPDATE content_items
                      SET content = @Content, updated_at = NOW()
                      WHERE collection_name = @Collection AND slug = @Slug",
                    new { Content = repaired, Collection = item.CollectionName, Slug = item.Slug },
                    cancellationToken: ct));
            }

            fixedCount++;
        }

        _logger.LogInformation("Markdown repair: fixed {Fixed} items out of {Total}", fixedCount, items.Count);
        return (items.Count, fixedCount);
    }

    /// <inheritdoc />
    public async Task<(int Count, IReadOnlyList<ValidationIssueDetail> Details)> ValidateMarkdownAsync(string? collectionFilter = null, CancellationToken ct = default)
    {
        var collectionClause = collectionFilter != null
            ? " WHERE collection_name = @CollectionFilter"
            : string.Empty;

        var sql = $@"
SELECT slug, collection_name AS CollectionName, content
FROM content_items
{collectionClause}
ORDER BY date_epoch DESC";

        var items = (await _connection.QueryAsync<MarkdownRow>(
            new CommandDefinition(sql, new { CollectionFilter = collectionFilter }, cancellationToken: ct))).ToList();

        var details = new List<ValidationIssueDetail>();
        var inReviewMode = _reviewRepo != null && _activeJobId.HasValue;

        foreach (var item in items)
        {
            ct.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(item.Content))
            {
                continue;
            }

            var issues = DetectStructuralIssues(item.Content);
            if (issues.Count > 0)
            {
                var issuesSummary = string.Join("; ", issues);

                details.Add(new ValidationIssueDetail
                {
                    Slug = item.Slug,
                    CollectionName = item.CollectionName,
                    Issues = issuesSummary
                });

                _logger.LogWarning(
                    "Markdown validation issues in [{Collection}/{Slug}]: {Issues}",
                    item.CollectionName, item.Slug, issuesSummary);

                if (inReviewMode)
                {
                    await _reviewRepo!.CreateAsync(
                        item.Slug, item.CollectionName,
                        ContentReviewChangeType.Validation,
                        originalValue: item.Content,
                        fixedValue: issuesSummary,
                        jobId: _activeJobId,
                        ct: ct);
                }
            }
        }

        _logger.LogInformation("Markdown validation: {IssueCount} items with issues out of {Total} scanned", details.Count, items.Count);
        return (details.Count, details);
    }

    /// <inheritdoc />
    public async Task<ContentFixerResult> FixAllAsync(string? collectionFilter = null, long? jobId = null, CancellationToken ct = default)
    {
        _activeJobId = jobId;
        try
        {
            var mode = _reviewRepo != null && jobId.HasValue ? "review" : "direct";
            _logger.LogInformation("Content fixer running in {Mode} mode", mode);

            var (tagsScanned, tagsFixed) = await NormalizeTagsAsync(collectionFilter, ct);
            var authorsFixed = await NormalizeAuthorsAsync(collectionFilter, ct);
            var (markdownScanned, markdownFixed) = await FixMarkdownAsync(collectionFilter, ct);
            var (validationIssues, validationDetails) = await ValidateMarkdownAsync(collectionFilter, ct);

            return new ContentFixerResult
            {
                TagsScanned = tagsScanned,
                TagsFixed = tagsFixed,
                AuthorsFixed = authorsFixed,
                MarkdownScanned = markdownScanned,
                MarkdownFixed = markdownFixed,
                ValidationIssues = validationIssues,
                ValidationDetails = validationDetails
            };
        }
        finally
        {
            _activeJobId = null;
        }
    }

    // ── Review mode state ────────────────────────────────────────────────────

    /// <summary>
    /// When set, changes are queued for review instead of applied directly.
    /// </summary>
    private long? _activeJobId;

    // ── Internal helpers ─────────────────────────────────────────────────────

    /// <summary>
    /// Lightweight Markdig pipeline for validation only (no rendering extensions needed).
    /// </summary>
    private static readonly MarkdownPipeline _validationPipeline = new MarkdownPipelineBuilder()
        .UsePipeTables()
        .Build();

    /// <summary>
    /// Parses markdown with Markdig and detects structural issues in the AST.
    /// Returns a list of human-readable issue descriptions. Empty list = no issues.
    /// </summary>
    internal static List<string> DetectStructuralIssues(string content)
    {
        var issues = new List<string>();
        var doc = Markdown.Parse(content, _validationPipeline);

        // 1. Empty headings
        foreach (var heading in doc.Descendants<HeadingBlock>())
        {
            var text = heading.Inline?.FirstChild?.ToString();
            if (string.IsNullOrWhiteSpace(text))
            {
                issues.Add($"Empty heading (level {heading.Level}) at line {heading.Line + 1}");
            }
        }

        // 2. Broken links (missing URL)
        foreach (var link in doc.Descendants<LinkInline>())
        {
            if (!link.IsImage && string.IsNullOrWhiteSpace(link.Url))
            {
                var linkText = link.FirstChild?.ToString() ?? "(empty)";
                issues.Add($"Link with empty URL \"{linkText}\" at line {link.Line + 1}");
            }
        }

        // 3. Broken images (missing URL)
        foreach (var image in doc.Descendants<LinkInline>().Where(l => l.IsImage))
        {
            if (string.IsNullOrWhiteSpace(image.Url))
            {
                var alt = image.FirstChild?.ToString() ?? "(no alt)";
                issues.Add($"Image with empty URL \"{alt}\" at line {image.Line + 1}");
            }
        }

        // 4. Unclosed/orphaned HTML blocks that suggest broken markdown
        foreach (var html in doc.Descendants<HtmlBlock>())
        {
            var htmlContent = html.Lines.ToString().Trim();
            // Detect common broken patterns: unclosed tags that aren't intentional
            if (htmlContent.StartsWith('<') && !htmlContent.Contains('>', StringComparison.Ordinal))
            {
                issues.Add($"Potentially broken HTML at line {html.Line + 1}: {Truncate(htmlContent, 60)}");
            }
        }

        // 5. Deeply nested lists (>4 levels often indicates formatting issues)
        foreach (var list in doc.Descendants<ListBlock>())
        {
            var depth = 0;
            Block? parent = list.Parent;
            while (parent is not null)
            {
                if (parent is ListBlock)
                {
                    depth++;
                }

                parent = parent.Parent;
            }

            if (depth >= 4)
            {
                issues.Add($"Deeply nested list ({depth + 1} levels) at line {list.Line + 1}");
            }
        }

        return issues;
    }

    private static string Truncate(string value, int maxLength)
    {
        return value.Length <= maxLength ? value : string.Concat(value.AsSpan(0, maxLength), "…");
    }

    private static List<string> ParseTagsCsv(string? tagsCsv)
    {
        if (string.IsNullOrWhiteSpace(tagsCsv))
        {
            return [];
        }

        return tagsCsv
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim())
            .Where(t => t.Length > 0)
            .ToList();
    }

    private static List<string> GetSectionSlugs(TagNormalizationRow item)
    {
        var sections = new List<string>();
        if (item.IsAi)
        {
            sections.Add("ai");
        }

        if (item.IsAzure)
        {
            sections.Add("azure");
        }

        if (item.IsDotnet)
        {
            sections.Add("dotnet");
        }

        if (item.IsDevops)
        {
            sections.Add("devops");
        }

        if (item.IsGithubCopilot)
        {
            sections.Add("github-copilot");
        }

        if (item.IsMl)
        {
            sections.Add("ml");
        }

        if (item.IsSecurity)
        {
            sections.Add("security");
        }

        return sections;
    }

    // ── Row models (Dapper mapping) ──────────────────────────────────────────

#pragma warning disable CA1812 // Instantiated by Dapper
    private sealed class TagNormalizationRow
    {
        public string Slug { get; init; } = string.Empty;
        public string CollectionName { get; init; } = string.Empty;
        public string? TagsCsv { get; init; }
        public long DateEpoch { get; init; }
        public int SectionsBitmask { get; init; }
        public bool IsAi { get; init; }
        public bool IsAzure { get; init; }
        public bool IsDotnet { get; init; }
        public bool IsDevops { get; init; }
        public bool IsGithubCopilot { get; init; }
        public bool IsMl { get; init; }
        public bool IsSecurity { get; init; }
    }

    private sealed class MarkdownRow
    {
        public string Slug { get; init; } = string.Empty;
        public string CollectionName { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
    }

    private sealed class AuthorRow
    {
        public string Slug { get; init; } = string.Empty;
        public string CollectionName { get; init; } = string.Empty;
        public string Author { get; init; } = string.Empty;
    }
#pragma warning restore CA1812
}
