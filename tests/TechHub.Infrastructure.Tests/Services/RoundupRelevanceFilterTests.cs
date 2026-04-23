using FluentAssertions;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services.RoundupGeneration;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Tests for <see cref="RoundupRelevanceFilter"/> ranking logic.
/// </summary>
public class RoundupRelevanceFilterTests
{
    [Fact]
    public void RankByImportance_SortsHighImpactFirst()
    {
        // Arrange
        var articles = new List<RoundupArticle>
        {
            BuildArticle("low-impact", impactLevel: "low", timeSensitivity: "this-week", dateEpoch: 100),
            BuildArticle("high-impact", impactLevel: "high", timeSensitivity: "this-week", dateEpoch: 100),
            BuildArticle("medium-impact", impactLevel: "medium", timeSensitivity: "this-week", dateEpoch: 100)
        };

        // Act
        var ranked = RoundupRelevanceFilter.RankByImportance(articles).ToList();

        // Assert
        ranked[0].Title.Should().Be("high-impact");
        ranked[1].Title.Should().Be("medium-impact");
        ranked[2].Title.Should().Be("low-impact");
    }

    [Fact]
    public void RankByImportance_BreaksTiesByTimeSensitivity()
    {
        // Arrange — same impact level, different time sensitivity
        var articles = new List<RoundupArticle>
        {
            BuildArticle("evergreen", impactLevel: "medium", timeSensitivity: "evergreen", dateEpoch: 100),
            BuildArticle("this-week", impactLevel: "medium", timeSensitivity: "this-week", dateEpoch: 100),
            BuildArticle("this-month", impactLevel: "medium", timeSensitivity: "this-month", dateEpoch: 100)
        };

        // Act
        var ranked = RoundupRelevanceFilter.RankByImportance(articles).ToList();

        // Assert
        ranked[0].Title.Should().Be("this-week");
        ranked[1].Title.Should().Be("this-month");
        ranked[2].Title.Should().Be("evergreen");
    }

    [Fact]
    public void RankByImportance_BreaksFurtherTiesByRecency()
    {
        // Arrange — same impact and time sensitivity, different dates
        var articles = new List<RoundupArticle>
        {
            BuildArticle("older", impactLevel: "medium", timeSensitivity: "this-week", dateEpoch: 1_000),
            BuildArticle("newest", impactLevel: "medium", timeSensitivity: "this-week", dateEpoch: 3_000),
            BuildArticle("middle", impactLevel: "medium", timeSensitivity: "this-week", dateEpoch: 2_000)
        };

        // Act
        var ranked = RoundupRelevanceFilter.RankByImportance(articles).ToList();

        // Assert
        ranked[0].Title.Should().Be("newest");
        ranked[1].Title.Should().Be("middle");
        ranked[2].Title.Should().Be("older");
    }

    [Fact]
    public void RankByImportance_HandlesUnknownValues()
    {
        // Arrange — unknown values should score lowest
        var articles = new List<RoundupArticle>
        {
            BuildArticle("unknown", impactLevel: "unknown", timeSensitivity: "unknown", dateEpoch: 100),
            BuildArticle("low", impactLevel: "low", timeSensitivity: "evergreen", dateEpoch: 100)
        };

        // Act
        var ranked = RoundupRelevanceFilter.RankByImportance(articles).ToList();

        // Assert
        ranked[0].Title.Should().Be("low");
        ranked[1].Title.Should().Be("unknown");
    }

    [Fact]
    public void RankByImportance_ImpactTakesPriorityOverTimeSensitivity()
    {
        // Arrange — high impact + evergreen should beat medium impact + this-week
        var articles = new List<RoundupArticle>
        {
            BuildArticle("medium-urgent", impactLevel: "medium", timeSensitivity: "this-week", dateEpoch: 100),
            BuildArticle("high-evergreen", impactLevel: "high", timeSensitivity: "evergreen", dateEpoch: 100)
        };

        // Act
        var ranked = RoundupRelevanceFilter.RankByImportance(articles).ToList();

        // Assert
        ranked[0].Title.Should().Be("high-evergreen");
        ranked[1].Title.Should().Be("medium-urgent");
    }

    private static RoundupArticle BuildArticle(
        string title,
        string impactLevel,
        string timeSensitivity,
        long dateEpoch) =>
        new()
        {
            SectionName = "ai",
            Title = title,
            ExternalUrl = $"https://example.com/{title}",
            Slug = title,
            CollectionName = "news",
            IsInternal = false,
            Summary = "Summary",
            KeyTopics = ["test"],
            Relevance = "medium",
            TopicType = "news",
            ImpactLevel = impactLevel,
            TimeSensitivity = timeSensitivity,
            DateEpoch = dateEpoch
        };
}
