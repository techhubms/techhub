using System.Globalization;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="GhcFeatureRepository"/>.
/// Covers reading the features of GitHub Copilot in the catalog (product_features, product_feature_content) and vscode_update_items.
/// Features are edited through <see cref="ProductFeatureRepository"/> (see ProductFeatureTests).
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
        legacy.Should().BeEmpty("migration 056 drops the ghc_ tables and the GHES flag");
    }

    [Fact]
    public async Task GetAllFeaturesAsync_FeatureOfAnotherProduct_IsNotReturned()
    {
        // Arrange
        var other = await CreateFeatureOfAnotherProductAsync();

        // Act
        var all = await _repository.GetAllFeaturesAsync(CancellationToken.None);

        // Assert
        all.Should().NotContain(f => f.Slug == other.Slug);
    }

    [Fact]
    public async Task GetAllFeaturesAsync_FeatureOfAChildProduct_IsReturned()
    {
        // Arrange
        var slug = $"ghc-test-child-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync(
            @"INSERT INTO product_features (product_id, name, slug)
              SELECT id, @Name, @Slug FROM products WHERE lower(name) = 'github copilot cli'",
            new { Name = $"Child feature {slug}", Slug = slug });

        // Act
        var all = await _repository.GetAllFeaturesAsync(CancellationToken.None);

        // Assert
        all.Should().Contain(f => f.Slug == slug);
    }

    [Fact]
    public async Task GetAllFeaturesAsync_NoReleaseDate_FallsBackOnTheFirstLaunchPreviewOrGaEvent()
    {
        // Arrange
        var title = $"Dated by events {Guid.NewGuid():N}";
        var slug = await SeedFeatureAsync(title);
        var itemSlug = await SeedNewsItemAsync();
        var timeline = new TimelineEventRepository(_fixture.Connection);
        await timeline.ReplaceEventsAsync("news", itemSlug,
            [GaEvent("GitHub Copilot", title, "2026-05-20"), GaEvent("GitHub Copilot", title, "2026-03-10", "preview"), GaEvent("GitHub Copilot", title, "2025-01-01", "retirement")],
            "source", 3, CancellationToken.None);

        // Act
        var feature = await _repository.GetFeatureBySlugAsync(slug, CancellationToken.None);

        // Assert
        feature!.ReleaseDate.Should().Be(new DateTimeOffset(2026, 3, 10, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds());
    }

    [Fact]
    public async Task GetAllFeaturesAsync_ReleaseDateOfTheFeature_WinsOverTheEvents()
    {
        // Arrange
        var title = $"Dated by admin {Guid.NewGuid():N}";
        var slug = await SeedFeatureAsync(title, releaseDate: 1_700_000_000L);
        var itemSlug = await SeedNewsItemAsync();
        var timeline = new TimelineEventRepository(_fixture.Connection);
        await timeline.ReplaceEventsAsync("news", itemSlug, [GaEvent("GitHub Copilot", title, "2026-05-20")], "source", 3, CancellationToken.None);

        // Act
        var feature = await _repository.GetFeatureBySlugAsync(slug, CancellationToken.None);

        // Assert
        feature!.ReleaseDate.Should().Be(1_700_000_000L);
    }

    [Fact]
    public async Task CatalogFeature_CreatedOnTheProductPage_GetsASlugAndAppearsInTheFeatures()
    {
        // Arrange
        var productId = await _fixture.Connection.QueryFirstAsync<long>("SELECT id FROM products WHERE lower(name) = 'github copilot'");
        var features = new ProductFeatureRepository(_fixture.Connection, NullLogger<ProductFeatureRepository>.Instance);
        var name = $"Catalog feature {Guid.NewGuid():N}";

        // Act
        var created = await features.CreateAsync(productId, name, CancellationToken.None);

        // Assert
        created.Succeeded.Should().BeTrue(created.Message);
        var all = await _repository.GetAllFeaturesAsync(CancellationToken.None);
        all.Should().ContainSingle(f => f.Title == name).Which.Slug.Should().StartWith("catalog-feature-");
    }

    [Fact]
    public async Task GetAllFeaturesAsync_ReturnsTheDescriptionOfTheFeature()
    {
        // Arrange
        var slug = await SeedFeatureAsync("Get All Test", description: "Excerpt for get all", releaseDate: 1_700_000_000L);

        // Act
        var all = await _repository.GetAllFeaturesAsync(CancellationToken.None);

        // Assert
        var found = all.Should().ContainSingle(f => f.Slug == slug).Subject;
        found.Title.Should().Be("Get All Test");
        found.Description.Should().Be("Excerpt for get all");
        found.ReleaseDate.Should().Be(1_700_000_000L);
    }

    [Fact]
    public async Task GetAllFeaturesAsync_OrderedByReleaseDateDescending()
    {
        // Arrange
        var slugOlder = await SeedFeatureAsync("Older", releaseDate: 1_600_000_000L);
        var slugNewer = await SeedFeatureAsync("Newer", releaseDate: 1_900_000_000L);

        // Act
        var all = await _repository.GetAllFeaturesAsync(CancellationToken.None);

        // Assert — newer feature must appear before older feature
        var newerIndex = all.ToList().FindIndex(f => f.Slug == slugNewer);
        var olderIndex = all.ToList().FindIndex(f => f.Slug == slugOlder);
        newerIndex.Should().BeLessThan(olderIndex, "features should be ordered by release_date DESC");
    }

    [Fact]
    public async Task GetFeatureBySlugAsync_ExistingSlug_ReturnsFeature()
    {
        // Arrange
        var slug = await SeedFeatureAsync("By Slug Feature", description: "excerpt", releaseDate: 1_750_000_000L);

        // Act
        var feature = await _repository.GetFeatureBySlugAsync(slug, CancellationToken.None);

        // Assert
        feature.Should().NotBeNull();
        feature!.Slug.Should().Be(slug);
        feature.Title.Should().Be("By Slug Feature");
        feature.ReleaseDate.Should().Be(1_750_000_000L);
    }

    [Fact]
    public async Task GetFeatureBySlugAsync_NonExistentSlug_ReturnsNull()
    {
        // Act
        var feature = await _repository.GetFeatureBySlugAsync("slug-that-does-not-exist-xyz", CancellationToken.None);

        // Assert
        feature.Should().BeNull("non-existent slug should return null");
    }

    [Fact]
    public async Task GetAllFeaturesAsync_ReturnsTheContentLinksWithTheThumbnail()
    {
        // Arrange
        var slug = await SeedFeatureAsync("Computed Thumb");
        var normalSlug = await SeedVideoAsync("Normal item");
        var thumbSlug = await SeedVideoAsync("Thumbnail item");
        await LinkAsync(slug, normalSlug, isThumbnail: false, sortOrder: 2);
        await LinkAsync(slug, thumbSlug, isThumbnail: true, sortOrder: 1);

        // Act
        var feature = await _repository.GetFeatureBySlugAsync(slug, CancellationToken.None);

        // Assert
        feature!.ContentLinks.Select(l => l.ItemSlug).Should().Equal(thumbSlug, normalSlug);
        feature.ContentLinks.Should().OnlyContain(l => l.CollectionName == "videos");
        feature.ThumbnailLink.Should().NotBeNull("ThumbnailLink should return the link marked as thumbnail");
        feature.ThumbnailLink!.ItemSlug.Should().Be(thumbSlug);
        feature.ThumbnailLink.ItemTitle.Should().Be("Thumbnail item");
    }

    // ── vscode_update_items ───────────────────────────────────────────────────

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

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static TimelineEvent GaEvent(string product, string feature, string date, string kind = "ga") => new()
    {
        Kind = kind,
        Product = product,
        Feature = feature,
        Description = $"{kind} of {feature}",
        Date = DateOnly.Parse(date, CultureInfo.InvariantCulture),
        DatePrecision = "day",
        DateSource = "explicit",
        Quote = null
    };

    /// <summary>Adds a feature to the GitHub Copilot product and returns its slug.</summary>
    private async Task<string> SeedFeatureAsync(string title, string description = "", long? releaseDate = null)
    {
        var slug = $"ghc-test-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync(
            @"INSERT INTO product_features (product_id, name, slug, description, release_date)
              SELECT id, @Title, @Slug, @Description, @ReleaseDate FROM products WHERE lower(name) = 'github copilot'",
            new { Title = title, Slug = slug, Description = description, ReleaseDate = releaseDate });
        return slug;
    }

    private async Task LinkAsync(string featureSlug, string itemSlug, bool isThumbnail, int sortOrder) =>
        await _fixture.Connection.ExecuteAsync(
            @"INSERT INTO product_feature_content (feature_id, collection_name, item_slug, is_thumbnail, sort_order)
              SELECT id, 'videos', @ItemSlug, @IsThumbnail, @SortOrder FROM product_features WHERE slug = @FeatureSlug",
            new { FeatureSlug = featureSlug, ItemSlug = itemSlug, IsThumbnail = isThumbnail, SortOrder = sortOrder });

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

    private async Task<string> SeedNewsItemAsync()
    {
        var slug = $"ghc-event-item-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync(@"
            INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                primary_section_name, external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, 'news', 'Event item', '', '', 1743278400, 'ai', @Url, '', 'Test Feed', '', @Hash)",
            new { Slug = slug, Url = $"https://example.com/{slug}", Hash = Guid.NewGuid().ToString() });
        return slug;
    }

    private async Task<(string Slug, string Name)> CreateFeatureOfAnotherProductAsync()
    {
        var slug = $"other-product-feature-{Guid.NewGuid():N}";
        var name = $"Other product feature {slug}";
        await _fixture.Connection.ExecuteAsync(
            @"INSERT INTO product_features (product_id, name, slug)
              SELECT id, @Name, @Slug FROM products WHERE lower(name) = 'microsoft fabric'",
            new { Name = name, Slug = slug });
        return (slug, name);
    }
}
