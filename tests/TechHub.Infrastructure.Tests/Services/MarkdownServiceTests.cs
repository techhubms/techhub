using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Tests for markdown rendering and processing
/// Validates: HTML conversion, excerpt extraction, YouTube embeds
/// </summary>
public class MarkdownServiceTests
{
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
        Assert.Contains("<h1", html); // Markdig adds id attributes
        Assert.Contains(">Heading 1</h1>", html);
        Assert.Contains("<p>This is a paragraph.</p>", html);
        Assert.Contains("<ul>", html);
        Assert.Contains("<li>List item 1</li>", html);
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
        Assert.True(html.Contains("<table>") || html.Contains("<table class=\"table\">"),
            "Expected table element with or without Bootstrap class");
        Assert.Contains("<th>Header 1</th>", html);
        Assert.Contains("<td>Cell 1</td>", html);
        Assert.Contains("<del>strikethrough</del>", html);
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
        Assert.Empty(html);
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
        Assert.Contains("<a href=\"https://github.com/features/copilot\">", html);
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
        Assert.Contains("This is the excerpt", excerpt);
        Assert.Contains("multiple paragraphs", excerpt);
        Assert.DoesNotContain("full article content", excerpt);
        Assert.DoesNotContain("<!--excerpt_end-->", excerpt);
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
        Assert.Contains("First paragraph", excerpt);
        Assert.DoesNotContain("Second paragraph", excerpt);
        Assert.DoesNotContain("Third paragraph", excerpt);
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
        Assert.True(excerpt.Length <= 1003); // 1000 + "..."
        Assert.EndsWith("...", excerpt);
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
        Assert.DoesNotContain("#", excerpt);
        Assert.DoesNotContain("**", excerpt);
        Assert.DoesNotContain("*", excerpt);
        Assert.DoesNotContain("`", excerpt);
        Assert.DoesNotContain("[", excerpt);
        Assert.DoesNotContain("](", excerpt);
        Assert.Contains("bold and italic and code and a link", excerpt);
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
        Assert.Empty(excerpt);
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
        Assert.Contains("<iframe", result);
        Assert.Contains("https://www.youtube.com/embed/dQw4w9WgXcQ", result);
        Assert.Contains("allowfullscreen", result);
        Assert.Contains("class=\"video-container\"", result);
        Assert.DoesNotContain("[YouTube:", result);
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
        Assert.Contains("embed/video1", result);
        Assert.Contains("embed/video2", result);
        Assert.Equal(2, System.Text.RegularExpressions.Regex.Matches(result, "<iframe").Count);
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
        Assert.Contains("embed/abc123", result);
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
        Assert.Contains("embed/abc123", result);
        Assert.Contains("video-container", result);
        Assert.DoesNotContain("{%", result);
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
        Assert.Equal(markdown, result);
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
        Assert.Empty(result);
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
        Assert.Contains("<h1", html); // Markdig adds id attributes
        Assert.Contains(">Video Tutorial</h1>", html);
        Assert.Contains("embed/tutorial123", html);
        Assert.Contains("<strong>Key points:</strong>", html);
        Assert.Contains("<ul>", html);
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
        Assert.Equal("Welcome to My Page! This is a test page.", result);
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
        Assert.Equal("xyz789", result);
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
        var exception = Assert.Throws<InvalidOperationException>(
            () => _service.ProcessJekyllVariables(content, frontMatter));

        Assert.Contains("missing", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("not found in frontmatter", exception.Message);
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
        Assert.Equal("Title and Desc", result);
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
        Assert.Equal(content ?? string.Empty, result);
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
        Assert.Equal("Code: ${{ secrets.TOKEN }}", result);
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
        Assert.Equal("""Link: [Video](/videos/2025-01-01-Test.html)""", result);
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
        Assert.Equal("""${{ test }} and /path""", result);
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
        Assert.Equal("{% youtube dQw4w9WgXcQ %}", result);
    }

    #endregion
}
