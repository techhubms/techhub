# Excel-Style Tag Dropdown Filter (001c) - Quick Reference

**Last Updated**: 2026-02-08  
**Status**: Not Started

## 🚀 Quick Start

If you're starting this work, **begin here**:

1. **Read**: [spec.md](spec.md) for complete feature details
2. **Dependencies**: 
   - ⚠️ Tag counting ([001a-tag-counting](../001a-tag-counting/)) for dynamic counts
   - ✅ Backend API for fetching all tags (complete)
3. **First Task**: Create `TagDropdownFilter.razor` component with dropdown structure
4. **Integration**: Synchronize with SidebarTagCloud, wire tag counting

## ❌ What Needs Building

### Component Structure
- ❌ TagDropdownFilter.razor component
- ❌ Dropdown button ("Filter by Tags (2)")
- ❌ Scrollable tag list panel
- ❌ Search box for filtering tags
- ❌ Checkboxes for multi-select
- ❌ "Select All" / "Clear All" actions

### Dynamic Features
- ❌ Display counts for each tag (requires [001a](../001a-tag-counting/))
- ❌ Update counts when selection changes (requires [001a](../001a-tag-counting/))
- ❌ Disable tags with count=0 (requires [001a](../001a-tag-counting/))
- ❌ Virtual scrolling for 50+ tags

### Integration
- ❌ Synchronize with SidebarTagCloud
- ❌ URL parameter handling (share state with tag cloud)
- ❌ Content list refresh on selection
- ❌ Date range integration (requires [001b](../001b-date-range-slider/))

### Testing
- ❌ Component tests (bUnit) - dropdown, search, selection
- ❌ E2E tests (Playwright) - user flows, keyboard navigation

## 📋 Implementation Checklist

### Phase 1: Basic Dropdown
- [ ] Create `src/TechHub.Web/Components/TagDropdownFilter.razor`
- [ ] Dropdown button with toggle
- [ ] Panel opens/closes
- [ ] Fetch all tags from API
- [ ] Display tags in scrollable list
- [ ] Basic checkbox selection

### Phase 2: Search & Actions
- [ ] Add search box at top of panel
- [ ] Filter tags in real-time as user types
- [ ] "Select All" button (only visible/enabled tags)
- [ ] "Clear All" button
- [ ] "No tags found" message when search is empty

### Phase 3: Dynamic Counts (requires [001a](../001a-tag-counting/))
- [ ] Display count next to each tag: "AI (901)"
- [ ] Update counts when selection changes (intersection)
- [ ] Disable tags with count=0 (grayed row, disabled checkbox)
- [ ] Thousand separator formatting

### Phase 4: Performance
- [ ] Virtual scrolling for 50+ tags
- [ ] Debounce search filtering (50ms)
- [ ] Debounce count updates (200ms)

### Phase 5: Integration
- [ ] Synchronize with SidebarTagCloud
- [ ] Update URL parameters
- [ ] Reload content on selection change
- [ ] Wire to date range slider (requires [001b](../001b-date-range-slider/))

### Phase 6: UX Polish
- [ ] Click outside to close
- [ ] Escape key to close
- [ ] Keyboard navigation (Tab, Space, Enter)
- [ ] Loading state while fetching tags
- [ ] Loading state while updating counts

### Phase 7: Testing
- [ ] Component tests (bUnit)
- [ ] E2E tests (Playwright)
- [ ] Accessibility tests

## 💡 Key Implementation Details

### Component API

```csharp
<TagDropdownFilter 
    SelectedTags="@selectedTags"
    FromDate="@fromDate"    // Optional: For dynamic counts
    ToDate="@toDate"        // Optional: For dynamic counts
    OnSelectionChanged="HandleSelectionChanged" />

@code {
    private string[] selectedTags = Array.Empty<string>();
    private DateOnly? fromDate;
    private DateOnly? toDate;

    private async Task HandleSelectionChanged(string[] tags)
    {
        selectedTags = tags;
        
        // Update URL
        var query = string.Join(",", tags);
        NavigationManager.NavigateTo($"?tags={query}");
        
        // Reload content
        await LoadContent();
        
        // Refresh all counts (sidebar + dropdown)
        await RefreshAllTagCounts();
    }
}
```

### Backend API (Already Exists)

```http
# Fetch all tags
GET /api/tags

# With dynamic counts (requires 001a)
GET /api/tags?tags=ai,azure&from=2025-01-01&to=2026-02-08
```

### Synchronization with Sidebar

**Strategy**: Parent component manages shared state

```csharp
// Section.razor (parent)
<TagDropdownFilter 
    SelectedTags="@selectedTags" 
    OnSelectionChanged="HandleTagSelectionChanged" />

<SidebarTagCloud 
    SelectedTags="@selectedTags" 
    OnTagClicked="HandleTagSelectionChanged" />

@code {
    private string[] selectedTags = Array.Empty<string>();

    private async Task HandleTagSelectionChanged(string[] tags)
    {
        selectedTags = tags;
        // Both components will update automatically via binding
        await LoadContent();
        await RefreshTagCounts();
    }
}
```

### Virtual Scrolling

**Trigger**: Activate when tag count ≥ 50

**Options**:
1. Use Blazor `<Virtualize>` component (built-in)
2. Custom implementation with IntersectionObserver

**Recommendation**: Start with `<Virtualize>` for simplicity

```razor
<Virtualize Items="@filteredTags" Context="tag">
    <label class="tag-item @(tag.Count == 0 ? "disabled" : "")">
        <input type="checkbox" 
               checked="@IsSelected(tag.Tag)" 
               disabled="@(tag.Count == 0)"
               @onchange="() => ToggleTag(tag.Tag)" />
        @tag.Tag (@tag.Count.ToString("N0"))
    </label>
</Virtualize>
```

### UI Design Reference

**Dropdown Button**:

```text
┌─────────────────────────────┐
│ Filter by Tags (2)       ▼ │
└─────────────────────────────┘
```

**Opened Dropdown**:

```text
┌─────────────────────────────┐
│ [🔍 Search tags...]       × │
├─────────────────────────────┤
│ ☐ Select All  ☑ Clear All  │
├─────────────────────────────┤
│ ☑ AI (901)                  │
│ ☐ Azure (0)        [grayed] │
│ ☑ GitHub Copilot (245)      │
│ ☐ Kubernetes (87)           │
│ ☐ Visual Studio (156)       │
│ ... (virtual scrolling)     │
└─────────────────────────────┘
```

## 🔗 Dependencies

### Required (Backend - ✅ Complete)
- ✅ `GET /api/tags` endpoint
- ✅ Repository layer tag listing

### Required (Frontend - Integration)
- ⚠️ [001a-tag-counting](../001a-tag-counting/) - Dynamic counts
- ✅ SidebarTagCloud - State synchronization

### Optional
- 🔜 [001b-date-range-slider](../001b-date-range-slider/) - Date filtering

## 🔗 Related Specs

- **[001a-tag-counting](../001a-tag-counting/)** - Dynamic counts feature
- **[001b-date-range-slider](../001b-date-range-slider/)** - Date range affects tags

## 📊 Acceptance Criteria

- [ ] Dropdown button displays "Filter by Tags" with selected count
- [ ] Clicking opens scrollable panel with all tags
- [ ] Search box filters tags in real-time
- [ ] Checkboxes allow multi-select
- [ ] Each tag displays dynamic count (requires [001a](../001a-tag-counting/))
- [ ] Counts update when selection changes (requires [001a](../001a-tag-counting/))
- [ ] Tags with count=0 are disabled (requires [001a](../001a-tag-counting/))
- [ ] Virtual scrolling works with 50+ tags
- [ ] "Select All" selects only visible/enabled tags
- [ ] "Clear All" deselects all tags
- [ ] Selections synchronize with sidebar tag cloud
- [ ] Click outside closes dropdown
- [ ] Escape key closes dropdown
- [ ] Keyboard navigation works (Tab, Space, Enter)

## 🧪 Testing Strategy

### Component Tests (bUnit)
- Dropdown renders and toggles
- Search filters tag list
- Checkbox selection/deselection
- "Select All" / "Clear All" functionality
- Virtual scrolling (if using custom)
- Disabled state rendering
- Keyboard events (Escape, Tab, Space)
- Click outside detection

### Integration Tests
- API call to fetch all tags
- State synchronization with SidebarTagCloud

### E2E Tests (Playwright)
- Open dropdown, search for tag
- Select multiple tags via checkboxes
- Verify content updates
- Verify counts update (with [001a](../001a-tag-counting/))
- Test virtual scrolling with 50+ tags
- Keyboard navigation flow
- Click outside to close
- "Select All" / "Clear All" actions

## ❓ Technical Decisions Needed

### Virtual Scrolling Library
**Options**:
1. Blazor `<Virtualize>` component (built-in, simple)
2. Custom implementation (full control)

**Recommendation**: Use `<Virtualize>` unless specific limitations found

### Search Filtering Performance
**Question**: Client-side or server-side search?

**Recommendation**: Client-side for <200 tags, server-side if tag count grows significantly

### Dropdown Positioning
**Question**: Position relative to button or fixed sidebar location?

**Recommendation**: Position below button with overflow handling

## ❓ Questions?

Read [spec.md](spec.md) for detailed requirements and acceptance scenarios.
