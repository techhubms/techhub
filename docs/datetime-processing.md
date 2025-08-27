# Date and Timezone Processing

This document covers all datetime handling, timezone configuration, and date filtering implementation across the Tech Hub. For content limiting rules using date filters, see [Filtering System](filtering-system.md).

## Critical Timezone Configuration

**CRITICAL**: Jekyll runs in the Europe/Brussels timezone and all date calculations must be consistent with this setting.

### Timezone Requirements

- The site is configured to run in `Europe/Brussels` timezone  
- All date filters and calculations must respect this timezone
- Server-side (Liquid) and Ruby plugin date handling must be synchronized
- All dates are normalized to midnight Brussels time for epoch comparisons

### Jekyll Configuration

The timezone is configured in `_config.yml`:

```yaml
timezone: Europe/Brussels
```

This affects:

- Jekyll build process date handling
- All Liquid date filters
- Ruby plugin date calculations
- Content sorting and filtering

## Date Conversion Patterns

### Standard Date Conversion Pattern

Use this pattern consistently throughout the site:

```liquid
{%- assign post_epoch = post.date | date_to_epoch -%}
{%- comment -%} now_epoch is available globally, no assignment needed -%}
```

### Global Date Variables

- `now_epoch`: Current date as epoch timestamp, available globally on all pages via Jekyll hooks
- This variable is set during the Jekyll build process and available without assignment

## Custom Date Filters

The site includes custom Liquid filters for date-to-epoch conversion and content limiting:

### Core Date Filters

- `date_to_epoch`: Converts any date to Unix epoch timestamp (matches the YYYY-MM-DD → epoch pattern)
- `now_epoch`: Gets current date as epoch timestamp (ignores input, use any string like `''`)
- `to_epoch`: Direct date-to-epoch conversion without formatting  

### Content Filtering Filters

- `limit_with_same_day`: Applies content limiting rule to content arrays (see [Filtering System](filtering-system.md))

### Filter Implementation

These filters are defined in `_plugins/date_filters.rb` which uses the shared utility functions from `_plugins/date_utils.rb`.

## Usage Examples

### Basic Date Comparison

```liquid
{%- assign post_epoch = post.date | date_to_epoch -%}
{%- comment -%} now_epoch is available globally -%}
{%- if post_epoch >= now_epoch -%}
  <!-- Post is from today or future -->
{%- endif -%}
```

### Content Limiting with Date Boundaries

```liquid
{%- comment -%} Apply content limiting rule to content -%}
{%- assign sorted_items = site.posts | sort: 'date' | reverse -%}
{%- assign limited_items = sorted_items | limit_with_same_day -%}
```

### Date Filtering in Collections

```liquid
{%- assign recent_posts = site.posts | where_exp: "post", "post.date >= now_epoch" -%}
```

## Ruby Plugin Date Handling

### Timezone Consistency

All Ruby plugins must use consistent date processing for `Europe/Brussels` timezone:

```ruby
# Correct: Use DateUtils for consistent processing

current_time = DateUtils.now_epoch()

# Correct: Parse dates using DateUtils methods

parsed_epoch = DateUtils.date_to_epoch(date_string)

# Correct: Normalize to midnight Brussels time

midnight_epoch = DateUtils.normalize_to_midnight(date_input)
```

### Date Normalization

- Use `DateUtils.now_epoch()` for current time as epoch timestamp
- Use `DateUtils.date_to_epoch()` for consistent date-to-epoch conversion in Brussels timezone
- Always use `DateUtils` methods for timezone-aware date operations

### Plugin Implementation Examples

```ruby
module Jekyll
  module DateFilters
    def date_to_epoch(date_input)
      DateUtils.date_to_epoch(date_input)
    end
    
    def now_epoch(input = nil)
      DateUtils.now_epoch()
    end
    
    def to_epoch(input)
      DateUtils.to_epoch(input)
    end
  end
end
```

## Critical Rule: No Relative Dates in Static Sites

**🚨 CRITICAL**: Jekyll sites are built once and contain static HTML. Relative dates in data attributes used for filtering logic must NEVER be hardcoded.

### Static Site Date Processing Requirements

- **Convert to Epoch Immediately**: All dates must be converted to Unix epoch timestamps as soon as possible
- **Client-Side Calculations**: Use epoch timestamps for all relative date calculations in JavaScript
- **No Hardcoded Relative Dates in Data Attributes**: Never use text like "last 3 days" in data attributes for filtering logic
- **Dynamic Calculation**: Calculate relative dates at runtime using current epoch vs stored epoch

### Why This Matters

```html
<!-- ❌ WRONG: Static HTML with relative date strings in data attributes -->
<h1>Last 3 days</h1>
<ul>
  <li class="post-item" data-date="last 3 days">item title</li>
  <li class="post-item" data-date="last 5 days">item title</li>
  <li class="post-item" data-date="last 1 days">item title</li>
  <li class="post-item" data-date="last 3 week">item title</li>
</ul>
```

```javascript
// ❌ WRONG: JavaScript filtering by parsing text strings
// This breaks when site was built days ago
document.querySelectorAll('.post-item').forEach(item => {
  if (item.dataset.date === "last 3 days") {
    item.style.display = 'block';
  } else {
    item.style.display = 'none';
  }
});
```

```html
<!-- ✅ CORRECT: Epoch timestamps for dynamic calculation -->
<h1>Last 3 days</h1>
<ul>
  <li class="post-item" data-epoch="1721541345">item title</li>
  <li class="post-item" data-epoch="1721513455">item title</li>
  <li class="post-item" data-epoch="1721423423">item title</li>
  <li class="post-item" data-epoch="1721134614">item title</li>
</ul>
```

```javascript
// ✅ CORRECT: JavaScript filtering by epoch calculation
// This works regardless of when the site was built
const now = Math.floor(Date.now() / 1000);
const cutoffEpoch = now - (3 * 24 * 60 * 60); // 3 days ago

document.querySelectorAll('.post-item').forEach(item => {
  const postEpoch = parseInt(item.dataset.dateEpoch);
  if (postEpoch >= cutoffEpoch) {
    item.style.display = 'block';
  } else {
    item.style.display = 'none';
  }
});
```

### What Is Allowed vs Not Allowed

**✅ ALLOWED (Display Text)**:

- Button labels: `<button>Last 3 days</button>`
- Headings: `<h1>Last 3 days</h1>`
- Dynamic display text calculated at runtime: `element.textContent = '3 days ago'`

**❌ NOT ALLOWED (Data Attributes for Filtering)**:

- Text in data attributes: `data-date="last 3 days"`
- Text in data attributes: `data-filter="last month"`
- JavaScript parsing text strings for filtering logic

**✅ REQUIRED (Filtering Logic)**:

- Numeric data attributes: `data-days="3"` or `data-epoch="1721541345"`
- Epoch timestamp calculations in JavaScript
- Pure numeric comparisons for filtering

### Button Labels vs Filter Logic

**Key Principle**: Button labels can be human-readable, but filter logic must use numeric data attributes.

```html
<!-- ✅ CORRECT: Human-readable button with numeric data -->
<button data-days="3" class="date-filter-btn">Last 3 days</button>
<button data-days="30" class="date-filter-btn">Last 30 days</button>

<!-- ❌ WRONG: Filtering logic parsing text strings -->
<button data-filter="last 3 days" class="date-filter-btn">Last 3 days</button>
<button data-filter="last month" class="date-filter-btn">Last month</button>
```

**Why This Matters**:

- ✅ `data-days="3"` → Direct numeric calculation: `now - (3 * 24 * 60 * 60)`
- ❌ `data-filter="last 3 days"` → Requires text parsing: error-prone and fragile

## Date Format Standards

### Input Formats

- **Standard Format**: `YYYY-MM-DD` (ISO 8601)
- **Full Datetime**: `YYYY-MM-DD HH:MM:SS +ZONE`
- **Jekyll Front Matter**: Use standard Jekyll date format

### Output Formats

- **Epoch Timestamps**: PRIMARY format for all date storage and calculations
- **Display Formats**: Generated dynamically at runtime, localized for Brussels timezone
- **URL Parameters**: ISO date format for consistency

## JavaScript Client-Side Date Handling

### Critical Pattern: Epoch-Based Calculations

All client-side date filtering and display must use epoch timestamps:

```javascript
// ✅ CORRECT: Epoch-based date filtering with visitor's local timezone
function isWithinDateFilter(postEpoch, days) {
  const now = new Date();
  const today = new Date(now);
  today.setHours(0, 0, 0, 0);
  const todayStart = Math.floor(today.getTime() / 1000);
  
  if (days === 0) {
    // Today only
    const todayEnd = todayStart + 86399;
    return postEpoch >= todayStart && postEpoch <= todayEnd;
  } else {
    // "Last N days" means today + (N-1) days back
    const daysInSeconds = (days - 1) * 86400;
    const cutoffEpoch = todayStart - daysInSeconds;
    return postEpoch >= cutoffEpoch;
  }
}

// ✅ CORRECT: Dynamic relative date display
function formatRelativeDate(postEpoch) {
  const now = Math.floor(Date.now() / 1000);
  const daysAgo = Math.floor((now - postEpoch) / 86400);
  return `${daysAgo} days ago`;
}

// ❌ WRONG: Hardcoded relative calculations
function isLast3Days(postDate) {
  // This breaks when site was built days ago
  const threeDaysAgo = new Date();
  threeDaysAgo.setDate(threeDaysAgo.getDate() - 3);
  return new Date(postDate) >= threeDaysAgo;
}
```

### Timezone Consistency in JavaScript

```javascript
// ✅ CORRECT: Use epoch timestamps with visitor's local timezone for better UX
const postEpoch = parseInt(element.dataset.epoch);
const now = new Date();
const today = new Date(now);
today.setHours(0, 0, 0, 0);
const todayStart = Math.floor(today.getTime() / 1000);

// ❌ WRONG: Mixing timezone formats
const postDate = new Date(element.dataset.date);
const now = new Date(); // Uses client timezone inconsistently
```

### Critical Epoch Timestamp Requirements

**IMPORTANT**: While epoch timestamps are timezone-neutral for storage, ensure consistency between server-side epoch generation and client-side calculations:

```javascript
// ✅ CORRECT: Server and client both use epoch timestamps
// Server generates epoch from Brussels timezone (via Jekyll plugins)
// Client uses Date.now() which gives UTC epoch - this is consistent

// ❌ WRONG: Mixing timezones in epoch calculations
// Server: Brussels timezone → epoch
// Client: Local timezone → epoch
// These will be inconsistent unless both use UTC or same timezone
```

**Best Practice**: Always use UTC-based epoch timestamps for consistency across all client timezones.

## Testing Date Functionality

### Unit Testing

Test date filters and utilities with timezone awareness:

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

### End-to-End Testing

Test date filtering functionality in Playwright tests:

```javascript
test('date filters work correctly', async ({ page }) => {
  await page.goto('/');
  
  // Test date filter buttons appear
  await expect(page.locator('.date-filter-btn')).toBeVisible();
  
  // Test date filtering functionality
  await page.click('[data-date="2025-01"]');
  await expect(page.locator('.content-item')).toHaveCount(expectedCount);
});
```

## Performance Considerations

### Date Processing Optimization

- **Pre-calculate epoch timestamps**: Convert dates during build, not runtime
- **Cache date calculations**: Store computed values to avoid repeated processing  
- **Efficient date comparisons**: Use epoch timestamps for fast comparisons
- **Timezone consistency**: Prevent timezone conversion overhead

### Content Limiting Integration

The date system integrates with content limiting for performance:

- Date-based content limiting respects same-day boundaries
- Epoch timestamps enable fast date range filtering
- Pre-calculated date filters reduce client-side processing

For detailed content limiting implementation, see [Filtering System](filtering-system.md).
