using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages;

/// <summary>
/// bUnit tests for NewsletterUnsubscribe.razor — query-string validation and unsubscribe outcomes.
/// </summary>
public class NewsletterUnsubscribeTests : BunitContext
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public NewsletterUnsubscribeTests()
    {
        Services.AddSingleton(_api.Object);
        JSInterop.SetupVoid("markScriptsReady");
    }

    [Theory]
    [InlineData("")]
    [InlineData("?email=user%40example.com")]
    [InlineData("?token=abc")]
    [InlineData("?email=%20&token=%20")]
    public void Render_MissingOrBlankQueryParameters_ShowsInvalidLinkErrorWithoutCallingApi(string query)
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/unsubscribe" + query);

        // Act
        var cut = Render<NewsletterUnsubscribe>();

        // Assert
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-error");
        alert.TextContent.Should().Contain("Invalid unsubscribe link.");
        _api.Verify(x => x.UnsubscribeNewsletterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Render_ValidLink_UnsubscribesAndShowsSuccess()
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/unsubscribe?email=user%40example.com&token=tok-1");

        // Act
        var cut = Render<NewsletterUnsubscribe>();

        // Assert
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-success");
        alert.TextContent.Should().Contain("You've been unsubscribed");
        _api.Verify(x => x.UnsubscribeNewsletterAsync("user@example.com", "tok-1", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Render_ApiRejectsToken_ShowsExpiredLinkError()
    {
        // Arrange
        _api.Setup(x => x.UnsubscribeNewsletterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("bad token", null, System.Net.HttpStatusCode.BadRequest));
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/unsubscribe?email=user%40example.com&token=bad");

        // Act
        var cut = Render<NewsletterUnsubscribe>();

        // Assert
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-error");
        alert.TextContent.Should().Contain("Invalid or expired unsubscribe link.");
    }

    [Fact]
    public void Render_ValidLink_CallsApiExactlyOnceWhenQueryChangesLater()
    {
        // Arrange
        var navigation = Services.GetRequiredService<NavigationManager>();
        navigation.NavigateTo("/newsletter/unsubscribe?email=user%40example.com&token=tok-1");
        var cut = Render<NewsletterUnsubscribe>();

        // Act
        navigation.NavigateTo("/newsletter/unsubscribe?email=other%40example.com&token=tok-2");
        cut.Render();

        // Assert
        _api.Verify(x => x.UnsubscribeNewsletterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
