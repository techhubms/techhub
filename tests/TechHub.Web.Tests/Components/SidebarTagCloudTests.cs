using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models;
using TechHub.Web.Components;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for SidebarTagCloud.razor component
/// 
/// REQUIREMENTS (from T023):
/// - Test tag cloud renders with correct tags
/// - Test tag selection toggles state
/// - Test selected tags are highlighted
/// - Test tag click updates URL parameters
/// - Test URL parameters restore tag selection
/// </summary>
public class SidebarTagCloudTests : BunitContext
{
    private readonly Mock<ITechHubApiClient> _mockApiClient;

    public SidebarTagCloudTests()
    {
        _mockApiClient = new Mock<ITechHubApiClient>();
        Services.AddSingleton(_mockApiClient.Object);
        AddBunitPersistentComponentState();
    }

    [Fact]
    public void SidebarTagCloud_RendersTagsCorrectly()
    {
        // Arrange
        var tags = CreateTestTagCloud();
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        // Act
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        // Assert
        cut.WaitForAssertion(() => cut.FindAll(".tag-cloud-item").Should().HaveCount(5));

        var tagButtons = cut.FindAll(".tag-cloud-item");
        tagButtons[0].TextContent.Should().Contain("AI");
        tagButtons[1].TextContent.Should().Contain("Azure");
        tagButtons[2].TextContent.Should().Contain("GitHub Copilot");
    }

    [Fact]
    public void SidebarTagCloud_AppliesCorrectSizeClasses()
    {
        // Arrange
        var tags = new List<TagCloudItem>
        {
            new TagCloudItem { Tag = "Large Tag", Count = 100, Size = TagSize.Large },
            new TagCloudItem { Tag = "Medium Tag", Count = 50, Size = TagSize.Medium },
            new TagCloudItem { Tag = "Small Tag", Count = 10, Size = TagSize.Small }
        };
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        // Act
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        // Assert
        cut.WaitForAssertion(() =>
        {
            var tagElements = cut.FindAll(".tag-cloud-item");
            tagElements[0].ClassList.Should().Contain("tag-size-large");
            tagElements[1].ClassList.Should().Contain("tag-size-medium");
            tagElements[2].ClassList.Should().Contain("tag-size-small");
        });
    }

    [Fact]
    public void SidebarTagCloud_TogglesSelection_OnClick()
    {
        // Arrange
        var tags = CreateTestTagCloud();
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        cut.WaitForAssertion(() => cut.FindAll(".tag-cloud-item").Should().HaveCount(5));

        // Act - Select first tag
        var firstTag = cut.Find(".tag-cloud-item");
        firstTag.Click();

        // Assert - Tag should be selected
        cut.WaitForAssertion(() => firstTag.ClassList.Should().Contain("selected"));

        // Act - Click again to deselect
        firstTag.Click();

        // Assert - Tag should be deselected
        cut.WaitForAssertion(() => firstTag.ClassList.Should().NotContain("selected"));
    }

    [Fact]
    public void SidebarTagCloud_AllowsMultipleSelections()
    {
        // Arrange
        var tags = CreateTestTagCloud();
        // Use ReturnsAsync for synchronous task completion - works with bUnit's rendering
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        cut.WaitForAssertion(() => cut.FindAll(".tag-cloud-item").Should().HaveCount(5));

        // Act - Select multiple tags (re-query DOM after each click using InvokeAsync)
        cut.InvokeAsync(() => cut.FindAll(".tag-cloud-item")[0].Click());
        cut.WaitForAssertion(() => cut.FindAll(".tag-cloud-item.selected").Should().HaveCount(1));

        cut.InvokeAsync(() => cut.FindAll(".tag-cloud-item")[1].Click());
        cut.WaitForAssertion(() => cut.FindAll(".tag-cloud-item.selected").Should().HaveCount(2));

        cut.InvokeAsync(() => cut.FindAll(".tag-cloud-item")[2].Click());

        // Assert - All selected tags should have selected class
        cut.WaitForAssertion(() =>
        {
            var selectedItems = cut.FindAll(".tag-cloud-item.selected");
            selectedItems.Should().HaveCount(3);
            selectedItems[0].TextContent.Should().Contain("AI");
            selectedItems[1].TextContent.Should().Contain("Azure");
            selectedItems[2].TextContent.Should().Contain("GitHub Copilot");
        });
    }

    [Fact]
    public void SidebarTagCloud_RaisesOnSelectionChanged_WhenTagClicked()
    {
        // Arrange
        var tags = CreateTestTagCloud();
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        List<string>? raisedTags = null;
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all")
            .Add(p => p.OnSelectionChanged, EventCallback.Factory.Create<List<string>>(
                this, selectedTags => raisedTags = selectedTags)));

        cut.WaitForAssertion(() => cut.FindAll(".tag-cloud-item").Should().HaveCount(5));

        // Act
        var firstTag = cut.Find(".tag-cloud-item");
        firstTag.Click();

        // Assert
        raisedTags.Should().NotBeNull();
        raisedTags.Should().ContainSingle();
        raisedTags![0].Should().Be("ai"); // Tags are normalized to lowercase
    }

    [Fact]
    public void SidebarTagCloud_InitializesWithSelectedTags()
    {
        // Arrange
        var tags = CreateTestTagCloud();
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        var preSelectedTags = new List<string> { "AI", "Azure" };

        // Act
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all")
            .Add(p => p.SelectedTags, preSelectedTags));

        // Assert
        cut.WaitForAssertion(() =>
        {
            var tagButtons = cut.FindAll(".tag-cloud-item");
            tagButtons.Where(t => t.ClassList.Contains("selected")).Should().HaveCount(2);
        });
    }

    [Fact]
    public void SidebarTagCloud_ShowsLoading_WhileFetchingTags()
    {
        // Arrange
        var tcs = new TaskCompletionSource<IReadOnlyList<TagCloudItem>>();
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(tcs.Task!);

        // Act
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        // Assert - Should show loading state
        cut.Markup.Should().Contain("Loading tags");

        // Complete the task
        tcs.SetResult(CreateTestTagCloud());

        // Assert - Should no longer show loading
        cut.WaitForAssertion(() => cut.Markup.Should().NotContain("Loading tags"));
    }

    [Fact]
    public void SidebarTagCloud_ShowsError_WhenApiFails()
    {
        // Arrange
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromException<IReadOnlyList<TagCloudItem>?>(
                new HttpRequestException("API error")));

        // Act
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        // Assert
        cut.WaitForAssertion(() => cut.Markup.Should().Contain("Error loading tags"));
    }

    [Fact]
    public void SidebarTagCloud_Section_FetchesTagsForSection()
    {
        // Arrange
        var tags = CreateTestTagCloud();
        // Use ReturnsAsync for synchronous task completion - works with bUnit's rendering
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                "ai",
                "all",
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        // Act
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "ai")
            .Add(p => p.CollectionName, "all"));

        // Assert
        cut.WaitForAssertion(() => cut.FindAll(".tag-cloud-item").Should().HaveCount(5));
        // Component loads baseline + filtered tags (2 calls when no filters, more when filters active)
        _mockApiClient.Verify(x => x.GetTagCloudAsync(
            "ai",
            "all",
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<List<string>?>(),
            It.IsAny<List<string>?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public void SidebarTagCloud_Collection_FetchesTagsForCollection()
    {
        // Arrange
        var tags = CreateTestTagCloud();
        // Use ReturnsAsync for synchronous task completion - works with bUnit's rendering
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                "ai",
                "news",
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        // Act
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "ai")
            .Add(p => p.CollectionName, "news"));

        // Assert
        cut.WaitForAssertion(() => cut.FindAll(".tag-cloud-item").Should().HaveCount(5));
        // Component loads baseline + filtered tags (2 calls when no filters, more when filters active)
        _mockApiClient.Verify(x => x.GetTagCloudAsync(
            "ai",
            "news",
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<List<string>?>(),
            It.IsAny<List<string>?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public void SidebarTagCloud_SelectedTag_RemainsVisible_WhenSearchChanges()
    {
        // Arrange - Popular tags change with search, but selected tags must remain visible
        // The API returns selected tag counts + popular fill in a single response
        var combinedTagsForSearch = new List<TagCloudItem>
        {
            new TagCloudItem { Tag = "Security", Count = 3, Size = TagSize.Small },
            new TagCloudItem { Tag = "AI", Count = 20, Size = TagSize.Large },
            new TagCloudItem { Tag = "Developer", Count = 15, Size = TagSize.Medium }
        };

        // Setup: Component makes ONE call with tagsToCount=selectedTags.
        // The API returns those tags' counts + fills remaining slots with popular tags.
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.Is<List<string>>(tags => tags != null && tags.Contains("security")),
                It.Is<List<string>?>(tags => tags != null && tags.Contains("security")),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                "test query",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(combinedTagsForSearch);

        // Act - Initial render, then select Security tag, then add search
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all")
            .Add(p => p.SelectedTags, new List<string> { "Security" })
            .Add(p => p.SearchQuery, "test query"));

        // Assert - Security tag MUST remain visible even though it's not in popular tags for this search
        cut.WaitForAssertion(() =>
        {
            var tagElements = cut.FindAll(".tag-cloud-item");
            tagElements.Should().Contain(t => t.TextContent.Contains("Security"),
                "selected tag must remain visible even when not in popular tags");
            
            // Should show popular tags for search + selected tag
            tagElements.Should().Contain(t => t.TextContent.Contains("AI"));
            tagElements.Should().Contain(t => t.TextContent.Contains("Developer"));
        });
    }

    [Fact]
    public void SidebarTagCloud_SelectedTag_RemainsVisible_WhenDateRangeChanges()
    {
        // Arrange - Popular tags change with date range, but selected tags must remain visible
        // The API returns selected tag counts + popular fill in a single response
        var combinedTagsInRange = new List<TagCloudItem>
        {
            new TagCloudItem { Tag = ".NET", Count = 2, Size = TagSize.Small },
            new TagCloudItem { Tag = "AI", Count = 30, Size = TagSize.Large },
            new TagCloudItem { Tag = "Cloud", Count = 25, Size = TagSize.Medium }
        };

        // Setup: Component makes ONE call with tagsToCount=selectedTags.
        // The API returns those tags' counts + fills remaining slots with popular tags.
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.Is<List<string>>(tags => tags != null && tags.Contains(".net")),
                It.Is<List<string>?>(tags => tags != null && tags.Contains(".net")),
                "2024-01-01",
                "2024-12-31",
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(combinedTagsInRange);

        // Act - Render with selected tag and date range
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all")
            .Add(p => p.SelectedTags, new List<string> { ".NET" })
            .Add(p => p.FromDate, "2024-01-01")
            .Add(p => p.ToDate, "2024-12-31"));

        // Assert - .NET tag MUST remain visible even though it's not in popular tags for this date range
        cut.WaitForAssertion(() =>
        {
            var tagElements = cut.FindAll(".tag-cloud-item");
            tagElements.Should().Contain(t => t.TextContent.Contains(".NET"),
                "selected tag must remain visible even when not in popular tags for date range");
            
            // Should show popular tags for date range + selected tag
            tagElements.Should().Contain(t => t.TextContent.Contains("AI"));
            tagElements.Should().Contain(t => t.TextContent.Contains("Cloud"));
        });
    }

    [Fact]
    public void SidebarTagCloud_MultipleSelectedTags_AllRemainVisible_WhenFiltersChange()
    {
        // Arrange - Multiple selected tags, popular tags change with filters
        // The API returns selected tag counts + popular fill in a single response
        var combinedTagsWithFilters = new List<TagCloudItem>
        {
            new TagCloudItem { Tag = "Security", Count = 5, Size = TagSize.Small },
            new TagCloudItem { Tag = "Azure", Count = 3, Size = TagSize.Small },
            new TagCloudItem { Tag = "AI", Count = 10, Size = TagSize.Large },
            new TagCloudItem { Tag = "Python", Count = 8, Size = TagSize.Medium }
        };

        // Setup: Component makes ONE call with tagsToCount=selectedTags.
        // The API returns those tags' counts + fills remaining slots with popular tags.
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.Is<List<string>>(tags => tags.Contains("security") && tags.Contains("azure")),
                It.Is<List<string>?>(tags => tags != null && tags.Contains("security") && tags.Contains("azure")),
                "2024-01-01",
                "2024-12-31",
                "test",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(combinedTagsWithFilters);

        // Act - Render with multiple selected tags and filters
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all")
            .Add(p => p.SelectedTags, new List<string> { "Security", "Azure" })
            .Add(p => p.FromDate, "2024-01-01")
            .Add(p => p.ToDate, "2024-12-31")
            .Add(p => p.SearchQuery, "test"));

        // Assert - ALL selected tags MUST remain visible
        cut.WaitForAssertion(() =>
        {
            var tagElements = cut.FindAll(".tag-cloud-item");
            
            // Both selected tags must be visible
            tagElements.Should().Contain(t => t.TextContent.Contains("Security"),
                "first selected tag must remain visible");
            tagElements.Should().Contain(t => t.TextContent.Contains("Azure"),
                "second selected tag must remain visible");
            
            // Popular tags for context should also be visible
            tagElements.Should().Contain(t => t.TextContent.Contains("AI"));
            tagElements.Should().Contain(t => t.TextContent.Contains("Python"));
            
            // Both selected tags should be marked as selected
            var selectedTags = cut.FindAll(".tag-cloud-item.selected");
            selectedTags.Should().HaveCount(2, "both tags should be marked as selected");
        });
    }

    [Fact]
    public void SidebarTagCloud_SelectedTags_AppearFirst()
    {
        // Arrange - Selected tags should appear first, then popular tags
        // The API returns selected tag counts + popular fill in a single response
        var combinedTags = new List<TagCloudItem>
        {
            new TagCloudItem { Tag = "Security", Count = 15, Size = TagSize.Small },
            new TagCloudItem { Tag = ".NET", Count = 25, Size = TagSize.Small },
            new TagCloudItem { Tag = "AI", Count = 100, Size = TagSize.Large },
            new TagCloudItem { Tag = "Python", Count = 80, Size = TagSize.Medium },
            new TagCloudItem { Tag = "Azure", Count = 60, Size = TagSize.Medium }
        };

        // Setup: Component makes ONE call with tagsToCount=selectedTags.
        // The API returns those tags' counts + fills remaining slots with popular tags.
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.Is<List<string>>(tags => tags.Contains("security") && tags.Contains(".net")),
                It.Is<List<string>?>(tags => tags != null && tags.Contains("security") && tags.Contains(".net")),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(combinedTags);

        // Act - Render with selected tags
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all")
            .Add(p => p.SelectedTags, new List<string> { "Security", ".NET" }));

        // Assert - Selected tags MUST appear first
        cut.WaitForAssertion(() =>
        {
            var tagElements = cut.FindAll(".tag-cloud-item");
            
            // Should have all 5 tags: 2 selected + 3 popular
            tagElements.Should().HaveCount(5, "should show selected tags + popular tags");
            
            // First two tags should be the selected ones (Security and .NET)
            var tagTexts = tagElements.Select(t => t.TextContent).ToList();
            tagTexts[0].Should().Contain("Security", "first selected tag should appear first");
            tagTexts[1].Should().Contain(".NET", "second selected tag should appear second");
            
            // Popular tags should appear after selected tags
            tagTexts.Skip(2).Should().Contain(t => t.Contains("AI"));
            tagTexts.Skip(2).Should().Contain(t => t.Contains("Python"));
            tagTexts.Skip(2).Should().Contain(t => t.Contains("Azure"));
        });
    }

    [Fact]
    public void SidebarTagCloud_SelectedTagWithZeroCount_IsClickableForDeselection()
    {
        // Arrange - Selected tag has 0 count (no results for current filters)
        var popularTags = new List<TagCloudItem>
        {
            new TagCloudItem { Tag = "AI", Count = 50, Size = TagSize.Large },
            new TagCloudItem { Tag = "Azure", Count = 30, Size = TagSize.Medium }
        };
        var selectedTagWithZeroCount = new List<TagCloudItem>
        {
            new TagCloudItem { Tag = "Security", Count = 0, Size = TagSize.Small } // No results!
        };

        // Setup: Popular tags
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.Is<List<string>>(tags => tags.Contains("security")),
                null,
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(popularTags);

        // Setup: Selected tag count (0 results)
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.Is<List<string>>(tags => tags.Contains("security")),
                It.Is<List<string>?>(tags => tags != null && tags.Count > 0),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(selectedTagWithZeroCount);

        // Setup: After deselection, return just popular tags
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                null, // No selected tags
                null,
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(popularTags);

        // Act - Render with selected tag that has 0 count
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all")
            .Add(p => p.SelectedTags, new List<string> { "Security" }));

        // Assert - Selected tag with 0 count should be visible and NOT disabled
        cut.WaitForAssertion(() =>
        {
            var tagElements = cut.FindAll(".tag-cloud-item");
            
            // Security tag should be present
            var securityTag = tagElements.FirstOrDefault(t => t.TextContent.Contains("Security"));
            securityTag.Should().NotBeNull("selected tag with 0 count must be visible");
            
            // Should be selected
            securityTag!.ClassList.Should().Contain("selected");
            
            // Should NOT be disabled (so it can be clicked to deselect)
            securityTag.HasAttribute("disabled").Should().BeFalse(
                "selected tags with 0 count should be clickable for deselection");
            securityTag.ClassList.Should().NotContain("disabled");
        });

        // Act - Click to deselect the zero-count tag
        var securityButton = cut.FindAll(".tag-cloud-item")
            .First(t => t.TextContent.Contains("Security"));
        securityButton.Click();

        // Assert - Tag should disappear after deselection and reload
        cut.WaitForAssertion(() =>
        {
            var tagElements = cut.FindAll(".tag-cloud-item");
            tagElements.Should().NotContain(t => t.TextContent.Contains("Security"),
                "deselected tag with 0 count should disappear");
        });
    }

    /// <summary>
    /// Verifies that the component always passes lastDays=null to the API,
    /// letting the API apply its configured default date range (90 days).
    /// Date range filtering is handled via explicit FromDate/ToDate parameters instead.
    /// </summary>
    [Fact]
    public void SidebarTagCloud_AlwaysPassesNullLastDays_ToLetApiApplyDefault()
    {
        // Arrange
        var tags = CreateTestTagCloud();
        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        // Act - Render without date params (simulates homepage usage)
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        // Assert - Verify lastDays is null (not 0) so API uses its default (90 days)
        cut.WaitForAssertion(() => cut.FindAll(".tag-cloud-item").Should().HaveCount(5));
        _mockApiClient.Verify(x => x.GetTagCloudAsync(
            "all",
            "all",
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            null, // lastDays must be null — API applies 90-day default
            It.IsAny<List<string>?>(),
            It.IsAny<List<string>?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    /// <summary>
    /// Creates test tag cloud data
    /// </summary>
    private static List<TagCloudItem> CreateTestTagCloud()
    {
        return
        [
            new TagCloudItem { Tag = "AI", Count = 150, Size = TagSize.Large },
            new TagCloudItem { Tag = "Azure", Count = 120, Size = TagSize.Large },
            new TagCloudItem { Tag = "GitHub Copilot", Count = 100, Size = TagSize.Medium },
            new TagCloudItem { Tag = ".NET", Count = 80, Size = TagSize.Medium },
            new TagCloudItem { Tag = "Security", Count = 50, Size = TagSize.Small }
        ];
    }

    [Fact]
    public void SidebarTagCloud_WithTags_FetchesRealCountsFromApi()
    {
        // Arrange - Tags parameter provides content item's tags
        var contentItemTags = new List<string> { "AI", "GitHub Copilot", "Security" };
        var apiResponse = new List<TagCloudItem>
        {
            new TagCloudItem { Tag = "AI", Count = 42, Size = TagSize.Large },
            new TagCloudItem { Tag = "GitHub Copilot", Count = 28, Size = TagSize.Medium },
            new TagCloudItem { Tag = "Security", Count = 15, Size = TagSize.Small },
            new TagCloudItem { Tag = "Azure", Count = 120, Size = TagSize.Large }
        };

        _mockApiClient.Setup(x => x.GetTagCloudAsync(
                "github-copilot",  // section name
                "all",             // collection name
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.Is<List<string>?>(t => t != null && t.Count == 3), // tagsToCount should have the 3 tags
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.Tags, (IReadOnlyList<string>)contentItemTags)
            .Add(p => p.SectionName, "github-copilot"));

        // Assert - Should show real counts from API plus popular fill tags
        cut.WaitForAssertion(() =>
        {
            var tagButtons = cut.FindAll(".tag-cloud-item");
            tagButtons.Should().HaveCount(4); // 3 content item tags + 1 popular tag

            // Verify real counts are displayed (not count=1)
            tagButtons[0].TextContent.Should().Contain("42");
        });

        // Verify API was called with tagsToCount parameter
        _mockApiClient.Verify(x => x.GetTagCloudAsync(
            "github-copilot",
            "all",
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<List<string>?>(),
            It.Is<List<string>?>(t => t != null && t.SequenceEqual(contentItemTags)),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void SidebarTagCloud_WithTags_NoSection_FallsBackToStaticDisplay()
    {
        // Arrange - Tags parameter provided but NO SectionName
        // This is the fallback case where we can't fetch from API
        var contentItemTags = new List<string> { "AI", "Security" };

        // Act
        var cut = Render<SidebarTagCloud>(parameters => parameters
            .Add(p => p.Tags, (IReadOnlyList<string>)contentItemTags));

        // Assert - Should show tags with count=1 (fallback behavior)
        cut.WaitForAssertion(() =>
        {
            var tagButtons = cut.FindAll(".tag-cloud-item");
            tagButtons.Should().HaveCount(2);

            // Should display count=1 for each tag
            tagButtons[0].TextContent.Should().Contain("1");
            tagButtons[1].TextContent.Should().Contain("1");
        });

        // Verify NO API call was made
        _mockApiClient.Verify(x => x.GetTagCloudAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<List<string>?>(),
            It.IsAny<List<string>?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
}
