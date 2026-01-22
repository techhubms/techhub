# Custom Page: GitHub Copilot Levels of Enlightenment

**Page**: GitHub Copilot Levels of Enlightenment  
**URL**: `/github-copilot/levels-of-enlightenment`  
**Priority**: P2 (Quick Win)  
**Status**: ✅ COMPLETE  
**Estimated Effort**: 1-2 hours

## Overview

The Levels of Enlightenment page is **already implemented** with comprehensive E2E tests in `LevelsOfEnlightenmentTests.cs`. This spec focuses on **verifying test coverage is complete** and adding any missing tests.

**Live Site Reference**: <https://tech.hub.ms/github-copilot/levels-of-enlightenment>

## Current Status

✅ **Complete**:

- Razor component exists: `src/TechHub.Web/Components/Pages/GitHubCopilotLevels.razor`
- SidebarToc component integrated
- JSON data file: `collections/_custom/levels.json`
- E2E tests exist in `LevelsOfEnlightenmentTests.cs` with 5 test methods

✅ **Existing E2E Tests**:

1. `LevelsOfEnlightenment_ShouldRender_WithSidebarToc`
2. `LevelsOfEnlightenment_TocLinks_ShouldScrollToLevels`
3. `LevelsOfEnlightenment_LastSection_ShouldScroll_ToDetectionPoint`
4. `LevelsOfEnlightenment_Scrolling_ShouldUpdateActiveTocLink`
5. `LevelsOfEnlightenment_Overview_ShouldBe_Highlighted`

❓ **To Verify**:

- Are all level sections tested?
- Are interactive elements tested (level cards, progression indicators)?
- Is keyboard navigation tested?
- Is JSON content complete vs original markdown?

## Acceptance Criteria

### Test Coverage Verification

1. **Existing Tests Review**
   - ✅ Verify all 5 tests still pass
   - ✅ Run tests with `Run -TestProject E2E.Tests -TestName LevelsOfEnlightenment`
   - ✅ Confirm no test failures or flakiness

2. **Additional Test Coverage (if needed)**
   - ✅ All proficiency levels display correctly
   - ✅ Level icons/visual indicators render
   - ✅ Level descriptions are readable
   - ✅ Progression indicators work

3. **Accessibility & Interactivity**
   - ✅ Keyboard navigation through levels
   - ✅ Screen reader compatibility (semantic HTML)
   - ✅ No console errors

## Implementation Tasks

### Task 1: Run Existing Tests

Execute existing E2E tests:

```powershell
Run -TestProject E2E.Tests -TestName LevelsOfEnlightenment
```

Verify all 5 tests pass with green status.

### Task 2: Review Test Coverage

**File**: `tests/TechHub.E2E.Tests/Web/LevelsOfEnlightenmentTests.cs`

Review lines 221-360 to ensure tests cover:

- ✅ Page load and rendering
- ✅ TOC generation and display
- ✅ TOC link navigation (scroll to section)
- ✅ Scroll spy (active TOC link updates)
- ✅ 50vh spacer (last section scrollable to detection point)

**Gap Analysis**: Determine if additional tests needed for:

- Individual level content verification
- Visual element rendering (icons, cards)
- Progression indicator functionality

### Task 3: Verify JSON Completeness (REQUIRED)

**File**: `collections/_custom/levels.json`

**Source**: `https://raw.githubusercontent.com/techhubms/techhub/main/sections/github-copilot/levels-of-enlightenment.md`

**CRITICAL**: This is MANDATORY - JSON must match original markdown exactly

Compare against original markdown to ensure all content is present:

- All proficiency levels (Novice → Expert)
- Level criteria and descriptions
- Skills required for each level
- Progression guidance

### Task 4: Add Missing Tests (if needed)

If gaps identified, add focused E2E tests for:

```csharp
[Fact]
public async Task Levels_AllLevelsShouldDisplay()

[Fact]
public async Task Levels_LevelIconsShouldRender()

[Fact]
public async Task Levels_KeyboardNavigation_ShouldWork()
```

**Location**: Add to `LevelsOfEnlightenmentTests.cs`

## Success Metrics

- ✅ All existing tests pass (5/5)
- ✅ Page loads in < 2 seconds
- ✅ No console errors
- ✅ TOC scroll spy works correctly
- ✅ All proficiency levels render
- ✅ Keyboard navigation works

## Dependencies

- ✅ SidebarToc component (already exists)
- ✅ GitHubCopilotLevels.razor (already implemented)
- ✅ levels.json (already exists)
- ✅ Existing E2E tests in LevelsOfEnlightenmentTests.cs

## Out of Scope

- Content updates or level changes
- Visual design modifications
- Gamification features (badges, tracking)
- Mobile navigation (covered in spec 011)

## Completion Checklist

- [ ] Existing E2E tests verified (all 5 pass)
- [ ] JSON content verified against original markdown
- [ ] Gap analysis completed for additional test coverage
- [ ] Additional tests added (if gaps found)
- [ ] All tests pass without failures
- [ ] No console errors during test execution
- [ ] Documentation updated (if needed)
