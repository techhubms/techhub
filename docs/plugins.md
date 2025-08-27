# Plugin Development

## Overview

The Tech Hub uses custom Jekyll plugins to extend functionality beyond standard Jekyll capabilities. These plugins handle content processing, data generation, and site structure automation.

## Plugin Architecture

### Plugin Types

- **Generators**: Create new pages and data files during build (run before template processing)
- **Filters**: Process data within Liquid templates (run during template rendering)
- **Hooks**: Execute code at specific Jekyll lifecycle points (triggered by Jekyll events)
- **Tags**: Add custom Liquid tags for special functionality (run during template processing)

### Plugin Directory Structure

```text
_plugins/
├── date_filters.rb           # Custom date processing filters
├── date_utils.rb            # Shared date utility functions
├── jekyll_file_writer.rb    # Safe file writing utilities
├── section_pages_generator.rb # Section and page generation
├── string_filters.rb        # String processing and validation filters
├── tag_filters.rb           # Tag processing and filtering system
└── youtube.rb              # YouTube embed functionality
```

## Generator Plugins

### section_pages_generator.rb

Automatically generates section index pages and collection pages based on configuration.

**Input**: `_data/sections.json` configuration file  
**Process**: Creates Jekyll pages dynamically using `PageWithoutAFile`  
**Output**: In-memory page objects added to `site.pages` collection

**Key Features**:

- **Highest Priority**: Runs first before all other plugins that depend on generated pages
- **Configuration-Driven**: Reads from `_data/sections.json`
- **Dynamic Page Creation**: Uses Jekyll's `PageWithoutAFile` for runtime page generation
- **Error Handling**: Comprehensive JSON parsing and file existence validation
- **Template Generation**: Automatically generates Liquid template content for pages

**Generated Pages**:

- **Section Index Pages**: `{section}/index.md` with collections grid and latest content
- **Collection Pages**: `{section}/{collection}.html` for individual content types
- **Auto-Generated Content**: Complete Liquid template code for consistent page structure

## Filter Plugins

### Date Processing (date_filters.rb)

Provides custom Liquid filters for date operations and content limiting, using shared utilities from `date_utils.rb`.

#### Core Date Filters

**to_epoch** - Converts any date to Unix epoch timestamp:

```liquid
{{ post.date | to_epoch }}
```

**date_to_epoch** - Converts date to epoch using YYYY-MM-DD format with Europe/Brussels timezone:

```liquid
{{ post.date | date_to_epoch }}
```

**now_epoch** - Gets current timestamp (ignores input):

```liquid
{{ '' | now_epoch }}
```

**normalize_date_format** - Handles timezone format issues in date strings:

```liquid
{{ raw_date | normalize_date_format }}
```

#### Collection Management Filters

**with_dates** - Filters array to only items with valid dates:

```liquid
{{ site.documents | with_dates }}
```

**sort_by_date** - Sorts items by date (newest first by default):

```liquid
{{ site.posts | sort_by_date }}
{{ site.posts | sort_by_date: 'asc' }}
```

**limit_with_same_day** - Applies server-side content limiting with collection-aware limiting rule plus 7-day recency filter:

- **7-day recency filter**: Excludes all items older than 7 days from current date
- **Collection grouping**: Groups remaining items by collection first
- **Configurable per-collection limiting**: Applies "N + same-day" rule to each collection independently (default N=20)
- **Fair representation**: Ensures fair representation across all collections
- **Sorted results**: Returns merged results sorted by date (newest first)

```liquid
{{ posts | limit_with_same_day }}           <!-- Uses default limit of 20 per collection -->
{{ posts | limit_with_same_day: 5 }}        <!-- Uses custom limit of 5 per collection -->
{{ posts | limit_with_same_day: 10 }}       <!-- Uses custom limit of 10 per collection -->
```

This prevents scenarios where one collection with many recent items would dominate the result set, while ensuring only recent content (within 7 days) is included.

#### Implementation Example

```ruby
module Jekyll
  module DateFilters
    def date_to_epoch(date)
      DateUtils.date_to_epoch(date)
    end
    
    def now_epoch(input)
      DateUtils.now_epoch()
    end
    
    def to_epoch(input)
      DateUtils.to_epoch(input)
    end
    
    def limit_with_same_day(posts, limit = 20)
      return [] if posts.nil? || posts.empty?
      
      # Apply 7-day recency filter and collection-aware limiting
      # Groups by collection and applies "N + same-day" rule per collection
      # Returns merged results sorted by date (newest first)
    end
  end
end

Liquid::Template.register_filter(Jekyll::DateFilters)
```

### Tag Processing (tag_filters.rb)

Provides comprehensive tag processing, filtering, and optimization functions for the unified tag-based filtering system.

**Input**: Content items with tags and normalized tags  
**Process**: Builds tag relationships, generates filter data, optimizes for performance with caching and parallel processing  
**Output**: Complete filter system data for client-side processing

**Key Features**:

- **Performance Caching**: Multiple cache layers for string operations, normalization, and date calculations
- **Parallel Processing**: Automatic parallel processing for large datasets using multi-core systems
- **Binary Search Optimization**: O(log n) date filtering instead of O(n) for improved performance
- **Memory Optimization**: Efficient data structures using Sets and optimized algorithms

**Main Filter**:

- **`generate_all_filters`**: Central function that generates complete filter data for all filtering modes (sections, collections, tags)

**Implementation Highlights**:

```ruby
module Jekyll
  module TagFilters
    # Performance caches
    @@date_filter_cache = {}
    @@string_split_cache = {}
    @@normalize_cache = {}
    @@word_index_cache = {}
    
    # Pre-compiled regex patterns for performance
    SPECIAL_CHARS_REGEX = /[^a-z0-9\s\-]/i.freeze
    WHITESPACE_REGEX = /\s+/.freeze
    
    def generate_all_filters(items, index_tag_mode, sections, section, category, collection, tag_filter_config)
      # Comprehensive filter generation with parallel processing
      # Pre-calculates tag relationships for ultra-fast client-side filtering
      # Returns complete filter data structure for JavaScript processing
    end
  end
end

Liquid::Template.register_filter(Jekyll::TagFilters)
```

### String Processing (string_filters.rb)

Provides string validation and regex matching filters.

**regex_match** - Checks if a string matches a regex pattern:

```liquid
{{ string | regex_match: pattern }}
```

Returns `true` if the string matches the pattern, `false` otherwise.

**is_letters_and_hyphen_only** - Validates that a string contains only letters and hyphens (for section name validation):

```liquid
{{ section_name | is_letters_and_hyphen_only }}
```

Returns `true` if valid, `false` otherwise.

**Implementation**:

```ruby
module Jekyll
  module StringFilters
    def regex_match(string, pattern)
      return false unless string && pattern
      
      begin
        regex = Regexp.new(pattern)
        !regex.match(string.to_s).nil?
      rescue RegexpError => e
        Jekyll.logger.warn "Invalid regex pattern '#{pattern}': #{e.message}"
        false
      end
    end
    
    def is_letters_and_hyphen_only(string)
      return false unless string.is_a?(String) && !string.empty?
      
      pattern = /^[a-zA-Z-]+$/
      !pattern.match(string).nil?
    end
  end
end

Liquid::Template.register_filter(Jekyll::StringFilters)
```

## Tag Plugins

### YouTube Embedding (youtube.rb)

Creates custom YouTube embed tag for responsive video embedding.

**Usage**:

```liquid
{% youtube VIDEO_ID %}
```

**Output**:

```html
<iframe class="youtube"
        src="https://www.youtube.com/embed/VIDEO_ID"
        loading="lazy"
        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
        allowfullscreen>
</iframe>
```

**Implementation**:

```ruby
class YouTube < Liquid::Tag
  Syntax = /^\s*([a-zA-Z0-9_-]+)(?:\s+.*)?$/

  def initialize(tagName, markup, tokens)
    super
    if markup.nil? || markup.strip.empty?
      raise "No YouTube ID provided in the \"youtube\" tag"
    end
    if markup.strip =~ Syntax then
      @id = $1
    else
      raise "No YouTube ID provided in the \"youtube\" tag"
    end
  end

  def render(context)
    "<iframe class=\"youtube\" src=\"https://www.youtube.com/embed/#{@id}\" loading=\"lazy\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>"
  end

  Liquid::Template.register_tag "youtube", self
end
```

## Utility Plugins

### Date Utilities (date_utils.rb)

Shared date utility functions used across multiple plugins for consistent date handling.

**Primary Features**:

- **Timezone Handling**: Consistent Europe/Brussels timezone calculations
- **Date Normalization**: Handles various date formats and timezone abbreviations
- **Error Handling**: Robust validation and fallback for invalid dates
- **Performance**: Optimized for repeated date calculations

**Core Functions**:

**now_epoch** - Gets current timestamp as epoch in Europe/Brussels timezone:

```ruby
def self.now_epoch
  current_time = Time.now
  brussels_offset = current_time.dst? ? '+02:00' : '+01:00'
  utc_now = current_time.utc
  brussels_time = utc_now.getlocal(brussels_offset)
  brussels_date_str = brussels_time.strftime('%Y-%m-%d')
  brussels_midnight = Time.parse("#{brussels_date_str} 00:00:00 #{brussels_offset}")
  brussels_midnight.to_i
end
```

**date_to_epoch** - Converts dates to epoch timestamps consistently using YYYY-MM-DD format:

```ruby
def self.date_to_epoch(input)
  return 0 if input.nil?
  
  time = to_time(input)
  return 0 if time == Time.at(0)
  
  formatted_date = time.strftime('%Y-%m-%d')
  current_time = Time.now
  brussels_offset = current_time.dst? ? '+02:00' : '+01:00'
  brussels_midnight = Time.parse("#{formatted_date} 00:00:00 #{brussels_offset}")
  brussels_midnight.to_i
end
```

**normalize_date_format** - Handles timezone format issues and abbreviations:

```ruby
def self.normalize_date_format(date_str)
  return '' if date_str.nil?
  
  normalized = date_str.to_s.strip
  
  # Convert +0000 to +00:00 format
  normalized = normalized.gsub(/([+-])(\d{2})(\d{2})$/, '\1\2:\3')
  
  # Handle timezone abbreviations
  if normalized.include?('CEST')
    normalized = normalized.gsub(/\s+CEST$/, ' +02:00')
  elsif normalized.include?('CET')
    normalized = normalized.gsub(/\s+CET$/, ' +01:00')
  end
  
  normalized
end
```

### File Operations (jekyll_file_writer.rb)

Generic utility for safe file writing with directory creation and error handling.

**write_file_with_dir** - Safely writes files with automatic directory creation:

```ruby
module JekyllFileWriter
  def self.write_file_with_dir(file_path, content, logger_tag)
    dir_path = File.dirname(file_path)
    begin
      FileUtils.mkdir_p(dir_path) unless Dir.exist?(dir_path)
    rescue => e
      Jekyll.logger.error logger_tag, "Error creating directory #{dir_path}: #{e.message}"
      return
    end
    
    begin
      if !File.exist?(file_path) || File.read(file_path) != content
        File.write(file_path, content)
        Jekyll.logger.info logger_tag, "Updated #{file_path}"
      end
    rescue => e
      Jekyll.logger.error logger_tag, "Error writing #{file_path}: #{e.message}"
    end
  end
end
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

## Plugin Development Guidelines

### Architecture Principles

#### Single Responsibility

Each plugin should have a clear, focused purpose:

- **Generators**: Create content or data files
- **Filters**: Process data within templates
- **Hooks**: Execute code at specific lifecycle points
- **Tags**: Add custom Liquid functionality

#### Performance Considerations

- **Caching**: Use module-level caches for expensive operations
- **Lazy Loading**: Load data only when needed
- **Efficient Algorithms**: Use optimized data structures and algorithms
- **Memory Management**: Clean up large objects when possible

#### Error Handling

```ruby
begin
  # Plugin operation
rescue StandardError => e
  Jekyll.logger.error "PluginName:", "Error: #{e.message}"
  # Provide fallback or fail gracefully
end
```

### Development Best Practices

#### Plugin Structure

```ruby
module Jekyll
  module PluginName
    # Constants at the top
    CONSTANT_VALUE = 'value'.freeze
    
    # Module-level caches
    @@cache = {}
    
    # Public methods
    def public_method(input)
      # Implementation
    end
    
    private
    
    # Private helper methods
    def helper_method
      # Implementation
    end
  end
end

Liquid::Template.register_filter(Jekyll::PluginName)
```

#### Testing

- **Unit Tests**: Test individual plugin functions
- **Integration Tests**: Test plugin interaction with Jekyll
- **Performance Tests**: Measure build time impact
- **Error Handling Tests**: Verify graceful failure modes

#### Documentation

- **Clear Purpose**: Explain what the plugin does
- **Usage Examples**: Show how to use in templates
- **Configuration Options**: Document available settings
- **Performance Notes**: Mention any performance considerations

### Jekyll Version Compatibility

#### Current Support

- **Current**: Jekyll 4.x support
- **Backward Compatibility**: Test with Jekyll 3.x
- **Future Proofing**: Monitor Jekyll development for breaking changes

#### Ruby Version Support

- **Current**: Ruby 2.7+ support
- **Testing**: Test with multiple Ruby versions
- **Dependencies**: Keep gem dependencies current

### Performance Monitoring

#### Build Time Tracking

```ruby
start_time = Time.now
perform_operation()
duration = Time.now - start_time
Jekyll.logger.info "Operation completed in #{duration}s"
```

#### Memory Usage

```ruby
require 'get_process_mem'

before_memory = GetProcessMem.new.mb
perform_operation()
after_memory = GetProcessMem.new.mb
Jekyll.logger.info "Memory used: #{after_memory - before_memory}MB"
```

This comprehensive plugin system enables the Tech Hub to provide advanced functionality while maintaining performance and reliability.
