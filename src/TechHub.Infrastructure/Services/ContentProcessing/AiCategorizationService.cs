using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Processes a raw feed item with AI in two steps:
/// <list type="number">
/// <item><b>Classify</b> (<see cref="AiProfile.Classification"/>): include or exclude, sections, tags, excerpt,
/// roundup metadata and timeline events, returned as strict JSON.</item>
/// <item><b>Rewrite</b> (<see cref="AiProfile.Rewrite"/>): only for included items, the cleaned-up markdown body.</item>
/// </list>
/// </summary>
public sealed class AiCategorizationService : IAiCategorizationService
{
    private const int MaxKeyFacts = 5;
    private const int MaxProducts = 3;
    private const int ClassificationMaxCompletionTokens = 12_000;
    private const int RewriteMinCompletionTokens = 8_000;
    private const int RewriteMaxCompletionTokens = 60_000;

    private readonly IAiCompletionClient _completionClient;
    private readonly AiCategorizationOptions _options;
    private readonly ILogger<AiCategorizationService> _logger;

    private static readonly string _classifySystemMessage = LoadClassifySystemMessage();
    private static readonly string _rewriteSystemMessage = LoadResource("content-rewrite-system.md");

    private static readonly HashSet<string> _validSections = new(ClassificationSchema.Sections, StringComparer.OrdinalIgnoreCase);

    public AiCategorizationService(
        IAiCompletionClient completionClient,
        IOptions<AiCategorizationOptions> options,
        ILogger<AiCategorizationService> logger)
    {
        ArgumentNullException.ThrowIfNull(completionClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _completionClient = completionClient;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<CategorizationResult> ClassifyAsync(RawFeedItem item, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        var json = AiRequestBodyBuilder.Build(
            _options.Profiles.Classification,
            _classifySystemMessage,
            BuildUserPrompt(item),
            ClassificationMaxCompletionTokens,
            item.IsInteractive,
            ClassificationSchema.BuildResponseFormat());

        var call = await SendWithRetryAsync(json, item, "Classification", ct);
        return call.Failure ?? ParseClassification(call.ResponseBody!, item);
    }

    /// <inheritdoc />
    public async Task<CategorizationResult> CategorizeAsync(RawFeedItem item, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        var classification = await ClassifyAsync(item, ct);
        if (classification.IsFailure || classification.Item is null)
        {
            return classification;
        }

        var rewritten = await RewriteAsync(item, classification.Item, ct);
        if (rewritten.Failure is not null)
        {
            return rewritten.Failure;
        }

        var classified = classification.Item;
        var contentHash = ComputeHash(classified.Title + rewritten.Markdown + classified.Excerpt);

        return new CategorizationResult
        {
            Item = classified.WithRewrittenContent(rewritten.Markdown!, contentHash),
            Explanation = classification.Explanation
        };
    }

    private async Task<(string? Markdown, CategorizationResult? Failure)> RewriteAsync(
        RawFeedItem source,
        ProcessedContentItem classified,
        CancellationToken ct)
    {
        var userContent = BuildRewritePrompt(source, classified);

        // The rewrite is about as long as the article itself, plus room for reasoning.
        var estimatedInputTokens = userContent.Length / 4;
        var maxTokens = Math.Clamp(estimatedInputTokens + 4_000, RewriteMinCompletionTokens, RewriteMaxCompletionTokens);

        var json = AiRequestBodyBuilder.Build(
            _options.Profiles.Rewrite,
            _rewriteSystemMessage,
            userContent,
            maxTokens,
            source.IsInteractive);

        var call = await SendWithRetryAsync(json, source, "Rewrite", ct);
        if (call.Failure is not null)
        {
            return (null, new CategorizationResult
            {
                Explanation = $"Rewrite failed: {call.Failure.Explanation}",
                IsFailure = true
            });
        }

        var choice = ReadChoice(call.ResponseBody!, source);
        if (choice.Failure is not null)
        {
            return (null, new CategorizationResult
            {
                Explanation = $"Rewrite failed: {choice.Failure.Explanation}",
                IsFailure = true
            });
        }

        return (UnwrapMarkdownFence(choice.Content!), null);
    }

    /// <summary>
    /// Sends the request, retrying on rate limits and transient failures.
    /// Returns either the response body or a failure result.
    /// </summary>
    private async Task<(string? ResponseBody, CategorizationResult? Failure)> SendWithRetryAsync(
        string json,
        RawFeedItem item,
        string stepName,
        CancellationToken ct)
    {
        var attempt = 0;

        while (attempt < _options.MaxRetries)
        {
            attempt++;
            try
            {
                var result = await _completionClient.SendCompletionAsync(json, ct);

                if (result.IsRateLimited)
                {
                    // Use server-provided retry-after if available, otherwise fall back to config-based delay
                    var delaySeconds = result.RetryAfterSeconds ?? (_options.RateLimitDelaySeconds * attempt);
                    _logger.LogWarning("AI rate limit hit ({Step}), waiting {Delay}s before retry {Attempt}/{Max}",
                        stepName, delaySeconds, attempt, _options.MaxRetries);
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds), ct);
                    continue;
                }

                if (result.ContentFilterMessage != null)
                {
                    _logger.LogWarning("AI content filter triggered ({Step}) for {Url}: {Message}",
                        stepName, item.ExternalUrl.Sanitize(), result.ContentFilterMessage);
                    return (null, new CategorizationResult { Explanation = result.ContentFilterMessage, IsFailure = true });
                }

                return (result.ResponseBody, null);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex) when (attempt < _options.MaxRetries
                && ex is HttpRequestException or TaskCanceledException or OperationCanceledException or TimeoutException)
            {
                var delaySeconds = 5 * attempt;
                _logger.LogWarning(ex, "AI API call failed ({Step}, attempt {Attempt}/{Max}), retrying in {Delay}s",
                    stepName, attempt, _options.MaxRetries, delaySeconds);
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds), ct);
            }
        }

        _logger.LogError("AI {Step} failed after {MaxRetries} attempts for {Url}", stepName, _options.MaxRetries, item.ExternalUrl.Sanitize());
        return (null, new CategorizationResult
        {
            Item = null,
            Explanation = $"AI categorization failed after {_options.MaxRetries} attempts",
            IsFailure = true
        });
    }

    /// <summary>
    /// Reads the first choice of a chat completion response and checks that it holds complete text.
    /// </summary>
    private (string? Content, CategorizationResult? Failure) ReadChoice(string responseJson, RawFeedItem source)
    {
        try
        {
            // Sanitize the outer API response JSON first — Azure OpenAI can occasionally
            // return improperly escaped characters (e.g. \. instead of \\.) in the content field.
            responseJson = JsonEscapeSanitizer.SanitizeJsonEscapes(responseJson);
            using var doc = JsonDocument.Parse(responseJson);
            var choice = doc.RootElement.GetProperty("choices")[0];

            var finishReason = choice.TryGetProperty("finish_reason", out var frProp)
                ? frProp.GetString()
                : null;

            var usageSuffix = ExtractUsageSuffix(doc.RootElement);

            var content = choice
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning("AI returned empty response for {Url} (finish_reason: {FinishReason}){Usage}",
                    source.ExternalUrl.Sanitize(), finishReason ?? "unknown", usageSuffix);
                return (null, new CategorizationResult
                {
                    Explanation = $"AI returned empty response (finish_reason: {finishReason ?? "unknown"}){usageSuffix}",
                    IsFailure = true
                });
            }

            if (finishReason != null && !string.Equals(finishReason, "stop", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("AI response has unexpected finish_reason '{FinishReason}' for {Url}{Usage}",
                    finishReason, source.ExternalUrl.Sanitize(), usageSuffix);
                return (null, new CategorizationResult
                {
                    Explanation = $"AI response incomplete (finish_reason: {finishReason}){usageSuffix}",
                    IsFailure = true
                });
            }

            return (content, null);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse AI response JSON for {Url}", source.ExternalUrl.Sanitize());
            return (null, new CategorizationResult { Explanation = $"Failed to parse AI response: {ex.Message}", IsFailure = true });
        }
        catch (Exception ex) when (ex is InvalidOperationException or KeyNotFoundException)
        {
            _logger.LogError(ex, "Failed to parse AI response structure for {Url}", source.ExternalUrl.Sanitize());
            return (null, new CategorizationResult { Explanation = $"Invalid AI response structure: {ex.Message}", IsFailure = true });
        }
    }

    private CategorizationResult ParseClassification(string responseJson, RawFeedItem source)
    {
        var choice = ReadChoice(responseJson, source);
        if (choice.Failure is not null)
        {
            return choice.Failure;
        }

        try
        {
            var jsonContent = ExtractJsonFromResponse(choice.Content!);
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                _logger.LogWarning("AI response did not contain valid JSON for {Url}", source.ExternalUrl.Sanitize());
                return new CategorizationResult { Explanation = "AI response did not contain valid JSON", IsFailure = true };
            }

            jsonContent = JsonEscapeSanitizer.SanitizeJsonEscapes(jsonContent);

            using var aiDoc = JsonDocument.Parse(jsonContent);
            var root = aiDoc.RootElement;

            var included = GetBool(root, "included");
            var explanation = GetString(root, "explanation") ?? string.Empty;

            if (included is null)
            {
                _logger.LogWarning("AI response has no 'included' flag for {Url}", source.ExternalUrl.Sanitize());
                return new CategorizationResult { Explanation = "AI response has no 'included' flag", IsFailure = true };
            }

            if (included == false)
            {
                if (string.IsNullOrWhiteSpace(explanation))
                {
                    _logger.LogWarning("AI excluded item but provided no explanation for {Url}", source.ExternalUrl.Sanitize());
                    return new CategorizationResult { Explanation = "AI excluded item but provided no explanation", IsFailure = true };
                }

                return new CategorizationResult { Explanation = explanation };
            }

            return BuildIncludedItem(root, explanation, source);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse AI response JSON for {Url}", source.ExternalUrl.Sanitize());
            return new CategorizationResult { Explanation = $"Failed to parse AI response: {ex.Message}", IsFailure = true };
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to parse AI response structure for {Url}", source.ExternalUrl.Sanitize());
            return new CategorizationResult { Explanation = $"Invalid AI response structure: {ex.Message}", IsFailure = true };
        }
    }

    private CategorizationResult BuildIncludedItem(JsonElement root, string explanation, RawFeedItem source)
    {
        var title = GetString(root, "title");
        var excerpt = GetString(root, "excerpt");
        var primarySection = GetString(root, "primary_section");
        var tags = GetStringArray(root, "tags");
        var sections = GetStringArray(root, "sections");
        var author = GetString(root, "author");

        // ── Output validation ─────────────────────────────────────────────
        // Reject the entire item if any required field is missing or invalid.
        // We do NOT silently fix bad data — the item is treated as failed.

        if (sections.Count == 0)
        {
            _logger.LogWarning("AI marked item as included but returned no sections for {Title}", source.Title.Sanitize());
            return Failure("AI marked item as included but returned no sections");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            _logger.LogWarning("AI returned item with empty title: {Url}", source.ExternalUrl.Sanitize());
            return Failure(string.IsNullOrWhiteSpace(explanation) ? "AI returned empty title" : explanation);
        }

        if (string.IsNullOrWhiteSpace(excerpt))
        {
            _logger.LogWarning("AI returned item with no excerpt: {Title}", source.Title.Sanitize());
            return Failure(string.IsNullOrWhiteSpace(explanation) ? "AI returned no excerpt" : explanation);
        }

        var invalidSections = sections.Where(s => !_validSections.Contains(s)).ToList();
        if (invalidSections.Count > 0)
        {
            _logger.LogWarning("AI returned invalid sections [{InvalidSections}] for {Title}",
                string.Join(", ", invalidSections).Sanitize(), source.Title.Sanitize());
            return Failure($"AI returned invalid sections: {string.Join(", ", invalidSections)}");
        }

        if (tags.Count == 0)
        {
            _logger.LogWarning("AI returned item with no tags: {Title}", source.Title.Sanitize());
            return Failure(string.IsNullOrWhiteSpace(explanation) ? "AI returned no tags" : explanation);
        }

        if (string.IsNullOrWhiteSpace(primarySection))
        {
            _logger.LogWarning("AI returned item with no primary_section: {Title}", source.Title.Sanitize());
            return Failure(string.IsNullOrWhiteSpace(explanation) ? "AI returned no primary_section" : explanation);
        }

        if (!_validSections.Contains(primarySection))
        {
            _logger.LogWarning("AI returned invalid primary_section '{PrimarySection}' for {Title}",
                primarySection.Sanitize(), source.Title.Sanitize());
            return Failure($"AI returned invalid primary_section: {primarySection}");
        }

        if (!sections.Contains(primarySection, StringComparer.OrdinalIgnoreCase))
        {
            _logger.LogWarning("AI primary_section '{PrimarySection}' is not in sections [{Sections}] for {Title}",
                primarySection.Sanitize(), string.Join(", ", sections).Sanitize(), source.Title.Sanitize());
            return Failure($"AI primary_section '{primarySection}' is not in sections list");
        }

        var metadataResult = ParseRoundupMetadata(root, sections, primarySection, source);
        if (metadataResult.Failure is not null)
        {
            return metadataResult.Failure;
        }

        var timelineEvents = root.TryGetProperty("timeline_events", out var eventsProp)
            ? TimelineEventParser.Parse(eventsProp, TimelineEventParser.GetBrusselsDate(source.PublishedAt), _logger, source.ExternalUrl)
            : [];

        return new CategorizationResult
        {
            Item = new ProcessedContentItem
            {
                Slug = GenerateSlug(title),
                Title = title,
                Excerpt = excerpt,
                DateEpoch = source.PublishedAt.ToUnixTimeSeconds(),
                CollectionName = source.CollectionName,
                ExternalUrl = source.ExternalUrl,
                Author = source.AuthorOverride ?? author ?? source.FeedLevelAuthor,
                FeedName = source.FeedName,
                Tags = tags,
                Sections = sections,
                PrimarySectionName = primarySection,
                ContentHash = ComputeHash(title + excerpt),
                RoundupMetadata = metadataResult.Metadata,
                TimelineEvents = timelineEvents
            },
            Explanation = explanation
        };
    }

    private (RoundupMetadata? Metadata, CategorizationResult? Failure) ParseRoundupMetadata(
        JsonElement root,
        List<string> sections,
        string primarySection,
        RawFeedItem source)
    {
        var keyFacts = GetStringArray(root, "key_facts").Select(f => f.Trim()).Take(MaxKeyFacts).ToList();
        if (keyFacts.Count == 0)
        {
            _logger.LogWarning("AI returned no key facts for {Title}", source.Title.Sanitize());
            return (null, Failure("AI returned no key facts"));
        }

        var significance = GetString(root, "significance")?.ToLowerInvariant();
        if (significance is null || !RoundupMetadata.ValidSignificance.Contains(significance))
        {
            _logger.LogWarning("AI returned invalid significance '{Value}' for {Title}", significance.Sanitize(), source.Title.Sanitize());
            return (null, Failure($"AI returned invalid significance: {significance}"));
        }

        var contentKind = GetString(root, "content_kind")?.ToLowerInvariant();
        if (contentKind is null || !RoundupMetadata.ValidContentKinds.Contains(contentKind))
        {
            _logger.LogWarning("AI returned invalid content_kind '{Value}' for {Title}", contentKind.Sanitize(), source.Title.Sanitize());
            return (null, Failure($"AI returned invalid content_kind: {contentKind}"));
        }

        var lifecycle = GetString(root, "lifecycle")?.ToLowerInvariant();
        if (lifecycle is null || !RoundupMetadata.ValidLifecycles.Contains(lifecycle))
        {
            _logger.LogWarning("AI returned invalid lifecycle '{Value}' for {Title}", lifecycle.Sanitize(), source.Title.Sanitize());
            return (null, Failure($"AI returned invalid lifecycle: {lifecycle}"));
        }

        var sectionRoles = ParseSectionRoles(root, sections, primarySection, source, out var rolesFailure);
        if (rolesFailure is not null)
        {
            return (null, rolesFailure);
        }

        var products = GetStringArray(root, "products")
            .Select(p => p.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(MaxProducts)
            .ToList();

        return (new RoundupMetadata
        {
            SchemaVersion = RoundupMetadata.CurrentSchemaVersion,
            KeyFacts = keyFacts,
            Significance = significance,
            ContentKind = contentKind,
            Lifecycle = lifecycle,
            Products = products,
            SectionRoles = sectionRoles!
        }, null);
    }

    /// <summary>
    /// Every section gets a role. The primary section is always a lead, and a section without a role defaults
    /// to lead ("when in doubt, lead": the worst case is duplication, never a missing story).
    /// A role for a section the item does not belong to is invalid output.
    /// </summary>
    private Dictionary<string, string>? ParseSectionRoles(
        JsonElement root,
        List<string> sections,
        string primarySection,
        RawFeedItem source,
        out CategorizationResult? failure)
    {
        failure = null;
        var given = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (root.TryGetProperty("section_roles", out var rolesProp) && rolesProp.ValueKind == JsonValueKind.Array)
        {
            foreach (var entry in rolesProp.EnumerateArray())
            {
                var section = GetString(entry, "section");
                var role = GetString(entry, "role")?.ToLowerInvariant();

                if (section is null || !sections.Contains(section, StringComparer.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("AI returned a section role for '{Section}', which is not in sections, for {Title}",
                        section.Sanitize(), source.Title.Sanitize());
                    failure = Failure($"AI returned a section role for a section that is not in the sections list: {section}");
                    return null;
                }

                if (role is not (RoundupMetadata.RoleLead or RoundupMetadata.RoleBrief))
                {
                    _logger.LogWarning("AI returned invalid section role '{Role}' for {Title}", role.Sanitize(), source.Title.Sanitize());
                    failure = Failure($"AI returned an invalid section role: {role}");
                    return null;
                }

                given[section] = role;
            }
        }

        var roles = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var section in sections)
        {
            var key = section.ToLowerInvariant();
            roles[key] = string.Equals(section, primarySection, StringComparison.OrdinalIgnoreCase)
                ? RoundupMetadata.RoleLead
                : given.GetValueOrDefault(section, RoundupMetadata.RoleLead);
        }

        return roles;
    }

    private static CategorizationResult Failure(string explanation) => new() { Explanation = explanation, IsFailure = true };

    internal static string BuildUserPrompt(RawFeedItem item)
    {
        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Please categorize the following content:");
        sb.AppendLine();

        if (item.SkipSalesPitchCheck)
        {
            sb.AppendLine("PROCESSING_OVERRIDE: SkipSalesPitchCheck=true — Do not apply the \"Sales Pitches\" generic exclusion rule for this item. If the content qualifies on any other grounds, categorize it normally.");
            sb.AppendLine();
        }

        sb.AppendLine(CultureInfo.InvariantCulture, $"FEED: {item.FeedName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"COLLECTION: {item.CollectionName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"URL: {item.ExternalUrl}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"DATE: {item.PublishedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"FALLBACK_AUTHOR: {item.FeedLevelAuthor}");

        AppendSourceContent(sb, item);
        return sb.ToString();
    }

    internal static string BuildRewritePrompt(RawFeedItem item, ProcessedContentItem classified)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Please rewrite the following article.");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"TITLE: {classified.Title}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"AUTHOR: {classified.Author}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"EXCERPT: {classified.Excerpt}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"SECTIONS: {string.Join(", ", classified.Sections)}");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"FEED: {item.FeedName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"COLLECTION: {item.CollectionName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"URL: {item.ExternalUrl}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"DATE: {item.PublishedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");

        AppendSourceContent(sb, item);
        return sb.ToString();
    }

    private static void AppendSourceContent(StringBuilder sb, RawFeedItem item)
    {
        if (!string.IsNullOrWhiteSpace(item.FeedItemData))
        {
            sb.AppendLine();
            sb.AppendLine("FEED_ITEM_DATA:");
            sb.AppendLine(item.FeedItemData);
        }

        if (!string.IsNullOrWhiteSpace(item.FullContent))
        {
            sb.AppendLine();
            sb.AppendLine(item.IsYouTube ? "TRANSCRIPT:" : "CONTENT:");
            sb.AppendLine(item.FullContent);
        }

        if (item.FeedTags.Count > 0)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"FEED TAGS: {string.Join(", ", item.FeedTags)}");
        }
    }

    private static string ExtractJsonFromResponse(string response)
    {
        var cleaned = Regex.Replace(response, @"```(?:json)?\s*", string.Empty, RegexOptions.IgnoreCase).Trim();
        var start = cleaned.IndexOf('{', StringComparison.Ordinal);
        var end = cleaned.LastIndexOf('}');
        if (start < 0 || end < 0 || end <= start)
        {
            return string.Empty;
        }

        return cleaned[start..(end + 1)];
    }

    /// <summary>
    /// Removes a code fence around the whole answer. Models occasionally wrap a markdown answer in
    /// <c>```markdown</c> even when told not to.
    /// </summary>
    internal static string UnwrapMarkdownFence(string content)
    {
        var trimmed = content.Trim();
        var match = Regex.Match(trimmed, @"\A```(?:markdown|md)[ \t]*\r?\n(?<body>.*)\r?\n```\z", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        return match.Success ? match.Groups["body"].Value.Trim() : trimmed;
    }

    /// <summary>
    /// Extracts prompt/completion/total token counts from the Azure OpenAI usage object
    /// and returns a human-readable suffix string for diagnostic messages.
    /// Returns empty string if usage data is not available.
    /// </summary>
    private static string ExtractUsageSuffix(JsonElement root)
    {
        if (!root.TryGetProperty("usage", out var usage))
        {
            return string.Empty;
        }

        var promptTokens = usage.TryGetProperty("prompt_tokens", out var pt) && pt.ValueKind == JsonValueKind.Number
            ? pt.GetInt32()
            : (int?)null;
        var completionTokens = usage.TryGetProperty("completion_tokens", out var ct) && ct.ValueKind == JsonValueKind.Number
            ? ct.GetInt32()
            : (int?)null;
        var totalTokens = usage.TryGetProperty("total_tokens", out var tt) && tt.ValueKind == JsonValueKind.Number
            ? tt.GetInt32()
            : (int?)null;

        if (promptTokens == null && completionTokens == null && totalTokens == null)
        {
            return string.Empty;
        }

        return string.Create(CultureInfo.InvariantCulture,
            $" (tokens: prompt={promptTokens?.ToString(CultureInfo.InvariantCulture) ?? "?"}, completion={completionTokens?.ToString(CultureInfo.InvariantCulture) ?? "?"}, total={totalTokens?.ToString(CultureInfo.InvariantCulture) ?? "?"})");
    }

    private static string? GetString(JsonElement root, string name)
    {
        if (root.TryGetProperty(name, out var prop) && prop.ValueKind == JsonValueKind.String)
        {
            return prop.GetString();
        }

        return null;
    }

    private static bool? GetBool(JsonElement root, string name)
    {
        if (root.TryGetProperty(name, out var prop) && (prop.ValueKind == JsonValueKind.True || prop.ValueKind == JsonValueKind.False))
        {
            return prop.GetBoolean();
        }

        return null;
    }

    private static List<string> GetStringArray(JsonElement root, string name)
    {
        if (!root.TryGetProperty(name, out var prop) || prop.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return prop.EnumerateArray()
            .Where(e => e.ValueKind == JsonValueKind.String)
            .Select(e => e.GetString()!)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();
    }

    private static string GenerateSlug(string title)
    {
        // Date prefix intentionally omitted — canonical slugs are title-only.
        // Existing date-prefixed URLs are handled by UrlNormalizationMiddleware (301 redirect).
        return Regex.Replace(title.ToLowerInvariant(), @"[^a-z0-9]+", "-").Trim('-');
    }

    private static string ComputeHash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToHexStringLower(SHA256.HashData(bytes));
    }

    private static string LoadClassifySystemMessage()
    {
        return LoadResource("content-classify-system.md")
            .Replace("{{TIMELINE_RULES}}", LoadResource("content-timeline-rules.md").TrimEnd(), StringComparison.Ordinal);
    }

    private static string LoadResource(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = string.Create(CultureInfo.InvariantCulture, $"TechHub.Infrastructure.Data.Resources.{fileName}");
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found. Ensure {fileName} is included as an embedded resource.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
