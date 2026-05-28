using System.Data;
using System.Net;
using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
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
        const string Email = "unsubscribe-me@example.com";

        await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = Email, WeeklySections = new[] { "dotnet" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        var token = NewsletterService.BuildUnsubscribeToken(Email, "integration-test-secret");
        var response = await _client.PostAsJsonAsync(
            "/api/newsletter/unsubscribe",
            new { Email = Email, Token = token },
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var unsubscribed = await connection.ExecuteScalarAsync<bool>(
            "SELECT unsubscribed_at IS NOT NULL FROM newsletter_subscribers WHERE email = @Email",
            new { Email });
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

    [Fact]
    public async Task Subscribe_WithOnlyInvalidSections_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new
            {
                Email = "invalid-sections@example.com",
                WeeklySections = new[] { "not-a-section" },
                DailySections = Array.Empty<string>()
            },
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM newsletter_subscribers WHERE email = 'invalid-sections@example.com'");
        count.Should().Be(0);
    }

    [Fact]
    public async Task ManageRequest_WithValidEmail_ReturnsOk()
    {
        const string Email = "manage-request@example.com";

        // Subscribe and confirm first so the manage-link email can be sent
        await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = Email, WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        await connection.ExecuteAsync(
            "UPDATE newsletter_subscribers SET is_confirmed = TRUE WHERE email = @Email",
            new { Email });

        var response = await _client.PostAsync(
            $"/api/newsletter/manage/request?email={Uri.EscapeDataString(Email)}",
            null,
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ManageRequest_WithUnknownEmail_ReturnsOk()
    {
        // Should return 200 regardless (don't leak whether email is subscribed)
        var response = await _client.PostAsync(
            "/api/newsletter/manage/request?email=nobody%40example.com",
            null,
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ManagePreferences_GetWithValidToken_ReturnsSubscriberData()
    {
        const string Email = "manage-get@example.com";

        await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = Email, WeeklySections = new[] { "ai", "azure" }, DailySections = new[] { "ai" } },
            TestContext.Current.CancellationToken);

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        await connection.ExecuteAsync(
            "UPDATE newsletter_subscribers SET is_confirmed = TRUE WHERE email = @Email",
            new { Email });

        var token = NewsletterService.BuildUnsubscribeToken(Email, "integration-test-secret");
        var response = await _client.GetAsync(
            $"/api/newsletter/manage?email={Uri.EscapeDataString(Email)}&token={Uri.EscapeDataString(token)}",
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var subscriber = await response.Content.ReadFromJsonAsync<NewsletterSubscriber>(
            TestContext.Current.CancellationToken);
        subscriber.Should().NotBeNull();
        subscriber!.Email.Should().Be(Email);
    }

    [Fact]
    public async Task ManagePreferences_GetWithInvalidToken_ReturnsUnauthorized()
    {
        const string Email = "manage-badtoken@example.com";

        await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = Email, WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        await connection.ExecuteAsync(
            "UPDATE newsletter_subscribers SET is_confirmed = TRUE WHERE email = @Email",
            new { Email });

        var response = await _client.GetAsync(
            $"/api/newsletter/manage?email={Uri.EscapeDataString(Email)}&token=wrong-token",
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ManagePreferences_UpdateWithValidToken_PersistsChanges()
    {
        const string Email = "manage-update@example.com";

        await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = Email, WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        await connection.ExecuteAsync(
            "UPDATE newsletter_subscribers SET is_confirmed = TRUE WHERE email = @Email",
            new { Email });

        var token = NewsletterService.BuildUnsubscribeToken(Email, "integration-test-secret");
        var response = await _client.PutAsJsonAsync(
            "/api/newsletter/manage",
            new
            {
                Email,
                Token = token,
                DisplayName = "Updated Name",
                WeeklySections = new[] { "azure" },
                DailySections = new[] { "dotnet" }
            },
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var displayName = await connection.ExecuteScalarAsync<string>(
            "SELECT display_name FROM newsletter_subscribers WHERE email = @Email",
            new { Email });
        displayName.Should().Be("Updated Name");
    }
}
