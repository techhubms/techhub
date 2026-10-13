using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// <see cref="AiCompletionClient"/> acquires an Entra ID token through a static <c>DefaultAzureCredential</c>
/// before every request, so HTTP behaviour (429 / content filter handling) cannot be exercised without
/// real credentials. Only the constructor guards are testable through the public API.
/// </summary>
public class AiCompletionClientTests
{
    [Fact]
    public void Constructor_NullHttpClient_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new AiCompletionClient(null!, Options.Create(new AiCategorizationOptions()), NullLogger<AiCompletionClient>.Instance);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("httpClient");
    }

    [Fact]
    public void Constructor_NullOptions_ThrowsArgumentNullException()
    {
        // Arrange
        using var httpClient = new HttpClient();

        // Act
        var act = () => new AiCompletionClient(httpClient, null!, NullLogger<AiCompletionClient>.Instance);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("options");
    }

    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        using var httpClient = new HttpClient();

        // Act
        var act = () => new AiCompletionClient(httpClient, Options.Create(new AiCategorizationOptions()), null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("logger");
    }
}
