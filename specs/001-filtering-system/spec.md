# Feature Specification: Sidebar Content Filtering

**Feature Branch**: `001-filtering-system`  
**Created**: 2026-01-16  
**Updated**: 2026-02-03  
**Status**: In Progress (Partial Implementation)  
**Input**: Implement sidebar-based tag and date filtering for section and collection pages to help users discover relevant content quickly

## Implementation Status

| Component | Status | Location |
|-----------|--------|----------|
| **Backend Infrastructure** | | |
| TagCloudItem, TagCloudRequest | ✅ Complete | `src/TechHub.Core/Models/Tags/` |
| FilterRequest, FilterResponse, FilterSummary | ✅ Complete | `src/TechHub.Core/Models/Filter/` |
| AllTagsResponse, TagWithCount | ✅ Complete | `src/TechHub.Core/Models/Tags/` |
| FacetRequest, FacetResults | ✅ Complete | `src/TechHub.Core/Models/Facets/` |
| ITagCloudService, TagCloudService | ✅ Complete | `src/TechHub.Core/Interfaces/`, `src/TechHub.Infrastructure/Services/` |
| GetFacetsAsync (repository) | ✅ Complete | Fully implemented with filtering support |
| GetTagCountsAsync (repository) | ✅ Complete | With date/section/collection filtering + caching |
| Tag cloud endpoint | ⚠️ Partial | Exists, needs filter params (tags, from, to) |
| **Frontend Components** | | |
| SidebarTagCloud component | ⚠️ Partial | Basic filtering complete, needs dynamic counts |
| SidebarTagCloudTests | ✅ Complete | `tests/TechHub.Web.Tests/Components/` |
| DateRangeSlider component | ❌ Not Started | - |
| TagDropdownFilter component | ❌ Not Started | - |
| FilterStateService | ❌ Not Started | - |
| **Testing** | | |
| TagFilteringTests (E2E) | ⚠️ Partial | Basic tag filtering complete, dynamic counts not tested |
| **Dynamic Counts Feature (NEW)** | | |
| Tag counts with filter parameters | ❌ Not Started | Add params to existing endpoint |
| Tag disabled state (0 results) | ❌ Not Started | Frontend logic (count === 0) |
| Date range affects tag counts | ❌ Not Started | Integration with date slider |
| **Tag Subset Matching** | | |
| Tag subset matching (word boundaries) | ✅ Complete | Via `content_tags_expanded` table in DB |

## Clarifications

### Session 2026-02-03 (New Requirements)

- Q: Should tag counts be static or dynamic based on current filter state? → A: **Dynamic counts** - Each tag displays how many content items would remain if that tag is activated (e.g., "AI (901)"). When a tag is already selected, other tags update their counts to show the intersection. Example: if "AI" (901) is selected, "GitHub Copilot" might show (300) indicating 300 items have both tags.
- Q: What happens to tags that would result in 0 items? → A: Tags that would bring total results to 0 MUST become **disabled** (grayed out, non-clickable). This prevents users from creating empty result sets.
- Q: Should the date slider affect tag cloud calculations? → A: **Yes** - Tag counts and availability must reflect the selected date range. If user selects "Last 30 days", tag counts should only include content from that period.
- Q: Excel-style dropdown behavior? → A: Dropdown allows scrolling and searching through ALL tags with their counts. Same dynamic count behavior applies - counts update based on current filter state.

### Session 2026-01-16

- Q: Tag Selection Interface Architecture - Are sidebar tag clicks and Excel dropdown two separate interfaces or one unified interface? → A: Both interfaces - (1) Sidebar tag cloud showing top 20 most-used tags (scoped to homepage/section/collection/content context), AND (2) Excel-style dropdown for finding specific tags not in top 20. Both work together, synchronized state, complementary purposes.
- Q: Tag Cloud Sizing Algorithm - How should tag sizes reflect relative popularity (linear, logarithmic, quantile-based, fixed with color)? → A: Quantile-based with 3 size tiers - Divide top 20 into Large (top 25% = 5 tags), Medium (middle 50% = 10 tags), Small (bottom 25% = 5 tags). Prevents excessively large tags while maintaining clear visual hierarchy.
- Q: Popular Tag Display Quantity Threshold - Should tag cloud always show exactly 20 tags, or adjust based on content volume and popularity? → A: Dynamic quantity with minimum threshold - Show top 20 OR all tags with ≥5 uses, whichever is fewer. Prevents displaying rarely-used tags in small sections while capping maximum display size.
- Q: Section/Collection Names in Tags - The content processing pipeline was adding section and collection names (e.g., "AI", "GitHub Copilot", "Blogs", "Videos") as tags to every content item for Jekyll static filtering. Should these remain in the tag cloud? → A: Stop ADDING them automatically, but keep them if AI assigned them. The old pipeline added these to EVERY item which cluttered tag clouds. **Action**: (1) Update ContentFixer to remove these auto-added tags from existing content (one-time cleanup), (2) Update content-processing pipeline to stop automatically adding categories/collection to tags, (3) Do NOT filter out section names if they're legitimately in the AI's tag response.

## User Scenarios & Testing

### User Story 1 - Filter via Sidebar Tag Cloud (Priority: P1)

Users can click tags in the sidebar tag cloud (showing top 20 most-used tags) to filter content, with dynamic counts showing how many items each tag would filter to.

**Why this priority**: Primary content discovery mechanism, surfaces trending/popular content, dynamic counts prevent empty result sets.

**Status**: ⚠️ Partial - Basic tag filtering ✅ complete (URL params, selection, OR logic), Dynamic counts ❌ pending

**Independent Test**: Navigate to section page, verify tag cloud shows top 20 tags with counts, click tag, verify counts update for remaining tags.

**Acceptance Scenarios**:

1. **Given** I'm on a section page (e.g., /ai), **When** I view the sidebar, **Then** I see top 20 tags for that section with item counts (e.g., "AI (901)")
2. **Given** I'm viewing tags in the cloud, **When** I examine any tag, **Then** it displays the count of items that would be shown if I select it (e.g., "GitHub Copilot (300)")
3. **Given** I select the "AI" tag (901 items), **When** other tags update, **Then** "GitHub Copilot" now shows the intersection count (e.g., "GitHub Copilot (245)" - items with BOTH tags)
4. **Given** I have "AI" selected, **When** a tag would result in 0 items, **Then** that tag becomes disabled (grayed out, non-clickable)
5. **Given** I'm on the homepage, **When** I view the sidebar, **Then** I see a tag cloud with top 20 most-used tags across entire website with counts
6. **Given** I'm on a collection page (e.g., /ai/news), **When** I view the sidebar, **Then** I see top 20 tags for that section/collection combo with counts
7. **Given** I'm viewing a content item, **When** I view the sidebar, **Then** I see ONLY that article's tags (no counts needed)
8. **Given** I click a tag in the sidebar cloud, **When** the filter applies, **Then** the content list updates AND all other tag counts recalculate
9. **Given** I have one tag selected, **When** I click another tag in the cloud, **Then** the content list shows items matching ANY of the selected tags (OR logic) and counts update
10. **Given** I have tags selected, **When** I click a selected tag again, **Then** it toggles off, content updates, and all tag counts recalculate
11. **Given** I click a tag to select it, **When** the tag becomes active, **Then** it displays a visual indicator (highlighted background/border) showing it's selected
12. **Given** I filter by tags, **When** I check the URL, **Then** it includes my selected tags as query parameters (e.g., `?tags=ai,azure`) with no duplicates
13. **Given** I share the URL with tags, **When** someone opens it, **Then** they see the same filtered view with the same tags selected and counts reflecting that state

**Tag Click Navigation Behavior**:

When clicking a tag, the navigation behavior depends on the current page context:

| Current Page                                     | Navigation Behavior                                                              | Example                                                   |
| ------------------------------------------------ | -------------------------------------------------------------------------------- | --------------------------------------------------------- |
| Homepage (`/`)                                   | Navigate to `/all?tags={tag}`                                                    | Click "AI" → `/all?tags=ai`                               |
| Section page (`/github-copilot`)                 | Stay on section, add filter: `/{sectionName}?tags={tag}`                             | Click "VS Code" → `/github-copilot?tags=vs-code`          |
| Collection page (`/github-copilot/videos`)       | Stay on collection, add filter: `/{sectionName}/{collectionName}?tags={tag}`             | Click "Tutorial" → `/github-copilot/videos?tags=tutorial` |
| Content item (`/github-copilot/videos/my-video`) | Navigate to content's primarySection with filter: `/{primarySection}?tags={tag}` | Click "AI" → `/github-copilot?tags=ai`                    |

**Rationale**:

- Homepage has no filtering context, so redirect to `/all` (shows all content with filter)
- Section/Collection pages already have context, so filter in place
- Content items don't have a content list to filter, so navigate to the item's primary section

---

### User Story 2 - Filter Content by Date Range (Priority: P1)

Users can select date ranges using an interactive slider to filter content by publication date. Date selection affects tag cloud counts.

**Why this priority**: Essential for finding recent content, directly affects tag availability and counts.

**Status**: ❌ Not started

**Independent Test**: Navigate to section page, use date slider, verify tag counts update based on selected date range.

**Acceptance Scenarios**:

1. **Given** I'm on a section page, **When** I view the date slider, **Then** it defaults to "last 3 months" (90 days)
2. **Given** I adjust the date slider to "Last 30 days", **When** the filter applies, **Then** the content list shows only items from the past 30 days
3. **Given** I change the date range, **When** tags update, **Then** all tag counts recalculate based on the new date range
4. **Given** I select "Last 7 days", **When** a tag has no items in that period, **Then** that tag becomes disabled (grayed out)
5. **Given** I have date and tag filters active, **When** I view results, **Then** I see items matching BOTH filters (AND logic)
6. **Given** I drag the slider handles, **When** I set a custom range, **Then** URL includes `from` and `to` date parameters
7. **Given** I extend the date range backward, **When** more content becomes available, **Then** tag counts increase and disabled tags may become enabled

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
**Status**: ✅ Complete (database layer) - Implemented via `content_tags_expanded` table with word-boundary matching
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

Users can filter content by tags using a modern dropdown interface similar to Excel column filtering, with scrollable list, search, and dynamic counts that update based on current filter state.

**Why this priority**: Provides access to ALL tags, intuitive Excel-like filtering, dynamic counts prevent empty results.

**Status**: ❌ Not started

**Independent Test**: Open tag dropdown, verify counts shown, select a tag, verify other tag counts update dynamically.

**Acceptance Scenarios**:

1. **Given** I'm on a section/collection page, **When** I view the sidebar, **Then** I see a "Filter by Tags" dropdown positioned below the date slider
2. **Given** I click the tag dropdown, **When** it opens, **Then** I see a scrollable, searchable list of ALL tags with checkboxes and counts
3. **Given** I view tags in the dropdown, **When** I examine any tag, **Then** it displays the dynamic count (e.g., "AI (901)")
4. **Given** I select "AI" checkbox (901 items), **When** other tags update, **Then** their counts show the intersection (e.g., "GitHub Copilot (245)")
5. **Given** I have tags selected, **When** a tag would result in 0 items, **Then** that tag row becomes disabled (grayed out, checkbox disabled)
6. **Given** the dropdown is open, **When** I type in the search box, **Then** the tag list filters to show only matching tags with their counts
7. **Given** I select multiple tags, **When** filters apply, **Then** content shows items matching ANY selected tag (OR logic) and counts update
8. **Given** I scroll through the dropdown, **When** there are 50+ tags, **Then** virtual scrolling provides smooth performance
9. **Given** the date range changes, **When** I view the dropdown, **Then** all tag counts reflect the new date range
10. **Given** the dropdown is open, **When** I click outside or press Escape, **Then** the dropdown closes and filters persist

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

### User Story 10 - Sidebar Tag Cloud Scoping & Display (Priority: P1)

Users see contextually-scoped tag clouds in the sidebar showing top 20 most-used tags, with appropriate scoping based on page type.

**Why this priority**: Core navigation feature, surfaces trending content, adapts to user's browsing context.

**Status**: ✅ Basic scoping complete, ❌ Dynamic counts pending

**Independent Test**: Navigate between homepage/section/collection/content pages, verify tag cloud scoping changes appropriately.

**Acceptance Scenarios**:

1. **Given** I'm on the homepage, **When** I view the sidebar, **Then** I see top 20 tags across entire website (based on current date range)
2. **Given** I'm on a section page (e.g., /ai), **When** I view the sidebar, **Then** I see top 20 tags for that section only (based on current date range)
3. **Given** I'm on a collection page (e.g., /ai/news), **When** I view the sidebar, **Then** I see top 20 tags for that section/collection combo (based on current date range)
4. **Given** I'm viewing a content item, **When** I view the sidebar, **Then** I see ONLY that article's tags (no counts, not limited to date range)
5. **Given** I view the tag cloud, **When** I examine the display, **Then** tag sizes use 3 discrete tiers (Large/Medium/Small based on quartiles, not continuous scaling)
6. **Given** I click a tag in the cloud, **When** navigation occurs, **Then** I see content filtered by that tag
7. **Given** I hover over a tag, **When** I wait briefly, **Then** the count is always visible inline (e.g., "AI (901)")
8. **Given** there are fewer than 20 qualifying tags with ≥5 uses, **When** I view the cloud, **Then** I see all tags with ≥5 uses (may be fewer than 20)

---

### User Story 11 - Dynamic Tag Counts Based on Filter State (Priority: P1)

Tag counts throughout the UI dynamically update based on current filter state (date range and selected tags), showing users exactly how many items each tag would filter to.

**Why this priority**: Prevents empty result sets, gives users confidence in filter choices, improves UX.

**Status**: ❌ Not started

**Independent Test**: Select a tag, verify all other tags update their counts to show intersection. Change date range, verify counts recalculate.

**Acceptance Scenarios**:

1. **Given** no filters are active, **When** I view tag counts, **Then** each shows total items with that tag (within date range)
2. **Given** I select "AI" tag (shows 901 items), **When** I view "GitHub Copilot" tag, **Then** it shows intersection count (e.g., 245 items with BOTH tags)
3. **Given** I have "AI" selected, **When** "Azure" tag would result in 0 items, **Then** "Azure" tag becomes disabled (grayed, non-clickable)
4. **Given** I deselect a tag, **When** counts recalculate, **Then** previously disabled tags may become enabled again
5. **Given** I change date range to "Last 7 days", **When** tags update, **Then** counts reflect only items within that period
6. **Given** I'm in dropdown and sidebar cloud, **When** counts update, **Then** both views show synchronized counts
7. **Given** API returns counts, **When** user sees UI, **Then** counts are formatted with commas for large numbers (e.g., "1,234")
8. **Given** counts are calculating, **When** user waits, **Then** a subtle loading indicator shows (not full page block)

---

### Edge Cases

- What happens when user selects tags that yield zero results? → Show "No results found" message with "Clear All Filters" button
- What happens when URL has invalid date range parameter? → Fallback to "last 3 months" default, log error
- What happens when no date range is specified in URL? → Default to last 3 months
- What happens when tag dropdown has 50+ tags? → Implement virtual scrolling at 50 tag threshold, show top 100 in viewport, require search to efficiently find specific tags
- What happens when user searches in tag dropdown with no matches? → Show "No tags found" message, allow clearing search
- What happens when there are no tags with items in selected date range? → Show message "No tags available for this period" and suggest expanding date range
- What happens when content item has many tags (e.g., 20+)? → Show all tags, allow horizontal scroll or wrapping as needed
- What happens when user clicks tag on content item vs popular tag cloud? → Both navigate to same filtered view, same behavior
- What happens when URL has invalid tag names? → Ignore invalid tags, apply only valid ones, log warning
- What happens when tag matching encounters different casing/punctuation? → Normalize both search term and tags (trim, lowercase, punctuation-agnostic) before word boundary matching
- What happens when user rapidly clicks multiple tags? → Debounce UI updates, only apply final state, batch count recalculations
- What happens on slow connections? → Show loading indicator on counts, don't block tag interaction, allow filter changes during load
- What happens when content is already filtered by collection via SubNav? → Combine collection filter with sidebar filters (AND logic)
- What happens when a tag would result in 0 items if selected? → Tag becomes **disabled** (grayed out, non-clickable, count shows "(0)")
- What happens when all remaining tags would result in 0 items? → Show message "No additional tags available" below tag cloud
- What happens when tag counts are loading after filter change? → Show subtle spinner/shimmer on count numbers only, keep tags interactive
- What happens when date range is set to "All Time" but no old content exists? → Counts simply reflect available content, no special handling needed

## Requirements

### Functional Requirements

**Tag Cloud & Display**:

- **FR-001**: Sidebar MUST display a tag cloud with top 20 most-used tags, scoped to current context
- **FR-001a**: Homepage tag cloud MUST show top 20 tags across entire website
- **FR-001b**: Section page tag cloud MUST show top 20 tags for that section only
- **FR-001c**: Collection page tag cloud MUST show top 20 tags for that section/collection combo
- **FR-001d**: Sidebar tag cloud on content item pages MUST show ONLY that item's tags (no counts)
- **FR-002**: Sidebar tag cloud tags MUST be clickable and toggle between selected/unselected states
- **FR-003**: Selected tags MUST be visually distinguished (highlighted, different color, checkmark icon)
- **FR-004**: System MUST filter content to show items matching ANY selected tag (OR logic within tags)

**Dynamic Tag Counts (NEW)**:

- **FR-020**: Each tag MUST display a count showing how many items would remain if selected (e.g., "AI (901)")
- **FR-020a**: When no filters are active, count shows total items with that tag (within date range)
- **FR-020b**: When tags are selected, counts MUST show intersection (items matching existing filters AND the tag)
- **FR-021**: Tags that would result in 0 items MUST become disabled (grayed out, non-clickable)
- **FR-021a**: Disabled tags MUST show "(0)" count to indicate why they're disabled
- **FR-021b**: Disabled tags MUST re-enable when filter state changes and they would yield >0 results
- **FR-022**: Tag counts MUST update within 200ms of filter state change (debounced for rapid changes)
- **FR-022a**: During count calculation, counts MUST show subtle loading state (shimmer/spinner)
- **FR-023**: Counts MUST be formatted with thousand separators (e.g., "1,234" not "1234")

**Date Range Filtering**:

- **FR-005**: System MUST provide date filtering via interactive slider with from-to date range selection
- **FR-005a**: Date slider MUST default to "last 3 months" when no date range specified in URL
- **FR-005b**: System MAY provide preset date range buttons (Last 7/30/90 days) as quick shortcuts alongside slider
- **FR-005c**: Date range changes MUST trigger tag count recalculation
- **FR-006**: Date range filters MUST combine with tag filters using AND logic (must match date AND any tag)

**Tag Subset Matching**:

- **FR-006a**: System MUST implement tag subset matching using case-insensitive word boundaries with punctuation normalization (hyphens, underscores, and spaces treated as word separators before matching; selecting "ai" matches "AI", "Generative AI", "Azure-AI", "Azure_AI")
- **FR-006b**: Subset matching MUST use complete word boundaries after normalization ("ai" matches "Azure AI" but not "AIR")

**Excel-Style Dropdown**:

- **FR-007**: System MUST provide Excel-style tag dropdown filter positioned below date slider
- **FR-007a**: Tag dropdown MUST include search box for filtering tag list
- **FR-007b**: Tag dropdown MUST show checkboxes for multi-select capability
- **FR-007c**: Tag dropdown MUST display dynamic item counts next to each tag (e.g., "AI (901)")
- **FR-007d**: Tag dropdown MUST support virtual scrolling when tag count is ≥50 tags
- **FR-007e**: Tag dropdown MUST disable tags that would result in 0 items
- **FR-007f**: Tag dropdown counts MUST stay synchronized with sidebar tag cloud counts

**Content Item Tags**:

- **FR-008**: Content items MUST display tag clouds showing all associated tags
- **FR-008a**: Tags in content item tag clouds MUST be clickable and navigate to filtered views
- **FR-008b**: Clicking a tag MUST set that tag as active in the tag dropdown filter

**Tag Cloud Sizing**:

- **FR-009**: Sidebar tag clouds MUST display top 20 most-used tags OR all tags with ≥5 uses (whichever is fewer)
- **FR-009a**: Tag cloud calculations MUST be based on the currently selected date range
- **FR-009b**: Tag cloud sizes MUST use quantile-based sizing with 3 tiers: Large (top 25%), Medium (middle 50%), Small (bottom 25%)
- **FR-009c**: Each scope (homepage/section/collection) MUST calculate its own top 20 (not global)
- **FR-009d**: Content item pages MUST show only that item's tags (no top 20 limit, no date range filter)

**URL State Management**:

- **FR-010**: System MUST update the URL query parameters when filters change (e.g., `?tags=ai,azure&from=2024-01-01&to=2024-03-01`)
- **FR-011**: System MUST parse URL parameters on page load and apply corresponding filters automatically
- **FR-012**: System MUST provide a "Clear All Filters" button that resets all filters and updates URL

**Result Display**:

- **FR-013**: System MUST display result count (e.g., "Showing 42 results") when filters are active
- **FR-014**: System MUST show "No results found" message when filters yield zero items
- **FR-015**: System MUST preserve filter state when navigating via browser back/forward buttons

**Performance**:

- **FR-016**: Filter operations MUST NOT cause full page reload (client-side filtering only)
- **FR-017**: Client-side filtering MUST update content list within 50ms after API response received
- **FR-018**: Tag count API MUST respond within 200ms for count calculations

**Accessibility**:

- **FR-019**: All filter controls MUST be keyboard accessible with specific interactions:
  - Tab: Focus navigation between controls
  - Space/Enter: Select/deselect tags, activate buttons
  - Arrow keys: Navigate within dropdown lists
  - Escape: Close dropdowns and modals

### Key Entities

- **SidebarTagCloud**: Component that displays available tags with size-based rendering and manages selection state
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
- **SC-015**: Popular tag clouds accurately reflect trending topics from selected date range
- **SC-016**: Tag dropdown handles large tag lists efficiently (virtual scrolling, search performance)

**Dynamic Count Success Criteria (NEW)**:

- **SC-017**: Tag counts update within 200ms of filter state change
- **SC-018**: Tags with 0 potential results are visually disabled (cannot be clicked)
- **SC-019**: Selecting a tag updates all other tag counts to show intersection counts
- **SC-020**: Changing date range recalculates all tag counts correctly
- **SC-021**: Sidebar tag cloud and dropdown show synchronized counts
- **SC-022**: Counts are formatted with thousand separators (e.g., "1,234")
- **SC-023**: Loading state is shown during count calculations (subtle, non-blocking)

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

- `SidebarTagCloud.razor` implemented with quantile-based sizing and interactive filtering
- Needs complete rewrite to support tag selection, URL parameters, and state management

### Component Architecture

**SidebarTagCloud Component** (contextually-scoped tag cloud):

- ✅ **Implemented** in `src/TechHub.Web/Components/SidebarTagCloud.razor`
- ✅ Displays in sidebar
- ✅ Shows top 20 most-used tags OR all tags with ≥5 uses (whichever is fewer)
- ✅ Scoping logic implemented:
  - Homepage: Top 20 across entire website
  - Section page: Top 20 for that section
  - Collection page: Top 20 for that section/collection combo
  - Content item page: Only that item's tags (no limits, no counts)
- ✅ Tags are clickable and toggleable (select/deselect)
- ✅ Tag sizes reflect relative popularity using quantile-based sizing:
  - Large (top 25%): 5 tags - Most popular
  - Medium (middle 50%): 10 tags - Popular
  - Small (bottom 25%): 5 tags - Less popular
- ✅ Selected tags visually highlighted
- ✅ Updates URL parameters when tags selected
- ✅ Reads URL parameters on load to restore selected state
- ✅ Navigation modes: Filter (update in-place) vs Navigate (go to different URL)
- ❌ **TODO: Dynamic counts** - Show count for each tag (e.g., "AI (901)")
- ❌ **TODO: Counts update when filters change** (show intersection with selected tags)
- ❌ **TODO: Disabled state** for tags with 0 potential results (grayed out, non-clickable)
- ❌ **TODO: Date range integration** - counts update when date range changes

**TagDropdownFilter Component** (Excel-style dropdown - COMPLEMENTARY to cloud):

- ❌ Not yet implemented
- Positioned below date range slider in sidebar
- Provides access to ALL tags (not just top 20 in cloud)
- Dropdown button shows "Filter by Tags" with count of selected tags
- Opens to reveal scrollable, searchable tag list with checkboxes
- **NEW: Each tag shows dynamic count (e.g., "AI (901)")**
- **NEW: Counts update when filters change (shows intersection)**
- **NEW: Tags with 0 potential results are disabled (grayed row, disabled checkbox)**
- Search box filters available tags in real-time
- Virtual scrolling activates automatically at ≥50 tags for performance
- Multi-select with checkboxes (OR logic)
- "Select All" / "Clear All" quick actions
- Synchronizes with sidebar tag cloud selections (same filter state)
- Updates URL parameters when selections change
- Reads URL parameters on load to restore checked state
- Keyboard accessible (Tab, Space for checkbox, Arrow keys for navigation)
- Emits events when tag selection changes

**DateRangeSlider Component** (separate component for complex interaction):

- ❌ Not yet implemented
- Interactive slider with two draggable handles (from and to dates)
- **Defaults to last 90 days (3 months)** when no URL parameters present
- Visual date range display (e.g., "Oct 16, 2025 - Jan 16, 2026")
- **NEW: Date changes trigger tag count recalculation** (calls tagcounts API)
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

**ContentItemTagBadges Component** (used ON content item cards in lists):

- Displays on content item cards/previews in lists (not in sidebar)
- Shows subset of item's tags as small pills/badges (e.g., first 3-5 tags)
- Tags are clickable and navigate to filtered view
- Clicking tag sets it as active in both sidebar cloud AND dropdown
- Consistent compact styling across all content item types
- May show "+2 more" indicator if item has many tags

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
- PopularTagCloud calculates tags from last 3 months with minimum threshold (≥5 uses)
- PopularTagCloud shows section-specific tags (not global), maximum 20 or all ≥5 uses

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

## Backend Implementation Status

### Database Layer (✅ COMPLETE from spec 011)

The database infrastructure for filtering is **fully implemented** in the repository layer:

**✅ Implemented**:

- Tag filtering with AND logic using `SearchAsync` method
- Tag subset matching via `content_tags_expanded` table (word-boundary matching)
- Section/collection/date filtering
- Hash-based incremental content sync
- Both SQLite (FTS5) and PostgreSQL (tsvector) providers
- `GetTagCountsAsync` with date range, section, collection filtering, caching
- 90+ passing repository integration tests

**✅ Implemented**:

- `GetFacetsAsync` repository method is **fully implemented** in FileBasedContentRepository and database repositories
- Calculates facet counts with filtering support (tags, sections, collections, date ranges)
- Used by `SearchAsync` when `IncludeFacets = true`
- `GetTagCountsAsync` is complete with date/section/collection filtering and caching

**❌ Missing: Filter Parameters on Tag Cloud Endpoint**:

- Tag cloud endpoint exists: `GET /api/sections/{section}/collections/{collection}/tags`
- Currently accepts: `maxTags`, `minUses`, `lastDays`
- **Need to add**: `tags` (selected), `from`, `to` query parameters
- Then pass these filters to `GetTagCountsAsync` (which already supports them!)

**Simple Enhancement Required**:

```http
# Current (works for static counts)
GET /api/sections/{section}/collections/{collection}/tags?maxTags=20&lastDays=90

# Enhanced (for dynamic counts with filter state)
GET /api/sections/{section}/collections/{collection}/tags?maxTags=20&tags=ai&from=2025-11-03&to=2026-02-03

Response (TagCloudItem[] - already has counts!):
[
  { "tag": "AI", "count": 901, "size": "Large" },
  { "tag": "GitHub Copilot", "count": 245, "size": "Medium" },
  { "tag": "Azure", "count": 0, "size": "Small" }
]
```

**Count Calculation Logic** (already in `GetTagCountsAsync`):

- When **no tags param**: Count = total items with that tag (within date range)
- When **tags param provided**: Count = items matching **existing tags** OR **this tag** (OR logic)
- Frontend determines disabled state: `disabled = (count === 0)`

**Performance Targets** (from spec 011):

- Tag filtering < 200ms (4000+ content items)
- **Tag count calculation < 200ms** (new requirement for dynamic counts)
- Facet counts 100% accurate using SQL GROUP BY aggregations
- First sync < 60 seconds, subsequent sync < 1 second

**Action Items for This Spec**:

1. ✅ Backend filtering logic complete (`SearchAsync` implemented)
2. ✅ Tag subset matching complete (`content_tags_expanded` table)
3. ✅ `GetTagCountsAsync` complete with filtering support
4. ✅ `GetFacetsAsync` fully implemented
5. ❌ Enhance existing tag cloud endpoint to accept filter parameters (tags, from, to)
6. ❌ Build frontend components to consume enhanced API with dynamic counts
7. ❌ Add E2E tests for dynamic count behavior

## Dependencies

- **Completed**: Database schema and repository layer (spec 011)
- **Completed**: Content sync with hash-based incremental updates (spec 011)
- **🚨 BLOCKER - Implement First**: Infinite scroll pagination (spec 003) - **MUST complete before dynamic tag counts**
  - **Why**: Tag counts showing "AI (901)" are misleading if users can only see 20 items
  - **Impact**: Without infinite scroll, users cannot browse all filtered content
  - **Recommendation**: Implement 003-infinite-scroll before continuing with dynamic counts feature
- **Needs Implementation**: Enhanced tag cloud endpoint with filter parameters (add to existing endpoint)
- **In Progress**: Blazor component architecture for state management
- **Needed**: URL state management library or custom implementation
- **Needed**: Visual design for Excel-style tag dropdown
- **Needed**: Virtual scrolling component/library for large tag lists
- **Needed**: Tag cloud visual design (content items vs popular tags)

## Out of Scope

- Text search filtering (covered in 002-search spec)
- Infinite scroll pagination (covered in 003-infinite-scroll spec)
- Advanced filtering (multi-select with AND logic, exclude filters, etc.)
- Filter analytics tracking (covered in 006-google-analytics spec)
- Saved filter presets or user preferences
- Tag popularity indicators or sorting
