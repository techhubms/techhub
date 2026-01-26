# Custom Page: GitHub Copilot Features

**Page**: GitHub Copilot Features  
**URL**: `/github-copilot/features`  
**Priority**: P1 (Most Visited)  
**Status**: Razor ✅ (Enhanced with Video Integration) | JSON ❓ (Needs Verification) | E2E Tests ⚠️ (Partial - Basic tests exist, new feature tests needed)  
**Estimated Effort**: 6-8 hours (4-5 hours remaining)

## Overview

The GitHub Copilot Features page is the **most visited custom page** and serves as the primary entry point for developers learning about GitHub Copilot capabilities. It showcases features organized into groups with visual cards.

**Live Site Reference**: <https://tech.hub.ms/github-copilot/features>

## Current Status

✅ **Complete**:

- Razor component exists: `src/TechHub.Web/Components/Pages/GitHubCopilotFeatures.razor`
- SidebarToc component integrated with h2 AND h3 elements
- Enhanced with video integration from `collections/_videos/ghc-features/`
- Domain models updated with Plans (IReadOnlyList<string>) and GhesSupport (bool)
- FileBasedContentRepository parses 'plans' and 'ghes_support' frontmatter
- API endpoints map Plans and GhesSupport to DTOs
- Feature cards render with GHES badges, video badges, and clickable links
- Interactive sidebar filters: "Show features with GHES support" and "Show features with videos"
- TOC functionality working with h2 and h3 headings
- Basic E2E tests exist (page load, rendering, TOC, no console errors)

❓ **Needs Verification**:

- JSON content completeness vs original markdown (`collections/_custom/features.json`)
- Feature card structure matches original design intent
- Subscription tier information accuracy

❌ **Missing E2E Tests**:

- Feature card video badge display (when date <= now)
- Feature card GHES badge display (when GhesSupport=true)
- Feature card "Coming Soon" state (when date > now)
- Feature card click navigation to `/github-copilot/videos/{slug}`
- Sidebar filter toggle behavior (GHES filter)
- Sidebar filter toggle behavior (Videos filter)
- Combined filter scenarios

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

1. **Page Load & Rendering** ✅ (Basic tests exist)
   - Navigate to `/github-copilot/features`
   - Verify page renders without errors
   - Verify header with background image displays

2. **Feature Cards Display** ⚠️ (Needs enhancement for video integration)
   - Verify all feature groups display
   - Verify feature cards render with icons, titles, descriptions
   - ❌ **NEEDED**: Verify video badges appear when date <= now
   - ❌ **NEEDED**: Verify GHES badges appear when GhesSupport=true
   - ❌ **NEEDED**: Verify "Coming Soon" state for future-dated features
   - ❌ **NEEDED**: Verify cards are clickable (navigate to video page)
   - ❌ **NEEDED**: Verify future features are NOT clickable

3. **Subscription Tiers** ✅ (Existing tests cover this)
   - Verify subscription tier information displays
   - Verify pricing information is visible
   - Verify tier comparison is clear

4. **SidebarToc Functionality** ✅ (Existing tests cover this)
   - TOC auto-extracts feature group headings (h2 and h3)
   - TOC links scroll to feature groups
   - Scroll spy highlights active group
   - Last section can scroll to detection point

5. **Interactive Elements** ⚠️ (Partially covered)
   - ❌ **NEEDED**: GHES filter toggle works correctly
   - ❌ **NEEDED**: Videos filter toggle works correctly
   - ❌ **NEEDED**: Combined filter scenarios
   - ❌ **NEEDED**: Filter state persists during scrolling
   - ✅ Pricing page links work (covered by existing tests)
   - ✅ Cards respond to keyboard navigation (accessibility tests exist)
   - ✅ No console errors (covered by existing tests)

## Implementation Tasks

### Task 1: Verify JSON Completeness (REQUIRED) ⚠️ **STILL NEEDED**

**File**: `collections/_custom/features.json`

**CRITICAL**: This is MANDATORY - JSON must match original markdown exactly

**Status**: ❌ Not started

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

### Task 2: Video Integration Enhancement ✅ **COMPLETE**

**Files**: 
- `src/TechHub.Web/Components/Pages/GitHubCopilotFeatures.razor`
- `src/TechHub.Core/Models/ContentItem.cs`
- `src/TechHub.Core/DTOs/ContentItemDto.cs`
- `src/TechHub.Infrastructure/Repositories/FileBasedContentRepository.cs`
- `src/TechHub.Api/Endpoints/ContentEndpoints.cs`

**Status**: ✅ Complete

**Implemented**:
- ✅ Added Plans (IReadOnlyList<string>) and GhesSupport (bool) to domain models
- ✅ FileBasedContentRepository parses 'plans' and 'ghes_support' from frontmatter
- ✅ API endpoints map Plans and GhesSupport to DTOs
- ✅ GitHubCopilotFeatures page queries ghc-features collection
- ✅ Feature cards render with video badges (when date <= now)
- ✅ Feature cards render with GHES badges (when GhesSupport=true)
- ✅ Feature cards show "Coming Soon" for future-dated content
- ✅ Clickable cards navigate to `/github-copilot/videos/{slug}`
- ✅ Future-dated features are NOT clickable
- ✅ Interactive sidebar filters for GHES and Videos
- ✅ TOC includes h3 elements for all feature titles
- ✅ Fixed DXSpace E2E test race condition
- ✅ Fixed PowerShell pipeline bug in TechHubRunner.psm1

### Task 3: Create E2E Tests ⚠️ **PARTIALLY COMPLETE**

**File**: `tests/TechHub.E2E.Tests/Web/GitHubCopilotFeaturesTests.cs`

**Status**: ⚠️ Basic tests exist, need enhancement for video integration

**Existing Tests** (from GitHubCopilotFeaturesTests.cs):
- ✅ `GitHubCopilotFeatures_ShouldLoad_Successfully`
- ✅ `GitHubCopilotFeatures_ShouldDisplay_Content`
- ✅ `GitHubCopilotFeatures_ShouldDisplay_SubscriptionTiers`
- ✅ `GitHubCopilotFeatures_SubscriptionTiers_ShouldDisplay_PricingInfo`
- ✅ `GitHubCopilotFeatures_TierCards_ShouldBe_VisuallyDistinct`
- ✅ `GitHubCopilotFeatures_Intro_ShouldDisplay_LinksAndNote`
- ✅ `GitHubCopilotFeatures_ShouldDisplay_FeatureSections`
- ✅ `GitHubCopilotFeatures_ShouldHave_NoConsoleErrors`

**Still Needed - Video Integration Tests**:

```csharp
[Fact]
public async Task Features_FeatureCards_WithVideo_ShouldDisplay_VideoBadge()
// Verify features with date <= now show video badge

[Fact]
public async Task Features_FeatureCards_WithGhesSupport_ShouldDisplay_GhesBadge()
// Verify features with GhesSupport=true show GHES badge

[Fact]
public async Task Features_FeatureCards_FutureDate_ShouldDisplay_ComingSoon()
// Verify features with date > now show "Coming Soon" and are not clickable

[Fact]
public async Task Features_FeatureCards_WithVideo_ShouldNavigate_ToVideoPage()
// Verify clicking feature card navigates to /github-copilot/videos/{slug}

[Fact]
public async Task Features_FilterToggle_GhesOnly_ShouldFilter_Correctly()
// Verify GHES filter toggle shows only GHES-supported features

[Fact]
public async Task Features_FilterToggle_VideosOnly_ShouldFilter_Correctly()
// Verify Videos filter toggle shows only features with videos (date <= now)

[Fact]
public async Task Features_FilterToggle_Combined_ShouldFilter_Correctly()
// Verify both filters can work together (AND logic)
```

**Estimated Time**: 2-3 hours

### Task 4: Run Tests & Validation ⚠️ **IN PROGRESS**

```powershell
Run -TestProject E2E.Tests -TestName Features
```

**Status**: ⚠️ Basic tests passing, new tests not yet written

**Validation**:

- ✅ Existing 8 tests pass
- ✅ No console errors on page load
- ✅ Subscription information displays correctly
- ❌ Video badge tests not yet run (not written)
- ❌ GHES badge tests not yet run (not written)
- ❌ Filter toggle tests not yet run (not written)

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
- [x] Razor component updated with video integration
- [x] Domain models updated (Plans, GhesSupport properties)
- [x] FileBasedContentRepository parses frontmatter (plans, ghes_support)
- [x] API endpoints map new properties to DTOs
- [x] Feature cards render with video badges
- [x] Feature cards render with GHES badges
- [x] Feature cards show "Coming Soon" for future content
- [x] Clickable cards navigate to video pages
- [x] Sidebar filter toggles implemented (GHES, Videos)
- [x] TOC includes h3 elements for features
- [x] Basic E2E tests pass (8 existing tests)
- [ ] Video badge E2E tests created
- [ ] GHES badge E2E tests created
- [ ] "Coming Soon" state E2E tests created
- [ ] Filter toggle E2E tests created
- [ ] All new E2E tests pass without failures
- [x] No console errors on page load
- [ ] Feature cards interactive behavior fully tested
- [ ] Documentation updated

**Current Progress**: ~60% complete
**Remaining Work**: JSON verification, new E2E tests for video integration features
