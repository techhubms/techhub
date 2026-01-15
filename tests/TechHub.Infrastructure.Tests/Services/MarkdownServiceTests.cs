using FluentAssertions;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Tests for markdown rendering and processing
/// Validates: HTML conversion, excerpt extraction, YouTube embeds
/// 
/// SINGLETON SERVICE: MarkdownService is registered as Singleton in Program.cs
/// Tests use shared instance to mirror production behavior - if someone adds
/// mutable state, parallel execution tests will fail (catching production bugs)
/// </summary>
public class MarkdownServiceTests
{
    // INTENTIONAL: Shared instance mirrors Singleton registration in production
    // This catches bugs if someone adds mutable state to the service
    // See: src/TechHub.Api/Program.cs line 50
    private readonly MarkdownService _service;

    public MarkdownServiceTests()
    {
        _service = new MarkdownService();
    }

    #region ToHtml Tests

    /// <summary>
    /// Test: Basic markdown should convert to HTML tags
    /// Why: Core functionality - markdown to HTML transformation
    /// </summary>
    [Fact]
    public void ToHtml_BasicMarkdown_ConvertsToHtml()
    {
        // Arrange: Simple markdown with headers, paragraphs, lists
        var markdown = """
            # Heading 1
            
            This is a paragraph.
            
            - List item 1
            - List item 2
            """;

        // Act: Convert to HTML
        var html = _service.RenderToHtml(markdown);

        // Assert: Markdown converted to proper HTML elements
        html.Should().Contain("<h1"); // Markdig adds id attributes
        html.Should().Contain(">Heading 1</h1>");
        html.Should().Contain("<p>This is a paragraph.</p>");
        html.Should().Contain("<ul>");
        html.Should().Contain("<li>List item 1</li>");
    }

    /// <summary>
    /// Test: GitHub Flavored Markdown features should work
    /// Why: We use GFM extensions (tables, task lists, strikethrough)
    /// </summary>
    [Fact]
    public void ToHtml_GitHubFlavoredMarkdown_SupportsGFMFeatures()
    {
        // Arrange: GFM-specific syntax (table and strikethrough)
        var markdown = """
            | Header 1 | Header 2 |
            |----------|----------|
            | Cell 1   | Cell 2   |
            
            ~~strikethrough~~
            """;

        // Act: Render GFM features
        var html = _service.RenderToHtml(markdown);

        // Assert: GFM features properly rendered
        // Note: Bootstrap extension adds class="table" to tables
        (html.Contains("<table>") || html.Contains("<table class=\"table\">")).Should().BeTrue(
            "Expected table element with or without Bootstrap class");
        html.Should().Contain("<th>Header 1</th>");
        html.Should().Contain("<td>Cell 1</td>");
        html.Should().Contain("<del>strikethrough</del>");
    }

    /// <summary>
    /// Test: Empty or null input should return empty string
    /// Why: Defensive programming - handle edge cases gracefully
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ToHtml_EmptyInput_ReturnsEmptyString(string? input)
    {
        // Act: Process empty/null markdown
        var html = _service.RenderToHtml(input!);

        // Assert: Returns empty string (no exceptions)
        html.Should().BeEmpty();
    }

    /// <summary>
    /// Test: Auto-linking URLs should work
    /// Why: Convenience feature - bare URLs become clickable links
    /// </summary>
    [Fact]
    public void ToHtml_AutoLinks_ConvertsUrlsToLinks()
    {
        // Arrange: Bare URL in markdown
        var markdown = "Check out https://github.com/features/copilot";

        // Act: Convert with auto-linking enabled
        var html = _service.RenderToHtml(markdown);

        // Assert: URL wrapped in <a> tag
        html.Should().Contain("<a href=\"https://github.com/features/copilot\">");
    }

    #endregion

    #region ExtractExcerpt Tests

    /// <summary>
    /// Test: Excerpt marker should extract content before marker
    /// Why: Standard excerpt pattern in Tech Hub content files
    /// </summary>
    [Fact]
    public void ExtractExcerpt_WithMarker_ReturnsContentBeforeMarker()
    {
        // Arrange: Markdown with excerpt_end marker
        var markdown = """
            This is the excerpt.
            
            It can span multiple paragraphs.
            
            <!--excerpt_end-->
            
            This is the full article content.
            With more details here.
            """;

        // Act: Extract excerpt
        var excerpt = _service.ExtractExcerpt(markdown);

        // Assert: Only content before marker returned (plain text)
        excerpt.Should().Contain("This is the excerpt");
        excerpt.Should().Contain("multiple paragraphs");
        excerpt.Should().NotContain("full article content");
        excerpt.Should().NotContain("<!--excerpt_end-->");
    }

    /// <summary>
    /// Test: Without marker, should return first paragraph
    /// Why: Fallback behavior when no explicit excerpt defined
    /// </summary>
    [Fact]
    public void ExtractExcerpt_WithoutMarker_ReturnsFirstParagraph()
    {
        // Arrange: Markdown without excerpt marker
        var markdown = """
            First paragraph here.
            
            Second paragraph here.
            
            Third paragraph here.
            """;

        // Act: Extract excerpt (fallback mode)
        var excerpt = _service.ExtractExcerpt(markdown);

        // Assert: First paragraph returned, others excluded
        excerpt.Should().Contain("First paragraph");
        excerpt.Should().NotContain("Second paragraph");
        excerpt.Should().NotContain("Third paragraph");
    }

    /// <summary>
    /// Test: Long excerpt should be truncated to maxLength (default 1000)
    /// Why: Prevents excessively long excerpts in listings
    /// </summary>
    [Fact]
    public void ExtractExcerpt_LongContent_TruncatesToMaxLength()
    {
        // Arrange: Very long first paragraph (1500+ characters)
        var markdown = new string('A', 1500);

        // Act: Extract excerpt with default maxLength (1000)
        var excerpt = _service.ExtractExcerpt(markdown);

        // Assert: Truncated to ~1000 chars with ellipsis
        excerpt.Length.Should().BeLessThanOrEqualTo(1003); // 1000 + "..."
        excerpt.Should().EndWith("...");
    }

    /// <summary>
    /// Test: Markdown formatting should be stripped from excerpt
    /// Why: Excerpts are plain text (no HTML/markdown in summaries)
    /// </summary>
    [Fact]
    public void ExtractExcerpt_FormattedMarkdown_StripsFormatting()
    {
        // Arrange: Markdown with various formatting
        var markdown = """
            # Heading
            
            This has **bold** and *italic* and `code` and [a link](https://example.com).
            
            <!--excerpt_end-->
            """;

        // Act: Extract excerpt
        var excerpt = _service.ExtractExcerpt(markdown);

        // Assert: All formatting removed (plain text only)
        excerpt.Should().NotContain("#");
        excerpt.Should().NotContain("**");
        excerpt.Should().NotContain("*");
        excerpt.Should().NotContain("`");
        excerpt.Should().NotContain("[");
        excerpt.Should().NotContain("](");
        excerpt.Should().Contain("bold and italic and code and a link");
    }

    /// <summary>
    /// Test: Empty markdown should return empty excerpt
    /// Why: Defensive programming - handle edge cases
    /// </summary>
    [Fact]
    public void ExtractExcerpt_EmptyMarkdown_ReturnsEmpty()
    {
        // Act: Extract from empty content
        var excerpt = _service.ExtractExcerpt(string.Empty);

        // Assert: Empty string returned (no exceptions)
        excerpt.Should().BeEmpty();
    }

    #endregion

    #region ProcessYouTubeEmbeds Tests

    /// <summary>
    /// Test: YouTube shortcode should convert to iframe embed
    /// Why: Tech Hub uses [YouTube: ID] syntax for video embeds
    /// </summary>
    [Fact]
    public void ProcessYouTubeEmbeds_ValidShortcode_ConvertsToIframe()
    {
        // Arrange: Markdown with YouTube shortcode
        var markdown = "Watch this video: [YouTube: dQw4w9WgXcQ]";

        // Act: Process YouTube embeds
        var result = _service.ProcessYouTubeEmbeds(markdown);

        // Assert: Shortcode replaced with iframe HTML
        result.Should().Contain("<iframe");
        result.Should().Contain("https://www.youtube.com/embed/dQw4w9WgXcQ");
        result.Should().Contain("allowfullscreen");
        result.Should().Contain("class=\"video-container\"");
        result.Should().NotContain("[YouTube:");
    }

    /// <summary>
    /// Test: Multiple YouTube embeds should all be processed
    /// Why: Articles may contain multiple videos
    /// </summary>
    [Fact]
    public void ProcessYouTubeEmbeds_MultipleVideos_ConvertsAll()
    {
        // Arrange: Multiple YouTube shortcodes
        var markdown = """
            First video: [YouTube: video1]
            
            Second video: [YouTube: video2]
            """;

        // Act: Process all embeds
        var result = _service.ProcessYouTubeEmbeds(markdown);

        // Assert: Both converted to iframes
        result.Should().Contain("embed/video1");
        result.Should().Contain("embed/video2");
        System.Text.RegularExpressions.Regex.Matches(result, "<iframe").Count.Should().Be(2);
    }

    /// <summary>
    /// Test: Case-insensitive YouTube tag matching
    /// Why: Allow [youtube:] and [YouTube:] and [YOUTUBE:]
    /// </summary>
    [Theory]
    [InlineData("[YouTube: abc123]")]
    [InlineData("[youtube: abc123]")]
    [InlineData("[YOUTUBE: abc123]")]
    public void ProcessYouTubeEmbeds_CaseInsensitive_Converts(string shortcode)
    {
        // Act: Process with different casing
        var result = _service.ProcessYouTubeEmbeds(shortcode);

        // Assert: All cases converted to iframe
        result.Should().Contain("embed/abc123");
    }

    /// <summary>
    /// Test: Jekyll/Liquid youtube tag format should convert to iframe
    /// Why: Migrated content uses {% youtube VIDEO_ID %} syntax
    /// </summary>
    [Theory]
    [InlineData("{% youtube abc123 %}")]
    [InlineData("{%youtube abc123%}")]
    [InlineData("{% youtube  abc123  %}")]
    [InlineData("{% YOUTUBE abc123 %}")]
    public void ProcessYouTubeEmbeds_JekyllTagFormat_ConvertsToIframe(string tag)
    {
        // Act: Process Jekyll youtube tag
        var result = _service.ProcessYouTubeEmbeds(tag);

        // Assert: Converted to iframe embed
        result.Should().Contain("embed/abc123");
        result.Should().Contain("video-container");
        result.Should().NotContain("{%");
    }

    /// <summary>
    /// Test: Markdown without YouTube tags should remain unchanged
    /// Why: Only process content with video embeds
    /// </summary>
    [Fact]
    public void ProcessYouTubeEmbeds_NoEmbeds_ReturnsOriginal()
    {
        // Arrange: Regular markdown without YouTube tags
        var markdown = "Just regular text with no videos.";

        // Act: Process (no embeds to replace)
        var result = _service.ProcessYouTubeEmbeds(markdown);

        // Assert: Original markdown unchanged
        result.Should().Be(markdown);
    }

    /// <summary>
    /// Test: Empty input should return empty string
    /// Why: Defensive programming
    /// </summary>
    [Fact]
    public void ProcessYouTubeEmbeds_EmptyInput_ReturnsEmpty()
    {
        // Act: Process empty content
        var result = _service.ProcessYouTubeEmbeds(string.Empty);

        // Assert: Empty string returned
        result.Should().BeEmpty();
    }

    #endregion

    #region Integration Tests

    /// <summary>
    /// Test: Full processing pipeline (YouTube → HTML)
    /// Why: Real-world usage combines YouTube processing + HTML rendering
    /// </summary>
    [Fact]
    public void FullPipeline_YouTubeAndMarkdown_ProcessesBoth()
    {
        // Arrange: Markdown with YouTube embed and formatting
        var markdown = """
            # Video Tutorial
            
            Watch this tutorial:
            
            [YouTube: tutorial123]
            
            **Key points:**
            - Point 1
            - Point 2
            """;

        // Act: Process YouTube, then convert to HTML
        var withYouTube = _service.ProcessYouTubeEmbeds(markdown);
        var html = _service.RenderToHtml(withYouTube);

        // Assert: Both transformations applied
        html.Should().Contain("<h1"); // Markdig adds id attributes
        html.Should().Contain(">Video Tutorial</h1>");
        html.Should().Contain("embed/tutorial123");
        html.Should().Contain("<strong>Key points:</strong>");
        html.Should().Contain("<ul>");
    }

    #endregion

    #region Jekyll Variable Tests

    /// <summary>
    /// Test: Jekyll page variables should be replaced with frontmatter values
    /// Why: Content files may contain {{ page.variable }} syntax from Jekyll
    /// </summary>
    [Fact]
    public void ProcessJekyllVariables_PageVariables_ReplacesWithFrontmatter()
    {
        // Arrange
        var content = "Welcome to {{ page.title }}! {{ page.description }}";
        var frontMatter = new Dictionary<string, object>
        {
            { "title", "My Page" },
            { "description", "This is a test page." }
        };

        // Act
        var result = _service.ProcessJekyllVariables(content, frontMatter);

        // Assert
        result.Should().Be("Welcome to My Page! This is a test page.");
    }

    /// <summary>
    /// <summary>
    /// Test: Variable matching should be case-insensitive
    /// Why: Frontmatter keys may have different casing than template variables
    /// </summary>
    [Fact]
    public void ProcessJekyllVariables_CaseInsensitive_ReplacesCorrectly()
    {
        // Arrange
        var content = "{{ page.YouTubeId }}";
        var frontMatter = new Dictionary<string, object>
        {
            { "youtubeid", "xyz789" }
        };

        // Act
        var result = _service.ProcessJekyllVariables(content, frontMatter);

        // Assert
        result.Should().Be("xyz789");
    }

    /// <summary>
    /// Test: Missing variables should throw exception
    /// Why: Catch content errors early during development
    /// </summary>
    [Fact]
    public void ProcessJekyllVariables_MissingVariable_ThrowsException()
    {
        // Arrange
        var content = "Title: {{ page.title }}, Missing: {{ page.missing }}";
        var frontMatter = new Dictionary<string, object>
        {
            { "title", "Test" }
        };

        // Act & Assert
        var act = () => _service.ProcessJekyllVariables(content, frontMatter);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*missing*")
            .WithMessage("*not found in frontmatter*");
    }

    /// <summary>
    /// Test: Variables with extra whitespace should be handled
    /// Why: Template syntax may have inconsistent spacing
    /// </summary>
    [Fact]
    public void ProcessJekyllVariables_WithWhitespace_ReplacesCorrectly()
    {
        // Arrange
        var content = "{{page.title}} and {{  page.description  }}";
        var frontMatter = new Dictionary<string, object>
        {
            { "title", "Title" },
            { "description", "Desc" }
        };

        // Act
        var result = _service.ProcessJekyllVariables(content, frontMatter);

        // Assert
        result.Should().Be("Title and Desc");
    }

    /// <summary>
    /// Test: Null or empty content should be handled gracefully
    /// Why: Edge case protection
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ProcessJekyllVariables_NullOrEmptyContent_ReturnsEmpty(string? content)
    {
        // Arrange
        var frontMatter = new Dictionary<string, object> { { "title", "Test" } };

        // Act
        var result = _service.ProcessJekyllVariables(content!, frontMatter);

        // Assert
        result.Should().Be(content ?? string.Empty);
    }

    /// <summary>
    /// Test: {% raw %} and {% endraw %} tags should be removed
    /// Why: These Jekyll tags escape GitHub Actions syntax in code samples
    /// </summary>
    [Fact]
    public void ProcessJekyllVariables_RawTags_RemovesThem()
    {
        // Arrange
        var content = "Code: {% raw %}${{ secrets.TOKEN }}{% endraw %}";
        var frontMatter = new Dictionary<string, object>();

        // Act
        var result = _service.ProcessJekyllVariables(content, frontMatter);

        // Assert
        result.Should().Be("Code: ${{ secrets.TOKEN }}");
    }

    /// <summary>
    /// Test: {{ "/path" | relative_url }} should extract just the path
    /// Why: Jekyll's relative_url filter is not needed in .NET
    /// </summary>
    [Fact]
    public void ProcessJekyllVariables_RelativeUrlFilter_ExtractsPath()
    {
        // Arrange
        var content = """Link: [Video]({{ "/videos/2025-01-01-Test.html" | relative_url }})""";
        var frontMatter = new Dictionary<string, object>();

        // Act
        var result = _service.ProcessJekyllVariables(content, frontMatter);

        // Assert
        result.Should().Be("""Link: [Video](/videos/2025-01-01-Test.html)""");
    }

    /// <summary>
    /// Test: Content with no frontmatter should still process raw and relative_url
    /// Why: Some content files may not have page variables but do have these patterns
    /// </summary>
    [Fact]
    public void ProcessJekyllVariables_NullFrontmatter_ProcessesOtherPatterns()
    {
        // Arrange
        var content = """{% raw %}${{ test }}{% endraw %} and {{ "/path" | relative_url }}""";

        // Act
        var result = _service.ProcessJekyllVariables(content, null!);

        // Assert
        result.Should().Be("""${{ test }} and /path""");
    }

    /// <summary>
    /// Test: page.variable inside Jekyll tags should be expanded
    /// Why: {% youtube page.youtube_id %} should become {% youtube actual_id %}
    /// </summary>
    [Fact]
    public void ProcessJekyllVariables_PageVariableInJekyllTag_Expands()
    {
        // Arrange
        var content = "{% youtube page.youtube_id %}";
        var frontMatter = new Dictionary<string, object>
        {
            { "youtube_id", "dQw4w9WgXcQ" }
        };

        // Act
        var result = _service.ProcessJekyllVariables(content, frontMatter);

        // Assert
        result.Should().Be("{% youtube dQw4w9WgXcQ %}");
    }

    #endregion

    #region Singleton Stateless Verification

    /// <summary>
    /// CRITICAL: Verifies MarkdownService is stateless (required for Singleton)
    /// This test will FAIL if someone adds mutable state to the service
    /// Production uses Singleton registration, so this catches production-breaking bugs
    /// </summary>
    [Fact]
    public async Task RenderToHtml_ParallelExecution_ProducesConsistentResults()
    {
        // Arrange: Same markdown for all parallel calls
        var markdown = """
            # Test Heading
            
            This is a **test** paragraph with `code`.
            
            - Item 1
            - Item 2
            """;

        // Get expected result from first call
        var expectedHtml = _service.RenderToHtml(markdown);

        // Act: Execute 100 parallel calls to shared instance
        // If service has mutable state, results will differ
        var tasks = Enumerable.Range(0, 100).Select(async _ =>
        {
            await Task.Yield(); // Force async execution on thread pool
            return _service.RenderToHtml(markdown);
        });

        var results = await Task.WhenAll(tasks);

        // Assert: ALL results must be identical (proves stateless)
        foreach (var html in results)
        {
            html.Should().Be(expectedHtml);
        }
    }

    /// <summary>
    /// CRITICAL: Verifies ExtractExcerpt is stateless
    /// </summary>
    [Fact]
    public async Task ExtractExcerpt_ParallelExecution_ProducesConsistentResults()
    {
        // Arrange
        var markdown = """
            First paragraph with lots of content.
            
            <!--excerpt_end-->
            
            Full article continues here.
            """;

        var expectedExcerpt = _service.ExtractExcerpt(markdown);

        // Act: 100 parallel calls
        var tasks = Enumerable.Range(0, 100).Select(async _ =>
        {
            await Task.Yield();
            return _service.ExtractExcerpt(markdown);
        });

        var results = await Task.WhenAll(tasks);

        // Assert: All identical
        foreach (var excerpt in results)
        {
            excerpt.Should().Be(expectedExcerpt);
        }
    }

    /// <summary>
    /// CRITICAL: Verifies ProcessYouTubeEmbeds is stateless
    /// </summary>
    [Fact]
    public async Task ProcessYouTubeEmbeds_ParallelExecution_ProducesConsistentResults()
    {
        // Arrange
        var markdown = "Watch this: [YouTube: abc123]";
        var expected = _service.ProcessYouTubeEmbeds(markdown);

        // Act: 100 parallel calls
        var tasks = Enumerable.Range(0, 100).Select(async _ =>
        {
            await Task.Yield();
            return _service.ProcessYouTubeEmbeds(markdown);
        });

        var results = await Task.WhenAll(tasks);

        // Assert: All identical
        foreach (var result in results)
        {
            result.Should().Be(expected);
        }
    }

    #endregion
}
