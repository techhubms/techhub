using System.Data;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Orchestrates the AI weekly roundup generation pipeline.
/// Reads article candidates from <c>content_items</c> (filtered by section flags and AI metadata),
/// runs AI steps 3-8, and writes the resulting roundup directly to <c>content_items</c>.
/// </summary>
public sealed class RoundupGeneratorService : IRoundupGeneratorService
{
    // Ordered list of sections — determines display order in the roundup.
    private static readonly string[] _sectionOrder =
        ["GitHub Copilot", "AI", "ML", "Azure", ".NET", "DevOps", "Security"];

    // Maps the display section name to the DB slug used for section boolean columns.
    private static readonly Dictionary<string, string> _sectionNameToSlug = new(StringComparer.OrdinalIgnoreCase)
    {
        ["GitHub Copilot"] = "github-copilot",
        ["AI"] = "ai",
        ["ML"] = "ml",
        ["Azure"] = "azure",
        [".NET"] = "dotnet",
        ["DevOps"] = "devops",
        ["Security"] = "security"
    };

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly ISectionRoundupRepository _roundupRepo;
    private readonly IAiCompletionClient _aiClient;
    private readonly IDbConnection _connection;
    private readonly RoundupGeneratorOptions _options;
    private readonly ILogger<RoundupGeneratorService> _logger;

    // Lazily-loaded embedded prompt resources.
    private static readonly Lazy<string> _step3System = LoadResource("roundup-step3-system.md");
    private static readonly Lazy<string> _step4System = LoadResource("roundup-step4-system.md");
    private static readonly Lazy<string> _step6System = LoadResource("roundup-step6-system.md");
    private static readonly Lazy<string> _step7System = LoadResource("roundup-step7-system.md");
    private static readonly Lazy<string> _writingGuidelines = LoadResource("roundup-writing-guidelines.md");

    public RoundupGeneratorService(
        ISectionRoundupRepository roundupRepo,
        IAiCompletionClient aiClient,
        IDbConnection connection,
        IOptions<RoundupGeneratorOptions> options,
        ILogger<RoundupGeneratorService> logger)
    {
        ArgumentNullException.ThrowIfNull(roundupRepo);
        ArgumentNullException.ThrowIfNull(aiClient);
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _roundupRepo = roundupRepo;
        _aiClient = aiClient;
        _connection = connection;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> GenerateAsync(DateOnly weekStart, DateOnly weekEnd, CancellationToken ct = default)
    {
        var publishDate = weekEnd.AddDays(1); // Monday after the week ends
        var slug = BuildSlug(publishDate);

        // Deduplication: skip if roundup already exists for this week.
        if (await RoundupExistsAsync(slug, ct))
        {
            _logger.LogInformation("Roundup for week {WeekStart} already exists (slug={Slug}), skipping", weekStart, slug);
            return false;
        }

        _logger.LogInformation("Generating roundup for week {WeekStart}–{WeekEnd}", weekStart, weekEnd);

        // ── Step 1: Load articles from DB ─────────────────────────────────────
        var articlesBySection = await _roundupRepo.GetArticlesForWeekAsync(weekStart, weekEnd, ct);

        if (articlesBySection.Count == 0)
        {
            _logger.LogWarning("No articles found for week {WeekStart}–{WeekEnd}, skipping roundup generation", weekStart, weekEnd);
            return false;
        }

        // ── Step 2: Apply relevance filtering ─────────────────────────────────
        var filtered = ApplyRelevanceFiltering(articlesBySection);

        if (filtered.Count == 0)
        {
            _logger.LogWarning("No articles remain after relevance filtering for week {WeekStart}–{WeekEnd}", weekStart, weekEnd);
            return false;
        }

        var weekDescription = string.Create(CultureInfo.InvariantCulture,
            $"the week of {weekStart.ToString("MMMM d", CultureInfo.InvariantCulture)} to {weekEnd.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture)}");

        var writingGuidelines = _writingGuidelines.Value;

        // ── Step 3: AI - Create news-like stories per section ─────────────────
        _logger.LogInformation("Step 3: Creating news-like stories per section");
        var step3Content = await RunStep3Async(filtered, weekDescription, writingGuidelines, ct);

        // ── Step 4: AI - Add ongoing narrative by comparing with previous roundup
        _logger.LogInformation("Step 4: Adding ongoing narrative");
        var step4Content = await RunStep4Async(step3Content, weekStart, writingGuidelines, ct);

        // ── Step 5: Merge (already merged from step 4) ────────────────────────
        var mergedContent = step4Content;

        // ── Step 6: AI - Condense content ─────────────────────────────────────
        _logger.LogInformation("Step 6: Condensing content");
        var step6Content = await RunStep6Async(mergedContent, writingGuidelines, ct);

        // ── Step 7: AI - Generate metadata ────────────────────────────────────
        _logger.LogInformation("Step 7: Generating metadata");
        var metadata = await RunStep7Async(step6Content, weekDescription, writingGuidelines, ct);

        // ── Step 8: Build table of contents + full content ────────────────────
        _logger.LogInformation("Step 8: Building final content");
        var tableOfContents = BuildTableOfContents(step6Content);
        var fullContent = BuildFullContent(step6Content, metadata, tableOfContents);

        // ── Write to database ─────────────────────────────────────────────────
        _logger.LogInformation("Writing roundup to database");
        await WriteRoundupAsync(slug, publishDate, metadata.Title, metadata.Description, fullContent, metadata, ct);

        _logger.LogInformation("Roundup for week {WeekStart}–{WeekEnd} written successfully (slug={Slug})", weekStart, weekEnd, slug);
        return true;
    }

    // ── Relevance Filtering ───────────────────────────────────────────────────

    private Dictionary<string, IReadOnlyList<RoundupArticle>> ApplyRelevanceFiltering(
        IReadOnlyDictionary<string, IReadOnlyList<RoundupArticle>> articlesBySection)
    {
        var result = new Dictionary<string, IReadOnlyList<RoundupArticle>>(StringComparer.OrdinalIgnoreCase);

        foreach (var (sectionSlug, articles) in articlesBySection)
        {
            // Map DB slug back to display name for logging.
            var displayName = _sectionNameToSlug
                .FirstOrDefault(kv => kv.Value.Equals(sectionSlug, StringComparison.OrdinalIgnoreCase)).Key
                ?? sectionSlug;

            var high = articles
                .Where(a => a.Relevance.Equals("high", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var medium = articles
                .Where(a => a.Relevance.Equals("medium", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var low = articles
                .Where(a => a.Relevance.Equals("low", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var selected = new List<RoundupArticle>(high);

            // Add medium if we don't have enough high articles.
            if (selected.Count < _options.MinHighArticlesPerSection)
            {
                selected.AddRange(medium);
            }

            // Add low if we still don't have enough articles.
            if (selected.Count < _options.MinTotalArticlesPerSection)
            {
                selected.AddRange(low);
            }

            if (selected.Count == 0)
            {
                _logger.LogInformation("Section {Section} has no articles after filtering, skipping", displayName);
                continue;
            }

            _logger.LogInformation(
                "Section {Section}: {HighCount} high + {MediumCount} medium + {LowCount} low => {Total} selected",
                displayName, high.Count, selected.Count - high.Count,
                selected.Count - high.Count - medium.Count, selected.Count);

            result[sectionSlug] = selected;
        }

        return result;
    }

    // ── Step 3: News Stories ──────────────────────────────────────────────────

    private async Task<string> RunStep3Async(
        Dictionary<string, IReadOnlyList<RoundupArticle>> filtered,
        string weekDescription,
        string writingGuidelines,
        CancellationToken ct)
    {
        var systemMessage = _step3System.Value
            .Replace("{WeekDescription}", weekDescription, StringComparison.Ordinal)
            .Replace("{WritingStyleGuidelines}", writingGuidelines, StringComparison.Ordinal);

        var responses = new List<string>();

        foreach (var displayName in _sectionOrder)
        {
            if (!_sectionNameToSlug.TryGetValue(displayName, out var slug))
            {
                continue;
            }

            // Sections may be keyed by slug or display name coming from the DB.
            if (!filtered.TryGetValue(slug, out var articles) &&
                !filtered.TryGetValue(displayName, out articles))
            {
                continue;
            }

            _logger.LogInformation("Step 3: Processing section {Section} ({Count} articles)", displayName, articles.Count);

            var sectionInput = BuildSectionInput(displayName, articles);

            var userMessage = string.Create(CultureInfo.InvariantCulture,
                $"ARTICLE ANALYSIS RESULTS FOR {displayName} SECTION TO TRANSFORM INTO NEWS-STYLE CONTENT:\n\n{sectionInput}");

            var response = await CallAiWithRetryAsync(systemMessage, userMessage, "Step 3 - " + displayName, ct);
            if (response is not null)
            {
                responses.Add(response);
                _logger.LogInformation("Step 3: Section {Section} complete", displayName);
            }
            else
            {
                _logger.LogWarning("Step 3: Section {Section} AI call failed, skipping section", displayName);
            }

            await Task.Delay(TimeSpan.FromSeconds(_options.RateLimitDelaySeconds), ct);
        }

        return string.Join("\n\n", responses);
    }

    // ── Step 4: Ongoing Narrative ─────────────────────────────────────────────

    private async Task<string> RunStep4Async(
        string step3Content,
        DateOnly weekStart,
        string writingGuidelines,
        CancellationToken ct)
    {
        var previousRoundupContent = await LoadPreviousRoundupContentAsync(weekStart, ct);

        if (string.IsNullOrWhiteSpace(previousRoundupContent))
        {
            _logger.LogInformation("Step 4: No previous roundup found, skipping narrative enhancement");
            return step3Content;
        }

        var systemMessage = _step4System.Value
            .Replace("{WritingStyleGuidelines}", writingGuidelines, StringComparison.Ordinal);

        var sectionContents = ParseSections(step3Content);
        var responses = new List<string>();

        foreach (var displayName in _sectionOrder)
        {
            if (!sectionContents.TryGetValue(displayName, out var currentSection))
            {
                continue;
            }

            var previousSection = ExtractSectionFromContent(previousRoundupContent, displayName);

            if (string.IsNullOrWhiteSpace(previousSection))
            {
                _logger.LogInformation("Step 4: No previous section for {Section}, using current", displayName);
                responses.Add(currentSection);
                continue;
            }

            _logger.LogInformation("Step 4: Processing ongoing narrative for section {Section}", displayName);

            var userMessage =
                $"PREVIOUS WEEK'S {displayName} SECTION:\n{previousSection}\n\n---\n\n" +
                $"CURRENT WEEK'S {displayName} SECTION CONTENT TO ENHANCE:\n{currentSection}";

            var response = await CallAiWithRetryAsync(systemMessage, userMessage, "Step 4 - " + displayName, ct);
            responses.Add(response ?? currentSection); // Fall back to step 3 content on failure

            await Task.Delay(TimeSpan.FromSeconds(_options.RateLimitDelaySeconds), ct);
        }

        return string.Join("\n\n", responses);
    }

    // ── Step 6: Condense ──────────────────────────────────────────────────────

    private async Task<string> RunStep6Async(string content, string writingGuidelines, CancellationToken ct)
    {
        var systemMessage = _step6System.Value
            .Replace("{WritingStyleGuidelines}", writingGuidelines, StringComparison.Ordinal);

        var userMessage = $"WELL-ORGANIZED ROUNDUP CONTENT TO CONDENSE:\n\n{content}";

        var response = await CallAiWithRetryAsync(systemMessage, userMessage, "Step 6", ct);

        if (response is null)
        {
            _logger.LogWarning("Step 6: AI condensing failed, using step 5 content");
            return content;
        }

        await Task.Delay(TimeSpan.FromSeconds(_options.RateLimitDelaySeconds), ct);
        return response;
    }

    // ── Step 7: Metadata ──────────────────────────────────────────────────────

    private async Task<RoundupMetadataAi> RunStep7Async(
        string condensedContent,
        string weekDescription,
        string writingGuidelines,
        CancellationToken ct)
    {
        var systemMessage = _step7System.Value
            .Replace("{WritingStyleGuidelines}", writingGuidelines, StringComparison.Ordinal);

        var userMessage =
            $"Generate metadata for the roundup covering {weekDescription} based on this condensed content:\n\n" +
            $"{condensedContent}\n\nReturn only JSON with fields: title, tags, description, introduction";

        for (var attempt = 0; attempt < _options.MaxRetries; attempt++)
        {
            var response = await CallAiWithRetryAsync(systemMessage, userMessage, "Step 7", ct);

            if (response is not null)
            {
                try
                {
                    var meta = JsonSerializer.Deserialize<RoundupMetadataAi>(response, _jsonOptions);
                    if (meta is not null && !string.IsNullOrWhiteSpace(meta.Title))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(_options.RateLimitDelaySeconds), ct);
                        return meta;
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Step 7: Failed to parse metadata JSON (attempt {Attempt})", attempt + 1);
                }
            }
        }

        _logger.LogWarning("Step 7: Metadata generation failed after retries, using fallback metadata");
        return new RoundupMetadataAi
        {
            Title = string.Create(CultureInfo.InvariantCulture, $"Weekly AI and Tech News Roundup - {weekDescription}"),
            Tags = ["AI", "Azure", "GitHub Copilot", ".NET", "DevOps", "Security"],
            Description = string.Create(CultureInfo.InvariantCulture, $"A roundup of the latest tech news for {weekDescription}."),
            Introduction = string.Create(CultureInfo.InvariantCulture, $"Welcome to this week's roundup covering {weekDescription}.")
        };
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string BuildSlug(DateOnly publishDate) =>
        publishDate.ToString("'Weekly-AI-and-Tech-News-Roundup-'yyyy-MM-dd", CultureInfo.InvariantCulture);

    private async Task<bool> RoundupExistsAsync(string slug, CancellationToken ct)
    {
        var count = await _connection.ExecuteScalarAsync<int>(new CommandDefinition(
            "SELECT COUNT(*) FROM content_items WHERE collection_name = 'roundups' AND slug = @Slug",
            new { Slug = slug },
            cancellationToken: ct));

        return count > 0;
    }

    private static string BuildSectionInput(string displayName, IReadOnlyList<RoundupArticle> articles)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"## {displayName}"));
        sb.AppendLine();

        foreach (var article in articles)
        {
            sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"ARTICLE: {article.Title}"));
            sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"SUMMARY: {article.Summary}"));
            sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"RELEVANCE: {article.Relevance}"));

            if (!string.IsNullOrEmpty(article.TopicType))
            {
                sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"TYPE: {article.TopicType}"));
            }

            if (!string.IsNullOrEmpty(article.ImpactLevel))
            {
                sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"IMPACT: {article.ImpactLevel}"));
            }

            if (!string.IsNullOrEmpty(article.TimeSensitivity))
            {
                sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"TIMING: {article.TimeSensitivity}"));
            }

            if (article.KeyTopics.Count > 0)
            {
                sb.AppendLine(string.Create(CultureInfo.InvariantCulture,
                    $"KEY_TOPICS: {string.Join(", ", article.KeyTopics)}"));
            }

            var link = article.IsInternal
                ? $"[{article.Title}]({{{{ \"{article.ExternalUrl}\" | relative_url }}}})"
                : $"[{article.Title}]({article.ExternalUrl})";

            sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"LINK: {link}"));
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static Dictionary<string, string> ParseSections(string content)
    {
        var sections = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var lines = content.Split('\n');
        string? currentSection = null;
        var currentLines = new List<string>();

        foreach (var line in lines)
        {
            if (line.StartsWith("## ", StringComparison.Ordinal))
            {
                if (currentSection is not null && currentLines.Count > 0)
                {
                    sections[currentSection] = string.Join("\n", currentLines).TrimEnd();
                }

                currentSection = line[3..].Trim();
                currentLines = [line];
            }
            else if (currentSection is not null)
            {
                currentLines.Add(line);
            }
        }

        if (currentSection is not null && currentLines.Count > 0)
        {
            sections[currentSection] = string.Join("\n", currentLines).TrimEnd();
        }

        return sections;
    }

    private static string ExtractSectionFromContent(string content, string sectionName)
    {
        var lines = content.Split('\n');
        var inSection = false;
        var sectionLines = new List<string>();

        foreach (var line in lines)
        {
            if (line.StartsWith("## ", StringComparison.Ordinal))
            {
                if (inSection)
                {
                    break;
                }

                if (line[3..].Trim().Equals(sectionName, StringComparison.OrdinalIgnoreCase))
                {
                    inSection = true;
                }
            }

            if (inSection)
            {
                sectionLines.Add(line);
            }
        }

        return string.Join("\n", sectionLines).Trim();
    }

    private static string BuildTableOfContents(string content)
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

    private static string BuildAnchor(string title)
    {
        // Matches Kramdown anchor generation: remove non-alphanumeric-space-dash, lowercase, replace non-alnum with dash.
        var clean = new string(title.Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-').ToArray());
        return new string(clean.ToLowerInvariant().Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray());
    }

    private static string BuildFullContent(string sectionContent, RoundupMetadataAi metadata, string tableOfContents) =>
        $"{metadata.Introduction}\n\n<!--excerpt_end-->\n\n## This Week's Overview\n\n{tableOfContents}\n\n{sectionContent}";

    private async Task<string?> LoadPreviousRoundupContentAsync(DateOnly weekStart, CancellationToken ct)
    {
        // Find the most recent roundup whose date_epoch is before this week's start.
        var brusselsZone = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Romance Standard Time" : "Europe/Brussels");
        var weekStartDt = weekStart.ToDateTime(TimeOnly.MinValue);
        var weekStartEpoch = (long)TimeZoneInfo.ConvertTimeToUtc(weekStartDt, brusselsZone)
            .Subtract(DateTime.UnixEpoch).TotalSeconds;

        var content = await _connection.QueryFirstOrDefaultAsync<string>(new CommandDefinition(
            @"SELECT content
              FROM content_items
              WHERE collection_name = 'roundups'
                AND date_epoch < @WeekStartEpoch
              ORDER BY date_epoch DESC
              LIMIT 1",
            new { WeekStartEpoch = weekStartEpoch },
            cancellationToken: ct));

        return content;
    }

    private async Task WriteRoundupAsync(
        string slug,
        DateOnly publishDate,
        string title,
        string description,
        string content,
        RoundupMetadataAi metadata,
        CancellationToken ct)
    {
        var brusselsZone = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Romance Standard Time" : "Europe/Brussels");

        // Publish at 9:00 AM Brussels time on publish date.
        var publishDt = publishDate.ToDateTime(new TimeOnly(9, 0, 0));
        var publishUtc = TimeZoneInfo.ConvertTimeToUtc(publishDt, brusselsZone);
        var dateEpoch = (long)publishUtc.Subtract(DateTime.UnixEpoch).TotalSeconds;

        var externalUrl = string.Create(CultureInfo.InvariantCulture, $"/all/roundups/{slug}");

        var tagsCsv = metadata.Tags.Count > 0
            ? string.Create(CultureInfo.InvariantCulture, $",{string.Join(",", metadata.Tags)},")
            : string.Empty;

        // Roundups span all sections.
        var computedHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(content)));

        var aiMetadataJson = JsonSerializer.Serialize(new
        {
            roundup_summary = description,
            key_topics = metadata.Tags,
            roundup_relevance = "high",
            topic_type = "news"
        }, _jsonOptions);

        await _connection.ExecuteAsync(new CommandDefinition(
            @"INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name,
                 tags_csv, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
                 is_ml, is_security, sections_bitmask, content_hash, ai_metadata)
              VALUES
                (@Slug, 'roundups', @Title, @Content, @Excerpt, @DateEpoch,
                 'github-copilot', @ExternalUrl, 'TechHub', 'TechHub',
                 @TagsCsv, TRUE, TRUE, TRUE, TRUE, TRUE,
                 TRUE, TRUE, 127, @ContentHash, @AiMetadata::jsonb)
              ON CONFLICT (collection_name, slug) DO UPDATE SET
                title            = EXCLUDED.title,
                content          = EXCLUDED.content,
                excerpt          = EXCLUDED.excerpt,
                tags_csv         = EXCLUDED.tags_csv,
                content_hash     = EXCLUDED.content_hash,
                ai_metadata      = EXCLUDED.ai_metadata,
                updated_at       = NOW()",
            new
            {
                Slug = slug,
                Title = title,
                Content = content,
                Excerpt = metadata.Introduction,
                DateEpoch = dateEpoch,
                ExternalUrl = externalUrl,
                TagsCsv = tagsCsv,
                ContentHash = computedHash,
                AiMetadata = aiMetadataJson
            },
            cancellationToken: ct));

        // Rebuild expanded tags for the roundup.
        await _connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM content_tags_expanded WHERE collection_name = 'roundups' AND slug = @Slug",
            new { Slug = slug },
            cancellationToken: ct));

        foreach (var tag in metadata.Tags)
        {
            var tagLower = tag.ToLowerInvariant();
            await _connection.ExecuteAsync(new CommandDefinition(
                @"INSERT INTO content_tags_expanded
                    (collection_name, slug, tag_word, tag_display, is_full_tag,
                     date_epoch, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
                     is_ml, is_security, sections_bitmask)
                  VALUES
                    ('roundups', @Slug, @TagWord, @TagDisplay, TRUE,
                     @DateEpoch, TRUE, TRUE, TRUE, TRUE, TRUE, TRUE, TRUE, 127)
                  ON CONFLICT DO NOTHING",
                new
                {
                    Slug = slug,
                    TagWord = tagLower,
                    TagDisplay = tag,
                    DateEpoch = dateEpoch
                },
                cancellationToken: ct));
        }
    }

    // ── AI Call Helpers ───────────────────────────────────────────────────────

    private async Task<string?> CallAiWithRetryAsync(
        string systemMessage,
        string userMessage,
        string stepName,
        CancellationToken ct)
    {
        var requestBody = new
        {
            messages = new[]
            {
                new { role = "system", content = systemMessage },
                new { role = "user", content = userMessage }
            },
            max_completion_tokens = 8000
        };

        var json = JsonSerializer.Serialize(requestBody, _jsonOptions);

        for (var attempt = 0; attempt < _options.MaxRetries; attempt++)
        {
            try
            {
                var result = await _aiClient.SendCompletionAsync(json, ct);

                if (result.IsRateLimited)
                {
                    _logger.LogWarning("{Step}: Rate limit hit, waiting before retry {Attempt}/{Max}",
                        stepName, attempt + 1, _options.MaxRetries);
                    await Task.Delay(TimeSpan.FromSeconds(_options.RateLimitDelaySeconds * (attempt + 1)), ct);
                    continue;
                }

                if (result.ResponseBody is null)
                {
                    continue;
                }

                return ExtractContentFromResponse(result.ResponseBody);
            }
            catch (HttpRequestException ex) when (attempt < _options.MaxRetries - 1)
            {
                _logger.LogWarning(ex, "{Step}: HTTP failure (attempt {Attempt}/{Max}), retrying",
                    stepName, attempt + 1, _options.MaxRetries);
                await Task.Delay(TimeSpan.FromSeconds(5 * (attempt + 1)), ct);
            }
        }

        _logger.LogError("{Step}: AI call failed after {MaxRetries} attempts", stepName, _options.MaxRetries);
        return null;
    }

    private string? ExtractContentFromResponse(string responseJson)
    {
        try
        {
            using var doc = JsonDocument.Parse(responseJson);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Failed to extract content from AI response");
            return null;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Unexpected AI response structure");
            return null;
        }
    }

    // ── Embedded Resource Loading ─────────────────────────────────────────────

    private static Lazy<string> LoadResource(string fileName) => new(() =>
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"TechHub.Infrastructure.Data.Resources.{fileName}";

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");

        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    });

    // ── Private DTOs ──────────────────────────────────────────────────────────

    private sealed class RoundupMetadataAi
    {
        public string Title { get; init; } = string.Empty;
        public IReadOnlyList<string> Tags { get; init; } = [];
        public string Description { get; init; } = string.Empty;
        public string Introduction { get; init; } = string.Empty;
    }
}
