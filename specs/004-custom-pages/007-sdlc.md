# Custom Page: AI in Software Development Lifecycle

**Page**: AI in Software Development Lifecycle (SDLC)  
**URL**: `/ai/sdlc`  
**Priority**: P3  
**Status**: Razor ✅ | JSON ❓ (Needs Verification) | E2E Tests ❌  
**Estimated Effort**: 8-10 hours

## Overview

The AI in SDLC page explores how AI integrates into software development lifecycle phases with timeline visualization and phase-specific guidance.

**Live Site Reference**: <https://tech.hub.ms/ai/sdlc>

## Current Status

✅ **Complete**:

- Razor component exists: `src/TechHub.Web/Components/Pages/AISDLC.razor`
- SidebarToc component integrated
- JSON data file: `collections/_custom/sdlc.json`

❓ **Needs Verification**:

- JSON content completeness vs original markdown
- Timeline/phase structure
- Visual elements (phase markers, connectors)
- AI enhancement details for each phase

❌ **Missing Tests**:

- No E2E tests exist for SDLC page

## Acceptance Criteria

### JSON Content Verification

**Source**: `https://raw.githubusercontent.com/techhubms/techhub/main/sections/ai/sdlc.md`

1. **SDLC Phases** must be present with complete information:
   - Planning (requirements, design)
   - Development (coding, testing)
   - Testing (QA, validation)
   - Deployment (release, monitoring)
   - Maintenance (support, optimization)
   - Each phase with: icon, color, description, AI enhancements

2. **For Each Phase**:
   - **What**: Traditional activities in this phase
   - **How**: How to execute this phase
   - **Tools**: Tools commonly used
   - **AI Enhancements**: How AI assists in this phase
   - **Handover**: What transfers to next phase
   - **Best Practices**: Tips for success

3. **Visual Elements**:
   - Timeline structure (horizontal or vertical)
   - Phase markers/icons
   - Visual connectors between phases
   - Color coding for different phases

4. **Additional Content**:
   - Page intro explaining AI's role in SDLC
   - Related resources/links
   - Examples of AI tools for each phase

### E2E Test Coverage

1. **Page Load & Rendering**
   - Navigate to `/ai/sdlc`
   - Verify page renders without errors
   - Verify timeline visualization displays

2. **Phase Content Display**
   - Verify all SDLC phases display
   - Verify phase icons/markers render
   - Verify phase descriptions are complete
   - Verify AI enhancement sections display

3. **Timeline Visualization**
   - Verify timeline structure renders correctly
   - Verify phase connectors display
   - Verify color coding works
   - Verify responsive layout (if applicable)

4. **SidebarToc Functionality**
   - TOC auto-extracts phase headings
   - TOC links scroll to phases
   - Scroll spy highlights active phase
   - Last section can scroll to detection point

5. **Interactive Elements**
   - Phase sections are expandable/readable
   - Resource links work
   - Keyboard navigation works
   - No console errors

## Implementation Tasks

### Task 1: Verify JSON Completeness

**File**: `collections/_custom/sdlc.json`

**Subtasks**:

1. **Fetch original markdown**:
   - Download from `https://raw.githubusercontent.com/techhubms/techhub/main/sections/ai/sdlc.md`
   - Compare structure with JSON

2. **Verify Phase Structure**:
   - Ensure all SDLC phases present
   - Verify each phase has complete data:
     - id, name, icon, color
     - what, how, tools
     - aiEnhancements (detailed)
     - handover, bestPractices

3. **Verify Timeline Data**:
   - Phase order is correct
   - Phase relationships are clear
   - Visual data (icons, colors) is complete

4. **Update JSON if Missing Content**:
   - Add missing phases
   - Add missing AI enhancement details
   - Add missing tools/resources
   - Add visual element data

**Estimated Time**: 3-4 hours

### Task 2: Update Razor Component (if needed)

**File**: `src/TechHub.Web/Components/Pages/AISDLC.razor`

**Verify Rendering**:

- Timeline visualization renders correctly
- Phase markers/icons display
- Phase content sections render
- AI enhancement sections are clear
- Visual connectors work

**May Need**:

- Timeline component or CSS
- Phase card component
- Expandable section logic
- Icon rendering

**Estimated Time**: 2-3 hours

### Task 3: Create E2E Tests

**File**: `tests/TechHub.E2E.Tests/Web/AISDLCTests.cs`

**Test Methods**:

```csharp
[Fact]
public async Task SDLC_OnPageLoad_ShouldRenderTimeline()

[Fact]
public async Task SDLC_ShouldRender_WithSidebarToc()

[Fact]
public async Task SDLC_AllPhases_ShouldDisplay()

[Fact]
public async Task SDLC_PhaseContent_ShouldBeComplete()

[Fact]
public async Task SDLC_AIEnhancements_ShouldDisplay()

[Fact]
public async Task SDLC_TocLinks_ShouldScrollToPhases()

[Fact]
public async Task SDLC_Scrolling_ShouldUpdateActiveTocLink()

[Fact]
public async Task SDLC_LastSection_ShouldScroll_ToDetectionPoint()
```

**Estimated Time**: 2-3 hours

### Task 4: Run Tests & Validation

```powershell
Run -TestProject E2E.Tests -TestName SDLC
```

**Validation**:

- All tests pass
- No console errors
- Timeline displays correctly
- All phases render
- AI enhancements are visible

**Estimated Time**: 1 hour

## Success Metrics

- ✅ JSON contains all SDLC phases with complete data
- ✅ Timeline visualization renders correctly
- ✅ All phase sections have complete content (what/how/tools/AI/handover/practices)
- ✅ All E2E tests pass (8 test methods)
- ✅ Page loads in < 2 seconds
- ✅ No console errors
- ✅ Visual elements enhance understanding
- ✅ Keyboard navigation works

## Dependencies

- ✅ SidebarToc component (already exists)
- ✅ AISDLC.razor (already exists)
- ❓ Complete JSON content (needs verification)
- ❌ Timeline visualization component (may need to create)
- ❌ E2E tests (need to create)

## Known Issues

- JSON completeness unknown (needs verification)
- Timeline visualization may need custom component
- Phase data structure may need adjustment
- Visual elements need verification

## Out of Scope

- Interactive SDLC simulator or wizard
- Phase templates or downloads
- Project planning tools
- Mobile navigation (covered in spec 011)
- SEO optimization (covered in spec 005)

## Completion Checklist

- [ ] Original markdown downloaded and analyzed
- [ ] JSON verified complete with all phases
- [ ] Phase structure verified (what/how/tools/AI/handover/practices)
- [ ] Timeline visualization verified
- [ ] Razor component updated if needed
- [ ] E2E test file created with 8 test methods
- [ ] All tests pass without failures
- [ ] No console errors
- [ ] Visual elements render correctly
- [ ] Documentation updated
