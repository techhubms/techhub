using FluentAssertions;
using Microsoft.Extensions.FileProviders;
using TechHub.Api.Authorization;

namespace TechHub.Api.Tests.Authorization;

public class AdminAuthorizationPolicyTests
{
    [Theory]
    [InlineData("Development", "", true)]
    [InlineData("IntegrationTest", "", true)]
    [InlineData("Staging", "", true)]
    [InlineData("Production", "", false)]
    [InlineData("Production", null, false)]
    [InlineData("Development", "some-client-id", false)]
    [InlineData("Production", "some-client-id", false)]
    public void AllowsAnonymousAdmin_ReturnsExpected(string environmentName, string? clientId, bool expected)
    {
        // Arrange
        var environment = new StubHostEnvironment(environmentName);

        // Act
        var result = AdminAuthorization.AllowsAnonymousAdmin(clientId, environment);

        // Assert
        result.Should().Be(expected);
    }

    private sealed class StubHostEnvironment : Microsoft.Extensions.Hosting.IHostEnvironment
    {
        public StubHostEnvironment(string environmentName)
        {
            EnvironmentName = environmentName;
        }

        public string EnvironmentName { get; set; }

        public string ApplicationName { get; set; } = "TechHub.Api.Tests";

        public string ContentRootPath { get; set; } = string.Empty;

        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
