using System.Net;
using FluentAssertions;

namespace TechHub.Web.Tests.Endpoints;

/// <summary>
/// The E2E suite runs every browser from one runner IP, so it carries a secret token (header or cookie) that exempts
/// it from the per-IP limits. The exemption is keyed on the token, never on the IP, and is off unless a token is configured.
/// </summary>
public class E2ERateLimitBypassTests(ProductionE2EBypassWebFixture bypassEnabled, ProductionWebFixture bypassDisabled)
    : IClassFixture<ProductionE2EBypassWebFixture>, IClassFixture<ProductionWebFixture>
{
    private const string RssPath = "/all/feed.xml";
    private const int RssLimit = 20;

    [Fact]
    public async Task RssFeed_WithTokenHeader_IsExemptFromRateLimit()
    {
        // Arrange
        using var client = bypassEnabled.CreateClient("203.0.113.40");
        client.DefaultRequestHeaders.Add("X-E2E-Token", ProductionE2EBypassWebFixture.Token);

        // Act
        var statuses = await GetRepeatedlyAsync(client, RssLimit + 10);

        // Assert
        statuses.Should().OnlyContain(s => s == HttpStatusCode.OK);
    }

    [Fact]
    public async Task RssFeed_WithTokenCookie_IsExemptFromRateLimit()
    {
        // Arrange
        using var client = bypassEnabled.CreateClient("203.0.113.41");
        client.DefaultRequestHeaders.Add("Cookie", $"e2e-token={ProductionE2EBypassWebFixture.Token}");

        // Act
        var statuses = await GetRepeatedlyAsync(client, RssLimit + 10);

        // Assert
        statuses.Should().OnlyContain(s => s == HttpStatusCode.OK);
    }

    [Fact]
    public async Task AuthorPages_WithTokenHeader_AreExemptFromRateLimit()
    {
        using var client = bypassEnabled.CreateClient("203.0.113.47");
        client.DefaultRequestHeaders.Add("X-E2E-Token", ProductionE2EBypassWebFixture.Token);

        for (var i = 0; i < 21; i++)
        {
            using var response = await client.GetAsync("/all/authors/Test", TestContext.Current.CancellationToken);
            response.StatusCode.Should().NotBe(HttpStatusCode.TooManyRequests);
        }
    }

    [Theory]
    [InlineData("wrong-token")]
    [InlineData("")]
    public async Task RssFeed_WithWrongOrEmptyToken_IsStillRateLimited(string token)
    {
        // Arrange
        using var client = bypassEnabled.CreateClient($"203.0.113.{(token.Length == 0 ? 42 : 43)}");
        client.DefaultRequestHeaders.Add("X-E2E-Token", token);

        // Act
        var statuses = await GetRepeatedlyAsync(client, RssLimit + 1);

        // Assert
        statuses[RssLimit].Should().Be(HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public async Task RssFeed_WithoutConfiguredToken_IgnoresAnyTokenHeader()
    {
        // Arrange
        using var client = bypassDisabled.CreateClient("203.0.113.44");
        client.DefaultRequestHeaders.Add("X-E2E-Token", ProductionE2EBypassWebFixture.Token);

        // Act
        var statuses = await GetRepeatedlyAsync(client, RssLimit + 1);

        // Assert
        statuses[RssLimit].Should().Be(HttpStatusCode.TooManyRequests, "the bypass is disabled until a token is configured");
    }

    [Fact]
    public async Task RssFeed_WithoutConfiguredToken_IgnoresAnEmptyTokenHeader()
    {
        // Arrange
        using var client = bypassDisabled.CreateClient("203.0.113.45");
        client.DefaultRequestHeaders.Add("X-E2E-Token", string.Empty);

        // Act
        var statuses = await GetRepeatedlyAsync(client, RssLimit + 1);

        // Assert
        statuses[RssLimit].Should().Be(HttpStatusCode.TooManyRequests, "an empty configured token must not match an empty header");
    }

    [Fact]
    public async Task RssFeed_WhenConfiguredTokenIsAnUnresolvedKeyVaultReference_IgnoresThatText()
    {
        // Arrange
        await using var fixture = new ProductionE2EBypassWebFixture();
        const string Reference = "@Microsoft.KeyVault(SecretUri=https://kv.vault.azure.net/secrets/e2e)";
        fixture.SetConfig("RateLimiting:E2EBypassToken", Reference);
        using var client = fixture.CreateClient("203.0.113.46");
        client.DefaultRequestHeaders.Add("X-E2E-Token", Reference);

        // Act
        var statuses = await GetRepeatedlyAsync(client, RssLimit + 1);

        // Assert
        statuses[RssLimit].Should().Be(HttpStatusCode.TooManyRequests, "App Service passes an unresolved reference through as literal text");
    }

    private static async Task<List<HttpStatusCode>> GetRepeatedlyAsync(HttpClient client, int count)
    {
        var statuses = new List<HttpStatusCode>(count);
        for (var i = 0; i < count; i++)
        {
            using var response = await client.GetAsync(RssPath, TestContext.Current.CancellationToken);
            statuses.Add(response.StatusCode);
        }

        return statuses;
    }
}
