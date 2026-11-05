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
    // The first page of every group; the sub menu bar lists the other pages of the active group.
    private static readonly string[] _expectedNavHrefs =
    [
        "/admin",
        "/admin/content-items",
        "/admin/reviews",
        "/admin/newsletter",
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
    [InlineData("/admin/feeds", "/admin")]
    [InlineData("/admin/processed-urls", "/admin")]
    [InlineData("/admin/reviews?status=approved", "/admin/reviews")]
    [InlineData("/admin/content-items/anything", "/admin/content-items")]
    [InlineData("/admin/content-preview/news/some-item", "/admin/content-items")]
    [InlineData("/admin/vscode-updates", "/admin/content-items")]
    [InlineData("/admin/custom-pages", "/admin/content-items")]
    [InlineData("/admin/products", "/admin/content-items")]
    [InlineData("/admin/products/12/features", "/admin/content-items")]
    [InlineData("/admin/newsletter/send-log", "/admin/newsletter")]
    [InlineData("/ADMIN/Settings", "/admin/settings")]
    public void Render_Authorized_MarksOnlyTheGroupOfTheCurrentPageAsActive(string currentPath, string expectedActiveHref)
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

    [Theory]
    [InlineData("/admin", new[] { "/admin", "/admin/feeds", "/admin/processed-urls" }, "/admin")]
    [InlineData("/admin/feeds", new[] { "/admin", "/admin/feeds", "/admin/processed-urls" }, "/admin/feeds")]
    [InlineData("/admin/content-preview/news/some-item", new[] { "/admin/content-items", "/admin/products", "/admin/vscode-updates", "/admin/custom-pages" }, "/admin/content-items")]
    [InlineData("/admin/products", new[] { "/admin/content-items", "/admin/products", "/admin/vscode-updates", "/admin/custom-pages" }, "/admin/products")]
    [InlineData("/admin/products/12/features", new[] { "/admin/content-items", "/admin/products", "/admin/vscode-updates", "/admin/custom-pages" }, "/admin/products")]
    [InlineData("/admin/newsletter", new[] { "/admin/newsletter", "/admin/newsletter/send-log" }, "/admin/newsletter")]
    [InlineData("/admin/newsletter/send-log", new[] { "/admin/newsletter", "/admin/newsletter/send-log" }, "/admin/newsletter/send-log")]
    [InlineData("/admin/settings", new[] { "/admin/settings", "/admin/feature-toggles" }, "/admin/settings")]
    [InlineData("/admin/feature-toggles", new[] { "/admin/settings", "/admin/feature-toggles" }, "/admin/feature-toggles")]
    public void Render_Authorized_ShowsTheSubMenuOfTheActiveGroupWithTheCurrentPageMarked(string currentPath, string[] expectedHrefs, string expectedActiveHref)
    {
        // Arrange
        AddAuthorization().SetAuthorized("admin@example.com");
        Services.GetRequiredService<NavigationManager>().NavigateTo(currentPath);

        // Act
        var cut = RenderLayout();

        // Assert
        cut.FindAll(".admin-subnav a").Select(a => a.GetAttribute("href")).Should().Equal(expectedHrefs);
        var active = cut.FindAll(".admin-subnav a.active");
        active.Select(a => a.GetAttribute("href")).Should().Equal(expectedActiveHref);
        active.Single().GetAttribute("aria-current").Should().Be("page");
    }

    [Theory]
    [InlineData("/admin/reviews", "/admin/reviews")]
    [InlineData("/admin/statistics", "/admin/statistics")]
    public void Render_Authorized_OnAPageWithoutSiblings_ShowsTheSubMenuWithThatPage(string currentPath, string expectedHref)
    {
        // Arrange
        AddAuthorization().SetAuthorized("admin@example.com");
        Services.GetRequiredService<NavigationManager>().NavigateTo(currentPath);

        // Act
        var cut = RenderLayout();

        // Assert
        cut.FindAll(".admin-subnav .admin-subnav-links a").Select(a => a.GetAttribute("href")).Should().Equal(expectedHref);
        cut.Find(".admin-subnav a.active").GetAttribute("href").Should().Be(expectedHref);
    }

    [Fact]
    public void Render_Authorized_PageActionsAppearInTheSubMenuInsteadOfThePage()
    {
        // Arrange
        AddAuthorization().SetAuthorized("admin@example.com");
        Services.GetRequiredService<NavigationManager>().NavigateTo("/admin/feeds");
        RenderFragment body = builder =>
        {
            builder.OpenComponent<AdminSubbarAction>(0);
            builder.AddAttribute(1, nameof(AdminSubbarAction.ChildContent), (RenderFragment)(inner => inner.AddMarkupContent(0, "<button id=\"page-refresh\">Refresh</button>")));
            builder.CloseComponent();
        };

        // Act
        var cut = Render<AdminLayout>(parameters => parameters.Add(p => p.Body, body));

        // Assert
        cut.Find(".admin-subnav .admin-subnav-actions #page-refresh").TextContent.Should().Be("Refresh");
        cut.FindAll("main.admin-main #page-refresh").Should().BeEmpty("the page's own action moved to the sub menu");
    }

    [Fact]
    public void SubbarAction_WithoutTheAdminLayout_RendersInline()
    {
        // Act
        var cut = Render<AdminSubbarAction>(parameters => parameters.AddChildContent("<button id=\"page-refresh\">Refresh</button>"));

        // Assert
        cut.Find("#page-refresh").TextContent.Should().Be("Refresh");
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
        cut.Find(".admin-subnav a[href='/admin']").ClassList.Should().NotContain("active");
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
