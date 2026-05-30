using System.Data;
using System.Net;
using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
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
    public async Task AdminTriggerNewsletter_WhenScheduledSendingDisabled_StillProcessesManualSend()
    {
        await using var arrangeScope = _factory.Services.CreateAsyncScope();
        var arrangeConnection = arrangeScope.ServiceProvider.GetRequiredService<IDbConnection>();
        var latestRoundups = await LoadLatestSectionRoundupsAsync(arrangeConnection);
        latestRoundups.Should().NotBeEmpty();

        var expectedMonday = GetExpectedRoundupMonday(DateTimeOffset.UtcNow, ResolveRoundupTimeZone());
        await SetLatestRoundupsEpochAsync(arrangeConnection, latestRoundups, ToRoundupEpoch(expectedMonday));
        var roundupTargetKey = expectedMonday.ToString("yyyy-MM-dd");

        try
        {
            var countBefore = await arrangeConnection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key = @TargetKey",
                new { TargetKey = roundupTargetKey });

            var response = await _client.PostAsync("/api/admin/newsletter/trigger?kind=roundup", null, TestContext.Current.CancellationToken);
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);

            var timedOut = true;
            for (var attempt = 0; attempt < 25; attempt++)
            {
                await Task.Delay(200, TestContext.Current.CancellationToken);
                await using var pollScope = _factory.Services.CreateAsyncScope();
                var pollConnection = pollScope.ServiceProvider.GetRequiredService<IDbConnection>();
                var countAfter = await pollConnection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key = @TargetKey",
                    new { TargetKey = roundupTargetKey });

                if (countAfter > countBefore)
                {
                    timedOut = false;
                    break;
                }
            }

            timedOut.Should().BeFalse("manual admin trigger should execute even when scheduled sends are disabled");
        }
        finally
        {
            await RestoreLatestRoundupsEpochAsync(arrangeConnection, latestRoundups);
        }
    }

    [Fact]
    public async Task AdminTriggerNewsletter_DailyTriggerProcessesDailyOverviewSend()
    {
        await using var arrangeScope = _factory.Services.CreateAsyncScope();
        var options = arrangeScope.ServiceProvider.GetRequiredService<IOptions<NewsletterOptions>>().Value;
        var timeZone = ResolveTimeZone(options.DailyDigestTimeZoneId);
        var localNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);
        var day = DateOnly.FromDateTime(localNow.DateTime.Date.AddDays(-1));
        var dayStartUtc = day.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

        var arrangeConnection = arrangeScope.ServiceProvider.GetRequiredService<IDbConnection>();
        await arrangeConnection.ExecuteAsync("""
            DELETE FROM newsletter_subscribers WHERE email = 'daily-trigger@example.com';
            DELETE FROM content_items WHERE collection_name = 'blogs' AND slug = 'daily-trigger-item';
            """);
        await arrangeConnection.ExecuteAsync("""
            INSERT INTO newsletter_subscribers (email, is_confirmed, confirmed_at, preferences)
            VALUES ('daily-trigger@example.com', TRUE, NOW(), '{"weeklySections":[],"dailySections":["ai"]}'::jsonb)
            """);
        await arrangeConnection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai, created_at)
            VALUES
                ('daily-trigger-item', 'blogs', 'Daily Trigger Item', 'Body', 'Excerpt', 0,
                 'ai', '/ai/all', 'TechHub', 'TechHub', ',AI,',
                 1, 'hash-daily-trigger-item', TRUE, @CreatedAt)
            ON CONFLICT (collection_name, slug) DO UPDATE SET created_at = EXCLUDED.created_at
            """, new { CreatedAt = dayStartUtc.AddHours(12) });

        var countBefore = await arrangeConnection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM newsletter_send_log WHERE send_kind = 'daily-overview' AND target_key = @TargetKey",
            new { TargetKey = day.ToString("yyyy-MM-dd") });

        var response = await _client.PostAsync("/api/admin/newsletter/trigger?kind=daily", null, TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var timedOut = true;
        for (var attempt = 0; attempt < 25; attempt++)
        {
            await Task.Delay(200, TestContext.Current.CancellationToken);
            await using var pollScope = _factory.Services.CreateAsyncScope();
            var pollConnection = pollScope.ServiceProvider.GetRequiredService<IDbConnection>();
            var countAfter = await pollConnection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM newsletter_send_log WHERE send_kind = 'daily-overview' AND target_key = @TargetKey",
                new { TargetKey = day.ToString("yyyy-MM-dd") });

            if (countAfter > countBefore)
            {
                timedOut = false;
                break;
            }
        }

        timedOut.Should().BeFalse("manual admin daily trigger should execute daily overview send");
    }

    [Fact]
    public async Task AdminTriggerNewsletter_RoundupsNotOnExpectedMonday_SkipsSend()
    {
        await using var arrangeScope = _factory.Services.CreateAsyncScope();
        var arrangeConnection = arrangeScope.ServiceProvider.GetRequiredService<IDbConnection>();
        var latestRoundups = await LoadLatestSectionRoundupsAsync(arrangeConnection);
        latestRoundups.Should().NotBeEmpty();

        var expectedMonday = GetExpectedRoundupMonday(DateTimeOffset.UtcNow, ResolveRoundupTimeZone());
        var nonMondayEpoch = ToRoundupEpoch(expectedMonday.AddYears(10).AddDays(1));
        var roundupTargetKey = expectedMonday.ToString("yyyy-MM-dd");

        await SetLatestRoundupsEpochAsync(arrangeConnection, latestRoundups, nonMondayEpoch);

        try
        {
            var countBefore = await arrangeConnection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key = @TargetKey",
                new { TargetKey = roundupTargetKey });

            var response = await _client.PostAsync("/api/admin/newsletter/trigger?kind=roundup", null, TestContext.Current.CancellationToken);
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);

            var wasSent = false;
            for (var attempt = 0; attempt < 25; attempt++)
            {
                await Task.Delay(200, TestContext.Current.CancellationToken);
                await using var pollScope = _factory.Services.CreateAsyncScope();
                var pollConnection = pollScope.ServiceProvider.GetRequiredService<IDbConnection>();
                var countAfter = await pollConnection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM newsletter_send_log WHERE send_kind = 'weekly-roundup' AND target_key = @TargetKey",
                    new { TargetKey = roundupTargetKey });
                if (countAfter > countBefore)
                {
                    wasSent = true;
                    break;
                }
            }

            wasSent.Should().BeFalse("roundup send should be skipped until latest section roundups match expected Monday");
        }
        finally
        {
            await RestoreLatestRoundupsEpochAsync(arrangeConnection, latestRoundups);
        }
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
    public async Task Confirm_WithValidToken_ReturnsOkConfirmed()
    {
        const string Email = "confirm-valid@example.com";

        await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = Email, WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        var token = NewsletterService.BuildConfirmToken(Email, "integration-test-secret");
        var response = await _client.GetAsync(
            $"/api/newsletter/confirm?email={Uri.EscapeDataString(Email)}&token={Uri.EscapeDataString(token)}",
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<MessageResponse>(TestContext.Current.CancellationToken);
        body!.Message.Should().Be("Your subscription is confirmed.");

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var isConfirmed = await connection.ExecuteScalarAsync<bool>(
            "SELECT is_confirmed FROM newsletter_subscribers WHERE email = @Email",
            new { Email });
        isConfirmed.Should().BeTrue();
    }

    [Fact]
    public async Task Confirm_WhenAlreadyConfirmed_ReturnsOkWithAlreadyConfirmedMessage()
    {
        const string Email = "confirm-already@example.com";

        await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = Email, WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        await connection.ExecuteAsync(
            "UPDATE newsletter_subscribers SET is_confirmed = TRUE WHERE email = @Email",
            new { Email });

        var token = NewsletterService.BuildConfirmToken(Email, "integration-test-secret");
        var response = await _client.GetAsync(
            $"/api/newsletter/confirm?email={Uri.EscapeDataString(Email)}&token={Uri.EscapeDataString(token)}",
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<MessageResponse>(TestContext.Current.CancellationToken);
        body!.Message.Should().Be("Your subscription has already been confirmed.");
    }

    [Fact]
    public async Task Confirm_WithInvalidToken_ReturnsBadRequest()
    {
        const string Email = "confirm-badtoken@example.com";

        await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = Email, WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        var response = await _client.GetAsync(
            $"/api/newsletter/confirm?email={Uri.EscapeDataString(Email)}&token=invalid-token",
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Subscribe_WhenSubscriberAlreadyConfirmed_DoesNotOverwritePreferences()
    {
        const string Email = "no-overwrite@example.com";

        await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = Email, WeeklySections = new[] { "ai" }, DailySections = Array.Empty<string>() },
            TestContext.Current.CancellationToken);

        await using var scope = _factory.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        await connection.ExecuteAsync(
            "UPDATE newsletter_subscribers SET is_confirmed = TRUE WHERE email = @Email",
            new { Email });

        // Attacker re-subscribes with different preferences
        var resubscribeResponse = await _client.PostAsJsonAsync(
            "/api/newsletter/subscribe",
            new { Email = Email, WeeklySections = new[] { "dotnet" }, DailySections = new[] { "azure" } },
            TestContext.Current.CancellationToken);

        resubscribeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var prefs = await connection.QuerySingleAsync<(string Preferences, bool IsConfirmed)>(
            "SELECT preferences::text AS Preferences, is_confirmed AS IsConfirmed FROM newsletter_subscribers WHERE email = @Email",
            new { Email });

        prefs.IsConfirmed.Should().BeTrue();
        prefs.Preferences.Should().Contain("ai", "confirmed subscriber preferences must not be overwritten");
        prefs.Preferences.Should().NotContain("dotnet");
    }

    private sealed class MessageResponse
    {
        public string? Message { get; init; }
    }

    private sealed record LatestRoundupRow(string Slug, long DateEpoch);

    private static TimeZoneInfo ResolveTimeZone(string configuredId)
    {
        if (!string.IsNullOrWhiteSpace(configuredId))
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(configuredId);
            }
            catch (TimeZoneNotFoundException)
            {
            }
            catch (InvalidTimeZoneException)
            {
            }
        }

        return TimeZoneInfo.Utc;
    }

    private static TimeZoneInfo ResolveRoundupTimeZone()
    {
        return TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Romance Standard Time" : "Europe/Brussels");
    }

    private static DateOnly GetExpectedRoundupMonday(DateTimeOffset utcNow, TimeZoneInfo roundupTimeZone)
    {
        var localDate = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(utcNow, roundupTimeZone).Date);
        var daysSinceMonday = ((int)localDate.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        return localDate.AddDays(-daysSinceMonday);
    }

    private static long ToRoundupEpoch(DateOnly publishDate)
    {
        var roundupTimeZone = ResolveRoundupTimeZone();
        var publishLocal = publishDate.ToDateTime(new TimeOnly(9, 0, 0));
        return (long)TimeZoneInfo.ConvertTimeToUtc(publishLocal, roundupTimeZone)
            .Subtract(DateTime.UnixEpoch)
            .TotalSeconds;
    }

    private static async Task<List<LatestRoundupRow>> LoadLatestSectionRoundupsAsync(IDbConnection connection)
    {
        return (await connection.QueryAsync<LatestRoundupRow>(
            """
            SELECT DISTINCT ON (primary_section_name)
                slug AS Slug,
                date_epoch AS DateEpoch
            FROM content_items
            WHERE collection_name = 'roundups'
              AND primary_section_name IS NOT NULL
              AND primary_section_name <> 'all'
            ORDER BY primary_section_name, date_epoch DESC
            """)).ToList();
    }

    private static Task SetLatestRoundupsEpochAsync(IDbConnection connection, IEnumerable<LatestRoundupRow> latestRoundups, long dateEpoch)
    {
        return connection.ExecuteAsync(
            "UPDATE content_items SET date_epoch = @DateEpoch WHERE collection_name = 'roundups' AND slug = ANY(@Slugs)",
            new
            {
                DateEpoch = dateEpoch,
                Slugs = latestRoundups.Select(r => r.Slug).ToArray()
            });
    }

    private static Task RestoreLatestRoundupsEpochAsync(IDbConnection connection, IEnumerable<LatestRoundupRow> latestRoundups)
    {
        return connection.ExecuteAsync(
            """
            UPDATE content_items AS c
            SET date_epoch = v.date_epoch
            FROM (SELECT UNNEST(@Slugs) AS slug, UNNEST(@Epochs) AS date_epoch) AS v
            WHERE c.collection_name = 'roundups'
              AND c.slug = v.slug
            """,
            new
            {
                Slugs = latestRoundups.Select(r => r.Slug).ToArray(),
                Epochs = latestRoundups.Select(r => r.DateEpoch).ToArray()
            });
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
    public async Task ManagePreferences_GetWithInvalidToken_ReturnsNotFound()
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
