using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for SidebarRssLinks.razor component.
///
/// Covers:
/// - RSS links render as anchor tags with correct href
/// - Newsletter entry (Url = null) renders as a button instead of an anchor
/// - Newsletter button invokes OnNewsletterClick callback
/// - Links with explicit Url render as external anchors
/// </summary>
public class SidebarRssLinksTests : BunitContext
{
    [Fact]
    public void SidebarRssLinks_RssLink_RendersAsAnchor()
    {
        var links = new[] { new SidebarRssLinks.RssLink("RSS Feed", "/ai/feed.xml") };

        var cut = Render<SidebarRssLinks>(p => p.Add(c => c.Links, links));

        var anchor = cut.Find("a");
        anchor.Should().NotBeNull();
        anchor.GetAttribute("href").Should().Be("/ai/feed.xml");
        anchor.TextContent.Should().Contain("RSS Feed");
    }

    [Fact]
    public void SidebarRssLinks_NewsletterLink_RendersAsButton()
    {
        var links = new[] { new SidebarRssLinks.RssLink("Newsletter", null) };

        var cut = Render<SidebarRssLinks>(p =>
        {
            p.Add(c => c.Links, links);
            p.Add(c => c.OnNewsletterClick, EventCallback.Factory.Create(this, () => { }));
        });

        // Should render a button, not an anchor
        cut.FindAll("button").Should().HaveCount(1, "newsletter entry without Url must render as a button");
        cut.FindAll("a").Should().BeEmpty("newsletter entry must not render as an anchor");

        cut.Find("button").TextContent.Should().Contain("Newsletter");
    }

    [Fact]
    public void SidebarRssLinks_NewsletterButton_InvokesCallback()
    {
        var clicked = false;
        var links = new[] { new SidebarRssLinks.RssLink("Newsletter", null) };

        var cut = Render<SidebarRssLinks>(p =>
        {
            p.Add(c => c.Links, links);
            p.Add(c => c.OnNewsletterClick, EventCallback.Factory.Create(this, () => clicked = true));
        });

        cut.Find("button").Click();

        clicked.Should().BeTrue("clicking the newsletter button must invoke OnNewsletterClick");
    }

    [Fact]
    public void SidebarRssLinks_MultipleLinks_RendersAll()
    {
        var links = new[]
        {
            new SidebarRssLinks.RssLink("RSS Feed", "/all/feed.xml"),
            new SidebarRssLinks.RssLink("Newsletter", null)
        };

        var cut = Render<SidebarRssLinks>(p =>
        {
            p.Add(c => c.Links, links);
            p.Add(c => c.OnNewsletterClick, EventCallback.Factory.Create(this, () => { }));
        });

        cut.FindAll("li").Should().HaveCount(2, "one list item per link");
        cut.FindAll("a").Should().HaveCount(1, "only the RSS link should be an anchor");
        cut.FindAll("button").Should().HaveCount(1, "only the newsletter link should be a button");
    }

    [Fact]
    public void SidebarRssLinks_RssLink_HasExternalAttributes()
    {
        var links = new[] { new SidebarRssLinks.RssLink("RSS Feed", "/ai/feed.xml") };

        var cut = Render<SidebarRssLinks>(p => p.Add(c => c.Links, links));

        var anchor = cut.Find("a");
        anchor.GetAttribute("target").Should().Be("_blank");
        anchor.GetAttribute("rel").Should().Contain("noopener");
    }
}
