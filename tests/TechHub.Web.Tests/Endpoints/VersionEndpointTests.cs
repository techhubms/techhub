using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace TechHub.Web.Tests.Endpoints;

/// <summary>
/// Tests for the <c>/version</c> deployment probe in Program.cs — the image tag and the deploy timestamp
/// parsed from the last 14 characters of the tag (<c>yyyyMMddHHmmss</c>).
/// </summary>
public class VersionEndpointTests(DevelopmentWebFixture fixture) : IClassFixture<DevelopmentWebFixture>
{
    [Theory]
    [InlineData("20260713141516", "2026-07-13T14:15:16+00:00")]
    [InlineData("pr-42-20260713141516", "2026-07-13T14:15:16+00:00")]
    [InlineData("latest-20260101000000", "2026-01-01T00:00:00+00:00")]
    [InlineData("20261231235959", "2026-12-31T23:59:59+00:00")]
    public async Task Get_TagEndingInTimestamp_ReturnsTagAndParsedUtcDeployTime(string tag, string expectedDeployedAt)
    {
        // Arrange
        fixture.SetConfig("DEPLOY_IMAGE_TAG", tag);
        using var client = fixture.CreateClient();

        // Act
        using var response = await client.GetAsync("/version", TestContext.Current.CancellationToken);
        using var json = await ReadJsonAsync(response);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        json.RootElement.GetProperty("tag").GetString().Should().Be(tag);
        json.RootElement.GetProperty("deployedAt").GetDateTimeOffset()
            .Should().Be(DateTimeOffset.Parse(expectedDeployedAt, System.Globalization.CultureInfo.InvariantCulture));
        json.RootElement.GetProperty("deployedAt").GetDateTimeOffset().Offset.Should().Be(TimeSpan.Zero);
    }

    [Theory]
    [InlineData("unknown")]
    [InlineData("2026071314151")]
    [InlineData("abcdefghijklmn")]
    [InlineData("20261345251516")]
    [InlineData("20260230141516")]
    [InlineData("pr-42-2026071314151x")]
    [InlineData("20260713141516-extra")]
    [InlineData("v1.2.3")]
    public async Task Get_TagWithoutValidTrailingTimestamp_ReturnsTagWithNullDeployTime(string tag)
    {
        // Arrange
        fixture.SetConfig("DEPLOY_IMAGE_TAG", tag);
        using var client = fixture.CreateClient();

        // Act
        using var response = await client.GetAsync("/version", TestContext.Current.CancellationToken);
        using var json = await ReadJsonAsync(response);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        json.RootElement.GetProperty("tag").GetString().Should().Be(tag);
        json.RootElement.GetProperty("deployedAt").ValueKind.Should().Be(JsonValueKind.Null);
    }

    [Fact]
    public async Task Get_TagNotConfigured_ReturnsUnknownTagWithNullDeployTime()
    {
        // Arrange
        fixture.SetConfig("DEPLOY_IMAGE_TAG", null);
        using var client = fixture.CreateClient();

        // Act
        using var response = await client.GetAsync("/version", TestContext.Current.CancellationToken);
        using var json = await ReadJsonAsync(response);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        json.RootElement.GetProperty("tag").GetString().Should().Be("unknown");
        json.RootElement.GetProperty("deployedAt").ValueKind.Should().Be(JsonValueKind.Null);
    }

    [Fact]
    public async Task Get_Always_ReturnsUncachedJsonWithoutRequiringAuthentication()
    {
        // Arrange
        fixture.SetConfig("DEPLOY_IMAGE_TAG", "20260713141516");
        using var client = fixture.CreateClient();

        // Act
        using var response = await client.GetAsync("/version", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.CacheControl.Should().NotBeNull();
        response.Headers.CacheControl!.NoStore.Should().BeTrue("the deployment script polls this to detect a new version");
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
    }

    [Fact]
    public async Task Get_Always_ExposesOnlyTagAndDeployTime()
    {
        // Arrange
        fixture.SetConfig("DEPLOY_IMAGE_TAG", "20260713141516");
        using var client = fixture.CreateClient();

        // Act
        using var response = await client.GetAsync("/version", TestContext.Current.CancellationToken);
        using var json = await ReadJsonAsync(response);

        // Assert
        json.RootElement.EnumerateObject().Select(p => p.Name).Should().BeEquivalentTo("tag", "deployedAt");
    }

    [Fact]
    public async Task Get_TagChangedBetweenRequests_ReflectsLatestConfiguration()
    {
        // Arrange
        using var client = fixture.CreateClient();
        fixture.SetConfig("DEPLOY_IMAGE_TAG", "20260101000000");
        using var first = await client.GetAsync("/version", TestContext.Current.CancellationToken);
        using var firstJson = await ReadJsonAsync(first);

        // Act
        fixture.SetConfig("DEPLOY_IMAGE_TAG", "20260202000000");
        using var second = await client.GetAsync("/version", TestContext.Current.CancellationToken);
        using var secondJson = await ReadJsonAsync(second);

        // Assert
        firstJson.RootElement.GetProperty("tag").GetString().Should().Be("20260101000000");
        secondJson.RootElement.GetProperty("tag").GetString().Should().Be("20260202000000");
    }

    private static async Task<JsonDocument> ReadJsonAsync(HttpResponseMessage response) =>
        await JsonDocument.ParseAsync(
            await response.Content.ReadAsStreamAsync(TestContext.Current.CancellationToken),
            cancellationToken: TestContext.Current.CancellationToken);
}
