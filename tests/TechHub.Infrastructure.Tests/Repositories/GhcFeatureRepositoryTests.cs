using System.Globalization;
using Dapper;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="GhcFeatureRepository"/>.
/// Covers the features of GitHub Copilot in the catalog (product_features, product_feature_content) and vscode_update_items.
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

    // ── UpsertFeatureAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task UpsertFeatureAsync_NewFeature_InsertsRow()
    {
        // Arrange
        var slug = $"ghc-test-insert-{Guid.NewGuid():N}";
        var feature = new GhcFeature
        {
            Slug = slug,
            Title = "Test Feature Insert",
            Description = "Short excerpt",
            ReleaseDate = 1_700_000_000L,
            GhesSupport = true
        };

        // Act
        var result = await _repository.UpsertFeatureAsync(feature, CancellationToken.None);

        // Assert
        result.Should().BeTrue("upsert of a new feature should return true");

        var row = await _fixture.Connection.QueryFirstOrDefaultAsync<dynamic>(
            "SELECT name, description, release_date, ghes_support FROM product_features WHERE slug = @Slug",
            new { Slug = slug });
        ((object?)row).Should().NotBeNull("the inserted row must exist in product_features");
        ((string)row!.name).Should().Be("Test Feature Insert");
        ((string)row.description).Should().Be("Short excerpt");
        ((long?)row.release_date).Should().Be(1_700_000_000L);
        ((bool)row.ghes_support).Should().BeTrue();
    }

    [Fact]
    public async Task UpsertFeatureAsync_ExistingFeature_UpdatesRow()
    {
        // Arrange — insert initial version
        var slug = $"ghc-test-update-{Guid.NewGuid():N}";
        var initial = new GhcFeature
        {
            Slug = slug,
            Title = "Original Title",
            Description = "Original excerpt",
            ReleaseDate = null,
            GhesSupport = false
        };
        await _repository.UpsertFeatureAsync(initial, CancellationToken.None);

        var updated = new GhcFeature
        {
            Slug = slug,
            Title = "Updated Title",
            Description = "Updated excerpt",
            ReleaseDate = 1_800_000_000L,
            GhesSupport = true
        };

        // Act
        var result = await _repository.UpsertFeatureAsync(updated, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var row = await _fixture.Connection.QueryFirstAsync<dynamic>(
            "SELECT name, description, release_date, ghes_support FROM product_features WHERE slug = @Slug",
            new { Slug = slug });
        ((string)row.name).Should().Be("Updated Title");
        ((string)row.description).Should().Be("Updated excerpt");
        ((long?)row.release_date).Should().Be(1_800_000_000L);
        ((bool)row.ghes_support).Should().BeTrue();
    }

    [Fact]
    public async Task UpsertFeatureAsync_NewFeature_IsAFeatureOfGitHubCopilot()
    {
        // Arrange
        var slug = $"ghc-test-product-{Guid.NewGuid():N}";

        // Act
        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = slug, Title = $"Product feature {slug}" }, CancellationToken.None);

        // Assert
        var product = await _fixture.Connection.QueryFirstAsync<string>(
            "SELECT p.name FROM product_features f JOIN products p ON p.id = f.product_id WHERE f.slug = @Slug",
            new { Slug = slug });
        product.Should().Be("GitHub Copilot");
    }

    [Fact]
    public async Task UpsertFeatureAsync_NewTitle_KeepsTheOldNameAsAliasSoEventLinksStay()
    {
        // Arrange
        var slug = $"ghc-test-rename-{Guid.NewGuid():N}";
        var oldTitle = $"Old name {slug}";
        var newTitle = $"New name {slug}";
        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = slug, Title = oldTitle }, CancellationToken.None);

        // Act
        var result = await _repository.UpsertFeatureAsync(new GhcFeature { Slug = slug, Title = newTitle }, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var aliases = await _fixture.Connection.QueryAsync<string>(
            "SELECT a.alias FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id WHERE f.slug = @Slug",
            new { Slug = slug });
        aliases.Should().Equal(oldTitle);
    }

    [Fact]
    public async Task UpsertFeatureAsync_TitleOfAnotherFeature_ReturnsFalseAndChangesNothing()
    {
        // Arrange
        var title = $"Taken title {Guid.NewGuid():N}";
        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = $"ghc-test-taken-{Guid.NewGuid():N}", Title = title }, CancellationToken.None);
        var slug = $"ghc-test-other-{Guid.NewGuid():N}";

        // Act
        var result = await _repository.UpsertFeatureAsync(new GhcFeature { Slug = slug, Title = title.ToUpperInvariant() }, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        (await _repository.GetFeatureBySlugAsync(slug, CancellationToken.None)).Should().BeNull();
    }

    [Fact]
    public async Task UpsertFeatureAsync_SlugOfAFeatureOfAnotherProduct_ReturnsFalse()
    {
        // Arrange
        var other = await CreateFeatureOfAnotherProductAsync();

        // Act
        var result = await _repository.UpsertFeatureAsync(new GhcFeature { Slug = other.Slug, Title = $"Hijack {other.Slug}" }, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        var name = await _fixture.Connection.QueryFirstAsync<string>("SELECT name FROM product_features WHERE slug = @Slug", new { other.Slug });
        name.Should().Be(other.Name);
    }

    [Fact]
    public async Task UpsertFeatureAsync_EmptyTitle_ReturnsFalse()
    {
        // Act
        var result = await _repository.UpsertFeatureAsync(new GhcFeature { Slug = $"ghc-test-empty-{Guid.NewGuid():N}", Title = "   " }, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpsertFeatureAsync_LinksTheTimelineEventsThatUseTheTitle()
    {
        // Arrange
        var title = $"Linked feature {Guid.NewGuid():N}";
        var itemSlug = await SeedNewsItemAsync();
        var timeline = new TimelineEventRepository(_fixture.Connection);
        await timeline.ReplaceEventsAsync("news", itemSlug, [GaEvent("GitHub Copilot", title, "2026-03-10")], "source", 3, CancellationToken.None);

        // Act
        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = $"ghc-test-linked-{Guid.NewGuid():N}", Title = title }, CancellationToken.None);

        // Assert
        var linked = await _fixture.Connection.QueryFirstAsync<bool>(
            "SELECT feature_id IS NOT NULL FROM content_timeline_events WHERE collection_name = 'news' AND item_slug = @Slug",
            new { Slug = itemSlug });
        linked.Should().BeTrue();
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
        var slug = $"ghc-test-dated-{Guid.NewGuid():N}";
        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = slug, Title = title }, CancellationToken.None);
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
        var slug = $"ghc-test-admin-date-{Guid.NewGuid():N}";
        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = slug, Title = title, ReleaseDate = 1_700_000_000L }, CancellationToken.None);
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
        var features = new ProductFeatureRepository(_fixture.Connection, Microsoft.Extensions.Logging.Abstractions.NullLogger<ProductFeatureRepository>.Instance);
        var name = $"Catalog feature {Guid.NewGuid():N}";

        // Act
        var created = await features.CreateAsync(productId, name, CancellationToken.None);

        // Assert
        created.Succeeded.Should().BeTrue(created.Message);
        var all = await _repository.GetAllFeaturesAsync(CancellationToken.None);
        all.Should().ContainSingle(f => f.Title == name).Which.Slug.Should().StartWith("catalog-feature-");
    }

    // ── GetAllFeaturesAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetAllFeaturesAsync_ReturnsInsertedFeature()
    {
        // Arrange
        var slug = $"ghc-test-getall-{Guid.NewGuid():N}";
        var feature = new GhcFeature
        {
            Slug = slug,
            Title = "Get All Test",
            Description = "Excerpt for get all",
            ReleaseDate = 1_700_000_000L,
            GhesSupport = false
        };
        await _repository.UpsertFeatureAsync(feature, CancellationToken.None);

        // Act
        var all = await _repository.GetAllFeaturesAsync(CancellationToken.None);

        // Assert
        all.Should().NotBeEmpty();
        var found = all.FirstOrDefault(f => f.Slug == slug);
        found.Should().NotBeNull("the newly inserted feature must be returned by GetAllFeaturesAsync");
        found!.Title.Should().Be("Get All Test");
        found.GhesSupport.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllFeaturesAsync_OrderedByReleaseDateDescending()
    {
        // Arrange — insert two features with different release dates
        var slugOlder = $"ghc-test-order-older-{Guid.NewGuid():N}";
        var slugNewer = $"ghc-test-order-newer-{Guid.NewGuid():N}";

        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = slugOlder, Title = "Older", Description = "", GhesSupport = false, ReleaseDate = 1_600_000_000L }, CancellationToken.None);
        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = slugNewer, Title = "Newer", Description = "", GhesSupport = false, ReleaseDate = 1_900_000_000L }, CancellationToken.None);

        // Act
        var all = await _repository.GetAllFeaturesAsync(CancellationToken.None);

        // Assert — newer feature must appear before older feature
        var newerIndex = all.ToList().FindIndex(f => f.Slug == slugNewer);
        var olderIndex = all.ToList().FindIndex(f => f.Slug == slugOlder);
        newerIndex.Should().BeLessThan(olderIndex, "features should be ordered by release_date DESC");
    }

    // ── GetFeatureBySlugAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetFeatureBySlugAsync_ExistingSlug_ReturnsFeature()
    {
        // Arrange
        var slug = $"ghc-test-byslug-{Guid.NewGuid():N}";
        await _repository.UpsertFeatureAsync(new GhcFeature
        {
            Slug = slug,
            Title = "By Slug Feature",
            Description = "excerpt",
            GhesSupport = true,
            ReleaseDate = 1_750_000_000L
        }, CancellationToken.None);

        // Act
        var feature = await _repository.GetFeatureBySlugAsync(slug, CancellationToken.None);

        // Assert
        feature.Should().NotBeNull();
        feature!.Slug.Should().Be(slug);
        feature.Title.Should().Be("By Slug Feature");
        feature.GhesSupport.Should().BeTrue();
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

    // ── DeleteFeatureAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteFeatureAsync_ExistingFeature_RemovesRow()
    {
        // Arrange
        var slug = $"ghc-test-delete-{Guid.NewGuid():N}";
        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = slug, Title = "To Delete", Description = "", GhesSupport = false }, CancellationToken.None);

        // Act
        var deleted = await _repository.DeleteFeatureAsync(slug, CancellationToken.None);

        // Assert
        deleted.Should().BeTrue();
        var exists = await _fixture.Connection.QueryFirstAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM product_features WHERE slug = @Slug)", new { Slug = slug });
        exists.Should().BeFalse("deleted feature should no longer exist in product_features");
    }

    [Fact]
    public async Task DeleteFeatureAsync_NonExistentSlug_ReturnsFalse()
    {
        // Act
        var deleted = await _repository.DeleteFeatureAsync("slug-that-does-not-exist-xyz", CancellationToken.None);

        // Assert
        deleted.Should().BeFalse("deleting a non-existent feature should return false");
    }

    // ── Content links ─────────────────────────────────────────────────────────

    [Fact]
    public async Task AddContentLinkAsync_CreatesLinkAndAppearsInFeature()
    {
        // Arrange — feature + a seeded content item
        var featureSlug = $"ghc-test-link-{Guid.NewGuid():N}";
        var itemSlug = $"ghc-link-item-{Guid.NewGuid():N}";

        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = featureSlug, Title = "Link Feature", Description = "", GhesSupport = false }, CancellationToken.None);

        // Insert a minimal content item so the FK is satisfied
        await _fixture.Connection.ExecuteAsync(@"
            INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                primary_section_name, external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, 'videos', 'Link Item', '', '', 0, 'ai', @Url, '', 'Test Feed', '', @Hash)
            ON CONFLICT DO NOTHING",
            new { Slug = itemSlug, Url = $"https://example.com/{itemSlug}", Hash = Guid.NewGuid().ToString() });

        // Act
        var added = await _repository.AddContentLinkAsync(featureSlug, "videos", itemSlug, isThumbnail: false, sortOrder: 1, CancellationToken.None);

        // Assert
        added.Should().BeTrue();
        var feature = await _repository.GetFeatureBySlugAsync(featureSlug, CancellationToken.None);
        feature.Should().NotBeNull();
        feature!.ContentLinks.Should().ContainSingle(l => l.ItemSlug == itemSlug && l.CollectionName == "videos");
    }

    [Fact]
    public async Task AddContentLinkAsync_Thumbnail_ClearsPreviousThumbnail()
    {
        // Arrange — feature + two content items
        var featureSlug = $"ghc-test-thumb-{Guid.NewGuid():N}";
        var item1Slug = $"ghc-thumb-item1-{Guid.NewGuid():N}";
        var item2Slug = $"ghc-thumb-item2-{Guid.NewGuid():N}";

        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = featureSlug, Title = "Thumb Feature", Description = "", GhesSupport = false }, CancellationToken.None);

        foreach (var itemSlug in new[] { item1Slug, item2Slug })
        {
            await _fixture.Connection.ExecuteAsync(@"
                INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                    primary_section_name, external_url, author, feed_name, tags_csv, content_hash)
                VALUES (@Slug, 'videos', 'Thumb Item', '', '', 0, 'ai', @Url, '', 'Test Feed', '', @Hash)
                ON CONFLICT DO NOTHING",
                new { Slug = itemSlug, Url = $"https://example.com/{itemSlug}", Hash = Guid.NewGuid().ToString() });
        }

        // Add first as thumbnail
        await _repository.AddContentLinkAsync(featureSlug, "videos", item1Slug, isThumbnail: true, sortOrder: 1, CancellationToken.None);

        // Act — add second as thumbnail (should clear first)
        await _repository.AddContentLinkAsync(featureSlug, "videos", item2Slug, isThumbnail: true, sortOrder: 2, CancellationToken.None);

        // Assert — only item2 should be thumbnail now
        var feature = await _repository.GetFeatureBySlugAsync(featureSlug, CancellationToken.None);
        feature.Should().NotBeNull();
        var thumb1 = feature!.ContentLinks.FirstOrDefault(l => l.ItemSlug == item1Slug);
        var thumb2 = feature.ContentLinks.FirstOrDefault(l => l.ItemSlug == item2Slug);
        thumb1.Should().NotBeNull();
        thumb1!.IsThumbnail.Should().BeFalse("first thumbnail should be cleared when a new one is set");
        thumb2.Should().NotBeNull();
        thumb2!.IsThumbnail.Should().BeTrue("second item should now be the thumbnail");
    }

    [Fact]
    public async Task RemoveContentLinkAsync_ExistingLink_RemovesIt()
    {
        // Arrange
        var featureSlug = $"ghc-test-rmlink-{Guid.NewGuid():N}";
        var itemSlug = $"ghc-rmlink-item-{Guid.NewGuid():N}";

        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = featureSlug, Title = "Remove Link Feature", Description = "", GhesSupport = false }, CancellationToken.None);

        await _fixture.Connection.ExecuteAsync(@"
            INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                primary_section_name, external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, 'videos', 'Remove Link Item', '', '', 0, 'ai', @Url, '', 'Test Feed', '', @Hash)
            ON CONFLICT DO NOTHING",
            new { Slug = itemSlug, Url = $"https://example.com/{itemSlug}", Hash = Guid.NewGuid().ToString() });

        await _repository.AddContentLinkAsync(featureSlug, "videos", itemSlug, isThumbnail: false, sortOrder: 1, CancellationToken.None);

        // Act
        var removed = await _repository.RemoveContentLinkAsync(featureSlug, "videos", itemSlug, CancellationToken.None);

        // Assert
        removed.Should().BeTrue();
        var feature = await _repository.GetFeatureBySlugAsync(featureSlug, CancellationToken.None);
        feature!.ContentLinks.Should().NotContain(l => l.ItemSlug == itemSlug, "removed link should not appear in content links");
    }

    // ── vscode_update_items ───────────────────────────────────────────────────

    [Fact]
    public async Task AddVscodeUpdateItemAsync_InsertsRow()
    {
        // Arrange — need a content item so FK is satisfied
        var slug = $"vscode-update-test-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync(@"
            INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                primary_section_name, external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, 'videos', 'VsCode Update Item', '', '', 0, 'ai', @Url, '', 'Test Feed', '', @Hash)
            ON CONFLICT DO NOTHING",
            new { Slug = slug, Url = $"https://example.com/{slug}", Hash = Guid.NewGuid().ToString() });

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
        var slug = $"vscode-idempotent-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync(@"
            INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                primary_section_name, external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, 'videos', 'Idempotent Item', '', '', 0, 'ai', @Url, '', 'Test Feed', '', @Hash)
            ON CONFLICT DO NOTHING",
            new { Slug = slug, Url = $"https://example.com/{slug}", Hash = Guid.NewGuid().ToString() });

        // Act — idempotent: calling twice should not throw
        await _repository.AddVscodeUpdateItemAsync("videos", slug, CancellationToken.None);
        var act = () => _repository.AddVscodeUpdateItemAsync("videos", slug, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync("ON CONFLICT DO NOTHING should make the call idempotent");
    }

    // ── ThumbnailLink computed property ───────────────────────────────────────

    [Fact]
    public async Task GhcFeature_ThumbnailLink_ReturnsMarkedThumbnail()
    {
        // Arrange
        var featureSlug = $"ghc-test-computed-thumb-{Guid.NewGuid():N}";
        var normalSlug = $"ghc-normal-item-{Guid.NewGuid():N}";
        var thumbSlug = $"ghc-thumb-item-{Guid.NewGuid():N}";

        await _repository.UpsertFeatureAsync(new GhcFeature { Slug = featureSlug, Title = "Computed Thumb", Description = "", GhesSupport = false }, CancellationToken.None);

        foreach (var (itemSlug, isThumbnail, sortOrder) in new[] { (normalSlug, false, 2), (thumbSlug, true, 1) })
        {
            await _fixture.Connection.ExecuteAsync(@"
                INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                    primary_section_name, external_url, author, feed_name, tags_csv, content_hash)
                VALUES (@Slug, 'videos', 'Item', '', '', 0, 'ai', @Url, '', 'Test Feed', '', @Hash)
                ON CONFLICT DO NOTHING",
                new { Slug = itemSlug, Url = $"https://example.com/{itemSlug}", Hash = Guid.NewGuid().ToString() });

            await _repository.AddContentLinkAsync(featureSlug, "videos", itemSlug, isThumbnail, sortOrder, CancellationToken.None);
        }

        // Act
        var feature = await _repository.GetFeatureBySlugAsync(featureSlug, CancellationToken.None);

        // Assert
        feature.Should().NotBeNull();
        feature!.ThumbnailLink.Should().NotBeNull("ThumbnailLink should return the link marked as thumbnail");
        feature.ThumbnailLink!.ItemSlug.Should().Be(thumbSlug);
        feature.ThumbnailLink.IsThumbnail.Should().BeTrue();
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
