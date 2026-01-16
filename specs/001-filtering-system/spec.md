# Feature Specification: Sidebar Content Filtering

**Feature Branch**: `001-filtering-system`  
**Created**: 2026-01-16  
**Status**: Draft  
**Input**: Implement sidebar-based tag and date filtering for section and collection pages to help users discover relevant content quickly

## User Scenarios & Testing

### User Story 1 - Filter Content by Tags (Priority: P1)

Users can click tags in the sidebar to filter content items and see only items matching selected tags.

**Why this priority**: Primary content discovery mechanism, most requested feature from users.

**Independent Test**: Navigate to any section page, click a tag in sidebar, verify content list updates to show only matching items.

**Acceptance Scenarios**:

1. **Given** I'm on a section page, **When** I click a tag in the sidebar, **Then** the content list updates to show only items with that tag
2. **Given** I have one tag selected, **When** I click another tag, **Then** the content list shows items matching ANY of the selected tags (OR logic)
3. **Given** I have tags selected, **When** I click a selected tag again, **Then** it deselects and content list updates
4. **Given** I filter by tags, **When** I check the URL, **Then** it includes my selected tags as query parameters (e.g., `?tags=ai,azure`)
5. **Given** I share the URL with tags, **When** someone opens it, **Then** they see the same filtered view

---

### User Story 2 - Filter Content by Date Range (Priority: P1)

Users can select date ranges (Last 7/30/90 days, All Time) to filter content by publication date.

**Why this priority**: Essential for finding recent content, complements tag filtering.

**Independent Test**: Navigate to section page, click "Last 30 days" filter, verify only recent content displays.

**Acceptance Scenarios**:

1. **Given** I'm on a section page, **When** I select "Last 30 days", **Then** the content list shows only items from the past 30 days
2. **Given** I have a date filter active, **When** I select a different date range, **Then** the content updates to that range
3. **Given** I have date and tag filters active, **When** I view results, **Then** I see items matching BOTH filters (AND logic)
4. **Given** I select a date range, **When** I check the URL, **Then** it includes the date parameter (e.g., `?date=last-30-days`)

---

### User Story 3 - Clear All Filters (Priority: P2)

Users can quickly reset all active filters to see all content again.

**Why this priority**: Escape hatch when users over-filter and get zero results.

**Independent Test**: Apply multiple filters, click "Clear All" button, verify all content displays and URL resets.

**Acceptance Scenarios**:

1. **Given** I have multiple filters active, **When** I click "Clear All Filters", **Then** all filters reset and all content displays
2. **Given** I clear all filters, **When** I check the URL, **Then** filter parameters are removed
3. **Given** I have filters active, **When** I click the browser back button, **Then** filters update to previous state

---

### User Story 4 - See Active Filter Indicators (Priority: P2)

Users can see which filters are currently active with visual badges and result counts.

**Why this priority**: Provides feedback and context about current filter state.

**Independent Test**: Apply filters, verify active tags are highlighted, result count updates, badges display.

**Acceptance Scenarios**:

1. **Given** I select a tag, **When** the filter applies, **Then** the tag is visually highlighted in the sidebar
2. **Given** I have filters active, **When** I view the content area, **Then** I see a result count (e.g., "Showing 15 results")
3. **Given** I have multiple filters, **When** I view the page, **Then** I see badges or chips indicating active filters
4. **Given** no items match my filters, **When** I view results, **Then** I see a "No results found" message with suggestion to clear filters

---

### User Story 5 - Navigate Filter State with Browser (Priority: P3)

Users can use browser back/forward buttons to navigate through filter states.

**Why this priority**: Natural browser behavior expectation, enhances UX but not critical for MVP.

**Independent Test**: Apply filters, use browser back button, verify previous filter state restores.

**Acceptance Scenarios**:

1. **Given** I apply filters, **When** I click browser back button, **Then** the previous filter state restores
2. **Given** I go back through filter states, **When** I click browser forward button, **Then** I move forward through filter history
3. **Given** I navigate filter states, **When** content updates, **Then** the page does not fully reload (client-side only)

---

### User Story 6 - Filter with Tag Subset Matching (Priority: P1)

Users benefit from intelligent tag matching where selecting a tag shows all content with that tag OR tags that contain it as complete words.

**Why this priority**: Critical for user experience - prevents users from missing relevant content due to tag variations.

**Independent Test**: Navigate to section page, click "AI" tag, verify results include items tagged with "AI", "Generative AI", "Azure AI", etc.

**Acceptance Scenarios**:

1. **Given** I'm on a section page, **When** I select "AI" tag, **Then** I see items tagged with "AI", "Generative AI", "Azure AI", "AI Agents", etc.
2. **Given** I select "Visual Studio" tag, **When** results display, **Then** I see items with "Visual Studio", "Visual Studio Code", "Visual Studio 2022"
3. **Given** I select "Azure" tag, **When** results display, **Then** I see items with "Azure", "Azure DevOps", "Azure AI", "Azure Functions"
4. **Given** I select a tag, **When** viewing results, **Then** subset matching uses complete word boundaries ("AI" matches "Azure AI" but not "AIR")
5. **Given** I combine subset-matched tags with date filters, **When** viewing results, **Then** AND logic applies correctly across all filters

---

### User Story 7 - Interactive Date Range Slider (Priority: P2)

Users can select custom date ranges using an interactive slider with from-to controls for precise temporal filtering, with smart defaults to prevent overwhelming content.

**Why this priority**: Modern UX pattern provides flexibility, prevents information overload with sensible defaults.

**Independent Test**: Navigate to section page, verify slider defaults to last 3 months, adjust range, verify content filters correctly.

**Acceptance Scenarios**:

1. **Given** I navigate to any page, **When** the page loads, **Then** the date slider defaults to "last 3 months" (prevents showing endless content)
2. **Given** I'm on a section page, **When** I interact with the date range slider, **Then** I see two draggable handles (from and to dates)
3. **Given** I drag the "from" handle backward, **When** I release, **Then** I can extend the range to see older content (no limit)
4. **Given** I drag the "to" handle, **When** I release, **Then** content filters to show items up to that date
5. **Given** I set a custom range, **When** I check the URL, **Then** it includes from/to date parameters (e.g., `?from=2024-10-16&to=2026-01-16`)
6. **Given** I use the slider, **When** I view the UI, **Then** I see the selected date range displayed clearly (e.g., "Oct 16, 2024 - Jan 16, 2026")
7. **Given** I combine slider with tag filters, **When** viewing results, **Then** AND logic applies (items must match date range AND tags)
8. **Given** I share a URL with custom date range, **When** someone opens it, **Then** they see the same filtered view with slider positioned correctly
9. **Given** I scroll down with infinite scroll enabled, **When** I reach the bottom, **Then** more content loads only within the selected date range

---

### User Story 8 - Excel-Style Tag Dropdown Filter (Priority: P1)

Users can filter content by tags using a modern dropdown interface similar to Excel column filtering, positioned below the date slider for optimal workflow.

**Why this priority**: Intuitive filtering pattern familiar from Excel, provides searchable tag list with checkboxes.

**Independent Test**: Click tag dropdown below date slider, search for tag, select via checkbox, verify content filters.

**Acceptance Scenarios**:

1. **Given** I'm on a section/collection page, **When** I view the sidebar, **Then** I see a "Filter by Tags" dropdown positioned below the date slider
2. **Given** I click the tag dropdown, **When** it opens, **Then** I see a searchable list of all available tags with checkboxes
3. **Given** the dropdown is open, **When** I type in the search box, **Then** the tag list filters to show only matching tags
4. **Given** I select a tag checkbox, **When** I apply the filter, **Then** content updates to show only items with that tag
5. **Given** I select multiple tags, **When** filters apply, **Then** content shows items matching ANY selected tag (OR logic)
6. **Given** I have tags selected, **When** I check the URL, **Then** it includes selected tags (e.g., `?tags=ai,azure`)
7. **Given** the dropdown shows tag counts, **When** I view the list, **Then** each tag displays the number of items (e.g., "AI (42)")
8. **Given** I have filters active, **When** I view the dropdown, **Then** selected tags are visually indicated (checkboxes checked)
9. **Given** the dropdown is open, **When** I click outside or press Escape, **Then** the dropdown closes and filters persist

---

### User Story 9 - Tag Clouds on Content Items (Priority: P2)

Users see clickable tag clouds on each content item that link directly to filtered views, enabling quick discovery of related content.

**Why this priority**: Improves content discovery, provides visual indication of article topics, creates natural navigation paths.

**Independent Test**: View a content item, click a tag in its tag cloud, verify navigation to filtered view.

**Acceptance Scenarios**:

1. **Given** I view a content item (article, video, etc.), **When** I look at the item card/detail, **Then** I see a tag cloud showing all tags for that item
2. **Given** I see a tag in the tag cloud, **When** I click it, **Then** I navigate to a page showing all content filtered by that tag
3. **Given** I click a tag, **When** the filtered page loads, **Then** the tag dropdown shows that tag as selected
4. **Given** I view multiple content items, **When** comparing tag clouds, **Then** tag sizes/styling are consistent across items
5. **Given** an item has many tags, **When** viewing the tag cloud, **Then** all tags are visible (or expandable if space-constrained)

---

### User Story 10 - Popular Tag Clouds on Homepage/Sections (Priority: P2)

Users see popular tag clouds on homepage and section pages showing the most-used tags in the last 3 months, helping discover trending topics.

**Why this priority**: Surfaces trending content, guides users to popular topics, provides at-a-glance topic overview.

**Independent Test**: Navigate to homepage, verify popular tags display (based on last 3 months), click tag to filter.

**Acceptance Scenarios**:

1. **Given** I'm on the homepage, **When** the page loads, **Then** I see a "Popular Tags" cloud showing most-used tags from the last 3 months
2. **Given** I'm on a section page (e.g., /ai), **When** the page loads, **Then** I see popular tags specific to that section (last 3 months)
3. **Given** I view the popular tags cloud, **When** I examine the display, **Then** tag sizes reflect relative popularity (larger = more used)
4. **Given** I click a popular tag, **When** navigation occurs, **Then** I see content filtered by that tag
5. **Given** I view popular tags, **When** I hover over a tag, **Then** I see a tooltip showing the count (e.g., "42 items")
6. **Given** the popular tags update, **When** viewing different sections, **Then** each section shows its own popular tags (not global)
7. **Given** I'm on a collection page (e.g., /ai/news), **When** I view the page, **Then** I do NOT see popular tags (only on homepage/section pages)

---

### Edge Cases

- What happens when user selects tags that yield zero results? → Show "No results found" message with "Clear All Filters" button
- What happens when URL has invalid date range parameter? → Fallback to "last 3 months" default, log error
- What happens when no date range is specified in URL? → Default to last 3 months
- What happens when tag dropdown has hundreds of tags? → Implement virtual scrolling, show top 100, require search to find others
- What happens when user searches in tag dropdown with no matches? → Show "No tags found" message, allow clearing search
- What happens when there are no popular tags in last 3 months? → Show message "No recent content" or hide popular tags section
- What happens when content item has many tags (e.g., 20+)? → Show all tags, allow horizontal scroll or wrapping as needed
- What happens when user clicks tag on content item vs popular tag cloud? → Both navigate to same filtered view, same behavior
- What happens when URL has invalid tag names? → Ignore invalid tags, apply only valid ones, log warning
- What happens when user rapidly clicks multiple tags? → Debounce UI updates, only apply final state
- What happens on slow connections? → Show loading indicator, don't block UI, allow filter changes during load
- What happens when content is already filtered by collection via SubNav? → Combine collection filter with sidebar filters (AND logic)

## Requirements

### Functional Requirements

- **FR-001**: Sidebar MUST display a "Tags" section with all available tags for the current section/collection
- **FR-002**: Tags MUST be clickable and toggle between selected/unselected states
- **FR-003**: Selected tags MUST be visually distinguished (highlighted, different color, checkmark icon)
- **FR-004**: System MUST filter content to show items matching ANY selected tag (OR logic within tags)
- **FR-005**: System MUST provide date filtering via interactive slider with from-to date range selection
- **FR-005a**: Date slider MUST default to "last 3 months" when no date range specified in URL
- **FR-005b**: System MAY provide preset date range buttons (Last 7/30/90 days) as quick shortcuts alongside slider
- **FR-006**: Date range filters MUST combine with tag filters using AND logic (must match date AND any tag)
- **FR-006a**: System MUST implement tag subset matching (selecting "AI" matches "AI", "Generative AI", "Azure AI", etc.)
- **FR-006b**: Subset matching MUST use complete word boundaries ("AI" matches "Azure AI" but not "AIR")
- **FR-007**: System MUST provide Excel-style tag dropdown filter positioned below date slider
- **FR-007a**: Tag dropdown MUST include search box for filtering tag list
- **FR-007b**: Tag dropdown MUST show checkboxes for multi-select capability
- **FR-007c**: Tag dropdown MUST display item counts next to each tag (e.g., "AI (42)")
- **FR-007d**: Tag dropdown MUST support virtual scrolling for large tag lists (100+ tags)
- **FR-008**: Content items MUST display tag clouds showing all associated tags
- **FR-008a**: Tags in content item tag clouds MUST be clickable and navigate to filtered views
- **FR-008b**: Clicking a tag MUST set that tag as active in the tag dropdown filter
- **FR-009**: Homepage and section pages MUST display popular tag clouds
- **FR-009a**: Popular tags MUST be calculated based on last 3 months of content
- **FR-009b**: Popular tag clouds MUST show relative size based on usage frequency
- **FR-009c**: Each section MUST calculate its own popular tags (not global)
- **FR-009d**: Collection pages MUST NOT show popular tag clouds (only homepage/sections)
- **FR-007**: System MUST update the URL query parameters when filters change (e.g., `?tags=ai,azure&date=last-30-days`)
- **FR-008**: System MUST parse URL parameters on page load and apply corresponding filters automatically
- **FR-009**: System MUST provide a "Clear All Filters" button that resets all filters and updates URL
- **FR-010**: System MUST display result count (e.g., "Showing 42 results") when filters are active
- **FR-011**: System MUST show "No results found" message when filters yield zero items
- **FR-012**: System MUST preserve filter state when navigating via browser back/forward buttons
- **FR-013**: Filter operations MUST NOT cause full page reload (client-side filtering only)
- **FR-014**: Filtered content list MUST update within 50ms of filter selection
- **FR-015**: All filter controls MUST be keyboard accessible (Tab, Space/Enter for selection)

### Key Entities

- **SidebarTags**: Component that displays available tags and manages selection state
- **TagFilter**: Represents selected tags with OR logic (match any tag)
- **DateRangeFilter**: Represents selected date range with preset options
- **FilterState**: Combined state of tag filters, date range, and URL parameters
- **ContentItem**: Item being filtered (has tags, publication date, and other metadata)

## Success Criteria

### Measurable Outcomes

- **SC-001**: Users can apply tag filters and see results update within 50ms (no perceived lag)
- **SC-002**: Users can combine tag and date filters with accurate results (AND logic between filter types)
- **SC-003**: Filter state is shareable via URL (copy URL, share, recipient sees same filters)
- **SC-004**: All filter controls are keyboard accessible (100% Tab/Space/Enter navigation)
- **SC-005**: Browser back/forward navigation works correctly with filter state
- **SC-006**: Zero console errors or warnings during filter operations
- **SC-007**: Filter UI provides clear feedback on active state (visual highlighting, badges, result counts)
- **SC-008**: "No results found" scenario provides clear message and recovery option (clear filters button)
- **SC-009**: Tag subset matching works correctly (selecting "AI" shows all "*AI*" tagged content)
- **SC-010**: Date range slider provides smooth interaction (drag handles, visual feedback, date display)
- **SC-011**: Custom date ranges via slider are shareable via URL and restore correctly on page load
- **SC-012**: Date slider defaults to last 3 months, preventing information overload on initial page load
- **SC-013**: Excel-style tag dropdown provides intuitive filtering (search works, checkboxes function, counts display)
- **SC-014**: Tag clouds on content items enable quick navigation to related content
- **SC-015**: Popular tag clouds accurately reflect trending topics from last 3 months
- **SC-016**: Tag dropdown handles large tag lists efficiently (virtual scrolling, search performance)

## Implementation Notes

### Reference Documentation

- [src/TechHub.Web/AGENTS.md](/src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [docs/toc-component.md](/docs/toc-component.md) - Sidebar component architecture reference

### Documentation Requirements

**🚨 CRITICAL - COMPLETE REWRITE REQUIRED**: The file `docs/filtering-system.md` has been deleted as it contained outdated Jekyll/Liquid implementation details. After implementing this feature in .NET/Blazor, you MUST create a NEW comprehensive functional documentation file covering:

**Required Content for `docs/filtering-system.md`**:

- **Filter Behavior**: Tag filtering (OR logic within tags, AND between filter types)
- **Subset Matching Logic**: How tag subset matching works with complete word boundaries - selecting "AI" shows all content tagged with "AI", "Generative AI", "Azure AI", etc.
- **Date Range Slider**: User interaction patterns, URL parameter format, preset vs custom ranges, slider handle interactions
- **Filter Combination Rules**: How different filter types interact (tags + dates + search = AND logic)
- **URL State Management**: Parameter format (`tags`, `from`, `to`, `date`), encoding rules, bookmarking, sharing filtered views
- **Performance Characteristics**: Client-side filtering performance, debouncing strategies, optimization techniques
- **Edge Cases**: Zero results handling, invalid parameters, browser compatibility, mobile interactions
- **Visual Feedback**: Active filter indicators, result counts, loading states, empty states

**Guidelines for Functional Documentation**:

- ✅ **Focus on WHAT the system does** - Describe behavior, not implementation
- ✅ **Framework-agnostic language** - Must survive tech stack changes
- ✅ **User-facing behavior** - How users experience the filtering system
- ✅ **Complete specifications** - Filter logic, URL formats, interaction patterns
- 🚫 **NO implementation code** - No C#, JavaScript, Razor examples
- 🚫 **NO development instructions** - No "how to build this" guidance
- 🚫 **NO framework-specific details** - No Blazor components, API endpoints, etc.

**Technical Documentation** (implementation details belong here):

- [src/TechHub.Web/AGENTS.md](/src/TechHub.Web/AGENTS.md) - Blazor component architecture, state management patterns, client-side filtering implementation
- [src/TechHub.Api/AGENTS.md](/src/TechHub.Api/AGENTS.md) - API endpoint patterns for filtering, query parameter handling
- [tests/TechHub.E2E.Tests/AGENTS.md](/tests/TechHub.E2E.Tests/AGENTS.md) - E2E testing patterns for filters, test automation strategies

### Current Status

**API Endpoints (COMPLETE)**:

- `GET /api/content/filter` - Supports tag and date filtering with query parameters
- Tested and working with 6 integration tests passing

**Frontend Components (PLACEHOLDER)**:

- `SidebarTags.razor` exists but is non-functional
- Needs complete rewrite to support tag selection, URL parameters, and state management

### Component Architecture

**TagDropdownFilter Component** (Excel-style dropdown):

- Positioned below date range slider in sidebar
- Dropdown button shows "Filter by Tags" with count of selected tags
- Opens to reveal searchable tag list with checkboxes
- Search box filters available tags in real-time
- Displays tag counts next to each tag (e.g., "AI (42)")
- Virtual scrolling for large tag lists (100+ tags)
- Multi-select with checkboxes (OR logic)
- "Select All" / "Clear All" quick actions
- Updates URL parameters when selections change
- Reads URL parameters on load to restore checked state
- Keyboard accessible (Tab, Space for checkbox, Arrow keys for navigation)
- Emits events when tag selection changes

**Date Range Slider Component** (separate component for complex interaction):

- Interactive slider with two draggable handles (from and to dates)
- **Defaults to last 3 months** when no URL parameters present
- Visual date range display (e.g., "Oct 16, 2025 - Jan 16, 2026")
- Allows extending range backward to show all historical content (no limit)
- Optional preset buttons (Last 7/30/90 days) for quick access
- Updates URL parameters when range changes (debounced to prevent excessive updates)
- Reads URL parameters on load to restore slider position
- Emits events when date range changes
- Accessible keyboard controls (arrow keys to adjust, tab navigation)
- Touch-friendly handles for mobile devices
- Integrates with infinite scroll (only loads content within selected range)

**Content List Component** (existing, needs filter integration):

- Receives filter state from parent
- Calls API with filter parameters
- Updates displayed content based on API response
- Shows loading state during filter operations
- Shows result count and "no results" message
- Integrates with infinite scroll to load more content within date range

**ContentItemTagCloud Component**:

- Displays all tags associated with a content item
- Renders tags as clickable pills/badges
- Each tag navigates to filtered view when clicked
- Consistent styling across all content item types
- Responsive layout (wraps or scrolls as needed for many tags)

**PopularTagCloud Component**:

- Displays on homepage and section pages ONLY (not collection pages)
- Calculates popular tags from last 3 months of content
- Tag sizes reflect relative popularity (visual weight)
- Each tag clickable, navigates to filtered view
- Shows tooltip with count on hover
- Section-specific calculations (not global)
- Hides if no content in last 3 months

### URL Parameter Format

**Query Parameters**:

- `tags` - Comma-separated tag names (e.g., `tags=ai,azure,copilot`)
- `from` - Start date for custom range (e.g., `from=2024-10-16`)
- `to` - End date for custom range (e.g., `to=2026-01-16`)
- `date` - Preset date range identifier (e.g., `date=last-30-days`) - optional, used when presets clicked
- **Default behavior**: If no date parameters present, defaults to last 3 months
- Example custom range: `/azure/all?tags=security,devops&from=2024-10-16&to=2026-01-16`
- Example preset: `/azure/all?tags=security,devops&date=last-90-days`
- Example tag from content item: `/ai?tags=github-copilot` (defaults to last 3 months)
- Example tag from popular cloud: `/ai?tags=azure` (defaults to last 3 months)

**URL Encoding**:

- Tag names with spaces: URL encode (e.g., `GitHub%20Copilot`)
- Multiple tags: Comma separator, no spaces
- Invalid parameters: Silently ignore, log warning

### Filter Logic

**Tag Subset Matching**:

- User selects "AI" tag
- System matches items with exact tag "AI" OR tags containing "AI" as complete word
- Examples: "AI", "Generative AI", "Azure AI", "AI Agents" (NOT "AIR" or "PAIR")
- Implementation: Pre-calculated tag relationships for performance

**Tag Filtering (OR with Subset Matching)**:

- User selects ["ai", "azure"]
- Show items with tag "ai" OR tag "azure" (including subset matches)
- With subset: Also shows "Generative AI", "Azure DevOps", etc.
- SQL equivalent: `WHERE tag IN ('ai', 'azure', 'generative ai', 'azure devops', ...)`

**Date Range Filtering (AND)**:

- User selects "Last 30 days" AND tags ["ai"]
- Show items published in last 30 days AND has tag "ai"
- SQL equivalent: `WHERE publishedDate >= @30DaysAgo AND tag = 'ai'`

**Collection Filtering (AND)**:

- User is on `/azure/blogs` (collection = blogs)
- User selects tag "security"
- Show items in "blogs" collection AND section "azure" AND tag "security"
- SQL equivalent: `WHERE collection = 'blogs' AND section = 'azure' AND tag = 'security'`

### Testing Strategy

**Component Tests** (bUnit):

- TagDropdownFilter renders with button and dropdown list
- Search box filters tag list correctly
- Checkbox selection updates state
- Tag counts display correctly (e.g., "AI (42)")
- Virtual scrolling works for large tag lists
- Date slider defaults to last 3 months on initial load
- Clear All button resets state
- ContentItemTagCloud renders all item tags
- PopularTagCloud calculates tags from last 3 months
- PopularTagCloud shows section-specific tags (not global)

**Integration Tests** (API already tested):

- Filter endpoint returns correct results for tag filtering
- Filter endpoint returns correct results for date filtering
- Filter endpoint combines filters correctly

**E2E Tests** (Playwright):

- Navigate to section page, verify date slider defaults to last 3 months
- Open tag dropdown below date slider, verify it displays all tags with counts
- Search in tag dropdown, verify filtering works
- Select tags via checkboxes, verify content updates with OR logic
- Verify tag subset matching (select "AI", confirm "Generative AI" content appears)
- Use date range slider to set custom range, verify content filters correctly
- Drag slider backward to extend to older content, verify no limit
- Drag slider handles, verify smooth interaction and debounced updates
- Test preset date buttons if implemented
- Select date range + tags via dropdown, verify AND logic
- Verify URL updates with filter parameters (tags, from, to, date)
- Click tag in content item tag cloud, verify navigation to filtered view
- Verify tag dropdown shows clicked tag as selected after navigation
- View homepage, verify popular tag cloud displays (last 3 months)
- View section page, verify section-specific popular tags
- Click popular tag, verify navigation to filtered view
- View collection page, verify NO popular tag cloud displays
- Copy filtered URL with custom date range, open in new tab, verify slider restores correctly
- Browser back/forward navigation with filters and slider state
- Keyboard navigation through tag dropdown (Tab, Space, Arrow keys)
- Touch interaction with slider handles on mobile
- Scroll down with infinite scroll, verify content loads only within date range
- "No results" scenario with clear filters recovery

**Accessibility Tests**:

- Keyboard-only navigation (Tab through tags, Space/Enter to select)
- Screen reader announces tag selection state
- ARIA labels for filter controls
- Focus states clearly visible

## Dependencies

- **Completed**: API filtering endpoints (`GET /api/content/filter`)
- **Completed**: Content repository with tag and date filtering logic
- **In Progress**: Blazor component architecture for state management
- **Needed**: URL state management library or custom implementation
- **Needed**: Visual design for Excel-style tag dropdown
- **Needed**: Popular tag calculation logic (most-used tags in last 3 months)
- **Needed**: Virtual scrolling component/library for large tag lists
- **Needed**: Tag cloud visual design (content items vs popular tags)
- **Needed**: Infinite scroll integration (003-infinite-scroll spec)

## Out of Scope

- Text search filtering (covered in 002-search spec)
- Infinite scroll pagination (covered in 003-infinite-scroll spec)
- Advanced filtering (multi-select with AND logic, exclude filters, etc.)
- Filter analytics tracking (covered in 006-google-analytics spec)
- Saved filter presets or user preferences
- Tag popularity indicators or sorting
