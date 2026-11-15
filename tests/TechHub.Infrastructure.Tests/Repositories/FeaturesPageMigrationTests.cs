using Dapper;
using FluentAssertions;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for migrations 058 (timeline open to everyone), 059 (no billing notice on the features page) and 060 (updated plans).
/// The fixture database already ran them; each test puts the old state back inside a transaction that is rolled back, runs the script
/// and checks the result, so the other tests keep the state the migrations left.
/// </summary>
public class FeaturesPageMigrationTests : IClassFixture<DatabaseFixture<FeaturesPageMigrationTests>>
{
    private readonly DatabaseFixture<FeaturesPageMigrationTests> _fixture;

    public FeaturesPageMigrationTests(DatabaseFixture<FeaturesPageMigrationTests> fixture) => _fixture = fixture;

    [Theory]
    [InlineData("admins")]
    [InlineData("disabled")]
    public async Task TimelineMigration_TimelineNotOpenToEveryone_OpensItAndLeavesOtherTogglesAlone(string previousMode)
    {
        using var transaction = _fixture.Connection.BeginTransaction();
        await _fixture.Connection.ExecuteAsync(
            "INSERT INTO feature_toggles (feature_key, mode) VALUES ('other-feature', 'admins')", transaction: transaction);
        await _fixture.Connection.ExecuteAsync(
            "UPDATE feature_toggles SET mode = @Mode WHERE feature_key = 'timeline'", new { Mode = previousMode }, transaction);

        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync("058_timeline_open_to_everyone.sql"), transaction: transaction);

        (await ModeAsync("timeline", transaction)).Should().Be("everyone");
        (await ModeAsync("other-feature", transaction)).Should().Be("admins");
        transaction.Rollback();
    }

    [Fact]
    public async Task TimelineMigration_RunAgain_ChangesNothing()
    {
        var updatedBefore = await _fixture.Connection.ExecuteScalarAsync<DateTime>(
            "SELECT updated_at FROM feature_toggles WHERE feature_key = 'timeline'");

        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync("058_timeline_open_to_everyone.sql"));

        (await ModeAsync("timeline")).Should().Be("everyone");
        (await _fixture.Connection.ExecuteScalarAsync<DateTime>(
            "SELECT updated_at FROM feature_toggles WHERE feature_key = 'timeline'")).Should().Be(updatedBefore);
    }

    [Fact]
    public async Task BillingNoticeMigration_PageDataWithABillingNotice_RemovesOnlyThatKey()
    {
        using var transaction = _fixture.Connection.BeginTransaction();
        await _fixture.Connection.ExecuteAsync(
            """
            INSERT INTO custom_page_data (key, description, json_data)
            VALUES ('githubcopilot-features', 'Features', '{"title":"Features","billingNotice":{"text":"x","links":[]},"subscriptionTiers":[]}')
            ON CONFLICT (key) DO UPDATE SET json_data = EXCLUDED.json_data
            """,
            transaction: transaction);

        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync("059_remove_features_billing_notice.sql"), transaction: transaction);

        var json = await _fixture.Connection.ExecuteScalarAsync<string>(
            "SELECT json_data FROM custom_page_data WHERE key = 'githubcopilot-features'", transaction: transaction);
        json.Should().NotContain("billingNotice").And.Contain("\"title\"").And.Contain("subscriptionTiers");
        transaction.Rollback();
    }

    [Fact]
    public async Task BillingNoticeMigration_RunAgain_ChangesNothing()
    {
        var before = await _fixture.Connection.ExecuteScalarAsync<string>(
            "SELECT json_data FROM custom_page_data WHERE key = 'githubcopilot-features'");

        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync("059_remove_features_billing_notice.sql"));

        (await _fixture.Connection.ExecuteScalarAsync<string>(
            "SELECT json_data FROM custom_page_data WHERE key = 'githubcopilot-features'")).Should().Be(before);
    }

    [Fact]
    public async Task PlansMigration_StoredTiersFromBefore_ReplacesTheTiersAndKeepsTheRestOfThePageData()
    {
        using var transaction = _fixture.Connection.BeginTransaction();
        await _fixture.Connection.ExecuteAsync(
            """
            INSERT INTO custom_page_data (key, description, json_data)
            VALUES ('githubcopilot-features', 'Features', '{"title":"Features","links":{"pricing":"https://example.com"},"subscriptionTiers":[{"id":"old","name":"Old"}]}')
            ON CONFLICT (key) DO UPDATE SET json_data = EXCLUDED.json_data
            """,
            transaction: transaction);

        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync("060_github_copilot_plans_update.sql"), transaction: transaction);

        var json = await _fixture.Connection.ExecuteScalarAsync<string>(
            "SELECT json_data FROM custom_page_data WHERE key = 'githubcopilot-features'", transaction: transaction);
        using var document = System.Text.Json.JsonDocument.Parse(json!);
        document.RootElement.GetProperty("title").GetString().Should().Be("Features");
        document.RootElement.GetProperty("links").GetProperty("pricing").GetString().Should().Be("https://example.com");
        document.RootElement.GetProperty("subscriptionTiers").EnumerateArray().Select(t => t.GetProperty("id").GetString())
            .Should().Equal("free", "student", "pro", "proplus", "max", "business", "enterprise");
        transaction.Rollback();
    }

    [Fact]
    public async Task PlansMigration_RunAgain_ChangesNothing()
    {
        using var transaction = _fixture.Connection.BeginTransaction();
        var migration = await ReadMigrationAsync("060_github_copilot_plans_update.sql");
        const string Select = "SELECT json_data FROM custom_page_data WHERE key = 'githubcopilot-features'";

        await _fixture.Connection.ExecuteAsync(migration, transaction: transaction);
        var afterFirstRun = await _fixture.Connection.ExecuteScalarAsync<string>(Select, transaction: transaction);
        await _fixture.Connection.ExecuteAsync(migration, transaction: transaction);

        (await _fixture.Connection.ExecuteScalarAsync<string>(Select, transaction: transaction)).Should().Be(afterFirstRun);
        transaction.Rollback();
    }

    private async Task<string?> ModeAsync(string key, System.Data.IDbTransaction? transaction = null) =>
        await _fixture.Connection.ExecuteScalarAsync<string>(
            "SELECT mode FROM feature_toggles WHERE feature_key = @Key", new { Key = key }, transaction);

    private static async Task<string> ReadMigrationAsync(string fileName)
    {
        var assembly = typeof(RssFeedConfigRepository).Assembly;
        var name = assembly.GetManifestResourceNames().Single(n => n.EndsWith(fileName, StringComparison.Ordinal));
        await using var stream = assembly.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
