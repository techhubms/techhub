using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using TechHub.Core.Models;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Integration tests for <see cref="SearchQueryCorrector"/> against PostgreSQL full-text search. The words are made up and
/// unique to this class, because the database is shared by the tests of the class.
/// </summary>
public class SearchQueryCorrectorTests : IClassFixture<DatabaseFixture<SearchQueryCorrectorTests>>
{
    private readonly DatabaseFixture<SearchQueryCorrectorTests> _fixture;
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());

    public SearchQueryCorrectorTests(DatabaseFixture<SearchQueryCorrectorTests> fixture)
    {
        _fixture = fixture;
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    private SearchQueryCorrector CreateCorrector() => new(_fixture.Connection, _cache, new PostgresDialect());

    private async Task SeedAsync(
        string title,
        string excerpt = "",
        string content = "",
        string tags = "",
        long dateEpoch = 1743278400,
        string section = "ai",
        string collection = "news")
    {
        var slug = $"corrector-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                primary_section_name, sections_bitmask, external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, @Collection, @Title, @Excerpt, @Content, @DateEpoch,
                @Section, @SectionMask, @Url, '', 'Test Feed', @Tags, @Hash)
            """,
            new
            {
                Slug = slug,
                Collection = collection,
                Title = title,
                Excerpt = excerpt,
                Content = content,
                DateEpoch = dateEpoch,
                Section = section,
                SectionMask = SectionBitmasks.For(section),
                Tags = tags,
                Url = $"https://example.com/{slug}",
                Hash = Guid.NewGuid().ToString()
            });
    }

    [Fact]
    public async Task CorrectAsync_WordNoArticleHasButACloseWordIsInTheTitles_ReplacesIt()
    {
        // Arrange
        await SeedAsync("Introducing the Quillamorph harness");
        await SeedAsync("Quillamorph harness in practice");

        // Act
        var corrected = await CreateCorrector().CorrectAsync("quillamorph harnass", ct: Ct);

        // Assert
        corrected.Should().Be("quillamorph harness");
    }

    [Fact]
    public async Task CorrectAsync_QueryThatFindsSomething_IsLeftAlone()
    {
        // Arrange
        await SeedAsync("Flibbertigibbet tooling");
        await SeedAsync("Flibbertigibbet news");

        // Act
        var corrected = await CreateCorrector().CorrectAsync("Flibbert", ct: Ct);

        // Assert
        corrected.Should().BeNull("a word that starts a word of an article is a valid prefix search, not a typo");
    }

    [Fact]
    public async Task CorrectAsync_WordThatOnlyTheBodyHas_IsNotReplacedByAWordOfTheTitles()
    {
        // Arrange
        await SeedAsync("Zorvexian runtime");
        await SeedAsync("Zorvexian tooling");
        await SeedAsync("Something else", content: "The body mentions zorvexiat once.");

        // Act
        var corrected = await CreateCorrector().CorrectAsync("zorvexiat", ct: Ct);

        // Assert
        corrected.Should().BeNull("the word is in an article, even if only in its text");
    }

    [Fact]
    public async Task CorrectAsync_QueryWithOneTypoAndOneGoodWord_ReplacesOnlyTheTypo()
    {
        // Arrange
        await SeedAsync("Glimmerwick agents explained");
        await SeedAsync("Glimmerwick agents at scale");

        // Act
        var corrected = await CreateCorrector().CorrectAsync("Glimmerwick agnets", ct: Ct);

        // Assert
        corrected.Should().Be("Glimmerwick agents");
    }

    [Theory]
    [InlineData("xq")]
    [InlineData("qzx")]
    [InlineData("1234")]
    [InlineData("")]
    public async Task CorrectAsync_ShortNumericOrEmptyQuery_IsNeverCorrected(string query)
    {
        var corrected = await CreateCorrector().CorrectAsync(query, ct: Ct);

        corrected.Should().BeNull();
    }

    [Fact]
    public async Task CorrectAsync_WordFarFromEveryKnownWord_IsLeftAlone()
    {
        var corrected = await CreateCorrector().CorrectAsync("wqzxvbnmplk", ct: Ct);

        corrected.Should().BeNull();
    }

    [Fact]
    public async Task CorrectAsync_ClosestWordIsOnlyATag_IsNotUsedBecauseTheSearchCannotMatchIt()
    {
        // Arrange: tags are not part of the search index
        await SeedAsync("Something about plumbing", tags: "Wobbleflux");
        await SeedAsync("Something else about plumbing", tags: "Wobbleflux");

        // Act
        var corrected = await CreateCorrector().CorrectAsync("wobbleflix", ct: Ct);

        // Assert
        corrected.Should().BeNull("a correction that still finds nothing is no help");
    }

    [Fact]
    public async Task CorrectAsync_UnsearchableWordTypedCorrectly_IsNotReturnedAsItsOwnCorrection()
    {
        // Arrange
        await SeedAsync("Something about plumbing", tags: "Snorkelwump");
        await SeedAsync("Something else about plumbing", tags: "Snorkelwump");

        // Act
        var corrected = await CreateCorrector().CorrectAsync("snorkelwump", ct: Ct);

        // Assert
        corrected.Should().BeNull();
    }

    [Fact]
    public async Task CorrectAsync_MoreFrequentWordOfTheExcerptIsStemmedInTheIndex_FallsBackToASearchableWord()
    {
        // Arrange: the index holds the English stem "zumbl" of the excerpt word "zumbling", which a search for "zumbling" cannot match.
        // "zumbline" is in two titles, which the index holds as written.
        await SeedAsync("Plain plumbing", excerpt: "Zumbling pipes");
        await SeedAsync("More plumbing", excerpt: "Zumbling taps");
        await SeedAsync("Even more plumbing", excerpt: "Zumbling valves");
        await SeedAsync("Zumbline guide");
        await SeedAsync("Zumbline tips");

        // Act
        var corrected = await CreateCorrector().CorrectAsync("zumblinx", ct: Ct);

        // Assert
        corrected.Should().Be("zumbline");
    }

    [Fact]
    public async Task CorrectAsync_WordThatOnlyItsCompoundExpansionMatches_IsLeftAlone()
    {
        // Arrange: no word starts with "openai", but the search expands it to "open", which the third article has
        await SeedAsync("Openapi specification");
        await SeedAsync("Openapi tooling");
        await SeedAsync("Open source in practice");

        // Act
        var corrected = await CreateCorrector().CorrectAsync("openai", ct: Ct);

        // Assert
        corrected.Should().BeNull("the search finds results for this query, so it is not a typo");
    }

    [Fact]
    public async Task CorrectAsync_WordThatOnlyAnArticleOutsideTheDateWindowHas_IsCorrected()
    {
        // Arrange: the search looks at the last months by default, so "brindlewax" finds nothing there, but it is in an old article
        const long Old = 1577836800; // 2020-01-01
        const long Recent = 1767225600; // 2026-01-01
        await SeedAsync("Brindlewax of old", dateEpoch: Old);
        await SeedAsync("Brindlewix guide", dateEpoch: Recent);
        await SeedAsync("Brindlewix tips", dateEpoch: Recent);
        var corrector = CreateCorrector();
        var window = new SearchScope(["ai"], ["all"], DateTimeOffset.FromUnixTimeSeconds(1735689600));

        // Act: without the window the old article makes the word a known one, which must not be remembered for the window
        var withoutWindow = await corrector.CorrectAsync("brindlewax", ct: Ct);
        var withWindow = await corrector.CorrectAsync("brindlewax", window, Ct);

        // Assert
        withoutWindow.Should().BeNull();
        withWindow.Should().Be("brindlewix");
    }

    [Fact]
    public async Task CorrectAsync_WordThatOnlyAnotherSectionHas_IsCorrectedWithinTheSection()
    {
        // Arrange
        await SeedAsync("Frobnitzel in azure", section: "azure");
        await SeedAsync("Frobnitzer in ai", section: "ai");
        await SeedAsync("Frobnitzer tips for ai", section: "ai");

        // Act
        var inSection = await CreateCorrector().CorrectAsync("frobnitzel", new SearchScope(["ai"], ["all"]), Ct);

        // Assert
        inSection.Should().Be("frobnitzer");
    }

    [Fact]
    public async Task CorrectAsync_WordThatOnlyAnotherCollectionHas_IsCorrectedWithinTheCollections()
    {
        // Arrange
        await SeedAsync("Quendlebar video", collection: "videos");
        await SeedAsync("Quendlebor news", collection: "news");
        await SeedAsync("Quendlebor news again", collection: "news");

        // Act
        var inCollection = await CreateCorrector().CorrectAsync("quendlebar", new SearchScope(["all"], ["news", "blogs"]), Ct);
        var inVideos = await CreateCorrector().CorrectAsync("quendlebar", new SearchScope(["all"], ["videos"]), Ct);

        // Assert
        inCollection.Should().Be("quendlebor");
        inVideos.Should().BeNull("the video has the word, so a search in the videos finds it");
    }
}
