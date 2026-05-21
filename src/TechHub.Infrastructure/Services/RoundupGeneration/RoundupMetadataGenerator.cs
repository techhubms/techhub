using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Step 7: Generates roundup metadata (title, tags, introduction) using AI.
/// </summary>
internal sealed class RoundupMetadataGenerator
{
    private static readonly Lazy<string> _systemPrompt = RoundupAiHelper.LoadResource("roundup-metadata-generator-system.md");

    private readonly RoundupAiHelper _aiHelper;
    private readonly RoundupGeneratorOptions _options;
    private readonly AppSettings _settings;
    private readonly ILogger<RoundupMetadataGenerator> _logger;

    public RoundupMetadataGenerator(
        RoundupAiHelper aiHelper,
        RoundupGeneratorOptions options,
        IOptions<AppSettings> settings,
        ILogger<RoundupMetadataGenerator> logger)
    {
        ArgumentNullException.ThrowIfNull(aiHelper);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(logger);

        _aiHelper = aiHelper;
        _options = options;
        _settings = settings.Value;
        _logger = logger;
    }

    /// <summary>
    /// Generates roundup metadata by calling AI and parsing the JSON response.
    /// Falls back to section-specific metadata if all retries fail.
    /// </summary>
    public async Task<RoundupMetadataAi> GenerateAsync(
        string condensedContent,
        string sectionName,
        string weekDescription,
        string writingGuidelines,
        CancellationToken ct)
    {
        var sectionTag = _settings.Content.Sections.TryGetValue(sectionName, out var sc)
            ? sc.Tag
            : sectionName;

        var systemMessage = _systemPrompt.Value
            .Replace("{WritingStyleGuidelines}", writingGuidelines, StringComparison.Ordinal);

        var userMessage = string.Create(CultureInfo.InvariantCulture,
            $"Generate metadata for the {sectionTag} section roundup covering {weekDescription} based on this condensed content:\n\n" +
            $"{condensedContent}\n\nReturn only JSON with fields: title, tags, introduction");

        for (var attempt = 0; attempt < _options.MaxRetries; attempt++)
        {
            var response = await _aiHelper.CallAiWithRetryAsync(systemMessage, userMessage, "Step 4", ct);

            if (response is not null)
            {
                try
                {
                    var meta = JsonSerializer.Deserialize<RoundupMetadataAi>(response, RoundupAiHelper.JsonOptions);
                    if (meta is not null && !string.IsNullOrWhiteSpace(meta.Title))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(_options.RateLimitDelaySeconds), ct);
                        return meta;
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Step 4: Failed to parse metadata JSON (attempt {Attempt})", attempt + 1);
                }
            }
        }

        _logger.LogWarning("Step 4: Metadata generation failed after retries, using fallback metadata");
        return new RoundupMetadataAi
        {
            Title = string.Create(CultureInfo.InvariantCulture, $"Weekly {sectionTag} Roundup"),
            Tags = [sectionTag, "Roundups"],
            Introduction = string.Create(CultureInfo.InvariantCulture, $"Welcome to this week's {sectionTag} roundup covering {weekDescription}.")
        };
    }

    /// <summary>
    /// AI-generated metadata for a weekly roundup.
    /// </summary>
    internal sealed class RoundupMetadataAi
    {
        public string Title { get; init; } = string.Empty;
        public IReadOnlyList<string> Tags { get; init; } = [];
        public string Introduction { get; init; } = string.Empty;
    }
}
