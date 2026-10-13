using Bunit;
using FluentAssertions;
using TechHub.Web.Components;
using TechHub.Web.Helpers;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for ReviewDiff.razor — line diff rendering with prefixes, per-operation CSS classes,
/// HTML-encoded text and visible trailing-whitespace markers on changed lines.
/// </summary>
public class ReviewDiffTests : BunitContext
{
    [Fact]
    public void Render_NoLines_RendersEmptyAccessibleContainer()
    {
        // Act
        var cut = RenderLines([]);

        // Assert
        var view = cut.Find(".diff-view");
        view.GetAttribute("role").Should().Be("region");
        view.GetAttribute("aria-label").Should().Be("Diff view");
        cut.FindAll(".diff-line").Should().BeEmpty();
    }

    [Fact]
    public void Render_Lines_RendersOneRowPerLineInOrderWithOperationClassAndPrefix()
    {
        // Arrange
        var lines = new[]
        {
            new DiffLine(DiffOp.Equal, "same"),
            new DiffLine(DiffOp.Delete, "gone"),
            new DiffLine(DiffOp.Insert, "new")
        };

        // Act
        var cut = RenderLines(lines);

        // Assert
        var rows = cut.FindAll(".diff-line");
        rows.Should().HaveCount(3);
        rows[0].ClassList.Should().Contain("diff-line-equal");
        rows[1].ClassList.Should().Contain("diff-line-delete");
        rows[2].ClassList.Should().Contain("diff-line-insert");
        rows.Select(r => r.QuerySelector(".diff-prefix")!.TextContent).Should().Equal(" ", "-", "+");
        rows.Select(r => r.QuerySelector(".diff-text")!.TextContent).Should().Equal("same", "gone", "new");
    }

    [Fact]
    public void Render_Prefix_IsHiddenFromAssistiveTechnology()
    {
        // Act
        var cut = RenderLines([new DiffLine(DiffOp.Insert, "x")]);

        // Assert
        cut.Find(".diff-prefix").GetAttribute("aria-hidden").Should().Be("true");
    }

    [Theory]
    [InlineData(DiffOp.Equal)]
    [InlineData(DiffOp.Insert)]
    [InlineData(DiffOp.Delete)]
    public void Render_HtmlInText_IsEncodedAndNeverInterpretedAsMarkup(DiffOp op)
    {
        // Arrange
        var text = "<script>alert('x')</script> & <b>bold</b>";

        // Act
        var cut = RenderLines([new DiffLine(op, text)]);

        // Assert
        cut.FindAll("script").Should().BeEmpty();
        cut.FindAll(".diff-text b").Should().BeEmpty();
        cut.Find(".diff-text").TextContent.Should().Be(text);
        cut.Find(".diff-text").InnerHtml.Should().Contain("&lt;script&gt;").And.Contain("&amp;");
    }

    [Fact]
    public void Render_AttributeBreakoutAttempt_StaysInsideTextNode()
    {
        // Arrange
        var text = "\"><img src=x onerror=alert(1)>";

        // Act
        var cut = RenderLines([new DiffLine(DiffOp.Insert, text)]);

        // Assert
        cut.FindAll("img").Should().BeEmpty();
        cut.Find(".diff-text").TextContent.Should().Be(text);
    }

    [Fact]
    public void Render_InsertedLineWithTrailingSpaces_ShowsMiddleDotMarkers()
    {
        // Act
        var cut = RenderLines([new DiffLine(DiffOp.Insert, "text   ")]);

        // Assert
        var text = cut.Find(".diff-text");
        text.TextContent.Should().Be("text\u00b7\u00b7\u00b7");
        text.QuerySelector(".diff-trailing-ws")!.TextContent.Should().Be("\u00b7\u00b7\u00b7");
    }

    [Fact]
    public void Render_DeletedLineWithTrailingSpace_ShowsSingleMarker()
    {
        // Act
        var cut = RenderLines([new DiffLine(DiffOp.Delete, "text ")]);

        // Assert
        cut.Find(".diff-trailing-ws").TextContent.Should().Be("\u00b7");
    }

    [Fact]
    public void Render_ChangedLineWithoutTrailingSpaces_HasNoMarker()
    {
        // Act
        var cut = RenderLines([new DiffLine(DiffOp.Insert, "  leading only")]);

        // Assert
        cut.FindAll(".diff-trailing-ws").Should().BeEmpty();
        cut.Find(".diff-text").TextContent.Should().Be("  leading only");
    }

    [Fact]
    public void Render_ChangedWhitespaceOnlyLine_IsRenderedAsMarkersOnly()
    {
        // Act
        var cut = RenderLines([new DiffLine(DiffOp.Insert, "    ")]);

        // Assert
        cut.Find(".diff-text").TextContent.Should().Be("\u00b7\u00b7\u00b7\u00b7");
    }

    [Fact]
    public void Render_EqualLineWithTrailingSpaces_DoesNotShowMarkers()
    {
        // Act
        var cut = RenderLines([new DiffLine(DiffOp.Equal, "text   ")]);

        // Assert
        cut.FindAll(".diff-trailing-ws").Should().BeEmpty();
        cut.Find(".diff-text").InnerHtml.Should().Be("text   ");
    }

    [Fact]
    public void Render_TrailingSpacesWithHtmlBody_EncodesBodyAndStillMarksWhitespace()
    {
        // Act
        var cut = RenderLines([new DiffLine(DiffOp.Delete, "<b>x</b>  ")]);

        // Assert
        cut.FindAll(".diff-text b").Should().BeEmpty();
        cut.Find(".diff-text").TextContent.Should().Be("<b>x</b>\u00b7\u00b7");
    }

    [Fact]
    public void Render_TrailingTabs_AreNotMarkedAsWhitespace()
    {
        // Act
        var cut = RenderLines([new DiffLine(DiffOp.Insert, "text\t")]);

        // Assert
        cut.FindAll(".diff-trailing-ws").Should().BeEmpty("only trailing spaces are replaced by markers");
    }

    [Fact]
    public void Render_EmptyText_RendersEmptyTextSpan()
    {
        // Act
        var cut = RenderLines([new DiffLine(DiffOp.Insert, string.Empty)]);

        // Assert
        cut.Find(".diff-text").TextContent.Should().BeEmpty();
    }

    [Fact]
    public void Render_VeryLongLine_IsRenderedInFullWithoutTruncation()
    {
        // Arrange
        var text = new string('x', 5000);

        // Act
        var cut = RenderLines([new DiffLine(DiffOp.Equal, text)]);

        // Assert
        cut.Find(".diff-text").TextContent.Should().HaveLength(5000).And.NotContain("…").And.NotContain("...");
    }

    [Fact]
    public void Render_ManyLines_RendersAllOfThemWithoutTruncation()
    {
        // Arrange
        var lines = Enumerable.Range(0, 500).Select(i => new DiffLine(DiffOp.Equal, $"line {i}")).ToList();

        // Act
        var cut = RenderLines(lines);

        // Assert
        cut.FindAll(".diff-line").Should().HaveCount(500);
        cut.FindAll(".diff-text").Select(e => e.TextContent).Last().Should().Be("line 499");
    }

    [Fact]
    public void Render_LinesFromDiffHelper_ReflectsComputedDiff()
    {
        // Arrange
        var lines = DiffHelper.DiffLines("a\nb\nc", "a\nc\nd");

        // Act
        var cut = RenderLines(lines);

        // Assert
        cut.FindAll(".diff-line-delete .diff-text").Select(e => e.TextContent).Should().Equal("b");
        cut.FindAll(".diff-line-insert .diff-text").Select(e => e.TextContent).Should().Equal("d");
        cut.FindAll(".diff-line-equal .diff-text").Select(e => e.TextContent).Should().Equal("a", "c");
    }

    private IRenderedComponent<ReviewDiff> RenderLines(IReadOnlyList<DiffLine> lines) =>
        Render<ReviewDiff>(p => p.Add(x => x.Lines, lines));
}
