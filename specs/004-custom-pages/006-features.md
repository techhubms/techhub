# Custom Page: GitHub Copilot Features

**Page**: GitHub Copilot Features  
**URL**: `/github-copilot/features`  
**Priority**: P1 (Most Visited)  
**Status**: Razor ✅ | JSON ❓ (Needs Verification) | E2E Tests ❌  
**Estimated Effort**: 6-8 hours

## Overview

The GitHub Copilot Features page is the **most visited custom page** and serves as the primary entry point for developers learning about GitHub Copilot capabilities. It showcases features organized into groups with visual cards.

**Live Site Reference**: <https://tech.hub.ms/github-copilot/features>

## Current Status

✅ **Complete**:

- Razor component exists: `src/TechHub.Web/Components/Pages/GitHubCopilotFeatures.razor`
- SidebarToc component integrated
- JSON data file: `collections/_custom/features.json`

❓ **Needs Verification**:

- JSON content completeness vs original markdown
- Feature card structure and styling
- Subscription tier information
- Feature group organization

❌ **Missing Tests**:

- No E2E tests exist for Features page

## Acceptance Criteria

### JSON Content Verification

**Source**: `https://raw.githubusercontent.com/techhubms/techhub/main/sections/github-copilot/features.md`

1. **Subscription Tiers** must be present with complete information:
   - Free tier (features and limitations)
   - Pro tier (pricing, features)
   - Business tier (pricing, features, enterprise capabilities)
   - Enterprise tier (pricing, features, advanced capabilities)

2. **Feature Groups** must be organized:
   - Core Features (available to all tiers)
   - Advanced Features (Pro and above)
   - Enterprise Features (Business/Enterprise only)
   - Each feature with: icon, title, description, documentation link

3. **Feature Cards** must include:
   - Feature icon (SVG or font icon)
   - Feature title
   - Feature description
   - Link to documentation
   - Tier availability indicator (if applicable)

4. **Additional Content**:
   - Page intro/overview
   - Links to pricing page
   - Links to documentation
   - Related resources

### E2E Test Coverage

1. **Page Load & Rendering**
   - Navigate to `/github-copilot/features`
   - Verify page renders without errors
   - Verify header with background image displays

2. **Feature Cards Display**
   - Verify all feature groups display
   - Verify feature cards render with icons, titles, descriptions
   - Verify cards are clickable/interactive
   - Verify hover states work

3. **Subscription Tiers**
   - Verify subscription tier information displays
   - Verify pricing information is visible
   - Verify tier comparison is clear

4. **SidebarToc Functionality**
   - TOC auto-extracts feature group headings
   - TOC links scroll to feature groups
   - Scroll spy highlights active group
   - Last section can scroll to detection point

5. **Interactive Elements**
   - Feature documentation links work
   - Pricing page links work
   - Cards respond to keyboard navigation
   - No console errors

## Implementation Tasks

### Task 1: Verify JSON Completeness

**File**: `collections/_custom/features.json`

**Subtasks**:

1. **Fetch original markdown**:
   - Download from `https://raw.githubusercontent.com/techhubms/techhub/main/sections/github-copilot/features.md`
   - Compare structure with JSON

2. **Verify Subscription Tiers**:
   - Ensure all 4 tiers present (Free, Pro, Business, Enterprise)
   - Verify pricing information is current
   - Verify feature lists are complete

3. **Verify Feature Cards**:
   - Count features in markdown vs JSON
   - Ensure all features have: icon, title, description, link
   - Verify feature grouping matches markdown

4. **Update JSON if Missing Content**:
   - Add missing features
   - Add missing subscription information
   - Add missing links/resources

**Estimated Time**: 2-3 hours

### Task 2: Update Razor Component (if needed)

**File**: `src/TechHub.Web/Components/Pages/GitHubCopilotFeatures.razor`

**Verify Rendering**:

- Feature cards display in grid layout
- Icons render correctly
- Subscription tiers display clearly
- Pricing information formats correctly
- Links are styled as buttons/cards

**May Need**:

- Feature card component styling
- Tier comparison table
- Icon rendering logic

**Estimated Time**: 2-3 hours

### Task 3: Create E2E Tests

**File**: `tests/TechHub.E2E.Tests/Web/GitHubCopilotFeaturesTests.cs`

**Test Methods**:

```csharp
[Fact]
public async Task Features_OnPageLoad_ShouldRenderContent()

[Fact]
public async Task Features_ShouldRender_WithSidebarToc()

[Fact]
public async Task Features_FeatureCards_ShouldDisplay()

[Fact]
public async Task Features_FeatureCards_ShouldBeClickable()

[Fact]
public async Task Features_SubscriptionTiers_ShouldDisplay()

[Fact]
public async Task Features_TocLinks_ShouldScrollToGroups()

[Fact]
public async Task Features_Scrolling_ShouldUpdateActiveTocLink()

[Fact]
public async Task Features_LastSection_ShouldScroll_ToDetectionPoint()
```

**Estimated Time**: 2-3 hours

### Task 4: Run Tests & Validation

```powershell
Run -TestProject E2E.Tests -TestName Features
```

**Validation**:

- All tests pass
- No console errors
- Feature cards display correctly
- Subscription information is accurate
- Links work

**Estimated Time**: 1 hour

## Success Metrics

- ✅ JSON contains all features from markdown
- ✅ All 4 subscription tiers with complete information
- ✅ All feature cards display with icons, titles, descriptions, links
- ✅ All E2E tests pass (8 test methods)
- ✅ Page loads in < 2 seconds
- ✅ No console errors
- ✅ Feature cards are interactive (hover, click)
- ✅ Keyboard navigation works

## Dependencies

- ✅ SidebarToc component (already exists)
- ✅ GitHubCopilotFeatures.razor (already exists)
- ❓ Complete JSON content (needs verification)
- ❌ E2E tests (need to create)

## Known Issues

- JSON completeness unknown (needs verification)
- Feature card styling may need adjustment
- Icon rendering needs verification
- Subscription pricing may need updating

## Out of Scope

- Subscription management or purchase flow
- Feature demos or interactive playgrounds
- Mobile navigation (covered in spec 011)
- SEO optimization (covered in spec 005)
- Analytics tracking (covered in spec 006)

## Completion Checklist

- [ ] Original markdown downloaded and analyzed
- [ ] JSON verified complete with all features
- [ ] All 4 subscription tiers verified
- [ ] Feature cards verified (icons, titles, descriptions, links)
- [ ] Razor component updated if needed
- [ ] E2E test file created with 8 test methods
- [ ] All tests pass without failures
- [ ] No console errors
- [ ] Feature cards interactive
- [ ] Documentation updated
