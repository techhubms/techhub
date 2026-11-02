using Bunit;
using FluentAssertions;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for TimelineView.razor - the year / month / card timeline shared by the section timelines and the GitHub Copilot features page.
/// </summary>
public class TimelineViewTests : BunitContext
{
    public TimelineViewTests()
    {
        JSInterop.SetupVoid("preventScrollOnExpand").SetVoidResult();
    }

    private static TimelineViewEntry Entry(string id, string title, DateOnly? date, string? description = "Description", params TimelineViewBadge[] badges) => new()
    {
        Id = id,
        Title = title,
        Date = date,
        Description = description,
        Badges = badges
    };

    private IRenderedComponent<TimelineView> RenderTimeline(IReadOnlyList<TimelineViewEntry> entries, bool withDetail = false, bool expandFirst = false) =>
        Render<TimelineView>(parameters =>
        {
            parameters.Add(p => p.Entries, entries);
            parameters.Add(p => p.ExpandFirst, expandFirst);
            if (withDetail)
            {
                parameters.Add(p => p.Detail, (TimelineViewEntry entry) => $"<span class=\"test-detail\">detail of {entry.Id}</span>");
            }
        });

    [Fact]
    public void Render_WithoutEntries_ShowsTheEmptyText()
    {
        var cut = Render<TimelineView>(p => p.Add(x => x.Entries, []).Add(x => x.EmptyText, "No dates yet."));

        cut.Find(".timeline-empty").TextContent.Should().Be("No dates yet.");
        cut.FindAll(".timeline-entry").Should().BeEmpty();
    }

    [Fact]
    public void Render_GroupsEntriesByYearAndMonth_NewestFirst_AndUndatedLast()
    {
        var cut = RenderTimeline(
        [
            Entry("a", "March 2026 first", new DateOnly(2026, 3, 20)),
            Entry("b", "March 2026 second", new DateOnly(2026, 3, 2)),
            Entry("c", "January 2026", new DateOnly(2026, 1, 5)),
            Entry("d", "Late 2025", new DateOnly(2025, 12, 1)),
            Entry("e", "Undated", null)
        ]);

        cut.FindAll(".timeline-year-label").Select(h => h.TextContent).Should().Equal("2026", "2025", "Date TBA");
        cut.FindAll(".timeline-month-row").Should().HaveCount(4);
        cut.FindAll(".timeline-date").Select(t => t.TextContent).Should().Equal("Mar", "Jan", "Dec");
        cut.FindAll(".timeline-date")[0].GetAttribute("datetime").Should().Be("2026-03-01");
        cut.FindAll(".timeline-name").Select(h => h.TextContent).Should().Equal("March 2026 first", "March 2026 second", "January 2026", "Late 2025", "Undated");
    }

    [Fact]
    public void Render_ShowsTheTitleBadgesAndDateLabel()
    {
        var entry = Entry("a", "Agent mode", new DateOnly(2026, 3, 10), "It ships.", new TimelineViewBadge("GA", "badge-success"), new TimelineViewBadge("Pro"));
        entry = entry with
        {
            DateLabel = "10 Mar 2026"
        };

        var cut = RenderTimeline([entry]);

        cut.Find(".timeline-name").TextContent.Should().Be("Agent mode");
        cut.Find(".timeline-entry-date").TextContent.Should().Be("10 Mar 2026");
        cut.Find(".timeline-badges .badge-success").TextContent.Should().Be("GA");
        cut.Find(".timeline-badges .badge-info").TextContent.Should().Be("Pro");
        cut.Find(".timeline-description").TextContent.Should().Be("It ships.");
    }

    [Fact]
    public void Render_WithoutADescription_HasNoDescriptionElement()
    {
        var cut = RenderTimeline([Entry("a", "Title", new DateOnly(2026, 3, 10), description: null)]);

        cut.FindAll(".timeline-description").Should().BeEmpty();
    }

    [Fact]
    public void Click_ExpandsTheCard_AndCollapsesItAgain()
    {
        var cut = RenderTimeline([Entry("a", "First", new DateOnly(2026, 3, 10))]);
        var card = cut.Find(".timeline-card");
        card.GetAttribute("aria-expanded").Should().Be("false");

        card.Click();
        cut.WaitForAssertion(() => cut.Find(".timeline-card").GetAttribute("aria-expanded").Should().Be("true"));
        cut.Find(".timeline-entry").ClassList.Should().Contain("expanded");

        cut.Find(".timeline-card").Click();
        cut.WaitForAssertion(() => cut.Find(".timeline-card").GetAttribute("aria-expanded").Should().Be("false"));
    }

    [Fact]
    public void Click_OpeningACardCollapsesTheOtherOne()
    {
        var cut = RenderTimeline([Entry("a", "First", new DateOnly(2026, 3, 10)), Entry("b", "Second", new DateOnly(2026, 2, 10))]);

        cut.FindAll(".timeline-card")[0].Click();
        cut.WaitForAssertion(() => cut.FindAll(".timeline-card")[0].GetAttribute("aria-expanded").Should().Be("true"));
        cut.FindAll(".timeline-card")[1].Click();

        cut.WaitForAssertion(() => cut.FindAll(".timeline-card").Select(c => c.GetAttribute("aria-expanded")).Should().Equal("false", "true"));
    }

    [Theory]
    [InlineData("Enter")]
    [InlineData(" ")]
    public void KeyDown_EnterOrSpace_TogglesTheCard(string key)
    {
        var cut = RenderTimeline([Entry("a", "First", new DateOnly(2026, 3, 10))]);

        cut.Find(".timeline-card").KeyDown(new Microsoft.AspNetCore.Components.Web.KeyboardEventArgs { Key = key });

        cut.WaitForAssertion(() => cut.Find(".timeline-card").GetAttribute("aria-expanded").Should().Be("true"));
    }

    [Fact]
    public void KeyDown_OtherKeys_DoNothing()
    {
        var cut = RenderTimeline([Entry("a", "First", new DateOnly(2026, 3, 10))]);

        cut.Find(".timeline-card").KeyDown(new Microsoft.AspNetCore.Components.Web.KeyboardEventArgs { Key = "a" });

        cut.Find(".timeline-card").GetAttribute("aria-expanded").Should().Be("false");
    }

    [Fact]
    public void Detail_IsOnlyRenderedAfterTheCardWasOpened_AndStaysAfterCollapse()
    {
        var cut = RenderTimeline([Entry("a", "First", new DateOnly(2026, 3, 10))], withDetail: true);
        cut.FindAll(".test-detail").Should().BeEmpty("the detail of a card that was never opened is not rendered");

        cut.Find(".timeline-card").Click();
        cut.WaitForAssertion(() => cut.Find(".test-detail").TextContent.Should().Be("detail of a"));

        cut.Find(".timeline-card").Click();
        cut.WaitForAssertion(() => cut.Find(".timeline-card").GetAttribute("aria-expanded").Should().Be("false"));
        cut.FindAll(".test-detail").Should().ContainSingle("the detail stays while the card collapses");
    }

    [Fact]
    public void ExpandFirst_OpensTheNewestCardWithItsDetail()
    {
        var cut = RenderTimeline([Entry("old", "Older", new DateOnly(2026, 1, 1)), Entry("new", "Newer", new DateOnly(2026, 4, 1))], withDetail: true, expandFirst: true);

        cut.FindAll(".timeline-card").Select(c => c.GetAttribute("aria-expanded")).Should().Equal("true", "false");
        cut.Find(".test-detail").TextContent.Should().Be("detail of new");
    }

    [Fact]
    public void Render_ExposesStableIdsForTheCardAndItsDetail()
    {
        var cut = RenderTimeline([Entry("agent-mode", "Agent mode", new DateOnly(2026, 3, 10))]);

        cut.Find("#timeline-entry-agent-mode").Should().NotBeNull();
        cut.Find(".timeline-card").GetAttribute("aria-controls").Should().Be("timeline-detail-agent-mode");
        cut.Find("#timeline-detail-agent-mode").Should().NotBeNull();
    }
}
