using FluentAssertions;
using TechHub.Web.Helpers;

namespace TechHub.Web.Tests.Helpers;

/// <summary>
/// Tests for <see cref="DiffHelper"/> — LCS line diff and tags CSV diff used by the admin reviews page.
/// </summary>
public class DiffHelperTests
{
    // ── DiffLines ───────────────────────────────────────────────────────────

    [Fact]
    public void DiffLines_IdenticalText_ReturnsOnlyEqualLinesInOrder()
    {
        // Arrange
        var text = "one\ntwo\nthree";

        // Act
        var result = DiffHelper.DiffLines(text, text);

        // Assert
        result.Should().Equal(
            new DiffLine(DiffOp.Equal, "one"),
            new DiffLine(DiffOp.Equal, "two"),
            new DiffLine(DiffOp.Equal, "three"));
    }

    [Fact]
    public void DiffLines_BothEmpty_ReturnsSingleEqualEmptyLine()
    {
        // Act
        var result = DiffHelper.DiffLines(string.Empty, string.Empty);

        // Assert
        result.Should().Equal(new DiffLine(DiffOp.Equal, string.Empty));
    }

    [Fact]
    public void DiffLines_AppendedLine_ReportsInsertAfterEqualLines()
    {
        // Act
        var result = DiffHelper.DiffLines("a\nb", "a\nb\nc");

        // Assert
        result.Should().Equal(
            new DiffLine(DiffOp.Equal, "a"),
            new DiffLine(DiffOp.Equal, "b"),
            new DiffLine(DiffOp.Insert, "c"));
    }

    [Fact]
    public void DiffLines_PrependedLine_ReportsInsertBeforeEqualLines()
    {
        // Act
        var result = DiffHelper.DiffLines("b\nc", "a\nb\nc");

        // Assert
        result.Should().Equal(
            new DiffLine(DiffOp.Insert, "a"),
            new DiffLine(DiffOp.Equal, "b"),
            new DiffLine(DiffOp.Equal, "c"));
    }

    [Fact]
    public void DiffLines_RemovedLine_ReportsDeleteBetweenEqualLines()
    {
        // Act
        var result = DiffHelper.DiffLines("a\nb\nc", "a\nc");

        // Assert
        result.Should().Equal(
            new DiffLine(DiffOp.Equal, "a"),
            new DiffLine(DiffOp.Delete, "b"),
            new DiffLine(DiffOp.Equal, "c"));
    }

    [Fact]
    public void DiffLines_ChangedLine_ReportsBothDeleteAndInsertAroundEqualLines()
    {
        // Act
        var result = DiffHelper.DiffLines("a\nold\nc", "a\nnew\nc");

        // Assert
        result.Should().Equal(
            new DiffLine(DiffOp.Equal, "a"),
            new DiffLine(DiffOp.Insert, "new"),
            new DiffLine(DiffOp.Delete, "old"),
            new DiffLine(DiffOp.Equal, "c"));
    }

    [Fact]
    public void DiffLines_CompletelyDifferent_ReportsAllDeletesAndAllInserts()
    {
        // Act
        var result = DiffHelper.DiffLines("a\nb", "x\ny\nz");

        // Assert
        result.Count(l => l.Op == DiffOp.Equal).Should().Be(0);
        result.Where(l => l.Op == DiffOp.Delete).Select(l => l.Text).Should().Equal("a", "b");
        result.Where(l => l.Op == DiffOp.Insert).Select(l => l.Text).Should().Equal("x", "y", "z");
    }

    [Fact]
    public void DiffLines_EmptyOriginal_InsertsAllModifiedLines()
    {
        // Act
        var result = DiffHelper.DiffLines(string.Empty, "a\nb");

        // Assert
        result.Should().Equal(
            new DiffLine(DiffOp.Insert, "a"),
            new DiffLine(DiffOp.Insert, "b"));
    }

    [Fact]
    public void DiffLines_EmptyModified_DeletesAllOriginalLines()
    {
        // Act
        var result = DiffHelper.DiffLines("a\nb", string.Empty);

        // Assert
        result.Should().Equal(
            new DiffLine(DiffOp.Delete, "a"),
            new DiffLine(DiffOp.Delete, "b"));
    }

    [Fact]
    public void DiffLines_DuplicateLines_PreservesMultiplicity()
    {
        // Act
        var result = DiffHelper.DiffLines("a\na\na", "a\na");

        // Assert
        result.Count(l => l.Op == DiffOp.Equal).Should().Be(2);
        result.Count(l => l.Op == DiffOp.Delete).Should().Be(1);
        result.Count(l => l.Op == DiffOp.Insert).Should().Be(0);
    }

    [Fact]
    public void DiffLines_TrailingNewlineRemoved_ReportsEmptyLineDeleted()
    {
        // Act
        var result = DiffHelper.DiffLines("a\n", "a");

        // Assert
        result.Should().Equal(
            new DiffLine(DiffOp.Equal, "a"),
            new DiffLine(DiffOp.Delete, string.Empty));
    }

    [Theory]
    [InlineData("a\r\nb\r\nc", "a\nb\nc")]
    [InlineData("a\rb\rc", "a\nb\nc")]
    [InlineData("a\r\nb\nc\r", "a\nb\r\nc\n")]
    public void DiffLines_DifferentLineEndingsOnly_ReportsNoChanges(string original, string modified)
    {
        // Act
        var result = DiffHelper.DiffLines(original, modified);

        // Assert
        result.Should().OnlyContain(l => l.Op == DiffOp.Equal);
    }

    [Fact]
    public void DiffLines_WindowsLineEndings_DoesNotLeakCarriageReturnsIntoText()
    {
        // Act
        var result = DiffHelper.DiffLines("a\r\nb", "a\r\nc");

        // Assert
        result.Should().OnlyContain(l => !l.Text.Contains('\r'));
    }

    [Fact]
    public void DiffLines_WhitespaceDifferences_AreTreatedAsChanges()
    {
        // Act
        var result = DiffHelper.DiffLines("a ", "a");

        // Assert
        result.Should().Contain(new DiffLine(DiffOp.Delete, "a "));
        result.Should().Contain(new DiffLine(DiffOp.Insert, "a"));
    }

    [Fact]
    public void DiffLines_CaseDifferences_AreTreatedAsChanges()
    {
        // Act
        var result = DiffHelper.DiffLines("Hello", "hello");

        // Assert
        result.Should().Contain(new DiffLine(DiffOp.Delete, "Hello"));
        result.Should().Contain(new DiffLine(DiffOp.Insert, "hello"));
    }

    [Fact]
    public void DiffLines_LargeInputWithSingleChange_ReportsOnlyThatChange()
    {
        // Arrange
        var original = Enumerable.Range(0, 1000).Select(i => $"line {i}").ToArray();
        var modified = (string[])original.Clone();
        modified[500] = "changed";

        // Act
        var result = DiffHelper.DiffLines(string.Join('\n', original), string.Join('\n', modified));

        // Assert
        result.Count(l => l.Op == DiffOp.Equal).Should().Be(999);
        result.Should().ContainSingle(l => l.Op == DiffOp.Delete).Which.Text.Should().Be("line 500");
        result.Should().ContainSingle(l => l.Op == DiffOp.Insert).Which.Text.Should().Be("changed");
    }

    [Fact]
    public void DiffLines_LargeIdenticalInput_ReturnsEveryLineAsEqual()
    {
        // Arrange
        var text = string.Join('\n', Enumerable.Range(0, 1500).Select(i => $"line {i}"));

        // Act
        var result = DiffHelper.DiffLines(text, text);

        // Assert
        result.Should().HaveCount(1500);
        result.Should().OnlyContain(l => l.Op == DiffOp.Equal);
    }

    [Fact]
    public void DiffLines_ResultReplayed_ReconstructsBothInputs()
    {
        // Arrange
        var original = "intro\nkeep 1\nremove me\nkeep 2\noutro";
        var modified = "intro\nadded\nkeep 1\nkeep 2\nextra\noutro";

        // Act
        var result = DiffHelper.DiffLines(original, modified);

        // Assert
        string.Join('\n', result.Where(l => l.Op != DiffOp.Insert).Select(l => l.Text)).Should().Be(original);
        string.Join('\n', result.Where(l => l.Op != DiffOp.Delete).Select(l => l.Text)).Should().Be(modified);
    }

    [Fact]
    public void DiffLines_NullOriginal_ThrowsArgumentNullException()
    {
        // Act
        var act = () => DiffHelper.DiffLines(null!, "a");

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("original");
    }

    [Fact]
    public void DiffLines_NullModified_ThrowsArgumentNullException()
    {
        // Act
        var act = () => DiffHelper.DiffLines("a", null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("modified");
    }

    // ── DiffTagsCsv ─────────────────────────────────────────────────────────

    [Fact]
    public void DiffTagsCsv_TagsRemovedAndAdded_ReportsRemovedAddedAndKept()
    {
        // Act
        var (removed, added, kept) = DiffHelper.DiffTagsCsv(",AI,GitHub Copilot,.NET,", ",AI,Azure,");

        // Assert
        removed.Should().Equal("GitHub Copilot", ".NET");
        added.Should().Equal("Azure");
        kept.Should().Equal("AI");
    }

    [Fact]
    public void DiffTagsCsv_IdenticalTags_ReportsOnlyKept()
    {
        // Act
        var (removed, added, kept) = DiffHelper.DiffTagsCsv(",AI,Azure,", ",AI,Azure,");

        // Assert
        removed.Should().BeEmpty();
        added.Should().BeEmpty();
        kept.Should().Equal("AI", "Azure");
    }

    [Fact]
    public void DiffTagsCsv_BothEmpty_ReturnsEmptyLists()
    {
        // Act
        var (removed, added, kept) = DiffHelper.DiffTagsCsv(string.Empty, ",,");

        // Assert
        removed.Should().BeEmpty();
        added.Should().BeEmpty();
        kept.Should().BeEmpty();
    }

    [Fact]
    public void DiffTagsCsv_EmptyFixed_RemovesEverything()
    {
        // Act
        var (removed, added, kept) = DiffHelper.DiffTagsCsv(",AI,Azure,", string.Empty);

        // Assert
        removed.Should().Equal("AI", "Azure");
        added.Should().BeEmpty();
        kept.Should().BeEmpty();
    }

    [Fact]
    public void DiffTagsCsv_EmptyOriginal_AddsEverything()
    {
        // Act
        var (removed, added, kept) = DiffHelper.DiffTagsCsv(string.Empty, "AI,Azure");

        // Assert
        removed.Should().BeEmpty();
        added.Should().Equal("AI", "Azure");
        kept.Should().BeEmpty();
    }

    [Fact]
    public void DiffTagsCsv_CaseDifferences_AreTreatedAsSameTagAndKeepOriginalCasing()
    {
        // Act
        var (removed, added, kept) = DiffHelper.DiffTagsCsv(",GitHub Copilot,", ",github copilot,");

        // Assert
        removed.Should().BeEmpty();
        added.Should().BeEmpty();
        kept.Should().Equal("GitHub Copilot");
    }

    [Fact]
    public void DiffTagsCsv_SurroundingWhitespace_IsTrimmed()
    {
        // Act
        var (removed, added, kept) = DiffHelper.DiffTagsCsv(" AI , Azure ", "Azure,  DevOps ");

        // Assert
        removed.Should().Equal("AI");
        added.Should().Equal("DevOps");
        kept.Should().Equal("Azure");
    }

    [Fact]
    public void DiffTagsCsv_ReorderedTags_AreNotReportedAsChanges()
    {
        // Act
        var (removed, added, kept) = DiffHelper.DiffTagsCsv("A,B,C", "C,B,A");

        // Assert
        removed.Should().BeEmpty();
        added.Should().BeEmpty();
        kept.Should().Equal("A", "B", "C");
    }

    [Fact]
    public void DiffTagsCsv_NullOriginal_ThrowsArgumentNullException()
    {
        // Act
        var act = () => DiffHelper.DiffTagsCsv(null!, "a");

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("originalCsv");
    }

    [Fact]
    public void DiffTagsCsv_NullFixed_ThrowsArgumentNullException()
    {
        // Act
        var act = () => DiffHelper.DiffTagsCsv("a", null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("fixedCsv");
    }
}
