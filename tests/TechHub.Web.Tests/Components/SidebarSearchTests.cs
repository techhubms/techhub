using Bunit;
using FluentAssertions;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for SidebarSearch.razor component
/// 
/// REQUIREMENTS (from spec 002-search):
/// - Test search box renders with placeholder text
/// - Test typing updates search query state
/// - Test clear button appears when query has text
/// - Test clear button removes search query
/// - Test Escape key clears search query
/// - Test component emits search query changes
/// - Test URL parameter initializes search query
/// </summary>
public class SidebarSearchTests : BunitContext
{

    [Fact]
    public void SidebarSearch_RendersSearchInputWithPlaceholder()
    {
        // Arrange & Act
        var cut = Render<SidebarSearch>();

        // Assert
        var searchInput = cut.Find("input[type='search']");
        searchInput.Should().NotBeNull("Search input should be rendered");
        
        var placeholder = searchInput.GetAttribute("placeholder");
        placeholder.Should().NotBeNullOrEmpty("Search input should have placeholder text");
        placeholder.Should().Contain("Search", "Placeholder should indicate search functionality");
    }

    [Fact]
    public void SidebarSearch_HasAriaLabel_ForAccessibility()
    {
        // Arrange & Act
        var cut = Render<SidebarSearch>();

        // Assert
        var searchInput = cut.Find("input[type='search']");
        var ariaLabel = searchInput.GetAttribute("aria-label");
        ariaLabel.Should().NotBeNullOrEmpty("Search input should have aria-label for accessibility");
    }

    [Fact]
    public void SidebarSearch_InitializesWithEmptyQuery()
    {
        // Arrange & Act
        var cut = Render<SidebarSearch>();

        // Assert
        var searchInput = cut.Find("input[type='search']");
        var value = searchInput.GetAttribute("value");
        value.Should().BeNullOrEmpty("Search input should be empty initially");
    }

    [Fact]
    public void SidebarSearch_InitializesWithQueryFromParameter()
    {
        // Arrange & Act
        var cut = Render<SidebarSearch>(parameters => parameters
            .Add(p => p.SearchQuery, "test query"));

        // Assert
        var searchInput = cut.Find("input[type='search']");
        var value = searchInput.GetAttribute("value");
        value.Should().Be("test query", "Search input should display query from parameter");
    }

    [Fact]
    public void SidebarSearch_ShowsClearButton_WhenQueryHasText()
    {
        // Arrange
        var cut = Render<SidebarSearch>(parameters => parameters
            .Add(p => p.SearchQuery, "test"));

        // Assert
        var clearButton = cut.Find("button[aria-label*='Clear']");
        clearButton.Should().NotBeNull("Clear button should be visible when query has text");
    }

    [Fact]
    public void SidebarSearch_HidesClearButton_WhenQueryIsEmpty()
    {
        // Arrange
        var cut = Render<SidebarSearch>(parameters => parameters
            .Add(p => p.SearchQuery, ""));

        // Assert
        var clearButtons = cut.FindAll("button[aria-label*='Clear']");
        clearButtons.Should().BeEmpty("Clear button should be hidden when query is empty");
    }

    // Note: Debouncing behavior is tested in E2E tests (SearchTests.cs)
    // Unit testing timer-based debouncing is problematic due to threading issues

    [Fact]
    public void SidebarSearch_ClearButton_RemovesQuery()
    {
        // Arrange
        string? capturedQuery = null;
        var cut = Render<SidebarSearch>(parameters => parameters
            .Add(p => p.SearchQuery, "test query")
            .Add(p => p.OnSearchQueryChanged, (string query) =>
            {
                capturedQuery = query;
            }));

        // Act - Click clear button
        var clearButton = cut.Find("button[aria-label*='Clear']");
        clearButton.Click();

        // Assert - Query should be cleared
        var searchInput = cut.Find("input[type='search']");
        var value = searchInput.GetAttribute("value");
        value.Should().BeNullOrEmpty("Search input should be empty after clicking clear");

        // Event should be emitted with empty string
        capturedQuery.Should().BeEmpty("OnSearchQueryChanged should emit empty string when cleared");
    }

    [Fact]
    public void SidebarSearch_EscapeKey_ClearsQuery()
    {
        // Arrange
        string? capturedQuery = null;
        var cut = Render<SidebarSearch>(parameters => parameters
            .Add(p => p.SearchQuery, "test query")
            .Add(p => p.OnSearchQueryChanged, (string query) =>
            {
                capturedQuery = query;
            }));

        // Act - Press Escape in search box
        var searchInput = cut.Find("input[type='search']");
        searchInput.KeyDown("Escape");

        // Assert - Query should be cleared
        cut.WaitForAssertion(() =>
        {
            var value = searchInput.GetAttribute("value");
            value.Should().BeNullOrEmpty("Search input should be empty after pressing Escape");
            capturedQuery.Should().BeEmpty("OnSearchQueryChanged should emit empty string when Escape is pressed");
        });
    }

    [Fact]
    public void SidebarSearch_MaxLength_LimitsInputTo100Characters()
    {
        // Arrange & Act
        var cut = Render<SidebarSearch>();

        // Assert
        var searchInput = cut.Find("input[type='search']");
        var maxLength = searchInput.GetAttribute("maxlength");
        maxLength.Should().Be("100", "Search input should have maxlength of 100 characters");
    }

    [Fact]
    public void SidebarSearch_RendersInSidebarSection()
    {
        // Arrange & Act
        var cut = Render<SidebarSearch>();

        // Assert
        var sidebarSection = cut.Find("nav.sidebar-section");
        sidebarSection.Should().NotBeNull("Component should render in a sidebar-section");
        
        var heading = cut.Find("h2.sidebar-h2");
        heading.Should().NotBeNull("Sidebar section should have a heading");
        heading.TextContent.Should().Contain("Search", "Heading should indicate search functionality");
    }
}
