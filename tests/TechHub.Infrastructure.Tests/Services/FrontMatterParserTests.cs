using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Tests for YAML frontmatter parsing
/// Validates: frontmatter extraction, content separation, error handling
/// </summary>
public class FrontMatterParserTests
{
    private readonly FrontMatterParser _parser;

    public FrontMatterParserTests()
    {
        _parser = new FrontMatterParser();
    }

    /// <summary>
    /// Test: Valid frontmatter with title, date, and tags should parse correctly
    /// Why: This is the most common markdown file format in the codebase
    /// </summary>
    [Fact]
    public void Parse_ValidFrontMatter_ExtractsMetadataAndContent()
    {
        // Arrange: Standard markdown file with frontmatter
        var markdown = """
            ---
            title: Test Article
            date: 2026-01-01
            tags:
              - AI
              - GitHub Copilot
            ---
            # Article Content
            
            This is the body.
            """;

        // Act: Parse the markdown
        var (frontMatter, content) = _parser.Parse(markdown);

        // Assert: Frontmatter extracted correctly, content separated
        Assert.Equal(3, frontMatter.Count);
        Assert.Equal("Test Article", frontMatter["title"]);
        Assert.Equal("2026-01-01", frontMatter["date"]);
        Assert.StartsWith("# Article Content", content);
        Assert.Contains("This is the body.", content);
    }

    /// <summary>
    /// Test: Empty or whitespace-only input should return empty results
    /// Why: Prevents null reference errors when processing empty files
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\n\n")]
    public void Parse_EmptyInput_ReturnsEmptyResults(string input)
    {
        // Act: Parse empty content
        var (frontMatter, content) = _parser.Parse(input);

        // Assert: Safe empty returns (no exceptions thrown)
        Assert.Empty(frontMatter);
        Assert.Empty(content);
    }

    /// <summary>
    /// Test: Markdown without frontmatter should return all content
    /// Why: Some files may not have frontmatter (e.g., README.md)
    /// </summary>
    [Fact]
    public void Parse_NoFrontMatter_ReturnsContentOnly()
    {
        // Arrange: Plain markdown without frontmatter
        var markdown = """
            # Regular Markdown
            
            No frontmatter here.
            """;

        // Act: Parse content-only markdown
        var (frontMatter, content) = _parser.Parse(markdown);

        // Assert: Empty frontmatter, full content preserved
        Assert.Empty(frontMatter);
        Assert.Equal(markdown, content);
    }

    /// <summary>
    /// Test: Invalid YAML should fail gracefully
    /// Why: Malformed frontmatter shouldn't crash the parser
    /// </summary>
    [Fact]
    public void Parse_InvalidYaml_ReturnsEmptyFrontMatter()
    {
        // Arrange: Broken YAML syntax (unclosed bracket)
        var markdown = """
            ---
            title: Test
            invalid: [broken yaml
            ---
            Content
            """;

        // Act: Parse malformed YAML
        var (frontMatter, content) = _parser.Parse(markdown);

        // Assert: Returns original content when YAML parsing fails
        Assert.Empty(frontMatter);
        Assert.Equal(markdown, content);
    }

    /// <summary>
    /// Test: Frontmatter with only opening --- should extract YAML until EOF
    /// Why: In practice, parsers tolerate missing closing --- and treat rest of file as YAML
    /// This matches behavior of many static site generators (Jekyll, Hugo)
    /// </summary>
    [Fact]
    public void Parse_UnclosedFrontMatter_ParsesUntilEOF()
    {
        // Arrange: Missing closing --- (rest is treated as YAML)
        var markdown = """
            ---
            title: Test
            description: Some value
            """;

        // Act: Parse incomplete frontmatter
        var (frontMatter, content) = _parser.Parse(markdown);

        // Assert: YAML parsed until EOF, no content after frontmatter
        Assert.Equal(2, frontMatter.Count);
        Assert.Equal("Test", frontMatter["title"]);
        Assert.Equal("Some value", frontMatter["description"]);
        Assert.Empty(content); // No content after unclosed frontmatter
    }

    /// <summary>
    /// Test: GetValue should return typed values or defaults
    /// Why: Type-safe access to frontmatter fields
    /// </summary>
    [Fact]
    public void GetValue_ExistingKey_ReturnsTypedValue()
    {
        // Arrange: Frontmatter with various types
        var frontMatter = new Dictionary<string, object>
        {
            { "title", "Test" },
            { "count", 42 },
            { "enabled", true }
        };

        // Act: Retrieve typed values
        var title = _parser.GetValue<string>(frontMatter, "title");
        var count = _parser.GetValue<int>(frontMatter, "count");
        var enabled = _parser.GetValue<bool>(frontMatter, "enabled");

        // Assert: Correct types returned
        Assert.Equal("Test", title);
        Assert.Equal(42, count);
        Assert.True(enabled);
    }

    /// <summary>
    /// Test: GetValue with missing key should return default
    /// Why: Safe fallback for optional frontmatter fields
    /// </summary>
    [Fact]
    public void GetValue_MissingKey_ReturnsDefault()
    {
        // Arrange: Empty frontmatter
        var frontMatter = new Dictionary<string, object>();

        // Act: Access non-existent keys with defaults
        var title = _parser.GetValue(frontMatter, "title", "Default Title");
        var count = _parser.GetValue(frontMatter, "count", 0);

        // Assert: Default values returned
        Assert.Equal("Default Title", title);
        Assert.Equal(0, count);
    }

    /// <summary>
    /// Test: GetListValue should handle various list formats
    /// Why: YAML supports multiple list syntaxes (array, flow, single value)
    /// </summary>
    [Fact]
    public void GetListValue_VariousFormats_ReturnsList()
    {
        // Arrange: Different list representations in YAML
        var frontMatter = new Dictionary<string, object>
        {
            { "tags_array", new List<object> { "ai", "ml" } },
            { "tags_single", "azure" },
            { "tags_missing", null! }
        };

        // Act: Parse different list formats
        var arrayList = _parser.GetListValue(frontMatter, "tags_array");
        var singleList = _parser.GetListValue(frontMatter, "tags_single");
        var missingList = _parser.GetListValue(frontMatter, "tags_missing");

        // Assert: All converted to List<string>
        Assert.Equal(2, arrayList.Count);
        Assert.Contains("ai", arrayList);
        Assert.Single(singleList);
        Assert.Equal("azure", singleList[0]);
        Assert.Empty(missingList);
    }

    /// <summary>
    /// Test: Complex real-world frontmatter should parse correctly
    /// Why: Integration test with actual content file structure
    /// </summary>
    [Fact]
    public void Parse_RealWorldExample_ParsesComplexFrontMatter()
    {
        // Arrange: Actual frontmatter format from Tech Hub content files
        var markdown = """
            ---
            title: "GitHub Copilot: New Features"
            date: 2026-01-01 10:30:00 +0100
            author: Microsoft
            section_names: GitHub Copilot
            tags:
              - Features
              - Updates
              - AI
            ---
            # New Features
            
            GitHub Copilot now supports...
            
            <!--excerpt_end-->
            
            Full article content here.
            """;

        // Act: Parse complex frontmatter
        var (frontMatter, content) = _parser.Parse(markdown);

        // Assert: All fields extracted, content separated after frontmatter
        Assert.Equal(5, frontMatter.Count); // title, date, author, section_names, tags
        Assert.Equal("GitHub Copilot: New Features", frontMatter["title"]);
        Assert.Equal("Microsoft", frontMatter["author"]); // Preserves original casing from YAML
        Assert.Equal("GitHub Copilot", frontMatter["section_names"]); // Preserves title case from YAML (mapper converts to lowercase)
        Assert.StartsWith("# New Features", content);
        Assert.Contains("<!--excerpt_end-->", content);
    }
}
