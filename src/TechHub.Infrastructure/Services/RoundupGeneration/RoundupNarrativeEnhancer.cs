using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Step 4: Enhances roundup sections with ongoing narrative by comparing with the previous week's roundup.
/// </summary>
internal sealed class RoundupNarrativeEnhancer
{
    private static readonly Lazy<string> _systemPrompt = RoundupAiHelper.LoadResource("roundup-narrative-enhancer-system.md");

    private readonly RoundupAiHelper _aiHelper;
    private readonly ISectionRoundupRepository _roundupRepo;
    private readonly AppSettings _settings;
    private readonly RoundupGeneratorOptions _options;
    private readonly ILogger<RoundupNarrativeEnhancer> _logger;

    public RoundupNarrativeEnhancer(
        RoundupAiHelper aiHelper,
        ISectionRoundupRepository roundupRepo,
        IOptions<AppSettings> settings,
        RoundupGeneratorOptions options,
        ILogger<RoundupNarrativeEnhancer> logger)
    {
        ArgumentNullException.ThrowIfNull(aiHelper);
        ArgumentNullException.ThrowIfNull(roundupRepo);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _aiHelper = aiHelper;
        _roundupRepo = roundupRepo;
        _settings = settings.Value;
        _options = options;
        _logger = logger;
    }

    /// <summary>
    /// Loads the previous roundup and enhances each section with ongoing narrative context.
    /// Returns <paramref name="step3Content"/> unchanged if no previous roundup exists.
    /// </summary>
    public async Task<string> EnhanceAsync(
        string step3Content,
        DateOnly weekStart,
        string writingGuidelines,
        CancellationToken ct)
    {
        var previousRoundupContent = await _roundupRepo.GetPreviousRoundupContentAsync(weekStart, ct);

        if (string.IsNullOrWhiteSpace(previousRoundupContent))
        {
            _logger.LogInformation("Step 4: No previous roundup found, skipping narrative enhancement");
            return step3Content;
        }

        var systemMessage = _systemPrompt.Value
            .Replace("{WritingStyleGuidelines}", writingGuidelines, StringComparison.Ordinal);

        var sectionContents = RoundupAiHelper.ParseSections(step3Content);
        var responses = new List<string>();

        foreach (var (sectionSlug, sectionConfig) in _settings.Content.Sections)
        {
            if (sectionSlug.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var displayName = sectionConfig.Title;

            if (!sectionContents.TryGetValue(displayName, out var currentSection))
            {
                continue;
            }

            var previousSection = RoundupAiHelper.ExtractSectionFromContent(previousRoundupContent, displayName);

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

            var response = await _aiHelper.CallAiWithRetryAsync(systemMessage, userMessage, "Step 4 - " + displayName, ct);
            responses.Add(response ?? currentSection);

            await Task.Delay(TimeSpan.FromSeconds(_options.RateLimitDelaySeconds), ct);
        }

        return string.Join("\n\n", responses);
    }
}
