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
}
