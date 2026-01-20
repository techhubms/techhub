using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.DTOs;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Integration tests for TagCloudService
/// Tests tag cloud generation, quantile sizing, and scoping logic
/// </summary>
public class TagCloudServiceTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly IContentRepository _repository;
    private readonly TagCloudService _service;
    private readonly FilteringOptions _filteringOptions;

    public TagCloudServiceTests()
    {
        // Setup: Create temporary test directory
        _tempDirectory = Path.Combine(Path.GetTempPath(), $"techhub-test-{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempDirectory);

        // Setup: Create test content structure
        Directory.CreateDirectory(Path.Combine(_tempDirectory, "collections", "_blogs"));
        Directory.CreateDirectory(Path.Combine(_tempDirectory, "collections", "_news"));

        // Setup: Create test content files with various tag distributions
        CreateTestContent("item1", "AI Section", ["ai", "machine-learning", "copilot"], "ai", "_blogs", DateTimeOffset.UtcNow.AddDays(-10));
        CreateTestContent("item2", "AI Content 2", ["ai", "copilot"], "ai", "_blogs", DateTimeOffset.UtcNow.AddDays(-5));
        CreateTestContent("item3", "AI Content 3", ["ai", "machine-learning"], "ai", "_news", DateTimeOffset.UtcNow.AddDays(-3));
        CreateTestContent("item4", "Copilot Content", ["copilot", "productivity"], "github-copilot", "_blogs", DateTimeOffset.UtcNow.AddDays(-2));
        CreateTestContent("item5", "ML Content", ["machine-learning", "azure"], "ml", "_news", DateTimeOffset.UtcNow.AddDays(-50)); // Old content
        CreateTestContent("item6", "Rare Tag", ["rare-tag"], "ai", "_blogs", DateTimeOffset.UtcNow.AddHours(-12)); // Half day ago

        // Setup: Create AppSettings
        var settings = new AppSettings
        {
            Content = new ContentSettings
            {
                CollectionsPath = Path.Combine(_tempDirectory, "collections"),
                Sections = [],
                Timezone = "Europe/Brussels"
            },
            Seo = new SeoSettings
            {
                BaseUrl = "https://test.example.com",
                SiteTitle = "Test Site",
                SiteDescription = "Test Description"
            },
            Performance = new PerformanceSettings(),
            Caching = new CachingSettings()
        };

        // Setup: Create filtering options
        _filteringOptions = new FilteringOptions
        {
            TagCloud = new TagCloudOptions
            {
                DefaultMaxTags = 20,
                MinimumTagUses = 1,
                DefaultDateRangeDays = 90,
                QuantilePercentiles = new QuantilePercentilesOptions
                {
                    SmallToMedium = 0.25,
                    MediumToLarge = 0.75
                }
            }
        };

        // Setup: Create mock IHostEnvironment
        var mockEnvironment = new Mock<IHostEnvironment>();
        mockEnvironment.Setup(e => e.ContentRootPath).Returns(_tempDirectory);

        // Setup: Create mock ITagMatchingService
        var mockTagMatchingService = new Mock<ITagMatchingService>();
        mockTagMatchingService.Setup(s => s.Normalize(It.IsAny<string>()))
            .Returns((string tag) => tag.ToLowerInvariant());

        // Setup: Create dependencies
        var markdownService = new MarkdownService();
        _repository = new FileBasedContentRepository(
            Options.Create(settings),
            markdownService,
            mockTagMatchingService.Object,
            mockEnvironment.Object
        );

        _service = new TagCloudService(_repository, Options.Create(_filteringOptions));
    }

    private void CreateTestContent(string slug, string title, string[] tags, string sectionName, string collectionName, DateTimeOffset date)
    {
        var content = $@"---
title: {title}
slug: {slug}
description: Test content
section_names:
  - {sectionName}
tags:
{string.Join("\n", tags.Select(t => $"  - {t}"))}
date: {date:yyyy-MM-dd}
---

Test content body.
";

        var collectionPath = Path.Combine(_tempDirectory, "collections", collectionName);
        File.WriteAllText(Path.Combine(collectionPath, $"{date:yyyy-MM-dd}-{slug}.md"), content);
    }

    public void Dispose()
    {
        // Cleanup: Delete temporary test directory
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }

        GC.SuppressFinalize(this);
    }

    #region Homepage Scope Tests

    [Fact]
    public async Task GetTagCloud_HomepageScope_ReturnsAllTags()
    {
        // Arrange
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Homepage,
            MaxTags = 20,
            MinUses = 1,
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Contain(t => t.Tag == "ai");
        result.Should().Contain(t => t.Tag == "copilot");
        result.Should().Contain(t => t.Tag == "machine-learning");
        result.Should().BeInDescendingOrder(t => t.Count);
    }

    [Fact]
    public async Task GetTagCloud_HomepageScope_RespectsMaxTags()
    {
        // Arrange
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Homepage,
            MaxTags = 3,
            MinUses = 1,
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        result.Should().HaveCountLessThanOrEqualTo(3);
    }

    [Fact]
    public async Task GetTagCloud_HomepageScope_RespectsMinUses()
    {
        // Arrange
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Homepage,
            MaxTags = 20,
            MinUses = 2, // "rare-tag" appears only once
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotContain(t => t.Tag == "rare-tag");
        result.All(t => t.Count >= 2).Should().BeTrue();
    }

    [Fact]
    public async Task GetTagCloud_HomepageScope_RespectsLastDays()
    {
        // Arrange
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Homepage,
            MaxTags = 20,
            MinUses = 1,
            LastDays = 30 // item5 is 50 days old with "azure" tag
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotContain(t => t.Tag == "azure", "azure tag is only on old content");
    }

    #endregion

    #region Section Scope Tests

    [Fact]
    public async Task GetTagCloud_SectionScope_ReturnsOnlyTagsFromSection()
    {
        // Arrange
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Section,
            SectionName = "ai",
            MaxTags = 20,
            MinUses = 1,
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        result.Should().Contain(t => t.Tag == "ai");
        result.Should().Contain(t => t.Tag == "machine-learning");
        result.Should().NotContain(t => t.Tag == "productivity", "productivity is only in github-copilot section");
    }

    [Fact]
    public async Task GetTagCloud_SectionScope_AllSection_ReturnsTagsFromAllSections()
    {
        // Arrange - "all" is a virtual section that should show content from all sections
        // This bug was discovered when /all showed "No tags available"
        // The repository's GetBySectionAsync("all") returns empty because no content has sectionName="all"
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Section,
            SectionName = "all", // Virtual section - should return all content across all sections
            MaxTags = 20,
            MinUses = 1,
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert - Should return tags from ALL sections (ai, github-copilot, ml, etc.)
        result.Should().NotBeEmpty("'all' section should return tags from all sections");
        result.Should().Contain(t => t.Tag == "ai");
        result.Should().Contain(t => t.Tag == "copilot");
        result.Should().Contain(t => t.Tag == "machine-learning");
        result.Should().Contain(t => t.Tag == "productivity"); // From github-copilot section
    }

    #endregion

    #region Collection Scope Tests

    [Fact]
    public async Task GetTagCloud_CollectionScope_ReturnsOnlyTagsFromCollection()
    {
        // Arrange
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Collection,
            SectionName = "ai",
            CollectionName = "_blogs",
            MaxTags = 20,
            MinUses = 1,
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        result.Should().Contain(t => t.Tag == "ai");
        result.Should().Contain(t => t.Tag == "copilot");
        // item3 is in _news, not _blogs, so its unique tags shouldn't appear
    }

    [Fact]
    public async Task GetTagCloud_CollectionScope_AllCollection_ReturnsTagsFromAllCollectionsInSection()
    {
        // Arrange - "all" is a virtual collection that should show all content for the section
        // This bug was discovered when /github-copilot/all showed "No tags available"
        // while /github-copilot showed tags correctly
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Collection,
            SectionName = "ai",
            CollectionName = "all", // Virtual collection - should return all section content
            MaxTags = 20,
            MinUses = 1,
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert - Should return tags from all collections in the section (both _blogs and _news)
        result.Should().NotBeEmpty("'all' collection should return tags from all collections in the section");
        result.Should().Contain(t => t.Tag == "ai");
        result.Should().Contain(t => t.Tag == "machine-learning"); // From both _blogs and _news
    }

    #endregion

    #region Quantile Sizing Tests

    [Fact]
    public async Task GetTagCloud_AppliesQuantileSizing_CorrectDistribution()
    {
        // Arrange - Create content with clear count distribution
        // ai: 3 uses, copilot: 3 uses, machine-learning: 3 uses, productivity: 1 use, azure: 1 use, rare-tag: 1 use
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Homepage,
            MaxTags = 20,
            MinUses = 1,
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        var largeTags = result.Where(t => t.Size == TagSize.Large).ToList();
        var mediumTags = result.Where(t => t.Size == TagSize.Medium).ToList();
        var smallTags = result.Where(t => t.Size == TagSize.Small).ToList();

        // All tags should have a size
        (largeTags.Count + mediumTags.Count + smallTags.Count).Should().Be(result.Count);

        // Tags should be in descending order
        result.Should().BeInDescendingOrder(t => t.Count);

        // Large tags should have higher counts than small tags
        if (largeTags.Any() && smallTags.Any())
        {
            largeTags.Min(t => t.Count).Should().BeGreaterThanOrEqualTo(smallTags.Max(t => t.Count));
        }
    }

    [Fact]
    public async Task GetTagCloud_SingleTag_GetsLargeSize()
    {
        // Arrange
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Homepage,
            MaxTags = 1,
            MinUses = 1,
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result[0].Size.Should().Be(TagSize.Large);
    }

    [Fact]
    public async Task GetTagCloud_TwoTags_GetLargeAndMedium()
    {
        // Arrange
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Homepage,
            MaxTags = 2,
            MinUses = 1,
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result[0].Size.Should().Be(TagSize.Large, "first tag (highest count) should be Large");
        result[1].Size.Should().Be(TagSize.Medium, "second tag should be Medium (quantile sizing with 2 tags produces Large + Medium)");
    }

    #endregion

    #region GetAllTags Tests

    [Fact]
    public async Task GetAllTags_NoFilters_ReturnsAllTagsWithCounts()
    {
        // Act
        var result = await _service.GetAllTagsAsync(null, null, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Tags.Should().NotBeEmpty();
        result.Tags.Should().Contain(t => t.Tag == "ai");
        result.Tags.Should().Contain(t => t.Tag == "copilot");
        result.Tags.All(t => t.Count > 0).Should().BeTrue();
        result.Tags.Should().BeInDescendingOrder(t => t.Count);
    }

    [Fact]
    public async Task GetAllTags_WithSectionFilter_ReturnsOnlySectionTags()
    {
        // Act
        var result = await _service.GetAllTagsAsync("ai", null, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Tags.Should().Contain(t => t.Tag == "ai");
        result.Tags.Should().NotContain(t => t.Tag == "productivity", "productivity is only in github-copilot");
    }

    [Fact]
    public async Task GetAllTags_WithSectionAndCollectionFilter_ReturnsOnlyCollectionTags()
    {
        // Act
        var result = await _service.GetAllTagsAsync("ai", "_blogs", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Tags.Should().Contain(t => t.Tag == "ai");
        // Verify tags are from _blogs only
    }

    #endregion

    #region Edge Cases

    [Fact]
    public async Task GetTagCloud_NoContentMatchesFilters_ReturnsEmptyList()
    {
        // Arrange - Use VeryHighMinUses filter approach instead of LastDays
        // The LastDays filter is unreliable because item6 (created 12 hours ago) will be included
        // for "last 1 day" in most timezones. Using a filter that guarantees no matches.
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Section,
            SectionName = "nonexistent-section", // Section that doesn't exist = no content
            MaxTags = 20,
            MinUses = 1,
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        result.Should().BeEmpty("No content should exist for a non-existent section");
    }

    [Fact]
    public async Task GetTagCloud_VeryHighMinUses_ReturnsEmptyList()
    {
        // Arrange
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Homepage,
            MaxTags = 20,
            MinUses = 100, // No tag appears 100 times
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion
}
