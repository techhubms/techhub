using System.Globalization;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for the product hierarchy of <see cref="ProductRepository"/>: a product can have a parent
/// (GitHub Copilot CLI belongs to GitHub Copilot) and a parent shows the articles and dates of its children.
/// </summary>
public class ProductHierarchyTests : IClassFixture<DatabaseFixture<ProductHierarchyTests>>
{
    private readonly DatabaseFixture<ProductHierarchyTests> _fixture;
    private readonly ProductRepository _sut;
    private readonly ContentItemWriteRepository _writeRepository;
    private readonly TimelineEventRepository _timelineRepository;

    public ProductHierarchyTests(DatabaseFixture<ProductHierarchyTests> fixture)
    {
        _fixture = fixture;
        _sut = new ProductRepository(fixture.Connection, NullLogger<ProductRepository>.Instance);
        _writeRepository = new ContentItemWriteRepository(fixture.Connection, NullLogger<ContentItemWriteRepository>.Instance);
        _timelineRepository = new TimelineEventRepository(fixture.Connection);
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task StartingCatalog_PutsTheCopilotProductsUnderGitHubCopilot()
    {
        var products = await _sut.GetProductsAsync("GitHub Copilot", Ct);

        var copilot = products.Should().Contain(p => p.Name == "GitHub Copilot").Subject;
        copilot.ParentId.Should().BeNull();
        foreach (var child in new[] { "GitHub Copilot CLI", "GitHub Copilot coding agent", "GitHub Copilot Fusion" })
        {
            var summary = products.Should().Contain(p => p.Name == child).Subject;
            summary.ParentId.Should().Be(copilot.Id);
            summary.ParentName.Should().Be("GitHub Copilot");
        }
    }

    [Fact]
    public async Task SetParent_ShowsTheParentAndChildrenOnTheDetail()
    {
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync();

        var result = await _sut.SetParentAsync(child.Id, parent.Id, Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        var childDetail = (await _sut.GetDetailAsync(child.Id, Ct))!;
        childDetail.Parent.Should().NotBeNull();
        childDetail.Parent!.Id.Should().Be(parent.Id);
        childDetail.Parent.Name.Should().Be(parent.Name);
        var parentDetail = (await _sut.GetDetailAsync(parent.Id, Ct))!;
        parentDetail.Parent.Should().BeNull();
        parentDetail.Children.Should().ContainSingle().Which.Id.Should().Be(child.Id);
    }

    [Fact]
    public async Task SetParent_WithNull_MakesTheProductTopLevelAgain()
    {
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync();
        (await _sut.SetParentAsync(child.Id, parent.Id, Ct)).Succeeded.Should().BeTrue();

        var result = await _sut.SetParentAsync(child.Id, null, Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        (await _sut.GetDetailAsync(child.Id, Ct))!.Parent.Should().BeNull();
    }

    [Fact]
    public async Task SetParent_ToItself_IsInvalid()
    {
        var product = await CreateProductAsync();

        var result = await _sut.SetParentAsync(product.Id, product.Id, Ct);

        result.Status.Should().Be(ProductOperationStatus.Invalid);
    }

    [Fact]
    public async Task SetParent_ToADescendant_IsInvalid()
    {
        var grandparent = await CreateProductAsync();
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync();
        (await _sut.SetParentAsync(parent.Id, grandparent.Id, Ct)).Succeeded.Should().BeTrue();
        (await _sut.SetParentAsync(child.Id, parent.Id, Ct)).Succeeded.Should().BeTrue();

        var result = await _sut.SetParentAsync(grandparent.Id, child.Id, Ct);

        result.Status.Should().Be(ProductOperationStatus.Invalid);
        (await _sut.GetDetailAsync(grandparent.Id, Ct))!.Parent.Should().BeNull();
    }

    [Fact]
    public async Task SetParent_WithUnknownProductOrParent_IsNotFound()
    {
        var product = await CreateProductAsync();

        (await _sut.SetParentAsync(long.MaxValue, product.Id, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.SetParentAsync(product.Id, long.MaxValue, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
    }

    [Fact]
    public async Task GetDetail_OfAParent_IncludesTheArticlesAndDatesOfItsDescendants()
    {
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync();
        var grandchild = await CreateProductAsync();
        (await _sut.SetParentAsync(child.Id, parent.Id, Ct)).Succeeded.Should().BeTrue();
        (await _sut.SetParentAsync(grandchild.Id, child.Id, Ct)).Succeeded.Should().BeTrue();
        var parentArticle = await SeedItemAsync();
        var childArticle = await SeedItemAsync();
        var grandchildArticle = await SeedItemAsync();
        await WriteProductsAsync(parentArticle, parent.Name);
        await WriteProductsAsync(childArticle, child.Name);
        await WriteProductsAsync(grandchildArticle, grandchild.Name);
        await _timelineRepository.ReplaceEventsAsync("news", childArticle, [Event(child.Name)], "source", 2, Ct);

        var detail = (await _sut.GetDetailAsync(parent.Id, Ct))!;

        detail.Articles.Select(a => a.Slug).Should().BeEquivalentTo(parentArticle, childArticle, grandchildArticle);
        detail.Articles.Single(a => a.Slug == childArticle).CatalogName.Should().Be(child.Name);
        detail.Events.Should().ContainSingle(e => e.ItemSlug == childArticle).Which.CatalogName.Should().Be(child.Name);

        var childDetail = (await _sut.GetDetailAsync(child.Id, Ct))!;
        childDetail.Articles.Select(a => a.Slug).Should().BeEquivalentTo(childArticle, grandchildArticle);
    }

    [Fact]
    public async Task GetProducts_CountsTheArticlesAndDatesOfDescendantsOncePerParent()
    {
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync();
        (await _sut.SetParentAsync(child.Id, parent.Id, Ct)).Succeeded.Should().BeTrue();
        var shared = await SeedItemAsync();
        var childOnly = await SeedItemAsync();
        await WriteProductsAsync(shared, parent.Name, child.Name);
        await WriteProductsAsync(childOnly, child.Name);
        await _timelineRepository.ReplaceEventsAsync("news", childOnly, [Event(child.Name), Event(parent.Name, kind: "preview")], "source", 2, Ct);

        var products = await _sut.GetProductsAsync(parent.Name, Ct);

        var summary = products.Should().ContainSingle(p => p.Id == parent.Id).Subject;
        summary.ItemCount.Should().Be(2);
        summary.EventCount.Should().Be(2);
    }

    [Fact]
    public async Task Delete_OfAParent_MakesItsChildrenTopLevel()
    {
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync();
        (await _sut.SetParentAsync(child.Id, parent.Id, Ct)).Succeeded.Should().BeTrue();

        (await _sut.DeleteAsync(parent.Id, Ct)).Succeeded.Should().BeTrue();

        (await _sut.GetDetailAsync(child.Id, Ct))!.Parent.Should().BeNull();
    }

    [Fact]
    public async Task Merge_MovesTheChildrenOfTheSourceToTheTarget()
    {
        var source = await CreateProductAsync();
        var target = await CreateProductAsync();
        var child = await CreateProductAsync();
        (await _sut.SetParentAsync(child.Id, source.Id, Ct)).Succeeded.Should().BeTrue();

        var result = await _sut.MergeAsync(source.Id, target.Id, Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        (await _sut.GetDetailAsync(child.Id, Ct))!.Parent!.Id.Should().Be(target.Id);
    }

    [Fact]
    public async Task Merge_OfAParentIntoItsOwnChild_KeepsTheChildAsTheTarget()
    {
        var grandparent = await CreateProductAsync();
        var source = await CreateProductAsync();
        var target = await CreateProductAsync();
        (await _sut.SetParentAsync(source.Id, grandparent.Id, Ct)).Succeeded.Should().BeTrue();
        (await _sut.SetParentAsync(target.Id, source.Id, Ct)).Succeeded.Should().BeTrue();

        var result = await _sut.MergeAsync(source.Id, target.Id, Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        (await _sut.GetDetailAsync(target.Id, Ct))!.Parent!.Id.Should().Be(grandparent.Id);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string UniqueName(string prefix) => $"{prefix}-{Guid.NewGuid():N}";

    private static TimelineEvent Event(string product, string kind = "ga") => new()
    {
        Kind = kind,
        Product = product,
        Feature = null,
        Description = $"{kind} of {product}",
        Date = DateOnly.Parse("2026-03-10", CultureInfo.InvariantCulture),
        DatePrecision = "day",
        DateSource = "explicit",
        Quote = null
    };

    private async Task<string> SeedItemAsync()
    {
        var slug = $"hierarchy-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash, sections_bitmask, created_at)
            VALUES (@Slug, 'news', 'Hierarchy test', '', '', 1743278400, 'ai',
                    @Url, 'Author', 'Feed', '', 'hash', 1, NOW())
            """,
            new { Slug = slug, Url = $"https://example.com/{slug}" });
        return slug;
    }

    private Task WriteProductsAsync(string slug, params string[] products) =>
        _writeRepository.UpdateAiMetadataAsync(
            "news",
            slug,
            ContentItemWriteRepository.SerializeAiMetadata(new RoundupMetadata { SchemaVersion = 2, Products = products }),
            Ct);

    private async Task<ProductSummary> CreateProductAsync()
    {
        var name = UniqueName("Product");
        var result = await _sut.CreateAsync(name, Ct);
        result.Succeeded.Should().BeTrue(result.Message);
        return new ProductSummary { Id = result.ProductId!.Value, Name = name };
    }
}
