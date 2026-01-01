# Liquid Templates & Includes Development Guide

> **AI CONTEXT**: Read this file when working on Liquid templates in `_includes/` or `_layouts/` directories.

> **ALSO READ**: [.github/agents/fullstack.md](../.github/agents/fullstack.md) for Jekyll/Liquid framework specifics

## Purpose

This directory contains reusable Liquid template components that are included in layouts and pages across the site. These templates handle navigation, content display, filtering UI, and other shared functionality.

## Directory Contents

**`_includes/`** - Reusable template components:

- `header.html` - Site header and navigation including section subnavigation
- `footer.html` - Site footer with links and copyright
- `filters.html` - Tag and date filtering UI components
- `items.html` - Content item display and listing
- `google-analytics.html` - Analytics tracking code

**`_layouts/`** - Page layout templates:

- `home.html` - Homepage layout with sections grid
- `default.html` - Base layout template (if exists)
- Other specialized layouts

## Website Navigation Structure

> **IMPLEMENTATION**: All header and navigation logic is implemented in [header.html](header.html)

The Tech Hub uses a **two-tier navigation system** consisting of:

1. **Main Navigation Bar** (`site-header`) - Top-level section navigation (AI, GitHub Copilot, ML, Azure, Coding, DevOps, Security)
2. **Sub-Navigation Bar** (`site-subheader`) - Collection links within the current section (News, Videos, Community, Blogs, etc.)

### Visual Layout

```text
┌─────────────────────────────────────────────────────────────┐
│ SITE HEADER (Main Navigation)                               │
│ [Logo]  AI | GitHub Copilot | ML | Azure | Coding | DevOps │
└─────────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────────┐
│ SITE SUBHEADER (Collection Navigation)                      │
│          News | Videos | Community | Blogs                  │
└─────────────────────────────────────────────────────────────┘
```

## Main Navigation Bar (site-header)

### What It Does

- Displays top-level **section links** (AI, GitHub Copilot, ML, Azure, etc.)
- Shows site logo(s):
  - **Default**: Tech Hub logo and About link (server-side rendered)
  - **Xebia mode**: Xebia + GitHub Copilot logos and Contact link (JavaScript override via `logo-manager.js`)
- Provides mobile hamburger menu
- **Highlights the current active section**

### Section Highlighting

The main navigation highlights the current section using the `active` class on section links:

```html
<a class="page-link regular active" href="/ai/">AI</a>
```

**Visual Effect**: Purple underline appears beneath the active section link (see [_sass/_layout.scss](../_sass/_layout.scss) lines 169-184)

### Section Detection Logic

The system determines the current section through **multiple detection mechanisms** (in priority order):

1. **URL Path Detection** (server-side, highest priority):
   - Extracts first URL segment (e.g., `/ai/news/` → `ai`)
   - Validates against sections defined in `_data/sections.json`
   - Sets `current_section = "ai"` and `section_in_url_path = true`

2. **Frontmatter `section` Field** (server-side):
   - Checks `page.section` in YAML frontmatter
   - Used when URL doesn't contain section path

3. **URL Querystring Parameter** (client-side via sections.js):
   - Reads `?section=` parameter from URL
   - JavaScript updates navigation highlighting dynamically
   - Used for content items outside section paths (e.g., `/videos/item.html?section=ai`)

**Example URL Processing**:

```text
/ai/news/                        → current_section = "ai" (from URL path)
/videos/item.html                → current_section = "" (no section detected)
/videos/item.html?section=ai     → sections.js activates AI section client-side
```

## Sub-Navigation Bar (site-subheader)

### What It Displays

- Displays **collection links** for the current section (News, Videos, Community, Blogs)
- Shows custom page links defined in section configuration
- **Highlights the current active collection**
- Adapts dynamically based on section context

### Collection Highlighting

The sub-navigation highlights the current collection using the `active` class:

```html
<a class="collection-link active" href="/ai/news.html">News</a>
```

**Visual Effect**: Purple background and bold font (see [_sass/_subheader.scss](../_sass/_subheader.scss) lines 41-45)

### Collection Detection Logic

**Server-Side Only** - Collection detection and highlighting is fully determined during Jekyll build:

1. **`alt-collection` Frontmatter** (highest priority):
   - Used for content in special subfolders (e.g., `_videos/ghc-features/`)
   - Overrides default collection behavior
   - Example: `alt-collection: "features"` → highlights "Features" tab

2. **`collection` Frontmatter**:
   - Standard Jekyll collection field (e.g., `collection: "videos"`)
   - Automatically set for content in collection directories

3. **URL Path Detection** (lowest priority):
   - Extracts second URL segment (e.g., `/ai/news/` → `news`)
   - Used when no frontmatter collection is specified

**Important**: Collection highlighting is always determined server-side. JavaScript never modifies collection highlighting.

**Example Collection Processing**:

```text
_videos/ghc-features/item.md with alt-collection: "features"
→ current_collection = "features" → highlights "Features" tab

_videos/regular-video.md
→ current_collection = "videos" → highlights "Videos" tab

/ai/news.html
→ current_collection = "news" → highlights "News" tab
```

## How Navigation Rendering Works

### Server-Side Rendering (header.html)

**Main Navigation**:

1. Loop through all sections from `site.data.sections`
2. Add `active` class if section matches `current_section`
3. Render all section links with appropriate highlighting

**Sub-Navigation**:

1. Render **empty default subnavigation** (id: `section-collections-default`)
   - Shown when `current_section == ''` (no section detected)
   - Hidden otherwise
2. Loop through all sections and render their subnavigations:
   - Each section gets a `div` with id `section-collections-{section-name}`
   - Visible if `current_section` matches that section
   - Hidden otherwise (CSS class `hidden` applied)
3. For each collection in the section:
   - Check if collection matches `current_collection`
   - Add `active` class to matching collection link

### Client-Side Enhancement (sections.js)

**When Loaded**: Only when `section_in_url_path == false` and not on homepage

**Purpose**: Handle `?section=` query parameter when no section was detected server-side

**What It Does**:

1. Read `?section=` parameter from URL
2. Hide all section subnavigations (including the default empty one)
3. Show the appropriate section's subnavigation
4. Highlight the section in main navigation bar

**What It Does NOT Do**:

- Does NOT determine or change collection highlighting (already set server-side)
- Does NOT modify the collection links themselves
- Does NOT handle any server-side logic

**Use Case**: Content items like `/videos/item.html?section=ai` that need section context

**Example**:

```text
User clicks video from AI section
→ /videos/item.html?section=ai
→ Server renders page with collection highlighting already set
→ sections.js reads "ai" parameter
→ Hides default subnavigation
→ Shows AI section's subnavigation (with correct collection already highlighted)
→ Highlights AI in main navigation
```

## Key Architectural Rules

### Critical Principles

1. **All subnavigations are always rendered** - Every section's subnavigation is rendered in HTML on every page
2. **Visibility is CSS-based** - Only the `hidden` class controls visibility, not conditional rendering
3. **Server-side first** - Initial state is determined server-side; JavaScript only enhances
4. **Exactly one subnavigation visible** - At any time, only one subnavigation should be visible
5. **All navigation logic is in header.html** - The complete navigation system is implemented in one file

### Why All Subnavigations Are Rendered

- Enables JavaScript to switch between sections without reloading
- Supports URL parameter-based section activation
- Maintains consistent HTML structure across all pages
- Simplifies client-side logic (just toggle CSS classes)

## Styling & Visual Design

### Main Navigation Styling

**Location**: [_sass/_layout.scss](../_sass/_layout.scss)

**Key Classes**:

- `.site-header` - Header container
- `.page-link.regular` - Section links with underline effect
- `.page-link.regular.active` - Active section (purple underline)
- `.page-link.contact` - Special styling for Contact/About buttons

**Highlighting Effect**:

```scss
&.regular {
  &::after {
    content: '';
    width: 0;
    transition: width 0.3s ease-in-out;
  }
  
  &.active::after {
    width: 100%; // Purple underline expands
  }
}
```

### Sub-Navigation Styling

**Location**: [_sass/_subheader.scss](../_sass/_subheader.scss)

**Key Classes**:

- `.site-subheader` - Subnavigation container
- `.section-collections` - Individual section's collection container
- `.section-collections.hidden` - Hidden state (`display: none`)
- `.collection-link` - Collection link
- `.collection-link.active` - Active collection (purple background, bold)

**Highlighting Effect**:

```scss
.collection-link.active {
  background: rgba($bright-purple, 0.5);
  color: $light-gray;
  font-weight: 600;
}
```

## Development Standards

### Liquid Template Patterns

**Keep Templates Simple**:

- Move complex logic to Ruby plugins (in `_plugins/`)
- Use custom filters for repeated operations
- Templates should focus on rendering, not business logic

**Data Access**:

```liquid
{%- comment -%} ✅ CORRECT: Access data files via site.data -%}
{%- assign sections = site.data.sections -%}

{%- comment -%} ❌ WRONG: This will NOT work -%}
{%- assign sections = site.sections -%}
```

**Passing Data to Includes**:

```liquid
{%- comment -%} Pass data to included files -%}
{% include component.html tags=page.tags blogs=site.blogs %}

{%- comment -%} Access in the included file with include. prefix -%}
{{ include.tags }}
{{ include.blogs }}
```

### Configuration-Driven Design

**Never Hardcode Values**:

```liquid
{%- comment -%} ✅ CORRECT: Dynamic from configuration -%}
{%- for section in site.data.sections -%}
  <a href="{{ section.url }}">{{ section.title }}</a>
{%- endfor -%}

{%- comment -%} ❌ WRONG: Hardcoded sections -%}
<a href="/ai">AI</a>
<a href="/github-copilot">GitHub Copilot</a>
```

Always derive sections, collections, categories from `_data/sections.json`.

### Performance Considerations

**Server-Side First**:

- All visible content MUST be rendered server-side during Jekyll build
- JavaScript should only enhance, never create initial content
- The ONLY exception: `assets/js/sections.js` for querystring-based section switching

**Efficient Rendering**:

- Use Jekyll filters instead of complex Liquid loops
- Pre-calculate data in plugins when possible
- Minimize template nesting depth

### Timezone Handling

**All Date Operations**:

- Use `Europe/Brussels` timezone (configured in `_config.yml`)
- Use custom filters from `_plugins/date_filters.rb`:
  - `date_to_epoch` - Convert dates to Unix epoch
  - `now_epoch` - Get current timestamp
  - `limit_with_same_day` - Apply content limiting rule

**Example**:

```liquid
{%- assign current_epoch = '' | now_epoch -%}
{%- assign filtered_blogs = site.blogs | where_exp: "item", "item.date | date_to_epoch >= current_epoch" -%}
```

## Testing

After modifying templates:

1. **Check syntax**: Jekyll will report template errors during build
2. **Verify rebuild**: Wait for "...done in X seconds" message in terminal
3. **Test in browser**: Hard refresh (Ctrl+Shift+R) to clear cache
4. **Run tests**: See [spec/AGENTS.md](../spec/AGENTS.md) for test commands

## Related Documentation

- [.github/agents/fullstack.md](../.github/agents/fullstack.md) - Jekyll and Liquid framework details
- [_plugins/AGENTS.md](../_plugins/AGENTS.md) - Ruby plugin development
- [assets/js/AGENTS.md](../assets/js/AGENTS.md) - JavaScript enhancements
- [Root AGENTS.md](../AGENTS.md) - Architecture and principles
