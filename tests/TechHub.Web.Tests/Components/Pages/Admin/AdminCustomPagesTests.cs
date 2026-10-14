using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models.Admin;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminCustomPages.razor together with its JsonEditorModal — list, open, validate and save JSON.
/// </summary>
public class AdminCustomPagesTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminCustomPagesTests()
    {
        Services.AddSingleton(_api.Object);
        SetupEntries([CreateEntry("levels", "Levels of enlightenment"), CreateEntry("handbook", "Handbook")]);
    }

    [Fact]
    public void Render_WithEntries_ShowsKeyDescriptionAndTimestamp()
    {
        // Act
        var cut = Render<AdminCustomPages>();

        // Assert
        var rows = cut.FindAll("tbody tr");
        rows.Should().HaveCount(2);
        rows[0].TextContent.Should().Contain("levels").And.Contain("Levels of enlightenment").And.Contain("2026-03-04 05:06");
    }

    [Fact]
    public void Render_NoEntries_ShowsEmptyMessage()
    {
        // Arrange
        SetupEntries([]);

        // Act
        var cut = Render<AdminCustomPages>();

        // Assert
        cut.Find(".admin-empty").TextContent.Should().Contain("No custom page data found in database.");
    }

    [Fact]
    public void Render_ApiFails_ShowsErrorAlert()
    {
        // Arrange
        _api.Setup(x => x.GetCustomPageEntriesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("down"));

        // Act
        var cut = Render<AdminCustomPages>();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load custom pages: down");
    }

    [Fact]
    public void Render_Initially_EditorIsClosed()
    {
        // Act
        var cut = Render<AdminCustomPages>();

        // Assert
        cut.FindAll(".json-editor-modal").Should().BeEmpty();
    }

    [Fact]
    public void Edit_Click_LoadsEntryAndOpensEditorWithJson()
    {
        // Arrange
        _api.Setup(x => x.GetCustomPageEntryAsync("levels", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateEntry("levels", "Levels of enlightenment", """{"a":1}"""));
        var cut = Render<AdminCustomPages>();

        // Act
        cut.FindAll("tbody .admin-link-btn")[0].Click();

        // Assert
        cut.Find("#json-editor-title").TextContent.Should().Be("Edit — Levels of enlightenment");
        GetTextAreaValue(cut.Find("textarea.json-editor-textarea")).Should().Be("""{"a":1}""");
    }

    [Fact]
    public void Edit_EntryNotFound_ShowsErrorMessageAndKeepsEditorClosed()
    {
        // Arrange
        _api.Setup(x => x.GetCustomPageEntryAsync("levels", It.IsAny<CancellationToken>())).ReturnsAsync((CustomPageEntry?)null);
        var cut = Render<AdminCustomPages>();

        // Act
        cut.FindAll("tbody .admin-link-btn")[0].Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Custom page 'levels' not found.");
        cut.FindAll(".json-editor-modal").Should().BeEmpty();
    }

    [Fact]
    public void Edit_LoadFails_ShowsErrorMessage()
    {
        // Arrange
        _api.Setup(x => x.GetCustomPageEntryAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminCustomPages>();

        // Act
        cut.FindAll("tbody .admin-link-btn")[0].Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load 'levels': boom");
    }

    [Fact]
    public void Save_InvalidJson_ShowsValidationErrorAndDoesNotCallApi()
    {
        // Arrange
        var cut = OpenEditor("levels", """{"a":1}""");
        cut.Find("textarea.json-editor-textarea").Change("{ not json");

        // Act
        cut.Find(".json-editor-footer .btn").Click();

        // Assert
        cut.Find(".json-editor-modal .admin-alert-error").TextContent.Should().Contain("Invalid JSON");
        _api.Verify(x => x.UpdateCustomPageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        cut.FindAll(".json-editor-modal").Should().ContainSingle();
    }

    [Fact]
    public void Save_ValidJson_UpdatesEntryClosesEditorAndRefreshesTimestamp()
    {
        // Arrange
        var updatedAt = new DateTimeOffset(2027, 1, 2, 3, 4, 0, TimeSpan.Zero);
        _api.Setup(x => x.UpdateCustomPageAsync("levels", """{"a":2}""", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateEntry("levels", "Levels of enlightenment", """{"a":2}""", updatedAt));
        var cut = OpenEditor("levels", """{"a":1}""");
        cut.Find("textarea.json-editor-textarea").Change("""{"a":2}""");

        // Act
        cut.Find(".json-editor-footer .btn").Click();

        // Assert
        _api.Verify(x => x.UpdateCustomPageAsync("levels", """{"a":2}""", It.IsAny<CancellationToken>()), Times.Once);
        cut.FindAll(".json-editor-modal").Should().BeEmpty();
        cut.Find(".admin-alert-success").TextContent.Should().Contain("'levels' saved successfully.");
        cut.FindAll("tbody tr")[0].TextContent.Should().Contain("2027-01-02 03:04");
        cut.FindAll("tbody tr")[1].TextContent.Should().Contain("2026-03-04 05:06");
    }

    [Fact]
    public void Save_ApiFails_ShowsSaveFailedInEditorAndKeepsItOpen()
    {
        // Arrange
        _api.Setup(x => x.UpdateCustomPageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("400 bad request"));
        var cut = OpenEditor("levels", """{"a":1}""");

        // Act
        cut.Find(".json-editor-footer .btn").Click();

        // Assert
        cut.Find(".json-editor-modal .admin-alert-error").TextContent.Should().Contain("Save failed: 400 bad request");
        cut.FindAll(".json-editor-modal").Should().ContainSingle();
    }

    [Fact]
    public void Cancel_Click_ClosesEditorWithoutSaving()
    {
        // Arrange
        var cut = OpenEditor("levels", """{"a":1}""");

        // Act
        cut.Find(".json-editor-footer .btn-secondary").Click();

        // Assert
        cut.FindAll(".json-editor-modal").Should().BeEmpty();
        _api.Verify(x => x.UpdateCustomPageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private IRenderedComponent<AdminCustomPages> OpenEditor(string key, string json)
    {
        _api.Setup(x => x.GetCustomPageEntryAsync(key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateEntry(key, "Levels of enlightenment", json));
        var cut = Render<AdminCustomPages>();
        cut.FindAll("tbody .admin-link-btn")[0].Click();
        return cut;
    }

    // Blazor renders a bound textarea value either as a value attribute or as text content depending on the renderer.
    private static string GetTextAreaValue(AngleSharp.Dom.IElement textarea) =>
        textarea.GetAttribute("value") ?? textarea.TextContent;

    private static CustomPageEntry CreateEntry(string key, string description, string json = "{}", DateTimeOffset? updatedAt = null) =>
        new()
        {
            Key = key,
            Description = description,
            JsonData = json,
            UpdatedAt = updatedAt ?? new DateTimeOffset(2026, 3, 4, 5, 6, 0, TimeSpan.Zero),
        };

    private void SetupEntries(IReadOnlyList<CustomPageEntry> entries) =>
        _api.Setup(x => x.GetCustomPageEntriesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(entries);
}
