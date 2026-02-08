# Feature Specification: Mobile Navigation

**Feature Branch**: `005-mobile-navigation`  
**Created**: 2026-01-16  
**Status**: Draft  
**Input**: Implement responsive navigation system that adapts to mobile devices using a hamburger menu pattern

## User Scenarios & Testing

### User Story 1 - Access Navigation on Mobile (Priority: P1)

Mobile users can access all navigation links through a hamburger menu that slides in from the side, without cluttering the small screen.

**Why this priority**: Essential for mobile user experience, majority of traffic is mobile.

**Independent Test**: Open site on mobile viewport (< 768px), verify hamburger icon visible, tap it, verify menu slides in with all navigation links.

**Acceptance Scenarios**:

1. **Given** I'm on a mobile device (< 768px), **When** I load the site, **Then** I see a hamburger menu icon (three horizontal lines) in the header
2. **Given** the hamburger menu is visible, **When** I tap it, **Then** the menu slides in from the right with smooth animation
3. **Given** the menu is open, **When** I view the menu, **Then** I see all section links (Home, AI, GitHub Copilot, Azure, ML, .NET, DevOps, Security, About)
4. **Given** the menu is open, **When** I tap a navigation link, **Then** I navigate to that page and the menu closes automatically

---

### User Story 2 - Close Menu Easily (Priority: P1)

Mobile users can close the navigation menu using multiple methods (tap outside, close button, navigate away) so they can quickly return to content.

**Why this priority**: Critical UX - users must be able to dismiss the menu easily.

**Independent Test**: Open hamburger menu, try closing via: (1) tap outside menu, (2) tap hamburger icon again, (3) press Escape key, verify all methods close the menu.

**Acceptance Scenarios**:

1. **Given** the menu is open, **When** I tap anywhere outside the menu area, **Then** the menu closes with smooth animation
2. **Given** the menu is open, **When** I tap the hamburger icon again, **Then** the menu closes and icon animates back to three lines
3. **Given** the menu is open, **When** I press the Escape key, **Then** the menu closes immediately
4. **Given** the menu is open, **When** I navigate to a page, **Then** the menu automatically closes

---

### User Story 3 - Navigate with Keyboard (Priority: P2)

Keyboard users can open the menu, navigate through links, and close the menu using only keyboard (no mouse required), ensuring accessibility for users who cannot use touch or mouse.

**Why this priority**: Accessibility requirement (WCAG 2.1 AA), ensures keyboard-only users can navigate.

**Independent Test**: Use only keyboard (Tab, Enter, Escape), verify can open menu, navigate through all links, and close menu.

**Acceptance Scenarios**:

1. **Given** I'm using only keyboard, **When** I press Tab to focus the hamburger button and press Enter, **Then** the menu opens
2. **Given** the menu is open, **When** I press Tab, **Then** focus moves through all navigation links in order
3. **Given** the menu is open, **When** I press Escape, **Then** the menu closes and focus returns to hamburger button
4. **Given** I'm navigating via keyboard, **When** focus moves to each link, **Then** I see a clear focus indicator (outline)

---

### User Story 4 - Desktop Navigation (Priority: P1)

Desktop users see the traditional horizontal navigation menu without any hamburger icon, as they have sufficient screen space for all links.

**Why this priority**: Essential to maintain desktop experience, hamburger menus should only appear on small screens.

**Independent Test**: Resize browser to desktop width (> 768px), verify hamburger icon hidden, horizontal navigation visible.

**Acceptance Scenarios**:

1. **Given** I'm on a desktop viewport (> 768px), **When** I load the site, **Then** I see a horizontal navigation menu with all section links visible
2. **Given** I'm on desktop viewport, **When** I look for the hamburger icon, **Then** it is not visible (display: none)
3. **Given** I'm on desktop, **When** I resize browser window below 768px, **Then** the horizontal menu switches to hamburger icon
4. **Given** I'm on mobile, **When** I rotate device to landscape (> 768px), **Then** the horizontal menu appears and hamburger hides

---

### Edge Cases

- What happens when menu is open and user resizes browser from mobile to desktop? → Menu closes automatically, horizontal nav appears
- What happens when page content is very long and user scrolls while menu is open? → Body scroll is locked (prevented), only menu scrolls
- What happens when hamburger button is too small to tap on touch device? → Minimum 44px touch target enforced (accessibility guideline)
- What happens when user taps a navigation link to the current page? → Menu closes, page doesn't reload (already on that page)
- What happens when screen reader user opens menu? → Menu announced as "navigation, expanded" with count of links
- What happens when user navigates via keyboard and reaches last menu item? → Tab wraps to first focusable element or exits menu

## Requirements

### Functional Requirements

- **FR-001**: System MUST hide horizontal navigation and show hamburger icon on viewports < 768px
- **FR-002**: System MUST show horizontal navigation and hide hamburger icon on viewports >= 768px
- **FR-003**: System MUST toggle menu visibility when hamburger button is clicked/tapped
- **FR-004**: System MUST display all section links in mobile menu (Home, AI, GitHub Copilot, Azure, ML, .NET, DevOps, Security, About)
- **FR-005**: System MUST close menu when user clicks/taps outside menu area
- **FR-006**: System MUST close menu when user presses Escape key
- **FR-007**: System MUST close menu when user navigates to a page
- **FR-008**: System MUST prevent body scroll when menu is open (scroll lock)
- **FR-009**: System MUST animate hamburger icon (three lines → X) when menu opens
- **FR-010**: System MUST slide menu in/out with smooth animation (from right side)
- **FR-011**: System MUST support keyboard navigation (Tab, Enter, Escape)
- **FR-012**: System MUST provide visible focus indicators for all interactive elements
- **FR-013**: System MUST include ARIA labels for accessibility (aria-label, aria-expanded)
- **FR-014**: System MUST ensure minimum 44px touch target for hamburger button
- **FR-015**: System MUST highlight current page/section in menu

### Non-Functional Requirements

- **NFR-001**: Hamburger button MUST have minimum 44px touch target (WCAG 2.1 AA)
- **NFR-002**: Menu animation MUST complete within 300ms
- **NFR-003**: All interactive elements MUST have visible focus indicators (2px outline, 4px offset)
- **NFR-004**: Color contrast MUST meet WCAG 2.1 AA standards (4.5:1 for text)
- **NFR-005**: Screen readers MUST announce menu state changes (opened/closed)
- **NFR-006**: Component MUST work on iOS Safari, Android Chrome, and desktop browsers
- **NFR-007**: Component MUST be tested with NVDA, JAWS, and VoiceOver screen readers

### Key Entities

- **HamburgerButton**: Toggle button that opens/closes menu
- **MobileMenu**: Slide-out navigation menu with all links
- **MenuOverlay**: Semi-transparent backdrop behind menu
- **MenuState**: Boolean tracking whether menu is open or closed
- **NavigationLinks**: List of all site sections and pages

## Success Criteria

### Measurable Outcomes

- **SC-001**: Hamburger menu icon appears on all mobile viewports (< 768px)
- **SC-002**: Horizontal navigation appears on all desktop viewports (>= 768px)
- **SC-003**: Menu opens and closes via all methods (tap hamburger, tap outside, Escape key, navigate away)
- **SC-004**: All navigation links are keyboard accessible (Tab navigation)
- **SC-005**: Menu animation completes within 300ms
- **SC-006**: Lighthouse accessibility score >= 95
- **SC-007**: Touch targets meet WCAG 2.1 AA minimum 44px
- **SC-008**: Zero console errors or warnings
- **SC-009**: Works correctly on iOS, Android, and desktop browsers

## Implementation Notes

### Reference Documentation

- [src/TechHub.Web/AGENTS.md](/src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [docs/filtering.md](/docs/filtering.md) - Content filtering system (for state management patterns if menu state needs to persist)

### Current Status

**Navigation Component**:

- Exists in header layout component
- Currently shows horizontal navigation only
- Needs responsive behavior and hamburger menu

**CSS Design System**:

- CSS custom properties already defined (--primary-blue, --dark-navy, --text-color)
- Responsive design patterns exist for other components
- Can follow existing breakpoint strategy (768px)

### Responsive Breakpoint

**Breakpoint**: `768px`

- **Mobile**: `@media (max-width: 768px)` → Hamburger menu
- **Desktop**: `@media (min-width: 769px)` → Horizontal navigation

**Why 768px**:

- Standard tablet/mobile boundary
- Matches common device widths
- Consistent with other responsive components

### Component Structure

**Blazor Component Pattern** (code-behind):

- Component: `Navigation.razor`
- Code-behind: `Navigation.razor.cs`
- CSS: `Navigation.razor.css` or shared in `site.css`
- State: Private `bool menuOpen` field
- Methods: `ToggleMenu()`, `CloseMenu()`
- Event handling: `@onclick`, `@onclick:stopPropagation`

**Markup Structure**:

```razor
<nav class="site-nav" aria-label="Main navigation">
    <button class="hamburger" 
            @onclick="ToggleMenu" 
            aria-label="@(menuOpen ? "Close menu" : "Open menu")"
            aria-expanded="@menuOpen">
        <span></span>
        <span></span>
        <span></span>
    </button>
    
    <div class="nav-menu @(menuOpen ? "open" : "")">
        @foreach (var section in sections)
        {
            <a href="/@section.UrlPath" @onclick="CloseMenu">@section.DisplayName</a>
        }
        <a href="/about" @onclick="CloseMenu">About</a>
    </div>
    
    @if (menuOpen)
    {
        <div class="menu-overlay" @onclick="CloseMenu"></div>
    }
</nav>
```

### CSS Animation

**Hamburger Icon Animation**:

- Default: Three horizontal lines (3 `<span>` elements)
- When open: Top line rotates 45°, middle line fades out, bottom line rotates -45° (forms X)
- Transition: `all 0.3s ease`

**Menu Slide Animation**:

- Closed: `transform: translateX(100%)` (off-screen right)
- Open: `transform: translateX(0)` (slide in from right)
- Transition: `transform 0.3s ease`

**Overlay Fade**:

- Closed: `display: none`
- Open: `display: block`, `background: var(--color-black-backdrop)`
- Fade in/out with opacity transition

### Body Scroll Lock

**Problem**: When menu is open, scrolling on menu should scroll menu content, not page body behind it.

**Solution** (JavaScript Interop):

```csharp
// In Navigation.razor.cs
[Inject] private IJSRuntime JSRuntime { get; set; } = default!;

private async Task ToggleMenu()
{
    menuOpen = !menuOpen;
    await JSRuntime.InvokeVoidAsync("toggleBodyScroll", menuOpen);
}
```

**JavaScript**:

```javascript
// In site.js or navigation.js
window.toggleBodyScroll = (menuOpen) => {
    if (menuOpen) {
        document.body.style.overflow = 'hidden';
    } else {
        document.body.style.overflow = '';
    }
};
```

### Keyboard Handling

**Tab Navigation**:

- Native HTML behavior handles Tab through focusable elements
- Focus order: hamburger button → menu links → page content
- Focus trap NOT needed (menu can be closed easily via Escape)

**Escape Key**:

- JavaScript event listener for Escape key
- Calls Blazor method to close menu
- Focus returns to hamburger button

**JavaScript**:

```javascript
// In site.js
let dotNetHelper;

window.registerEscapeHandler = (helper) => {
    dotNetHelper = helper;
    document.addEventListener('keydown', handleEscape);
};

function handleEscape(e) {
    if (e.key === 'Escape' && dotNetHelper) {
        dotNetHelper.invokeMethodAsync('CloseMenu');
    }
}
```

**Blazor**:

```csharp
// In Navigation.razor.cs
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender)
    {
        var dotNetRef = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("registerEscapeHandler", dotNetRef);
    }
}

[JSInvokable]
public void CloseMenu()
{
    menuOpen = false;
    StateHasChanged();
}
```

### Testing Strategy

**Component Tests** (bUnit):

- Menu toggle state changes on button click
- Menu closes when link is clicked
- ARIA attributes update correctly (aria-expanded, aria-label)
- Menu CSS classes toggle (open/closed)

**E2E Tests** (Playwright):

- Hamburger appears on mobile viewport (< 768px)
- Horizontal nav appears on desktop viewport (> 768px)
- Menu opens when hamburger clicked
- Menu closes when: (1) outside clicked, (2) hamburger clicked again, (3) Escape pressed, (4) link clicked
- Keyboard navigation works (Tab, Enter, Escape)
- Body scroll locked when menu open
- Menu slides in/out with animation
- Current page highlighted in menu

**Accessibility Tests**:

- Lighthouse accessibility audit (score >= 95)
- Keyboard-only navigation (no mouse)
- Screen reader testing (NVDA, JAWS, VoiceOver)
- Touch target size validation (>= 44px)
- Color contrast validation (WCAG AA)
- Focus indicator visibility

**Visual Regression Tests** (optional):

- Screenshot comparison of hamburger icon (open vs closed)
- Screenshot comparison of menu (mobile vs desktop)

## Dependencies

- **Needed**: Section configuration from appsettings.json (to populate navigation links)
- **Needed**: Current page/section detection (to highlight active link)
- **In Progress**: Header layout component (navigation lives in header)

## Out of Scope

- Multi-level nested navigation (sub-menus) - all links are top-level
- Mega menu (complex dropdown with columns/images) - simple slide-out only
- Off-canvas push (pushing page content aside) - overlay only
- Swipe gestures to open/close menu - tap/click only
- Persistent menu state across sessions - menu always starts closed
- Customizable breakpoint - fixed at 768px
