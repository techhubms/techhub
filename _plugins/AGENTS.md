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

# Limit with same-day grouping
{{ site.posts | limit_with_same_day: 10 }}
```

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

## Testing Standards

### Framework

Use **RSpec** for all Jekyll plugin testing.

### Test File Structure

```text
spec/
└── _plugins/
    └── [plugin_name]_spec.rb
```

### Test Pattern

```ruby
require 'spec_helper'

RSpec.describe Jekyll::YourFilterName do
  include Jekyll::YourFilterName
  
  describe '#filter_method' do
    context 'with valid input' do
      it 'processes input correctly' do
        result = filter_method('test-input')
        expect(result).to eq('expected-output')
      end
    end
    
    context 'with invalid input' do
      it 'handles errors gracefully' do
        expect { filter_method(nil) }.to raise_error(ArgumentError)
      end
    end
    
    context 'edge cases' do
      it 'handles empty strings' do
        result = filter_method('')
        expect(result).to eq('')
      end
    end
  end
end
```

### Critical Testing Rules

**CRITICAL**: Test real plugin implementation, never duplicate logic
**CRITICAL**: Mock Jekyll site objects and external dependencies only
**CRITICAL**: Test actual Liquid template rendering when relevant
**CRITICAL**: Verify plugin behavior in Jekyll build lifecycle

## Date Handling

### Timezone Configuration

**Critical**: All dates use `Europe/Brussels` timezone (CET/CEST).

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

### Date Filter Usage

```liquid
<!-- Convert post date to epoch -->
{{ post.date | to_epoch }}

<!-- Sort posts by date -->
{{ site.posts | sort_by_date }}

<!-- Filter to items with dates in last 7 days -->
{{ site.posts | limit_with_same_day: 10 }}
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

## Resources

- [plugins.md](../docs/plugins.md) - Complete plugin documentation
- [datetime-processing.md](../docs/datetime-processing.md) - Date handling details
- [jekyll-development.md](../docs/jekyll-development.md) - Jekyll patterns
- [testing-guidelines.md](../docs/testing-guidelines.md) - Testing strategy

## Never Do

- Never modify files directly in `_site/` (Jekyll overwrites on build)
- Never use timezone-naive date comparisons
- Never duplicate production logic in test files
- Never create plugins that run on every page render (use generators instead)
- Never ignore Jekyll lifecycle hooks
- Never write files outside Jekyll's build process
