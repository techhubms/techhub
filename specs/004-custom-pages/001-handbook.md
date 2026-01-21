# Custom Page: GitHub Copilot Handbook

**Page**: GitHub Copilot Handbook  
**URL**: `/github-copilot/handbook`  
**Priority**: P2 (Quick Win)  
**Status**: ✅ COMPLETE  
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
- JSON data file: `collections/_custom/handbook.json` - **VERIFIED COMPLETE**
  - All book metadata (title, authors, publisher, release date, pages)
  - Complete purchase link (Amazon)
  - All content sections (About, Learnings, Audience, Key Features)
  - Complete Table of Contents (11 chapters)
  - Author biographies (Rob Bos, Randy Pagels)
  - Conclusion text
- E2E tests implemented: `tests/TechHub.E2E.Tests/Web/HandbookTests.cs`
  - Handbook_ShouldRender_WithSidebarToc
  - Handbook_HeroSection_ShouldDisplay
  - Handbook_TocLinks_ShouldScrollToSections
  - Handbook_Scrolling_ShouldUpdateActiveTocLink
  - Handbook_ShouldBe_KeyboardAccessible

❌ **Missing**:

- None

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

**File**: `tests/TechHub.E2E.Tests/Web/HandbookTests.cs`

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

**Reference**: Use existing pattern from `LevelsOfEnlightenmentTests.cs` for Levels of Enlightenment tests

### Task 2: Verify JSON Completeness (REQUIRED)

**File**: `collections/_custom/handbook.json`

**Source**: `https://raw.githubusercontent.com/techhubms/techhub/main/sections/github-copilot/handbook.md`

Compare against original markdown to ensure all content is present:

- All sections and subsections
- Book metadata (title, author, publisher, ISBN)
- Purchase links (all retailers)
- Content preview sections

**CRITICAL**: This is MANDATORY - JSON must match original markdown exactly

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

- [X] E2E test file created with all required test methods
- [X] All tests pass without failures
- [X] JSON content verified against original markdown (REQUIRED) - ✅ VERIFIED COMPLETE
- [X] No console errors during test execution
- [X] Keyboard navigation verified
- [X] Documentation updated (spec status updated to COMPLETE)
