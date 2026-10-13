using System.Diagnostics.Metrics;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ModelContextProtocol;
using Moq;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.MCP;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.MCP;

public sealed class LatestContentProviderTests : IDisposable
{
    private static readonly DateTimeOffset _now = new(2026, 10, 13, 8, 0, 0, TimeSpan.Zero);

    private readonly Mock<ITechHubApiClient> _apiClient = new();
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());

    public void Dispose() => _cache.Dispose();

    [Fact]
    public async Task GetLatestAsync_RequestsAllCollectionsOfSectionWithPaging()
    {
        // Arrange
        SetupApi("ai", skip: 20, items: [new ContentItemBuilder().Build()], totalCount: 25);
        var sut = CreateSut();

        // Act
        var result = await sut.GetLatestAsync("ai", new ContentFilter { Page = 3 }, TestContext.Current.CancellationToken);

        // Assert
        result.Page.Should().Be(3);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(25);
        result.TotalPages.Should().Be(3);
        _apiClient.Verify(c => c.GetCollectionItemsAsync(
            "ai", "all", 10, 20, null, null, null, null, null, null, false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetLatestAsync_MoreItemsThanMaxPageAllows_CapsTotalPagesAtMaxPage()
    {
        // Arrange
        SetupApi("ai", skip: 0, items: [new ContentItemBuilder().Build()], totalCount: 5000);
        var sut = CreateSut();

        // Act
        var result = await sut.GetLatestAsync("ai", new ContentFilter(), TestContext.Current.CancellationToken);

        // Assert
        result.TotalPages.Should().Be(LatestContentProvider.MaxPage);
        result.TotalCount.Should().Be(5000);
    }

    [Fact]
    public async Task GetLatestAsync_MapsItemsToAbsoluteCanonicalUrlsOnConfiguredBaseUrl()
    {
        // Arrange
        var item = new ContentItemBuilder()
            .WithSlug("My-Slug")
            .WithCollectionName("blogs")
            .WithPrimarySectionName("dotnet")
            .Build();
        SetupApi("dotnet", skip: 0, items: [item], totalCount: 1);
        var sut = CreateSut();

        // Act
        var result = await sut.GetLatestAsync("dotnet", new ContentFilter(), TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle()
            .Which.Url.Should().Be("https://techhub.test/dotnet/blogs/my-slug");
    }

    [Fact]
    public async Task GetLatestAsync_MapsDateAsBrusselsIsoAndExposesCollection()
    {
        // Arrange
        var item = new ContentItemBuilder()
            .WithDateEpoch(new DateTimeOffset(2026, 9, 24, 8, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds())
            .WithCollectionName("videos")
            .Build();
        SetupApi("ai", skip: 0, items: [item], totalCount: 1);
        var sut = CreateSut();

        // Act
        var result = await sut.GetLatestAsync("ai", new ContentFilter(), TestContext.Current.CancellationToken);

        // Assert
        var mapped = result.Items.Should().ContainSingle().Subject;
        mapped.Date.Should().Be("2026-09-24T10:00:00+02:00");
        mapped.Collection.Should().Be("videos");
    }

    [Fact]
    public async Task GetLatestAsync_ExternalItem_LinksToTechHubAndDoesNotExposeOriginalUrl()
    {
        // Arrange - the Tech Hub page shows the summary and the button to the original, so the original is never handed to agents
        var item = new ContentItemBuilder()
            .WithSlug("my-slug")
            .WithCollectionName("news")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://example.org/original")
            .Build();
        SetupApi("ai", skip: 0, items: [item], totalCount: 1);
        var sut = CreateSut();

        // Act
        var result = await sut.GetLatestAsync("ai", new ContentFilter(), TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle().Which.Url.Should().Be("https://techhub.test/ai/news/my-slug");
        JsonSerializer.Serialize(result).Should().NotContain("example.org");
    }

    [Fact]
    public async Task GetLatestAsync_ReportsGeneratedAtFromTimeProviderInBrusselsIso()
    {
        // Arrange
        SetupApi("ai", skip: 0, items: [], totalCount: 0);
        var sut = CreateSut();

        // Act
        var result = await sut.GetLatestAsync("ai", new ContentFilter(), TestContext.Current.CancellationToken);

        // Assert
        result.GeneratedAt.Should().Be("2026-10-13T10:00:00+02:00");
    }

    [Fact]
    public async Task GetLatestAsync_ByDefault_OmitsTags()
    {
        // Arrange
        SetupApi("ai", skip: 0, items: [new ContentItemBuilder().WithTags("mcp", "agents").Build()], totalCount: 1);
        var sut = CreateSut();

        // Act
        var result = await sut.GetLatestAsync("ai", new ContentFilter(), TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle().Which.Tags.Should().BeNull();
    }

    [Fact]
    public async Task GetLatestAsync_IncludeTags_ReturnsTags()
    {
        // Arrange
        SetupApi("ai", skip: 0, items: [new ContentItemBuilder().WithTags("mcp", "agents").Build()], totalCount: 1);
        var sut = CreateSut();

        // Act
        var result = await sut.GetLatestAsync("ai", new ContentFilter { IncludeTags = true }, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle().Which.Tags.Should().Equal("mcp", "agents");
    }

    [Fact]
    public async Task GetLatestAsync_IncludeTagsToggled_DoesNotCallApiAgain()
    {
        // Arrange
        SetupApi("ai", skip: 0, items: [new ContentItemBuilder().WithTags("mcp").Build()], totalCount: 1);
        var sut = CreateSut();

        // Act
        await sut.GetLatestAsync("ai", new ContentFilter(), TestContext.Current.CancellationToken);
        var withTags = await sut.GetLatestAsync("ai", new ContentFilter { IncludeTags = true }, TestContext.Current.CancellationToken);

        // Assert
        withTags.Items.Should().ContainSingle().Which.Tags.Should().Equal("mcp");
        VerifyApiCalls(Times.Once());
    }

    [Fact]
    public async Task GetLatestAsync_Days_IsForwardedAsLastDays()
    {
        // Arrange
        SetupApi("ai", skip: 0, items: [], totalCount: 0, lastDays: 14);
        var sut = CreateSut();

        // Act
        await sut.GetLatestAsync("ai", new ContentFilter { Days = 14 }, TestContext.Current.CancellationToken);

        // Assert
        _apiClient.VerifyAll();
    }

    [Fact]
    public async Task GetLatestAsync_Since_IsForwardedAsFromDateAndWinsOverDays()
    {
        // Arrange
        SetupApi("ai", skip: 0, items: [], totalCount: 0, fromDate: "2026-09-29");
        var sut = CreateSut();

        // Act
        await sut.GetLatestAsync("ai", new ContentFilter { Since = "2026-09-29", Days = 14 }, TestContext.Current.CancellationToken);

        // Assert
        _apiClient.VerifyAll();
    }

    [Fact]
    public async Task GetLatestAsync_ContentTypes_AreNormalizedAndForwardedAsTypes()
    {
        // Arrange
        SetupApi("ai", skip: 0, items: [], totalCount: 0, types: "news,roundups");
        var sut = CreateSut();

        // Act
        await sut.GetLatestAsync("ai", new ContentFilter { ContentTypes = " News, Roundups " }, TestContext.Current.CancellationToken);

        // Assert
        _apiClient.VerifyAll();
    }

    [Fact]
    public async Task GetLatestAsync_QueryAndTags_AreTrimmedAndForwarded()
    {
        // Arrange
        SetupApi("ai", skip: 0, items: [], totalCount: 0, query: "agent framework", tags: "mcp,.net");
        var sut = CreateSut();

        // Act
        await sut.GetLatestAsync(
            "ai",
            new ContentFilter { Query = "  agent framework ", Tags = " mcp , .net " },
            TestContext.Current.CancellationToken);

        // Assert
        _apiClient.VerifyAll();
    }

    [Theory]
    [InlineData("2026-9-29")]
    [InlineData("29/09/2026")]
    [InlineData("yesterday")]
    [InlineData("2026-09-29T10:00:00Z")]
    public async Task GetLatestAsync_InvalidSince_ThrowsMcpExceptionWithHint(string since)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var act = () => sut.GetLatestAsync("ai", new ContentFilter { Since = since }, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("yyyy-MM-dd");
        _apiClient.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3651)]
    public async Task GetLatestAsync_DaysOutOfRange_ThrowsMcpException(int days)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var act = () => sut.GetLatestAsync("ai", new ContentFilter { Days = days }, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("days");
    }

    [Theory]
    [InlineData("news,../etc")]
    [InlineData("news;drop")]
    [InlineData("a,b,c,d,e,f,g,h,i,j,k")]
    public async Task GetLatestAsync_InvalidContentTypes_ThrowsMcpException(string contentTypes)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var act = () => sut.GetLatestAsync("ai", new ContentFilter { ContentTypes = contentTypes }, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("contentType");
    }

    [Fact]
    public async Task GetLatestAsync_UnknownContentType_ThrowsMcpExceptionListingValidTypes()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var act = () => sut.GetLatestAsync("ai", new ContentFilter { ContentTypes = "news,typo" }, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("typo").And.Contain("blogs, community, news, roundups, videos");
        _apiClient.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetLatestAsync_QueryTooLong_ThrowsMcpException()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var act = () => sut.GetLatestAsync("ai", new ContentFilter { Query = new string('a', 201) }, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("query");
    }

    [Fact]
    public async Task GetLatestAsync_TooManyOrTooLongTags_ThrowsMcpException()
    {
        // Arrange
        var sut = CreateSut();
        var tooMany = string.Join(',', Enumerable.Range(0, 21).Select(i => $"tag{i}"));

        // Act
        var manyTags = () => sut.GetLatestAsync("ai", new ContentFilter { Tags = tooMany }, TestContext.Current.CancellationToken);
        var longTag = () => sut.GetLatestAsync("ai", new ContentFilter { Tags = new string('a', 101) }, TestContext.Current.CancellationToken);

        // Assert
        (await manyTags.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("tags");
        (await longTag.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("tags");
    }

    [Fact]
    public async Task GetLatestAsync_SamePageTwice_CallsApiOnce()
    {
        // Arrange
        SetupApi("azure", skip: 0, items: [new ContentItemBuilder().Build()], totalCount: 1);
        var sut = CreateSut();

        // Act
        var first = await sut.GetLatestAsync("azure", new ContentFilter(), TestContext.Current.CancellationToken);
        var second = await sut.GetLatestAsync("azure", new ContentFilter(), TestContext.Current.CancellationToken);

        // Assert
        second.Should().BeEquivalentTo(first);
        VerifyApiCalls(Times.Once());
    }

    [Fact]
    public async Task GetLatestAsync_DateAndTypeFilters_BypassTheCache()
    {
        // Arrange - day counts, dates and type lists have too many distinct values to cache safely
        SetupApi("azure", skip: 0, items: [], totalCount: 0, lastDays: 7);
        SetupApi("azure", skip: 0, items: [], totalCount: 0, fromDate: "2026-09-29");
        SetupApi("azure", skip: 0, items: [], totalCount: 0, types: "news");
        var sut = CreateSut();

        // Act
        for (var i = 0; i < 2; i++)
        {
            await sut.GetLatestAsync("azure", new ContentFilter { Days = 7 }, TestContext.Current.CancellationToken);
            await sut.GetLatestAsync("azure", new ContentFilter { Since = "2026-09-29" }, TestContext.Current.CancellationToken);
            await sut.GetLatestAsync("azure", new ContentFilter { ContentTypes = "news" }, TestContext.Current.CancellationToken);
        }

        // Assert
        VerifyApiCalls(Times.Exactly(6));
    }

    [Fact]
    public async Task GetLatestAsync_FreeTextFilters_BypassTheCache()
    {
        // Arrange - free-text values are unbounded, so caching them would let callers fill the cache
        SetupApi("azure", skip: 0, items: [], totalCount: 0, query: "agents");
        SetupApi("azure", skip: 0, items: [], totalCount: 0, tags: "mcp");
        var sut = CreateSut();

        // Act
        await sut.GetLatestAsync("azure", new ContentFilter { Query = "agents" }, TestContext.Current.CancellationToken);
        await sut.GetLatestAsync("azure", new ContentFilter { Query = "agents" }, TestContext.Current.CancellationToken);
        await sut.GetLatestAsync("azure", new ContentFilter { Tags = "mcp" }, TestContext.Current.CancellationToken);
        await sut.GetLatestAsync("azure", new ContentFilter { Tags = "mcp" }, TestContext.Current.CancellationToken);

        // Assert
        VerifyApiCalls(Times.Exactly(4));
    }

    [Theory]
    [InlineData("https://techhub.test")]
    [InlineData("https://techhub.test/")]
    public async Task GetLatestAsync_TrimsTrailingSlashOfConfiguredBaseUrl(string baseUrl)
    {
        // Arrange
        SetupApi("azure", skip: 0, items: [new ContentItemBuilder().WithPrimarySectionName("azure").Build()], totalCount: 1);
        var sut = CreateSut(baseUrl);

        // Act
        var result = await sut.GetLatestAsync("azure", new ContentFilter(), TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle().Which.Url.Should().StartWith("https://techhub.test/azure/");
    }

    [Fact]
    public async Task GetLatestAsync_DifferentPagesAndSections_AreCachedSeparately()
    {
        // Arrange
        var items = new[] { new ContentItemBuilder().Build() };
        SetupApi("ml", skip: 0, items, totalCount: 30);
        SetupApi("ml", skip: 10, items, totalCount: 30);
        SetupApi("security", skip: 0, items, totalCount: 30);
        var sut = CreateSut();

        // Act
        await sut.GetLatestAsync("ml", new ContentFilter { Page = 1 }, TestContext.Current.CancellationToken);
        await sut.GetLatestAsync("ml", new ContentFilter { Page = 2 }, TestContext.Current.CancellationToken);
        await sut.GetLatestAsync("security", new ContentFilter { Page = 1 }, TestContext.Current.CancellationToken);

        // Assert
        VerifyApiCalls(Times.Exactly(3));
    }

    [Fact]
    public async Task GetLatestAsync_ApiFailure_IsNotCachedAndPropagates()
    {
        // Arrange
        var calls = 0;
        _apiClient
            .Setup(c => c.GetCollectionItemsAsync(
                "devops", "all", 10, 0, null, null, null, null, null, null, false, It.IsAny<CancellationToken>()))
            .Returns(() => ++calls == 1
                ? throw new HttpRequestException("boom")
                : Task.FromResult<CollectionItemsResponse?>(new CollectionItemsResponse([new ContentItemBuilder().Build()], 1)));
        var sut = CreateSut();

        // Act
        var failing = () => sut.GetLatestAsync("devops", new ContentFilter(), TestContext.Current.CancellationToken);
        await failing.Should().ThrowAsync<HttpRequestException>();
        var result = await sut.GetLatestAsync("devops", new ContentFilter(), TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().HaveCount(1);
        calls.Should().Be(2);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-5, 1)]
    [InlineData(1, 1)]
    [InlineData(100, 100)]
    [InlineData(101, 100)]
    [InlineData(int.MaxValue, 100)]
    public async Task GetLatestAsync_ClampsPageToSupportedRange(int requestedPage, int expectedPage)
    {
        // Arrange
        _apiClient
            .Setup(c => c.GetCollectionItemsAsync(
                "ai", "all", 10, (expectedPage - 1) * 10, null, null, null, null, null, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse([], 0));
        var sut = CreateSut();

        // Act
        var result = await sut.GetLatestAsync("ai", new ContentFilter { Page = requestedPage }, TestContext.Current.CancellationToken);

        // Assert
        result.Page.Should().Be(expectedPage);
        _apiClient.VerifyAll();
    }

    [Fact]
    public async Task GetLatestAsync_RecordsCacheMissThenCacheHitMetric()
    {
        // Arrange - unique section so parallel tests sharing the static meter do not interfere
        const string Section = "telemetry-test-section";
        using var recorder = new RequestMetricRecorder(Section);
        SetupApi(Section, skip: 0, items: [new ContentItemBuilder().Build()], totalCount: 1);
        var sut = CreateSut();

        // Act
        await sut.GetLatestAsync(Section, new ContentFilter(), TestContext.Current.CancellationToken);
        await sut.GetLatestAsync(Section, new ContentFilter(), TestContext.Current.CancellationToken);

        // Assert
        recorder.Results.Should().Equal("cache_miss", "cache_hit");
    }

    [Fact]
    public async Task GetLatestAsync_FreeTextFilter_RecordsCacheBypassMetric()
    {
        // Arrange
        const string Section = "telemetry-bypass-section";
        using var recorder = new RequestMetricRecorder(Section);
        SetupApi(Section, skip: 0, items: [], totalCount: 0, query: "agents");
        var sut = CreateSut();

        // Act
        await sut.GetLatestAsync(Section, new ContentFilter { Query = "agents" }, TestContext.Current.CancellationToken);

        // Assert
        recorder.Results.Should().Equal("cache_bypass");
    }

    private LatestContentProvider CreateSut(string baseUrl = "https://techhub.test") =>
        new(_apiClient.Object, _cache, new SectionCacheBuilder().Build(), Options.Create(new McpOptions { BaseUrl = baseUrl }), new FrozenTimeProvider(_now));

    private void SetupApi(
        string section,
        int skip,
        IReadOnlyList<ContentItem> items,
        long totalCount,
        string? query = null,
        string? tags = null,
        int? lastDays = null,
        string? fromDate = null,
        string? types = null)
    {
        _apiClient
            .Setup(c => c.GetCollectionItemsAsync(
                section, "all", 10, skip, query, tags, lastDays, fromDate, null, types, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse(items, totalCount));
    }

    private void VerifyApiCalls(Times times) =>
        _apiClient.Verify(c => c.GetCollectionItemsAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>(),
            It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
            It.IsAny<bool>(), It.IsAny<CancellationToken>()), times);

    private sealed class FrozenTimeProvider : TimeProvider
    {
        private readonly DateTimeOffset _utcNow;

        public FrozenTimeProvider(DateTimeOffset utcNow) => _utcNow = utcNow;

        public override DateTimeOffset GetUtcNow() => _utcNow;
    }

    private sealed class RequestMetricRecorder : IDisposable
    {
        private readonly MeterListener _listener = new();
        private readonly List<string> _results = [];

        public RequestMetricRecorder(string section)
        {
            _listener.InstrumentPublished = (instrument, l) =>
            {
                if (instrument.Meter.Name == McpTelemetry.Name)
                {
                    l.EnableMeasurementEvents(instrument);
                }
            };
            _listener.SetMeasurementEventCallback<long>((instrument, _, tags, _) =>
            {
                if (instrument.Name != McpTelemetry.RequestsMetricName)
                {
                    return;
                }

                string? tagSection = null;
                string? result = null;
                foreach (var tag in tags)
                {
                    if (tag.Key == "techhub.section")
                    {
                        tagSection = (string?)tag.Value;
                    }
                    else if (tag.Key == "techhub.result")
                    {
                        result = (string?)tag.Value;
                    }
                }

                if (tagSection == section && result is not null)
                {
                    lock (_results)
                    {
                        _results.Add(result);
                    }
                }
            });
            _listener.Start();
        }

        public IReadOnlyList<string> Results
        {
            get
            {
                lock (_results)
                {
                    return [.. _results];
                }
            }
        }

        public void Dispose() => _listener.Dispose();
    }
}
