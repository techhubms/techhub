# Filtering System

This document explains how the Tech Hub filtering system works to help users discover content.

## Filtering System Overview

The Tech Hub uses multiple filtering mechanisms that work together to help users find relevant content:

### Date Filters

**What it does**: Client-side filtering that narrows content by publication date ranges.

**Purpose**: Helps users find recent content or content from specific time periods.

**How it works**:

- Filter items by date ranges (e.g., "Last 30 days", "Last 6 months")
- Dynamically update displayed content without page reload
- Combine with other filters for refined searches

**Implementation**: JavaScript-based filtering using date metadata from item front matter.

### Section Tag Filters

**What it does**: Client-side tag filtering on the root index page that allows users to filter by main site sections.

**Purpose**: Enables users to focus on content from specific topical areas (AI vs GitHub Copilot).

**How it works**:

- Filter items using normalized section tags ("ai", "github copilot")
- Available only on the main index page (/)
- Uses the same tag matching logic as all other filters
- Dynamic content updates based on selected section tags
- Implements subset matching for tag-based filtering

**Implementation**: JavaScript-based using pre-calculated tag relationships from server-side generation.

### Collection Tag Filters

**What it does**: Client-side tag filtering on section index pages that filters by content type.

**Purpose**: Enables users to focus on specific content formats (News, Videos, Community) within a section.

**How it works**:

- Filter items using normalized collection tags ("news", "blogs", "videos")
- Available only on section index pages (/ai, /github-copilot)
- Uses the same tag matching logic as all other filters
- Each collection type corresponds to a normalized tag
- Dynamic content updates based on selected collection tags

**Implementation**: JavaScript-based using pre-calculated tag relationships from server-side generation.

### Content Tag Filters

**What it does**: Client-side tag filtering on collection pages that filters by keywords and topics.

**Purpose**: Enables users to find content related to specific technologies, concepts, or themes.

**How it works**:

- Filter items using normalized content tags from front matter
- Support multiple tag selection for intersection filtering (AND logic)
- Available only on individual collection pages
- Uses the same tag matching logic as all other filters
- Implements subset matching for tag-based content discovery

**Implementation**: JavaScript-based using pre-calculated tag relationships from server-side generation.

### Text Search Filter

**What it does**: Client-side real-time text search functionality.

**Purpose**: Enables users to quickly find content by searching across titles, descriptions, metadata, and tags using free-form text input.

**How it works**:

- Real-time filtering as user types with debounced input (300ms delay)
- Searches across multiple content areas: titles, descriptions, author info, and tags
- Case-insensitive and partial word matching
- Works alongside date and tag filters using AND logic
- URL parameter persistence for bookmarking search results
- Dedicated clear button for immediate search reset
- Keyboard shortcuts (Escape key) for quick clearing

**Implementation**: JavaScript-based using pre-indexed content strings generated during page load.

**Integration**: Text search enhances the filtering system by allowing users to combine structured filtering (dates, tags) with unstructured search (keywords).

## Server-Side Content Limiting: "20 + Same-Day" Rule

### Overview

**The "20 + Same-Day" Rule** is a server-side performance optimization applied to all index pages that limits the initial content load while ensuring users see all content from the current day.

### What It Does

1. **Load exactly 20 items** from the sorted content (newest first)
2. **Plus any additional items from the same day** as the 20th item
3. **Ensures complete daily coverage** so users never miss content from "today"
4. **Applied server-side during Jekyll build** before any client-side filtering

### Implementation Details

**Processing Order**:

1. Sort all content by date (newest first)
2. Take the first 20 items
3. Check the date of the 20th item
4. Include ALL additional items that share the same date as the 20th item
5. This becomes the "limited dataset" for the page

**Example Scenario**:

```text
Item 1-19: Various dates
Item 20: July 15, 2025
Item 21: July 15, 2025  ← Included (same day as 20th)
Item 22: July 15, 2025  ← Included (same day as 20th)
Item 23: July 14, 2025  ← NOT included (different day)
```

**Result**: Page shows 22 items (20 + 2 same-day items), ensuring complete July 15th coverage.

### Why This Rule Exists

**Performance Benefits**:

- **Reduces Initial Load Time**: Limits server processing and HTML size
- **Improves Client Performance**: Less DOM manipulation for filtering
- **Faster Page Rendering**: Smaller initial dataset to process

**User Experience Benefits**:

- **Complete Daily Coverage**: Users never miss content from the current day
- **Logical Boundaries**: Content cuts off at day boundaries, not arbitrary counts
- **Consistent Behavior**: Same rule applied across all index pages

### Where It's Applied

- **Root Index Page** (`/`)
- **Section Index Pages** (`/ai/`, `/github-copilot/`)
- **All other index pages** that aggregate content from multiple sources

### Integration with Filtering

**Filter Interaction**:

- All client-side filters operate on this limited dataset
- Filter counts reflect only the limited content shown
- Date filters respect the same-day boundary
- Tag filters work within the limited scope

**Performance Impact**:

- Dramatically reduces JavaScript processing load
- Enables real-time filtering without performance degradation
- Maintains responsive interactions even with large content libraries

### Technical Implementation

**Liquid Filter**: `limit_with_same_day`

```liquid
{%- assign sorted_items = collection | sort: 'date' | reverse -%}
{%- assign limited_items = sorted_items | limit_with_same_day -%}
{%- assign custom_limited_items = sorted_items | limit_with_same_day: 5 -%}
```

**Features**:

- **Configurable Limit**: Default 20 items, can be customized (e.g., `limit_with_same_day: 5`)
- **7-Day Recency Filter**: Automatically excludes content older than 7 days
- **Collection-Aware**: Applies limiting per collection independently
- **Same-Day Inclusion**: Includes all items from the same day as the limit boundary

**Custom Jekyll Filter Location**: `_plugins/date_filters.rb`

**Plugin Integration**: The `limit_with_same_day` filter is part of the broader plugin ecosystem that handles content processing. See [_plugins/AGENTS.md](../_plugins/AGENTS.md) for complete implementation details of this and other filtering-related plugins.

### 7-Day Recency Filtering

**Automatic Content Freshness**: The `limit_with_same_day` filter includes an automatic 7-day recency filter that ensures only recent content is displayed.

**How It Works**:

1. **Cutoff Calculation**: Items older than 7 days from the current date are automatically excluded
2. **Epoch Comparison**: Uses epoch timestamps for precise date comparison
3. **Applied Before Limiting**: Recency filtering happens before the "20 + Same-Day" rule
4. **Collection Independent**: Applied to each collection separately

**Example**:

```text
Current Date: July 20, 2025
Cutoff Date: July 13, 2025 (7 days ago)

Item from July 21, 2025  ← Included (recent)
Item from July 14, 2025  ← Included (within 7 days)
Item from July 12, 2025  ← Excluded (older than 7 days)
Item from July 10, 2025  ← Excluded (older than 7 days)
```

**Benefits**:

- **Improved Relevance**: Users see only fresh, recent content
- **Performance Optimization**: Reduces dataset size before other processing
- **Automatic Maintenance**: No manual content curation needed for freshness

## Core Filtering Behavior

### Filter Interaction Rules

The filtering system operates with three types of filters that work together to provide precise content discovery:

#### Filter Types and Logic

1. **Date Filters (Exclusive)**: Only one date filter can be active at a time
   - Selecting a new date filter replaces the previous selection
   - Date filters define the temporal scope for all content

2. **Tag-Based Filters (Inclusive)**: Multiple tag filters can be active simultaneously
   - Logic operates as AND (intersection), not OR (union)
   - Each additional filter further narrows the results
   - All active filters must match for content to be displayed
   - **Unified Implementation**: Sections, collections, and content tags are all implemented as tags

3. **Text Search Filter (Real-Time)**: Free-form text search that filters content by keywords
   - Searches across titles, descriptions, meta information, and tags
   - Works independently with both date and tag filters
   - Uses real-time filtering with debounced input for performance
   - Supports partial matching and case-insensitive search

#### Universal Tag-Based Architecture

The filtering system now uses a unified tag-based approach where all filter types (sections, collections, and content tags) are implemented as normalized tags with subset matching:

- **Sections as Tags**: "AI" and "GitHub Copilot" sections are treated as tags
- **Collections as Tags**: "News", "Blogs", "Videos", etc. are treated as tags  
- **Content Tags**: Traditional content tags like "Azure", "Visual Studio", etc.

**Subset Matching Logic**: Selecting a tag shows content with:

- That exact tag, OR
- Tags that contain the selected tag as complete words
- **Example**: Selecting "AI" matches "AI", "Generative AI", "Azure AI", "AI Agents"

**Additional Examples**:

- Selecting "Visual Studio" shows: "Visual Studio", "Visual Studio Code", "Visual Studio 2022"  
- Selecting "Azure" shows: "Azure", "Azure DevOps", "Azure AI", "Azure Functions"

**Implementation**: Server-side pre-calculated tag relationships provide ultra-fast lookups:

```javascript
// Tag relationships are pre-calculated on server and cached for performance
const relatedPostIndices = window.tagRelationships[tag];
const isMatch = relatedPostIndices.includes(postIndex);
```

**Server-Side Generation**: The `generate_all_filters` filter in `tag_filters.rb` provides unified tag relationship generation. See [_plugins/AGENTS.md](../_plugins/AGENTS.md) for the Ruby implementation details.

### Text Search Implementation

#### Overview

The text search functionality provides real-time content filtering based on user-entered keywords. It works seamlessly alongside date and tag filters to create powerful content discovery capabilities.

#### Search Functionality

**Search Scope**: Text search indexes and searches across multiple content areas:

- **Item titles**: Full title text content
- **Item descriptions**: Meta descriptions and excerpts  
- **Author and meta information**: Author names, publication dates, and metadata
- **Tags and categories**: All associated tags and category data

**Search Features**:

- **Case-insensitive matching**: Searches ignore letter case
- **Partial word matching**: Matches substrings within words
- **Real-time filtering**: Results update as you type with debounced input (300ms delay)
- **URL persistence**: Search terms are preserved in browser URL for bookmarking and sharing
- **Clear functionality**: One-click search clearing with dedicated clear button

#### Implementation Details

**Content Indexing**: During page load, JavaScript pre-processes all content items to create searchable text:

```javascript
// Pre-extracted content for fast text search
const content = titleText + ' ' + descriptionText + ' ' + metaText + ' ' + tagData;
cachedItem.content = content.toLowerCase().trim();
```

**Search Processing**: The `passesTextSearch()` function handles real-time filtering:

```javascript
function passesTextSearch(cachedPost) {
    if (!window.textSearchQuery) return true;
    const query = window.textSearchQuery.toLowerCase();
    return cachedItem.content.includes(query);
}
```

**Event Handling**: Multiple event handlers provide comprehensive search interaction:

- **Input events**: `handleTextSearchInput()` processes typing with debouncing
- **Keyboard shortcuts**: Escape key clears search (`handleTextSearchKeydown()`)
- **Clear button**: Dedicated clear button (`handleTextSearchClear()`)

#### User Interface

**Search Input Field**: Located prominently in the filter interface between the "Clear All" button and date filters:

```html
<div class="text-search-container">
  <input type="text" id="text-search-input" placeholder="Search" class="text-search-input">
  <button id="text-search-clear" class="text-search-clear-btn hidden" title="Clear search">×</button>
</div>
```

**Clear Button Behavior**:

- Automatically appears when text is entered
- Hidden when search field is empty
- Provides immediate search clearing functionality

#### Integration with Other Filters

**Filter Combination Logic**: Text search works additively with other filter types:

1. **Date Filter + Text Search**: Shows content from selected time period matching search terms
2. **Tag Filter + Text Search**: Shows tagged content matching search terms
3. **All Filters Combined**: Shows content matching date range, selected tags, AND search terms

**Filter Count Updates**: When text search is active, all filter button counts automatically update to reflect the intersection of search results with each filter option.

**URL State Management**: Search terms are preserved in the URL using the `search` parameter:

```
/ai/?filters=azure,visual%20studio&search=copilot
```

#### Performance Optimization

**Debounced Input**: Search updates are debounced (300ms delay) to prevent excessive filtering during rapid typing:

```javascript
clearTimeout(window.textSearchTimeout);
window.textSearchTimeout = setTimeout(() => {
    updateDisplay();
    updateURL();
}, 300);
```

**Pre-computed Content**: Search content is indexed once during page load, not recalculated during each search.

**Efficient Filtering**: Text search uses simple string inclusion rather than complex regex for maximum performance.

### Filter Interaction Behavior

#### Initial State

When first visiting a page, all filters display their full counts based on the complete dataset:

**Example - Collection Page (/ai/news)**:

```text
Date Filters:
- Today (3)
- Last 3 days (15)  
- Last week (200)

Tag Filters:
- Azure (15)
- OpenAI (10)
- Blog (3)
- News (60)
```

**Initial Button States**:

- All buttons are **inactive** (not selected)
- All buttons are **enabled** (clickable)
- Counts reflect the complete dataset

#### Date Filter Application

When a date filter is selected, it becomes the active temporal boundary:

**Example - After clicking "Last 3 days"**:

```text
Date Filters:
- Today (3)          ← Count unchanged (shows posts in period)
- Last 3 days (15)   ← Selected filter becomes active
- Last week (200)    ← Count unchanged (shows posts in period)

Tag Filters:
- Azure (10)         ← Filtered down from 15
- OpenAI (3)         ← Filtered down from 10  
- Blog (0)           ← Filtered down from 3, becomes disabled
- News (30)          ← Filtered down from 60
```

**Date Filter Button Behavior**:

- Selected filter becomes **active** and **enabled**
- Other date filters maintain their original counts and remain **inactive**
- Date filter counts always reflect the full time period given current tag filters
- Tag filters with zero count become **disabled**

**Key Independence Principle**: Date filters never affect each other's counts, but tag filters affect all filter counts including date filters.

#### Tag Filter Application

When tag filters are applied, they further narrow results using AND logic:

**Example - After adding "Azure" filter**:

```text
Date Filters:
- Today (1)          ← Count updated to show Azure posts today
- Last 3 days (10)   ← Remains active, count updated to show Azure posts
- Last week (25)     ← Count updated to show Azure posts in week

Tag Filters:
- Azure (10)         ← Becomes active, shows intersection count
- OpenAI (1)         ← Count updated to show intersection
- Blog (0)           ← Remains disabled
- News (10)          ← Count updated to show intersection
```

#### Multiple Tag Filter Application

Adding additional tag filters creates intersection (AND logic):

**Example - After adding "OpenAI" filter**:

```text
Date Filters:
- Today (1)          ← Count updated to show Azure+OpenAI posts today
- Last 3 days (8)    ← Remains active, count updated to show Azure+OpenAI posts
- Last week (15)     ← Count updated to show Azure+OpenAI posts in week

Tag Filters:
- Azure (1)          ← Remains active, shows final intersection count
- OpenAI (1)         ← Becomes active, shows final intersection count
- Blog (0)           ← Remains disabled
- News (1)           ← Count updated to show final intersection
```

#### Filter Removal

When filters are deselected, the filtering scope adjusts accordingly:

**Example - After deselecting "Last 3 days"**:

- Date filtering is removed completely
- Tag filters ("Azure" + "OpenAI") remain active
- All counts recalculate based on the new scope (full dataset + active tag filters)
- Button states update to reflect the new filtering state

### Button State Definitions

#### Active vs Inactive

- **Active**: Filter is currently applied (colored/highlighted button)
- **Inactive**: Filter is available but not applied (normal button appearance)

#### Enabled vs Disabled

- **Enabled**: Button is clickable and functional
- **Disabled**: Button is grayed out and non-functional (zero count scenarios)

#### State Combinations

1. **Inactive + Enabled**: Available filter showing count > 0
2. **Active + Enabled**: Currently applied filter with count > 0  
3. **Inactive + Disabled**: Available filter with count = 0
4. **Active + Disabled**: Not applicable (active filters always have count > 0)

### Critical Behavioral Rules

1. **Zero-Count Handling**: Filters with zero count become disabled to prevent empty results

2. **Date Filter Exclusivity**: Only one date filter can be active; selecting a new date filter replaces the previous selection

3. **Date Filter Count Independence**: Date filter counts never affect each other; each date filter always shows how many posts exist in that time period given current tag filters

4. **Tag Filter Accumulation**: Tag filters use AND logic; each additional filter further narrows results

5. **Text Search Integration**: Text search works alongside all other filters using AND logic; search terms must match in addition to any active date or tag filters

6. **Real-Time Updates**: All filter counts update immediately when any filter state changes, including text search input

7. **Subset Matching Consistency**: Tag subset matching applies consistently across all filter interactions

8. **Unified Tag Implementation**: All filters (sections, collections, content tags) are implemented as tags with identical behavior

9. **Text Search Persistence**: Search terms are preserved in URL parameters for bookmarking and sharing

### Server-Client Consistency Requirements

**Critical Implementation Requirements**:

- **Tag Relationships**: JavaScript uses server-generated tag relationship mappings for consistent results
- **Tag Filter Counts**: The server generates initial counts for all TAG filters (sections, collections, content tags). These counts are always correct and consistent, as they are not dependent on the user's timezone.
- **Date Filter Counts**: The server does NOT generate initial counts for DATE filters. Instead, date filter counts are always recalculated client-side in JavaScript, using the user's local timezone. This ensures that date-based filtering (e.g., "Today") reflects the user's actual day, not the server's timezone. This also means that the set of visible posts for a date filter is always correct for the user's context, even if it differs from the server's timezone. The logic for which date filter buttons are visible and enabled is now handled entirely on the client, based on the user's timezone and the current filtered dataset.
- **Date Filter Options**: The available date filter options (e.g., "Today", "Last 3 days", "Last 4 days", etc.) are configured in `_config.yml` and may change over time. Documentation and UI should reflect the current set of options.
- **Normalization**: Tag processing must use server-generated normalized tag data

**Server-Side vs Client-Side Processing**:

The filtering system maintains consistency between server-side Jekyll/Liquid processing and client-side JavaScript enhancement through carefully coordinated data handling:

1. **Server-Side (Jekyll/Liquid)**:
   - Renders all initial content before JavaScript loads using `Europe/Brussels` timezone consistently
   - Handles content limiting using `limit_with_same_day` filter from `date_filters.rb`
   - Calculates tag relationships and initial tag filter counts using `generate_all_filters` filter from `tag_filters.rb`
   - Pre-calculates tag-to-post mappings for ultra-fast client-side lookups
   - All date operations enforced by date utility plugins for timezone consistency

2. **Client-Side (JavaScript)**:
   - Enhances server-rendered content with interactivity
   - Handles real-time filtering without page reload
   - Uses pre-calculated tag relationships for instant filtering
   - Recalculates all DATE filter counts (e.g., "Today", "Last 3 days") using the user's local timezone. This means the set of visible posts for a date filter is always correct for the user's context, even if it differs from the server's timezone.
   - Implements real-time text search with content indexing and debounced input processing
   - Manages text search URL parameters for state persistence
   - Exception: Only `assets/js/sections.js` may modify content on page load

**Plugin-JavaScript Consistency**: Client-side filtering logic uses server-generated tag relationship mappings to ensure consistent user experience. See [_plugins/AGENTS.md](../_plugins/AGENTS.md) for the server-side filter implementations that generate the data structures JavaScript uses.

**Special Rule for Date Filter Buttons**:

- The "Today (0)" date filter button (and other date filter buttons with zero count) should only be hidden client-side if there are truly zero posts for that date range in the user's timezone (as determined by JavaScript). If the count is zero due to active tag filters, the button should remain visible (with a count of 0), allowing the user to understand the effect of their tag selections. This logic is now handled entirely on the client, and applies to all date filter buttons.

### Filter Display Logic

#### Date Filter Display Rules

**Current Date Filter Options**: The following date filter options are currently configured in `_config.yml` and available in the filtering UI:

- Today
- Last 2 days
- Last 3 days
- Last 4 days
- Last 5 days
- Last 6 days
- Last 7 days
- Last 14 days
- Last 30 days
- Last 60 days
- Last 90 days
- Last 180 days
- Last 365 days

These options may change over time. Always check `_config.yml` for the authoritative list.

**🚨 SERVER-SIDE REQUIREMENT**: On initial page load, the server only generates date filters with `count > 0` (using the `generate_eligible_date_filters` plugin). This means zero-count date filters cannot exist when no tag filters are active, ensuring users never see unusable filter buttons on first load.

**Client-Side Behavior**:

- When no tag filters are active: All visible date filters have count > 0 (guaranteed by server-side generation).
- When tag filters ARE active: Some date filters may show (0) count as they update dynamically. The client recalculates all date filter counts in the user's timezone, and applies the following rules:
  - Date filter buttons are only hidden if there are truly zero posts for that date range in the user's timezone (not just due to tag filters).
  - If a date filter has zero posts due to tag filters, the button remains visible (with a count of 0), so users can see the effect of their selections.
  - This logic applies to all date filter buttons, not just "Today".

**Additional Display Rules When No Tag Filters Active**:

1. **Apply 50% growth rule**: Only show filters that add meaningful growth
   - First filter with posts is always included
   - Subsequent filters need ≥50% more posts: `max(prev_count * 1.5, prev_count + 1)`
   - **Example**: "Today (6)" → "Last 3 days" needs ≥9 posts to show
2. **Apply final cleanup rule**: Remove last filter if it includes ≥85% of all posts
   - **Example**: Remove "Last 365 days (3)" when only 3 total posts exist

#### Tag Filter Display Management

**Tag Display Limits**:

```text
collapsed_view_count: 30
expanded_view_max_count: 100
```

**Display Behavior**:

- **Collapsed View**: Maximum 30 tags shown, sorted alphabetically
- **Expanded View**: Maximum 100 tags shown, sorted alphabetically
- **"More" button**: Expands to full view when >30 tags available
- **"Less" button**: Collapses back to 30 tags

**Selection Logic**:

1. Start with complete alphabetically-sorted tag list
2. Apply count-based filtering to fit display limits
3. Remove lowest-count tags until target reached
4. Display remaining tags alphabetically

#### Interactive Button Behaviors

**Core Interactive Elements**:

- **More button**: Expands to show up to 100 tags (when >30 available)
- **Less button**: Collapses back to 30 tags
- **Clear All button**: Removes all selections (date filters, tag filters, and text search), collapses tags, resets to default state

**Filter Interaction Flow**:

```javascript
User clicks tag → JavaScript filters content → DOM updated → URL updated
```

## Tag Data Structure

Each tag object contains standardized properties:

```json
{
    "count": 130,
    "display": "Visual Studio",
    "first_seen": "2025-02-24",
    "normalized": "visual studio"
}
```

**Property Definitions**:

- **`count`**: Number of times this tag appears (calculated by `tag_filters.rb` plugin during build)
- **`display`**: Human-readable tag name for UI display (processed through `normalize_tag` filter)
- **`first_seen`**: Date when tag first appeared (format: "YYYY-MM-DD", tracked during processing)
- **`normalized`**: Lowercase, standardized version for programmatic use (generated by `tag_filters.rb`)

**Plugin Data Source**: Tag data is processed by the `tag_filters.rb` plugin during Jekyll build. See [_plugins/AGENTS.md](../_plugins/AGENTS.md) for detailed plugin architecture and tag processing.

## Filter Modes by Page Type

The Tech Hub implements three distinct filtering modes based on page hierarchy, each with date filters plus one additional filter type. All non-date filters are implemented as tags for unified behavior.

### Root Index: Date + Section Tag Filters

- **URL**: '/' (main index page)
- **Date Filters**: "Last 3 days", "Last 30 days", etc.
- **Section Tag Filters**: Filter by main site sections (AI, GitHub Copilot) - implemented as tags
- **Data Source**: `site.data.sections` configuration
- **Filtering Logic**: Uses normalized tag matching on section tags
- **Implementation**: Dynamically generates section filter buttons based on sections with content
- **Expected Behavior**: Shows both date filters and section tag filters due to content diversity across multiple sections

### Section Index: Date + Collection Tag Filters

- **URLs**: '/ai' and '/github-copilot' (section index pages)
- **Date Filters**: "Last 3 days", "Last 30 days", etc.
- **Collection Tag Filters**: Filter by content types within the section (News, Blogs, Videos, Community, etc.) - implemented as tags
- **Data Source**: `site.data.sections` configuration for collections within each section
- **Filtering Logic**: Uses normalized tag matching on collection tags
- **Behavior**: Shows mixed content from multiple collections within the section, allowing users to filter by content type
- **Implementation**: Filters by collection type tags (news, blogs, videos, community, etc.) within the current section
- **Expected Behavior**: Shows both date filters and collection tag filters when sufficient content diversity exists across collections

### Collection Pages: Date + Content Tag Filters

- **URLs**: '/ai/news.html', '/github-copilot/blogs.html', etc. (individual collection pages)
- **Date Filters**: "Last 3 days", "Last 30 days", etc.
- **Content Tag Filters**: Filter by content tags (AI, Azure, Visual Studio, etc.) - traditional content tags
- **Data Source**: Pre-calculated tag relationships from `generate_all_filters` filter
- **Filtering Logic**: Uses normalized tag matching with subset matching for content tags
- **Implementation**: Shows actual content tags with counts, applies subset matching for filtering
- **Expected Behavior**: Shows both date filters and content tag filters due to content diversity across different topics and technologies

**Note**: All filter types except date filters are now implemented as tags using the same unified tag-based architecture and filtering logic.

### Filter Mode Requirements

#### Expected Behavior: Both Filter Types Present

Each of the three filter modes is expected to display both filter types under normal operating conditions:

1. **Root Index (/)**: Expected to show both date filters AND section tag filters due to content diversity across multiple sections (AI, GitHub Copilot)
2. **Section Index (/ai, /github-copilot)**: Expected to show both date filters AND collection tag filters when content spans multiple collections within the section  
3. **Collection Pages (*/collection.html)**: Expected to show both date filters AND content tag filters due to content diversity across different topics and technologies

**Note**: In rare cases when new collections or sections are created without content, one filter type may temporarily be absent. This is expected behavior and requires no special handling - normal content publication will restore both filter types.

## Filter State Management

### URL Parameters and State

- **Filter State**: Preserved in URL parameters for bookmarking and sharing
- **Cross-Page Consistency**: Filter preferences maintained across navigation
- **Reset Behavior**: Clear buttons reset to default state

## Performance and Data Consistency

### Performance Optimizations

**Server-Side Optimizations**:

1. **Content Limiting**: "20 + same-day" rule reduces processing load on index pages (see above)
2. **Pre-calculated Tag Counts**: Tag counts computed during build
3. **Efficient JSON Lookup Tables**: Optimized data structures for fast access
4. **Plugin Processing**: Complex logic handled during build, not runtime
5. **Server-Side Limiting**: "20 + Same-Day" rule reduces initial dataset
6. **Caching**: Plugin-generated data files cached between builds

**Client-Side Optimizations**:

1. **DOM Manipulation**: Show/hide existing elements (no re-rendering)
2. **Minimal DOM Queries**: Efficient event delegation
3. **Debounced Updates**: Prevent excessive recalculations during rapid interactions
4. **Pre-indexed Text Search**: Content indexed once on page load for fast search
5. **Client Efficiency**: JavaScript operates on pre-limited, structured data

### Data Consistency Requirements

1. **Filter Counts Must Match Filtered Content**: Exactly matching counts across all states
2. **Content Limiting Integration**: "20 + same-day" rule must work seamlessly with filters
3. **Graceful Degradation**: Site works with JavaScript disabled
4. **Error Handling**: Invalid states reset to default

### Count Accuracy

- **Real-Time Updates**: Filter counts update as selections change
- **Subset Matching**: Counts reflect subset matching logic
- **Limited Dataset**: Index page counts based on "20 + same-day" limited item set
- **Server-Client Parity**: Server rendering matches client filtering results

## Filter Debugging and Troubleshooting

### Jekyll Restart Requirements

**Critical Rule**: Restart Jekyll after tag data changes to ensure proper filter synchronization.

For complete Jekyll server management rules, restart commands, and troubleshooting, see [.github/agents/fullstack.md](../.github/agents/fullstack.md).

**Filter-Specific Restart Triggers**:

- After adding posts with NEW tags that didn't exist before
- After removing posts that were the ONLY ones using specific tags
- After modifying the `tags:` field in any post's front matter
- When tag filters show incorrect counts or missing tags

**Symptoms of Out-of-Sync Filters**:

- Tag buttons missing for posts that should have them
- Filter counts not matching actual post counts
- Date filter buttons showing incorrect availability
- Any discrepancy between visible posts and filter button state

### Common Filter Issues

#### Filter Mismatches

**Symptoms**:

- Server-side and client-side filters show different counts
- Posts appearing in wrong date ranges
- Missing expected filters

**Common Causes**:

1. **Timezone mismatches**: Posts in wrong date ranges
2. **Filter count discrepancies**: Server vs client calculations differ
3. **Missing expected filters**: Threshold calculations incorrect

**Solutions**:

- Check timezone handling between Jekyll and JavaScript
- Validate that the content limiting logic works correctly (see "20 + Same-Day" Rule above)
- Verify server-rendered HTML structure
- Compare server vs client date calculations

#### Tag Filtering Issues

**Symptoms**:

- Expected tags not appearing in filters
- Incorrect tag counts
- Subset matching not working

**Solutions**:

- Check PowerShell tag enhancement execution
- Verify JSON tag data generation
- Test regex word boundary matching
- Validate tag normalization process

### Debug Tools and Techniques

**Browser Console Debugging**:

See [spec/AGENTS.md](../spec/AGENTS.md#debugging-tests) for debugging techniques including Playwright debugging.

**Server-Side Debugging**:

```liquid
{%- assign category_tags = site.data.category_tags[collection_type][category] -%}
{%- for tag in category_tags -%}
  <!-- Debug: {{ tag.normalized }} ({{ tag.count }}) -->
{%- endfor -%}
```

### PowerShell vs Frontend Distinction

**PowerShell Scripts**:

- Normalize common tag variations
- Run during content processing, not user interaction

**Frontend JavaScript**:

- Implements universal subset matching for any tag combination
- Handles real-time user interactions
- Works with any tag - no configuration needed

## Mobile and Responsive Considerations

### Touch-Friendly Design

- **Touch Events**: All filter elements sized for touch interaction
- **No Hover Dependencies**: Hover effects only on devices that support hover
- **Responsive Layout**: Adapts to all screen sizes without horizontal scrolling

### Performance on Mobile

- **Efficient DOM Manipulation**: Optimized for mobile device performance
- **Network Optimization**: Minimal data transfer for filter updates
- **Battery Efficiency**: Debounced updates to reduce processing load

## Complete Data and Rendering Flow

**INDEX:**

- [Stage 1: Content Creation and Frontmatter](#stage-1-content-creation-and-frontmatter)
- [Stage 2: PowerShell Preprocessing](#stage-2-powershell-preprocessing)
- [Stage 3: Jekyll Plugin Processing](#stage-3-jekyll-plugin-processing)
- [Stage 4: Dynamic Page Generation](#stage-4-dynamic-page-generation)
- [Stage 5: Template Processing and Rendering](#stage-5-template-processing-and-rendering)
- [Stage 6: Runtime Filter Processing](#stage-6-runtime-filter-processing)
- [Stage 7: Server-Side HTML Generation](#stage-7-server-side-html-generation)
- [Stage 8: Client-Side Enhancement](#stage-8-client-side-enhancement)
- [Stage 9: User Interaction Flow](#stage-9-user-interaction-flow)

The Tech Hub filtering system implements a comprehensive data processing pipeline that transforms content from initial creation to interactive browser experience. This end-to-end flow ensures consistent tag processing, efficient content delivery, and seamless user interactions.

**Overall Pipeline**:

```text
Content Creation → PowerShell Enhancement → Jekyll Plugin Processing → Dynamic Page Generation → Template Rendering → Runtime Processing → HTML Generation → Client Enhancement → User Interaction
```

### Stage 1: Content Creation and Frontmatter

**Initial State**: Content creators add tags to frontmatter in markdown files:

```markdown
---
title: "Getting Started with GitHub Copilot"
date: 2025-07-21
tags: ["GitHub Copilot", "AI", "Visual Studio Code", "Development Tools"]
---
```

**Tag Source Locations**:

- `_blogs/`: Blogs with tags in frontmatter
- `_community/`: Magazines, community discussions and Q&A posts
- `_news/`: News articles and announcements
- `_videos/`: Video content with descriptive tags
- Other collections: Roundups

### Stage 2: PowerShell Preprocessing

**Tag Enhancement Process**: PowerShell scripts clean and standardize tags before Jekyll processing:

**Key Operations**:

1. **Tag Normalization**: Standardize formatting and capitalization
2. **Tag Enhancement**: Add related tags based on content analysis
3. **Duplicate Removal**: Eliminate redundant or similar tags
4. **Validation**: Ensure tags follow site conventions

**Output**: Enhanced markdown files with standardized, cleaned tags ready for Jekyll processing.

### Stage 3: Jekyll Plugin Processing

**Plugin Execution Order** (during Jekyll startup):

1. **`tag_filters.rb`**:
   - Processes all collections for tag data during template rendering
   - Provides tag filtering and counting functionality
   - Calculates tag statistics dynamically

2. **`section_pages_generator.rb`**:
   - Creates dynamic section pages (e.g., `/ai/`, `/github-copilot/`)
   - Configures tag filtering capability for each generated page

**Tag Processing**:

Tag data is processed dynamically by the `tag_filters.rb` plugin, which provides:

- Tag counting and aggregation functions
- Tag normalization and filtering capabilities  
- Dynamic tag relationship processing for optimized filtering

### Stage 4: Dynamic Page Generation

**Page Generation Process**: `section_pages_generator.rb` creates pages that will use the filtering system:

- **Section Index Pages**: Generated with `index_tag_mode` set appropriately for each page type
- **Collection Pages**: Created with tag filtering configuration
- **Template Assignment**: Pages assigned appropriate layouts and includes
- **Filter Mode Configuration**: Each page type configured with correct filtering mode

### Stage 5: Template Processing and Rendering

**Template Architecture Flow**:

1. **Page Layout Setup**: `_includes/section-index.html`
   - Coordinates overall page structure for section index pages
   - Filters content by section category using `site.documents | where: "categories", section_data.category`
   - Applies "20 + Same-Day" limiting rule via `limit_with_same_day` filter
   - Delegates to `items.html` for content display and filtering

2. **Content Display and Filter Coordination**: `_includes/items.html`
   - Receives items from parent templates
   - Sorts and processes content items
   - Calculates oldest item date for filter generation
   - Delegates to `filters.html` for filter generation
   - Renders content items with data attributes for JavaScript filtering

3. **Filter Generation**: `_includes/filters.html`
   - Main entry point for the unified filtering system
   - Reads configuration from `_config.yml` for date filters and display limits
   - Generates JavaScript data for client-side filtering using `generate_all_filters` filter
   - Conditionally renders filter components based on `index_tag_mode`:
     - Date range filters (always rendered first when applicable)
     - Section filters (for root index when `index_tag_mode == 'sections'`)
     - Collection type filters (for section indexes when `index_tag_mode == 'collections'`)
     - Tag-based filters (for collection pages when `index_tag_mode == 'tags'` or default)
   - All non-date filters are now implemented as tags with unified rendering
   - Loads single JavaScript file with mode-specific logic

### Stage 6: Runtime Filter Processing

**Dynamic Filter Generation**: Additional Liquid filters process content during template rendering:

- **`generate_eligible_date_filters`**: Determines which date filters to show based on content
- **`generate_all_filters`**: Creates comprehensive filter data including tag relationships and counts

**Key Liquid Filters Used**:

```liquid
{%- assign limited_blogs = posts | limit_with_same_day -%}
{%- assign filter_results = items | generate_all_filters: index_tag_mode, site.data.sections, section, category, collection, site.tag_filter -%}
{%- assign tag_filter_data = filter_results.tag_filter_data -%}
{%- assign tag_relationships = filter_results.tag_relationships -%}
{%- assign date_filters = filter_results.date_filter_config -%}
```

### Stage 7: Server-Side HTML Generation

**Output**: Complete functional HTML page with:

- **Filter Buttons**: Pre-rendered with accurate counts from plugin data
- **Content Items**: Limited dataset with data attributes for JavaScript
- **Data Attributes**: `data-tags`, `data-epoch`, `data-collection`, `data-categories` for client-side filtering
- **Graceful Degradation**: Fully functional without JavaScript

**Example Generated HTML**:

```html
<button class="tag-filter" data-tag="github copilot">
  GitHub Copilot (15)
</button>

<li class="post-item"
    data-tags="github copilot,ai,visual studio code"
    data-epoch="1752876000"
    data-collection="news"
    data-categories="ai,github copilot">
  <!-- Content -->
</li>
```

### Stage 8: Client-Side Enhancement

**JavaScript Architecture:**

The filtering system uses a unified JavaScript file that handles all filtering modes and interactions:

**`assets/js/filters.js`** - Complete filtering functionality including:

- **Unified Tag Logic**: Handles all filter types (sections, collections, tags) as tags
- **Real-Time Filtering**: Updates content visibility and filter counts instantly
- **URL State Management**: Preserves filter state in browser URL for bookmarking
- **Subset Matching**: Implements universal tag subset matching with pre-calculated relationships
- **Date Filter Logic**: Handles temporal filtering with epoch timestamp calculations
- **Cross-Browser Compatibility**: Ensures consistent behavior across all browsers

**Client-Side Processing**:

1. **DOM Ready**: JavaScript scans server-rendered content
2. **Filter Setup**: Attaches click handlers to filter buttons
3. **State Management**: Initializes URL-based filter state
4. **Interactive Filtering**: Real-time content filtering without page reload

**JavaScript Filter Logic** (uses server-side data):

```javascript
// Tag relationship lookup (uses pre-calculated mappings)
const relatedPostIndices = window.tagRelationships[tag];
const isMatch = relatedPostIndices.includes(postIndex);

// Date filtering (matches date_to_epoch filter)
const postEpoch = parseInt(element.dataset.epoch);
return postEpoch >= selectedDateRange;
```

### Stage 9: User Interaction Flow

**Complete User Journey**:

1. **Page Load**: Server renders complete functional page
2. **JavaScript Enhancement**: Adds interactive filtering
3. **User Clicks Filter**: JavaScript filters visible content
4. **DOM Update**: Content shown/hidden based on filter criteria
5. **URL Update**: Browser URL reflects current filter state
6. **Back/Forward**: URL state restored for browser navigation

This pipeline ensures consistent, performant tag filtering from content creation through user interaction.
