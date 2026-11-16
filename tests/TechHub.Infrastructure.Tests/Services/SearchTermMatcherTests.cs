using FluentAssertions;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for <see cref="SearchTermMatcher"/>: the closest known word to a search term that matches nothing.
/// </summary>
public class SearchTermMatcherTests
{
    // Most frequent first, like the vocabulary of the corrector
    private static readonly IReadOnlyList<SearchVocabularyWord> _vocabulary =
    [
        new("copilot", 900),
        new("harness", 95),
        new("harnesses", 18),
        new("hardness", 3),
        new("agent", 500),
        new("agents", 400)
    ];

    [Theory]
    [InlineData("harnass", "harness")]
    [InlineData("harnes", "harness")]
    [InlineData("copilott", "copilot")]
    [InlineData("copliot", "copilot")]
    [InlineData("agnet", "agent")]
    public void FindClosest_TermWithATypo_ReturnsTheKnownWord(string term, string expected)
    {
        // Act
        var closest = SearchTermMatcher.FindClosest(term, _vocabulary);

        // Assert
        closest.Should().Be(expected);
    }

    [Fact]
    public void FindClosest_TieBetweenWords_PrefersTheMoreFrequentWord()
    {
        // "agens" is one edit from "agent" (500 articles) and from "agents" (400 articles)
        var closest = SearchTermMatcher.FindClosest("agens", _vocabulary);

        closest.Should().Be("agent");
    }

    [Theory]
    [InlineData("zzzzzz")]
    [InlineData("kubernetes")]
    [InlineData("cat")]
    public void FindClosest_NothingWithinReach_ReturnsNull(string term)
    {
        SearchTermMatcher.FindClosest(term, _vocabulary).Should().BeNull();
    }

    [Theory]
    [InlineData("harnass", "harness", 1)]
    [InlineData("harnses", "harness", 1)]
    [InlineData("harness", "harness", 0)]
    [InlineData("copliot", "copilot", 1)]
    [InlineData("abcd", "wxyz", 4)]
    public void Distance_CountsInsertionsDeletionsSubstitutionsAndSwaps(string a, string b, int expected)
    {
        // The distance is capped at the maximum plus one, so a result above the cap only means "too far"
        SearchTermMatcher.Distance(a, b, maxDistance: 2).Should().Be(Math.Min(expected, 3));
    }
}
