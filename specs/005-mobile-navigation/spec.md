# Feature Specification: Mobile Navigation & Responsive Layout

**Feature Branch**: `005-mobile-navigation`
**Created**: 2026-01-16
**Updated**: 2026-02-19
**Status**: Draft
**Input**: Implement responsive navigation and layout system that adapts to mobile devices using a hamburger menu with hierarchical sections, collapsible sidebar, and compact sticky header

## Current State

The site currently has three navigation/layout elements that need mobile adaptation:

1. **NavHeader** (`NavHeader.razor`) — Sticky primary nav at `top: 0` (76px tall) with logo + 8 section links + About link. On mobile (≤768px) links simply stack vertically, pushing content far down. No hamburger menu exists.
2. **SubNav** (`SubNav.razor`) — Sticky secondary nav at `top: 76px` (~54px tall) showing collections (All, News, Blogs, Videos, Community) and custom pages per section. Uses `overflow-x: auto` for horizontal scrolling.
3. **Sidebar** (`<aside class="sidebar">`) — 300px sidebar used on Home, Section/Collection, and ContentItem pages containing search, date range slider, tag cloud, RSS links, and/or table of contents. At ≤1024px it drops below content as a horizontal grid; at ≤640px it goes single-column.

**Total sticky header space on mobile: ~130px** (NavHeader 76px + SubNav 54px), consuming ~20% of a typical mobile viewport.

**Components involved**: `Header.razor` (orchestrator, uses `display: contents`), `NavHeader.razor`, `SubNav.razor`, `SectionBanner.razor`, plus sidebar components (`SidebarSearch`, `SidebarTagCloud`, `SidebarToc`, `SidebarRssLinks`).

## User Scenarios & Testing

### User Story 1 — Hamburger Menu with Hierarchical Sections (Priority: P1)

Mobile users can access all navigation through a single hamburger menu that combines the NavHeader sections and SubNav collections into an expandable hierarchy, without cluttering the small screen.

**Why this priority**: Essential for mobile UX. Currently the full nav + subnav consume too much vertical space and the nav links stack vertically without any toggle mechanism.

**Independent Test**: Open site on mobile viewport (≤1024px), verify hamburger icon visible and horizontal nav links hidden. Tap hamburger, verify menu slides in with all sections. Tap a section, verify it expands to show its collections and custom pages.

**Acceptance Scenarios**:

1. **Given** I'm on a mobile viewport (≤1024px), **When** I load the site, **Then** I see only the logo and a hamburger icon in the header (compact ~50px)
2. **Given** the hamburger icon is visible, **When** I tap it, **Then** the menu slides in from the right with smooth animation
3. **Given** the menu is open, **When** I view a section (e.g., "GitHub Copilot"), **Then** I can tap it to expand and see sub-items: All, News, Blogs, Videos, Community, and any custom pages (Features, Handbook, Levels of Enlightenment, VS Code Updates)
4. **Given** a section is expanded, **When** I tap a sub-item link, **Then** I navigate to that page and the menu closes automatically
5. **Given** the menu is open, **When** I tap "About", **Then** I navigate to the About page (no sub-items) and the menu closes

---

### User Story 2 — Close Menu Easily (Priority: P1)

Mobile users can close the navigation menu using multiple methods so they can quickly return to content.

**Why this priority**: Critical UX — users must be able to dismiss the menu easily.

**Independent Test**: Open hamburger menu, try closing via: (1) tap outside menu overlay, (2) tap hamburger icon again, (3) press Escape key, verify all methods close the menu.

**Acceptance Scenarios**:

1. **Given** the menu is open, **When** I tap anywhere on the overlay (outside the menu panel), **Then** the menu closes with smooth animation
2. **Given** the menu is open, **When** I tap the hamburger icon again, **Then** the menu closes and icon animates back to three lines
3. **Given** the menu is open, **When** I press the Escape key, **Then** the menu closes immediately
4. **Given** the menu is open, **When** I navigate to a page via a menu link, **Then** the menu automatically closes

---

### User Story 3 — Collapsible Sidebar on Mobile (Priority: P1)

On mobile viewports, the sidebar (filters, search, tags, ToC) appears **above** the main content in a collapsed state. Users can expand it to access filters and collapse it again. A small toggle bar remains sticky for quick access.

**Why this priority**: The sidebar contains critical interactive elements (search, filtering, tag cloud, ToC) that must be accessible on mobile. Currently it drops below content at ≤1024px, making it undiscoverable.

**Independent Test**: Open a section page on mobile viewport (≤1024px), verify sidebar content is collapsed above main content with a toggle button visible. Tap the toggle, verify sidebar expands inline showing search/filters/tags. Tap toggle again to collapse.

**Acceptance Scenarios**:

1. **Given** I'm on a section page on mobile (≤1024px), **When** I load the page, **Then** I see a collapsible toggle bar labeled "Filters & Search" above the main content
2. **Given** the sidebar is collapsed, **When** I tap the toggle, **Then** the sidebar content expands inline (pushes content down) showing search, date range slider, tag cloud, and RSS links
3. **Given** the sidebar is expanded, **When** I tap the toggle again, **Then** the sidebar collapses and content scrolls back up
4. **Given** I have active tag filters selected, **When** I collapse the sidebar, **Then** the filters remain applied to the content below
5. **Given** I'm on a content detail page with a ToC sidebar, **When** I view on mobile, **Then** I see a collapsible "Table of Contents" toggle instead of "Filters & Search"
6. **Given** I'm on the homepage on mobile, **When** I view the sidebar area, **Then** I see a collapsible toggle for the homepage sidebar sections (Latest Roundup, Latest Content, RSS, Popular Tags)

---

### User Story 4 — Compact Sticky Header on Mobile (Priority: P1)

On mobile, the NavHeader shrinks to a compact bar (logo + hamburger, ~50px) that remains sticky. The SubNav is not sticky — its items are accessible through the hamburger menu instead.

**Why this priority**: Maximizes content viewport on mobile. 130px of sticky headers is excessive on small screens.

**Independent Test**: Load a section page on mobile viewport (≤1024px), verify only the compact header (~50px) is sticky. Scroll down, verify SubNav scrolls away. Verify SubNav items are accessible via hamburger menu.

**Acceptance Scenarios**:

1. **Given** I'm on mobile (≤1024px), **When** I load a section page, **Then** the NavHeader is a compact bar (~50px) with just logo and hamburger icon, and it remains sticky at `top: 0`
2. **Given** I'm on mobile, **When** I scroll down, **Then** the SubNav (and SectionBanner) scroll away with the content — they are NOT sticky
3. **Given** I'm on desktop (>1024px), **When** I load the site, **Then** both NavHeader (76px) and SubNav (54px) are sticky as they are today
4. **Given** I'm on mobile and the SubNav has scrolled off-screen, **When** I tap the hamburger, **Then** I can access the same section/collection links that were in the SubNav

---

### User Story 5 — Keyboard Navigation (Priority: P2)

Keyboard users can open the menu, navigate through sections and sub-items, and close the menu using only keyboard.

**Why this priority**: Accessibility requirement (WCAG 2.1 AA).

**Independent Test**: Use only keyboard (Tab, Enter, Escape, Arrow keys), verify can open menu, expand sections, navigate sub-items, and close menu.

**Acceptance Scenarios**:

1. **Given** I'm using only keyboard, **When** I press Tab to focus the hamburger button and press Enter, **Then** the menu opens
2. **Given** the menu is open, **When** I press Tab, **Then** focus moves through section headers and links in order
3. **Given** a section header is focused, **When** I press Enter, **Then** the section expands/collapses
4. **Given** the menu is open, **When** I press Escape, **Then** the menu closes and focus returns to hamburger button
5. **Given** I'm navigating via keyboard, **When** focus moves to each link or button, **Then** I see a clear focus indicator

---

### User Story 6 — Desktop Navigation Preserved (Priority: P1)

Desktop users see the existing horizontal NavHeader and sticky SubNav without any changes.

**Why this priority**: Must not regress desktop experience.

**Independent Test**: Load site at desktop width (>1024px), verify hamburger icon hidden, horizontal navigation and SubNav visible and sticky.

**Acceptance Scenarios**:

1. **Given** I'm on a desktop viewport (>1024px), **When** I load the site, **Then** I see the horizontal NavHeader with all section links and the sticky SubNav below it
2. **Given** I'm on desktop, **When** I look for the hamburger icon, **Then** it is not visible
3. **Given** I'm on desktop, **When** I resize below 1024px, **Then** the compact header + hamburger appear and SubNav becomes non-sticky
4. **Given** I'm on desktop, **When** I view a section page sidebar, **Then** the sidebar renders as a 300px left column (no collapsible toggle)

---

### Edge Cases

**Menu**:

- What happens when menu is open and user resizes from mobile to desktop? → Menu closes automatically, horizontal nav appears
- What happens when page content is long and user scrolls while menu is open? → Body scroll is locked, only menu content scrolls
- What happens when hamburger button is too small to tap? → Minimum 44px touch target enforced
- What happens when user taps a link to the current page? → Menu closes, page doesn't reload
- What happens when screen reader user opens menu? → Menu announced as "navigation, expanded"
- What happens when user reaches last menu item via keyboard? → Focus exits menu or wraps

**Sidebar**:

- What happens when sidebar toggle is open and user scrolls past its content? → Sidebar stays inline (not sticky), user scrolls past it
- What happens when user has active filters, then collapses sidebar? → Filters remain applied, content stays filtered
- What happens when both hamburger menu and sidebar toggle are open? → Both can be open simultaneously (they serve different purposes)
- What happens on the About page (no sidebar)? → No sidebar toggle is shown
- What happens when the ToC sidebar has many entries? → Collapsible block shows all entries, scrolls if needed
- What happens on resize from mobile to desktop while sidebar is collapsed? → Sidebar returns to normal 300px column layout, collapse state is reset

**Breakpoint transition**:

- What happens when device orientation changes across the 1024px boundary? → Layout switches between mobile and desktop modes seamlessly

## Requirements

### Functional Requirements

**Hamburger Menu**:

- **FR-001**: System MUST hide horizontal nav links and show hamburger icon on viewports ≤1024px
- **FR-002**: System MUST show horizontal nav and SubNav (sticky) and hide hamburger icon on viewports >1024px
- **FR-003**: System MUST toggle menu visibility when hamburger button is clicked/tapped
- **FR-004**: System MUST display all sections in the menu with expandable sub-items (collections + custom pages per section)
- **FR-005**: Sections in menu: All, GitHub Copilot, Artificial Intelligence, Machine Learning, DevOps, Azure, .NET, Security — plus About (no sub-items)
- **FR-006**: Sub-items per section: All, News, Blogs, Videos, Community, plus any custom pages (ordered by Order then Title)
- **FR-007**: System MUST close menu when user taps overlay, taps hamburger again, presses Escape, or navigates away
- **FR-008**: System MUST prevent body scroll when menu is open
- **FR-009**: System MUST animate hamburger icon (three lines ↔ X)
- **FR-010**: System MUST slide menu in/out with smooth animation (from right side)
- **FR-011**: System MUST highlight current section and current sub-item in menu

**Sidebar Collapse**:

- **FR-012**: System MUST show sidebar content in a collapsible block above main content on viewports ≤1024px
- **FR-013**: Collapsible block MUST be collapsed by default on page load
- **FR-014**: System MUST show appropriate label: "Filters & Search" (section/collection pages), "Table of Contents" (content detail pages), or context-appropriate label (homepage)
- **FR-015**: Expanding/collapsing MUST NOT affect applied filters — filter state persists across toggle
- **FR-016**: System MUST show sidebar as normal 300px column on viewports >1024px (no toggle)

**Compact Sticky Header**:

- **FR-017**: On mobile (≤1024px), NavHeader MUST be compact (~50px): logo + hamburger only, sticky at `top: 0`
- **FR-018**: On mobile, SubNav MUST NOT be sticky (renders inline, scrolls with content)
- **FR-019**: On desktop (>1024px), NavHeader and SubNav MUST remain sticky as they are today

**Accessibility**:

- **FR-020**: System MUST support full keyboard navigation (Tab, Enter, Escape)
- **FR-021**: System MUST provide visible focus indicators for all interactive elements
- **FR-022**: System MUST include ARIA attributes (aria-label, aria-expanded, aria-controls)
- **FR-023**: System MUST ensure minimum 44px touch targets for hamburger button and sidebar toggle

### Non-Functional Requirements

- **NFR-001**: Touch targets MUST be minimum 44px (WCAG 2.1 AA)
- **NFR-002**: Animations MUST complete within 300ms
- **NFR-003**: Focus indicators MUST be visible (using existing design token patterns)
- **NFR-004**: Color contrast MUST meet WCAG 2.1 AA (4.5:1 for text)
- **NFR-005**: Screen readers MUST announce menu and sidebar state changes
- **NFR-006**: MUST work on iOS Safari, Android Chrome, and desktop browsers
- **NFR-007**: MUST respect `prefers-reduced-motion` (existing pattern in `base.css`)

### Key Entities

- **HamburgerButton**: Toggle button in compact NavHeader that opens/closes the mobile menu
- **MobileMenu**: Slide-out panel with hierarchical section → sub-item navigation
- **MenuOverlay**: Semi-transparent backdrop (`var(--color-black-backdrop)`) behind menu
- **MenuSection**: Expandable section in mobile menu with collections/custom pages as sub-items
- **SidebarToggle**: Collapsible toggle bar above content on mobile that shows/hides sidebar content
- **CompactNavHeader**: Reduced-height NavHeader on mobile (logo + hamburger, ~50px)

## Success Criteria

### Measurable Outcomes

- **SC-001**: Hamburger icon appears on all viewports ≤1024px; horizontal nav on >1024px
- **SC-002**: Menu contains all sections with correct expandable sub-items
- **SC-003**: Menu opens/closes via all methods (tap hamburger, tap overlay, Escape, navigate)
- **SC-004**: Sidebar content appears above main content on mobile, collapsed by default
- **SC-005**: Sidebar toggle expands/collapses without affecting filter state
- **SC-006**: Compact header is ~50px on mobile; full 76px header on desktop
- **SC-007**: SubNav is NOT sticky on mobile; sticky on desktop
- **SC-008**: All keyboard navigation works (Tab, Enter, Escape)
- **SC-009**: Lighthouse accessibility score ≥ 95
- **SC-010**: Touch targets meet minimum 44px
- **SC-011**: Zero console errors
- **SC-012**: Works on iOS Safari, Android Chrome, and desktop browsers

## Implementation Notes

### Reference Documentation

- [src/TechHub.Web/AGENTS.md](/src/TechHub.Web/AGENTS.md) — Blazor component patterns, design tokens, CSS architecture
- [docs/page-structure.md](/docs/page-structure.md) — Semantic HTML, sticky header architecture, sidebar patterns
- [docs/design-system.md](/docs/design-system.md) — Design tokens (colors, spacing, typography)
- [docs/filtering.md](/docs/filtering.md) — Filtering system (sidebar filter state management)

### Responsive Breakpoint

**Primary breakpoint**: `1024px`

- **Mobile**: `@media (max-width: 1024px)` → Hamburger menu, compact header, collapsible sidebar, non-sticky SubNav
- **Desktop**: `@media (min-width: 1025px)` → Horizontal navigation, full header, sidebar column, sticky SubNav

**Why 1024px** (changed from original 768px):

- The sidebar grid already collapses at 1024px — aligning the nav breakpoint creates consistency
- With 8 section links + About, horizontal nav gets crowded well before 768px
- iPad portrait (1024px) benefits from mobile nav with this many items
- The SubNav already overflows horizontally on tablets
- 768px remains for secondary layout adjustments (padding, font sizes, card layouts) that are already in place across 15+ CSS files

### Component Architecture

The implementation modifies existing components rather than creating a new monolithic navigation component:

**`NavHeader.razor` / `NavHeader.razor.css`** (modified):

- Add hamburger button markup (hidden on desktop via CSS)
- Add mobile menu panel markup with sections and sub-items
- Add overlay div
- Add state: `bool menuOpen`, `HashSet<string> expandedSections`
- Add methods: `ToggleMenu()`, `CloseMenu()`, `ToggleSection(string sectionName)`
- CSS: Compact header styles at ≤1024px, hamburger icon, slide-out menu panel, animations

**`SubNav.razor.css`** (modified):

- Add `@media (max-width: 1024px)` to make SubNav non-sticky (`position: static`)

**`sidebar.css`** (modified):

- At ≤1024px: Sidebar renders inside a collapsible `<details>`/`<summary>` element (or Blazor-controlled toggle)
- Sidebar content hidden by default, expanded on toggle click

**Page components** (`Home.razor`, `SectionCollection.razor`, `ContentItem.razor`):

- Wrap `<aside class="sidebar">` content in a collapsible container on mobile
- Or: Create a `SidebarCollapse.razor` wrapper component that handles the responsive toggle

**`design-tokens.css`** (modified):

- Update `--main-nav-height` for mobile (50px vs 76px desktop)
- Update `--sticky-header-height` calculation for mobile

### Mobile Menu Markup Structure

```razor
@* Inside NavHeader.razor — hamburger button (hidden on desktop) *@
<button class="hamburger-btn"
        @onclick="ToggleMenu"
        aria-label="@(menuOpen ? "Close navigation menu" : "Open navigation menu")"
        aria-expanded="@menuOpen"
        aria-controls="mobile-menu">
    <span class="hamburger-line"></span>
    <span class="hamburger-line"></span>
    <span class="hamburger-line"></span>
</button>

@* Mobile menu panel *@
<div id="mobile-menu" class="mobile-menu @(menuOpen ? "open" : "")" role="navigation" aria-label="Mobile navigation">
    @foreach (var sectionItem in SectionCache.Sections)
    {
        <div class="mobile-menu-section">
            <button class="mobile-menu-section-header @(IsActiveSection(sectionItem.Name) ? "active" : "")"
                    @onclick="() => ToggleSection(sectionItem.Name)"
                    aria-expanded="@expandedSections.Contains(sectionItem.Name)">
                @sectionItem.Title
                <span class="chevron @(expandedSections.Contains(sectionItem.Name) ? "expanded" : "")"></span>
            </button>

            @if (expandedSections.Contains(sectionItem.Name))
            {
                <div class="mobile-menu-sub-items">
                    <a href="/@sectionItem.Name/all" @onclick="CloseMenu">All</a>
                    @foreach (var collection in sectionItem.Collections.Where(c => !c.IsCustom))
                    {
                        <a href="/@sectionItem.Name/@collection.Name" @onclick="CloseMenu">@collection.Title</a>
                    }
                    @foreach (var customPage in sectionItem.Collections.Where(c => c.IsCustom).OrderBy(c => c.Order).ThenBy(c => c.Title))
                    {
                        <a href="@customPage.Url" @onclick="CloseMenu">@customPage.Title</a>
                    }
                </div>
            }
        </div>
    }
    <a href="/about" class="mobile-menu-link @(IsActivePage("/about") ? "active" : "")" @onclick="CloseMenu">About</a>
</div>

@if (menuOpen)
{
    <div class="mobile-menu-overlay" @onclick="CloseMenu"></div>
}
```

### Sidebar Collapse Pattern

The collapsible sidebar uses a wrapper component or inline pattern:

```razor
@* Inside page component, wrapping the existing sidebar *@
<aside class="sidebar">
    <div class="sidebar-collapse-toggle" @onclick="ToggleSidebar" role="button" tabindex="0"
         aria-expanded="@sidebarExpanded" aria-controls="sidebar-content">
        <span>@SidebarToggleLabel</span>
        <span class="chevron @(sidebarExpanded ? "expanded" : "")"></span>
    </div>

    <div id="sidebar-content" class="sidebar-content @(sidebarExpanded ? "expanded" : "collapsed")">
        @* Existing sidebar components render here *@
        <SidebarSearch ... />
        <SidebarTagCloud ... />
        <SidebarRssLinks ... />
    </div>
</aside>
```

On desktop (>1024px), the toggle is hidden via CSS and the sidebar content is always visible. On mobile (≤1024px), the toggle is visible and content is collapsed by default.

### CSS Animation

**Hamburger Icon Animation**:

- Default: Three horizontal lines (3 `<span>` elements)
- When open: Top line rotates 45°, middle line fades out, bottom line rotates -45° (forms X)
- Transition: `all 0.3s var(--easing-ease)`

**Menu Slide Animation**:

- Closed: `transform: translateX(100%)` (off-screen right)
- Open: `transform: translateX(0)` (slides in)
- Transition: `transform 0.3s var(--easing-ease)`

**Overlay**: Fade in/out with opacity, `background: var(--color-black-backdrop)`

**Sidebar Toggle**: `max-height` transition for expand/collapse, or CSS grid `grid-template-rows: 0fr` → `1fr` pattern

**Reduced Motion**: All animations respect `prefers-reduced-motion: reduce` (existing pattern in `base.css`)

### Body Scroll Lock

When hamburger menu is open, body scroll must be locked. Use JavaScript interop:

```javascript
// In mobile-nav.js (new file, loaded statically)
window.mobileNav = {
    lockScroll: () => document.body.style.overflow = 'hidden',
    unlockScroll: () => document.body.style.overflow = '',
    registerEscapeHandler: (dotNetHelper) => {
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape') {
                dotNetHelper.invokeMethodAsync('CloseMenuFromJs');
            }
        });
    }
};
```

### Mobile Layout Summary

```
┌──────────────────────────┐
│ Logo            ☰        │  ← Sticky compact nav (~50px)
├──────────────────────────┤
│ [Section Banner]         │  ← Scrolls with content
├──────────────────────────┤
│ [SubNav items...]        │  ← Scrolls with content (NOT sticky)
├──────────────────────────┤
│ ▼ Filters & Search       │  ← Collapsed sidebar toggle
├──────────────────────────┤
│                          │
│    Main Content          │  ← Gets ~90% of viewport
│                          │
└──────────────────────────┘
```

### Testing Strategy

**Component Tests (bUnit)**:

- NavHeader: Hamburger button renders, menu toggles, sections expand/collapse, ARIA attributes correct
- Sidebar collapse: Toggle renders, content shows/hides, filter state persists across toggle
- Active link highlighting in mobile menu

**E2E Tests (Playwright)**:

- Hamburger appears on ≤1024px viewport, hidden on >1024px
- Compact header height ~50px on mobile, 76px on desktop
- Menu opens/closes via: hamburger tap, overlay tap, Escape key, link navigation
- Section expansion reveals correct sub-items (collections + custom pages)
- SubNav is sticky on desktop, non-sticky on mobile
- Sidebar toggle visible on mobile, hidden on desktop
- Sidebar expands/collapses, filter state persists
- Keyboard navigation: Tab through menu items, Enter to expand sections, Escape to close
- Body scroll locked when menu open
- Resize from mobile↔desktop transitions correctly

**Accessibility Tests**:

- Lighthouse accessibility audit (score ≥ 95)
- Keyboard-only navigation
- Touch target size validation (≥ 44px)
- Color contrast validation (WCAG AA)
- Focus indicator visibility
- Screen reader: menu and sidebar state announced correctly

## Dependencies

- **Existing**: `SectionCache.Sections` provides section data with collections and custom pages (already used by NavHeader and SubNav)
- **Existing**: Active section/page detection (already in NavHeader and SubNav)
- **Existing**: Header.razor orchestration with `display: contents` pattern
- **Existing**: Design tokens in `design-tokens.css`
- **New**: JavaScript file for scroll lock and Escape key handling (`mobile-nav.js`)

## Out of Scope

- Mega menu (complex dropdown with columns/images) — simple expandable sections only
- Off-canvas push (pushing page content aside) — overlay only
- Swipe gestures to open/close menu — tap/click only
- Persistent menu state across sessions — menu always starts closed
- Persistent sidebar collapse state across pages — always starts collapsed on mobile
- Search within the hamburger menu — search stays in the sidebar
- Changing the 768px breakpoint used by other components for padding/font adjustments
