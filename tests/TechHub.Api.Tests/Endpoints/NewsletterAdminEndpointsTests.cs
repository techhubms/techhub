using System.Data;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the admin newsletter endpoints: subscriber listing/filtering/paging/update/delete,
/// send log, trigger kind normalisation and test-send validation.
/// Every test seeds subscribers with a unique prefix so listings can be scoped with the search filter.
/// </summary>
public class NewsletterAdminEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;
    private readonly TechHubIntegrationTestApiFactory _factory;

    public NewsletterAdminEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _factory = factory;
        _client = factory.CreateClient();
    }

    // ── GET /api/admin/newsletter/subscribers ────────────────────────────────

    [Fact]
    public async Task GetSubscribers_SearchMatchesEmailAndDisplayName()
    {
        // Arrange
        var prefix = NewPrefix();
        await InsertSubscriberAsync($"{prefix}-a@example.com", displayName: $"Alpha {prefix}");
        await InsertSubscriberAsync($"{prefix}-b@example.com", displayName: $"Beta {prefix}");

        // Act
        var byPrefix = await GetSubscribersAsync($"search={prefix}");
        var byDisplayName = await GetSubscribersAsync($"search={Uri.EscapeDataString($"Alpha {prefix}")}");

        // Assert
        byPrefix.Should().HaveCount(2);
        byDisplayName.Should().ContainSingle().Which.Email.Should().Be($"{prefix}-a@example.com");
    }

    [Fact]
    public async Task GetSubscribers_ConfirmedFilter_ReturnsOnlyMatchingSubscribers()
    {
        // Arrange
        var prefix = NewPrefix();
        await InsertSubscriberAsync($"{prefix}-confirmed@example.com", isConfirmed: true);
        await InsertSubscriberAsync($"{prefix}-pending1@example.com");
        await InsertSubscriberAsync($"{prefix}-pending2@example.com");

        // Act
        var confirmed = await GetSubscribersAsync($"search={prefix}&confirmed=true");
        var unconfirmed = await GetSubscribersAsync($"search={prefix}&confirmed=false");
        var all = await GetSubscribersAsync($"search={prefix}");

        // Assert
        confirmed.Should().ContainSingle().Which.Email.Should().Be($"{prefix}-confirmed@example.com");
        unconfirmed.Should().HaveCount(2).And.OnlyContain(s => !s.IsConfirmed);
        all.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetSubscribers_Paging_SplitsResultsWithoutOverlap()
    {
        // Arrange
        var prefix = NewPrefix();
        for (var i = 0; i < 3; i++)
        {
            await InsertSubscriberAsync($"{prefix}-{i}@example.com");
        }

        // Act
        var firstPage = await GetSubscribersAsync($"search={prefix}&page=1&pageSize=2");
        var secondPage = await GetSubscribersAsync($"search={prefix}&page=2&pageSize=2");

        // Assert
        firstPage.Should().HaveCount(2);
        secondPage.Should().HaveCount(1);
        firstPage.Select(s => s.Id).Concat(secondPage.Select(s => s.Id)).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task GetSubscribers_ExcludesUnsubscribedSubscribers()
    {
        // Arrange
        var prefix = NewPrefix();
        var activeId = await InsertSubscriberAsync($"{prefix}-active@example.com");
        var leftId = await InsertSubscriberAsync($"{prefix}-left@example.com");
        await MarkUnsubscribedAsync(leftId);

        // Act
        var subscribers = await GetSubscribersAsync($"search={prefix}");

        // Assert
        subscribers.Should().ContainSingle().Which.Id.Should().Be(activeId);
    }

    // ── PUT /api/admin/newsletter/subscribers/{id} ───────────────────────────

    [Fact]
    public async Task UpdateSubscriber_ExistingSubscriber_UpdatesNameAndSections()
    {
        // Arrange
        var prefix = NewPrefix();
        var email = $"{prefix}@example.com";
        var id = await InsertSubscriberAsync(email, displayName: "Old Name");

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/newsletter/subscribers/{id}",
            new { DisplayName = "New Name", WeeklySections = new[] { "azure" }, DailySections = new[] { "ai" } },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var subscriber = (await GetSubscribersAsync($"search={prefix}")).Should().ContainSingle().Subject;
        subscriber.DisplayName.Should().Be("New Name");
        subscriber.WeeklySections.Should().BeEquivalentTo(["azure"]);
        subscriber.DailySections.Should().BeEquivalentTo(["ai"]);
    }

    [Fact]
    public async Task UpdateSubscriber_UnknownId_ReturnsNotFound()
    {
        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/newsletter/subscribers/999999999",
            new { DisplayName = "Nobody", WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateSubscriber_UnsubscribedSubscriber_ReturnsNotFound()
    {
        // Arrange
        var id = await InsertSubscriberAsync($"{NewPrefix()}@example.com");
        await MarkUnsubscribedAsync(id);

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/newsletter/subscribers/{id}",
            new { DisplayName = "Gone", WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── DELETE /api/admin/newsletter/subscribers/{id} ────────────────────────

    [Fact]
    public async Task DeleteSubscriber_UnknownId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync(
            "/api/admin/newsletter/subscribers/999999999",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── GET /api/admin/newsletter/send-log ───────────────────────────────────

    [Fact]
    public async Task GetSendLog_ReturnsLoggedEntryWithAllFields()
    {
        // Arrange
        var targetKey = $"log-{NewPrefix()}";
        await InsertSendLogAsync(targetKey);

        // Act
        var response = await _client.GetAsync(
            "/api/admin/newsletter/send-log?count=500",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var log = await response.Content.ReadFromJsonAsync<List<NewsletterSendLogEntry>>(
            TestContext.Current.CancellationToken);
        var entry = log!.Should().ContainSingle(e => e.TargetKey == targetKey).Subject;
        entry.SendKind.Should().Be("weekly-roundup");
        entry.RecipientCount.Should().Be(5);
        entry.FailedCount.Should().Be(1);
        entry.Status.Should().Be("sent");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task GetSendLog_CountIsClampedToAtLeastOne(int count)
    {
        // Arrange — at least two entries exist
        await InsertSendLogAsync($"log-{NewPrefix()}");
        await InsertSendLogAsync($"log-{NewPrefix()}");

        // Act
        var response = await _client.GetAsync(
            $"/api/admin/newsletter/send-log?count={count}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var log = await response.Content.ReadFromJsonAsync<List<NewsletterSendLogEntry>>(
            TestContext.Current.CancellationToken);
        log.Should().HaveCount(1);
    }

    // ── POST /api/admin/newsletter/test-send ─────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("?email=")]
    [InlineData("?email=not-an-email")]
    [InlineData("?email=a%40")]
    public async Task TestSend_MissingOrInvalidEmail_ReturnsBadRequest(string query)
    {
        // Act
        var response = await _client.PostAsync(
            $"/api/admin/newsletter/test-send{query}",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("")]
    [InlineData("&kind=daily")]
    [InlineData("&kind=unknown")]
    public async Task TestSend_ValidEmailAndSections_ReturnsAccepted(string kindQuery)
    {
        // Act
        var response = await _client.PostAsync(
            $"/api/admin/newsletter/test-send?email=test-send%40example.com&sections=ai&sections=azure{kindQuery}",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        body.GetProperty("message").GetString().Should().Be("Newsletter test send triggered");
    }

    // ── POST /api/admin/newsletter/trigger ───────────────────────────────────

    [Theory]
    [InlineData("", "roundup")]
    [InlineData("bogus", "roundup")]
    [InlineData(" DAILY ", "daily")]
    public async Task Trigger_NormalisesKind(string kind, string expectedKind)
    {
        // Act
        var response = await _client.PostAsync(
            $"/api/admin/newsletter/trigger?kind={Uri.EscapeDataString(kind)}",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        body.GetProperty("message").GetString().Should().Be($"Newsletter {expectedKind} send triggered");
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string NewPrefix() => $"nladmin-{Guid.NewGuid():N}";

    private async Task<List<NewsletterSubscriber>> GetSubscribersAsync(string query)
    {
        var response = await _client.GetAsync(
            $"/api/admin/newsletter/subscribers?{query}",
            TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var subscribers = await response.Content.ReadFromJsonAsync<List<NewsletterSubscriber>>(
            TestContext.Current.CancellationToken);
        return subscribers!;
    }

    private async Task<long> InsertSubscriberAsync(string email, string? displayName = null, bool isConfirmed = false)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        return await connection.ExecuteScalarAsync<long>(
            """
            INSERT INTO newsletter_subscribers (email, display_name, is_confirmed, confirmed_at, preferences)
            VALUES (@Email, @DisplayName, @IsConfirmed, CASE WHEN @IsConfirmed THEN NOW() ELSE NULL END,
                    '{"weeklySections":["ai"],"dailySections":[]}'::jsonb)
            RETURNING id
            """,
            new { Email = email, DisplayName = displayName, IsConfirmed = isConfirmed });
    }

    private async Task MarkUnsubscribedAsync(long id)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        await connection.ExecuteAsync(
            "UPDATE newsletter_subscribers SET unsubscribed_at = NOW() WHERE id = @Id",
            new { Id = id });
    }

    private async Task InsertSendLogAsync(string targetKey)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        await connection.ExecuteAsync(
            """
            INSERT INTO newsletter_send_log (send_kind, target_key, recipient_count, failed_count, status, error_message)
            VALUES ('weekly-roundup', @TargetKey, 5, 1, 'sent', NULL)
            """,
            new { TargetKey = targetKey });
    }
}
