using System.Data;
using System.Net;
using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Infrastructure.Services.Newsletter;

namespace TechHub.Api.Tests.Endpoints;

public class NewsletterEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;
    private readonly TechHubIntegrationTestApiFactory _factory;

    public NewsletterEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Subscribe_WithWeeklyAndDailySections_ReturnsOkAndPersists()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new
            {
                Email = "newsletter-user@example.com",
                DisplayName = "Newsletter User",
                WeeklySections = new[] { "ai", "azure" },
                DailySections = new[] { "ai" }
            },
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM newsletter_subscribers WHERE email = 'newsletter-user@example.com'");
        count.Should().Be(1);
    }

    [Fact]
    public async Task Unsubscribe_WithValidToken_ReturnsOk()
    {
        const string email = "unsubscribe-me@example.com";

        await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = email, WeeklySections = new[] { "dotnet" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        var token = NewsletterService.BuildUnsubscribeToken(email, "integration-test-secret");
        var response = await _client.PostAsJsonAsync(
            "/api/newsletter/unsubscribe",
            new { Email = email, Token = token },
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var unsubscribed = await connection.ExecuteScalarAsync<bool>(
            "SELECT unsubscribed_at IS NOT NULL FROM newsletter_subscribers WHERE email = @Email",
            new { Email = email });
        unsubscribed.Should().BeTrue();
    }

    [Fact]
    public async Task AdminDeleteSubscriber_RemovesRow()
    {
        await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = "bot-remove@example.com", WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var id = await connection.ExecuteScalarAsync<long>(
            "SELECT id FROM newsletter_subscribers WHERE email = 'bot-remove@example.com' LIMIT 1");

        var response = await _client.DeleteAsync($"/api/admin/newsletter/subscribers/{id}", TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM newsletter_subscribers WHERE id = @Id",
            new { Id = id });
        count.Should().Be(0);
    }
}
