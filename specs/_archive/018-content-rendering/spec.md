# Content Rendering Specification

> **Feature**: Markdown to HTML rendering pipeline with semantic markup and Schema.org integration

## Overview

The content rendering system converts Markdown source files into semantic HTML with embedded Schema.org structured data. Rendering uses Markdig with custom extensions optimized for performance and modern web features. All initial rendering happens server-side (SSR), with optional client-side enhancements for interactive features.

## Requirements

### Functional Requirements

**FR-1**: The system MUST render all Markdown content to HTML during SSR  
**FR-2**: The system MUST support GitHub Flavored Markdown (GFM) syntax  
**FR-3**: The system MUST apply syntax highlighting to code blocks using modern highlight.js or Prism.js  
**FR-4**: The system MUST generate semantic HTML5 elements  
**FR-5**: The system MUST embed Schema.org JSON-LD structured data  
**FR-6**: The system MUST support custom extensions (YouTube embeds, callouts, diagrams)  
**FR-7**: The system MUST generate table of contents for articles with headings  
**FR-8**: The system MUST lazy-load images with modern `loading="lazy"` attribute  
**FR-9**: The system MUST generate responsive images with `srcset` for different screen sizes  
**FR-10**: The system MUST support dark mode for syntax highlighting  

### Non-Functional Requirements

**NFR-1**: Rendering MUST complete in < 20ms per item (p95)  
**NFR-2**: Output HTML MUST pass W3C HTML5 validation  
**NFR-3**: Rendered content MUST be accessible (WCAG 2.1 Level AA)  
**NFR-4**: Code blocks MUST use accessible color schemes with 4.5:1 contrast  
**NFR-5**: Images MUST include width/height to prevent layout shift (CLS < 0.1)  
**NFR-6**: Syntax highlighting MUST support light/dark themes  

## Use Cases

### UC-1: Render News Article

**Actor**: Content System  
**Precondition**: Markdown file exists with valid frontmatter  
**Trigger**: Page request or build process  

**Flow**:

1. System reads Markdown file
2. System parses YAML frontmatter
3. System converts Markdown to HTML using Markdig
4. System applies syntax highlighting to code blocks
5. System wraps content in semantic HTML (`<article>`, `<section>`, etc.)
6. System embeds Schema.org `Article` structured data
7. System returns rendered HTML

**Postcondition**: HTML page is ready for display

### UC-2: Render Video Content

**Actor**: Content System  
**Precondition**: Video Markdown file with `youtube_id` frontmatter  

**Flow**:

1. System reads Markdown file
2. System detects `youtube_id` in frontmatter
3. System converts Markdown to HTML
4. System replaces `{% youtube VIDEO_ID %}` tags with iframe embeds
5. System embeds Schema.org `VideoObject` structured data
6. System returns rendered HTML

**Postcondition**: HTML page includes embedded video player

### UC-3: Generate Table of Contents

**Actor**: Content System  
**Precondition**: Article has 3+ headings  

**Flow**:

1. System renders Markdown to HTML
2. System extracts all `<h2>` and `<h3>` elements
3. System generates anchor links for each heading
4. System creates nested TOC structure
5. System injects TOC before first heading

**Postcondition**: Article includes navigable table of contents

## Acceptance Criteria

**AC-1**: Given a Markdown file with frontmatter, when rendered, then valid HTML is produced  
**AC-2**: Given a code block with language specified, when rendered, then syntax highlighting is applied  
**AC-3**: Given a video file with youtube_id, when rendered, then YouTube iframe is embedded  
**AC-4**: Given an article with 3+ headings, when rendered, then TOC is generated  
**AC-5**: Given any rendered content, when validated, then W3C validation passes  
**AC-6**: Given any rendered content, when tested with screen reader, then all content is accessible  

## Technical Approach

### Technology Stack

- **Markdown Parser**: Markdig (extensible, GFM-compliant)
- **Syntax Highlighter**: Markdig.SyntaxHighlighting (Roslyn-based)
- **Template Engine**: Razor (for layout composition)
- **Schema.org**: Custom Razor components

### Rendering Pipeline

```text
Markdown File → Frontmatter Parser → Markdig Pipeline → HTML String
                                          ↓
                                  Syntax Highlighting
                                          ↓
                                  Custom Extensions
                                          ↓
                              Semantic HTML Wrapper
                                          ↓
                              Schema.org Injection
                                          ↓
                                   Final HTML
```

### Semantic HTML Mapping

| Markdown | HTML Element | Schema.org Type |
| ---------- | -------------- | ----------------- |
| Article content | `<article>` | `Article` |
| Blog post | `<article>` | `BlogPosting` |
| Video | `<article>` | `VideoObject` |
| Sections | `<section>` | - |
| Headings | `<h1>`-`<h6>` | - |
| Paragraphs | `<p>` | - |
| Lists | `<ul>`, `<ol>` | - |
| Code blocks | `<pre><code>` | - |
| Images | `<figure><img>` | `ImageObject` |
| Blockquotes | `<blockquote>` | - |
| Tables | `<table>` | - |

### Markdig Configuration

```csharp
var pipeline = new MarkdownPipelineBuilder()
    .UseAdvancedExtensions()          // GFM support
    .UseSyntaxHighlighting()          // Code highlighting
    .UseAutoLinks()                   // Auto-link URLs
    .UseEmphasisExtras()              // Strikethrough, etc.
    .UseGenericAttributes()           // Custom attributes
    .UsePipeTables()                  // Tables
    .UseTaskLists()                   // Checkboxes
    .UseAutoIdentifiers()             // Heading IDs
    .Build();
```

### Custom Extensions

**YouTube Tag Processor**:

```csharp
public class YoutubeExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline) { }
    
    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        renderer.ObjectRenderers.AddIfNotAlready<YoutubeRenderer>();
    }
}
```

### Schema.org Template

```html
<script type="application/ld+json">
{
  "@context": "https://schema.org",
  "@type": "Article",
  "headline": "@Model.Title",
  "datePublished": "@Model.Date.ToString("o")",
  "author": {
    "@type": "Organization",
    "name": "Tech Hub"
  },
  "publisher": {
    "@type": "Organization",
    "name": "Tech Hub",
    "logo": {
      "@type": "ImageObject",
      "url": "https://tech.hub.ms/assets/logo.png"
    }
  },
  "description": "@Model.Excerpt",
  "image": "@Model.Image"
}
</script>
```

## Testing Strategy

### Unit Tests

- Test Markdown → HTML conversion accuracy
- Test frontmatter parsing
- Test custom tag processing (YouTube embeds)
- Test TOC generation logic
- Test Schema.org data generation

### Integration Tests

- Test full rendering pipeline end-to-end
- Test with various Markdown inputs
- Test semantic HTML output structure
- Test W3C validation compliance

### Accessibility Tests

- Test with screen readers (NVDA, JAWS)
- Test keyboard navigation
- Test color contrast ratios
- Test ARIA attributes

## Edge Cases

**EC-1**: Empty Markdown file → Render with default structure  
**EC-2**: Invalid frontmatter → Log warning, use defaults  
**EC-3**: Malformed Markdown → Render as-is, log error  
**EC-4**: Missing youtube_id → Render placeholder  
**EC-5**: Extremely long content → Apply lazy loading, pagination  
**EC-6**: Special characters in headings → Sanitize for IDs  

## Performance Considerations

- Cache rendered HTML output (in-memory, per-request)
- Pre-compile Markdig pipeline at startup
- Use string builders for HTML generation
- Lazy-load images below the fold
- Minify HTML output (optional)

## Dependencies

- Markdig NuGet package
- Markdig.SyntaxHighlighting NuGet package
- YamlDotNet (frontmatter parsing)
- HtmlAgilityPack (HTML manipulation if needed)

## Migration Notes

**From Jekyll**:

- Replace Liquid templates with Razor
- Replace Rouge syntax highlighting with Markdig.SyntaxHighlighting
- Migrate custom Liquid tags to Markdig extensions
- Ensure same HTML output structure for SEO

## Open Questions

1. Should we cache rendered HTML in Redis or just in-memory?
2. Do we need real-time preview during content editing?
3. Should TOC generation be optional via frontmatter flag?
4. How to handle videos from sources other than YouTube?

## References

- [Markdig Documentation](https://github.com/xoofx/markdig)
- [Schema.org](https://schema.org/)
- [GFM Specification](https://github.github.com/gfm/)
