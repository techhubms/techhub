using System.Globalization;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for the curated features of a product (<see cref="ProductFeatureRepository"/>) and for the feature
/// that timeline events link to.
/// </summary>
public class ProductFeatureTests : IClassFixture<DatabaseFixture<ProductFeatureTests>>
{
    private readonly DatabaseFixture<ProductFeatureTests> _fixture;
    private readonly ProductFeatureRepository _sut;
    private readonly ProductRepository _products;
    private readonly TimelineEventRepository _timelineRepository;

    public ProductFeatureTests(DatabaseFixture<ProductFeatureTests> fixture)
    {
        _fixture = fixture;
        _sut = new ProductFeatureRepository(fixture.Connection, NullLogger<ProductFeatureRepository>.Instance);
        _products = new ProductRepository(fixture.Connection, NullLogger<ProductRepository>.Instance);
        _timelineRepository = new TimelineEventRepository(fixture.Connection);
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    // ── Events link to the feature ────────────────────────────────────────────

    [Fact]
    public async Task ReplaceEvents_LinksTheFeatureByNameOrAliasIgnoringCaseAndWhitespace()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Agent mode");
        (await _sut.AddAliasAsync(product.Id, feature, "autonomous mode", Ct)).Succeeded.Should().BeTrue();
        var slug = await SeedItemAsync();

        await _timelineRepository.ReplaceEventsAsync("news", slug,
            [Event(product.Name, "AGENT  mode", "ga"), Event(product.Name, " Autonomous Mode ", "preview"), Event(product.Name, "Other", "launch")], "source", 3, Ct);

        var links = await GetEventFeaturesAsync(slug);
        links.Should().Equal(("AGENT  mode", feature), (" Autonomous Mode ", feature), ("Other", (long?)null));
    }

    [Fact]
    public async Task ReplaceEvents_WithoutFeature_HasNoFeatureLink()
    {
        var product = await CreateProductAsync();
        await CreateFeatureAsync(product.Id, "Agent mode");
        var slug = await SeedItemAsync();

        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(product.Name, null, "ga")], "source", 3, Ct);

        (await GetEventFeaturesAsync(slug)).Should().ContainSingle().Which.FeatureId.Should().BeNull();
    }

    [Fact]
    public async Task ReplaceEvents_OfAChildProduct_MatchesTheFeaturesOfItsParents()
    {
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync();
        (await _products.SetParentAsync(child.Id, parent.Id, Ct)).Succeeded.Should().BeTrue();
        var parentFeature = await CreateFeatureAsync(parent.Id, "Plan mode");
        var childFeature = await CreateFeatureAsync(child.Id, "Plan mode");
        var slug = await SeedItemAsync();
        var otherSlug = await SeedItemAsync();

        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(child.Name, "plan mode", "ga")], "source", 3, Ct);
        await _timelineRepository.ReplaceEventsAsync("news", otherSlug, [Event(parent.Name, "plan mode", "ga")], "source", 3, Ct);

        (await GetEventFeaturesAsync(slug)).Single().FeatureId.Should().Be(childFeature);
        (await GetEventFeaturesAsync(otherSlug)).Single().FeatureId.Should().Be(parentFeature);
    }

    [Fact]
    public async Task ReplaceEvents_OfAChildProduct_FallsBackToAFeatureOfTheParent()
    {
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync();
        (await _products.SetParentAsync(child.Id, parent.Id, Ct)).Succeeded.Should().BeTrue();
        var parentFeature = await CreateFeatureAsync(parent.Id, "Custom instructions");
        var slug = await SeedItemAsync();

        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(child.Name, "Custom instructions", "ga")], "source", 3, Ct);

        (await GetEventFeaturesAsync(slug)).Single().FeatureId.Should().Be(parentFeature);
    }

    [Fact]
    public async Task GetDetail_ShowsTheCuratedFeatureNameOfEvents()
    {
        var product = await CreateProductAsync();
        await CreateFeatureAsync(product.Id, "Agent mode");
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(product.Name, "agent-mode-text", "ga"), Event(product.Name, "agent MODE", "preview")], "source", 3, Ct);

        var detail = (await _products.GetDetailAsync(product.Id, Ct))!;

        detail.Events.Should().Contain(e => e.Feature == "agent MODE" && e.FeatureName == "Agent mode");
        detail.Events.Should().Contain(e => e.Feature == "agent-mode-text" && e.FeatureName == null);
    }

    // ── Catalog changes relink ────────────────────────────────────────────────

    [Fact]
    public async Task Create_LinksTheEventsThatUseTheName_AndLeavesTheReviewQueue()
    {
        var product = await CreateProductAsync();
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(product.Name, "Coding agent", "ga"), Event(product.Name, "coding  agent", "preview")], "source", 3, Ct);
        var before = await _sut.GetOverviewAsync(product.Id, Ct);
        before.Unmatched.Should().ContainSingle().Which.EventCount.Should().Be(2);

        var feature = await CreateFeatureAsync(product.Id, "Coding agent");

        (await GetEventFeaturesAsync(slug)).Select(l => l.FeatureId).Should().OnlyContain(id => id == feature);
        var after = await _sut.GetOverviewAsync(product.Id, Ct);
        after.Unmatched.Should().BeEmpty();
        after.Features.Should().ContainSingle().Which.EventCount.Should().Be(2);
    }

    [Fact]
    public async Task GetOverview_UnmatchedNamesIncludeTheEventsOfChildProducts_MostUsedFirst()
    {
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync();
        (await _products.SetParentAsync(child.Id, parent.Id, Ct)).Succeeded.Should().BeTrue();
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug,
            [Event(child.Name, "Rare", "ga"), Event(child.Name, "Common", "ga"), Event(parent.Name, "common", "preview")], "source", 3, Ct);

        var overview = await _sut.GetOverviewAsync(parent.Id, Ct);

        overview.Unmatched.Select(u => (Name: u.Name.ToLowerInvariant(), u.EventCount)).Should().Equal(("common", 2), ("rare", 1));
    }

    [Fact]
    public async Task GetUnmatchedSummary_ListsTheProductsWithUnmatchedFeatureNames_MostEventsFirst()
    {
        var busy = await CreateProductAsync();
        var quiet = await CreateProductAsync();
        var done = await CreateProductAsync();
        await CreateFeatureAsync(done.Id, "Matched");
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug,
            [
                Event(busy.Name, "Alpha", "ga"), Event(busy.Name, "alpha", "preview"), Event(busy.Name, "Beta", "ga"),
                Event(quiet.Name, "Gamma", "ga"),
                Event(done.Name, "matched", "ga"), Event(done.Name, null, "launch")
            ], "source", 3, Ct);

        var summary = await _sut.GetUnmatchedSummaryAsync(Ct);

        var ours = summary.Where(s => s.ProductId == busy.Id || s.ProductId == quiet.Id || s.ProductId == done.Id).ToList();
        ours.Select(s => (s.ProductId, s.ProductName, s.NameCount, s.EventCount)).Should().Equal(
            (busy.Id, busy.Name, 2, 3),
            (quiet.Id, quiet.Name, 1, 1));
    }
    [Fact]
    public async Task AddAlias_LinksTheEventsThatUseTheAlias_AndRemoveAliasUnlinksThem()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Agent mode");
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(product.Name, "Autopilot", "ga")], "source", 3, Ct);

        (await _sut.AddAliasAsync(product.Id, feature, "autopilot", Ct)).Succeeded.Should().BeTrue();

        (await GetEventFeaturesAsync(slug)).Single().FeatureId.Should().Be(feature);
        var aliasId = (await _sut.GetOverviewAsync(product.Id, Ct)).Features.Single().Aliases.Single().Id;

        (await _sut.RemoveAliasAsync(product.Id, feature, aliasId, Ct)).Succeeded.Should().BeTrue();

        (await GetEventFeaturesAsync(slug)).Single().FeatureId.Should().BeNull();
    }

    [Fact]
    public async Task Rename_KeepsTheOldNameAsAliasSoLinksStay()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Old name");
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(product.Name, "Old name", "ga")], "source", 3, Ct);

        var result = await _sut.RenameAsync(product.Id, feature, "New name", Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        var renamed = (await _sut.GetOverviewAsync(product.Id, Ct)).Features.Single();
        renamed.Name.Should().Be("New name");
        renamed.Aliases.Select(a => a.Alias).Should().Equal("Old name");
        (await GetEventFeaturesAsync(slug)).Single().FeatureId.Should().Be(feature);
    }

    [Fact]
    public async Task Delete_UnlinksTheEventsButKeepsTheirFeatureText()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Doomed");
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(product.Name, "Doomed", "ga")], "source", 3, Ct);

        (await _sut.DeleteAsync(product.Id, feature, Ct)).Succeeded.Should().BeTrue();

        (await GetEventFeaturesAsync(slug)).Should().Equal(("Doomed", (long?)null));
        (await _sut.GetOverviewAsync(product.Id, Ct)).Unmatched.Should().ContainSingle(u => u.Name == "Doomed");
    }

    [Fact]
    public async Task DeleteProduct_RemovesItsFeatures()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Gone");

        (await _products.DeleteAsync(product.Id, Ct)).Succeeded.Should().BeTrue();

        (await _fixture.Connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM product_features WHERE id = @Id", new { Id = feature })).Should().Be(0);
    }

    // ── Names that are not allowed ────────────────────────────────────────────

    [Fact]
    public async Task Create_WithTheNameOrAliasOfAFeatureOfTheSameProduct_IsAConflict()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Agent mode");
        (await _sut.AddAliasAsync(product.Id, feature, "Autopilot", Ct)).Succeeded.Should().BeTrue();

        (await _sut.CreateAsync(product.Id, "  AGENT MODE ", Ct)).Status.Should().Be(ProductOperationStatus.Conflict);
        (await _sut.CreateAsync(product.Id, "autopilot", Ct)).Status.Should().Be(ProductOperationStatus.Conflict);
    }

    [Fact]
    public async Task Create_TheSameNameForAnotherProduct_IsAllowed()
    {
        var first = await CreateProductAsync();
        var second = await CreateProductAsync();
        await CreateFeatureAsync(first.Id, "Agent mode");

        (await _sut.CreateAsync(second.Id, "Agent mode", Ct)).Succeeded.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Create_WithEmptyName_IsInvalid(string name)
    {
        var product = await CreateProductAsync();

        (await _sut.CreateAsync(product.Id, name, Ct)).Status.Should().Be(ProductOperationStatus.Invalid);
    }

    [Fact]
    public async Task Create_ForUnknownProduct_IsNotFound()
    {
        (await _sut.CreateAsync(long.MaxValue, "Feature", Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
    }

    [Fact]
    public async Task Rename_ToTheNameOfAnotherFeatureOfTheProduct_IsAConflict()
    {
        var product = await CreateProductAsync();
        await CreateFeatureAsync(product.Id, "First");
        var second = await CreateFeatureAsync(product.Id, "Second");

        (await _sut.RenameAsync(product.Id, second, "first", Ct)).Status.Should().Be(ProductOperationStatus.Conflict);
    }

    [Fact]
    public async Task Operations_OnAFeatureOfAnotherProduct_AreNotFound()
    {
        var owner = await CreateProductAsync();
        var other = await CreateProductAsync();
        var feature = await CreateFeatureAsync(owner.Id, "Agent mode");
        (await _sut.AddAliasAsync(owner.Id, feature, "Autopilot", Ct)).Succeeded.Should().BeTrue();
        var aliasId = (await _sut.GetOverviewAsync(owner.Id, Ct)).Features.Single().Aliases.Single().Id;

        (await _sut.RenameAsync(other.Id, feature, "Renamed", Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.DeleteAsync(other.Id, feature, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.AddAliasAsync(other.Id, feature, "Alias", Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.RemoveAliasAsync(other.Id, feature, aliasId, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.GetOverviewAsync(owner.Id, Ct)).Features.Should().ContainSingle().Which.Name.Should().Be("Agent mode");
    }

    // ── Details and content ───────────────────────────────────────────────────

    [Fact]
    public async Task UpdateDetails_SetsTheDescriptionAndTheReleaseDate_AndNullClearsTheDate()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Agent mode");

        var saved = await _sut.UpdateDetailsAsync(product.Id, feature, "  Works on its own.  ", new DateOnly(2026, 3, 10), Ct);

        saved.Succeeded.Should().BeTrue(saved.Message);
        var stored = (await _sut.GetOverviewAsync(product.Id, Ct)).Features.Single();
        stored.Description.Should().Be("Works on its own.");
        stored.ReleaseDate.Should().Be(new DateOnly(2026, 3, 10));
        (await _fixture.Connection.ExecuteScalarAsync<long>("SELECT release_date FROM product_features WHERE id = @Id", new { Id = feature }))
            .Should().Be(new DateTimeOffset(2026, 3, 10, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds(), "the date is stored as midnight UTC");

        (await _sut.UpdateDetailsAsync(product.Id, feature, null, null, Ct)).Succeeded.Should().BeTrue();

        var cleared = (await _sut.GetOverviewAsync(product.Id, Ct)).Features.Single();
        cleared.Description.Should().BeEmpty();
        cleared.ReleaseDate.Should().BeNull();
    }

    [Fact]
    public async Task UpdateDetails_WithATooLongDescription_IsInvalidAndChangesNothing()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Agent mode");
        (await _sut.UpdateDetailsAsync(product.Id, feature, "Short", null, Ct)).Succeeded.Should().BeTrue();

        var result = await _sut.UpdateDetailsAsync(product.Id, feature, new string('x', ProductFeatureRepository.MaxDescriptionLength + 1), null, Ct);

        result.Status.Should().Be(ProductOperationStatus.Invalid);
        (await _sut.GetOverviewAsync(product.Id, Ct)).Features.Single().Description.Should().Be("Short");
    }

    [Fact]
    public async Task AddContent_LinksAfterTheExistingLinks_AndTheOverviewShowsTheTitles()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Agent mode");
        var first = await SeedVideoAsync("First video");
        var second = await SeedVideoAsync("Second video");

        (await _sut.AddContentAsync(product.Id, feature, "videos", first, false, Ct)).Succeeded.Should().BeTrue();
        (await _sut.AddContentAsync(product.Id, feature, "videos", second, false, Ct)).Succeeded.Should().BeTrue();

        var content = (await _sut.GetOverviewAsync(product.Id, Ct)).Features.Single().Content;
        content.Select(c => (c.ItemSlug, c.Title, c.IsThumbnail)).Should().Equal((first, "First video", false), (second, "Second video", false));
        content.Select(c => c.SortOrder).Should().BeInAscendingOrder().And.OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task AddContent_AsThumbnail_ReplacesThePreviousThumbnail()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Agent mode");
        var first = await SeedVideoAsync("First video");
        var second = await SeedVideoAsync("Second video");
        (await _sut.AddContentAsync(product.Id, feature, "videos", first, true, Ct)).Succeeded.Should().BeTrue();

        (await _sut.AddContentAsync(product.Id, feature, "videos", second, true, Ct)).Succeeded.Should().BeTrue();

        var content = (await _sut.GetOverviewAsync(product.Id, Ct)).Features.Single().Content;
        content.Single(c => c.ItemSlug == first).IsThumbnail.Should().BeFalse();
        content.Single(c => c.ItemSlug == second).IsThumbnail.Should().BeTrue();
    }

    [Fact]
    public async Task AddContent_ForAnUnknownItemFeatureOrBlankValues_LinksNothing()
    {
        var product = await CreateProductAsync();
        var other = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Agent mode");
        var video = await SeedVideoAsync("Video");

        (await _sut.AddContentAsync(product.Id, feature, "videos", $"missing-{Guid.NewGuid():N}", false, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.AddContentAsync(product.Id, long.MaxValue, "videos", video, false, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.AddContentAsync(other.Id, feature, "videos", video, false, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.AddContentAsync(product.Id, feature, " ", video, false, Ct)).Status.Should().Be(ProductOperationStatus.Invalid);
        (await _sut.AddContentAsync(product.Id, feature, "videos", " ", false, Ct)).Status.Should().Be(ProductOperationStatus.Invalid);

        (await _sut.GetOverviewAsync(product.Id, Ct)).Features.Single().Content.Should().BeEmpty();
    }

    [Fact]
    public async Task RemoveContent_RemovesOnlyThatLink_AndKeepsTheContentItem()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Agent mode");
        var removed = await SeedVideoAsync("Removed video");
        var kept = await SeedVideoAsync("Kept video");
        (await _sut.AddContentAsync(product.Id, feature, "videos", removed, false, Ct)).Succeeded.Should().BeTrue();
        (await _sut.AddContentAsync(product.Id, feature, "videos", kept, false, Ct)).Succeeded.Should().BeTrue();

        (await _sut.RemoveContentAsync(product.Id, feature, "videos", removed, Ct)).Succeeded.Should().BeTrue();

        (await _sut.GetOverviewAsync(product.Id, Ct)).Features.Single().Content.Select(c => c.ItemSlug).Should().Equal(kept);
        (await _fixture.Connection.ExecuteScalarAsync<bool>("SELECT EXISTS (SELECT 1 FROM content_items WHERE slug = @Slug)", new { Slug = removed })).Should().BeTrue();
        (await _sut.RemoveContentAsync(product.Id, feature, "videos", removed, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
    }

    [Fact]
    public async Task SetThumbnail_MovesTheThumbnail_AndAnUnknownLinkKeepsThePreviousOne()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Agent mode");
        var current = await SeedVideoAsync("Current");
        var next = await SeedVideoAsync("Next");
        var unlinked = await SeedVideoAsync("Unlinked");
        (await _sut.AddContentAsync(product.Id, feature, "videos", current, true, Ct)).Succeeded.Should().BeTrue();
        (await _sut.AddContentAsync(product.Id, feature, "videos", next, false, Ct)).Succeeded.Should().BeTrue();

        (await _sut.SetThumbnailAsync(product.Id, feature, "videos", unlinked, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.GetOverviewAsync(product.Id, Ct)).Features.Single().Content.Single(c => c.IsThumbnail).ItemSlug.Should().Be(current);

        (await _sut.SetThumbnailAsync(product.Id, feature, "videos", next, Ct)).Succeeded.Should().BeTrue();

        (await _sut.GetOverviewAsync(product.Id, Ct)).Features.Single().Content.Single(c => c.IsThumbnail).ItemSlug.Should().Be(next);
    }

    [Fact]
    public async Task Operations_OnTheDetailsAndContentOfAFeatureOfAnotherProduct_AreNotFound()
    {
        var owner = await CreateProductAsync();
        var other = await CreateProductAsync();
        var feature = await CreateFeatureAsync(owner.Id, "Agent mode");
        var video = await SeedVideoAsync("Video");
        (await _sut.AddContentAsync(owner.Id, feature, "videos", video, true, Ct)).Succeeded.Should().BeTrue();

        (await _sut.UpdateDetailsAsync(other.Id, feature, "Hijacked", null, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.RemoveContentAsync(other.Id, feature, "videos", video, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.SetThumbnailAsync(other.Id, feature, "videos", video, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);

        var stored = (await _sut.GetOverviewAsync(owner.Id, Ct)).Features.Single();
        stored.Description.Should().BeEmpty();
        stored.Content.Should().ContainSingle().Which.IsThumbnail.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_RemovesTheContentLinksOfTheFeatureButNotTheContent()
    {
        var product = await CreateProductAsync();
        var feature = await CreateFeatureAsync(product.Id, "Doomed");
        var video = await SeedVideoAsync("Video");
        (await _sut.AddContentAsync(product.Id, feature, "videos", video, false, Ct)).Succeeded.Should().BeTrue();

        (await _sut.DeleteAsync(product.Id, feature, Ct)).Succeeded.Should().BeTrue();

        (await _fixture.Connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM product_feature_content WHERE feature_id = @Id", new { Id = feature })).Should().Be(0);
        (await _fixture.Connection.ExecuteScalarAsync<bool>("SELECT EXISTS (SELECT 1 FROM content_items WHERE slug = @Slug)", new { Slug = video })).Should().BeTrue();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string UniqueName(string prefix) => $"{prefix}-{Guid.NewGuid():N}";

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

    private async Task<string> SeedItemAsync()
    {
        var slug = $"feature-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash, sections_bitmask, created_at)
            VALUES (@Slug, 'news', 'Feature test', '', '', 1743278400, 'ai',
                    @Url, 'Author', 'Feed', '', 'hash', 1, NOW())
            """,
            new { Slug = slug, Url = $"https://example.com/{slug}" });
        return slug;
    }

    private async Task<string> SeedVideoAsync(string title)
    {
        var slug = $"feature-video-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash, sections_bitmask, created_at)
            VALUES (@Slug, 'videos', @Title, '', '', 1743278400, 'ai',
                    @Url, 'Author', 'Feed', '', 'hash', 1, NOW())
            """,
            new { Slug = slug, Title = title, Url = $"https://example.com/{slug}" });
        return slug;
    }

    private async Task<ProductSummary> CreateProductAsync()
    {
        var name = UniqueName("Product");
        var result = await _products.CreateAsync(name, Ct);
        result.Succeeded.Should().BeTrue(result.Message);
        return new ProductSummary { Id = result.ProductId!.Value, Name = name };
    }

    private async Task<long> CreateFeatureAsync(long productId, string name)
    {
        var result = await _sut.CreateAsync(productId, name, Ct);
        result.Succeeded.Should().BeTrue(result.Message);
        return result.ProductId!.Value;
    }

    private async Task<List<(string Feature, long? FeatureId)>> GetEventFeaturesAsync(string slug) =>
        (await _fixture.Connection.QueryAsync<(string Feature, long? FeatureId)>(
            "SELECT feature AS Feature, feature_id AS FeatureId FROM content_timeline_events WHERE collection_name = 'news' AND item_slug = @Slug ORDER BY id",
            new { Slug = slug })).ToList();
}
