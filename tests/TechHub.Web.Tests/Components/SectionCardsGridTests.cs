using Bunit;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for SectionCardsGrid.razor — the "Browse Sections" grid composed of <see cref="SectionCard"/> components.
/// </summary>
public class SectionCardsGridTests : BunitContext
{
    [Fact]
    public void Render_WithSections_RendersOneCardPerSectionInGivenOrder()
    {
        // Arrange
        var sections = new[]
        {
            A.Section.WithName("ai").WithTitle("AI").WithUrl("/ai").Build(),
            A.Section.WithName("azure").WithTitle("Azure").WithUrl("/azure").Build(),
            A.Section.WithName("dotnet").WithTitle(".NET").WithUrl("/dotnet").Build()
        };

        // Act
        var cut = RenderGrid(sections);

        // Assert
        cut.FindComponents<SectionCard>().Should().HaveCount(3);
        cut.FindAll(".section-card-container").Should().HaveCount(3);
        cut.FindAll(".section-card h2").Select(h => h.TextContent).Should().Equal("AI", "Azure", ".NET");
        cut.FindAll("a.section-card").Select(a => a.GetAttribute("href")).Should().Equal("/ai", "/azure", "/dotnet");
    }

    [Fact]
    public void Render_Always_ShowsSkipTargetHeadingInsideLabelledSection()
    {
        // Act
        var cut = RenderGrid([A.Section.Build()]);

        // Assert
        var heading = cut.Find("h1");
        heading.Id.Should().Be("skiptohere");
        heading.TextContent.Should().Be("Browse Sections");
        heading.ClassList.Should().Contain("page-h1");
        cut.Find("section.sections-grid").GetAttribute("aria-label").Should().Be("Browse sections");
    }

    [Fact]
    public void Render_Cards_AreWrappedInListGrid()
    {
        // Act
        var cut = RenderGrid([A.Section.Build()]);

        // Assert
        var grid = cut.Find(".grid");
        grid.GetAttribute("role").Should().Be("list");
        grid.QuerySelectorAll(".section-card-container").Should().HaveCount(1);
    }

    [Fact]
    public void Render_NoSections_RendersHeadingAndEmptyGrid()
    {
        // Act
        var cut = RenderGrid([]);

        // Assert
        cut.Find("h1").TextContent.Should().Be("Browse Sections");
        cut.FindAll(".section-card-container").Should().BeEmpty();
    }

    [Fact]
    public void Render_SectionWithCollections_PassesCollectionBadgesThroughToCard()
    {
        // Arrange
        var section = A.Section
            .WithName("ai")
            .WithUrl("/ai")
            .WithCollections(new Collection("news", "News", "/ai/news", "News items", "News"))
            .Build();

        // Act
        var cut = RenderGrid([section]);

        // Assert
        cut.Find(".section-collections a.badge-purple").GetAttribute("href").Should().Be("/ai/news");
    }

    [Fact]
    public void Render_SectionsChanged_ReplacesCards()
    {
        // Arrange
        var cut = RenderGrid([A.Section.WithName("ai").WithTitle("AI").WithUrl("/ai").Build()]);

        // Act
        cut.Render(p => p.Add(x => x.Sections, [A.Section.WithName("ml").WithTitle("ML").WithUrl("/ml").Build()]));

        // Assert
        cut.FindAll(".section-card h2").Select(h => h.TextContent).Should().Equal("ML");
    }

    private IRenderedComponent<SectionCardsGrid> RenderGrid(IEnumerable<Section> sections) =>
        Render<SectionCardsGrid>(p => p.Add(x => x.Sections, sections));
}
