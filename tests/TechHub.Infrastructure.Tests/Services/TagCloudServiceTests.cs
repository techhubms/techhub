using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for TagCloudService
/// Tests tag cloud generation, quantile sizing, and scoping logic
/// Uses FileBasedContentRepository with TestCollections for test data
/// </summary>
public class TagCloudServiceTests
{
    private readonly IContentRepository _repository;
    private readonly TagCloudService _service;
    private readonly FilteringOptions _filteringOptions;

    public TagCloudServiceTests()
    {
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

        // Setup: Create FileBasedContentRepository pointing to TestCollections
        var testCollectionsPath = "/workspaces/techhub/tests/TechHub.TestUtilities/TestCollections";

        // Load real AppSettings from appsettings.json and override collections path
        var settings = ConfigurationHelper.LoadAppSettings(overrideCollectionsPath: testCollectionsPath);

        var mockEnvironment = new Mock<IHostEnvironment>();
        mockEnvironment.Setup(e => e.ContentRootPath).Returns(testCollectionsPath);

        var cache = new MemoryCache(new MemoryCacheOptions());
        var markdownService = new MarkdownService();

        _repository = new FileBasedContentRepository(
            Options.Create(settings),
            markdownService,
            mockEnvironment.Object,
            cache
        );

        _service = new TagCloudService(_repository, Options.Create(_filteringOptions));
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
        result.Should().Contain(t => t.Tag == "Code Review");
        result.Should().Contain(t => t.Tag == "Developer Tools");
        result.Should().Contain(t => t.Tag == "Collaboration");
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

        // Assert - Tags that exist in "ai" section files within the last 90 days
        // (2026-01-* files with section_names containing "ai")
        result.Should().Contain(t => t.Tag == "Developer Tools", "Developer Tools tag exists in ai section files");
        result.Should().Contain(t => t.Tag == "Developer Productivity", "Developer Productivity tag exists in ai section files");
        result.Should().NotContain(t => t.Tag == "Uncategorized", "Uncategorized tag doesn't exist in ai section");
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
            LastDays = 1095 // Use 3 years to include all test data (2024-2027)
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert - Should return tags from ALL sections (ai, github-copilot, ml, etc.)
        result.Should().NotBeEmpty("'all' section should return tags from all sections");
        result.Should().Contain(t => t.Tag == "Code Review");
        result.Should().Contain(t => t.Tag == "Developer Tools");
        result.Should().Contain(t => t.Tag == "Collaboration");
        result.Should().Contain(t => t.Tag == "Coding", "Coding is not a section name and should appear in tag cloud");
    }

    #endregion

    #region Collection Scope Tests

    [Fact]
    public async Task GetTagCloud_CollectionScope_ReturnsOnlyTagsFromCollection()
    {
        // Arrange - use "blogs" without underscore (items have CollectionName = "blogs")
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Collection,
            SectionName = "ai",
            CollectionName = "blogs", // Collection names don't have underscore prefix
            MaxTags = 20,
            MinUses = 1,
            LastDays = 1095 // Use 3 years to include all test data (2024-2027)
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        result.Should().Contain(t => t.Tag == "Code Review");
        result.Should().Contain(t => t.Tag == "Collaboration");
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
            LastDays = 1095 // Use 3 years to include all test data (2024-2027)
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert - Should return tags from all collections in the section (both _blogs and _news)
        result.Should().NotBeEmpty("'all' collection should return tags from all collections in the section");
        result.Should().Contain(t => t.Tag == "Copilot");
        result.Should().Contain(t => t.Tag == "Developer Tools"); // From multiple collections
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

    [Fact]
    public async Task GetTagCloud_AllSectionWithSpecificCollection_ReturnsTagsFromAllSections()
    {
        // Arrange - The "all" section is virtual and should show content from all sections
        // When accessing /all/blogs, we want tags from blog posts across ALL sections (ai, github-copilot, etc.)
        var request = new TagCloudRequest
        {
            Scope = TagCloudScope.Collection,
            SectionName = "all", // Virtual "all" section
            CollectionName = "blogs", // Specific collection
            MaxTags = 20,
            MinUses = 1,
            LastDays = 90
        };

        // Act
        var result = await _service.GetTagCloudAsync(request, CancellationToken.None);

        // Assert
        // Should return tags from blogs across all sections (not filtered by section)
        result.Should().NotBeEmpty("Tags from blogs across all sections should be included");

        // Verify we get tags from blog posts
        var tags = result.Select(t => t.Tag).ToList();

        // We have 2 blog posts in test data:
        // 1. "From Tool to Teammate" (ai, github-copilot sections)
        // 2. "Azure Cost Estimation" (azure section)
        // With section="all", we should see tags from blogs in all sections
        tags.Should().Contain("Azure Pricing", "Azure Cost blog should be included");
    }

    #endregion
}
