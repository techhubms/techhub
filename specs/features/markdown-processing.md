# Markdown Processing Specification

> **Feature**: Markdown to HTML rendering with Markdig and custom extensions

## Overview

The markdown processing system converts Markdown source files with YAML frontmatter into ContentItem domain models with rendered HTML content. Uses Markdig for GFM-compliant parsing with custom extensions for YouTube embeds, syntax highlighting, and modern web features.

## Requirements

### Functional Requirements

**FR-1**: The system MUST parse YAML frontmatter from markdown files  
**FR-2**: The system MUST render Markdown to semantic HTML5  
**FR-3**: The system MUST support GitHub Flavored Markdown (GFM)  
**FR-4**: The system MUST apply syntax highlighting to code blocks  
**FR-5**: The system MUST extract excerpt (content before `<!--excerpt_end-->`)  
**FR-6**: The system MUST replace YouTube tags with iframe embeds  
**FR-7**: The system MUST normalize tags (lowercase, trim whitespace)  
**FR-8**: The system MUST convert dates to Unix epoch timestamps  
**FR-9**: The system MUST determine canonical URL from first category  
**FR-10**: The system MUST handle alt-collection for subfolder organization  

### Non-Functional Requirements

**NFR-1**: Parsing MUST complete in < 20ms per file (p95)  
**NFR-2**: Rendered HTML MUST be valid HTML5  
**NFR-3**: Code blocks MUST have accessible color contrast (4.5:1)  
**NFR-4**: Images MUST include width/height attributes  
**NFR-5**: Parsing MUST be thread-safe  

## Markdig Configuration

```csharp
// TechHub.Infrastructure/Services/MarkdownProcessor.cs
public class MarkdownProcessor : IMarkdownProcessor
{
    private readonly MarkdownPipeline _pipeline;
    private readonly ILogger<MarkdownProcessor> _logger;
    
    public MarkdownProcessor(ILogger<MarkdownProcessor> logger)
    {
        _logger = logger;
        
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()      // GFM tables, task lists, etc.
            .UseYamlFrontMatter()          // Parse YAML front matter
            .UseSyntaxHighlighting()       // Code syntax highlighting
            .UseAutoLinks()                // Auto-linkify URLs
            .UseEmojiAndSmiley()           // Emoji support
            .UseMediaLinks()               // Enhanced media support
            .UseFigures()                  // <figure> for images
            .Build();
    }
}
```

## YAML Frontmatter Parsing

**Required Fields**:
- `title` (string)
- `date` (string, YYYY-MM-DD or YYYY-MM-DD HH:MM:SS +ZONE)
- `author` (string)
- `categories` (array of strings)

**Optional Fields**:
- `description` (string) - Falls back to excerpt
- `tags` (array of strings) - Defaults to []
- `youtube_id` (string) - For video content
- `external_url` (string) - For external links
- `alt_collection` (string) - For subfolder organization

**Example**:
```yaml
---
title: "Introducing GitHub Copilot Workspace"
date: 2024-05-09
author: "GitHub Team"
categories: [ai, github-copilot]
tags: [copilot, workspace, vscode]
youtube_id: "abc123xyz"
---
```

## Excerpt Extraction

Excerpt is content before `<!--excerpt_end-->` marker:

```markdown
---
title: "Example Post"
---

This is the excerpt. It summarizes the main points.

<!--excerpt_end-->

This is the full content that appears after the excerpt.
```

**Rules**:
- If `<!--excerpt_end-->` present, excerpt is everything before it
- If no marker, excerpt is first 200 words
- Excerpt is rendered as HTML separately from content
- Both excerpt and content use same Markdig pipeline

## YouTube Embed Processing

Replace `{% youtube VIDEO_ID %}` tags with responsive iframe:

**Input**:
```markdown
{% youtube abc123xyz %}
```

**Output**:
```html
<div class="video-container">
  <iframe 
    width="560" 
    height="315" 
    src="https://www.youtube-nocookie.com/embed/abc123xyz" 
    title="YouTube video player" 
    frameborder="0" 
    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" 
    allowfullscreen>
  </iframe>
</div>
```

## Implementation

```csharp
// TechHub.Infrastructure/Services/MarkdownProcessor.cs
public class MarkdownProcessor : IMarkdownProcessor
{
    public async Task<ContentItem> ParseMarkdownFileAsync(
        string filePath, 
        CancellationToken ct = default)
    {
        _logger.LogDebug("Parsing markdown file: {File}", filePath);
        
        var markdown = await File.ReadAllTextAsync(filePath, ct);
        
        // Parse YAML frontmatter
        var frontMatter = ParseFrontMatter(markdown, out var content);
        
        // Extract excerpt
        var (excerpt, fullContent) = ExtractExcerpt(content);
        
        // Render HTML
        var renderedContent = RenderHtml(fullContent);
        var renderedExcerpt = RenderHtml(excerpt);
        
        // Process YouTube embeds
        renderedContent = ProcessYouTubeEmbeds(renderedContent);
        renderedExcerpt = ProcessYouTubeEmbeds(renderedExcerpt);
        
        // Determine collection from directory structure
        var collection = DetermineCollection(filePath);
        
        // Get item ID from filename
        var itemId = Path.GetFileNameWithoutExtension(filePath);
        
        // Normalize tags
        var tags = frontMatter.Tags
            .Select(t => t.ToLowerInvariant().Trim())
            .ToList();
        
        // Convert date to epoch
        var dateEpoch = DateUtils.ParseToEpoch(
            frontMatter.Date, 
            "Europe/Brussels");
        
        // Determine canonical URL from first category
        var canonicalUrl = $"/{frontMatter.Categories[0]}/{collection}/{itemId}.html";
        
        return new ContentItem
        {
            Id = itemId,
            Title = frontMatter.Title,
            Description = frontMatter.Description ?? excerpt,
            Author = frontMatter.Author,
            DateEpoch = dateEpoch,
            Collection = collection,
            Categories = frontMatter.Categories,
            Tags = tags,
            Content = renderedContent,
            Excerpt = renderedExcerpt,
            CanonicalUrl = canonicalUrl,
            ExternalUrl = frontMatter.ExternalUrl,
            VideoId = frontMatter.YouTubeId,
            AltCollection = frontMatter.AltCollection
        };
    }
    
    private FrontMatter ParseFrontMatter(string markdown, out string content)
    {
        var yamlMatch = Regex.Match(markdown, @"^---\s*\n(.*?)\n---\s*\n(.*)", 
            RegexOptions.Singleline);
        
        if (!yamlMatch.Success)
        {
            throw new InvalidOperationException("No YAML frontmatter found");
        }
        
        var yaml = yamlMatch.Groups[1].Value;
        content = yamlMatch.Groups[2].Value;
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        
        return deserializer.Deserialize<FrontMatter>(yaml);
    }
    
    private (string excerpt, string content) ExtractExcerpt(string markdown)
    {
        var excerptEndIndex = markdown.IndexOf("<!--excerpt_end-->");
        
        if (excerptEndIndex > 0)
        {
            var excerpt = markdown.Substring(0, excerptEndIndex).Trim();
            var content = markdown.Substring(excerptEndIndex + 18).Trim();
            return (excerpt, markdown); // Full content includes excerpt
        }
        
        // No marker: use first 200 words as excerpt
        var words = markdown.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var excerptWords = words.Take(200);
        var excerpt = string.Join(' ', excerptWords);
        
        return (excerpt, markdown);
    }
    
    private string RenderHtml(string markdown)
    {
        return Markdown.ToHtml(markdown, _pipeline);
    }
    
    private string ProcessYouTubeEmbeds(string html)
    {
        return Regex.Replace(html, 
            @"\{%\s*youtube\s+([a-zA-Z0-9_-]+)\s*%\}",
            match => GenerateYouTubeEmbed(match.Groups[1].Value));
    }
    
    private string GenerateYouTubeEmbed(string videoId)
    {
        return $@"
<div class=""video-container"">
  <iframe 
    width=""560"" 
    height=""315"" 
    src=""https://www.youtube-nocookie.com/embed/{videoId}"" 
    title=""YouTube video player"" 
    frameborder=""0"" 
    allow=""accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"" 
    allowfullscreen>
  </iframe>
</div>";
    }
    
    private string DetermineCollection(string filePath)
    {
        // Extract collection from directory: _videos -> videos
        var dirName = Path.GetFileName(Path.GetDirectoryName(filePath));
        return dirName?.TrimStart('_') ?? "unknown";
    }
}
```

## Date Parsing

```csharp
// TechHub.Core/Utilities/DateUtils.cs
public static long ParseToEpoch(string dateString, string timeZoneId)
{
    var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    
    // Try parsing with timezone offset first
    if (DateTimeOffset.TryParse(dateString, out var dto))
    {
        return dto.ToUnixTimeSeconds();
    }
    
    // Try parsing as date only (YYYY-MM-DD)
    if (DateTime.TryParse(dateString, out var dt))
    {
        // Assume midnight in specified timezone
        var offset = tz.GetUtcOffset(dt);
        dto = new DateTimeOffset(dt, offset);
        return dto.ToUnixTimeSeconds();
    }
    
    throw new FormatException($"Unable to parse date: {dateString}");
}
```

## Testing

```csharp
public class MarkdownProcessorTests
{
    [Fact]
    public async Task ParseMarkdownFileAsync_ValidFile_ReturnsContentItem()
    {
        // Arrange
        var processor = new MarkdownProcessor(NullLogger<MarkdownProcessor>.Instance);
        var testFile = "test-content.md";
        
        // Act
        var item = await processor.ParseMarkdownFileAsync(testFile);
        
        // Assert
        Assert.NotNull(item);
        Assert.NotEmpty(item.Title);
        Assert.NotEmpty(item.Content);
    }
    
    [Fact]
    public async Task ParseMarkdownFileAsync_WithYouTubeTag_RendersIframe()
    {
        var processor = new MarkdownProcessor(NullLogger<MarkdownProcessor>.Instance);
        
        // File contains {% youtube abc123 %}
        var item = await processor.ParseMarkdownFileAsync("video-test.md");
        
        Assert.Contains("<iframe", item.Content);
        Assert.Contains("youtube-nocookie.com/embed/abc123", item.Content);
    }
    
    [Fact]
    public async Task ParseMarkdownFileAsync_WithExcerptMarker_SeparatesContent()
    {
        var processor = new MarkdownProcessor(NullLogger<MarkdownProcessor>.Instance);
        
        var item = await processor.ParseMarkdownFileAsync("excerpt-test.md");
        
        Assert.NotEqual(item.Excerpt, item.Content);
        Assert.True(item.Content.Length > item.Excerpt.Length);
    }
}
```

## Performance Optimization

**Caching Rendered HTML**:
- Markdown parsing happens once at startup
- Rendered HTML stored in ContentItem
- No repeated parsing during requests

**Parallel Processing**:
```csharp
// Process all files in parallel
var tasks = files.Select(f => processor.ParseMarkdownFileAsync(f));
var items = await Task.WhenAll(tasks);
```

## References

- Markdig documentation: https://github.com/xoofx/markdig
- `/specs/features/domain-models.md` - ContentItem definition
- `/specs/features/repository-pattern.md` - Repository usage

