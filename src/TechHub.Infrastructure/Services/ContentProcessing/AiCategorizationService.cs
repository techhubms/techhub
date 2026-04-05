using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Calls Azure OpenAI to categorize a raw feed item and produce a <see cref="ProcessedContentItem"/>.
/// Also extracts roundup-ready metadata (summary, key topics, relevance) in the same call.
/// </summary>
public sealed class AiCategorizationService : IAiCategorizationService
{
    private readonly IAiCompletionClient _completionClient;
    private readonly AiCategorizationOptions _options;
    private readonly ILogger<AiCategorizationService> _logger;

    private static readonly string _systemMessage = LoadSystemMessage();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Valid section names that map to database boolean columns and bitmask positions.
    /// Any section returned by the AI that is not in this set is discarded.
    /// </summary>
    private static readonly HashSet<string> _validSections = new(StringComparer.OrdinalIgnoreCase)
    {
        "ai", "azure", "dotnet", "devops", "github-copilot", "ml", "security"
    };

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

    /// <summary>
    /// Calls Azure OpenAI to categorize <paramref name="item"/>.
    /// Returns a <see cref="CategorizationResult"/> containing the processed item (or <c>null</c> if excluded)
    /// and the AI's explanation for why the content was included or excluded.
    /// </summary>
    public async Task<CategorizationResult> CategorizeAsync(RawFeedItem item, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        var userContent = BuildUserPrompt(item);

        // Estimate prompt tokens (~4 chars/token) and allow half that for completion.
        // The output is structured JSON (summary + categories), never the full article.
        // Floor at 2000 tokens so short articles still get enough output space.
        var estimatedPromptTokens = (_systemMessage.Length + userContent.Length) / 4;
        var completionTokens = Math.Max(2000, estimatedPromptTokens / 2);

        var requestBody = new
        {
            messages = new[]
            {
                new { role = "system", content = _systemMessage },
                new { role = "user", content = userContent }
            },
            temperature = 0.1,
            max_completion_tokens = completionTokens
        };

        var json = JsonSerializer.Serialize(requestBody, _jsonOptions);
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
                    _logger.LogWarning("AI rate limit hit, waiting {Delay}s before retry {Attempt}/{Max}",
                        delaySeconds, attempt, _options.MaxRetries);
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds), ct);
                    continue;
                }

                if (result.ContentFilterMessage != null)
                {
                    _logger.LogWarning("AI content filter triggered for {Url}: {Message}",
                        item.ExternalUrl, result.ContentFilterMessage);
                    return new CategorizationResult { Explanation = result.ContentFilterMessage, IsFailure = true };
                }

                return ParseAiResponse(result.ResponseBody!, item);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex) when (attempt < _options.MaxRetries
                && ex is HttpRequestException or TaskCanceledException or OperationCanceledException or TimeoutException)
            {
                var delaySeconds = 5 * attempt;
                _logger.LogWarning(ex, "AI API call failed (attempt {Attempt}/{Max}), retrying in {Delay}s",
                    attempt, _options.MaxRetries, delaySeconds);
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds), ct);
            }
        }

        _logger.LogError("AI categorization failed after {MaxRetries} attempts for {Url}", _options.MaxRetries, item.ExternalUrl);
        return new CategorizationResult
        {
            Item = null,
            Explanation = $"AI categorization failed after {_options.MaxRetries} attempts",
            IsFailure = true
        };
    }

    private CategorizationResult ParseAiResponse(string responseJson, RawFeedItem source)
    {
        try
        {
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
                    source.ExternalUrl, finishReason ?? "unknown", usageSuffix);
                return new CategorizationResult
                {
                    Explanation = $"AI returned empty response (finish_reason: {finishReason ?? "unknown"}){usageSuffix}",
                    IsFailure = true
                };
            }

            if (finishReason != null && !string.Equals(finishReason, "stop", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("AI response has unexpected finish_reason '{FinishReason}' for {Url}{Usage}",
                    finishReason, source.ExternalUrl, usageSuffix);
                return new CategorizationResult
                {
                    Explanation = $"AI response incomplete (finish_reason: {finishReason}){usageSuffix}",
                    IsFailure = true
                };
            }

            var jsonContent = ExtractJsonFromResponse(content);
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                _logger.LogWarning("AI response did not contain valid JSON for {Url}", source.ExternalUrl);
                return new CategorizationResult { Explanation = "AI response did not contain valid JSON", IsFailure = true };
            }

            using var aiDoc = JsonDocument.Parse(jsonContent);
            var root = aiDoc.RootElement;

            // Extract explanation from the AI's response (available for both included and excluded items)
            var explanation = GetString(root, "explanation") ?? string.Empty;

            var title = GetString(root, "title");
            var excerpt = GetString(root, "excerpt");
            var primarySection = GetString(root, "primary_section");
            var tags = GetStringArray(root, "tags");
            var sections = GetStringArray(root, "sections");
            var itemContent = GetString(root, "content");
            var author = GetString(root, "author");

            // If there are no sections, determine whether this is a legitimate skip or a failure.
            // Option B response: AI returns only an explanation (no content fields) → legitimate skip.
            // But if the AI returned content fields (title, content) with empty sections → failure.
            if (sections.Count == 0)
            {
                var hasContentFields = !string.IsNullOrWhiteSpace(title) || !string.IsNullOrWhiteSpace(itemContent);
                if (hasContentFields)
                {
                    _logger.LogWarning("AI returned content fields but no sections for {Title}", source.Title);
                    return new CategorizationResult { Explanation = "AI returned content fields but no valid sections", IsFailure = true };
                }

                return new CategorizationResult { Explanation = explanation };
            }

            // ── Comprehensive AI output validation ────────────────────────────
            // Reject the entire item if any required field is missing or invalid.
            // We do NOT silently fix bad data — the item is treated as failed.

            if (string.IsNullOrWhiteSpace(title))
            {
                _logger.LogWarning("AI returned item with empty title: {Url}", source.ExternalUrl);
                return new CategorizationResult { Explanation = string.IsNullOrWhiteSpace(explanation) ? "AI returned empty title" : explanation, IsFailure = true };
            }

            if (string.IsNullOrWhiteSpace(excerpt))
            {
                _logger.LogWarning("AI returned item with no excerpt: {Title}", source.Title);
                return new CategorizationResult { Explanation = string.IsNullOrWhiteSpace(explanation) ? "AI returned no excerpt" : explanation, IsFailure = true };
            }

            if (string.IsNullOrWhiteSpace(itemContent))
            {
                _logger.LogWarning("AI returned item with no content: {Title}", source.Title);
                return new CategorizationResult { Explanation = string.IsNullOrWhiteSpace(explanation) ? "AI returned no content" : explanation, IsFailure = true };
            }

            // All sections must be from the known set — reject if any are invalid
            var invalidSections = sections.Where(s => !_validSections.Contains(s)).ToList();
            if (invalidSections.Count > 0)
            {
                _logger.LogWarning("AI returned invalid sections [{InvalidSections}] for {Title}",
                    string.Join(", ", invalidSections), source.Title);
                return new CategorizationResult { Explanation = $"AI returned invalid sections: {string.Join(", ", invalidSections)}", IsFailure = true };
            }

            if (tags.Count == 0)
            {
                _logger.LogWarning("AI returned item with no tags: {Title}", source.Title);
                return new CategorizationResult { Explanation = string.IsNullOrWhiteSpace(explanation) ? "AI returned no tags" : explanation, IsFailure = true };
            }

            // Validate primary_section: must be present and a known section
            if (string.IsNullOrWhiteSpace(primarySection))
            {
                _logger.LogWarning("AI returned item with no primary_section: {Title}", source.Title);
                return new CategorizationResult { Explanation = string.IsNullOrWhiteSpace(explanation) ? "AI returned no primary_section" : explanation, IsFailure = true };
            }

            if (!_validSections.Contains(primarySection))
            {
                _logger.LogWarning("AI returned invalid primary_section '{PrimarySection}' for {Title}",
                    primarySection, source.Title);
                return new CategorizationResult { Explanation = $"AI returned invalid primary_section: {primarySection}", IsFailure = true };
            }

            if (!sections.Contains(primarySection, StringComparer.OrdinalIgnoreCase))
            {
                _logger.LogWarning("AI primary_section '{PrimarySection}' is not in sections [{Sections}] for {Title}",
                    primarySection, string.Join(", ", sections), source.Title);
                return new CategorizationResult { Explanation = $"AI primary_section '{primarySection}' is not in sections list", IsFailure = true };
            }

            // All required fields validated — safe to generate slug and hash
            var slug = GenerateSlug(title, source.PublishedAt);
            var contentHash = ComputeHash(title + itemContent + excerpt);

            // Extract roundup metadata
            RoundupMetadata? roundupMetadata = null;
            if (root.TryGetProperty("roundup", out var roundupProp))
            {
                var roundupSummary = GetString(roundupProp, "summary");
                var roundupRelevance = GetString(roundupProp, "relevance");
                var roundupTopicType = GetString(roundupProp, "topic_type");
                var roundupImpactLevel = GetString(roundupProp, "impact_level");
                var roundupTimeSensitivity = GetString(roundupProp, "time_sensitivity");

                if (string.IsNullOrWhiteSpace(roundupSummary) || string.IsNullOrWhiteSpace(roundupRelevance)
                    || string.IsNullOrWhiteSpace(roundupTopicType) || string.IsNullOrWhiteSpace(roundupImpactLevel)
                    || string.IsNullOrWhiteSpace(roundupTimeSensitivity))
                {
                    _logger.LogWarning("AI returned incomplete roundup metadata for {Title}", source.Title);
                    return new CategorizationResult { Explanation = "AI returned incomplete roundup metadata", IsFailure = true };
                }

                roundupMetadata = new RoundupMetadata
                {
                    Summary = roundupSummary,
                    KeyTopics = GetStringArray(roundupProp, "key_topics"),
                    Relevance = roundupRelevance,
                    TopicType = roundupTopicType,
                    ImpactLevel = roundupImpactLevel,
                    TimeSensitivity = roundupTimeSensitivity
                };
            }

            return new CategorizationResult
            {
                Item = new ProcessedContentItem
                {
                    Slug = slug,
                    Title = title,
                    Content = itemContent,
                    Excerpt = excerpt,
                    DateEpoch = source.PublishedAt.ToUnixTimeSeconds(),
                    CollectionName = source.CollectionName,
                    ExternalUrl = source.ExternalUrl,
                    Author = author ?? source.FeedLevelAuthor,
                    FeedName = source.FeedName,
                    Tags = tags,
                    Sections = sections,
                    PrimarySectionName = primarySection,
                    ContentHash = contentHash,
                    RoundupMetadata = roundupMetadata
                },
                Explanation = explanation
            };
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse AI response JSON for {Url}", source.ExternalUrl);
            return new CategorizationResult { Explanation = $"Failed to parse AI response: {ex.Message}", IsFailure = true };
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to parse AI response structure for {Url}", source.ExternalUrl);
            return new CategorizationResult { Explanation = $"Invalid AI response structure: {ex.Message}", IsFailure = true };
        }
    }

    private static string BuildUserPrompt(RawFeedItem item)
    {
        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Please categorize the following content:");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"FEED: {item.FeedName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"COLLECTION: {item.CollectionName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"URL: {item.ExternalUrl}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"DATE: {item.PublishedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"FALLBACK_AUTHOR: {item.FeedLevelAuthor}");

        if (!string.IsNullOrWhiteSpace(item.FeedItemData))
        {
            sb.AppendLine();
            sb.AppendLine("FEED_ITEM_DATA:");
            sb.AppendLine(item.FeedItemData);
        }

        if (!string.IsNullOrWhiteSpace(item.FullContent))
        {
            sb.AppendLine();
            if (item.IsYouTube)
            {
                sb.AppendLine("TRANSCRIPT (auto-generated from video closed captions):");
            }
            else
            {
                sb.AppendLine("CONTENT:");
            }

            sb.AppendLine(item.FullContent);
        }

        if (item.FeedTags.Count > 0)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"FEED TAGS: {string.Join(", ", item.FeedTags)}");
        }

        return sb.ToString();
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

    private static string GenerateSlug(string title, DateTimeOffset date)
    {
        var datePrefix = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var slugBase = Regex.Replace(title.ToLowerInvariant(), @"[^a-z0-9]+", "-").Trim('-');
        return string.Create(CultureInfo.InvariantCulture, $"{datePrefix}-{slugBase}");
    }

    private static string ComputeHash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToHexStringLower(SHA256.HashData(bytes));
    }

    private static string LoadSystemMessage()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string ResourceName = "TechHub.Infrastructure.Data.Resources.system-message.md";
        using var stream = assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{ResourceName}' not found. Ensure system-message.md is included as an embedded resource.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
