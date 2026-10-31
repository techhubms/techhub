using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the plan and surface endpoints under /api/admin/qualifiers and for the aliases of products
/// that imply a plan or surface. Names are unique per test because the database is shared by the tests of this class.
/// </summary>
public class AdminQualifiersEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public AdminQualifiersEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    private static string UniqueName(string prefix) => $"{prefix}-{Guid.NewGuid():N}";

    private async Task<ProductQualifier> CreateAsync(string kind = "plan", string? name = null)
    {
        var response = await _client.PostAsJsonAsync("/api/admin/qualifiers", new { kind, name = name ?? UniqueName("Qualifier") }, Ct);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<ProductQualifier>(Ct))!;
    }

    [Fact]
    public async Task GetQualifiers_ReturnsTheStartingVocabularyWithAliases()
    {
        var qualifiers = await _client.GetFromJsonAsync<List<ProductQualifier>>("/api/admin/qualifiers", Ct);

        qualifiers.Should().Contain(q => q.Kind == "plan" && q.Name == "Business");
        qualifiers.Should().Contain(q => q.Kind == "surface" && q.Name == "Visual Studio Code" && q.Aliases.Any(a => a.Alias == "VS Code"));
    }

    [Fact]
    public async Task GetUnmatched_ReturnsAList()
    {
        var response = await _client.GetAsync("/api/admin/qualifiers/unmatched", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<List<UnmatchedQualifierName>>(Ct)).Should().NotBeNull();
    }

    [Fact]
    public async Task CreateQualifier_ReturnsCreatedWithLocation()
    {
        var name = UniqueName("Created");

        var response = await _client.PostAsJsonAsync("/api/admin/qualifiers", new { kind = "surface", name = $"  {name}  " }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var qualifier = await response.Content.ReadFromJsonAsync<ProductQualifier>(Ct);
        qualifier!.Name.Should().Be(name);
        qualifier.Kind.Should().Be("surface");
        response.Headers.Location!.ToString().Should().Be($"/api/admin/qualifiers/{qualifier.Id}");
    }

    [Theory]
    [InlineData("""{"kind":"tier","name":"X"}""")]
    [InlineData("""{"kind":"plan","name":"   "}""")]
    [InlineData("""{"kind":"plan"}""")]
    [InlineData("""{}""")]
    public async Task CreateQualifier_WithInvalidInput_ReturnsBadRequest(string body)
    {
        using var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/admin/qualifiers", content, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateQualifier_WithExistingName_ReturnsConflictWithMessage()
    {
        var response = await _client.PostAsJsonAsync("/api/admin/qualifiers", new { kind = "plan", name = "business" }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        (await response.Content.ReadAsStringAsync(Ct)).Should().Contain("already used");
    }

    [Fact]
    public async Task RenameQualifier_KeepsTheOldNameAsAlias()
    {
        var qualifier = await CreateAsync();
        var newName = UniqueName("Renamed");

        var response = await _client.PutAsJsonAsync($"/api/admin/qualifiers/{qualifier.Id}", new { name = newName }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var renamed = await response.Content.ReadFromJsonAsync<ProductQualifier>(Ct);
        renamed!.Name.Should().Be(newName);
        renamed.Aliases.Select(a => a.Alias).Should().Equal(qualifier.Name);
    }

    [Fact]
    public async Task RenameQualifier_ToNameOfAnotherQualifier_ReturnsConflict()
    {
        var first = await CreateAsync();
        var second = await CreateAsync();

        var response = await _client.PutAsJsonAsync($"/api/admin/qualifiers/{second.Id}", new { name = first.Name }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task RenameAndDelete_UnknownQualifier_ReturnNotFound()
    {
        (await _client.PutAsJsonAsync("/api/admin/qualifiers/999999999", new { name = "X" }, Ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await _client.DeleteAsync("/api/admin/qualifiers/999999999", Ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteQualifier_RemovesIt()
    {
        var qualifier = await CreateAsync();

        var deleted = await _client.DeleteAsync($"/api/admin/qualifiers/{qualifier.Id}", Ct);

        deleted.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var qualifiers = await _client.GetFromJsonAsync<List<ProductQualifier>>("/api/admin/qualifiers", Ct);
        qualifiers.Should().NotContain(q => q.Id == qualifier.Id);
    }

    [Fact]
    public async Task AddAndRemoveAlias_UpdatesTheQualifier()
    {
        var qualifier = await CreateAsync("surface");
        var alias = UniqueName("Alias");

        var added = await _client.PostAsJsonAsync($"/api/admin/qualifiers/{qualifier.Id}/aliases", new { alias }, Ct);

        added.StatusCode.Should().Be(HttpStatusCode.OK);
        var aliasId = (await added.Content.ReadFromJsonAsync<ProductQualifier>(Ct))!.Aliases.Should().ContainSingle(a => a.Alias == alias).Subject.Id;

        var removed = await _client.DeleteAsync($"/api/admin/qualifiers/{qualifier.Id}/aliases/{aliasId}", Ct);

        removed.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await _client.DeleteAsync($"/api/admin/qualifiers/{qualifier.Id}/aliases/{aliasId}", Ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddAlias_UsedByAnotherQualifier_ReturnsConflict()
    {
        var first = await CreateAsync();
        var second = await CreateAsync();

        var response = await _client.PostAsJsonAsync($"/api/admin/qualifiers/{second.Id}/aliases", new { alias = first.Name }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ── Product aliases that imply a plan or surface ──────────────────────────

    [Fact]
    public async Task AddProductAlias_WithAQualifier_ShowsItOnTheAlias_AndItCanBeChangedAndCleared()
    {
        var product = await CreateProductAsync();
        var plan = await CreateAsync();
        var alias = UniqueName("TierAlias");

        var added = await _client.PostAsJsonAsync($"/api/admin/products/{product.Id}/aliases", new { alias, qualifierId = plan.Id }, Ct);

        added.StatusCode.Should().Be(HttpStatusCode.OK);
        var productAlias = (await added.Content.ReadFromJsonAsync<ProductDetail>(Ct))!.Aliases.Should().ContainSingle().Subject;
        productAlias.QualifierName.Should().Be(plan.Name);
        productAlias.QualifierKind.Should().Be("plan");

        var other = await CreateAsync("surface");
        var changed = await _client.PutAsJsonAsync($"/api/admin/products/{product.Id}/aliases/{productAlias.Id}/qualifier", new { qualifierId = other.Id }, Ct);
        (await changed.Content.ReadFromJsonAsync<ProductDetail>(Ct))!.Aliases.Single().QualifierName.Should().Be(other.Name);

        var cleared = await _client.PutAsJsonAsync($"/api/admin/products/{product.Id}/aliases/{productAlias.Id}/qualifier", new { qualifierId = (long?)null }, Ct);
        (await cleared.Content.ReadFromJsonAsync<ProductDetail>(Ct))!.Aliases.Single().QualifierId.Should().BeNull();
    }

    [Fact]
    public async Task AddProductAlias_WithUnknownQualifier_ReturnsNotFound()
    {
        var product = await CreateProductAsync();

        var response = await _client.PostAsJsonAsync($"/api/admin/products/{product.Id}/aliases", new { alias = UniqueName("Alias"), qualifierId = 999999999 }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetAliasQualifier_ForUnknownAlias_ReturnsNotFound()
    {
        var product = await CreateProductAsync();

        var response = await _client.PutAsJsonAsync($"/api/admin/products/{product.Id}/aliases/999999999/qualifier", new { qualifierId = (long?)null }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<ProductDetail> CreateProductAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/admin/products", new { name = UniqueName("Product") }, Ct);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<ProductDetail>(Ct))!;
    }
}
