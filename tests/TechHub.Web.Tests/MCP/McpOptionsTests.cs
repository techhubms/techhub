using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TechHub.Web.MCP;

namespace TechHub.Web.Tests.MCP;

public class McpOptionsTests
{
    [Theory]
    [InlineData("https://tech.hub.ms")]
    [InlineData("https://tech.hub.ms/")]
    [InlineData("https://localhost:5003")]
    [InlineData("http://localhost:5003")]
    public void Validation_ValidHttpBaseUrl_Succeeds(string baseUrl)
    {
        // Arrange
        using var services = BuildServices(baseUrl);

        // Act
        var options = services.GetRequiredService<IOptions<McpOptions>>().Value;

        // Assert
        options.BaseUrl.Should().Be(baseUrl);
    }

    [Theory]
    [InlineData("")]
    [InlineData("https://")]
    [InlineData("ftp://tech.hub.ms")]
    [InlineData("tech.hub.ms")]
    [InlineData("/relative")]
    [InlineData("https://tech.hub.ms?x=1")]
    [InlineData("https://tech.hub.ms/?")]
    [InlineData("https://tech.hub.ms#top")]
    public void Validation_InvalidBaseUrl_Throws(string baseUrl)
    {
        // Arrange
        using var services = BuildServices(baseUrl);

        // Act
        var act = () => services.GetRequiredService<IOptions<McpOptions>>().Value;

        // Assert
        act.Should().Throw<OptionsValidationException>();
    }

    private static ServiceProvider BuildServices(string baseUrl)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Mcp:BaseUrl"] = baseUrl })
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddOptions<McpOptions>()
            .BindConfiguration(McpOptions.ConfigurationKey)
            .ValidateDataAnnotations();
        return services.BuildServiceProvider();
    }
}
