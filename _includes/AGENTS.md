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

## Section Subnavigation Behavior

The subnavigation bar (`site-subheader`) displays links to collections and custom pages within the current section. Visibility is determined as follows:

### Server-Side Rendering (`_includes/header.html`)

1. **Section in URL** (e.g., `/ai/`, `/coding/`): Shows ONLY that section's subnavigation
2. **No section in URL** (e.g., `/videos/item.html`): Shows default subnavigation (empty), ALL section subnavigations are hidden
3. **Homepage**: Shows its own dedicated subnavigation (`section-collections-home`)

### Client-Side Enhancement (`assets/js/sections.js`)

1. Only loads when section is NOT in the URL path
2. Reads `?section=` querystring parameter
3. Hides the default subnavigation
4. Shows the appropriate section subnavigation by removing `hidden` class
5. Used for content items that link back with section context (e.g., `/videos/item.html?section=coding`)

### Key Rules

- Every section ALWAYS gets its subnavigation rendered in HTML (never conditionally excluded)
- Only visibility is controlled via CSS `hidden` class
- Server-side sets initial visibility based on URL path
- JavaScript overrides based on querystring when section is not in URL path
- **At any time, exactly ONE subnavigation should be visible**

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
