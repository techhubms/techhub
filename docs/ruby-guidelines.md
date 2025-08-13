# Ruby Development Guidelines

This document covers Ruby development standards for the Tech Hub, focusing on Jekyll plugin development and testing practices.

## Ruby in the Tech Hub Context

### Jekyll Plugin Development

Ruby is used primarily for Jekyll plugin development in the Tech Hub. The Ruby code handles:

- **Server-side Content Processing**: Jekyll plugins that generate data files and process content
- **Liquid Filter Extensions**: Custom filters for date formatting, content transformation, and data manipulation
- **Site Data Generation**: Creating JSON data files that power the client-side filtering system
- **Content Transformation**: Processing markdown files and frontmatter during Jekyll build

### Ruby Files Location

```text
_plugins/
├── date_filters.rb           # Date formatting and calculation filters
├── date_utils.rb            # Date utility functions and timezone handling
├── section_pages_generator.rb  # Generates section landing pages
└── [other_plugins].rb       # Additional Jekyll plugins
```

## Jekyll Plugin Standards

### Plugin Structure

**Use proper Jekyll plugin conventions**:

```ruby
# Standard Jekyll plugin structure

module Jekyll
  module DateFilters
    # Plugin methods here
  end
end

# Register the plugin with Liquid

Liquid::Template.register_filter(Jekyll::DateFilters)
```

### Error Handling

**Implement robust error handling for Jekyll plugins**:

```ruby
def to_epoch(date_string)
  return 0 if date_string.nil? || date_string.empty?
  
  begin
    DateUtils.to_epoch(date_string)
  rescue StandardError => e
    Jekyll.logger.warn "Date conversion failed for '#{date_string}': #{e.message}"
    0
  end
end
```

### Performance Considerations

**Optimize for Jekyll build performance**:

```ruby
# Cache expensive operations

def expensive_calculation(input)
  @cache ||= {}
  @cache[input] ||= perform_calculation(input)
end

# Use Jekyll's built-in utilities when possible

def site_timezone
  @site_timezone ||= site.config['timezone'] || 'UTC'
end
```

## Testing with RSpec

This section covers Ruby testing standards using RSpec for Phase 3 integration testing in the Tech Hub filtering system.

### Testing Framework

**Framework**: RSpec  
**Purpose**: Integration testing of Jekyll plugins, Liquid filters, and server-side data generation  
**Test Location**: `spec/plugins/`  
**Test Execution**: Use `/workspaces/techhub/run-plugin-tests.ps1` wrapper script

### Test Structure and Organization

```text
spec/plugins/
├── spec_helper.rb                      # RSpec configuration
├── date_filters_spec.rb                # Date filter tests
├── date_utils_spec.rb                  # Date utility tests
├── section_pages_generator_spec.rb     # Page generation tests
└── [plugin_name]_spec.rb              # Additional plugin tests
```

### Running Ruby Tests

```powershell
# Run all Ruby tests using wrapper script

pwsh /workspaces/techhub/run-plugin-tests.ps1

# Run specific test file

pwsh /workspaces/techhub/run-plugin-tests.ps1 -SpecFile "spec/plugins/date_filters_spec.rb"

# Run with verbose output for debugging

pwsh /workspaces/techhub/run-plugin-tests.ps1 -Verbose
```

### Test Coverage Requirements

**Ruby tests should cover**:

- **Plugin Public Methods**: All public methods exposed by Jekyll plugins
- **Liquid Filters**: All custom Liquid filters and their edge cases
- **Data Processing**: Logic that processes Jekyll data files or frontmatter
- **Utility Functions**: Shared utility functions used across plugins
- **Error Handling**: Invalid inputs and error conditions
- **Edge Cases**: Boundary conditions, empty inputs, malformed data

**Ruby tests should NOT cover**:

- Jekyll's internal functionality (trust the framework)
- File system operations (avoid testing actual I/O unless necessary)
- External gem behavior (focus on your plugin logic)
- Browser rendering (belongs in Playwright tests)
- Client-side logic (belongs in JavaScript tests)

### RSpec Configuration

**spec_helper.rb setup**:

```ruby
require 'rspec'

# Configure RSpec

RSpec.configure do |config|
  # Use expect syntax only (no should)
  config.expect_with :rspec do |expectations|
    expectations.syntax = :expect
  end

  # Use proper mocking
  config.mock_with :rspec do |mocks|
    mocks.verify_partial_doubles = true
  end

  # Run tests in random order
  config.order = :random
  Kernel.srand config.seed

  # Support for focused tests
  config.filter_run_when_matching :focus

  # Performance profiling
  config.profile_examples = 10 if ENV['PROFILE']
end

# Test helpers (if needed)

def create_test_site(config = {})
  # Helper method to create test Jekyll site
end

def create_test_post(frontmatter = {}, content = '')
  # Helper method to create test Jekyll posts
end
```

### Test Writing Standards

**Use descriptive RSpec structure with proper context**:

```ruby
require 'spec_helper'

RSpec.describe Jekyll::DateFilters do
  # Create a test object that includes the module
  let(:filter_tester) do
    Class.new do
      include Jekyll::DateFilters
    end.new
  end

  describe '#to_epoch' do
    context 'with valid date strings' do
      it 'converts ISO date to epoch timestamp' do
        result = filter_tester.to_epoch('2024-01-01T00:00:00Z')
        expected = Time.parse('2024-01-01T00:00:00Z').to_i
        
        expect(result).to eq(expected)
      end
    end

    context 'with invalid inputs' do
      it 'returns 0 for nil input' do
        expect(filter_tester.to_epoch(nil)).to eq(0)
      end
    end

    context 'error handling' do
      it 'delegates to DateUtils.to_epoch' do
        expect(DateUtils).to receive(:to_epoch).with('2024-01-01')
        filter_tester.to_epoch('2024-01-01')
      end
    end
  end
end
```

### Testing Patterns

#### Mock Usage and Testing Doubles

**Use RSpec mocks appropriately**:

```ruby
# Good: Mock external dependencies

describe '#process_posts' do
  it 'calls date conversion for each post' do
    posts = [{ 'date' => '2024-01-01' }, { 'date' => '2024-01-02' }]
    
    allow(DateUtils).to receive(:to_epoch).and_return(1640995200)
    processor.process_posts(posts)
    
    expect(DateUtils).to have_received(:to_epoch).twice
  end
end

# Good: Use test doubles for complex objects

let(:mock_site) do
  site = double('Jekyll::Site')
  allow(site).to receive(:config).and_return(test_config)
  allow(site).to receive(:data).and_return(test_data)
  site
end

# Avoid: Over-mocking core logic - Don't mock the method you're testing
```

#### Jekyll Plugin Integration Testing

**Test plugins in Jekyll context when necessary**:

```ruby
RSpec.describe SectionPagesGenerator do
  let(:site) do
    site = double('Jekyll::Site')
    allow(site).to receive(:config).and_return({
      'collections' => { 'news' => { 'output' => true } },
      'timezone' => 'Europe/Brussels'
    })
    allow(site).to receive(:data).and_return({
      'sections' => { 'ai' => { 'title' => 'AI', 'collections' => ['news'] } }
    })
    site
  end

  describe '#generate' do
    it 'creates section pages for each defined section' do
      pages = []
      allow(site).to receive(:pages) { pages }
      
      generator.generate(site)
      
      expect(pages).not_to be_empty
      ai_page = pages.find { |p| p.data['section_key'] == 'ai' }
      expect(ai_page).not_to be_nil
    end
  end
end
```

#### Error Handling Tests

**Test error conditions and edge cases**:

```ruby
describe 'error handling' do
  context 'with invalid site configuration' do
    it 'handles missing timezone gracefully' do
      site_without_timezone = double('Jekyll::Site')
      allow(site_without_timezone).to receive(:config).and_return({})
      
      expect { generator.generate(site_without_timezone) }.not_to raise_error
    end
  end

  context 'with malformed data' do
    it 'skips invalid date formats' do
      posts_with_invalid_dates = [
        { 'date' => 'not-a-date', 'categories' => ['ai'] },
        { 'date' => '2024-01-01', 'categories' => ['ai'] }
      ]
      
      result = DateUtils.calculate_date_filters(posts_with_invalid_dates, 'ai')
      
      # Should process valid date and skip invalid one
      expect(result.map { |f| f['count'] }).to include(1)
    end
  end
end
```

### Assertion Best Practices

**Use specific RSpec matchers**:

```ruby
# Excellent: Specific assertions

expect(result).to be_an(Array)
expect(result).to have_attributes(count: 3, first: 'ai')
expect(result).to include(hash_including('label' => 'Last 7 days', 'count' => 5))

# Good: Content validation

expect(result['ai']['count']).to eq(10)
expect(result.keys).to contain_exactly('ai', 'devops', 'github copilot')

# Good: Error checking

expect { invalid_operation }.to raise_error(ArgumentError, /specific message/)
expect { valid_operation }.not_to raise_error

# Good: Mock verification

expect(DateUtils).to have_received(:to_epoch).with('2024-01-01')
expect(mock_logger).to have_received(:warn).once

# Avoid: Generic assertions

expect(result).to be_truthy  # Too vague
expect(result).not_to be_nil # Usually redundant
```

### Test Documentation Standards

**Write descriptive test names that explain behavior**:

```ruby
# Excellent: Test names explain exact behavior

it 'converts ISO 8601 date strings to Unix epoch timestamps' do
it 'calculates date filter counts excluding posts outside date range' do
it 'generates section landing pages with proper frontmatter and content' do

# Good: Clear expectation

it 'handles timezone conversion between UTC and Brussels time' do
it 'validates required section configuration before page generation' do

# Avoid: Vague or implementation-focused names

it 'works correctly' do
it 'calls the right method' do
it 'does not break' do
```

### Jekyll Integration Helper

**When testing requires Jekyll context**:

```ruby
# Helper method for Jekyll integration tests

def with_jekyll_site(config = {})
  default_config = {
    'source' => File.expand_path('fixtures', __dir__),
    'destination' => File.expand_path('tmp', __dir__),
    'timezone' => 'Europe/Brussels'
  }
  
  site = Jekyll::Site.new(Jekyll.configuration(default_config.merge(config)))
  site.reset
  site.read
  
  yield site if block_given?
  site
end

# Usage in tests

it 'integrates with Jekyll site generation' do
  with_jekyll_site do |site|
    generator.generate(site)
    
    expect(site.pages).not_to be_empty
    ai_page = site.pages.find { |p| p.data['section_key'] == 'ai' }
    expect(ai_page.content).to include('AI section content')
  end
end
```

## Best Practices Summary

### Development Guidelines

1. **Focus on Plugin Logic**: Test your plugin's behavior, not Jekyll's functionality
2. **Use Descriptive Tests**: Test names should explain the expected behavior clearly
3. **Mock External Dependencies**: Mock Jekyll site objects and external utilities appropriately
4. **Test Edge Cases**: Include tests for error conditions and boundary scenarios
5. **Keep Tests Fast**: Avoid unnecessary file I/O and heavy operations
6. **Test Data Transformation**: Focus on testing data processing and generation logic
7. **Validate Error Handling**: Ensure plugins handle invalid input gracefully

### Performance Guidelines

- **Cache expensive operations** using instance variables or class-level caches
- **Use Jekyll's built-in utilities** when possible instead of reimplementing
- **Optimize for build time** - plugins run during Jekyll build process
- **Test performance** with realistic data sizes

### Integration with CI/CD

**Ruby tests in the testing pipeline**:

1. **Medium Speed**: Ruby tests run after unit tests but before E2E tests
2. **Jekyll Integration**: Tests validate server-side data generation
3. **Dependency Management**: Tests ensure proper gem dependencies
4. **Build Validation**: Tests confirm plugins work with Jekyll build process

**CI execution example**:

```powershell
# CI pipeline step

pwsh /workspaces/techhub/run-plugin-tests.ps1
```

This testing approach ensures Jekyll plugins are thoroughly validated for data generation and content processing, providing confidence in the server-side components of the filtering system.
