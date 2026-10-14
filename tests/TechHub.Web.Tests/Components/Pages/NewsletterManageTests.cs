using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages;

/// <summary>
/// bUnit tests for NewsletterManage.razor — request-link form, token validation and preference saving.
/// </summary>
public class NewsletterManageTests : BunitContext
{
    private const string ManageUrl = "/newsletter/manage?email=user%40example.com&token=tok-1";

    private readonly Mock<ITechHubApiClient> _api = new();

    public NewsletterManageTests()
    {
        _api.Setup(x => x.GetNewsletterSectionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(NewsletterSubscribeTests.CreateSections("ai", "azure"));

        Services.AddSingleton(_api.Object);
        Services.AddSingleton(A.SectionCache.WithSections("ai", "all").Build());
        JSInterop.SetupVoid("markScriptsReady");
    }

    // ── Request-link form (no token) ────────────────────────────────────────

    [Fact]
    public void Render_NoQueryParameters_ShowsRequestLinkFormWithDisabledButton()
    {
        // Act
        var cut = Render<NewsletterManage>();

        // Assert
        cut.Find("#manage-email").Should().NotBeNull();
        cut.Find("button.btn").HasAttribute("disabled").Should().BeTrue("an email is required first");
        _api.Verify(x => x.GetNewsletterManagePreferencesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void RequestLink_ValidEmail_CallsApiWithTrimmedEmailAndShowsConfirmation()
    {
        // Arrange
        var cut = Render<NewsletterManage>();
        cut.Find("#manage-email").Change("  user@example.com  ");

        // Act
        cut.Find("button.btn").Click();

        // Assert
        _api.Verify(x => x.RequestNewsletterManageLinkAsync("user@example.com", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".subscribe-alert-success").TextContent.Should().Contain("Check your inbox");
        cut.FindAll("#manage-email").Should().BeEmpty("the form is replaced by the confirmation");
    }

    [Fact]
    public void RequestLink_ApiFails_ShowsErrorAndKeepsForm()
    {
        // Arrange
        _api.Setup(x => x.RequestNewsletterManageLinkAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("down"));
        var cut = Render<NewsletterManage>();
        cut.Find("#manage-email").Change("user@example.com");

        // Act
        cut.Find("button.btn").Click();

        // Assert
        cut.Find(".subscribe-alert-error").TextContent.Should().Contain("Failed to send the management link");
        cut.FindAll("#manage-email").Should().ContainSingle();
    }

    // ── Token handling ──────────────────────────────────────────────────────

    [Fact]
    public void Render_ValidToken_LoadsPreferencesAndPreselectsSections()
    {
        // Arrange
        SetupPreferences(CreateSubscriber(displayName: "Jane", weekly: ["azure"], daily: ["ai"]));
        Services.GetRequiredService<NavigationManager>().NavigateTo(ManageUrl);

        // Act
        var cut = Render<NewsletterManage>();

        // Assert
        _api.Verify(x => x.GetNewsletterManagePreferencesAsync("user@example.com", "tok-1", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find("#manage-name").GetAttribute("value").Should().Be("Jane");

        var groups = cut.FindAll(".subscribe-group");
        var weekly = groups[0].QuerySelectorAll("input[type='checkbox']");
        var daily = groups[1].QuerySelectorAll("input[type='checkbox']");
        weekly[0].HasAttribute("checked").Should().BeFalse();
        weekly[1].HasAttribute("checked").Should().BeTrue();
        daily[0].HasAttribute("checked").Should().BeTrue();
        daily[1].HasAttribute("checked").Should().BeFalse();
    }

    [Fact]
    public void Render_ValidToken_UnsubscribeLinkCarriesEscapedEmailAndToken()
    {
        // Arrange
        SetupPreferences(CreateSubscriber());
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/manage?email=a%2Bb%40example.com&token=t%2Fk%3D");

        // Act
        var cut = Render<NewsletterManage>();

        // Assert
        cut.Find("a.btn-secondary").GetAttribute("href")
            .Should().Be("/newsletter/unsubscribe?email=a%2Bb%40example.com&token=t%2Fk%3D");
    }

    [Fact]
    public void Render_UnknownToken_DoesNotShowPreferencesAndOffersRequestLinkForm()
    {
        // Arrange
        _api.Setup(x => x.GetNewsletterManagePreferencesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((NewsletterSubscriber?)null);
        Services.GetRequiredService<NavigationManager>().NavigateTo(ManageUrl);

        // Act
        var cut = Render<NewsletterManage>();

        // Assert
        cut.FindAll("#manage-name").Should().BeEmpty("preferences must not be exposed for an unknown token");
        cut.FindAll(".subscribe-group").Should().BeEmpty();
        cut.FindAll("#manage-email").Should().ContainSingle("the user can request a fresh link");
    }

    [Fact]
    public void Render_PreferencesLookupThrows_DoesNotShowPreferencesAndOffersRequestLinkForm()
    {
        // Arrange
        _api.Setup(x => x.GetNewsletterManagePreferencesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("down"));
        Services.GetRequiredService<NavigationManager>().NavigateTo(ManageUrl);

        // Act
        var cut = Render<NewsletterManage>();

        // Assert
        cut.FindAll("#manage-name").Should().BeEmpty();
        cut.FindAll("#manage-email").Should().ContainSingle();
    }

    [Fact]
    public void Render_OnlyEmailInQuery_ShowsRequestLinkFormInsteadOfPreferences()
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/manage?email=user%40example.com");

        // Act
        var cut = Render<NewsletterManage>();

        // Assert
        cut.FindAll("#manage-email").Should().ContainSingle();
        _api.Verify(x => x.GetNewsletterManagePreferencesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // ── Saving ──────────────────────────────────────────────────────────────

    [Fact]
    public void Save_NoSectionsSelected_ShowsValidationErrorWithoutCallingApi()
    {
        // Arrange
        SetupPreferences(CreateSubscriber(weekly: [], daily: []));
        Services.GetRequiredService<NavigationManager>().NavigateTo(ManageUrl);
        var cut = Render<NewsletterManage>();

        // Act
        cut.FindAll("button.btn").Single(b => b.TextContent.Contains("Save")).Click();

        // Assert
        cut.Find(".subscribe-alert-error").TextContent.Should().Contain("Select at least one section");
        _api.Verify(
            x => x.UpdateNewsletterManagePreferencesAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(),
                It.IsAny<IReadOnlyList<string>>(), It.IsAny<IReadOnlyList<string>>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void Save_WithSelections_SendsPreferencesAndShowsSuccess()
    {
        // Arrange
        SetupPreferences(CreateSubscriber(displayName: "Jane", weekly: ["ai"], daily: []));
        Services.GetRequiredService<NavigationManager>().NavigateTo(ManageUrl);
        var cut = Render<NewsletterManage>();
        cut.FindAll(".subscribe-group")[1].QuerySelectorAll("input[type='checkbox']")[1].Change(true);

        // Act
        cut.FindAll("button.btn").Single(b => b.TextContent.Contains("Save")).Click();

        // Assert
        _api.Verify(
            x => x.UpdateNewsletterManagePreferencesAsync(
                "user@example.com",
                "tok-1",
                "Jane",
                It.Is<IReadOnlyList<string>>(l => l.SequenceEqual(new[] { "ai" })),
                It.Is<IReadOnlyList<string>>(l => l.SequenceEqual(new[] { "azure" })),
                It.IsAny<CancellationToken>()),
            Times.Once);
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-success");
        alert.TextContent.Should().Contain("Preferences saved.");
    }

    [Fact]
    public void Save_ApiFails_ShowsErrorAlert()
    {
        // Arrange
        SetupPreferences(CreateSubscriber(weekly: ["ai"]));
        _api.Setup(x => x.UpdateNewsletterManagePreferencesAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(),
                It.IsAny<IReadOnlyList<string>>(), It.IsAny<IReadOnlyList<string>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("down"));
        Services.GetRequiredService<NavigationManager>().NavigateTo(ManageUrl);
        var cut = Render<NewsletterManage>();

        // Act
        cut.FindAll("button.btn").Single(b => b.TextContent.Contains("Save")).Click();

        // Assert
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-error");
        alert.TextContent.Should().Contain("Failed to save preferences");
    }

    private void SetupPreferences(NewsletterSubscriber subscriber)
    {
        _api.Setup(x => x.GetNewsletterManagePreferencesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subscriber);
    }

    private static NewsletterSubscriber CreateSubscriber(
        string? displayName = null,
        IReadOnlyList<string>? weekly = null,
        IReadOnlyList<string>? daily = null) =>
        new()
        {
            Id = 1,
            Email = "user@example.com",
            DisplayName = displayName,
            IsConfirmed = true,
            WeeklySections = weekly ?? [],
            DailySections = daily ?? [],
        };
}
