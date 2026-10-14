using System.Data;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
using TechHub.Infrastructure.Services.Newsletter;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Negative-path and validation tests for the public newsletter endpoints
/// (sections, subscribe, confirm, unsubscribe, manage). Every test uses its own unique email address.
/// </summary>
public class NewsletterEndpointsNegativeTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private const string Secret = "integration-test-secret";

    private readonly HttpClient _client;
    private readonly TechHubIntegrationTestApiFactory _factory;

    public NewsletterEndpointsNegativeTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _factory = factory;
        _client = factory.CreateClient();
    }

    // ── GET /api/newsletter/sections ─────────────────────────────────────────

    [Fact]
    public async Task GetNewsletterSections_ExcludesSyntheticAllSection()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/newsletter/sections",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var sections = await response.Content.ReadFromJsonAsync<List<Section>>(
            TestContext.Current.CancellationToken);
        sections.Should().NotBeNull();
        sections!.Should().NotContain(s => s.Name == "all");
        sections.Should().Contain(s => s.Name == "ai");
        sections.Should().Contain(s => s.Name == "azure");
    }

    // ── POST /api/newsletter/subscribe ───────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-an-email")]
    [InlineData("a@")]
    [InlineData("@example.com")]
    [InlineData("Name <someone@example.com>")]
    public async Task Subscribe_InvalidEmail_ReturnsBadRequest(string email)
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = email, WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Subscribe_MissingEmail_ReturnsBadRequest()
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { WeeklySections = new[] { "ai" } },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("null-sections")]
    [InlineData("empty-sections")]
    [InlineData("blank-sections")]
    [InlineData("all-section-only")]
    public async Task Subscribe_WithoutAnyValidSection_ReturnsBadRequestAndDoesNotPersist(string scenario)
    {
        // Arrange
        var email = NewEmail();
        var payload = NoValidSectionPayload(email, scenario);

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            payload,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await CountSubscribersAsync(email)).Should().Be(0);
    }

    [Fact]
    public async Task Subscribe_WithValidAndUnknownSections_PersistsOnlyKnownSectionsCaseInsensitively()
    {
        // Arrange
        var email = NewEmail();

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = email, WeeklySections = new[] { "ai", "bogus-section" }, DailySections = new[] { "AZURE" } },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var stored = await GetManagePreferencesAsync(email, Token(email));
        stored.StatusCode.Should().Be(HttpStatusCode.OK);
        var subscriber = await stored.Content.ReadFromJsonAsync<NewsletterSubscriber>(
            TestContext.Current.CancellationToken);
        subscriber!.WeeklySections.Should().BeEquivalentTo(["ai"]);
        subscriber.DailySections.Should().BeEquivalentTo(["azure"]);
    }

    [Fact]
    public async Task Subscribe_NewSubscriber_RequiresConfirmation()
    {
        // Arrange
        var email = NewEmail();

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = email, WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        body.GetProperty("needsConfirmation").GetBoolean().Should().BeTrue();
        body.GetProperty("message").GetString().Should().Be("Check your inbox to confirm your subscription.");
        (await IsConfirmedAsync(email)).Should().BeFalse();
    }

    // ── GET /api/newsletter/confirm ──────────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("?email=someone%40example.com")]
    [InlineData("?token=some-token")]
    [InlineData("?email=%20&token=%20")]
    public async Task Confirm_MissingOrBlankParams_ReturnsBadRequest(string query)
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/newsletter/confirm{query}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Confirm_ValidTokenForUnknownSubscriber_ReturnsBadRequest()
    {
        // Arrange
        var email = NewEmail();
        var token = NewsletterService.BuildConfirmToken(email, Secret);

        // Act
        var response = await ConfirmAsync(email, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Confirm_TokenIssuedForOtherEmail_ReturnsBadRequestAndStaysUnconfirmed()
    {
        // Arrange
        var email = await SubscribeAsync();
        var otherToken = NewsletterService.BuildConfirmToken(NewEmail(), Secret);

        // Act
        var response = await ConfirmAsync(email, otherToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await IsConfirmedAsync(email)).Should().BeFalse();
    }

    [Fact]
    public async Task Confirm_UnsubscribeTokenUsedAsConfirmToken_ReturnsBadRequestAndStaysUnconfirmed()
    {
        // Arrange
        var email = await SubscribeAsync();

        // Act — tokens are domain separated, so an unsubscribe token must not confirm
        var response = await ConfirmAsync(email, Token(email));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await IsConfirmedAsync(email)).Should().BeFalse();
    }

    // ── POST /api/newsletter/unsubscribe ─────────────────────────────────────

    [Theory]
    [InlineData(null, "some-token")]
    [InlineData("", "some-token")]
    [InlineData("someone@example.com", null)]
    [InlineData("someone@example.com", "   ")]
    public async Task Unsubscribe_MissingEmailOrToken_ReturnsBadRequest(string? email, string? token)
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/newsletter/unsubscribe",
            new { Email = email, Token = token },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Unsubscribe_InvalidToken_ReturnsBadRequestAndKeepsSubscription()
    {
        // Arrange
        var email = await SubscribeAsync();

        // Act
        var response = await UnsubscribeAsync(email, "invalid-token");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await IsUnsubscribedAsync(email)).Should().BeFalse();
    }

    [Fact]
    public async Task Unsubscribe_TokenIssuedForOtherEmail_ReturnsBadRequestAndKeepsSubscription()
    {
        // Arrange
        var email = await SubscribeAsync();

        // Act
        var response = await UnsubscribeAsync(email, Token(NewEmail()));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await IsUnsubscribedAsync(email)).Should().BeFalse();
    }

    [Fact]
    public async Task Unsubscribe_ConfirmTokenUsedAsUnsubscribeToken_ReturnsBadRequestAndKeepsSubscription()
    {
        // Arrange
        var email = await SubscribeAsync();

        // Act — tokens are domain separated, so a confirm token must not unsubscribe
        var response = await UnsubscribeAsync(email, NewsletterService.BuildConfirmToken(email, Secret));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await IsUnsubscribedAsync(email)).Should().BeFalse();
    }

    [Fact]
    public async Task Unsubscribe_ValidTokenForUnknownSubscriber_ReturnsNotFound()
    {
        // Arrange
        var email = NewEmail();

        // Act
        var response = await UnsubscribeAsync(email, Token(email));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Unsubscribe_CalledTwice_SecondCallReturnsNotFound()
    {
        // Arrange
        var email = await SubscribeAsync();
        var first = await UnsubscribeAsync(email, Token(email));
        first.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        var second = await UnsubscribeAsync(email, Token(email));

        // Assert
        second.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Unsubscribe_WhenSecretNotConfigured_ReturnsBadRequestNotConfigured()
    {
        // Arrange — a host whose unsubscribe secret is blank
        await using var blankSecretFactory = _factory.WithWebHostBuilder(
            builder => builder.UseSetting("Newsletter:UnsubscribeSecret", string.Empty));
        using var client = blankSecretFactory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/newsletter/unsubscribe",
            new { Email = NewEmail(), Token = "any-token" },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain("not configured");
    }

    // ── GET /api/newsletter/manage ───────────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("?email=someone%40example.com")]
    [InlineData("?token=some-token")]
    public async Task GetManage_MissingParams_ReturnsBadRequest(string query)
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/newsletter/manage{query}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetManage_InvalidToken_ReturnsNotFound()
    {
        // Arrange
        var email = await SubscribeAsync();

        // Act
        var response = await GetManagePreferencesAsync(email, "invalid-token");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetManage_ValidTokenForUnknownSubscriber_ReturnsNotFound()
    {
        // Arrange
        var email = NewEmail();

        // Act
        var response = await GetManagePreferencesAsync(email, Token(email));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetManage_ValidToken_ReturnsSubscriberPreferences()
    {
        // Arrange
        var email = await SubscribeAsync(weekly: ["ai", "azure"], daily: ["dotnet"]);

        // Act
        var response = await GetManagePreferencesAsync(email, Token(email));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var subscriber = await response.Content.ReadFromJsonAsync<NewsletterSubscriber>(
            TestContext.Current.CancellationToken);
        subscriber!.Email.Should().Be(email);
        subscriber.WeeklySections.Should().BeEquivalentTo(["ai", "azure"]);
        subscriber.DailySections.Should().BeEquivalentTo(["dotnet"]);
    }

    // ── PUT /api/newsletter/manage ───────────────────────────────────────────

    [Theory]
    [InlineData(null, "some-token")]
    [InlineData("someone@example.com", null)]
    [InlineData("", "")]
    public async Task UpdateManage_MissingEmailOrToken_ReturnsBadRequest(string? email, string? token)
    {
        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/newsletter/manage",
            new { Email = email, Token = token, WeeklySections = new[] { "ai" } },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("no-sections")]
    [InlineData("only-unknown-sections")]
    public async Task UpdateManage_WithoutAnyValidSection_ReturnsBadRequestAndKeepsPreferences(string scenario)
    {
        // Arrange
        var email = await SubscribeAsync(weekly: ["ai"]);
        await ConfirmInDbAsync(email);
        object payload = scenario == "no-sections"
            ? new { Email = email, Token = Token(email), WeeklySections = Array.Empty<string>(), DailySections = Array.Empty<string>() }
            : new { Email = email, Token = Token(email), WeeklySections = new[] { "bogus" }, DailySections = new[] { "all" } };

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/newsletter/manage",
            payload,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await GetWeeklySectionsAsync(email)).Should().BeEquivalentTo(["ai"]);
    }

    [Fact]
    public async Task UpdateManage_InvalidToken_ReturnsNotFoundAndKeepsPreferences()
    {
        // Arrange
        var email = await SubscribeAsync(weekly: ["ai"]);
        await ConfirmInDbAsync(email);

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/newsletter/manage",
            new { Email = email, Token = "invalid-token", WeeklySections = new[] { "azure" } },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await GetWeeklySectionsAsync(email)).Should().BeEquivalentTo(["ai"]);
    }

    [Fact]
    public async Task UpdateManage_ValidTokenForUnconfirmedSubscriber_ReturnsNotFoundAndKeepsPreferences()
    {
        // Arrange — subscriber never confirmed
        var email = await SubscribeAsync(weekly: ["ai"]);

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/newsletter/manage",
            new { Email = email, Token = Token(email), WeeklySections = new[] { "azure" } },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await GetWeeklySectionsAsync(email)).Should().BeEquivalentTo(["ai"]);
    }

    [Fact]
    public async Task UpdateManage_ValidTokenForConfirmedSubscriber_UpdatesNameAndKnownSectionsOnly()
    {
        // Arrange
        var email = await SubscribeAsync(weekly: ["ai"]);
        await ConfirmInDbAsync(email);

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/newsletter/manage",
            new
            {
                Email = email,
                Token = Token(email),
                DisplayName = "Updated Name",
                WeeklySections = new[] { "azure", "bogus" },
                DailySections = new[] { "dotnet" }
            },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var stored = await GetManagePreferencesAsync(email, Token(email));
        var subscriber = await stored.Content.ReadFromJsonAsync<NewsletterSubscriber>(
            TestContext.Current.CancellationToken);
        subscriber!.DisplayName.Should().Be("Updated Name");
        subscriber.WeeklySections.Should().BeEquivalentTo(["azure"]);
        subscriber.DailySections.Should().BeEquivalentTo(["dotnet"]);
    }

    // ── POST /api/newsletter/manage/request ──────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("?email=")]
    [InlineData("?email=not-an-email")]
    [InlineData("?email=a%40")]
    public async Task RequestManageLink_MissingOrInvalidEmail_ReturnsBadRequest(string query)
    {
        // Act
        var response = await _client.PostAsync(
            $"/api/newsletter/manage/request{query}",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string NewEmail() => $"neg-{Guid.NewGuid():N}@example.com";

    private static string Token(string email) => NewsletterService.BuildUnsubscribeToken(email, Secret);

    private static object NoValidSectionPayload(string email, string scenario) => scenario switch
    {
        "null-sections" => new { Email = email },
        "empty-sections" => new { Email = email, WeeklySections = Array.Empty<string>(), DailySections = Array.Empty<string>() },
        "blank-sections" => new { Email = email, WeeklySections = new[] { "  " }, DailySections = new[] { "" } },
        "all-section-only" => new { Email = email, WeeklySections = new[] { "all" }, DailySections = Array.Empty<string>() },
        _ => throw new ArgumentOutOfRangeException(nameof(scenario), scenario, "Unknown scenario")
    };

    private async Task<string> SubscribeAsync(string[]? weekly = null, string[]? daily = null)
    {
        var email = NewEmail();
        var response = await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = email, WeeklySections = weekly ?? ["ai"], DailySections = daily ?? [] },
            TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return email;
    }

    private Task<HttpResponseMessage> ConfirmAsync(string email, string token) =>
        _client.GetAsync(
            $"/api/newsletter/confirm?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}",
            TestContext.Current.CancellationToken);

    private Task<HttpResponseMessage> UnsubscribeAsync(string email, string token) =>
        _client.PostAsJsonAsync(
            "/api/newsletter/unsubscribe",
            new { Email = email, Token = token },
            TestContext.Current.CancellationToken);

    private Task<HttpResponseMessage> GetManagePreferencesAsync(string email, string token) =>
        _client.GetAsync(
            $"/api/newsletter/manage?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}",
            TestContext.Current.CancellationToken);

    private async Task<IReadOnlyList<string>> GetWeeklySectionsAsync(string email)
    {
        var response = await GetManagePreferencesAsync(email, Token(email));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var subscriber = await response.Content.ReadFromJsonAsync<NewsletterSubscriber>(
            TestContext.Current.CancellationToken);
        return subscriber!.WeeklySections;
    }

    private async Task ConfirmInDbAsync(string email)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        await connection.ExecuteAsync(
            "UPDATE newsletter_subscribers SET is_confirmed = TRUE, confirmed_at = NOW() WHERE email = @Email",
            new { Email = email });
    }

    private async Task<int> CountSubscribersAsync(string email)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        return await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM newsletter_subscribers WHERE email = @Email",
            new { Email = email });
    }

    private async Task<bool> IsConfirmedAsync(string email)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        return await connection.ExecuteScalarAsync<bool>(
            "SELECT is_confirmed FROM newsletter_subscribers WHERE email = @Email",
            new { Email = email });
    }

    private async Task<bool> IsUnsubscribedAsync(string email)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        return await connection.ExecuteScalarAsync<bool>(
            "SELECT unsubscribed_at IS NOT NULL FROM newsletter_subscribers WHERE email = @Email",
            new { Email = email });
    }
}
