# Jekyll Development Guidelines

This file contains all Jekyll-specific development guidelines, configuration, debugging, and operational procedures for the Tech Hub site.

## Jekyll Data Files Access Convention

**⚠️ CRITICAL JEKYLL CONVENTION**: All data files in Jekyll's `_data` directory are accessed via `site.data.filename` (without the `.json` extension).

**Key Rules**:

- ✅ **CORRECT**: `site.data.sections` - Accesses `_data/sections.json`
- ❌ **WRONG**: `site.sections` - This will NOT work in Jekyll
- ✅ **CORRECT**: `site.data.copilot_plans` - Accesses `_data/copilot_plans.json`
- ❌ **WRONG**: `site.copilot_plans` - This will NOT work in Jekyll

**Remember**: The `.data` part is MANDATORY for Jekyll data file access. This is a Jekyll convention that cannot be omitted.

## Jekyll Configuration and Setup

### Core Configuration

The site is configured as a Jekyll static site generator with custom plugins and specific timezone requirements.

**Main Configuration File (`_config.yml`)**:

```yaml
title: Tech Hub
email: reinier.vanmaanen@xebia.com
description: >-
  All Microsoft tech content in once place.
timezone: Europe/Brussels
google_analytics: G-95LLB67KJV

markdown: kramdown
theme: minima
plugins:
  - jekyll-feed
  - jekyll-seo-tag
  - jekyll-sitemap
```

**Key Configuration Settings**:

- **Timezone**: `Europe/Brussels` - All date calculations must respect this timezone
- **Collections**: Multiple Jekyll collections (news, videos, community, posts, etc.)
- **Plugins**: Custom Ruby plugins for page generation and tag processing
- **Build Process**: PowerShell enhancement → Jekyll build → Client enhancement

**Collections Configuration**:

```yaml
collections:
  news:
    output: true
  videos:
    output: true    
  community:
    output: true       
  events:
    output: true
  roundups:
    output: true
```

**Performance Optimizations**:

```yaml
kramdown:
  input: GFM
  hard_wrap: false
  syntax_highlighter: rouge
  syntax_highlighter_opts:
    block:
      line_numbers: false
    default_lang: text

sass:
  style: compressed
  sass_dir: _sass
```

## Jekyll Development Workflow

### Server Management Rules

**CRITICAL**: Never attempt to kill Jekyll processes using system commands like `pkill` or `kill`. Always use the provided PowerShell scripts for Jekyll server management.

**Recommended Jekyll Server Management:**

1. **Use `/workspaces/techhub/jekyll-start.ps1` to start or restart Jekyll**
   - This script automatically stops any running Jekyll servers before starting a new one
   - Includes proper cleanup and configuration for development
   - Use when you need a fresh Jekyll restart for plugin/data changes
   - No need to sleep or use curl, look at the terminal until you see `Server running` to detect when Jekyll is started.

2. **Use `/workspaces/techhub/jekyll-stop.ps1` to stop Jekyll servers**
   - Safe way to stop running Jekyll processes
   - Use when you need to stop Jekyll without restarting

3. **Scripts handle all server lifecycle management**
   - No need to manually manage Jekyll processes
   - Prevents unexpected interruptions and data loss
   - Ensures proper cleanup and restart procedures

**Available Jekyll Start Parameters:**

- `-SkipStop`: Skip stopping existing Jekyll processes (faster restart)
- `-SkipClean`: Skip Jekyll cache cleaning (faster restart)
- `-BuildInsteadOfServe`: Build site only without starting server
- `-VerboseOutput`: Enable verbose Jekyll output for debugging

**Required Jekyll Flags (handled by scripts):**

- `--livereload`: Enables automatic browser refresh during development
- `--force_polling`: Ensures file change detection works properly in all environments
- `--host 0.0.0.0`: Allows access from container environments
- `--incremental`: Faster rebuilds during development

**Example Server Management:**

```powershell
# Start or restart Jekyll (automatically stops existing servers)

pwsh /workspaces/techhub/jekyll-start.ps1

# Start with verbose output for debugging

pwsh /workspaces/techhub/jekyll-start.ps1 -VerboseOutput

# Fast restart (skip stop and clean for quicker iteration)

pwsh /workspaces/techhub/jekyll-start.ps1 -SkipStop -SkipClean

# Build site only without serving (fastest for debugging)

pwsh /workspaces/techhub/jekyll-start.ps1 -SkipStop -SkipClean -BuildInsteadOfServe

# Stop Jekyll server

pwsh /workspaces/techhub/jekyll-stop.ps1
```

### Debugging Without Server

**Static Site Debugging**: For debugging and inspection, you don't need a running Jekyll server since this is a static HTML website.

**Alternative Debugging Workflow:**

**Jekyll Build Debugging:**

1. **Build the site**: Use `pwsh ./jekyll-start.ps1 -SkipStop -SkipClean -BuildInsteadOfServe -VerboseOutput` for fastest debug build
2. **Inspect output**: Check the generated files in the `_site/` directory
3. **View HTML directly**: Open HTML files from `_site/` in a browser or text editor
4. **See plugin output directly**

   ```powershell
   # Check generated data files
   Get-Content "_data/sections.json" | ConvertFrom-Json | ConvertTo-Json -Depth 10
   Get-Content "_data/copilot_plans.json" | ConvertFrom-Json | ConvertTo-Json -Depth 10
   ```

5. **Advantages**:
   - Faster than running a full server
   - See exact generated HTML output
   - No server startup time for quick debugging
   - Useful for inspecting Liquid template output

**Fastest Debug Build Combination:**

For the fastest possible debug build when iterating on code changes, use all parameters together:

```powershell
pwsh /workspaces/techhub/jekyll-start.ps1 -SkipStop -SkipClean -BuildInsteadOfServe -VerboseOutput
```

This combination:

- Skips stopping existing processes (-SkipStop)
- Skips cache cleaning (-SkipClean)
- Only builds without serving (-BuildInsteadOfServe)
- Provides verbose output for debugging (-VerboseOutput)
- Provides the fastest feedback loop for debugging Jekyll issues

## Custom Plugins and Extensions

The site uses custom Jekyll plugins to extend functionality and keep Liquid templates simple and fast. For complete plugin documentation, implementation details, and development guidelines, see [Plugins](plugins.md).

### Jekyll Development Philosophy

**Core Principle**: Keep Liquid templates focused on rendering, move logic to plugins and filters.

**Preferred Architecture Order**:

1. **Plugins**: Complex data processing, page generation, data aggregation
2. **Filters**: Data transformation and formatting operations  
3. **Liquid Templates**: Simple rendering logic only

**Benefits**:

- **Performance**: Ruby plugins are faster than complex Liquid logic
- **Maintainability**: Centralized logic in dedicated plugin files
- **Testability**: Plugins can be unit tested independently
- **Readability**: Templates remain clean and focused on presentation

### Plugin Overview

```text
_plugins/
├── date_filters.rb           # Custom date handling filters
├── date_utils.rb            # Shared date utility functions
├── jekyll_file_writer.rb    # Safe file writing utilities
├── section_pages_generator.rb # Auto-generate section pages
├── string_filters.rb        # String processing and validation filters
├── tag_filters.rb           # Tag processing filters
└── youtube.rb              # YouTube embed functionality
```

### Development Approach Examples

#### Example: Preferred Development Approach

##### Logic in Plugins/Filters (Recommended)

```liquid
{%- comment -%} Simple filter usage {%- endcomment -%}
{%- assign limited_posts = posts | limit_with_same_day -%}
{%- assign post_epoch = post.date | date_to_epoch -%}
```

##### Complex Logic in Liquid (Avoid)

```liquid
{%- comment -%} Complex date calculations in Liquid (slow and hard to maintain) {%- endcomment -%}
{%- assign current_year = site.time | date: "%Y" -%}
{%- assign post_year = post.date | date: "%Y" -%}
{%- if current_year == post_year -%}
  {%- comment -%} Multiple date comparisons and calculations... {%- endcomment -%}
{%- endif -%}
```

### Development Considerations

- **Restart Requirement**: Jekyll must be restarted after plugin modifications
- **Plugin Priority**: Plugins execute in priority-based order (see [Plugins](plugins.md))
- **Data Generation**: Tag-related changes require full restart for data file regeneration
- **Filter Performance**: Custom filters should be optimized for repeated use

## Liquid Template Development

### Core Principles

**Keep Templates Simple**: Liquid templates should focus on rendering, not complex logic.

**Use Available Filters**: The site provides custom filters for common operations (detailed in [Plugins](plugins.md)):

- **Date Operations**: `date_to_epoch`, `now_epoch`, `to_epoch`
- **Content Management**: `limit_with_same_day` for performance optimization
- **Tag Processing**: Custom tag normalization and processing filters

### Essential Liquid Patterns

#### Configuration-Driven Development

**Always use dynamic, configuration-driven approaches**:

```liquid
<!-- ✅ CORRECT: Dynamic sections -->
{% for section in site.data.sections %}
  {% assign section_data = section[1] %}
  {% assign category = section_data.category %}
{% endfor %}

<!-- ❌ WRONG: Hardcoded sections -->
{% if page.categories contains "AI" %}
```

**Key Principle**: Never hardcode sections or categories. Always use `site.data.sections` for section data.

#### Jekyll Data Access Patterns

**Correct data access patterns**:

```liquid
<!-- ✅ CORRECT patterns -->
{{ site.data.sections }}
{{ site.data.all_tags }}
{{ site.data.category_tags }}

<!-- ❌ WRONG patterns -->
{{ site.sections }}
{{ site.all_tags }}
{{ site.category_tags }}
```

**Remember**: The `site.data.` prefix is mandatory for accessing Jekyll data files.

#### Date and Timezone Handling

**Critical Timezone Requirements**:

- All date processing must use `Europe/Brussels` timezone consistently
- Use custom filters for date operations instead of complex Liquid logic

For comprehensive date and timezone processing guidelines, see [Date and Timezone Processing](datetime-processing.md).

**Standard Date Conversion Pattern**:

```liquid
{%- assign post_epoch = post.date | date_to_epoch -%}
{%- comment -%} now_epoch is available globally via Jekyll hooks -%}
{%- if post_epoch >= now_epoch -%}
  <!-- Future or current post -->
{%- endif -%}
```

#### Data Access Patterns

**Site Configuration**:

```liquid
{%- comment -%} Access site structure {%- endcomment -%}
{%- for section in site.data.sections -%}
  {%- assign section_data = section[1] -%}
  {{ section_data.title }}
{%- endfor -%}
```

**Tag Data Access**:

```liquid
{%- comment -%} Access tag data {%- endcomment -%}
{%- assign tags = site.data.category_tags[collection_type][category] -%}
```

#### Include Data Passing

When data is explicitly passed to included files:

```liquid
{%- include posts.html posts=limited_posts -%}
{%- include filters.html posts=posts collection_type=page.collection -%}
```

Access in includes using the `include.` prefix: `include.posts`, `include.collection_type`

### Template Processing Architecture

**Purpose**: Jekyll templates enable modular, reusable content generation that separates presentation from data processing. Templates allow the same layout and filtering logic to be applied across different content types and sections while maintaining consistency.

**Why Templates Are Essential**:

- **Modularity**: Break complex pages into manageable, reusable components
- **Consistency**: Ensure uniform presentation across all pages
- **Maintainability**: Update one template to change multiple pages
- **Dynamic Content**: Generate pages based on data and configuration
- **Performance**: Server-side rendering provides fast initial page loads

**Template Hierarchy Example**:

```liquid
<!-- _layouts/default.html - Base layout -->
<!DOCTYPE html>
<html>
<head>
  {% include head.html %}
</head>
<body>
  {% include header.html %}
  <main>
    {{ content }}
  </main>
  {% include footer.html %}
</body>
</html>

<!-- _includes/section-index.html - Page structure -->
{%- assign limited_posts = posts | limit_with_same_day -%}
{% include filters.html posts=limited_posts collection_type=page.collection %}
{% include posts.html posts=limited_posts %}

<!-- _includes/filters.html - Filter generation -->
{%- assign tags = site.data.category_tags[collection_type][category] -%}
{%- for tag in tags -%}
  <button class="tag-filter" data-tag="{{ tag.normalized }}">
    {{ tag.display }} ({{ tag.count }})
  </button>
{%- endfor -%}
```

**Template Data Flow**:

1. **Layout Templates**: Provide overall page structure and meta information
2. **Include Templates**: Handle specific functionality like filtering and content display
3. **Data Injection**: Templates receive data from Jekyll plugins and site configuration
4. **Dynamic Generation**: Templates adapt based on page type and content collection

**Note**: For complete tag flow and template processing details, see [Filtering System](filtering-system.md#tag-flow-pipeline).

### Available Global Variables

- **now_epoch**: Current timestamp for date comparisons
- **site.data.sections**: Complete site structure configuration
- **site.data.category_tags**: Aggregated tag data for filtering
- **site.data.all_tags**: Global tag usage across all content

## Data Files Management

### Site Configuration Data

#### _data/sections.json

- **Purpose**: Single source of truth for site structure
- **Content**: Section definitions, page configurations, navigation data
- **Usage**: Read by plugins and templates for dynamic generation

#### Existing Data Files

- **_data/sections.json**: Site structure and navigation configuration
- **_data/copilot_plans.json**: GitHub Copilot feature comparison data
- **Purpose**: Centralized configuration data for Jekyll templates and plugins
- **Usage**: Accessed via `site.data.filename` convention in Liquid templates

## Content Type Terminology

**CRITICAL**: For complete content type definitions and terminology, see [Content Management](content-management.md).

### Key Points for AI Models

- **`site.posts`**: Jekyll's default collection containing blogs only
- **"Post" in variables**: Usually refers to ANY collection item, not just blogs
- **Check context**: When you see "post" in code, verify what collection is actually being processed

## 🚨 Critical Rule: Never Hardcode Sections or Collections

**ABSOLUTE REQUIREMENT**: Never hardcode section names, collection names, or any configuration values in code.

**Always use dynamic configuration sources:**

- **Sections**: Always derive from `site.data.sections` configuration
- **Collections**: Always derive from Jekyll's collections configuration
- **Categories**: Always derive from the `category` field in `site.data.sections`
- **Content Types**: Always derive from Jekyll's collection definitions

### Examples of Correct vs Incorrect Patterns

#### Dynamic Configuration (Correct)

```liquid
{%- for section in site.data.sections -%}
  {%- assign section_data = section[1] -%}
  <a href="{{ section_data.url }}">{{ section_data.title }}</a>
{%- endfor -%}
```

```liquid
{%- assign collection_types = site.collections | map: 'label' -%}
{%- for collection_type in collection_types -%}
  {%- comment -%} Process collection dynamically {%- endcomment -%}
{%- endfor -%}
```

#### Hardcoded Values (Incorrect)

```liquid
<a href="/ai">AI</a>
<a href="/github-copilot">GitHub Copilot</a>
```

```liquid
{%- assign collection_types = 'news,videos,community,posts' | split: ',' -%}
```

**Why This Matters:**

- **Future-proofing**: Site configuration can change without code updates
- **Maintainability**: Single source of truth for all configuration
- **Flexibility**: Easy to add new sections or collections
- **Consistency**: All parts of the site stay synchronized automatically

## Jekyll Performance Optimization

### Build Performance

**Server-Side Optimizations**:

1. **Plugin-Based Processing**: Move complex logic to Ruby plugins for better performance
2. **Pre-calculated Data**: Use plugins to generate data files during build rather than runtime calculations
3. **Efficient Liquid Templates**: Keep templates simple and focused on rendering
4. **Content Limiting**: Apply performance rules like "20 + same-day" via custom filters

### Jekyll Plugin Integration

**Plugin Usage in Development**: For detailed plugin information, see [Plugins](plugins.md).

**Key Performance Principles**:

- **Logic in Plugins**: Complex operations should be handled by Jekyll plugins
- **Filters for Transformation**: Use custom filters for data formatting and transformation
- **Templates for Rendering**: Keep Liquid templates focused on presentation logic

### Build Process Integration

**Critical Integration Points**:

1. **Content Processing → Plugin Processing**: PowerShell creates standardized tags that Jekyll plugins can process
2. **Plugin Output → Template Rendering**: Generated data files feed Liquid template logic
3. **Server Rendering → Client Enhancement**: Server renders complete functional page that JavaScript can enhance

## Jekyll Debugging and Troubleshooting

### Common Jekyll Issues

#### Tag Data Synchronization Issues

**Symptoms**:

- Missing expected filters
- Jekyll restart needed for tag data synchronization
- Incorrect filter counts
- Ruby plugin errors during build

**Solutions**:

1. Restart Jekyll server
2. Check existing data files (`_data/sections.json`, `_data/copilot_plans.json`)
3. Verify data structure and format
4. Check Jekyll build logs for plugin errors

#### Server vs Client Discrepancies

**Symptoms**:

- Server-side and client-side filters show different counts
- Filter count discrepancies between server vs client calculations
- Posts appearing in wrong date ranges

**Solutions**:

- Check timezone handling between Jekyll and JavaScript
- Verify server-rendered HTML structure
- Move heavy processing to server-side (Liquid)
- Compare server vs client date calculations

### Debug Commands

**Check Data Files**:

```powershell
Get-Content "_data/sections.json" | ConvertFrom-Json
Get-Content "_data/copilot_plans.json" | ConvertFrom-Json
```

**Verbose Build for Debugging**:

```powershell
pwsh /workspaces/techhub/jekyll-start.ps1 -SkipStop -SkipClean -BuildInsteadOfServe -VerboseOutput
```

**Check Tag Enhancement**:

```powershell
Get-Content "_posts/example.md" | Select-String "tags:"
```

### Performance Debugging

**Common Performance Issues**:

- Too much client-side processing
- Large datasets without optimization
- Inefficient server-side calculations

**Solutions**:

1. Move processing to server-side (Liquid)
2. Implement proper caching
3. Use content limiting rules correctly (see [Filtering System](filtering-system.md))
4. Optimize Jekyll queries and data access

## Jekyll Documentation References

When working with Jekyll/Liquid, reference these official documentation sources:

- **Liquid Syntax**: [Jekyll Liquid Documentation](https://jekyllrb.com/docs/liquid/)
- **Includes**: [Jekyll Includes Documentation](https://jekyllrb.com/docs/includes/)
- **Front Matter**: [Jekyll Front Matter Documentation](https://jekyllrb.com/docs/front-matter/)
- **Variables**: [Jekyll Variables Documentation](https://jekyllrb.com/docs/variables/)
