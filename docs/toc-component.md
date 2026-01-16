# Table of Contents Component

**Purpose**: Describes the SidebarToc component architecture, scroll spy behavior, and usage patterns for content pages with table of contents.

## Component Overview

The `<SidebarToc>` component provides automatic table of contents generation from HTML content with scroll spy functionality that highlights the current section as users scroll.

**Location**: `src/TechHub.Web/Components/SidebarToc.razor`

## Usage Pattern

### Basic Usage

```razor
@* Auto-extract from rendered HTML *@
<SidebarToc HtmlContent="@item.RenderedHtml" />

@* With custom title *@
<SidebarToc HtmlContent="@html" Title="On This Page" />

@* With manual items *@
<SidebarToc Items="@tocItems" />
```

### Component Features

- **Auto-extraction**: Parses `<h2>` and `<h3>` tags from HTML content
- **Hierarchical nesting**: Proper parent-child relationships for nested headings
- **Scroll spy**: Highlights active TOC link based on scroll position
- **Accessibility**: ARIA labels and semantic navigation markup
- **Consistent styling**: Tech Hub design system integration

## Scroll Spy Architecture

### Detection Point

The scroll spy detection line is positioned at **~40% from the top of the viewport**. When a heading crosses this line while scrolling, the corresponding TOC link highlights.

**Why 40%?**

- Natural reading position (users tend to read content in upper-middle viewport area)
- Balances early activation (too high) vs late activation (too low)
- Works well on mobile and desktop viewports

### Scroll Height Requirement

**Critical**: Content pages with TOC must have sufficient vertical scroll space to allow the last content section to reach the detection point.

**Implementation**: Global CSS pseudo-element in `markdown-content.css`:

```css
.content-detail-body::after {
    content: '';
    display: block;
    height: 50vh;
}
```

**Explanation**:

- Adds 50% of viewport height as empty space after content
- Allows last heading to scroll to detection point (~40% from top)
- Not full viewport to avoid excessive empty scrolling
- Applied globally to all `.content-detail-body` containers

### How Scroll Spy Works

1. **Setup**: Component marks itself with `data-toc-scroll-spy` attribute
2. **Content selector**: Points to `.content-detail-body` container
3. **JavaScript observer**: Tracks heading positions relative to detection line
4. **Highlighting**: Updates active TOC link when heading crosses detection point

**Data Attributes**:

```razor
<nav class="sidebar-section sidebar-toc" 
     aria-label="@Title" 
     data-toc-scroll-spy
     data-content-selector=".content-detail-body">
    @* TOC content *@
</nav>
```

## Content Container Requirements

### Required Structure

Pages using `<SidebarToc>` must wrap content in `.content-detail-body` container:

```razor
<div class="content-detail-body">
    @((MarkupString)renderedHtml)
</div>
```

**Why this structure?**

- Provides scroll spy with target container selector
- Applies global scroll height spacer (via `::after` pseudo-element)
- Ensures consistent styling across content pages
- Enables scroll spy JavaScript to find headings

### Automatic vs Manual TOC

**Always prefer automatic extraction**:

```razor
@* CORRECT - Auto-extract from HTML *@
<SidebarToc HtmlContent="@item.RenderedHtml" />
```

**Avoid manual TOC**:

```razor
@* INCORRECT - Manual TOC (maintenance burden) *@
<nav class="sidebar-toc">
    <h2>Table of Contents</h2>
    <ul>
        <li><a href="#section1">Section 1</a></li>
        <li><a href="#section2">Section 2</a></li>
    </ul>
</nav>
```

**Exceptions**: Manual TOC only when content structure doesn't follow standard heading hierarchy.

## Expected Behavior

### User Interactions

**Clicking TOC Links**:

- Smooth scroll to corresponding heading in content
- Update browser URL with heading anchor (#section-id)
- Focus moves to target heading for keyboard navigation

**Scrolling Content**:

- TOC link highlights when corresponding heading crosses detection point (~40% from viewport top)
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

- [Content Rendering](content-rendering.md) - How markdown converts to HTML with heading anchors
- [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md) - Blazor component implementation patterns
