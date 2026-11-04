using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for the part of the catalog migration (050_product_catalog.sql) that turns the features of the old GitHub Copilot features page
/// into events of the timeline and into features of the catalog. The fixture runs the migration on an empty database, so the old features are
/// created here and the script runs again (it is repeatable).
/// </summary>
public class GitHubCopilotFeaturesMigrationTests : IClassFixture<DatabaseFixture<GitHubCopilotFeaturesMigrationTests>>
{
    private static readonly int _gitHubCopilotBit = SectionBitmasks.For("github-copilot");

    private readonly DatabaseFixture<GitHubCopilotFeaturesMigrationTests> _fixture;
    private readonly TimelineEventRepository _timeline;
    private readonly ProductFeatureRepository _features;
    private readonly ProductRepository _products;

    public GitHubCopilotFeaturesMigrationTests(DatabaseFixture<GitHubCopilotFeaturesMigrationTests> fixture)
    {
        _fixture = fixture;
        _timeline = new TimelineEventRepository(fixture.Connection);
        _features = new ProductFeatureRepository(fixture.Connection, NullLogger<ProductFeatureRepository>.Instance);
        _products = new ProductRepository(fixture.Connection, NullLogger<ProductRepository>.Instance);
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    private static long Epoch(int year, int month, int day) => new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();

    private async Task<long> SeedFeatureAsync(string name, string description = "", long? releaseDate = null, params string[] contentTitles)
    {
        // The migration seeds the features of the catalog, so a feature of an old name replaces the seeded one.
        await _fixture.Connection.ExecuteAsync(
            "DELETE FROM product_features f USING products p WHERE p.id = f.product_id AND lower(p.name) = 'github copilot' AND lower(f.name) = lower(@Name)",
            new { Name = name });

        var id = await _fixture.Connection.ExecuteScalarAsync<long>(
            """
            INSERT INTO product_features (product_id, name, description, release_date)
            SELECT id, @Name, @Description, @ReleaseDate FROM products WHERE lower(name) = 'github copilot'
            RETURNING id
            """,
            new { Name = name, Description = description, ReleaseDate = releaseDate });

        foreach (var title in contentTitles)
        {
            var slug = $"ghc-feature-video-{Guid.NewGuid():N}";
            await _fixture.Connection.ExecuteAsync(
                """
                INSERT INTO content_items
                    (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                     external_url, author, feed_name, tags_csv, sections_bitmask, content_hash)
                VALUES (@Slug, 'videos', @Title, '', '', 1743278400, 'github-copilot', @Url, 'Author', 'Feed', '', @Bitmask, 'hash');
                INSERT INTO product_feature_content (feature_id, collection_name, item_slug, sort_order)
                VALUES (@Id, 'videos', @Slug, 0);
                """,
                new { Slug = slug, Title = title, Url = $"https://example.com/{slug}", Bitmask = _gitHubCopilotBit, Id = id });
        }

        return id;
    }

    private async Task RunMigrationAsync() => await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync());

    private async Task<IReadOnlyList<TimelineEntry>> TimelineAsync() =>
        await _timeline.GetTimelineAsync(new TimelineRequest("github-copilot"), Ct);

    private async Task<long> CopilotIdAsync() =>
        (await _products.GetProductsAsync("GitHub Copilot", Ct)).Single(p => p.Name == "GitHub Copilot").Id;

    private async Task<IReadOnlyList<ProductFeature>> CopilotFeaturesAsync() =>
        (await _features.GetOverviewAsync(await CopilotIdAsync(), Ct)).Features;

    [Fact]
    public async Task ADatedFeatureWithContent_BecomesAnEventWithEveryLinkedItemAsASource()
    {
        await SeedFeatureAsync("Fetch Webpage", "Include a web page as context.", Epoch(2025, 4, 4), "Fetch video one", "Fetch video two");

        await RunMigrationAsync();

        var entry = (await TimelineAsync()).Single(e => e.Feature == "Fetch webpage");
        entry.Kind.Should().Be("launch");
        entry.Date.Should().Be(new DateOnly(2025, 4, 4));
        entry.DatePrecision.Should().Be("day");
        entry.Description.Should().Be("Include a web page as context.");
        entry.Product.Should().Be("GitHub Copilot");
        entry.Sources.Select(s => s.Title).Should().BeEquivalentTo("Fetch video one", "Fetch video two");
        (await CopilotFeaturesAsync()).Select(f => f.Name).Should().Contain("Fetch webpage").And.NotContain("Fetch Webpage");
    }

    [Fact]
    public async Task AReleaseDateOnTheFirstOfAMonth_IsAMonth()
    {
        await SeedFeatureAsync("Code Referencing", "Shows when a suggestion matches public code.", Epoch(2024, 3, 1), "Referencing video");

        await RunMigrationAsync();

        (await TimelineAsync()).Single(e => e.Feature == "Code referencing").DatePrecision.Should().Be("month");
    }

    [Fact]
    public async Task AFeatureWithoutContentOrReleaseDate_GivesNoEventAndStaysAFeature()
    {
        await SeedFeatureAsync("Security Advisory Summaries", "Summaries of advisories.", Epoch(2024, 6, 1));

        await RunMigrationAsync();

        (await TimelineAsync()).Should().NotContain(e => e.Feature == "Security Advisory Summaries");
        (await CopilotFeaturesAsync()).Select(f => f.Name).Should().Contain("Security Advisory Summaries");
    }

    [Fact]
    public async Task FeaturesThatDescribeTheSameCapability_AreMergedAndTheEventsKeepTheirOwnName()
    {
        await SeedFeatureAsync("Chat in IDE", "Chat in the editor.", Epoch(2025, 1, 2), "Chat video");
        await SeedFeatureAsync("Chat on Mobile", "Brings Copilot Chat to the mobile app.", Epoch(2024, 12, 24), "Mobile video");

        await RunMigrationAsync();

        var features = await CopilotFeaturesAsync();
        features.Select(f => f.Name).Should().Contain("Copilot Chat").And.NotContain(["Chat in IDE", "Chat on Mobile"]);
        features.Single(f => f.Name == "Copilot Chat").Aliases.Select(a => a.Alias).Should().Contain(["Chat in IDE", "Chat on Mobile"]);

        var mobile = (await TimelineAsync()).Single(e => e.Feature == "Copilot Chat" && e.Description.StartsWith("Chat on Mobile:", StringComparison.Ordinal));
        mobile.Description.Should().Be("Chat on Mobile: Brings Copilot Chat to the mobile app.");
        mobile.Date.Should().Be(new DateOnly(2024, 12, 24));
    }

    [Fact]
    public async Task AFeatureThatIsAProduct_BecomesAnEventOfThatProductAndLosesItsFeature()
    {
        await SeedFeatureAsync("GitHub Spark", "Build apps from a prompt.", Epoch(2025, 10, 1), "Spark video");

        await RunMigrationAsync();

        var entry = (await TimelineAsync()).Single(e => e.Product == "GitHub Spark");
        entry.Feature.Should().BeNull();
        entry.Description.Should().Be("GitHub Spark: Build apps from a prompt.");
        (await CopilotFeaturesAsync()).Select(f => f.Name).Should().NotContain("GitHub Spark");
        (await _products.GetProductsAsync("GitHub Spark", Ct)).Single(p => p.Name == "GitHub Spark").ParentName.Should().Be("GitHub");
    }

    [Fact]
    public async Task APreviewOfAProduct_KeepsItsKind()
    {
        await SeedFeatureAsync("Preview for Copilot Workspace", "A task-centric environment.", Epoch(2024, 5, 9), "Workspace video");

        await RunMigrationAsync();

        var entry = (await TimelineAsync()).Single(e => e.Product == "GitHub Copilot Workspace");
        entry.Kind.Should().Be("preview");
        (await _products.GetProductsAsync("GitHub Copilot Workspace", Ct)).Single(p => p.Name == "GitHub Copilot Workspace").ParentName.Should().Be("GitHub Copilot");
    }

    [Fact]
    public async Task WhatIsNoFeature_IsRemoved()
    {
        await SeedFeatureAsync("User Management", "Manage seats.");
        await SeedFeatureAsync("Data Excluded From Training by Default", "A policy.");

        await RunMigrationAsync();

        (await CopilotFeaturesAsync()).Select(f => f.Name).Should().NotContain(["User Management", "Data Excluded From Training by Default"]);
    }

    [Fact]
    public async Task TokenBasedBillingAiCreditsAndPremiumRequests_AreFeaturesOfGitHubCopilot()
    {
        const string Combined = "Token-based billing: from premium request units to AI credits and tokens";
        await SeedFeatureAsync(Combined, "Billing changes.", Epoch(2026, 6, 1), "Billing video");

        await RunMigrationAsync();

        var features = await CopilotFeaturesAsync();
        features.Select(f => f.Name).Should().Contain(["Token-based billing", "AI credits", "Premium requests"]);
        features.Single(f => f.Name == "Token-based billing").Aliases.Select(a => a.Alias).Should().Contain(Combined).And.NotContain("AI credits");
        features.Single(f => f.Name == "AI credits").Aliases.Select(a => a.Alias).Should().NotContain(Combined);

        var billing = (await TimelineAsync()).Single(e => e.Feature == "Token-based billing");
        billing.Kind.Should().Be("pricing-change");
        billing.Date.Should().Be(new DateOnly(2026, 6, 1));
    }

    [Fact]
    public async Task AnEventOfTheAiThatNamesAiCredits_LinksToTheFeature()
    {
        var slug = $"ghc-ai-credits-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync(
            """
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, sections_bitmask, content_hash)
            VALUES (@Slug, 'news', 'Credits', '', '', 1743278400, 'github-copilot', @Url, 'Author', 'Feed', '', @Bitmask, 'hash')
            """,
            new { Slug = slug, Url = $"https://example.com/{slug}", Bitmask = _gitHubCopilotBit });
        await _timeline.ReplaceEventsAsync(
            "news",
            slug,
            [new() { Kind = "pricing-change", Product = "GitHub Copilot", Feature = "AI credits", Description = "Usage moves to credits", Date = new DateOnly(2026, 6, 1), DatePrecision = "day", DateSource = "explicit" }],
            ITimelineEventRepository.ExtractedFromSource,
            ITimelineEventRepository.CurrentPromptVersion,
            Ct);

        var featureId = await _fixture.Connection.ExecuteScalarAsync<long?>(
            "SELECT feature_id FROM content_timeline_events WHERE collection_name = 'news' AND item_slug = @Slug", new { Slug = slug });

        featureId.Should().NotBeNull();
        (await _fixture.Connection.ExecuteScalarAsync<string>("SELECT name FROM product_features WHERE id = @Id", new { Id = featureId })).Should().Be("AI credits");
    }

    [Fact]
    public async Task RunningTheMigrationAgain_DoesNotAddTheEventsTwice()
    {
        await SeedFeatureAsync("Fine Tuned Models", "Models trained on your code.", Epoch(2024, 10, 1), "Fine tuned video");
        await RunMigrationAsync();
        var first = await _fixture.Connection.ExecuteScalarAsync<int>("SELECT COUNT(*)::int FROM content_timeline_events WHERE extracted_from = 'catalog'");

        await RunMigrationAsync();

        (await _fixture.Connection.ExecuteScalarAsync<int>("SELECT COUNT(*)::int FROM content_timeline_events WHERE extracted_from = 'catalog'")).Should().Be(first);
    }

    private static async Task<string> ReadMigrationAsync()
    {
        var assembly = typeof(ProductRepository).Assembly;
        var name = assembly.GetManifestResourceNames().Single(n => n.EndsWith("050_product_catalog.sql", StringComparison.Ordinal));
        await using var stream = assembly.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
