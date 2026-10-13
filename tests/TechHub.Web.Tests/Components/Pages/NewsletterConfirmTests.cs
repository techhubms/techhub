using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages;

/// <summary>
/// bUnit tests for NewsletterConfirm.razor — query-string validation and confirmation outcomes.
/// </summary>
public class NewsletterConfirmTests : BunitContext
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public NewsletterConfirmTests()
    {
        Services.AddSingleton(_api.Object);
        AddBunitPersistentComponentState();
        JSInterop.SetupVoid("markScriptsReady");
    }

    [Theory]
    [InlineData("")]
    [InlineData("?email=user%40example.com")]
    [InlineData("?token=abc")]
    [InlineData("?email=&token=")]
    [InlineData("?email=%20&token=%20")]
    public void Render_MissingOrBlankQueryParameters_ShowsInvalidLinkErrorWithoutCallingApi(string query)
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/confirm" + query);

        // Act
        var cut = Render<NewsletterConfirm>();

        // Assert
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-error");
        alert.TextContent.Should().Contain("Invalid confirmation link.");
        _api.Verify(x => x.ConfirmNewsletterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Render_ValidLink_ShowsServerMessageAsSuccess()
    {
        // Arrange
        _api.Setup(x => x.ConfirmNewsletterAsync("user@example.com", "tok-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync("Welcome aboard!");
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/confirm?email=user%40example.com&token=tok-1");

        // Act
        var cut = Render<NewsletterConfirm>();

        // Assert
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-success");
        alert.TextContent.Should().Contain("Welcome aboard!");
        _api.Verify(x => x.ConfirmNewsletterAsync("user@example.com", "tok-1", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Render_ApiRejectsToken_ShowsExpiredLinkError()
    {
        // Arrange
        _api.Setup(x => x.ConfirmNewsletterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("bad token", null, System.Net.HttpStatusCode.BadRequest));
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/confirm?email=user%40example.com&token=bad");

        // Act
        var cut = Render<NewsletterConfirm>();

        // Assert
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-error");
        alert.TextContent.Should().Contain("Invalid or expired confirmation link.");
    }

    [Fact]
    public void Render_AfterCompletion_HidesLoadingMessage()
    {
        // Arrange
        _api.Setup(x => x.ConfirmNewsletterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("done");
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/confirm?email=a%40b.c&token=t");

        // Act
        var cut = Render<NewsletterConfirm>();

        // Assert
        cut.FindAll(".subscribe-intro").Should().BeEmpty();
        cut.Find("h1").TextContent.Should().Be("Newsletter Subscription");
    }
}
