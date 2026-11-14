using System.Globalization;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="GhcFeatureRepository"/>: the vscode_update_items table and the features of the catalog
/// that have content (the features page). The curation of features is tested through <see cref="ProductFeatureRepository"/>
/// (see ProductFeatureTests). Each test uses unique slugs and product names to avoid cross-test contamination in the shared database.
/// </summary>
public class GhcFeatureRepositoryTests : IClassFixture<DatabaseFixture<GhcFeatureRepositoryTests>>
{
    private readonly DatabaseFixture<GhcFeatureRepositoryTests> _fixture;
    private readonly GhcFeatureRepository _repository;
    private readonly ProductRepository _products;
    private readonly ProductFeatureRepository _features;
    private readonly TimelineEventRepository _timeline;

    public GhcFeatureRepositoryTests(DatabaseFixture<GhcFeatureRepositoryTests> fixture)
    {
        _fixture = fixture;
        _repository = new GhcFeatureRepository(fixture.Connection);
        _products = new ProductRepository(fixture.Connection, NullLogger<ProductRepository>.Instance);
        _features = new ProductFeatureRepository(fixture.Connection, NullLogger<ProductFeatureRepository>.Instance);
        _timeline = new TimelineEventRepository(fixture.Connection);
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

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

    [Fact]
    public async Task GetFeaturesWithContentAsync_ReturnsOnlyFeaturesWithContent_ThumbnailFirst()
    {
        var product = await CreateProductAsync();
        var withVideo = await CreateFeatureAsync(product.Id, "Agent mode", "Works on its own.", new DateOnly(2025, 2, 6));
        await CreateFeatureAsync(product.Id, "Feature without content");
        var article = await SeedItemAsync("news", "Agent mode article", "https://example.com/agent-mode");
        var video = await SeedItemAsync("videos", "Agent mode video", "https://youtu.be/abc123test1");
        (await _features.AddContentAsync(product.Id, withVideo, "news", article, false, Ct)).Succeeded.Should().BeTrue();
        (await _features.AddContentAsync(product.Id, withVideo, "videos", video, true, Ct)).Succeeded.Should().BeTrue();

        var result = await _repository.GetFeaturesWithContentAsync(product.Name, Ct);

        var feature = result.Should().ContainSingle().Subject;
        feature.Name.Should().Be("Agent mode");
        feature.ProductName.Should().Be(product.Name);
        feature.Description.Should().Be("Works on its own.");
        feature.ReleaseDate.Should().Be(new DateOnly(2025, 2, 6));
        feature.Content.Select(c => c.ItemSlug).Should().Equal(video, article);
        feature.Content[0].IsThumbnail.Should().BeTrue();
        feature.Content[0].ExternalUrl.Should().StartWith("https://youtu.be/abc123test1");
        feature.Content.Should().OnlyContain(c => !c.IsDetected, "an admin linked both items");
    }

    [Fact]
    public async Task GetFeaturesWithContentAsync_FeatureNamedByADatedEvent_HasThatArticleAsDetectedContent()
    {
        var product = await CreateProductAsync();
        await CreateFeatureAsync(product.Id, "Agent mode");
        var slug = await SeedItemAsync("news", "Agent mode is GA", "https://example.com/ga");
        await _timeline.ReplaceEventsAsync("news", slug, [Event(product.Name, "Agent mode", "ga")], "source", 3, Ct);

        var result = await _repository.GetFeaturesWithContentAsync(product.Name, Ct);

        var content = result.Should().ContainSingle().Subject.Content;
        content.Should().ContainSingle(c => c.ItemSlug == slug && c.IsDetected);
    }

    [Fact]
    public async Task GetFeaturesWithContentAsync_FeatureListedByAnArticleWithoutADate_HasThatArticleAsContent()
    {
        var product = await CreateProductAsync();
        await CreateFeatureAsync(product.Id, "Plan mode");
        var slug = await SeedItemAsync("news", "Plan mode in practice", "https://example.com/plan");
        await _fixture.Connection.ExecuteAsync(
            """
            UPDATE content_items
            SET ai_metadata = jsonb_build_object(
                'product_features', jsonb_build_array(jsonb_build_object('product', @Product, 'feature', 'plan  MODE')))
            WHERE collection_name = 'news' AND slug = @Slug
            """,
            new { Product = product.Name, Slug = slug });

        var result = await _repository.GetFeaturesWithContentAsync(product.Name, Ct);

        result.Should().ContainSingle().Which.Content.Should().ContainSingle().Which.ItemSlug.Should().Be(slug);
    }

    [Fact]
    public async Task GetFeaturesWithContentAsync_ItemThatIsLinkedAndDetected_IsListedOnceAsCurated()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Agent mode");
        var slug = await SeedItemAsync("news", "Agent mode is GA", "https://example.com/ga");
        (await _features.AddContentAsync(product.Id, feature, "news", slug, false, Ct)).Succeeded.Should().BeTrue();
        await _timeline.ReplaceEventsAsync("news", slug, [Event(product.Name, "Agent mode", "ga")], "source", 3, Ct);

        var result = await _repository.GetFeaturesWithContentAsync(product.Name, Ct);

        result.Should().ContainSingle().Which.Content.Should().ContainSingle().Which.IsDetected.Should().BeFalse();
    }

    [Fact]
    public async Task GetFeaturesWithContentAsync_RoundupsNeverCountAsContent()
    {
        var product = await CreateProductAsync();
        await CreateFeatureAsync(product.Id, "Agent mode");
        var slug = await SeedItemAsync("roundups", "Weekly roundup", "https://example.com/roundup");
        await _timeline.ReplaceEventsAsync("roundups", slug, [Event(product.Name, "Agent mode", "ga")], "source", 3, Ct);

        var result = await _repository.GetFeaturesWithContentAsync(product.Name, Ct);

        result.Should().BeEmpty("a roundup states dates about everything");
    }

    [Fact]
    public async Task GetFeaturesWithContentAsync_IncludesChildProductsAndNotUnrelatedOnes()
    {
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync(parent.Id);
        var unrelated = await CreateProductAsync();
        foreach (var (productId, featureName) in new[] { (parent.Id, "Parent feature"), (child.Id, "Child feature"), (unrelated.Id, "Unrelated feature") })
        {
            var feature = await CreateFeatureAsync(productId, featureName);
            var slug = await SeedItemAsync("videos", featureName, $"https://example.com/{Guid.NewGuid():N}");
            (await _features.AddContentAsync(productId, feature, "videos", slug, false, Ct)).Succeeded.Should().BeTrue();
        }

        var result = await _repository.GetFeaturesWithContentAsync(parent.Name, Ct);

        result.Select(f => (f.Name, f.ProductName)).Should().BeEquivalentTo(
            new[] { ("Parent feature", parent.Name), ("Child feature", child.Name) });
    }

    [Fact]
    public async Task GetFeaturesWithContentAsync_OrdersNewestReleaseFirstAndUndatedLast()
    {
        var product = await CreateProductAsync();
        foreach (var (name, date) in new (string, DateOnly?)[] { ("Old", new DateOnly(2024, 1, 1)), ("Undated", null), ("New", new DateOnly(2025, 6, 1)) })
        {
            var feature = await CreateFeatureAsync(product.Id, name, releaseDate: date);
            var slug = await SeedItemAsync("news", name, $"https://example.com/{Guid.NewGuid():N}");
            (await _features.AddContentAsync(product.Id, feature, "news", slug, false, Ct)).Succeeded.Should().BeTrue();
        }

        var result = await _repository.GetFeaturesWithContentAsync(product.Name, Ct);

        result.Select(f => f.Name).Should().Equal("New", "Old", "Undated");
    }

    [Fact]
    public async Task GetFeaturesWithContentAsync_NameThatIsNoProduct_ReturnsNothing()
    {
        var result = await _repository.GetFeaturesWithContentAsync($"No product {Guid.NewGuid():N}", Ct);

        result.Should().BeEmpty();
    }

    private async Task<(long Id, string Name)> CreateProductAsync(long? parentId = null)
    {
        var name = $"Product-{Guid.NewGuid():N}";
        var created = await _products.CreateAsync(name, Ct);
        created.Succeeded.Should().BeTrue(created.Message);
        var id = created.ProductId!.Value;
        if (parentId is { } parent)
        {
            (await _products.SetParentAsync(id, parent, Ct)).Succeeded.Should().BeTrue();
        }

        return (id, name);
    }

    private async Task<long> CreateFeatureAsync(long productId, string name, string? description = null, DateOnly? releaseDate = null)
    {
        var created = await _features.CreateAsync(productId, name, Ct);
        created.Succeeded.Should().BeTrue(created.Message);
        var id = created.ProductId!.Value;
        if (description is not null || releaseDate is not null)
        {
            (await _features.UpdateDetailsAsync(productId, id, description, releaseDate, Ct)).Succeeded.Should().BeTrue();
        }

        return id;
    }

    // The external url is unique per item, so every seeded item gets its own query string.
    private async Task<string> SeedItemAsync(string collection, string title, string url)
    {
        var slug = $"ghc-item-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                primary_section_name, external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, @Collection, @Title, '', '', 1743278400, 'github-copilot', @Url, '', 'Test Feed', '', @Hash)
            """,
            new { Slug = slug, Collection = collection, Title = title, Url = $"{url}?u={slug}", Hash = Guid.NewGuid().ToString() });
        return slug;
    }

    private static TimelineEvent Event(string product, string? feature, string kind) => new()
    {
        Kind = kind,
        Product = product,
        Feature = feature,
        Description = $"{kind} of {feature ?? product}",
        Date = DateOnly.Parse("2026-03-10", CultureInfo.InvariantCulture),
        DatePrecision = "day",
        DateSource = "explicit",
        Quote = null
    };

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
