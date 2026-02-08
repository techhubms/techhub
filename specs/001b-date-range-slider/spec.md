# Feature Specification: Date Range Slider

**Feature Branch**: `001b-date-range-slider`  
**Created**: 2026-02-08 (split from 001-filtering-system)  
**Updated**: 2026-02-08  
**Status**: Not Started  
**Input**: Implement interactive date range slider for filtering content by publication date with smart defaults

## 📋 Current Status (as of 2026-02-08)

### ❌ NOT STARTED

All work for this feature is pending:
- ❌ DateRangeSlider component
- ❌ Date range parameter handling in URL
- ❌ Integration with tag cloud (reload counts when date changes)
- ❌ Preset date buttons (optional)
- ❌ Component tests
- ❌ E2E tests

### 📂 Dependencies

**Required (Complete)**:
- ✅ Database schema with date filtering support
- ✅ Repository layer with date range filtering (`SearchAsync`)
- ✅ API endpoints accept date parameters

**Optional**:
- ⚠️ Tag counting feature ([001a-tag-counting](../001a-tag-counting/)) for dynamic count updates
- 🔜 Tag dropdown filter ([001c-tag-dropdown-filter](../001c-tag-dropdown-filter/))

---

## Overview

The date range slider provides an interactive way for users to filter content by publication date. It defaults to "last 3 months" to prevent information overload, but allows users to extend the range backward to see all historical content. The slider integrates with tag counts to update available tags based on the selected date range.

**Key Features**:
- Interactive slider with two draggable handles (from and to dates)
- **Defaults to last 90 days (3 months)** to prevent overwhelming users
- Backward extension to unlimited historical content
- Optional preset buttons (Last 7/30/90 days) for quick access
- URL state preservation (`from` and `to` parameters)
- Triggers tag count recalculation when date range changes

## User Stories

### User Story 2: Filter Content by Date Range

**Priority**: P1  
**Status**: ❌ Not Started

Users can select date ranges using an interactive slider to filter content by publication date. Date selection affects tag cloud counts.

**Acceptance Scenarios**:

1. ❌ **Given** I'm on a section page, **When** I view the date slider, **Then** it defaults to "last 3 months" (90 days)
2. ❌ **Given** I adjust the date slider to "Last 30 days", **When** the filter applies, **Then** the content list shows only items from the past 30 days
3. ❌ **Given** I change the date range, **When** tags update, **Then** all tag counts recalculate based on the new date range (requires [001a-tag-counting](../001a-tag-counting/))
4. ❌ **Given** I select "Last 7 days", **When** a tag has no items in that period, **Then** that tag becomes disabled (requires [001a-tag-counting](../001a-tag-counting/))
5. ❌ **Given** I have date and tag filters active, **When** I view results, **Then** I see items matching BOTH filters (AND logic)
6. ❌ **Given** I drag the slider handles, **When** I set a custom range, **Then** URL includes `from` and `to` date parameters
7. ❌ **Given** I extend the date range backward, **When** more content becomes available, **Then** tag counts increase and disabled tags may become enabled (requires [001a-tag-counting](../001a-tag-counting/))

### User Story 7: Interactive Date Range Slider

**Priority**: P2  
**Status**: ❌ Not Started

Users can select custom date ranges using an interactive slider with from-to controls for precise temporal filtering, with smart defaults to prevent overwhelming content.

**Acceptance Scenarios**:

1. ❌ **Given** I navigate to any page, **When** the page loads, **Then** the date slider defaults to "last 3 months" (prevents showing endless content)
2. ❌ **Given** I'm on a section page, **When** I interact with the date range slider, **Then** I see two draggable handles (from and to dates)
3. ❌ **Given** I drag the "from" handle backward, **When** I release, **Then** I can extend the range to see older content (no limit)
4. ❌ **Given** I drag the "to" handle, **When** I release, **Then** content filters to show items up to that date
5. ❌ **Given** I set a custom range, **When** I check the URL, **Then** it includes from/to date parameters (e.g., `?from=2024-10-16&to=2026-01-16`)
6. ❌ **Given** I use the slider, **When** I view the UI, **Then** I see the selected date range displayed clearly (e.g., "Oct 16, 2024 - Jan 16, 2026")
7. ❌ **Given** I combine slider with tag filters, **When** viewing results, **Then** AND logic applies (items must match date range AND tags)
8. ❌ **Given** I share a URL with custom date range, **When** someone opens it, **Then** they see the same filtered view with slider positioned correctly
9. ❌ **Given** I scroll down with infinite scroll enabled, **When** I reach the bottom, **Then** more content loads only within the selected date range

## Requirements

### Functional Requirements

**Slider Behavior**:
- **FR-001**: System MUST provide date filtering via interactive slider with from-to date range selection
- **FR-001a**: Date slider MUST default to "last 3 months" (90 days) when no date range specified in URL
- **FR-001b**: Users MUST be able to drag the "from" handle backward to extend range to all historical content (no limit)
- **FR-001c**: Users MUST be able to drag the "to" handle to set end date
- **FR-001d**: Slider handles MUST be draggable via mouse and touch (mobile-friendly)

**Optional Presets**:
- **FR-002**: System MAY provide preset date range buttons (Last 7/30/90 days) as quick shortcuts alongside slider
- **FR-002a**: Clicking a preset MUST update slider handles and apply filter immediately

**Visual Display**:
- **FR-003**: Slider MUST display the selected date range clearly (e.g., "Oct 16, 2024 - Jan 16, 2026")
- **FR-003a**: Date format MUST be user-friendly (short month names, day, year)

**Integration**:
- **FR-004**: Date range changes MUST trigger tag count recalculation (when [001a-tag-counting](../001a-tag-counting/) is integrated)
- **FR-005**: Date range filters MUST combine with tag filters using AND logic (must match date AND any tag)
- **FR-006**: Slider updates MUST be debounced to prevent excessive API calls during dragging

**URL State**:
- **FR-007**: System MUST update URL query parameters when date range changes
- **FR-007a**: Parameters MUST use format: `from=2024-10-16&to=2026-01-16` (ISO 8601)
- **FR-007b**: System MUST parse URL parameters on page load and restore slider position
- **FR-007c**: Invalid date formats MUST fallback to default (last 3 months) and log warning

**Accessibility**:
- **FR-008**: Slider MUST be keyboard accessible (arrow keys to adjust, tab navigation)
- **FR-008a**: Screen readers MUST announce current date range and handle positions
- **FR-008b**: Focus states MUST be clearly visible

### Performance Requirements

- **PR-001**: Slider drag updates MUST feel smooth (60fps target)
- **PR-002**: Date filter application MUST trigger content refresh within 200ms
- **PR-003**: Debounce slider updates during drag (apply filter only on release or after 500ms pause)

## Success Criteria

- **SC-001**: ❌ Date slider defaults to last 3 months on initial page load
- **SC-002**: ❌ Users can drag handles to select custom date ranges
- **SC-003**: ❌ Date range updates immediately reflect in content list
- **SC-004**: ❌ Tag counts update when date range changes (with [001a-tag-counting](../001a-tag-counting/))
- **SC-005**: ❌ URL parameters update and are shareable/bookmarkable
- **SC-006**: ❌ Slider position restores correctly from URL parameters
- **SC-007**: ❌ Keyboard navigation works (arrow keys, tab)
- **SC-008**: ❌ Touch interaction works on mobile devices
- **SC-009**: ❌ Preset buttons (if implemented) work correctly
- **SC-010**: ❌ Invalid URLs fallback to default (last 3 months)

## Implementation Notes

### Component Structure

**File**: `src/TechHub.Web/Components/DateRangeSlider.razor`

**Parameters**:
```csharp
[Parameter] public DateOnly? FromDate { get; set; }
[Parameter] public DateOnly? ToDate { get; set; }
[Parameter] public EventCallback<DateRangeChangedEventArgs> OnDateRangeChanged { get; set; }
[Parameter] public int DefaultLastDays { get; set; } = 90;  // Default to 3 months
```

**Events**:
```csharp
public class DateRangeChangedEventArgs : EventArgs
{
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
}
```

### URL Parameter Format

**Query Parameters**:
- `from` - Start date (e.g., `from=2024-10-16`) - ISO 8601 format
- `to` - End date (e.g., `to=2026-01-16`) - ISO 8601 format
- `date` - (Optional) Preset identifier (e.g., `date=last-30-days`)

**Examples**:
```
/ai?from=2024-10-16&to=2026-01-16         # Custom range
/ai?date=last-30-days                      # Preset
/ai?tags=azure&from=2025-01-01             # Combined with tags
```

**Defaults**:
- If no parameters: Default to last 90 days
- If only `from`: Set `to` to today
- If only `to`: Set `from` to 90 days before `to`

### Integration Points

**With Tag Counts** (requires [001a-tag-counting](../001a-tag-counting/)):
```razor
<DateRangeSlider OnDateRangeChanged="HandleDateRangeChanged" />
<SidebarTagCloud DateRange="@currentDateRange" />

@code {
    private async Task HandleDateRangeChanged(DateRangeChangedEventArgs args)
    {
        currentDateRange = args;
        // Reload tag counts with new date range
        await sidebarTagCloud.RefreshCounts();
        // Reload content
        await LoadContent();
    }
}
```

**With Content List**:
- Call API with `from` and `to` parameters
- API endpoint: `GET /api/content/filter?section={section}&from={from}&to={to}`

**With Infinite Scroll**:
- Infinite scroll only loads content within selected date range
- No pagination beyond date boundaries

### UI/UX Considerations

**Visual Design**:
- Dual handle slider (from and to)
- Clear date range display above slider
- Optional preset buttons for quick selection
- Visual timeline showing content availability

**Interaction**:
- Draggable handles with touch support
- Click on track to move nearest handle
- Keyboard arrow keys to adjust
- Debounced updates (only apply after drag complete or 500ms pause)

**Mobile**:
- Touch-friendly handle size (44x44px minimum)
- Responsive layout
- Consider alternative UI (date pickers) for very small screens

### Testing Strategy

**Component Tests** (bUnit):
- Slider renders with correct default (last 90 days)
- Dragging handles updates state
- URL parameters restore slider position
- Preset buttons update slider (if implemented)
- Invalid dates fallback to default
- Event firing on date range change
- Keyboard navigation

**Integration Tests**:
- Date filtering with API (already exists in backend)
- Combined date + tag filtering

**E2E Tests** (Playwright):
- Navigate to page, verify slider defaults to 3 months
- Drag "from" handle backward to extend range
- Drag "to" handle to set end date
- Verify content updates immediately
- Verify URL updates with parameters
- Share URL, verify slider restores correctly
- Use preset buttons (if implemented)
- Keyboard navigation (arrow keys, tab)
- Touch interaction on mobile

## Dependencies

**Required (Backend - Complete)**:
- ✅ API accepts `from` and `to` date parameters
- ✅ Repository layer filters by date range
- ✅ Content metadata includes publication dates

**Optional (Frontend)**:
- ⚠️ Tag counting feature ([001a-tag-counting](../001a-tag-counting/)) to refresh counts
- 🔜 Tag dropdown filter ([001c-tag-dropdown-filter](../001c-tag-dropdown-filter/))
- ✅ Infinite scroll (already implemented)

**Libraries**:
- Consider using a slider library (e.g., noUiSlider, rc-slider) or build custom
- Date formatting library (already have .NET date formatting)

## Related Specs

- **[001a-tag-counting](../001a-tag-counting/)** - Tag counts update when date changes
- **[001c-tag-dropdown-filter](../001c-tag-dropdown-filter/)** - Dropdown also affected by date range
- **[003-infinite-scroll](https://github.com/techhub)** - Pagination respects date boundaries (see [docs/page-structure.md](../../docs/page-structure.md))

## Out of Scope

- Advanced date presets (custom ranges like "This year", "Last quarter")
- Date histogram showing content distribution over time
- Calendar picker interface (slider only for MVP)
- Time-of-day filtering (dates only, no time component)
- Saved date preferences
