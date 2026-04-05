using Microsoft.Extensions.Logging;
using TechHub.Core.Configuration;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Step 6: Condenses roundup content using AI.
/// </summary>
internal sealed class RoundupCondenser
{
    private static readonly Lazy<string> _systemPrompt = RoundupAiHelper.LoadResource("roundup-condenser-system.md");

    private readonly RoundupAiHelper _aiHelper;
    private readonly RoundupGeneratorOptions _options;
    private readonly ILogger<RoundupCondenser> _logger;

    public RoundupCondenser(
        RoundupAiHelper aiHelper,
        RoundupGeneratorOptions options,
        ILogger<RoundupCondenser> logger)
    {
        ArgumentNullException.ThrowIfNull(aiHelper);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _aiHelper = aiHelper;
        _options = options;
        _logger = logger;
    }

    /// <summary>
    /// Condenses the roundup content via AI. Falls back to the original content on failure.
    /// </summary>
    public async Task<string> CondenseAsync(string content, string writingGuidelines, CancellationToken ct)
    {
        var systemMessage = _systemPrompt.Value
            .Replace("{WritingStyleGuidelines}", writingGuidelines, StringComparison.Ordinal);

        var userMessage = $"WELL-ORGANIZED ROUNDUP CONTENT TO CONDENSE:\n\n{content}";

        var response = await _aiHelper.CallAiWithRetryAsync(systemMessage, userMessage, "Step 6", ct);

        if (response is null)
        {
            _logger.LogWarning("Step 6: AI condensing failed, using step 5 content");
            return content;
        }

        await Task.Delay(TimeSpan.FromSeconds(_options.RateLimitDelaySeconds), ct);
        return response;
    }
}
