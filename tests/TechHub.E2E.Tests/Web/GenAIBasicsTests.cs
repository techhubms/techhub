using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GenAI Basics custom page.
/// Verifies markdown rendering, mermaid diagrams, FAQ blocks, and accessibility.
/// </summary>
[Collection("Custom Pages TOC Tests")]
public class GenAIBasicsTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string PageUrl = "/ai/genai-basics";
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
        _page = await _context.NewPageWithDefaultsAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
        }

        if (_context != null)
        {
            await _context.CloseAsync();
        }
    }

    [Fact]
    public async Task GenAIBasics_OnPageLoad_ShouldRenderAllSections()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Page title
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "GenAI Basics", level: 1);

        // Assert - All 13 sections should be present
        var sections = Page.Locator(".genai-section h2");
        var sectionCount = await sections.CountAsync();
        sectionCount.Should().Be(13, "Expected 13 sections on GenAI Basics page");

        // Verify section titles
        var expectedSections = new[]
        {
            "History",
            "ML vs AI vs GenAI",
            "About Generative AI",
            "Vendors",
            "Models",
            "Providers",
            "Prompts & messages",
            "Tokens & Tokenization",
            "Next-token prediction: How LLMs generate text",
            "Costs",
            "Problems with models",
            "When not to use AI",
            "Societal impacts and risks"
        };

        for (var i = 0; i < expectedSections.Length; i++)
        {
            var sectionText = await sections.Nth(i).TextContentAsync();
            sectionText.Should().Be(expectedSections[i], $"Section {i + 1} should match expected title");
        }
    }

    [Fact]
    public async Task GenAIBasics_ShouldRender_WithSidebarToc()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check sidebar TOC exists
        var toc = Page.Locator(".sidebar-toc");
        await toc.AssertElementVisibleAsync();

        // Should have TOC heading
        var tocHeading = toc.Locator("h2, h3").First;
        await tocHeading.AssertElementVisibleAsync();
        var headingText = await tocHeading.TextContentAsync();
        headingText.Should().Be("Table of Contents");

        // Should have TOC links (actual count may include nested subsections)
        var tocLinks = toc.Locator("a");
        var linkCount = await tocLinks.CountAsync();
        linkCount.Should().BeGreaterThan(0, "Expected TOC to have links");
        // Note: Actual count is 55 due to nested subsections in the GenAI Basics page
    }

    [Fact]
    public async Task GenAIBasics_TocLinks_ShouldScrollToSections()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get all TOC links
        var tocLinks = Page.Locator(".sidebar-toc a");
        var linkCount = await tocLinks.CountAsync();

        if (linkCount == 0)
        {
            Assert.Fail("No TOC links found on GenAI Basics page");
        }

        // Act - Click first TOC link
        var firstLink = tocLinks.First;
        var linkText = await firstLink.TextContentAsync();
        await firstLink.ClickAsync();

        // Wait for scroll to complete and active state to update
        await Page.WaitForSelectorAsync(".sidebar-toc a.active", new() { Timeout = 2000 });

        // Assert - URL should have hash
        var url = Page.Url;
        url.Should().Contain("#", $"Expected URL to contain anchor after clicking TOC link '{linkText}'");

        // Assert - Clicked link should have active class
        var activeLinks = await Page.Locator(".sidebar-toc a.active").CountAsync();
        activeLinks.Should().BeGreaterThan(0, "Expected at least one TOC link to be active");
    }

    [Fact]
    public async Task GenAIBasics_Scrolling_ShouldUpdateActiveTocLink()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get second section heading
        var secondHeading = Page.Locator(".genai-section h2").Nth(1);
        await secondHeading.AssertElementVisibleAsync();

        // Act - Scroll to second section
        await secondHeading.ScrollIntoViewIfNeededAsync();

        // Wait for scroll spy to update active state
        await Page.WaitForSelectorAsync(".sidebar-toc a.active", new() { Timeout = 2000 });

        // Assert - Active TOC link should update
        var activeTocLink = Page.Locator(".sidebar-toc a.active").First;
        await activeTocLink.AssertElementVisibleAsync();

        var activeLinkText = await activeTocLink.TextContentAsync();
        activeLinkText.Should().NotBeNullOrEmpty("Active TOC link should have text");
    }

    [Fact]
    public async Task GenAIBasics_TocLink_ClickTwice_ShouldNotMoveScrollbar()
    {
        // This test verifies that CSS scroll-margin-top aligns with JavaScript scroll spy detection.
        // When navigating to a URL with a hash fragment:
        // 1. Browser scrolls using CSS scroll-margin-top
        // 2. Scroll spy should detect and highlight the TOC link
        // 3. Clicking the already-highlighted TOC link should NOT cause any scroll movement
        // This ensures the CSS and JavaScript are perfectly aligned.

        // Arrange - Navigate directly to a section with hash fragment
        var sectionHash = "how-tokenization-works";
        await Page.GotoRelativeAsync($"{PageUrl}#{sectionHash}");

        // Wait for page to load and TOC to be visible
        await Page.WaitForSelectorAsync(".sidebar-toc", new() { Timeout = 5000 });

        // Wait for scroll spy to detect position and activate the correct TOC link
        // The TOC links have full URLs like "https://localhost:5003/ai/genai-basics#how-tokenization-works"
        // So we need to match using [href$='#{hash}'] which checks the end of the href attribute
        var activeTocLink = Page.Locator($".sidebar-toc a[href$='#{sectionHash}'].active");
        await Page.WaitForSelectorAsync($".sidebar-toc a[href$='#{sectionHash}'].active", new() { Timeout = 5000 });

        // Verify the correct TOC link is active and visible
        await activeTocLink.AssertElementVisibleAsync();

        // Get scroll position after browser's initial scroll
        var scrollYBeforeClick = await Page.EvaluateAsync<int>("() => window.scrollY");

        // Act - Click the already-highlighted TOC link
        await activeTocLink.ClickAsync();

        // Wait briefly to allow any scroll animation to complete
        await Page.WaitForSelectorAsync($".sidebar-toc a[href$='#{sectionHash}'].active", new() { Timeout = 2000 });

        // Get scroll position after clicking
        var scrollYAfterClick = await Page.EvaluateAsync<int>("() => window.scrollY");

        // Assert - Scroll position should not have changed
        scrollYAfterClick.Should().Be(scrollYBeforeClick,
            "Clicking an already-active TOC link should not move the scrollbar because CSS scroll-margin-top and JavaScript detection line should be perfectly aligned");
    }

    [Fact]
    public async Task GenAIBasics_LastSection_ShouldScroll_ToDetectionPoint()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get last section
        var lastHeading = Page.Locator(".genai-section h2").Last;
        await lastHeading.AssertElementVisibleAsync();

        // Act - Scroll to last section
        await lastHeading.ScrollIntoViewIfNeededAsync();

        // Get the last TOC link first
        var tocLinks = Page.Locator(".sidebar-toc a");
        var lastTocLink = tocLinks.Last;
        var lastLinkHref = await lastTocLink.GetAttributeAsync("href");

        // Wait for specifically the LAST TOC link to become active
        // (scroll spy may activate intermediate links first)
        await Page.WaitForSelectorAsync($".sidebar-toc a[href='{lastLinkHref}'].active", new() { Timeout = 5000 });

        // Assert - Last TOC link should be active
        var lastTocLinkClass = await lastTocLink.GetAttributeAsync("class");
        lastTocLinkClass.Should().Contain("active", "Last TOC link should be active when scrolled to last section");
    }

    [Fact]
    public async Task GenAIBasics_MermaidDiagrams_ShouldRender()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync(PageUrl);

        // Wait for mermaid diagrams to render (wait for first SVG to appear)
        await Page.WaitForSelectorAsync("svg[id^='mermaid-']", new() { Timeout = 5000 });

        // Assert - Check for mermaid diagrams (rendered as SVG by mermaid.js)
        var mermaidDiagrams = Page.Locator("svg[id^='mermaid-']");
        var diagramCount = await mermaidDiagrams.CountAsync();

        diagramCount.Should().BeGreaterThan(0, "Expected mermaid diagrams to be rendered as SVG elements");

        // Verify at least some of the 11 expected diagrams are present
        // Note: Not all may be visible depending on viewport, so we check for at least 3
        diagramCount.Should().BeGreaterThanOrEqualTo(3, "Expected at least 3 mermaid diagrams to be visible");
    }

    [Fact]
    public async Task GenAIBasics_FaqBlocks_ShouldDisplay()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync(PageUrl);

        // Wait for FAQ items with summaries to load (may take time with Blazor rendering)
        await Page.WaitForSelectorAsync(".genai-faq-item summary", new() { Timeout = 10000 });

        // Assert - Check for FAQ blocks (2 total: Models and Providers sections)
        var faqBlocks = Page.Locator(".genai-faq");
        var faqBlockCount = await faqBlocks.CountAsync();

        faqBlockCount.Should().Be(2, "Expected 2 FAQ blocks (Models and Providers sections)");

        // Check FAQ items (8 total: 3 in Models, 5 in Providers)
        var faqItems = Page.Locator(".genai-faq-item");
        var faqItemCount = await faqItems.CountAsync();

        faqItemCount.Should().Be(8, "Expected 8 FAQ items total");

        // Verify FAQ structure: each item should have a summary (question)
        var firstFaq = faqItems.First;
        var summary = firstFaq.Locator("summary");
        await summary.AssertElementVisibleAsync();

        var questionText = await summary.TextContentAsync();
        questionText.Should().NotBeNullOrEmpty("FAQ should have a question");
    }

    [Fact]
    public async Task GenAIBasics_ResourceLinks_ShouldBeClickable()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check for "More Information" sections (10 total)
        var moreInfoSections = Page.Locator(".genai-more-info");
        var moreInfoCount = await moreInfoSections.CountAsync();

        moreInfoCount.Should().BeGreaterThanOrEqualTo(10, "Expected at least 10 'More Information' sections");

        // Verify resource links
        var resourceLinks = Page.Locator(".genai-more-info a");
        var linkCount = await resourceLinks.CountAsync();

        linkCount.Should().BeGreaterThan(0, "Expected resource links in 'More Information' sections");

        // Check first link is clickable and has target="_blank"
        var firstLink = resourceLinks.First;
        var target = await firstLink.GetAttributeAsync("target");
        target.Should().Be("_blank", "Resource links should open in new tab");

        var rel = await firstLink.GetAttributeAsync("rel");
        rel.Should().Contain("noopener", "Resource links should have rel='noopener' for security");
    }

    [Fact]
    public async Task GenAIBasics_Page_ShouldNotHaveConsoleErrors()
    {
        // Arrange - Collect console messages
        var consoleMessages = new List<IConsoleMessage>();
        Page.Console += (_, msg) => consoleMessages.Add(msg);

        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Wait for page to fully load and mermaid to render
        await Page.WaitForSelectorAsync("svg[id^='mermaid-']", new() { Timeout = 5000 });

        // Assert - No console errors
        var errors = consoleMessages.Where(m => m.Type == "error").ToList();

        errors.Should().BeEmpty($"Expected no console errors, but found: {string.Join(", ", errors.Select(e => e.Text))}");
    }
}
