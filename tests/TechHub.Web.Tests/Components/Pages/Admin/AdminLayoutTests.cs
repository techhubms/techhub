using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Web.Components.Layout;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminLayout.razor — authorization gating, navigation and error boundary.
/// </summary>
public class AdminLayoutTests : BunitContext
{
    private static readonly string[] _expectedNavHrefs =
    [
        "/admin",
        "/admin/feeds",
        "/admin/processed-urls",
        "/admin/content-items",
        "/admin/newsletter",
        "/admin/reviews",
        "/admin/custom-pages",
        "/admin/copilot-features",
        "/admin/vscode-updates",
        "/admin/statistics",
        "/admin/settings",
    ];

    public AdminLayoutTests()
    {
        ComponentFactories.AddStub<AntiforgeryToken>();
        JSInterop.SetupVoid("markScriptsReady");
        Services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["AzureAd:ClientId"] = "test-client-id" })
            .Build());
    }

    [Fact]
    public void Render_Anonymous_ShowsSignInInsteadOfBodyAndNavigation()
    {
        // Arrange
        AddAuthorization().SetNotAuthorized();

        // Act
        var cut = RenderLayout();

        // Assert
        cut.Find(".admin-login-card").TextContent.Should().Contain("Sign in with Microsoft");
        cut.FindAll("#admin-body").Should().BeEmpty("protected content must not render for anonymous users");
        cut.FindAll("nav.admin-nav").Should().BeEmpty();
        cut.FindAll("form[action='/admin/logout']").Should().BeEmpty();
    }

    [Fact]
    public void Render_Authorized_ShowsNavigationAndBody()
    {
        // Arrange
        AddAuthorization().SetAuthorized("admin@example.com");

        // Act
        var cut = RenderLayout();

        // Assert
        cut.Find("nav.admin-nav").Should().NotBeNull();
        cut.Find("main.admin-main #admin-body").TextContent.Should().Be("protected content");
        cut.FindAll(".admin-login-card").Should().BeEmpty();
    }

    [Fact]
    public void Render_Authorized_ShowsAllAdminNavigationLinks()
    {
        // Arrange
        AddAuthorization().SetAuthorized("admin@example.com");

        // Act
        var cut = RenderLayout();

        // Assert
        var hrefs = cut.FindAll(".admin-nav-links a").Select(a => a.GetAttribute("href")).ToList();
        hrefs.Should().Equal(_expectedNavHrefs);
    }

    [Fact]
    public void Render_Authorized_LogoutIsAPostFormToLogoutEndpoint()
    {
        // Arrange
        AddAuthorization().SetAuthorized("admin@example.com");

        // Act
        var cut = RenderLayout();

        // Assert
        var form = cut.Find("form.admin-logout-form");
        form.GetAttribute("method").Should().Be("post");
        form.GetAttribute("action").Should().Be("/admin/logout");
        form.QuerySelector("button[type='submit']").Should().NotBeNull();
    }

    [Theory]
    [InlineData("/admin", "/admin")]
    [InlineData("/admin/", "/admin")]
    [InlineData("/admin/feeds", "/admin/feeds")]
    [InlineData("/admin/reviews?status=approved", "/admin/reviews")]
    [InlineData("/admin/content-items/anything", "/admin/content-items")]
    [InlineData("/ADMIN/Settings", "/admin/settings")]
    public void Render_Authorized_MarksOnlyCurrentSectionLinkAsActive(string currentPath, string expectedActiveHref)
    {
        // Arrange
        AddAuthorization().SetAuthorized("admin@example.com");
        Services.GetRequiredService<NavigationManager>().NavigateTo(currentPath);

        // Act
        var cut = RenderLayout();

        // Assert
        var active = cut.FindAll(".admin-nav-link.active").Select(a => a.GetAttribute("href")).ToList();
        active.Should().Equal(expectedActiveHref);
    }

    [Fact]
    public void Render_AuthorizedOnAdminSubPage_DashboardLinkIsNotActive()
    {
        // Arrange
        AddAuthorization().SetAuthorized("admin@example.com");
        Services.GetRequiredService<NavigationManager>().NavigateTo("/admin/feeds");

        // Act
        var cut = RenderLayout();

        // Assert
        cut.Find("a.admin-nav-link[href='/admin']").ClassList.Should().NotContain("active");
    }

    [Fact]
    public void Render_Authorized_BodyThrows_ShowsErrorBoundaryMessageAndKeepsNavigation()
    {
        // Arrange
        AddAuthorization().SetAuthorized("admin@example.com");
        RenderFragment body = builder =>
        {
            builder.OpenComponent<ThrowingComponent>(0);
            builder.CloseComponent();
        };

        // Act
        var cut = Render<AdminLayout>(parameters => parameters.Add(p => p.Body, body));

        // Assert
        cut.Find("main.admin-main .admin-alert-error[role='alert']").TextContent.Should().Contain("An unexpected error occurred.");
        cut.Find("nav.admin-nav").Should().NotBeNull();
    }

    private IRenderedComponent<AdminLayout> RenderLayout()
    {
        RenderFragment body = builder => builder.AddMarkupContent(0, "<p id=\"admin-body\">protected content</p>");
        return Render<AdminLayout>(parameters => parameters.Add(p => p.Body, body));
    }

    private sealed class ThrowingComponent : ComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder) =>
            throw new InvalidOperationException("boom");
    }
}
