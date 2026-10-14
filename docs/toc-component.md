# Table of Contents Component

**Purpose**: Describes the SidebarToc component architecture, scroll spy behavior, and usage patterns for content pages with table of contents.

## Component Overview

The `<SidebarToc>` component provides automatic table of contents generation from HTML content with scroll spy functionality that highlights the current section as users scroll.

**Location**: `src/TechHub.Web/Components/SidebarToc.razor`

## Usage Pattern

### Basic Usage

```razor
@* Extract from rendered HTML content *@
<SidebarToc HtmlContent="@item.RenderedHtml" />

@* With custom title *@
<SidebarToc HtmlContent="@GetRenderedContent()" Title="On This Page" />
```

### Generating HTML Content

The component extracts headings from HTML strings. For pages with dynamic content, generate HTML in a method:

```csharp
private string GetRenderedContent()
{
    if (pageData == null)
        return string.Empty;

    var sb = new System.Text.StringBuilder();
    sb.Append("<h2 id=\"overview\">Overview</h2>");
    sb.Append("<h2 id=\"features\">Features</h2>");
    sb.Append("<h2 id=\"conclusion\">Conclusion</h2>");
    return sb.ToString();
}
```

**Important**: The heading IDs in the generated HTML must match the actual heading IDs in your rendered content.

### Component Features

- **HTML extraction**: Parses heading tags with IDs from HTML strings. By default only h2 to h4 are included; `MinLevel` (default 2) and `MaxLevel` (default 4) change the range
- **Hierarchical nesting**: Automatically builds parent-child relationships for nested headings
- **Scroll spy integration**: Highlights active TOC link based on scroll position
- **Accessibility**: ARIA labels, semantic navigation markup, keyboard-accessible
- **Consistent styling**: Tech Hub design system integration
- **Automatic ID generation**: Generates IDs from heading text if not explicitly provided

## Scroll Spy Architecture

### Detection Point

The scroll spy detection line is positioned about **30% of the way down the area below the sticky header**. Concretely, it sits at `106px + 0.30 × (viewport height − 106px)`, where 106px is the sticky header offset (`STICKY_HEADER_OFFSET` in `scroll-manager.js`). When a heading crosses this line while scrolling, the corresponding TOC link highlights.

**Why 30%?**

- Natural reading position (users tend to read content in upper-middle viewport area)
- Balances early activation (too high) vs late activation (too low)
- Works well on mobile and desktop viewports

### Scroll Height Requirement

**Critical**: Content pages with TOC must have sufficient vertical scroll space to allow the last content section to reach the detection point.

**Implementation**: Global CSS pseudo-element in `article.css`:

```css
.article-body::after {
    content: '';
    display: block;
    height: 50vh;
}
```

**Explanation**:

- Adds 50% of viewport height as empty space after content
- Allows last heading to scroll to the detection point
- Not full viewport to avoid excessive empty scrolling
- Applied globally to all `.article-body` containers

### How Scroll Spy Works

1. **Setup**: Component marks itself with `data-toc-scroll-spy` attribute
2. **Content selector**: Points to `.article-body` container
3. **Heading tracking**: The scroll spy tracks `h2` and `h3` elements that have an `id` (it skips `h4` and deeper to reduce noise, even if the TOC lists them)
4. **JavaScript observer**: Tracks heading positions relative to detection line
5. **Highlighting**: Updates active TOC link when heading crosses detection point

**Data Attributes**:

```razor
<nav class="sidebar-section sidebar-toc" 
     aria-label="@Title" 
     data-toc-scroll-spy
     data-content-selector=".article-body">
    @* TOC content *@
</nav>
```

## Mobile Behavior

On mobile and tablet viewports (≤ 1292px) `initTocScrollSpy` adds the `toc-mobile-mode` class to the TOC and the list becomes an accordion:

- Clicking a top-level (h2) link that has sub-items expands or collapses its sub-list (`.expanded`) and does not scroll
- Clicking an h2 without children, or any sub-item, scrolls to the heading, updates the URL hash with `replaceState` and highlights the link
- No initial highlight pass runs on mobile, so the accordion is not pre-expanded before any user interaction

## Content Container Requirements

### Required Structure

Pages using `<SidebarToc>` must wrap content in `.article-body` container:

```razor
<div class="article-body">
    @((MarkupString)renderedHtml)
</div>
```

**Why this structure?**

- Provides scroll spy with target container selector
- Applies global scroll height spacer (via `::after` pseudo-element)
- Ensures consistent styling across content pages
- Enables scroll spy JavaScript to find headings

### HTML Content Generation

**For static markdown content**:

```razor
@* Content already rendered to HTML *@
<SidebarToc HtmlContent="@item.RenderedHtml" />
```

**For dynamic Blazor pages**:

```razor
@* Generate HTML string matching your page structure *@
<SidebarToc HtmlContent="@GetRenderedContent()" Title="Table of Contents" />

@code {
    private string GetRenderedContent()
    {
        var sb = new System.Text.StringBuilder();
        
        // Generate headings that match your actual page structure
        foreach (var section in pageData.Sections)
        {
            sb.Append($"<h2 id=\"{section.Id}\">{section.Title}</h2>");
        }
        
        return sb.ToString();
    }
}
```

**Key principle**: The generated HTML must mirror your actual page heading structure with matching IDs.

## Expected Behavior

### User Interactions

**Clicking TOC Links**:

- Smooth scroll to corresponding heading in content
- Update browser URL with heading anchor (#section-id)
- Focus moves to target heading for keyboard navigation

**Scrolling Content**:

- TOC link highlights when corresponding heading crosses the detection point (see [Detection Point](#detection-point))
- Only one TOC link highlighted at a time (the current/active section)
- Highlighting updates automatically as user scrolls through content

**Last Section Scrolling**:

- Users can scroll the last content section high enough for it to reach the detection point
- 50vh of empty space after content ensures this behavior
- No jarring stops or excessive empty scrolling

### Accessibility Expectations

- TOC wrapped in semantic `<nav>` element with descriptive `aria-label`
- All TOC links keyboard-accessible via Tab navigation
- Screen readers announce TOC structure and navigation purpose
- Focus states clearly visible for keyboard users
- Heading anchors receive focus when clicked from TOC

## Related Documentation

- [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md) - Blazor component implementation patterns
- [src/TechHub.Infrastructure/AGENTS.md](../src/TechHub.Infrastructure/AGENTS.md) - Markdown processing and rendering services
