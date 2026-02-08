# Date Range Slider (001b) - Quick Reference

**Last Updated**: 2026-02-08  
**Status**: Not Started

## 🚀 Quick Start

If you're starting this work, **begin here**:

1. **Read**: [spec.md](spec.md) for complete feature details
2. **Dependencies**: Backend date filtering is ✅ complete (repository layer, API endpoints)
3. **First Task**: Create `DateRangeSlider.razor` component with dual handles
4. **Integration**: Wire up to content filtering and tag count updates

## ❌ What Needs Building

### Component
- ❌ DateRangeSlider.razor component
- ❌ Dual handle slider (from and to dates)
- ❌ Default to last 90 days (3 months)
- ❌ Visual date range display (e.g., "Oct 16, 2024 - Jan 16, 2026")
- ❌ Optional preset buttons (Last 7/30/90 days)

### Integration
- ❌ URL parameter handling (`from`, `to`)
- ❌ Trigger tag count recalculation on date change (requires [001a-tag-counting](../001a-tag-counting/))
- ❌ Content list refresh on date change
- ❌ Infinite scroll respects date boundaries

### Testing
- ❌ Component tests (bUnit) - slider behavior, defaults, URL restore
- ❌ E2E tests (Playwright) - drag handles, preset buttons, keyboard navigation

## 📋 Implementation Checklist

### Phase 1: Basic Slider Component
- [ ] Create `src/TechHub.Web/Components/DateRangeSlider.razor`
- [ ] Add dual handle slider (mouse dragging)
- [ ] Display selected date range
- [ ] Default to last 90 days
- [ ] Emit `OnDateRangeChanged` event

### Phase 2: URL Integration
- [ ] Read `from`/`to` parameters on page load
- [ ] Update URL when slider changes
- [ ] Handle invalid date formats (fallback to default)
- [ ] Debounce updates during dragging

### Phase 3: Filter Integration
- [ ] Wire to content list (reload on date change)
- [ ] Trigger tag count refresh (requires [001a](../001a-tag-counting/))
- [ ] Combine with tag filters (AND logic)

### Phase 4: Enhanced UX
- [ ] Add preset buttons (Last 7/30/90 days) - optional
- [ ] Touch support for mobile
- [ ] Keyboard accessibility (arrow keys)
- [ ] Loading state during content refresh

### Phase 5: Testing
- [ ] Component tests
- [ ] E2E tests
- [ ] Accessibility tests

## 💡 Key Implementation Details

### URL Parameter Format

```text
# Custom range
/ai?from=2024-10-16&to=2026-01-16

# With tags (AND logic)
/ai?tags=azure,security&from=2025-01-01&to=2026-02-08

# Preset (optional)
/ai?date=last-30-days
```

### Component API

```csharp
<DateRangeSlider 
    FromDate="@fromDate"
    ToDate="@toDate"
    DefaultLastDays="90"
    OnDateRangeChanged="HandleDateRangeChanged" />

@code {
    private DateOnly? fromDate;
    private DateOnly? toDate;

    private async Task HandleDateRangeChanged(DateRangeChangedEventArgs args)
    {
        fromDate = args.FromDate;
        toDate = args.ToDate;
        
        // Update URL
        NavigationManager.NavigateTo($"?from={fromDate:yyyy-MM-dd}&to={toDate:yyyy-MM-dd}");
        
        // Reload content
        await LoadContent();
        
        // Refresh tag counts (requires 001a)
        await RefreshTagCounts();
    }
}
```

### Backend API (Already Exists)

```http
GET /api/content/filter?section=ai&from=2024-10-16&to=2026-01-16
GET /api/sections/ai/collections/all/tags?from=2024-10-16&to=2026-01-16
```

### Default Behavior

- **No URL params**: Default to last 90 days from today
- **Only `from`**: Set `to` to today
- **Only `to`**: Set `from` to 90 days before `to`
- **Invalid dates**: Fallback to last 90 days, log warning

### Performance Considerations

- **Debounce**: Apply filter only after drag complete or 500ms pause
- **60fps**: Smooth slider animation during drag
- **API response**: < 200ms for content + tag count refresh

## 🔗 Dependencies

### Required (Backend - ✅ Complete)
- ✅ API date filtering endpoints
- ✅ Repository layer date range support
- ✅ Content metadata with publication dates

### Optional (Frontend - Integration Points)
- ⚠️ [001a-tag-counting](../001a-tag-counting/) - Refresh tag counts when date changes
- 🔜 [001c-tag-dropdown-filter](../001c-tag-dropdown-filter/) - Dropdown affected by date range
- ✅ [003-infinite-scroll](../003-infinite-scroll/) - Pagination respects date boundaries

## 🔗 Related Specs

- **[001a-tag-counting](../001a-tag-counting/)** - Tag counts affected by date range
- **[001c-tag-dropdown-filter](../001c-tag-dropdown-filter/)** - Dropdown also filtered by date

## 📊 Acceptance Criteria

- [ ] Slider defaults to last 90 days on page load
- [ ] Users can drag handles to select custom ranges
- [ ] "From" handle can extend backward to unlimited historical content
- [ ] Date range displays clearly (e.g., "Oct 16, 2024 - Jan 16, 2026")
- [ ] Content list updates immediately when range changes
- [ ] Tag counts recalculate for new date range (with 001a)
- [ ] URL parameters update and are shareable
- [ ] Slider restores position from URL
- [ ] Keyboard navigation works (arrow keys)
- [ ] Touch interaction works on mobile
- [ ] Invalid URLs fallback to last 90 days

## ❓ Technical Decisions Needed

### Slider Library
**Options**:
1. Build custom slider (full control, no dependencies)
2. Use existing library (faster, battle-tested)
   - noUiSlider
   - rc-slider
   - range-slider-input

**Recommendation**: Start with custom for simplicity, use library if complexity grows

### Preset Buttons
**Question**: Include preset buttons (Last 7/30/90 days) in MVP?

**Recommendation**: Optional - add if time permits, not critical for MVP

### Date Picker Alternative
**Question**: Provide date picker option for exact date selection?

**Recommendation**: Out of scope for MVP, slider-only for now

## 🧪 Testing Strategy

### Component Tests (bUnit)
- Slider renders with defaults
- Handle dragging updates state
- URL parameters restore correctly
- Preset buttons work (if implemented)
- Invalid dates fallback
- Event emission
- Keyboard navigation

### E2E Tests (Playwright)
- Drag handles
- Content updates
- URL updates
- Share URL and restore
- Preset buttons
- Keyboard usage
- Touch interaction

## ❓ Questions?

Read [spec.md](spec.md) for detailed requirements and acceptance scenarios.
