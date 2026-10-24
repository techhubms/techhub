using System.Text.Json;
using FluentAssertions;
using TechHub.Core.Configuration;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for <see cref="AiRequestBodyBuilder"/>: maps an AI profile onto a v1 chat completions request body.
/// </summary>
public class AiRequestBodyBuilderTests
{
    private static JsonElement Build(AiProfileOptions profile, bool interactive = false, object? responseFormat = null)
    {
        var json = AiRequestBodyBuilder.Build(profile, "system text", "user text", 1234, interactive, responseFormat);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    [Fact]
    public void Build_UsesProfileDeploymentAsModel()
    {
        var body = Build(new AiProfileOptions { DeploymentName = "gpt-6-luna" });

        body.GetProperty("model").GetString().Should().Be("gpt-6-luna");
    }

    [Fact]
    public void Build_WritesMessagesAndMaxCompletionTokens()
    {
        var body = Build(new AiProfileOptions { DeploymentName = "m" });

        var messages = body.GetProperty("messages");
        messages.GetArrayLength().Should().Be(2);
        messages[0].GetProperty("role").GetString().Should().Be("system");
        messages[0].GetProperty("content").GetString().Should().Be("system text");
        messages[1].GetProperty("role").GetString().Should().Be("user");
        messages[1].GetProperty("content").GetString().Should().Be("user text");
        body.GetProperty("max_completion_tokens").GetInt32().Should().Be(1234);
    }

    [Fact]
    public void Build_NeverSendsTemperature()
    {
        var body = Build(new AiProfileOptions { DeploymentName = "m", ReasoningEffort = "low", ServiceTier = "flex" });

        body.TryGetProperty("temperature", out _).Should().BeFalse();
    }

    [Fact]
    public void Build_WithReasoningEffort_IncludesIt()
    {
        var body = Build(new AiProfileOptions { DeploymentName = "m", ReasoningEffort = "medium" });

        body.GetProperty("reasoning_effort").GetString().Should().Be("medium");
    }

    [Fact]
    public void Build_WithoutReasoningEffort_OmitsIt()
    {
        var body = Build(new AiProfileOptions { DeploymentName = "m" });

        body.TryGetProperty("reasoning_effort", out _).Should().BeFalse();
    }

    [Fact]
    public void Build_DefaultTier_OmitsServiceTier()
    {
        var body = Build(new AiProfileOptions { DeploymentName = "m", ServiceTier = "default" });

        body.TryGetProperty("service_tier", out _).Should().BeFalse();
    }

    [Fact]
    public void Build_FlexTier_SendsFlex()
    {
        var body = Build(new AiProfileOptions { DeploymentName = "m", ServiceTier = "flex" });

        body.GetProperty("service_tier").GetString().Should().Be("flex");
    }

    [Fact]
    public void Build_FlexTierForInteractiveCall_UsesStandardTier()
    {
        var body = Build(new AiProfileOptions { DeploymentName = "m", ServiceTier = "flex" }, interactive: true);

        body.TryGetProperty("service_tier", out _).Should().BeFalse();
    }

    [Fact]
    public void Build_WithResponseFormat_IncludesIt()
    {
        var format = new { type = "json_schema", json_schema = new { name = "x", strict = true } };

        var body = Build(new AiProfileOptions { DeploymentName = "m" }, responseFormat: format);

        body.GetProperty("response_format").GetProperty("type").GetString().Should().Be("json_schema");
        body.GetProperty("response_format").GetProperty("json_schema").GetProperty("strict").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public void Build_WithoutResponseFormat_OmitsIt()
    {
        var body = Build(new AiProfileOptions { DeploymentName = "m" });

        body.TryGetProperty("response_format", out _).Should().BeFalse();
    }

    [Fact]
    public void Build_EmptyDeploymentName_Throws()
    {
        var act = () => AiRequestBodyBuilder.Build(new AiProfileOptions(), "s", "u", 100);

        act.Should().Throw<InvalidOperationException>().WithMessage("*deployment*");
    }

    [Theory]
    [InlineData(AiProfile.Classification)]
    [InlineData(AiProfile.Rewrite)]
    [InlineData(AiProfile.RoundupNewsWriter)]
    [InlineData(AiProfile.RoundupMetadata)]
    [InlineData(AiProfile.TimelineExtraction)]
    public void ProfilesGet_ReturnsTheMatchingProfile(AiProfile profile)
    {
        var profiles = new AiProfilesOptions
        {
            Classification = new AiProfileOptions { DeploymentName = "c" },
            Rewrite = new AiProfileOptions { DeploymentName = "r" },
            RoundupNewsWriter = new AiProfileOptions { DeploymentName = "w" },
            RoundupMetadata = new AiProfileOptions { DeploymentName = "m" },
            TimelineExtraction = new AiProfileOptions { DeploymentName = "t" }
        };

        var expected = profile switch
        {
            AiProfile.Classification => "c",
            AiProfile.Rewrite => "r",
            AiProfile.RoundupNewsWriter => "w",
            AiProfile.TimelineExtraction => "t",
            _ => "m"
        };

        profiles.Get(profile).DeploymentName.Should().Be(expected);
    }
}
