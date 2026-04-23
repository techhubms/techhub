using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Shared AI call logic used by the roundup generation pipeline steps.
/// </summary>
internal sealed class RoundupAiHelper
{
    private readonly IAiCompletionClient _aiClient;
    private readonly RoundupGeneratorOptions _options;
    private readonly ILogger<RoundupAiHelper> _logger;

    public RoundupAiHelper(
        IAiCompletionClient aiClient,
        RoundupGeneratorOptions options,
        ILogger<RoundupAiHelper> logger)
    {
        ArgumentNullException.ThrowIfNull(aiClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _aiClient = aiClient;
        _options = options;
        _logger = logger;
    }

    /// <summary>
    /// Gets the shared JSON serializer options (snake_case, skip nulls).
    /// </summary>
    public static JsonSerializerOptions JsonOptions { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Gets the configured rate-limit delay in seconds.
    /// </summary>
    public int RateLimitDelaySeconds => _options.RateLimitDelaySeconds;

    /// <summary>
    /// Gets the maximum number of retries for AI calls.
    /// </summary>
    public int MaxRetries => _options.MaxRetries;

    /// <summary>
    /// Sends a chat completion request with retry logic for rate-limiting and transient HTTP errors.
    /// </summary>
    public async Task<string?> CallAiWithRetryAsync(
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

        var json = JsonSerializer.Serialize(requestBody, JsonOptions);

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

    /// <summary>
    /// Extracts the content string from an OpenAI chat completion JSON response.
    /// </summary>
    public string? ExtractContentFromResponse(string responseJson)
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

    /// <summary>
    /// Lazily loads an embedded resource from the Infrastructure assembly.
    /// </summary>
    public static Lazy<string> LoadResource(string fileName) => new(() =>
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = string.Create(System.Globalization.CultureInfo.InvariantCulture,
            $"TechHub.Infrastructure.Data.Resources.{fileName}");

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(string.Create(System.Globalization.CultureInfo.InvariantCulture,
                $"Embedded resource '{resourceName}' not found."));

        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    });

    /// <summary>
    /// Parses markdown content into sections keyed by <c>## </c> header title.
    /// </summary>
    public static Dictionary<string, string> ParseSections(string content)
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

    /// <summary>
    /// Extracts a single section from markdown content by its <c>## </c> header name.
    /// </summary>
    public static string ExtractSectionFromContent(string content, string sectionName)
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
}
