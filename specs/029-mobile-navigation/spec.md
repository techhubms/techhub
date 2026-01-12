# Mobile Navigation (Hamburger Menu)

## Overview

Responsive navigation system that adapts to mobile devices using a hamburger menu pattern. This provides optimal navigation experience across all screen sizes while maintaining accessibility standards.

## Functional Requirements

### FR-001: Responsive Navigation Behavior

The navigation must adapt based on viewport width:

- **Desktop** (`>768px`): Standard horizontal navigation menu with all sections visible
- **Mobile** (`<768px`): Hamburger icon that toggles a slide-out or overlay menu

### FR-002: Hamburger Menu Toggle

The hamburger menu button must:

- Display three horizontal lines (standard hamburger icon)
- Toggle menu visibility on click/tap
- Animate smoothly between open/closed states
- Include proper ARIA labels for accessibility
- Be easily tappable (minimum 44px touch target)

### FR-003: Menu Content

The mobile menu must include:

- All section links (Home, AI, GitHub Copilot, Azure, ML, .NET, DevOps, Security)
- About page link
- RSS feed link
- Clear visual hierarchy
- Current page/section highlighting

### FR-004: Menu Interaction

The menu must:

- Close when a navigation link is clicked
- Close when clicking/tapping outside the menu area
- Close when pressing the Escape key
- Prevent body scroll when menu is open
- Support keyboard navigation (Tab, Enter, Escape)

### FR-005: Accessibility

The navigation must meet WCAG 2.1 AA standards:

- Keyboard accessible (all interactive elements)
- Screen reader accessible (ARIA labels, semantic HTML)
- Focus indicators visible
- Proper heading hierarchy
- Touch targets minimum 44px

## Technical Requirements

### Implementation Pattern

**Component Structure**:

```razor
<nav class="site-nav" aria-label="Main navigation">
    <button 
        class="hamburger" 
        @onclick="ToggleMenu" 
        aria-label="@(menuOpen ? "Close menu" : "Open menu")"
        aria-expanded="@menuOpen">
        <span></span>
        <span></span>
        <span></span>
    </button>
    
    <div class="nav-menu @(menuOpen ? "open" : "")" @onclick:stopPropagation>
        <a href="/" @onclick="CloseMenu">Home</a>
        <a href="/ai" @onclick="CloseMenu">AI</a>
        <a href="/github-copilot" @onclick="CloseMenu">GitHub Copilot</a>
        <a href="/azure" @onclick="CloseMenu">Azure</a>
        <a href="/ml" @onclick="CloseMenu">Machine Learning</a>
        <a href="/dotnet" @onclick="CloseMenu">.NET</a>
        <a href="/devops" @onclick="CloseMenu">DevOps</a>
        <a href="/security" @onclick="CloseMenu">Security</a>
        <a href="/about" @onclick="CloseMenu">About</a>
    </div>
    
    @if (menuOpen)
    {
        <div class="menu-overlay" @onclick="CloseMenu"></div>
    }
</nav>

@code {
    private bool menuOpen = false;
    
    private void ToggleMenu()
    {
        menuOpen = !menuOpen;
    }
    
    private void CloseMenu()
    {
        menuOpen = false;
    }
    
    protected override void OnInitialized()
    {
        // Close menu on Escape key (implement via JS interop if needed)
    }
}
```

### CSS Requirements

**Desktop Styles**:

```css
/* Desktop - normal nav */
.hamburger {
    display: none;
}

.nav-menu {
    display: flex;
    gap: 1rem;
}

.menu-overlay {
    display: none;
}
```

**Mobile Styles**:

```css
/* Mobile - hamburger menu */
@media (max-width: 768px) {
    .hamburger {
        display: block;
        background: none;
        border: none;
        cursor: pointer;
        padding: 0.5rem;
        z-index: 1001;
        position: relative;
    }
    
    .hamburger span {
        display: block;
        width: 25px;
        height: 3px;
        background: var(--text-color);
        margin: 5px 0;
        transition: all 0.3s ease;
    }
    
    /* Hamburger animation when open */
    .hamburger[aria-expanded="true"] span:nth-child(1) {
        transform: rotate(45deg) translate(6px, 6px);
    }
    
    .hamburger[aria-expanded="true"] span:nth-child(2) {
        opacity: 0;
    }
    
    .hamburger[aria-expanded="true"] span:nth-child(3) {
        transform: rotate(-45deg) translate(6px, -6px);
    }
    
    .nav-menu {
        display: none;
        flex-direction: column;
        position: fixed;
        top: 60px;
        right: 0;
        width: 280px;
        height: calc(100vh - 60px);
        background: var(--dark-navy);
        padding: 2rem;
        box-shadow: -2px 0 8px rgba(0, 0, 0, 0.3);
        z-index: 1000;
        overflow-y: auto;
        transform: translateX(100%);
        transition: transform 0.3s ease;
    }
    
    .nav-menu.open {
        display: flex;
        transform: translateX(0);
    }
    
    .nav-menu a {
        padding: 1rem 0;
        border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        color: var(--text-color);
        text-decoration: none;
        font-size: 1.1rem;
    }
    
    .nav-menu a:hover,
    .nav-menu a:focus {
        color: var(--primary-blue);
        outline: 2px solid var(--primary-blue);
        outline-offset: 4px;
    }
    
    .menu-overlay {
        display: none;
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        z-index: 999;
    }
    
    .nav-menu.open ~ .menu-overlay {
        display: block;
    }
}
```

### JavaScript Enhancements (Optional)

**Body Scroll Lock**:

```javascript
// Prevent body scroll when menu is open
function toggleBodyScroll(menuOpen) {
    if (menuOpen) {
        document.body.style.overflow = 'hidden';
    } else {
        document.body.style.overflow = '';
    }
}
```

**Keyboard Handling**:

```javascript
// Close menu on Escape key
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && menuOpen) {
        DotNet.invokeMethodAsync('TechHub.Web', 'CloseMenu');
    }
});
```

## Acceptance Criteria

1. ✅ Hamburger menu appears only on mobile (`<768px`)
2. ✅ Desktop shows full horizontal navigation (`>768px`)
3. ✅ Clicking hamburger toggles menu visibility
4. ✅ Menu slides in from right with smooth animation
5. ✅ Clicking a link closes the menu
6. ✅ Clicking overlay closes the menu
7. ✅ Pressing Escape closes the menu
8. ✅ Body scroll disabled when menu is open
9. ✅ Hamburger icon animates to X when menu is open
10. ✅ All links keyboard accessible (Tab navigation)
11. ✅ ARIA labels present (`aria-label`, `aria-expanded`)
12. ✅ Focus indicators visible on all interactive elements
13. ✅ Touch targets minimum 44px (hamburger button, menu links)
14. ✅ Current page/section highlighted in menu
15. ✅ Menu overlay has semi-transparent background
16. ✅ Menu works on all mobile devices (iOS, Android)
17. ✅ No horizontal scroll on mobile

## Testing Strategy

**Component Tests** (bUnit):

- Test menu toggle state changes
- Test menu close on link click
- Test ARIA attributes update correctly

**E2E Tests** (Playwright):

- Test hamburger menu appears on mobile viewport
- Test menu opens/closes on click
- Test menu closes when clicking outside
- Test keyboard navigation (Tab, Enter, Escape)
- Test menu links navigate correctly
- Test body scroll lock when menu open

**Accessibility Tests**:

- Lighthouse accessibility audit (score > 95)
- Keyboard navigation verification
- Screen reader compatibility (NVDA, JAWS, VoiceOver)
- Color contrast validation (WCAG AA)
- Touch target size validation

## Dependencies

- Existing navigation structure and section links
- Blazor SSR for server-side rendering
- CSS design tokens from design system

## Priority

**P1** - High priority for mobile user experience

## Related Specifications

- [015-nlweb-semantic-html](../015-nlweb-semantic-html/) - Semantic HTML and accessibility
- [016-visual-design-system](../016-visual-design-system/) - Design tokens and CSS architecture
