using FluentAssertions;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services.RoundupGeneration;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for the code-built parts of a roundup: <see cref="RoundupBlocksBuilder"/>, <see cref="RoundupHeadings"/>
/// and <see cref="RoundupArticleSelector"/>.
/// </summary>
public class RoundupBlocksBuilderTests
{
    private static readonly DateOnly _weekEnd = new(2025, 4, 13);

    private static RoundupDateEvent Event(
        string kind = "retirement",
        string product = "Product",
        string? feature = null,
        string date = "2025-06-01",
        string precision = "day") => new()
        {
            Kind = kind,
            Product = product,
            Feature = feature,
            Description = "Something happens",
            Date = DateOnly.Parse(date, System.Globalization.CultureInfo.InvariantCulture),
            DatePrecision = precision,
            SourceTitle = "Source",
            SourceUrl = "https://example.com/source"
        };

    private static RoundupArticle Article(int index, string primary = "ai", string[]? facts = null) => new()
    {
        SectionName = "azure",
        PrimarySectionName = primary,
        Title = $"Article {index}",
        ExternalUrl = $"https://example.com/{index}",
        Slug = $"article-{index}",
        CollectionName = "news",
        KeyFacts = facts ?? []
    };

    // ── Dates to know ─────────────────────────────────────────────────────────

    [Fact]
    public void BuildDatesToKnow_NoEvents_ReturnsEmpty()
    {
        RoundupBlocksBuilder.BuildDatesToKnow([], _weekEnd).Should().BeEmpty();
    }

    [Theory]
    [InlineData("announced")]
    [InlineData("event")]
    public void BuildDatesToKnow_IgnoresKindsThatAreNeitherDeadlinesNorReleases(string kind)
    {
        RoundupBlocksBuilder.BuildDatesToKnow([Event(kind: kind)], _weekEnd).Should().BeEmpty();
    }

    [Fact]
    public void BuildDatesToKnow_IgnoresDatesInOrBeforeTheRoundupWeek()
    {
        var result = RoundupBlocksBuilder.BuildDatesToKnow([Event(date: "2025-04-13"), Event(date: "2025-03-01")], _weekEnd);

        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("deprecation")]
    [InlineData("retirement")]
    [InlineData("end-of-support")]
    [InlineData("breaking-change")]
    [InlineData("pricing-change")]
    public void BuildDatesToKnow_PutsDeadlineKindsUnderDeadlines(string kind)
    {
        var result = RoundupBlocksBuilder.BuildDatesToKnow([Event(kind: kind)], _weekEnd);

        result.Should().Contain("### Deadlines").And.NotContain("### Coming up");
    }

    [Theory]
    [InlineData("preview")]
    [InlineData("ga")]
    [InlineData("launch")]
    [InlineData("release")]
    public void BuildDatesToKnow_PutsReleaseKindsUnderComingUp(string kind)
    {
        var result = RoundupBlocksBuilder.BuildDatesToKnow([Event(kind: kind)], _weekEnd);

        result.Should().Contain("### Coming up").And.NotContain("### Deadlines");
    }

    [Fact]
    public void BuildDatesToKnow_MergesDuplicatesAndSortsByDate()
    {
        var result = RoundupBlocksBuilder.BuildDatesToKnow(
        [
            Event(product: "B", date: "2025-09-01"),
            Event(product: "A", date: "2025-06-01"),
            Event(product: "a", date: "2025-06-01")
        ], _weekEnd);

        result.Split('\n').Count(l => l.StartsWith("- ", StringComparison.Ordinal)).Should().Be(2);
        result.IndexOf("A:", StringComparison.Ordinal).Should().BeLessThan(result.IndexOf("B:", StringComparison.Ordinal));
    }

    [Fact]
    public void BuildDatesToKnow_CapsEachGroupAtTenEntries()
    {
        var events = Enumerable.Range(1, 15).Select(i => Event(product: $"Product {i}", date: $"2025-06-{i:00}")).ToList();

        var result = RoundupBlocksBuilder.BuildDatesToKnow(events, _weekEnd);

        result.Split('\n').Count(l => l.StartsWith("- ", StringComparison.Ordinal)).Should().Be(10);
    }

    [Theory]
    [InlineData("2025-06-01", "day", "1 June 2025")]
    [InlineData("2025-06-01", "month", "June 2025")]
    [InlineData("2025-04-01", "quarter", "Q2 2025")]
    [InlineData("2025-10-01", "quarter", "Q4 2025")]
    public void FormatDate_UsesThePrecision(string date, string precision, string expected)
    {
        RoundupBlocksBuilder.FormatDate(DateOnly.Parse(date, System.Globalization.CultureInfo.InvariantCulture), precision).Should().Be(expected);
    }

    // ── Also relevant ─────────────────────────────────────────────────────────

    [Fact]
    public void BuildAlsoRelevant_NoBriefArticles_ReturnsEmpty()
    {
        RoundupBlocksBuilder.BuildAlsoRelevant([], new DateOnly(2025, 4, 14), new Dictionary<string, string>(), new HashSet<string>()).Should().BeEmpty();
    }

    [Fact]
    public void BuildAlsoRelevant_WithoutKeyFacts_StillListsTheLinkAndTheRoundup()
    {
        var result = RoundupBlocksBuilder.BuildAlsoRelevant(
            [Article(1)], new DateOnly(2025, 4, 14), new Dictionary<string, string> { ["ai"] = "Artificial Intelligence" }, new HashSet<string> { "ai" });

        result.Should().Contain("- [Article 1](https://example.com/1) Full story in the [Artificial Intelligence roundup](/ai/roundups/weekly-ai-roundup-2025-04-14).");
    }

    [Fact]
    public void BuildAlsoRelevant_EndsTheKeyFactWithAPeriod()
    {
        var result = RoundupBlocksBuilder.BuildAlsoRelevant(
            [Article(1, facts: ["It is generally available"])], new DateOnly(2025, 4, 14), new Dictionary<string, string>(), new HashSet<string> { "ai" });

        result.Should().Contain("): It is generally available. Full story in the [ai roundup]");
    }

    [Fact]
    public void BuildAlsoRelevant_PrimaryRoundupDoesNotExist_KeepsTheSourceLinkWithoutTheFullStoryClaim()
    {
        var result = RoundupBlocksBuilder.BuildAlsoRelevant(
            [Article(1, facts: ["It is generally available"])], new DateOnly(2025, 4, 14),
            new Dictionary<string, string> { ["ai"] = "Artificial Intelligence" }, new HashSet<string> { "azure" });

        result.Should().Contain("- [Article 1](https://example.com/1): It is generally available.");
        result.Should().NotContain("Full story");
        result.Should().NotContain("/ai/roundups/");
    }

    // ── Completeness ──────────────────────────────────────────────────────────

    [Fact]
    public void EnsureLeadLinks_AllLinked_ReturnsContentUnchanged()
    {
        var (content, appended) = RoundupBlocksBuilder.EnsureLeadLinks("## T\n\n- [Article 1](https://example.com/1)", [Article(1)], "AI");

        content.Should().Be("## T\n\n- [Article 1](https://example.com/1)");
        appended.Should().BeEmpty();
    }

    [Fact]
    public void EnsureLeadLinks_InsertsBeforeTheNextHeadingWhenOtherNewsIsNotLast()
    {
        var input = "## Other AI News\n\nMisc.\n\n- [Old](https://example.com/old)\n\n## Later Topic\n\nText.";

        var (content, appended) = RoundupBlocksBuilder.EnsureLeadLinks(input, [Article(2)], "AI");

        appended.Should().ContainSingle();
        content.IndexOf("[Article 2]", StringComparison.Ordinal).Should().BeLessThan(content.IndexOf("## Later Topic", StringComparison.Ordinal));
        content.IndexOf("[Article 2]", StringComparison.Ordinal).Should().BeGreaterThan(content.IndexOf("[Old]", StringComparison.Ordinal));
    }

    [Fact]
    public void EnsureLeadLinks_MatchesSchemeAndHostCaseInsensitively()
    {
        var (_, appended) = RoundupBlocksBuilder.EnsureLeadLinks("- [x](HTTPS://EXAMPLE.COM/1)", [Article(1)], "AI");

        appended.Should().BeEmpty();
    }

    [Fact]
    public void EnsureLeadLinks_PathCasing_DistinguishesArticles()
    {
        var upper = Article(1) with { ExternalUrl = "https://example.com/Release", Title = "Upper" };
        var lower = Article(2) with { ExternalUrl = "https://example.com/release", Title = "Lower" };

        var (_, appended) = RoundupBlocksBuilder.EnsureLeadLinks("- [Upper](https://example.com/Release)", [upper, lower], "AI");

        appended.Should().ContainSingle().Which.ExternalUrl.Should().Be("https://example.com/release");
    }

    [Fact]
    public void EnsureLeadLinks_OverlappingUrls_DoNotCountAsLinked()
    {
        var (content, appended) = RoundupBlocksBuilder.EnsureLeadLinks("- [Ten](https://example.com/10)", [Article(1), Article(10)], "AI");

        appended.Should().ContainSingle().Which.ExternalUrl.Should().Be("https://example.com/1");
        content.Should().Contain("[Article 1](https://example.com/1)");
    }

    [Theory]
    [InlineData("- [x](https://example.com/1 \"Title\")")]
    [InlineData("- [x](<https://example.com/1>)")]
    [InlineData("Read <https://example.com/1> now")]
    public void EnsureLeadLinks_LinkVariants_CountAsLinked(string input)
    {
        var (_, appended) = RoundupBlocksBuilder.EnsureLeadLinks(input, [Article(1)], "AI");

        appended.Should().BeEmpty();
    }

    [Fact]
    public void EnsureLeadLinks_UrlOnlyMentionedAsPlainText_DoesNotCountAsLinked()
    {
        var (_, appended) = RoundupBlocksBuilder.EnsureLeadLinks("See https://example.com/1 for details", [Article(1)], "AI");

        appended.Should().ContainSingle();
    }

    // ── Headings ──────────────────────────────────────────────────────────────

    [Fact]
    public void Extract_ReturnsTopicHeadingsOnly()
    {
        var content = "Intro text\n\n<!--excerpt_end-->\n\n## This Week's Overview\n\n- toc\n\n## Topic A\n\nText\n\n### Sub A1\n\n## Also relevant this week\n\n- x\n\n## Dates to know\n\n```\n## inside fence\n```";

        RoundupHeadings.Extract(content).Should().Be("## Topic A\n### Sub A1");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Extract_BlankContent_ReturnsEmpty(string? content)
    {
        RoundupHeadings.Extract(content).Should().BeEmpty();
    }

    // ── Selector ──────────────────────────────────────────────────────────────

    [Fact]
    public void Order_ArticlesWithoutProducts_SortAfterThoseWithProductsOfTheSameSignificance()
    {
        var withProduct = Article(1) with { Products = ["Zeta"] };
        var without = Article(2);

        var ordered = RoundupArticleSelector.Order([without, withProduct]);

        ordered[0].Title.Should().Be("Article 1");
    }

    [Fact]
    public void Order_ProductComparisonIgnoresCaseAndWhitespace()
    {
        var first = Article(2) with { Products = [" github copilot "] };
        var second = Article(1) with { Products = ["GitHub Copilot"] };

        var ordered = RoundupArticleSelector.Order([first, second]);

        // Same product after normalization, so the title decides.
        ordered.Select(a => a.Title).Should().Equal("Article 1", "Article 2");
    }
}
