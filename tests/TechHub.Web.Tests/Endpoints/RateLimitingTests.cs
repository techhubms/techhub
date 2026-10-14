using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace TechHub.Web.Tests.Endpoints;

/// <summary>
/// Tests for the per-IP rate limiting configured in Program.cs: web-general (60/min), web-rss (20/min) and
/// web-mcp (60/min). Loopback clients are exempt in Development only, so a spoofed
/// <c>X-Forwarded-For: 127.0.0.1</c> cannot bypass the limit in Production.
/// Each test uses its own client IP (via <c>X-Forwarded-For</c>) so limiter partitions never overlap.
/// </summary>
public class RateLimitingTests(DevelopmentWebFixture development, ProductionWebFixture production)
    : IClassFixture<DevelopmentWebFixture>, IClassFixture<ProductionWebFixture>
{
    private const string RssPath = "/all/feed.xml";
    private const string McpPath = "/mcp";
    private const int RssLimit = 20;
    private const int McpLimit = 60;

    // ── web-rss ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task RssFeed_WithinLimit_ServesAllRequests()
    {
        // Arrange
        using var client = development.CreateClient("198.51.100.10");

        // Act
        var statuses = await GetRepeatedlyAsync(client, RssPath, RssLimit);

        // Assert
        statuses.Should().OnlyContain(s => s == HttpStatusCode.OK);
    }

    [Fact]
    public async Task RssFeed_OverLimit_ReturnsTooManyRequestsWithRetryAfterAndMessage()
    {
        // Arrange
        using var client = development.CreateClient("198.51.100.11");
        await GetRepeatedlyAsync(client, RssPath, RssLimit);

        // Act
        using var response = await client.GetAsync(RssPath, TestContext.Current.CancellationToken);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
        body.Should().Be("Rate limit exceeded. Please retry later.");
        response.Headers.RetryAfter.Should().NotBeNull();
        response.Headers.RetryAfter!.Delta.Should().NotBeNull();
        response.Headers.RetryAfter.Delta!.Value.TotalSeconds.Should().BeInRange(0, 60);
    }

    [Fact]
    public async Task RssFeed_OneIpExhausted_OtherIpsAreUnaffected()
    {
        // Arrange
        using var heavyClient = development.CreateClient("198.51.100.12");
        await GetRepeatedlyAsync(heavyClient, RssPath, RssLimit + 1);
        using var otherClient = development.CreateClient("198.51.100.13");

        // Act
        using var heavyResponse = await heavyClient.GetAsync(RssPath, TestContext.Current.CancellationToken);
        using var otherResponse = await otherClient.GetAsync(RssPath, TestContext.Current.CancellationToken);

        // Assert
        heavyResponse.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
        otherResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RssFeed_ExhaustedLimit_StillAllowsNonRateLimitedEndpoints()
    {
        // Arrange
        using var client = development.CreateClient("198.51.100.14");
        await GetRepeatedlyAsync(client, RssPath, RssLimit + 1);

        // Act
        using var response = await client.GetAsync("/version", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK, "the deployment probe is not behind a rate-limit policy");
    }

    // ── Loopback exemption ──────────────────────────────────────────────────

    [Fact]
    public async Task RssFeed_DevelopmentLoopbackClient_IsExemptFromRateLimit()
    {
        // Arrange
        using var client = development.CreateClient("127.0.0.1");

        // Act
        var statuses = await GetRepeatedlyAsync(client, RssPath, RssLimit + 10);

        // Assert
        statuses.Should().NotContain(HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public async Task RssFeed_DevelopmentNonLoopbackClient_IsStillRateLimited()
    {
        // Arrange
        using var client = development.CreateClient("203.0.113.20");

        // Act
        var statuses = await GetRepeatedlyAsync(client, RssPath, RssLimit + 1);

        // Assert
        statuses.Take(RssLimit).Should().OnlyContain(s => s == HttpStatusCode.OK);
        statuses[RssLimit].Should().Be(HttpStatusCode.TooManyRequests);
    }

    [Theory]
    [InlineData("127.0.0.1")]
    [InlineData("::1")]
    public async Task RssFeed_ProductionSpoofedLoopbackForwardedFor_IsStillRateLimited(string spoofedAddress)
    {
        // Arrange
        using var client = production.CreateClient(spoofedAddress);

        // Act
        var statuses = await GetRepeatedlyAsync(client, RssPath, RssLimit + 1);

        // Assert
        statuses.Take(RssLimit).Should().OnlyContain(s => s == HttpStatusCode.OK);
        statuses[RssLimit].Should().Be(HttpStatusCode.TooManyRequests, "loopback is only exempt in Development");
    }

    [Fact]
    public async Task RssFeed_ProductionRegularClient_IsRateLimitedAfterTwentyRequests()
    {
        // Arrange
        using var client = production.CreateClient("203.0.113.21");

        // Act
        var statuses = await GetRepeatedlyAsync(client, RssPath, RssLimit + 1);

        // Assert
        statuses.Take(RssLimit).Should().OnlyContain(s => s == HttpStatusCode.OK);
        statuses[RssLimit].Should().Be(HttpStatusCode.TooManyRequests);
    }

    // ── web-mcp ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task Mcp_OverSixtyRequestsPerMinute_ReturnsTooManyRequests()
    {
        // Arrange
        using var client = development.CreateClient("203.0.113.30");

        // Act
        var statuses = await PostRepeatedlyAsync(client, McpPath, McpLimit + 1);

        // Assert
        statuses.Take(McpLimit).Should().NotContain(HttpStatusCode.TooManyRequests);
        statuses[McpLimit].Should().Be(HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public async Task Mcp_ProductionSpoofedLoopbackForwardedFor_IsStillRateLimited()
    {
        // Arrange
        using var client = production.CreateClient("127.0.0.1");

        // Act
        var statuses = await PostRepeatedlyAsync(client, McpPath, McpLimit + 1);

        // Assert
        statuses[McpLimit].Should().Be(HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public async Task Mcp_ExhaustedLimit_DoesNotConsumeRssQuotaOfSameIp()
    {
        // Arrange
        using var client = development.CreateClient("203.0.113.31");
        await PostRepeatedlyAsync(client, McpPath, McpLimit + 1);

        // Act
        using var response = await client.GetAsync(RssPath, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK, "each policy keeps its own per-IP counter");
    }

    // ── Policy wiring ───────────────────────────────────────────────────────

    [Fact]
    public void Endpoints_RazorPages_UseWebGeneralPolicy()
    {
        // Act
        var policies = PoliciesFor("/about");

        // Assert
        policies.Should().NotBeEmpty();
        policies.Should().OnlyContain(p => p == "web-general");
    }

    [Fact]
    public void Endpoints_AllContentRssFeed_UsesWebRssPolicy()
    {
        // Act
        var policies = PoliciesFor(RssPath);

        // Assert
        policies.Should().Equal("web-rss");
    }

    [Fact]
    public void Endpoints_VersionProbe_HasNoRateLimitPolicy()
    {
        // Act
        var policies = PoliciesFor("/version");

        // Assert
        policies.Should().Equal((string?)null);
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private List<string?> PoliciesFor(string routePattern)
    {
        var dataSource = development.Services.GetRequiredService<EndpointDataSource>();
        return dataSource.Endpoints
            .OfType<RouteEndpoint>()
            .Where(e => string.Equals(e.RoutePattern.RawText, routePattern, StringComparison.OrdinalIgnoreCase))
            .Select(e => e.Metadata.GetMetadata<EnableRateLimitingAttribute>()?.PolicyName)
            .ToList();
    }

    private static async Task<List<HttpStatusCode>> GetRepeatedlyAsync(HttpClient client, string path, int count)
    {
        var statuses = new List<HttpStatusCode>(count);
        for (var i = 0; i < count; i++)
        {
            using var response = await client.GetAsync(path, TestContext.Current.CancellationToken);
            statuses.Add(response.StatusCode);
        }

        return statuses;
    }

    private static async Task<List<HttpStatusCode>> PostRepeatedlyAsync(HttpClient client, string path, int count)
    {
        var statuses = new List<HttpStatusCode>(count);
        for (var i = 0; i < count; i++)
        {
            using var content = new StringContent("{}", Encoding.UTF8, "application/json");
            using var response = await client.PostAsync(path, content, TestContext.Current.CancellationToken);
            statuses.Add(response.StatusCode);
        }

        return statuses;
    }
}
