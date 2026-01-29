using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GenAI Basics custom page.
/// Verifies page-specific content including sections, FAQ blocks, and resource links.
/// 
/// Common component tests (TOC, mermaid) are in:
/// - SidebarTocTests.cs: Table of contents behavior
/// - MermaidTests.cs: Diagram rendering
/// </summary>
[Collection("Custom Pages TOC Tests")]
public class GenAIBasicsTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;

    public GenAIBasicsTests(PlaywrightCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
    }

    private const string PageUrl = "/ai/genai-basics";
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public async Task InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();
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
    public async Task GenAIBasics_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("GenAI Basics"));
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
    public async Task GenAIBasics_FaqBlocks_ShouldDisplay()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check for FAQ blocks (2 total: Models and Providers sections)
        var faqBlocks = Page.Locator(".genai-faq");

        // Wait for FAQ blocks to be visible (use Playwright's built-in retry)
        await Assertions.Expect(faqBlocks.First).ToBeVisibleAsync();

        var faqBlockCount = await faqBlocks.CountAsync();
        faqBlockCount.Should().Be(2, "Expected 2 FAQ blocks (Models and Providers sections)");

        // Check FAQ items (8 total: 3 in Models, 5 in Providers)
        var faqItems = Page.Locator(".genai-faq-item");
        var faqItemCount = await faqItems.CountAsync();

        faqItemCount.Should().Be(8, "Expected 8 FAQ items total");

        // Verify FAQ structure: each item should have a question div (not summary - no disclosure widget)
        var firstFaq = faqItems.First;
        var question = firstFaq.Locator(".genai-faq-question");
        await Assertions.Expect(question).ToBeVisibleAsync();

        var questionText = await question.TextContentAsync();
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
}
