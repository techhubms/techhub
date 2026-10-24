using System.Text.Json;
using System.Text.Json.Serialization;
using TechHub.Core.Configuration;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Builds the JSON body of a v1 chat completions request from an <see cref="AiProfileOptions"/>.
/// The profile decides the model, reasoning effort and service tier; callers only supply the prompts.
/// </summary>
public static class AiRequestBodyBuilder
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Builds the request body. The v1 API selects the deployment through <c>model</c>, so no deployment is in the URL.
    /// </summary>
    /// <param name="profile">Settings of the pipeline step.</param>
    /// <param name="systemMessage">System prompt.</param>
    /// <param name="userMessage">User prompt.</param>
    /// <param name="maxCompletionTokens">Upper bound for reasoning plus visible output tokens.</param>
    /// <param name="interactive">When <c>true</c> an admin is waiting for the answer, so the Standard tier is used even for Flex profiles.</param>
    /// <param name="responseFormat">Optional <c>response_format</c> object, e.g. a strict JSON schema.</param>
    public static string Build(
        AiProfileOptions profile,
        string systemMessage,
        string userMessage,
        int maxCompletionTokens,
        bool interactive = false,
        object? responseFormat = null)
    {
        ArgumentNullException.ThrowIfNull(profile);

        if (string.IsNullOrWhiteSpace(profile.DeploymentName))
        {
            throw new InvalidOperationException("AI profile has no deployment name configured (AiCategorization:Profiles).");
        }

        var request = new ChatRequest
        {
            Model = profile.DeploymentName,
            Messages =
            [
                new ChatMessage("system", systemMessage),
                new ChatMessage("user", userMessage)
            ],
            MaxCompletionTokens = maxCompletionTokens,
            ReasoningEffort = string.IsNullOrWhiteSpace(profile.ReasoningEffort) ? null : profile.ReasoningEffort,
            ServiceTier = profile.IsFlex && !interactive ? AiProfileOptions.FlexTier : null,
            ResponseFormat = responseFormat
        };

        return JsonSerializer.Serialize(request, _jsonOptions);
    }

    private sealed record ChatMessage(string Role, string Content);

    private sealed class ChatRequest
    {
        public required string Model { get; init; }

        public required IReadOnlyList<ChatMessage> Messages { get; init; }

        public int MaxCompletionTokens { get; init; }

        public string? ReasoningEffort { get; init; }

        public string? ServiceTier { get; init; }

        public object? ResponseFormat { get; init; }
    }
}
