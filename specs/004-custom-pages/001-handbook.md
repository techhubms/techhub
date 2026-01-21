# Custom Page: GitHub Copilot Handbook

**Page**: GitHub Copilot Handbook  
**URL**: `/github-copilot/handbook`  
**Priority**: P2 (Quick Win)  
**Status**: Razor ✅ | E2E Tests ❌  
**Estimated Effort**: 2-4 hours

## Overview

The GitHub Copilot Handbook page is **already implemented** as a Blazor component with SidebarToc. This spec focuses on **adding comprehensive E2E tests** to validate the implementation.

**Live Site Reference**: <https://tech.hub.ms/github-copilot/handbook>

## Current Status

✅ **Complete**:

- Razor component exists: `src/TechHub.Web/Components/Pages/GitHubCopilotHandbook.razor`
- SidebarToc component integrated
- Styled hero section with book cover
- Purchase links implemented
- JSON data file: `collections/_custom/handbook.json`

❌ **Missing**:

- E2E tests for page rendering
- E2E tests for TOC functionality
- E2E tests for purchase link navigation
- E2E tests for scroll spy behavior

## Acceptance Criteria

### E2E Test Coverage Required

1. **Page Load & Content**
   - ✅ Navigate to `/github-copilot/handbook`
   - ✅ Verify page renders without errors
   - ✅ Verify book cover image displays
   - ✅ Verify title and author information present
   - ✅ Verify purchase links are visible and clickable

2. **SidebarToc Functionality**
   - ✅ Verify TOC renders with correct headings
   - ✅ Click TOC link → smooth scrolls to section
   - ✅ Scroll through content → TOC highlights active section
   - ✅ Verify last section can scroll to detection point (50vh spacer)

3. **Interactive Elements**
   - ✅ Verify all purchase links have valid URLs
   - ✅ Verify keyboard navigation works (Tab/Enter)
   - ✅ Verify no console errors on page load

4. **Accessibility**
   - ✅ Book cover has alt text
   - ✅ Proper semantic HTML structure
   - ✅ ARIA labels for purchase buttons

## Implementation Tasks

### Task 1: Create E2E Test File

**File**: `tests/TechHub.E2E.Tests/Web/GitHubCopilotHandbookTests.cs`

**Test Methods**:

```csharp
[Fact]
public async Task Handbook_OnPageLoad_ShouldRenderContent()

[Fact]
public async Task Handbook_ShouldRender_WithSidebarToc()

[Fact]
public async Task Handbook_TocLinks_ShouldScrollToSections()

[Fact]
public async Task Handbook_Scrolling_ShouldUpdateActiveTocLink()

[Fact]
public async Task Handbook_LastSection_ShouldScroll_ToDetectionPoint()

[Fact]
public async Task Handbook_PurchaseLinks_ShouldBeClickable()
```

**Reference**: Use existing pattern from `CustomPagesTocTests.cs` for Levels of Enlightenment tests

### Task 2: Verify JSON Completeness (Optional)

**File**: `collections/_custom/handbook.json`

Compare against original markdown to ensure all content is present:

- All sections and subsections
- Book metadata (title, author, publisher, ISBN)
- Purchase links (all retailers)
- Content preview sections

**Original Markdown**: Check if exists in main branch at `sections/github-copilot/handbook.md`

### Task 3: Run Tests

Execute E2E tests:

```powershell
Run -TestProject E2E.Tests -TestName Handbook
```

Verify all tests pass with no regressions.

## Success Metrics

- ✅ All E2E tests pass
- ✅ Page loads in < 2 seconds
- ✅ No console errors
- ✅ TOC scroll spy works correctly
- ✅ Purchase links navigate to correct retailers
- ✅ Keyboard navigation works (Tab/Enter)

## Dependencies

- ✅ SidebarToc component (already exists)
- ✅ GitHubCopilotHandbook.razor (already implemented)
- ✅ handbook.json (already exists)
- ✅ Playwright E2E test infrastructure

## Out of Scope

- Content updates or changes
- Visual design modifications
- Mobile navigation (covered in spec 011)
- SEO optimization (covered in spec 005)

## Completion Checklist

- [ ] E2E test file created with all required test methods
- [ ] All tests pass without failures
- [ ] JSON content verified against original markdown (if available)
- [ ] No console errors during test execution
- [ ] Keyboard navigation verified
- [ ] Documentation updated (if needed)
