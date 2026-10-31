using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the product catalog endpoints under /api/admin/products.
/// Product names are unique per test because the database is shared by the tests of this class.
/// </summary>
public class AdminProductsEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public AdminProductsEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    private static string UniqueName(string prefix) => $"{prefix}-{Guid.NewGuid():N}";

    private async Task<ProductDetail> CreateAsync(string? name = null)
    {
        var response = await _client.PostAsJsonAsync("/api/admin/products", new { name = name ?? UniqueName("Product") }, Ct);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<ProductDetail>(Ct))!;
    }

    [Fact]
    public async Task GetProducts_ReturnsTheStartingCatalogWithAliases()
    {
        var products = await _client.GetFromJsonAsync<List<ProductSummary>>("/api/admin/products?search=Microsoft%20Foundry", Ct);

        products.Should().Contain(p => p.Name == "Microsoft Foundry" && p.Aliases.Any(a => a.Alias == "Azure AI Foundry"));
    }

    [Fact]
    public async Task GetProducts_WithTooLongSearch_ReturnsBadRequest()
    {
        var response = await _client.GetAsync($"/api/admin/products?search={new string('x', 201)}", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedDetailWithLocation()
    {
        var name = UniqueName("Created");

        var response = await _client.PostAsJsonAsync("/api/admin/products", new { name = $"  {name}  " }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var detail = await response.Content.ReadFromJsonAsync<ProductDetail>(Ct);
        detail!.Name.Should().Be(name);
        response.Headers.Location!.ToString().Should().Be($"/api/admin/products/{detail.Id}");
    }

    [Fact]
    public async Task CreateProduct_WithExistingName_ReturnsConflictWithMessage()
    {
        var existing = await CreateAsync();

        var response = await _client.PostAsJsonAsync("/api/admin/products", new { name = existing.Name.ToUpperInvariant() }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        (await response.Content.ReadAsStringAsync(Ct)).Should().Contain("already used");
    }

    [Theory]
    [InlineData("""{"name":""}""")]
    [InlineData("""{"name":"   "}""")]
    [InlineData("""{}""")]
    public async Task CreateProduct_WithoutName_ReturnsBadRequest(string body)
    {
        using var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/admin/products", content, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetProduct_Unknown_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/admin/products/999999999", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RenameProduct_KeepsTheOldNameAsAlias()
    {
        var product = await CreateAsync();
        var newName = UniqueName("Renamed");

        var response = await _client.PutAsJsonAsync($"/api/admin/products/{product.Id}", new { name = newName }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var detail = await response.Content.ReadFromJsonAsync<ProductDetail>(Ct);
        detail!.Name.Should().Be(newName);
        detail.Aliases.Select(a => a.Alias).Should().Equal(product.Name);
    }

    [Fact]
    public async Task RenameProduct_ToNameOfAnotherProduct_ReturnsConflict()
    {
        var first = await CreateAsync();
        var second = await CreateAsync();

        var response = await _client.PutAsJsonAsync($"/api/admin/products/{second.Id}", new { name = first.Name }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task AddAndRemoveAlias_UpdatesTheProduct()
    {
        var product = await CreateAsync();
        var alias = UniqueName("Alias");

        var added = await _client.PostAsJsonAsync($"/api/admin/products/{product.Id}/aliases", new { alias }, Ct);

        added.StatusCode.Should().Be(HttpStatusCode.OK);
        var detail = await added.Content.ReadFromJsonAsync<ProductDetail>(Ct);
        var aliasId = detail!.Aliases.Should().ContainSingle(a => a.Alias == alias).Subject.Id;

        var removed = await _client.DeleteAsync($"/api/admin/products/{product.Id}/aliases/{aliasId}", Ct);

        removed.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await _client.GetFromJsonAsync<ProductDetail>($"/api/admin/products/{product.Id}", Ct))!.Aliases.Should().BeEmpty();
    }

    [Fact]
    public async Task AddAlias_UsedByAnotherProduct_ReturnsConflict()
    {
        var first = await CreateAsync();
        var second = await CreateAsync();

        var response = await _client.PostAsJsonAsync($"/api/admin/products/{second.Id}/aliases", new { alias = first.Name }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task RemoveAlias_Unknown_ReturnsNotFound()
    {
        var product = await CreateAsync();

        var response = await _client.DeleteAsync($"/api/admin/products/{product.Id}/aliases/999999999", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task MergeProduct_MakesTheSourceAnAliasOfTheTarget()
    {
        var source = await CreateAsync();
        var target = await CreateAsync();

        var response = await _client.PostAsJsonAsync($"/api/admin/products/{source.Id}/merge", new { targetId = target.Id }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var detail = await response.Content.ReadFromJsonAsync<ProductDetail>(Ct);
        detail!.Id.Should().Be(target.Id);
        detail.Aliases.Select(a => a.Alias).Should().Equal(source.Name);
        (await _client.GetAsync($"/api/admin/products/{source.Id}", Ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task MergeProduct_IntoItself_ReturnsBadRequest()
    {
        var product = await CreateAsync();

        var response = await _client.PostAsJsonAsync($"/api/admin/products/{product.Id}/merge", new { targetId = product.Id }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteProduct_RemovesIt_AndUnknownReturnsNotFound()
    {
        var product = await CreateAsync();

        var deleted = await _client.DeleteAsync($"/api/admin/products/{product.Id}", Ct);
        var again = await _client.DeleteAsync($"/api/admin/products/{product.Id}", Ct);

        deleted.StatusCode.Should().Be(HttpStatusCode.NoContent);
        again.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task IgnoreAndUnignoreName_ChangesTheIgnoredNames()
    {
        var name = UniqueName("NotAProduct");

        var ignored = await _client.PostAsJsonAsync("/api/admin/products/ignored", new { name }, Ct);

        ignored.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await _client.GetFromJsonAsync<List<string>>("/api/admin/products/ignored", Ct)).Should().Contain(name);

        var unignored = await _client.DeleteAsync($"/api/admin/products/ignored?name={Uri.EscapeDataString(name)}", Ct);

        unignored.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await _client.GetFromJsonAsync<List<string>>("/api/admin/products/ignored", Ct)).Should().NotContain(name);
    }

    [Fact]
    public async Task IgnoreName_OfAProduct_ReturnsConflict()
    {
        var product = await CreateAsync();

        var response = await _client.PostAsJsonAsync("/api/admin/products/ignored", new { name = product.Name }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task UnignoreName_NotIgnored_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"/api/admin/products/ignored?name={UniqueName("Never")}", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUnmatched_ReturnsAList()
    {
        var response = await _client.GetAsync("/api/admin/products/unmatched", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<List<UnmatchedProductName>>(Ct)).Should().NotBeNull();
    }
}
