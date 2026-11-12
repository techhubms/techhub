using Dapper;
using FluentAssertions;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for the DevBlogs feed review migration (052_devblogs_feed_review.sql). The fixture database already ran it,
/// so these tests check the feeds it added and disabled, and that running the script again changes nothing.
/// </summary>
public class DevBlogsFeedMigrationTests : IClassFixture<DatabaseFixture<DevBlogsFeedMigrationTests>>
{
    private const string AiFoundryUrl = "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=azure-ai-foundry";
    private const string DeletedAiUrl = "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=AI";

    private readonly DatabaseFixture<DevBlogsFeedMigrationTests> _fixture;

    public DevBlogsFeedMigrationTests(DatabaseFixture<DevBlogsFeedMigrationTests> fixture) => _fixture = fixture;

    private async Task<bool?> IsEnabledAsync(string url) =>
        await _fixture.Connection.ExecuteScalarAsync<bool?>("SELECT enabled FROM rss_feed_configs WHERE url = @Url", new { Url = url });

    [Theory]
    [InlineData("https://devblogs.microsoft.com/aspire/feed/", "_news")]
    [InlineData("https://devblogs.microsoft.com/aspnet/feed/", "_news")]
    [InlineData("https://devblogs.microsoft.com/cosmosdb/feed/", "_news")]
    [InlineData("https://devblogs.microsoft.com/azure-sql/feed/", "_news")]
    [InlineData("https://devblogs.microsoft.com/documentdb/feed/", "_news")]
    [InlineData("https://devblogs.microsoft.com/java/feed/", "_news")]
    [InlineData("https://devblogs.microsoft.com/python/feed/", "_news")]
    [InlineData("https://devblogs.microsoft.com/ise/feed/", "_news")]
    [InlineData("https://devblogs.microsoft.com/identity/feed/", "_news")]
    [InlineData("https://devblogs.microsoft.com/foundry-on-windows/feed/", "_news")]
    [InlineData("https://devblogs.microsoft.com/xcode/feed/", "_news")]
    [InlineData(AiFoundryUrl, "_community")]
    [InlineData("https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/board?board.id=copilot-studio", "_community")]
    [InlineData("https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=AzureDatabases", "_community")]
    public async Task Migration_NewFeed_IsEnabledInItsCollection(string url, string outputDir)
    {
        (await IsEnabledAsync(url)).Should().BeTrue();
        (await _fixture.Connection.ExecuteScalarAsync<string>("SELECT output_dir FROM rss_feed_configs WHERE url = @Url", new { Url = url }))
            .Should().Be(outputDir);
    }

    [Theory]
    [InlineData("https://devblogs.microsoft.com/semantic-kernel/feed/")]
    [InlineData(DeletedAiUrl)]
    public async Task Migration_FeedThatNoLongerExists_IsDisabled(string url)
    {
        (await IsEnabledAsync(url)).Should().BeFalse();
    }

    [Fact]
    public async Task Migration_DeletedAiFeedWasRenamedToAiFoundry_DoesNotAddItASecondTime()
    {
        // Arrange: production state, where the deleted AI feed row was edited into the AI Foundry feed under another name.
        // Done in a transaction that is rolled back, so the other tests keep the state the migration left.
        using var transaction = _fixture.Connection.BeginTransaction();
        await _fixture.Connection.ExecuteAsync("DELETE FROM rss_feed_configs WHERE url = @Url", new { Url = AiFoundryUrl }, transaction);
        await _fixture.Connection.ExecuteAsync(
            "UPDATE rss_feed_configs SET name = 'Renamed in production', url = @Url, enabled = TRUE WHERE url = @OldUrl",
            new { Url = AiFoundryUrl, OldUrl = DeletedAiUrl },
            transaction);

        // Act
        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync(), transaction: transaction);

        // Assert
        var rows = (await _fixture.Connection.QueryAsync<(string Name, bool Enabled)>(
            "SELECT name, enabled FROM rss_feed_configs WHERE url = @Url", new { Url = AiFoundryUrl }, transaction)).ToList();
        rows.Should().ContainSingle().Which.Should().Be(("Renamed in production", true));
        (await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM rss_feed_configs WHERE name = 'Microsoft Tech Community (AI Foundry)'", transaction: transaction))
            .Should().Be(0);

        transaction.Rollback();
    }

    [Fact]
    public async Task Migration_FeedWithTheNameButAnotherUrl_IsNotAddedAndDoesNotFail()
    {
        // Arrange: the name is unique too, so an admin-edited feed with one of the new names must not break the startup
        using var transaction = _fixture.Connection.BeginTransaction();
        await _fixture.Connection.ExecuteAsync("DELETE FROM rss_feed_configs WHERE url = @Url", new { Url = AiFoundryUrl }, transaction);
        await _fixture.Connection.ExecuteAsync(
            "INSERT INTO rss_feed_configs (name, url, output_dir, enabled) VALUES ('Microsoft Tech Community (AI Foundry)', 'https://example.com/other', '_community', TRUE)",
            transaction: transaction);

        // Act
        var act = async () => await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync(), transaction: transaction);

        // Assert
        await act.Should().NotThrowAsync();
        (await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM rss_feed_configs WHERE url = @Url", new { Url = AiFoundryUrl }, transaction))
            .Should().Be(0);

        transaction.Rollback();
    }
    [Fact]
    public async Task Migration_RunAgain_ChangesNothing()
    {
        var before = await _fixture.Connection.ExecuteScalarAsync<long>("SELECT count(*) FROM rss_feed_configs");

        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync());

        (await _fixture.Connection.ExecuteScalarAsync<long>("SELECT count(*) FROM rss_feed_configs")).Should().Be(before);
    }

    private static async Task<string> ReadMigrationAsync()
    {
        var assembly = typeof(RssFeedConfigRepository).Assembly;
        var name = assembly.GetManifestResourceNames().Single(n => n.EndsWith("052_devblogs_feed_review.sql", StringComparison.Ordinal));
        await using var stream = assembly.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
