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
        copilot.ParentName.Should().Be("GitHub");
        foreach (var child in new[] { "GitHub Copilot CLI", "GitHub Copilot Workspace" })
        {
            var summary = products.Should().Contain(p => p.Name == child).Subject;
            summary.ParentId.Should().Be(copilot.Id);
            summary.ParentName.Should().Be("GitHub Copilot");
        }
    }

    [Fact]
    public async Task StartingCatalog_GroupsTheFamiliesUnderTheirProducts()
    {
        var all = await _sut.GetProductsAsync(null, Ct);

        var parents = all.ToDictionary(p => p.Name, p => p.ParentName);
        parents["GitHub Actions"].Should().Be("GitHub");
        parents["GitHub CodeQL"].Should().Be("GitHub Advanced Security");
        parents["GitHub Enterprise Cloud"].Should().Be("GitHub Enterprise");
        parents["Azure Pipelines"].Should().Be("Azure DevOps");
        parents["Azure Blob Storage"].Should().Be("Azure Storage");
        parents["Fabric Notebook Toolkit"].Should().Be("Microsoft Fabric");
        parents["GitHub MCP Server"].Should().Be("GitHub");
        parents["Microsoft Teams"].Should().Be("Microsoft 365");
        parents["GitHub"].Should().BeNull();
        parents["Microsoft Azure"].Should().BeNull();
        parents["Azure Functions"].Should().Be("Microsoft Azure");
        parents["Azure SDK"].Should().Be("Microsoft Azure");
        parents["Azure Pipelines"].Should().Be("Azure DevOps");
        parents["Azure DevOps"].Should().Be("Microsoft Azure");
        parents["Microsoft Windows Server"].Should().Be("Microsoft Windows");
        parents["Windows Terminal"].Should().Be("Microsoft Windows");
        parents["ASP.NET Core"].Should().Be(".NET");
        parents["Microsoft Entra ID"].Should().Be("Microsoft Azure");
        parents["Microsoft Foundry"].Should().Be("Microsoft Azure");
        parents["Foundry Local"].Should().Be("Microsoft Foundry");
        parents["Microsoft Copilot Studio"].Should().Be("Microsoft Power Platform");
        parents["Microsoft Copilot"].Should().BeNull();
        parents.Should().NotContainKey("Microsoft Entra Agent ID", "it is a feature of Microsoft Entra ID");
    }

    [Fact]
    public async Task StartingCatalog_MicrosoftEntraAgentIdIsAFeatureOfMicrosoftEntraId()
    {
        var entra = (await _sut.GetProductsAsync("Microsoft Entra ID", Ct)).Single(p => p.Name == "Microsoft Entra ID");

        var features = (await new ProductFeatureRepository(_fixture.Connection, NullLogger<ProductFeatureRepository>.Instance).GetOverviewAsync(entra.Id, Ct)).Features;

        features.Should().ContainSingle(f => f.Name == "Agent ID").Which.Aliases.Select(a => a.Alias)
            .Should().Contain(["Microsoft Entra Agent ID", "Entra Agent ID"]);
        (await _sut.GetDetailAsync(entra.Id, Ct))!.Aliases.Select(a => a.Alias).Should().NotContain(["Microsoft Entra Agent ID", "Entra Agent ID"]);
    }

    [Fact]
    public async Task StartingCatalog_HasPopularProductsAndTheAliasesOfTheirShortNames()
    {
        var all = await _sut.GetProductsAsync(null, Ct);

        all.Select(p => p.Name).Should().Contain(["GitHub Codespaces", "Model Context Protocol", "Azure OpenAI Service", "Microsoft 365 Copilot", "TypeScript"]);
        all.Single(p => p.Name == "Model Context Protocol").Aliases.Select(a => a.Alias).Should().Contain("MCP");
        all.Single(p => p.Name == "Microsoft SQL Server").Aliases.Select(a => a.Alias).Should().Contain("Microsoft SQL");
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

        var articles = (await _sut.GetArticlesAsync(parent.Id, true, 1, 100, Ct)).Items;
        var events = (await _sut.GetEventsAsync(parent.Id, true, 1, 100, Ct)).Items;

        articles.Select(a => a.Slug).Should().BeEquivalentTo(parentArticle, childArticle, grandchildArticle);
        articles.Single(a => a.Slug == childArticle).CatalogName.Should().Be(child.Name);
        events.Should().ContainSingle(e => e.ItemSlug == childArticle).Which.CatalogName.Should().Be(child.Name);

        var childArticles = (await _sut.GetArticlesAsync(child.Id, true, 1, 100, Ct)).Items;
        childArticles.Select(a => a.Slug).Should().BeEquivalentTo(childArticle, grandchildArticle);

        var detail = (await _sut.GetDetailAsync(parent.Id, Ct))!;
        (detail.ArticleCount, detail.EventCount).Should().Be((3, 1));
    }

    [Fact]
    public async Task GetArticlesAndEvents_WithoutChildren_ListOnlyTheProductItself_AndPageTheResults()
    {
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync();
        (await _sut.SetParentAsync(child.Id, parent.Id, Ct)).Succeeded.Should().BeTrue();
        var parentArticle = await SeedItemAsync();
        var childArticles = new[] { await SeedItemAsync(), await SeedItemAsync(), await SeedItemAsync() };
        await WriteProductsAsync(parentArticle, parent.Name);
        foreach (var slug in childArticles)
        {
            await WriteProductsAsync(slug, child.Name);
            await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(child.Name)], "source", 2, Ct);
        }

        var own = await _sut.GetArticlesAsync(parent.Id, includeChildren: false, 1, 10, Ct);
        var ownEvents = await _sut.GetEventsAsync(parent.Id, includeChildren: false, 1, 10, Ct);
        var firstPage = await _sut.GetArticlesAsync(parent.Id, includeChildren: true, 1, 3, Ct);
        var secondPage = await _sut.GetArticlesAsync(parent.Id, includeChildren: true, 2, 3, Ct);
        var eventsPage = await _sut.GetEventsAsync(parent.Id, includeChildren: true, 2, 2, Ct);

        own.Items.Select(a => a.Slug).Should().Equal(parentArticle);
        own.TotalCount.Should().Be(1);
        ownEvents.TotalCount.Should().Be(0);
        firstPage.TotalCount.Should().Be(4);
        firstPage.Items.Should().HaveCount(3);
        secondPage.Items.Should().HaveCount(1);
        firstPage.Items.Concat(secondPage.Items).Select(a => a.Slug).Should().OnlyHaveUniqueItems();
        eventsPage.TotalCount.Should().Be(3);
        eventsPage.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetArticles_CapsThePageSize()
    {
        var product = await CreateProductAsync();

        var page = await _sut.GetArticlesAsync(product.Id, true, 1, int.MaxValue, Ct);

        page.Items.Should().BeEmpty();
        page.TotalCount.Should().Be(0);
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
    public async Task GetProducts_CountsTheChildProductsAndTheFeaturesOfEachProduct()
    {
        var parent = await CreateProductAsync();
        var child = await CreateProductAsync();
        var grandchild = await CreateProductAsync();
        (await _sut.SetParentAsync(child.Id, parent.Id, Ct)).Succeeded.Should().BeTrue();
        (await _sut.SetParentAsync(grandchild.Id, child.Id, Ct)).Succeeded.Should().BeTrue();
        await _fixture.Connection.ExecuteAsync(
            "INSERT INTO product_features (product_id, name) VALUES (@ParentId, 'Agent mode'), (@ParentId, 'Plan mode'), (@ChildId, 'CLI mode')",
            new { ParentId = parent.Id, ChildId = child.Id });

        var parentSummary = (await _sut.GetProductsAsync(parent.Name, Ct)).Single(p => p.Id == parent.Id);
        var childSummary = (await _sut.GetProductsAsync(child.Name, Ct)).Single(p => p.Id == child.Id);

        (parentSummary.ChildCount, parentSummary.FeatureCount).Should().Be((1, 2), "only direct children and own features are counted");
        (childSummary.ChildCount, childSummary.FeatureCount).Should().Be((1, 1));
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
