using Dapper;
using FluentAssertions;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="GhcFeatureRepository"/>: the vscode_update_items table.
/// The features of GitHub Copilot are part of the catalog and are tested through <see cref="ProductFeatureRepository"/> (see ProductFeatureTests).
/// Each test uses unique slugs to avoid cross-test contamination in the shared database.
/// </summary>
public class GhcFeatureRepositoryTests : IClassFixture<DatabaseFixture<GhcFeatureRepositoryTests>>
{
    private readonly DatabaseFixture<GhcFeatureRepositoryTests> _fixture;
    private readonly GhcFeatureRepository _repository;

    public GhcFeatureRepositoryTests(DatabaseFixture<GhcFeatureRepositoryTests> fixture)
    {
        _fixture = fixture;
        _repository = new GhcFeatureRepository(fixture.Connection);
    }

    [Fact]
    public async Task Schema_HasNoLegacyGhcTablesOrGhesColumn()
    {
        // Act
        var legacy = (await _fixture.Connection.QueryAsync<string>(
            @"SELECT table_name || '.' || column_name FROM information_schema.columns
              WHERE table_schema = current_schema()
                AND (table_name IN ('ghc_features', 'ghc_feature_content')
                     OR (table_name = 'product_features' AND column_name = 'ghes_support'))")).ToList();

        // Assert
        legacy.Should().BeEmpty("the catalog migration drops the ghc_ tables and the GHES flag");
    }

    [Fact]
    public async Task AddVscodeUpdateItemAsync_InsertsRow()
    {
        // Arrange — need a content item so FK is satisfied
        var slug = await SeedVideoAsync("VsCode Update Item");

        // Act
        await _repository.AddVscodeUpdateItemAsync("videos", slug, CancellationToken.None);

        // Assert
        var exists = await _fixture.Connection.QueryFirstAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM vscode_update_items WHERE collection_name = 'videos' AND slug = @Slug)",
            new { Slug = slug });
        exists.Should().BeTrue("AddVscodeUpdateItemAsync should insert into vscode_update_items");
    }

    [Fact]
    public async Task AddVscodeUpdateItemAsync_CalledTwice_DoesNotThrow()
    {
        // Arrange
        var slug = await SeedVideoAsync("Idempotent Item");

        // Act — idempotent: calling twice should not throw
        await _repository.AddVscodeUpdateItemAsync("videos", slug, CancellationToken.None);
        var act = () => _repository.AddVscodeUpdateItemAsync("videos", slug, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync("ON CONFLICT DO NOTHING should make the call idempotent");
    }

    private async Task<string> SeedVideoAsync(string title)
    {
        var slug = $"ghc-video-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync(@"
            INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                primary_section_name, external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, 'videos', @Title, '', '', 0, 'ai', @Url, '', 'Test Feed', '', @Hash)",
            new { Slug = slug, Title = title, Url = $"https://example.com/{slug}", Hash = Guid.NewGuid().ToString() });
        return slug;
    }
}
