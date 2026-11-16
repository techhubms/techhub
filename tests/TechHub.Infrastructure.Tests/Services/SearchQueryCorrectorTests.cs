using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using TechHub.Infrastructure.Data;
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

    private SearchQueryCorrector CreateCorrector() => new(_fixture.Connection, _cache);

    private async Task SeedAsync(string title, string excerpt = "", string content = "")
    {
        var slug = $"corrector-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                primary_section_name, external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, 'news', @Title, @Excerpt, @Content, 1743278400, 'ai', @Url, '', 'Test Feed', '', @Hash)
            """,
            new { Slug = slug, Title = title, Excerpt = excerpt, Content = content, Url = $"https://example.com/{slug}", Hash = Guid.NewGuid().ToString() });
    }

    [Fact]
    public async Task CorrectAsync_WordNoArticleHasButACloseWordIsInTheTitles_ReplacesIt()
    {
        // Arrange
        await SeedAsync("Introducing the Quillamorph harness");
        await SeedAsync("Quillamorph harness in practice");

        // Act
        var corrected = await CreateCorrector().CorrectAsync("quillamorph harnass", Ct);

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
        var corrected = await CreateCorrector().CorrectAsync("Flibbert", Ct);

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
        var corrected = await CreateCorrector().CorrectAsync("zorvexiat", Ct);

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
        var corrected = await CreateCorrector().CorrectAsync("Glimmerwick agnets", Ct);

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
        var corrected = await CreateCorrector().CorrectAsync(query, Ct);

        corrected.Should().BeNull();
    }

    [Fact]
    public async Task CorrectAsync_WordFarFromEveryKnownWord_IsLeftAlone()
    {
        var corrected = await CreateCorrector().CorrectAsync("wqzxvbnmplk", Ct);

        corrected.Should().BeNull();
    }
}
