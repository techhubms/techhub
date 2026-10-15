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
        cut.FindAll("button").Should().BeEmpty();
        _api.Verify(x => x.UnsubscribeNewsletterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Render_ValidLink_ShowsConfirmationWithoutCallingApi()
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/unsubscribe?email=user%40example.com&token=tok-1");

        // Act
        var cut = Render<NewsletterUnsubscribe>();

        // Assert
        cut.Find("button").TextContent.Should().Be("Unsubscribe");
        cut.FindAll(".subscribe-alert").Should().BeEmpty();
        _api.Verify(x => x.UnsubscribeNewsletterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Unsubscribe_ValidLink_UnsubscribesAndShowsSuccess()
    {
        // Arrange
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/unsubscribe?email=user%40example.com&token=tok-1");

        // Act
        var cut = Render<NewsletterUnsubscribe>();
        cut.Find("button").Click();

        // Assert
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-success");
        alert.TextContent.Should().Contain("You've been unsubscribed");
        cut.FindAll("button").Should().BeEmpty();
        _api.Verify(x => x.UnsubscribeNewsletterAsync("user@example.com", "tok-1", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Unsubscribe_ApiRejectsToken_ShowsExpiredLinkError()
    {
        // Arrange
        _api.Setup(x => x.UnsubscribeNewsletterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("bad token", null, System.Net.HttpStatusCode.BadRequest));
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/unsubscribe?email=user%40example.com&token=bad");

        // Act
        var cut = Render<NewsletterUnsubscribe>();
        cut.Find("button").Click();

        // Assert
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-error");
        alert.TextContent.Should().Contain("Invalid or expired unsubscribe link.");
    }

    [Fact]
    public void Render_QueryChanges_DoesNotCallApiUntilConfirmation()
    {
        // Arrange
        var navigation = Services.GetRequiredService<NavigationManager>();
        navigation.NavigateTo("/newsletter/unsubscribe?email=user%40example.com&token=tok-1");
        var cut = Render<NewsletterUnsubscribe>();

        // Act
        navigation.NavigateTo("/newsletter/unsubscribe?email=other%40example.com&token=tok-2");
        cut.Render();

        // Assert
        _api.Verify(x => x.UnsubscribeNewsletterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);

        // Act
        cut.Find("button").Click();

        // Assert
        _api.Verify(x => x.UnsubscribeNewsletterAsync("other@example.com", "tok-2", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Unsubscribe_RequestPending_HidesConfirmationAndShowsProcessing()
    {
        // Arrange
        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        _api.Setup(x => x.UnsubscribeNewsletterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(completion.Task);
        Services.GetRequiredService<NavigationManager>().NavigateTo("/newsletter/unsubscribe?email=user%40example.com&token=tok-1");
        var cut = Render<NewsletterUnsubscribe>();

        // Act
        var button = cut.Find("button");
        var click = button.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // Assert
        cut.WaitForAssertion(() => cut.Find(".subscribe-intro").TextContent.Should().Contain("Processing"));
        cut.FindAll("button").Should().BeEmpty();
        _api.Verify(x => x.UnsubscribeNewsletterAsync("user@example.com", "tok-1", It.IsAny<CancellationToken>()), Times.Once);

        // Act
        completion.SetResult();
        await click;

        // Assert
        cut.WaitForAssertion(() => cut.Find(".subscribe-alert-success").TextContent.Should().Contain("You've been unsubscribed"));
    }
}
