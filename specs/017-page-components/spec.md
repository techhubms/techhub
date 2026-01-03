# 017-page-components

> **Feature**: Blazor page components for Home, Section Index, Collection, Item Detail, and custom pages with special frontmatter handling

## Overview

This specification defines all page-level Blazor components that compose the user-facing pages of Tech Hub. Pages are organized into standard page types (Home, Section Index, Collection, Item Detail) and custom pages with specialized behavior (GitHub Copilot Features with subscription tier filtering, Levels of Enlightenment learning path, A(i) to Z guides). All pages follow server-side rendering with client-side progressive enhancement, responsive layouts, and semantic HTML structure.

## Requirements

### Functional Requirements

#### Standard Page Types

**FR-1**: The system MUST provide Home page component displaying section cards grid and latest roundups  
**FR-2**: The system MUST provide Section Index page component with collection tabs and filtering controls  
**FR-3**: The system MUST provide Collection page component displaying filtered content lists  
**FR-4**: The system MUST provide Item Detail page component rendering markdown content with metadata  

#### Custom Page Support

**FR-5**: The system MUST support custom page components with markdown content and embedded components  
**FR-6**: The system MUST support GitHub Copilot Features page with subscription tier filtering  
**FR-7**: The system MUST support special frontmatter fields (`plans`, `ghes_support`, `alt-collection`)  
**FR-8**: The system MUST support Levels of Enlightenment page with embedded videos and learning path structure  
**FR-9**: The system MUST support A(i) to Z guide pages (AI fundamentals, applied AI, advanced AI)  
**FR-10**: The system MUST support SDLC guide page with software development lifecycle + AI content  
**FR-11**: The system MUST support DX Space page (Developer Experience space)  

#### Navigation & Layout

**FR-12**: The system MUST display section header with background image on all pages  
**FR-13**: The system MUST display collection tabs navigation on section index pages  
**FR-14**: The system MUST highlight active collection tab based on current page  
**FR-15**: The system MUST support hamburger menu on mobile viewports (< 768px)  
**FR-16**: The system MUST provide skip navigation links for accessibility  

#### Content Display

**FR-17**: The system MUST display content metadata (date, author, tags, categories)  
**FR-18**: The system MUST render markdown content with proper heading hierarchy  
**FR-19**: The system MUST display infinite scroll pagination on collection pages with configurable batch sizes  
**FR-20**: The system MUST support embedded YouTube videos in markdown content  
**FR-21**: The system MUST display related items based on tags/categories  

#### Filtering & Search

**FR-22**: The system MUST display date filter controls on collection pages  
**FR-23**: The system MUST display tag filter controls with active tags  
**FR-24**: The system MUST display text search input with debounced search  
**FR-25**: The system MUST update URL parameters when filters change  
**FR-26**: The system MUST restore filters from URL parameters on page load  

### Non-Functional Requirements

**NFR-1**: All pages MUST server-side render complete HTML for SEO and performance  
**NFR-2**: Client-side JavaScript MUST only enhance behavior (progressive enhancement)  
**NFR-3**: All pages MUST be fully functional without JavaScript enabled  
**NFR-4**: All pages MUST meet WCAG 2.1 AA accessibility standards  
**NFR-5**: All interactive elements MUST be keyboard navigable  
**NFR-6**: All pages MUST use semantic HTML structure  
**NFR-7**: All pages MUST be responsive (mobile, tablet, desktop breakpoints)  
**NFR-8**: Page load time MUST be under 2 seconds on 3G connection  
**NFR-9**: All components MUST use design system tokens for styling  
**NFR-10**: Component composition MUST be modular and reusable  

## Use Cases

### UC-1: View Home Page

**Actor**: Site Visitor  
**Precondition**: Visitor navigates to site root  
**Trigger**: Visitor loads home page  

**Flow**:

1. Visitor requests `/` URL
2. Server renders Home page with section cards grid
3. Server includes latest 3 roundups in HTML
4. Page displays immediately with all content visible
5. Client-side JavaScript enhances interactions (filter animations)
6. Visitor sees 8 section cards (All, GitHub Copilot, AI, ML, Azure, .NET, DevOps, Security)
7. Visitor sees latest roundups with "View All Roundups" link

**Postcondition**: Visitor sees complete home page content

### UC-2: Navigate to Section Index

**Actor**: Site Visitor  
**Precondition**: Home page is displayed  
**Trigger**: Visitor clicks section card  

**Flow**:

1. Visitor clicks "GitHub Copilot" section card
2. Browser navigates to `/github-copilot/`
3. Server renders Section Index page with GitHub Copilot header
4. Server includes collection tabs (News, Videos, Community, Blogs)
5. Server renders default collection (News) with first 20 items
6. Page displays section header image and title
7. Collection tabs are clickable, first tab highlighted
8. Content list displays with filtering controls

**Postcondition**: Visitor sees section index with default collection

### UC-3: Filter Content by Date

**Actor**: Site Visitor  
**Precondition**: Collection page is displayed  
**Trigger**: Visitor selects date filter "Last 30 days"  

**Flow**:

1. Visitor clicks "Last 30 days" date filter button
2. Client-side JavaScript calculates date range
3. JavaScript filters visible items to last 30 days
4. URL updates to include `?date=30d` parameter
5. Content list updates to show only matching items
6. Filter button shows active state
7. Item count updates to show filtered count

**Postcondition**: Only items from last 30 days are visible

### UC-4: View GitHub Copilot Feature (Future Date)

**Actor**: GitHub Copilot Subscriber  
**Precondition**: Visitor navigates to `/github-copilot/features.html`  
**Trigger**: Page loads with features list  

**Flow**:

1. Visitor requests GitHub Copilot Features page
2. Server renders features list from `_videos/ghc-features/` collection
3. Server checks each feature's `date` frontmatter
4. Features with future dates render as non-clickable (coming soon)
5. Features with past dates render as clickable links
6. Each feature displays subscription tier badges from `plans` frontmatter (Free, Pro, Business, Enterprise)
7. Features show GitHub Enterprise Server badge if `ghes_support: true`
8. Visitor can filter by subscription tier (All, Free, Pro, Business, Pro+, Enterprise)
9. Tab navigation highlights "Features" tab via `alt-collection: "features"`

**Postcondition**: Visitor sees features with appropriate click ability and tier badges

### UC-5: View Levels of Enlightenment Learning Path

**Actor**: Site Visitor  
**Precondition**: Visitor navigates to `/github-copilot/levels-of-enlightenment.html`  
**Trigger**: Page loads  

**Flow**:

1. Visitor requests Levels of Enlightenment page
2. Server renders markdown content with embedded video components
3. Page displays learning path structure with 5 levels (Novice, Apprentice, Competent, Proficient, Expert)
4. Each level includes description and embedded YouTube videos
5. Videos are embedded using YouTube component (responsive iframe)
6. Content is fully visible on page load (server-rendered)
7. Visitor scrolls through learning path

**Postcondition**: Visitor sees learning path with embedded videos

### UC-6: Mobile Navigation

**Actor**: Mobile User  
**Precondition**: User accesses site on mobile device (< 768px viewport)  
**Trigger**: Page loads on mobile  

**Flow**:

1. User loads any page on mobile device
2. Server detects mobile viewport (responsive CSS)
3. Page renders with hamburger menu icon instead of full navigation
4. User taps hamburger icon (44x44px touch target)
5. Mobile menu slides in from left/top
6. User sees all navigation options (sections, collections)
7. User taps section link
8. Menu closes and navigates to section

**Postcondition**: User navigates on mobile with touch-friendly controls

### UC-7: Search Content

**Actor**: Site Visitor  
**Precondition**: Collection page is displayed  
**Trigger**: Visitor types in search box  

**Flow**:

1. Visitor focuses text search input
2. Visitor types search query "blazor"
3. JavaScript debounces input (300ms delay)
4. After delay, JavaScript filters content by title/description/tags
5. Content list updates to show matching items
6. URL updates to include `?search=blazor` parameter
7. Search highlighting shows matched terms

**Postcondition**: Only items matching search query are visible

## Acceptance Criteria

### Home Page

**AC-1**: Given Home page request, when server renders page, then 8 section cards are visible in grid layout  
**AC-2**: Given Home page load, when JavaScript is disabled, then section cards remain clickable and navigate correctly  
**AC-3**: Given Home page on mobile, when viewport < 768px, then section cards stack vertically  
**AC-4**: Given Home page, when rendered, then latest 3 roundups are displayed with titles and dates  

### Section Index Pages

**AC-5**: Given Section Index request, when server renders page, then section header displays background image and title  
**AC-6**: Given collection tabs, when page loads, then default collection tab is highlighted  
**AC-7**: Given collection tab click, when JavaScript enabled, then content updates without full page reload  
**AC-8**: Given collection tab click, when JavaScript disabled, then browser navigates to collection URL  
**AC-9**: Given filtering controls, when no filters applied, then first batch of items (30-50, configurable) is visible with total count  

### Custom Pages

**AC-10**: Given GitHub Copilot Features page, when feature has future date, then feature is displayed but not clickable  
**AC-11**: Given feature with `plans: ["Pro", "Business"]`, when rendered, then Pro and Business badges are displayed  
**AC-12**: Given feature with `ghes_support: true`, when rendered, then GitHub Enterprise Server badge is displayed  
**AC-13**: Given Features page, when visitor filters by "Free", then only features with "Free" in plans array are visible  
**AC-14**: Given Features page with `alt-collection: "features"`, when tab navigation renders, then "Features" tab is highlighted  
**AC-15**: Given Levels of Enlightenment page, when rendered, then all embedded videos are visible on page load  
**AC-16**: Given custom markdown page, when rendered, then markdown content is converted to semantic HTML  

### Responsive Layout

**AC-17**: Given any page on desktop (> 1024px), when rendered, then full navigation and side-by-side layouts are used  
**AC-18**: Given any page on mobile (< 768px), when rendered, then hamburger menu and stacked layouts are used  
**AC-19**: Given touch device, when interactive element is tapped, then element is at least 44x44px  

### Accessibility

**AC-20**: Given any page, when keyboard user tabs through page, then all interactive elements are reachable  
**AC-21**: Given any page, when keyboard user presses Tab, then focus indicator is visible  
**AC-22**: Given any page, when screen reader user accesses page, then skip navigation link is announced first  
**AC-23**: Given any page, when rendered, then heading hierarchy is logical (h1 → h2 → h3, no skips)  

## Page Types Specification

### 1. Home Page Component

**Route**: `/`  
**Template**: `Pages/Index.razor`  

**Structure**:

```html
<header class="site-header">
  <nav class="main-navigation">
    <!-- Site navigation -->
  </nav>
</header>

<main class="home-content">
  <h1>Tech Hub</h1>
  
  <section class="sections-grid">
    <h2>Explore Topics</h2>
    <div class="grid grid-cols-4">
      <!-- 8 section cards -->
      <SectionCard section="All" />
      <SectionCard section="GitHub Copilot" />
      <SectionCard section="AI" />
      <!-- ... more sections -->
    </div>
  </section>
  
  <section class="latest-roundups">
    <h2>Latest Roundups</h2>
    <div class="roundups-list">
      <!-- 3 most recent roundups -->
      <RoundupCard roundup="roundup1" />
      <RoundupCard roundup="roundup2" />
      <RoundupCard roundup="roundup3" />
    </div>
    <a href="/roundups/" class="view-all">View All Roundups →</a>
  </section>
</main>

<footer class="site-footer">
  <!-- Footer content -->
</footer>
```

**Data Requirements**:

- `List<Section>` sections (from sections.json)
- `List<Item>` latestRoundups (from roundups collection, sorted by date DESC, take 3)

### 2. Section Index Page Component

**Route**: `/{section-path}/`  
**Template**: `Pages/Sections/Index.razor`  

**Structure**:

```html
<header class="section-header" style="background-image: url(...)">
  <h1>@SectionTitle</h1>
  <p>@SectionDescription</p>
</header>

<nav class="collection-tabs">
  <ul>
    <li><a href="/{section}/news/" class="@(IsActive("news"))">News</a></li>
    <li><a href="/{section}/videos/" class="@(IsActive("videos"))">Videos</a></li>
    <li><a href="/{section}/community/" class="@(IsActive("community"))">Community</a></li>
    <li><a href="/{section}/blogs/" class="@(IsActive("blogs"))">Blogs</a></li>
    <!-- Custom tabs if alt-collection -->
  </ul>
</nav>

<section class="filters">
  <DateFilter />
  <TagFilter />
  <TextSearch />
</section>

<main class="content-list">
  <div class="items">
    <!-- Initial batch of items (30-50, configurable) -->
    @foreach (var item in Items) {
      <ItemCard item="@item" />
    }
  </div>
  
  @if (HasMore) {
    <div id="infinite-scroll-sentinel"></div>
    <div class="loading-indicator">Loading...</div>
  }
</main>
```

**Data Requirements**:

- `Section` currentSection (from sections.json)
- `Collection` currentCollection (determined from URL)
- `List<Item>` items (from API, filtered by section/collection/category)
- `int` totalCount (for pagination)
- `Dictionary<string, int>` tagCounts (for tag filter)

### 3. Collection Page Component

**Route**: `/{section}/{collection}/`  
**Template**: `Pages/Collections/Index.razor`  

**Structure**:

Same as Section Index, but:

- Collection-specific heading
- Pre-filtered to single collection
- Breadcrumb: Home → Section → Collection

**Data Requirements**:

- Same as Section Index but scoped to single collection

### 4. Item Detail Page Component

**Route**: `/{year}-{month}-{day}-{slug}.html`  
**Template**: `Pages/Items/Detail.razor`  

**Structure**:

```html
<article class="item-detail">
  <header class="item-header">
    <h1>@Item.Title</h1>
    <div class="item-metadata">
      <time datetime="@Item.Date.ToString("yyyy-MM-dd")">@Item.FormattedDate</time>
      <span class="author">@Item.Author</span>
      <div class="tags">
        @foreach (var tag in Item.Tags) {
          <a href="?tag=@tag" class="tag">@tag</a>
        }
      </div>
    </div>
  </header>
  
  <div class="item-content">
    @((MarkupString)Item.HtmlContent)
  </div>
  
  <aside class="related-items">
    <h2>Related Content</h2>
    <div class="related-list">
      @foreach (var related in RelatedItems) {
        <ItemCard item="@related" />
      }
    </div>
  </aside>
</article>
```

**Data Requirements**:

- `Item` item (full content and metadata)
- `List<Item>` relatedItems (based on shared tags/categories)

### 5. GitHub Copilot Features Page

**Route**: `/github-copilot/features.html`  
**Template**: `Pages/GitHubCopilot/Features.razor`  

**Structure**:

```html
<header class="section-header">
  <h1>GitHub Copilot Features</h1>
</header>

<nav class="collection-tabs">
  <!-- Highlights "Features" tab via alt-collection -->
  <li><a href="/github-copilot/news/">News</a></li>
  <li><a href="/github-copilot/features.html" class="active">Features</a></li>
  <li><a href="/github-copilot/videos/">Videos</a></li>
</nav>

<section class="tier-filters">
  <button data-tier="all" class="active">All</button>
  <button data-tier="free">Free</button>
  <button data-tier="pro">Pro</button>
  <button data-tier="business">Business</button>
  <button data-tier="pro-plus">Pro+</button>
  <button data-tier="enterprise">Enterprise</button>
</section>

<main class="features-list">
  @foreach (var feature in Features) {
    <article class="feature-card @(feature.IsFuture ? "future" : "")">
      @if (feature.IsFuture) {
        <span class="coming-soon">Coming Soon</span>
      }
      
      <h2>
        @if (!feature.IsFuture) {
          <a href="@feature.Url">@feature.Title</a>
        } else {
          @feature.Title
        }
      </h2>
      
      <div class="badges">
        @foreach (var plan in feature.Plans) {
          <span class="badge badge-@plan.ToLower()">@plan</span>
        }
        @if (feature.GhesSupport) {
          <span class="badge badge-ghes">GHES</span>
        }
      </div>
      
      <p>@feature.Description</p>
    </article>
  }
</main>
```

**Data Requirements**:

- `List<Feature>` features (from `_videos/ghc-features/` collection)
- Each feature includes:
  - `title`, `date`, `url`, `description`
  - `plans: ["Free"|"Pro"|"Business"|"Pro+"|"Enterprise"]` (array)
  - `ghes_support: true|false` (boolean)
  - `alt-collection: "features"` (for tab highlighting)

**Filtering Logic**:

- `IsFuture = feature.Date > DateTime.Now` (using Europe/Brussels timezone)
- Filter by tier: `feature.Plans.Contains(selectedTier)`
- "Pro+" tier = features with `["Pro", "Business"]` plans

### 6. Levels of Enlightenment Page

**Route**: `/github-copilot/levels-of-enlightenment.html`  
**Template**: `Pages/GitHubCopilot/LevelsOfEnlightenment.razor`  

**Structure**:

```html
<article class="learning-path">
  <h1>Levels of Enlightenment</h1>
  
  @((MarkupString)PageContent)
  <!-- Markdown content with embedded YouTube components -->
  <!-- Example: {% youtube 'VIDEO_ID' %} becomes <YouTubeEmbed videoId="VIDEO_ID" /> -->
</article>
```

**Data Requirements**:

- `string` pageContent (markdown rendered to HTML)
- YouTube embeds use `YouTubeEmbed` component (from spec 009)

### 7. A(i) to Z Guide Pages

**Routes**:

- `/ai/genai-basics.html`
- `/ai/genai-applied.html`
- `/ai/genai-advanced.html`

**Structure**:

Same as Levels of Enlightenment (markdown content with embedded components)

### 8. SDLC Guide Page

**Route**: `/ai/sdlc.html`  
**Template**: `Pages/AI/SDLC.razor`  

**Structure**:

```html
<article class="sdlc-guide">
  <h1>Software Development Lifecycle with AI</h1>
  @((MarkupString)PageContent)
</article>
```

### 9. DX Space Page

**Route**: `/devops/dx-space.html`  
**Template**: `Pages/DevOps/DXSpace.razor`  

**Structure**:

```html
<article class="dx-space">
  <h1>Developer Experience Space</h1>
  @((MarkupString)PageContent)
</article>
```

## Special Frontmatter Handling

### Standard Frontmatter (All Items)

```yaml
---
title: "Item Title"
date: 2025-01-15
author: "Author Name"
categories: ["AI", "GitHub Copilot"]
tags: ["tag1", "tag2"]
excerpt: "Short description"
---
```

### GitHub Copilot Features Frontmatter

```yaml
---
title: "Feature Name"
date: 2025-02-01  # Future date = non-clickable
plans: ["Pro", "Business", "Enterprise"]  # Subscription tiers
ghes_support: true  # GitHub Enterprise Server support
alt-collection: "features"  # Highlights Features tab in navigation
categories: ["GitHub Copilot"]
tags: ["copilot", "features"]
---
```

**Tier Badge Rendering**:

- `Free` → Green badge
- `Pro` → Blue badge
- `Business` → Purple badge
- `Enterprise` → Red badge
- `GHES` (ghes_support: true) → Gray badge

**Pro+ Filter Logic**:

- "Pro+" tier shows features with BOTH "Pro" AND "Business" in plans array
- Logical: `plans.Contains("Pro") && plans.Contains("Business")`

### Alt-Collection for Custom Tabs

Used when custom page should highlight a collection tab:

```yaml
---
alt-collection: "features"  # Highlights "Features" tab even though URL is not /github-copilot/features/
---
```

## Responsive Layout Requirements

### Breakpoints (from spec 011)

- Mobile: < 768px
- Tablet: 768px - 1024px
- Desktop: > 1024px

### Layout Adaptations

**Home Page**:

- Desktop: 4-column section grid
- Tablet: 2-column section grid
- Mobile: 1-column section grid (stacked)

**Section Index**:

- Desktop: Filters sidebar (left), content (right)
- Tablet: Filters top, content below
- Mobile: Filters collapsible accordion, content below

**Navigation**:

- Desktop: Full horizontal navigation
- Tablet: Condensed navigation
- Mobile: Hamburger menu (44x44px touch target)

**Content Cards**:

- Desktop: 3-column grid
- Tablet: 2-column grid
- Mobile: 1-column stack

## Component Composition

### Shared Components (from spec 009)

- `SectionCard` - Section card with image and link
- `ItemCard` - Content item card with metadata
- `RoundupCard` - Roundup summary card
- `DateFilter` - Date range filter buttons
- `TagFilter` - Tag filter chips
- `TextSearch` - Search input with debounce
- `YouTubeEmbed` - Responsive YouTube video embed
- `InfiniteScroll` - Progressive content loading with intersection observer

### Page-Specific Components

- `SectionHeader` - Section hero with background image
- `CollectionTabs` - Horizontal tab navigation
- `FeatureCard` - GitHub Copilot feature with tier badges
- `TierFilter` - Subscription tier filter buttons
- `HamburgerMenu` - Mobile navigation menu

## Server-Side Rendering Pattern

**Critical Principle**: All content MUST be server-rendered for SEO, performance, and accessibility.

### Server Responsibilities

1. **Fetch data from API** (sections, collections, items)
2. **Render complete HTML** (all visible content in initial response)
3. **Apply default filters** (date, tags from URL parameters)
4. **Calculate pagination** (first batch of 30-50 items, configurable)
5. **Render metadata** (Open Graph, structured data)

### Client Responsibilities (Progressive Enhancement)

1. **Enhance filtering** (instant updates without page reload)
2. **Update URL** (push state for browser history)
3. **Animate transitions** (fade in/out content changes)
4. **Handle infinite scroll** (load more pagination)
5. **Debounce search input** (delay API calls)

### Fallback Behavior (No JavaScript)

- Filters trigger form submission (full page reload)
- Pagination uses traditional page links
- Search submits form to new URL with query parameter
- All functionality remains intact, just less smooth

## Edge Cases

**EC-1**: No items match filter criteria → Display "No results found" message with clear filters button  
**EC-2**: Future-dated feature on Features page → Render non-clickable with "Coming Soon" badge  
**EC-3**: Feature missing `plans` frontmatter → Display as "All Tiers" or default to Free  
**EC-4**: YouTube video ID invalid → Display placeholder with error message  
**EC-5**: Mobile device with hover support (hybrid) → Support both touch and mouse interactions  
**EC-6**: Browser doesn't support JavaScript → All functionality works via form submission and links  
**EC-7**: Section has no collections → Hide collection tabs, display message  
**EC-8**: Collection has no items → Display "No content yet" message  
**EC-9**: Deep-linked URL with invalid filters → Reset to default filters, show notification  
**EC-10**: User zooms > 200% → Layout scales proportionally without breaking  

## Migration Notes

**From Jekyll**:

- Jekyll generates static HTML files for each page
- Current layouts: `home.html`, no section index layout (auto-generated), item layout from collection
- **MIGRATION**: Convert Jekyll layouts to Blazor `@page` components
- **MIGRATION**: Extract special frontmatter handling logic from Jekyll plugins
- **MIGRATION**: Preserve exact HTML structure for visual consistency
- **PRESERVE**: All URLs must remain identical for SEO (see spec 004)

## Testing Strategy

### Unit Tests

- Test each page component renders correctly with mock data
- Test frontmatter parsing and special field handling
- Test filter logic (date ranges, tag matching, tier filtering)
- Test infinite scroll batch loading and intersection observer
- Test future date detection for features

### Integration Tests

- Test page navigation flows (home → section → collection → item)
- Test filter interactions (applying, clearing, combining filters)
- Test tab navigation (collection switching)
- Test search with filtering

### Visual Regression Tests

- Capture screenshots of each page type at each breakpoint
- Compare against baseline after changes
- Flag any unintended visual differences

### Accessibility Tests

- Keyboard navigation (Tab through all interactive elements)
- Screen reader compatibility (heading hierarchy, ARIA labels)
- Focus indicators (visible on all focusable elements)
- Touch target sizes on mobile (minimum 44x44px)

### Cross-Browser Tests

- Test in Chrome, Firefox, Safari, Edge (latest 2 versions)
- Test responsive behavior at breakpoints
- Test progressive enhancement (JavaScript disabled)

## Open Questions

None - all page requirements are clearly defined

## References

- [004-url-routing Spec](../009-url-routing/spec.md) - URL structure and routing
- [014-blazor-components Spec](../014-blazor-components/spec.md) - Reusable components
- [016-visual-design-system Spec](../016-visual-design-system/spec.md) - Design tokens and styling
- [018-content-rendering Spec](../018-content-rendering/spec.md) - Markdown rendering
- [019-filtering-system Spec](../019-filtering-system/spec.md) - Filtering logic
- `/github-copilot/features.md` - Current features page
- `/github-copilot/levels-of-enlightenment.md` - Current learning path page
