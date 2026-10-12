using System.Diagnostics.Metrics;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.MCP;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.MCP;

public sealed class LatestContentProviderTests : IDisposable
{
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
        var result = await sut.GetLatestAsync("ai", page: 3, TestContext.Current.CancellationToken);

        // Assert
        result.Page.Should().Be(3);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(25);
        result.TotalPages.Should().Be(3);
        _apiClient.Verify(c => c.GetCollectionItemsAsync(
            "ai", "all", 10, 20, null, null, null, null, null, null, false, It.IsAny<CancellationToken>()), Times.Once);
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
        var result = await sut.GetLatestAsync("dotnet", page: 1, TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().ContainSingle()
            .Which.Url.Should().Be("https://techhub.test/dotnet/blogs/my-slug");
    }

    [Fact]
    public async Task GetLatestAsync_SamePageTwice_CallsApiOnce()
    {
        // Arrange
        SetupApi("azure", skip: 0, items: [new ContentItemBuilder().Build()], totalCount: 1);
        var sut = CreateSut();

        // Act
        var first = await sut.GetLatestAsync("azure", page: 1, TestContext.Current.CancellationToken);
        var second = await sut.GetLatestAsync("azure", page: 1, TestContext.Current.CancellationToken);

        // Assert
        second.Should().BeEquivalentTo(first);
        _apiClient.Verify(c => c.GetCollectionItemsAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>(),
            It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
            It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
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
        var result = await sut.GetLatestAsync("azure", page: 1, TestContext.Current.CancellationToken);

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
        await sut.GetLatestAsync("ml", page: 1, TestContext.Current.CancellationToken);
        await sut.GetLatestAsync("ml", page: 2, TestContext.Current.CancellationToken);
        await sut.GetLatestAsync("security", page: 1, TestContext.Current.CancellationToken);

        // Assert
        _apiClient.Verify(c => c.GetCollectionItemsAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>(),
            It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
            It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
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
        var failing = () => sut.GetLatestAsync("devops", page: 1, TestContext.Current.CancellationToken);
        await failing.Should().ThrowAsync<HttpRequestException>();
        var result = await sut.GetLatestAsync("devops", page: 1, TestContext.Current.CancellationToken);

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
        var result = await sut.GetLatestAsync("ai", requestedPage, TestContext.Current.CancellationToken);

        // Assert
        result.Page.Should().Be(expectedPage);
        _apiClient.VerifyAll();
    }

    [Fact]
    public async Task GetLatestAsync_RecordsCacheMissThenCacheHitMetric()
    {
        // Arrange - unique section so parallel tests sharing the static meter do not interfere
        const string Section = "telemetry-test-section";
        var results = new List<string>();
        using var listener = new MeterListener();
        listener.InstrumentPublished = (instrument, l) =>
        {
            if (instrument.Meter.Name == McpTelemetry.Name)
            {
                l.EnableMeasurementEvents(instrument);
            }
        };
        listener.SetMeasurementEventCallback<long>((instrument, _, tags, _) =>
        {
            if (instrument.Name != McpTelemetry.RequestsMetricName)
            {
                return;
            }

            string? section = null;
            string? result = null;
            foreach (var tag in tags)
            {
                if (tag.Key == "techhub.section")
                {
                    section = (string?)tag.Value;
                }
                else if (tag.Key == "techhub.result")
                {
                    result = (string?)tag.Value;
                }
            }

            if (section == Section && result is not null)
            {
                lock (results)
                {
                    results.Add(result);
                }
            }
        });
        listener.Start();

        SetupApi(Section, skip: 0, items: [new ContentItemBuilder().Build()], totalCount: 1);
        var sut = CreateSut();

        // Act
        await sut.GetLatestAsync(Section, page: 1, TestContext.Current.CancellationToken);
        await sut.GetLatestAsync(Section, page: 1, TestContext.Current.CancellationToken);

        // Assert
        lock (results)
        {
            results.Should().Equal("cache_miss", "cache_hit");
        }
    }

    private LatestContentProvider CreateSut(string baseUrl = "https://techhub.test") =>
        new(_apiClient.Object, _cache, Options.Create(new McpOptions { BaseUrl = baseUrl }));

    private void SetupApi(string section, int skip, IReadOnlyList<ContentItem> items, long totalCount)
    {
        _apiClient
            .Setup(c => c.GetCollectionItemsAsync(
                section, "all", 10, skip, null, null, null, null, null, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse(items, totalCount));
    }
}
