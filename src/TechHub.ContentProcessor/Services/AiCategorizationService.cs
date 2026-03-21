using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using TechHub.ContentProcessor.Models;
using TechHub.ContentProcessor.Options;

namespace TechHub.ContentProcessor.Services;

/// <summary>
/// Calls Azure OpenAI to categorize a raw feed item and produce a <see cref="ProcessedContentItem"/>.
/// </summary>
public sealed class AiCategorizationService
{
    private readonly HttpClient _httpClient;
    private readonly AiCategorizationOptions _options;
    private readonly ILogger<AiCategorizationService> _logger;

    // Loaded once at construction time from embedded resource
    private static readonly string SystemMessage = LoadSystemMessage();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public AiCategorizationService(
        HttpClient httpClient,
        IOptions<AiCategorizationOptions> options,
        ILogger<AiCategorizationService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Calls Azure OpenAI with the system message and item content to produce a
    /// <see cref="ProcessedContentItem"/>.
    /// Returns <see langword="null"/> when the AI determines the content should be skipped.
    /// </summary>
    public async Task<ProcessedContentItem?> CategorizeAsync(RawFeedItem item, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!_options.Enabled)
        {
            return null;
        }

        var userContent = BuildUserPrompt(item);

        // Truncate to stay within model context window
        if (userContent.Length > _options.MaxContentLength)
        {
            userContent = userContent[.._options.MaxContentLength];
        }

        var requestBody = new
        {
            messages = new[]
            {
                new { role = "system", content = SystemMessage },
                new { role = "user", content = userContent }
            },
            temperature = 0.1,
            max_tokens = 2000
        };

        var json = JsonSerializer.Serialize(requestBody, JsonOptions);
        var attempt = 0;

        while (attempt < _options.MaxRetries)
        {
            attempt++;

            try
            {
                using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    string.Create(CultureInfo.InvariantCulture,
                        $"{_options.Endpoint.TrimEnd('/')}/openai/deployments/{_options.DeploymentName}/chat/completions?api-version=2024-10-21"));

                request.Headers.Add("api-key", _options.ApiKey);
                request.Content = new StringContent(json, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));

                using var response = await _httpClient.SendAsync(request, ct);

                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    _logger.LogWarning("AI rate limit hit, waiting before retry {Attempt}/{Max}", attempt, _options.MaxRetries);
                    await Task.Delay(TimeSpan.FromSeconds(_options.RateLimitDelaySeconds * attempt), ct);
                    continue;
                }

                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync(ct);
                return ParseAiResponse(responseJson, item);
            }
            catch (HttpRequestException ex) when (attempt < _options.MaxRetries)
            {
                _logger.LogWarning(ex, "AI API call failed (attempt {Attempt}/{Max}), retrying", attempt, _options.MaxRetries);
                await Task.Delay(TimeSpan.FromSeconds(5 * attempt), ct);
            }
        }

        _logger.LogError("AI categorization failed after {MaxRetries} attempts for {Url}", _options.MaxRetries, item.ExternalUrl);
        return null;
    }

    private ProcessedContentItem? ParseAiResponse(string responseJson, RawFeedItem source)
    {
        try
        {
            using var doc = JsonDocument.Parse(responseJson);
            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            // Extract JSON object from AI response (may be wrapped in markdown code fences)
            var jsonContent = ExtractJsonFromResponse(content);
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                _logger.LogWarning("AI response did not contain valid JSON for {Url}", source.ExternalUrl);
                return null;
            }

            using var aiDoc = JsonDocument.Parse(jsonContent);
            var root = aiDoc.RootElement;

            // Check if the AI decided to skip this item
            if (root.TryGetProperty("skip", out var skipProp) && skipProp.GetBoolean())
            {
                _logger.LogInformation("AI decided to skip item: {Title}", source.Title);
                return null;
            }

            var title = GetStringProperty(root, "title") ?? source.Title;
            var excerpt = GetStringProperty(root, "excerpt") ?? source.Description;
            var collectionName = GetStringProperty(root, "collection") ?? source.CollectionName;
            var author = GetStringProperty(root, "author") ?? source.Author;
            var primarySection = GetStringProperty(root, "primary_section");

            var tags = GetStringArray(root, "tags");
            var sections = GetStringArray(root, "sections");

            var itemContent = GetStringProperty(root, "content") ?? string.Empty;
            var slug = GenerateSlug(title, source.PublishedAt);
            var contentHash = ComputeHash(title + itemContent + excerpt);

            return new ProcessedContentItem
            {
                Slug = slug,
                Title = title,
                Content = itemContent,
                Excerpt = TruncateExcerpt(excerpt),
                DateEpoch = source.PublishedAt.ToUnixTimeSeconds(),
                CollectionName = collectionName,
                ExternalUrl = source.ExternalUrl,
                Author = author,
                FeedName = source.FeedName,
                Tags = tags,
                Sections = sections,
                PrimarySectionName = primarySection,
                ContentHash = contentHash
            };
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse AI response JSON for {Url}", source.ExternalUrl);
            return null;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to parse AI response structure for {Url}", source.ExternalUrl);
            return null;
        }
    }

    private static string BuildUserPrompt(RawFeedItem item)
    {
        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Please categorize the following content:");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"FEED: {item.FeedName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"COLLECTION: {item.CollectionName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"TITLE: {item.Title}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"URL: {item.ExternalUrl}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"DATE: {item.PublishedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");

        if (!string.IsNullOrWhiteSpace(item.Author))
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"AUTHOR: {item.Author}");
        }

        if (!string.IsNullOrWhiteSpace(item.Description))
        {
            sb.AppendLine();
            sb.AppendLine("DESCRIPTION:");
            sb.AppendLine(item.Description);
        }

        if (!string.IsNullOrWhiteSpace(item.FullContent))
        {
            sb.AppendLine();
            sb.AppendLine("CONTENT:");
            sb.AppendLine(item.FullContent);
        }

        if (item.FeedTags.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine(CultureInfo.InvariantCulture, $"FEED TAGS: {string.Join(", ", item.FeedTags)}");
        }

        return sb.ToString();
    }

    private static string ExtractJsonFromResponse(string response)
    {
        // Remove markdown code fences if present
        var cleaned = Regex.Replace(response, @"```(?:json)?\s*", string.Empty, RegexOptions.IgnoreCase).Trim();

        // Find first { and last }
        var start = cleaned.IndexOf('{', StringComparison.Ordinal);
        var end = cleaned.LastIndexOf('}');

        if (start < 0 || end < 0 || end <= start)
        {
            return string.Empty;
        }

        return cleaned[start..(end + 1)];
    }

    private static string? GetStringProperty(JsonElement root, string name)
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

        // Truncate to reasonable length
        if (slugBase.Length > 80)
        {
            slugBase = slugBase[..80].TrimEnd('-');
        }

        return string.Create(CultureInfo.InvariantCulture, $"{datePrefix}-{slugBase}");
    }

    private static string TruncateExcerpt(string excerpt)
    {
        const int MaxExcerptLength = 300;
        if (string.IsNullOrWhiteSpace(excerpt))
        {
            return string.Empty;
        }

        excerpt = excerpt.Trim();
        if (excerpt.Length <= MaxExcerptLength)
        {
            return excerpt;
        }

        // Truncate at word boundary
        var truncated = excerpt[..MaxExcerptLength];
        var lastSpace = truncated.LastIndexOf(' ');
        return lastSpace > 0 ? truncated[..lastSpace] + "\u2026" : truncated + "\u2026";
    }

    private static string ComputeHash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexStringLower(hash);
    }

    private static string LoadSystemMessage()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string ResourceName = "TechHub.ContentProcessor.Resources.system-message.md";

        using var stream = assembly.GetManifestResourceStream(ResourceName);
        if (stream == null)
        {
            return "You are a content categorization assistant. Analyze the provided content and return a JSON object with fields: title, excerpt, collection, sections (array), tags (array), author, primary_section, content (markdown). If the content is irrelevant or low quality, return {\"skip\": true}.";
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
