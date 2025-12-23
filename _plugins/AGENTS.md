# Jekyll Plugin Development Agent

## Overview

You are a Ruby specialist focused on Jekyll plugin development for the Tech Hub. These plugins extend Jekyll's functionality with custom generators, filters, and tags for content processing and site generation.

## Tech Stack

- **Ruby**: 3.2+
- **Jekyll**: 4.3+
- **Testing Framework**: RSpec
- **Key Dependencies**: Time zone libraries, HTML/XML processors

## Plugin Types

### Generators

Create new pages and data files during Jekyll build (run before template processing).

Example: `section_pages_generator.rb` - Creates section index and collection pages dynamically.

### Filters

Process data within Liquid templates (run during template rendering).

Example: `date_filters.rb` - Custom date formatting and epoch conversions.

### Tags

Add custom Liquid tags for special functionality (run during template processing).

Example: `youtube.rb` - Embed YouTube videos with responsive wrappers.

## Plugin Files

```text
_plugins/
├── date_filters.rb             # Date processing filters
├── date_utils.rb               # Shared date utility functions
├── jekyll_file_writer.rb       # Safe file writing utilities
├── section_pages_generator.rb  # Dynamic page generation
├── string_filters.rb           # String processing filters
├── tag_filters.rb              # Tag processing and filtering
└── youtube.rb                  # YouTube embed tag
```

## Key Plugins

### section_pages_generator.rb

**Purpose**: Automatically generates section index pages and collection pages.

**Priority**: Runs FIRST before all other plugins (highest priority).

**Input**: `_data/sections.json` configuration file

**Output**: In-memory Jekyll pages (no files written to disk)

**Key Features**:
- Configuration-driven page generation
- Uses `Jekyll::PageWithoutAFile` for runtime page creation
- Comprehensive error handling
- Automatic Liquid template generation

### date_filters.rb

**Purpose**: Custom date operations for Liquid templates.

**Key Filters**:

```ruby
# Convert to Unix epoch timestamp
{{ post.date | to_epoch }}

# Convert with Europe/Brussels timezone
{{ post.date | date_to_epoch }}

# Get current timestamp
{{ '' | now_epoch }}

# Filter items with valid dates
{{ site.documents | with_dates }}

# Sort by date (newest first)
{{ site.posts | sort_by_date }}

# Limit with same-day grouping (collection-aware)
{{ site.posts | limit_with_same_day: 10 }}
```

**limit_with_same_day Filter**: Applies server-side content limiting with collection-aware rule plus 7-day recency filter:

- **7-day recency filter**: Excludes all items older than 7 days from current date
- **Collection grouping**: Groups remaining items by collection first
- **Configurable per-collection limiting**: Applies "N + same-day" rule to each collection independently (default N=20)
- **Fair representation**: Ensures fair representation across all collections
- **Sorted results**: Returns merged results sorted by date (newest first)

**Dependency**: Uses `date_utils.rb` for shared functionality.

### tag_filters.rb

**Purpose**: Tag processing and filtering system.

**Key Filter**:

```ruby
# Extract and normalize tags from frontmatter
{{ post.tags | extract_tags }}
```

**Features**:
- Tag normalization (lowercase, hyphenated)
- Duplicate removal
- Validation and sanitization

### string_filters.rb

**Purpose**: String processing and validation.

**Key Filters**:

```ruby
# URL validation
{{ url_string | is_valid_url }}

# Safe string output
{{ content | safe_string }}
```

### youtube.rb

**Purpose**: Embed YouTube videos responsively.

**Usage in Liquid**:

```liquid
{% youtube VIDEO_ID %}
```

**Output**: Responsive iframe with 16:9 aspect ratio container.

## Ruby Development Standards

### Module Structure

```ruby
module Jekyll
  module YourFilterName
    def filter_method(input, param = nil)
      # Implementation
      return processed_output
    end
  end
end

Liquid::Template.register_filter(Jekyll::YourFilterName)
```

### Generator Structure

```ruby
module Jekyll
  class YourGenerator < Generator
    priority :highest # or :high, :normal, :low, :lowest
    
    def generate(site)
      # Access site data
      # Create pages
      # Modify site collections
    end
  end
end
```

### Tag Structure

```ruby
module Jekyll
  class YourTag < Liquid::Tag
    def initialize(tag_name, text, tokens)
      super
      @param = text.strip
    end

    def render(context)
      # Generate HTML output
    end
  end
end

Liquid::Template.register_tag('your_tag', Jekyll::YourTag)
```

## Liquid Template Development

### Core Principles

**Keep Templates Simple**: Liquid templates should focus on rendering, not complex logic.

**Preferred Architecture Order**:
1. **Plugins**: Complex data processing, page generation, data aggregation
2. **Filters**: Data transformation and formatting operations  
3. **Liquid Templates**: Simple rendering logic only

**Benefits**:
- **Performance**: Ruby plugins are faster than complex Liquid logic
- **Maintainability**: Centralized logic in dedicated plugin files
- **Testability**: Plugins can be unit tested independently
- **Readability**: Templates remain clean and focused on presentation

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

#### Jekyll Data Access

**⚠️ CRITICAL JEKYLL CONVENTION**: All data files in `_data` directory are accessed via `site.data.filename` (without the `.json` extension).

```liquid
<!-- ✅ CORRECT patterns -->
{{ site.data.sections }}
{{ site.data.all_tags }}
{{ site.data.category_tags }}

<!-- ❌ WRONG patterns -->
{{ site.sections }}
{{ site.all_tags }}
```

#### Include Data Passing

When data is explicitly passed to included files:

```liquid
{%- include posts.html posts=limited_posts -%}
{%- include filters.html posts=posts collection_type=page.collection -%}
```

Access in includes using the `include.` prefix: `include.posts`, `include.collection_type`

### Formatting Requirements

- Add proper indentation wherever possible
- Never place conditions and actions on the same line

```liquid
{%- comment -%} ❌ BAD {%- endcomment -%}
{% if true %} then dosomething {% else %} bla {% endif %}

{%- comment -%} ✅ GOOD {%- endcomment -%}
{%- if condition -%}
  {%- assign result = value -%}
{%- else -%}
  {%- assign result = alternative -%}
{%- endif -%}
```

## Date Handling

### Timezone Configuration

**Critical**: All dates use `Europe/Brussels` timezone (CET/CEST) as configured in `_config.yml`.

**Date Utils Module** (`date_utils.rb`):

```ruby
module Jekyll
  module DateUtils
    TIMEZONE = 'Europe/Brussels'
    
    def normalize_time_for_epoch(time_input)
      # Handles multiple date formats
      # Returns Unix epoch timestamp
    end
  end
end
```

### Timezone Consistency in Plugins

All Ruby plugins must use consistent date processing:

```ruby
# Correct: Use DateUtils for consistent processing
current_time = DateUtils.now_epoch()

# Correct: Parse dates using DateUtils methods
parsed_epoch = DateUtils.date_to_epoch(date_string)

# Correct: Normalize to midnight Brussels time
midnight_epoch = DateUtils.normalize_to_midnight(date_input)
```

### Testing Date Functions

Test date filters with timezone awareness:

```ruby
describe DateUtils do
  before do
    Time.zone = 'Europe/Brussels'
  end
  
  it 'converts dates to epoch correctly' do
    date = '2025-01-01'
    expected = Time.zone.parse(date).to_i
    expect(DateUtils.to_epoch(date)).to eq(expected)
  end
end
```

## Testing Standards

Use **RSpec** for all Jekyll plugin testing. For complete testing patterns, test organization, and critical testing rules, see [spec/AGENTS.md](../spec/AGENTS.md).

### Test File Location

```text
spec/_plugins/
└── [plugin_name]_spec.rb
```

### Running RSpec Tests

```bash
# All plugin tests
./scripts/run-plugin-tests.ps1

# Specific file
bundle exec rspec spec/_plugins/date_filters_spec.rb
```

## Page Generation

### Dynamic Page Creation

```ruby
def create_page(site, filename, content, data = {})
  page = Jekyll::PageWithoutAFile.new(site, __dir__, '', filename)
  page.content = content
  page.data.merge!(data)
  site.pages << page
  page
end
```

### Section Index Page Generation

Uses configuration from `_data/sections.json`:

```json
{
  "sections": [
    {
      "title": "Section Name",
      "path": "section-path",
      "collections": ["collection1", "collection2"]
    }
  ]
}
```

## Best Practices

### Error Handling

```ruby
def safe_operation(input)
  return default_value if input.nil? || input.empty?
  
  begin
    # Process input
    result = process(input)
  rescue => e
    Jekyll.logger.warn "Operation failed: #{e.message}"
    return fallback_value
  end
  
  result
end
```

### Performance

- Cache expensive operations
- Minimize site.documents iteration
- Use efficient array operations
- Avoid N+1 queries in filters

### Logging

```ruby
Jekyll.logger.info "Processing: #{item_name}"
Jekyll.logger.warn "Unexpected condition: #{details}"
Jekyll.logger.error "Failed: #{error_message}"
```

## Common Patterns

### Accessing Site Data

```ruby
def generate(site)
  # Get data from _data/ files
  sections_data = site.data['sections']
  
  # Access collections
  posts = site.collections['posts']
  
  # Get all documents
  all_docs = site.documents
end
```

### Creating Pages

```ruby
page = Jekyll::PageWithoutAFile.new(site, __dir__, '', 'index.md')
page.content = "# Page Content\n\n{{ content }}"
page.data['layout'] = 'default'
page.data['title'] = 'Page Title'
site.pages << page
```

### Processing Frontmatter

```ruby
def process_document(doc)
  tags = doc.data['tags'] || []
  date = doc.data['date']
  title = doc.data['title']
  
  # Process data
end
```

## Testing Plugins

```bash
# Run all plugin tests
./scripts/run-plugin-tests.ps1

# Run specific test file
bundle exec rspec spec/_plugins/date_filters_spec.rb

# Run with coverage
bundle exec rspec --format documentation
```

## Complete Filter Reference

### Date Filters (from date_filters.rb)

| Filter | Purpose | Usage |
|--------|---------|-------|
| `to_epoch` | Convert date to Unix timestamp | `{{ post.date \| to_epoch }}` |
| `date_to_epoch` | Convert date to epoch (Brussels timezone) | `{{ post.date \| date_to_epoch }}` |
| `now_epoch` | Get current timestamp | `{{ '' \| now_epoch }}` |
| `normalize_date_format` | Fix timezone format issues | `{{ raw_date \| normalize_date_format }}` |
| `normalize_to_midnight` | Normalize time to midnight | `{{ item.date \| normalize_to_midnight }}` |
| `with_dates` | Filter items with valid dates | `{{ site.documents \| with_dates }}` |
| `sort_by_date` | Sort items by date | `{{ site.posts \| sort_by_date }}` |
| `limit_with_same_day` | Apply "N + same-day" rule per collection | `{{ posts \| limit_with_same_day }}` |

### String Filters (from string_filters.rb)

| Filter | Purpose | Usage |
|--------|---------|-------|
| `regex_match` | Check string against regex | `{{ string \| regex_match: pattern }}` |
| `is_letters_and_hyphen_only` | Validate section names | `{{ section \| is_letters_and_hyphen_only }}` |

### Tag Filters (from tag_filters.rb)

| Filter | Purpose | Usage |
|--------|---------|-------|
| `generate_all_filters` | Generate unified filter data | `{{ items \| generate_all_filters: mode, sections, ... }}` |

## Resources

- [datetime-processing.md](../docs/datetime-processing.md) - Date handling details
- [jekyll-development.md](../docs/jekyll-development.md) - Jekyll operational patterns
- [spec/AGENTS.md](../spec/AGENTS.md) - Testing strategy and RSpec patterns

## Never Do

- Never modify files directly in `_site/` (Jekyll overwrites on build)
- Never use timezone-naive date comparisons
- Never duplicate production logic in test files
- Never create plugins that run on every page render (use generators instead)
- Never ignore Jekyll lifecycle hooks
- Never write files outside Jekyll's build process
