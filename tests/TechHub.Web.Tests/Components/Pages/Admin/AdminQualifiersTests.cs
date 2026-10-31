using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models.Admin;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminQualifiers.razor: the review queue of unmatched plans and surfaces and the vocabulary itself.
/// </summary>
public class AdminQualifiersTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminQualifiersTests()
    {
        Services.AddSingleton(_api.Object);
        Setup(qualifiers: [], unmatched: []);
    }

    [Fact]
    public void Render_ShowsTheVocabularyPerKindAndTheReviewQueue()
    {
        // Arrange
        Setup(
            qualifiers:
            [
                Qualifier(1, "plan", "Business", events: 4, items: 2, aliases: ["Copilot Business"]),
                Qualifier(2, "surface", "Visual Studio Code", aliases: ["VS Code", "VSCode"])
            ],
            unmatched: [new UnmatchedQualifierName { Kind = "surface", Name = "Cursor", EventCount = 3 }]);

        // Act
        var cut = Render<AdminQualifiers>();

        // Assert
        cut.Markup.Should().Contain("To review (1)").And.Contain("Plans (1)").And.Contain("Surfaces (1)");
        cut.FindAll("#unmatched-table tbody tr").Should().ContainSingle().Which.TextContent.Should().Contain("Cursor").And.Contain("surface");
        var business = cut.Find("#qualifier-1 summary").TextContent;
        business.Should().Contain("Business").And.Contain("4 dates, 2 articles").And.Contain("Copilot Business");
        cut.Find("#qualifier-2 summary").TextContent.Should().Contain("VS Code, VSCode");
    }

    [Fact]
    public void Render_NothingToReview_ShowsEmptyMessage()
    {
        // Act
        var cut = Render<AdminQualifiers>();

        // Assert
        cut.Find("#unmatched-empty").TextContent.Should().Contain("Nothing to review.");
    }

    [Fact]
    public void Render_ApiFails_ShowsErrorAlert()
    {
        // Arrange
        _api.Setup(x => x.GetQualifiersAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("down"));

        // Act
        var cut = Render<AdminQualifiers>();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load plans and surfaces: down");
    }

    [Fact]
    public void Add_CreatesTheQualifierOfTheChosenKind()
    {
        // Arrange
        _api.Setup(x => x.CreateQualifierAsync("surface", "Neovim", It.IsAny<CancellationToken>())).ReturnsAsync(Qualifier(9, "surface", "Neovim"));
        var cut = Render<AdminQualifiers>();
        cut.Find("#newKind").Change("surface");
        cut.Find("#newName").Change("Neovim");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Add").Click();

        // Assert
        _api.Verify(x => x.CreateQualifierAsync("surface", "Neovim", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("'Neovim' added.");
    }

    [Fact]
    public void Add_Refused_ShowsTheServerMessage()
    {
        // Arrange
        _api.Setup(x => x.CreateQualifierAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("'Business' is already used by 'Business'."));
        var cut = Render<AdminQualifiers>();
        cut.Find("#newName").Change("Business");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Add").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("already used");
    }

    [Fact]
    public void ReviewQueue_AddAsQualifier_CreatesItWithTheKindOfTheName()
    {
        // Arrange
        Setup(qualifiers: [], unmatched: [new UnmatchedQualifierName { Kind = "surface", Name = "Cursor", EventCount = 3 }]);
        _api.Setup(x => x.CreateQualifierAsync("surface", "Cursor", It.IsAny<CancellationToken>())).ReturnsAsync(Qualifier(9, "surface", "Cursor"));
        var cut = Render<AdminQualifiers>();

        // Act
        cut.Find("#unmatched-table button").Click();

        // Assert
        _api.Verify(x => x.CreateQualifierAsync("surface", "Cursor", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("'Cursor' added as surface.");
    }

    [Fact]
    public void ReviewQueue_AddAsAlias_AddsTheNameToTheChosenQualifierOfTheSameKind()
    {
        // Arrange
        Setup(
            qualifiers: [Qualifier(1, "plan", "Pro"), Qualifier(2, "surface", "Visual Studio Code")],
            unmatched: [new UnmatchedQualifierName { Kind = "surface", Name = "Code", EventCount = 1 }]);
        _api.Setup(x => x.AddQualifierAliasAsync(2, "Code", It.IsAny<CancellationToken>())).ReturnsAsync(Qualifier(2, "surface", "Visual Studio Code"));
        var cut = Render<AdminQualifiers>();
        cut.Find("#unmatched-table input").Change("visual studio code");

        // Act
        cut.FindAll("#unmatched-table button").Single(b => b.TextContent == "Add as alias").Click();

        // Assert
        _api.Verify(x => x.AddQualifierAliasAsync(2, "Code", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("'Code' is now an alias of 'Visual Studio Code'.");
    }

    [Fact]
    public void ReviewQueue_AddAsAlias_OfAQualifierOfAnotherKind_ShowsErrorWithoutCallingTheApi()
    {
        // Arrange
        Setup(
            qualifiers: [Qualifier(1, "plan", "Pro")],
            unmatched: [new UnmatchedQualifierName { Kind = "surface", Name = "Code", EventCount = 1 }]);
        var cut = Render<AdminQualifiers>();
        cut.Find("#unmatched-table input").Change("Pro");

        // Act
        cut.FindAll("#unmatched-table button").Single(b => b.TextContent == "Add as alias").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("'Pro' is not a surface.");
        _api.Verify(x => x.AddQualifierAliasAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Rename_CallsTheApiWithTheNewName()
    {
        // Arrange
        Setup(qualifiers: [Qualifier(1, "plan", "Pro+")], unmatched: []);
        _api.Setup(x => x.RenameQualifierAsync(1, "Pro Plus", It.IsAny<CancellationToken>())).ReturnsAsync(Qualifier(1, "plan", "Pro Plus"));
        var cut = Render<AdminQualifiers>();
        cut.Find("#rename-1").Change("Pro Plus");

        // Act
        cut.FindAll("#qualifier-1 button").Single(b => b.TextContent == "Rename").Click();

        // Assert
        _api.Verify(x => x.RenameQualifierAsync(1, "Pro Plus", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Renamed to 'Pro Plus'.");
    }

    [Fact]
    public void AddAndRemoveAlias_CallTheApi()
    {
        // Arrange
        Setup(qualifiers: [Qualifier(1, "surface", "Visual Studio Code", aliases: ["VS Code"])], unmatched: []);
        _api.Setup(x => x.AddQualifierAliasAsync(1, "Code", It.IsAny<CancellationToken>())).ReturnsAsync(Qualifier(1, "surface", "Visual Studio Code"));
        var cut = Render<AdminQualifiers>();
        cut.Find("#alias-1").Change("Code");

        // Act
        cut.FindAll("#qualifier-1 button").Single(b => b.TextContent == "Add alias").Click();
        cut.FindAll("#qualifier-1 li button").Single(b => b.TextContent == "Remove").Click();

        // Assert
        _api.Verify(x => x.AddQualifierAliasAsync(1, "Code", It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.RemoveQualifierAliasAsync(1, 100, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Delete_AsksForConfirmationBeforeCallingTheApi()
    {
        // Arrange
        Setup(qualifiers: [Qualifier(1, "plan", "Pro")], unmatched: []);
        var cut = Render<AdminQualifiers>();

        // Act
        cut.FindAll("#qualifier-1 button").Single(b => b.TextContent == "Delete…").Click();

        // Assert — nothing happens until confirmed
        cut.Find("#qualifier-1 .admin-alert-error").TextContent.Should().Contain("Delete 'Pro'?");
        _api.Verify(x => x.DeleteQualifierAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);

        // Act
        cut.FindAll("#qualifier-1 button").Single(b => b.TextContent == "Confirm delete").Click();

        // Assert
        _api.Verify(x => x.DeleteQualifierAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("'Pro' deleted.");
    }

    private void Setup(IReadOnlyList<ProductQualifier> qualifiers, IReadOnlyList<UnmatchedQualifierName> unmatched)
    {
        _api.Setup(x => x.GetQualifiersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(qualifiers);
        _api.Setup(x => x.GetUnmatchedQualifierNamesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(unmatched);
    }

    private static ProductQualifier Qualifier(long id, string kind, string name, int events = 0, int items = 0, string[]? aliases = null) => new()
    {
        Id = id,
        Kind = kind,
        Name = name,
        EventCount = events,
        ItemCount = items,
        Aliases = (aliases ?? []).Select((alias, index) => new QualifierAlias { Id = 100 + index, Alias = alias }).ToList()
    };
}
