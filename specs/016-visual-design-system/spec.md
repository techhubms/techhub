# Visual Design System Specification

> **Feature**: Design tokens, color palette, typography, spacing standards, and CSS architecture for consistent visual design

## Overview

The visual design system defines the foundational design language for Tech Hub including color palette (GitHub brand colors), typography system, spacing standards, responsive breakpoints, and accessibility requirements. All design decisions are documented as design tokens that can be consumed by components and pages to ensure visual consistency across the entire application.

## Requirements

### Functional Requirements

**FR-1**: The system MUST define GitHub brand color palette with semantic naming  
**FR-2**: The system MUST define typography hierarchy (headings h1-h6, body text, code, labels)  
**FR-3**: The system MUST define spacing scale using 8px base unit (8px, 16px, 24px, 32px, 40px, 48px, 64px)  
**FR-4**: The system MUST define responsive breakpoints (mobile < 768px, tablet 768-1024px, desktop > 1024px)  
**FR-5**: The system MUST define focus indicator styles for keyboard navigation  
**FR-6**: The system MUST define touch target minimum sizes (44px for mobile interactions)  
**FR-7**: The system MUST provide CSS custom properties (CSS variables) for all design tokens  
**FR-8**: The system MUST support dark theme color scheme  
**FR-9**: The system MUST define component-specific design patterns (cards, buttons, inputs, navigation)  
**FR-10**: The system MUST define animation and transition standards

### Non-Functional Requirements

**NFR-1**: All color combinations MUST meet WCAG 2.1 AA contrast ratios (4.5:1 for normal text, 3:1 for large text/UI)  
**NFR-2**: Typography scale MUST use relative units (rem) for accessibility  
**NFR-3**: Spacing system MUST be applied consistently across all components  
**NFR-4**: CSS architecture MUST support maintainability and modularity  
**NFR-5**: Design tokens MUST be consumable by both CSS and JavaScript  
**NFR-6**: Responsive breakpoints MUST match current Jekyll site behavior

## Use Cases

### UC-1: Apply Brand Colors to Component

**Actor**: Developer  
**Precondition**: Design system is defined  
**Trigger**: Developer creates new component  

**Flow**:

1. Developer needs to style a button component
2. Developer references design system documentation
3. Developer finds semantic color token `--color-button-primary-bg`
4. Developer applies color using CSS custom property: `background: var(--color-button-primary-bg);`
5. Color updates automatically if design system changes

**Postcondition**: Component uses consistent brand colors

### UC-2**: Set Typography for Content

**Actor**: Developer  
**Precondition**: Typography scale is defined  
**Trigger**: Developer styles content section  

**Flow**:

1. Developer needs to style article headings
2. Developer references typography documentation
3. Developer applies heading styles: `font-size: var(--font-size-h2);`
4. Developer applies line height: `line-height: var(--line-height-heading);`
5. Text scales correctly on mobile and desktop

**Postcondition**: Typography is consistent and accessible

### UC-3: Ensure Accessible Color Contrast

**Actor**: Designer/Developer  
**Precondition**: Color palette is defined with contrast ratios  
**Trigger**: Implementing new UI element  

**Flow**:

1. Developer needs text color for light background
2. Developer checks design system contrast table
3. Developer finds `--color-text-primary` meets 4.5:1 ratio on `--color-bg-default`
4. Developer applies colors confidently knowing accessibility is met
5. Automated tests verify contrast ratios remain compliant

**Postcondition**: All text meets WCAG 2.1 AA standards

### UC-4: Implement Responsive Layout

**Actor**: Developer  
**Precondition**: Breakpoints are defined  
**Trigger**: Creating responsive component  

**Flow**:

1. Developer needs component to adapt to mobile, tablet, desktop
2. Developer uses defined breakpoints in media queries
3. Developer applies: `@media (min-width: var(--breakpoint-tablet)) { ... }`
4. Component responds consistently with other components
5. Site maintains visual harmony across viewports

**Postcondition**: Responsive behavior is consistent

## Acceptance Criteria

**AC-1**: Given design system documentation, when developer looks up color, then semantic color name and value are clearly defined  
**AC-2**: Given any color combination, when tested for contrast, then ratio meets or exceeds WCAG 2.1 AA requirements  
**AC-3**: Given typography scale, when applied to content, then text is readable on all devices  
**AC-4**: Given spacing scale, when applied to layout, then spacing follows 8px grid system  
**AC-5**: Given responsive breakpoints, when viewport resizes, then layout adapts at defined breakpoints  
**AC-6**: Given focus indicators, when keyboard user tabs through page, then focused elements have visible outline/border  
**AC-7**: Given touch targets on mobile, when user taps interactive element, then element is at least 44x44px  

## Design Tokens

### Color Palette

**Primary Colors** (from GitHub brand):

```css
:root {
  /* Background Colors */
  --color-bg-default: #0d1117;        /* Dark background */
  --color-bg-subtle: #161b22;         /* Slightly lighter background */
  --color-bg-emphasis: #21262d;       /* Emphasized background */
  
  /* Text Colors */
  --color-text-primary: #c9d1d9;      /* Primary text (4.5:1 on bg-default) */
  --color-text-secondary: #8b949e;    /* Secondary text */
  --color-text-tertiary: #6e7681;     /* Tertiary text */
  --color-text-link: #58a6ff;         /* Link color */
  
  /* Border Colors */
  --color-border-default: #30363d;    /* Default border */
  --color-border-emphasis: #6e7681;   /* Emphasized border */
  
  /* Accent Colors */
  --color-accent-primary: #58a6ff;    /* Primary accent (links, highlights) */
  --color-accent-success: #3fb950;    /* Success state */
  --color-accent-warning: #d29922;    /* Warning state */
  --color-accent-danger: #f85149;     /* Danger/error state */
}
```

**Semantic Color Mapping**:

```css
:root {
  /* Interactive Elements */
  --color-button-primary-bg: var(--color-accent-primary);
  --color-button-primary-text: #ffffff;
  --color-button-hover: #1f6feb;
  
  /* Tags */
  --color-tag-bg: var(--color-bg-emphasis);
  --color-tag-text: var(--color-text-secondary);
  --color-tag-border: var(--color-border-default);
}
```

### Typography

**Font Families**:

```css
:root {
  --font-family-base: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
  --font-family-mono: ui-monospace, SFMono-Regular, "SF Mono", Menlo, Consolas, "Liberation Mono", monospace;
}
```

**Font Sizes** (using 16px base):

```css
:root {
  --font-size-xs: 0.75rem;    /* 12px */
  --font-size-sm: 0.875rem;   /* 14px */
  --font-size-base: 1rem;     /* 16px */
  --font-size-lg: 1.125rem;   /* 18px */
  --font-size-xl: 1.25rem;    /* 20px */
  --font-size-2xl: 1.5rem;    /* 24px */
  --font-size-3xl: 2rem;      /* 32px */
  --font-size-4xl: 2.5rem;    /* 40px */
  
  /* Semantic Headings */
  --font-size-h1: var(--font-size-4xl);
  --font-size-h2: var(--font-size-3xl);
  --font-size-h3: var(--font-size-2xl);
  --font-size-h4: var(--font-size-xl);
  --font-size-h5: var(--font-size-lg);
  --font-size-h6: var(--font-size-base);
}
```

**Font Weights**:

```css
:root {
  --font-weight-normal: 400;
  --font-weight-medium: 500;
  --font-weight-semibold: 600;
  --font-weight-bold: 700;
}
```

**Line Heights**:

```css
:root {
  --line-height-tight: 1.25;
  --line-height-normal: 1.5;
  --line-height-relaxed: 1.75;
  
  /* Semantic */
  --line-height-heading: var(--line-height-tight);
  --line-height-body: var(--line-height-normal);
}
```

### Spacing

**Base 8px Grid**:

```css
:root {
  --spacing-0: 0;
  --spacing-1: 0.5rem;   /* 8px */
  --spacing-2: 1rem;     /* 16px */
  --spacing-3: 1.5rem;   /* 24px */
  --spacing-4: 2rem;     /* 32px */
  --spacing-5: 2.5rem;   /* 40px */
  --spacing-6: 3rem;     /* 48px */
  --spacing-8: 4rem;     /* 64px */
  --spacing-10: 5rem;    /* 80px */
  --spacing-12: 6rem;    /* 96px */
}
```

### Responsive Breakpoints

```css
:root {
  --breakpoint-mobile: 768px;
  --breakpoint-tablet: 1024px;
  --breakpoint-desktop: 1280px;
}
```

**Media Query Mixins** (for CSS preprocessors):

```scss
@mixin mobile {
  @media (max-width: #{$breakpoint-mobile - 1px}) {
    @content;
  }
}

@mixin tablet {
  @media (min-width: #{$breakpoint-mobile}) and (max-width: #{$breakpoint-tablet - 1px}) {
    @content;
  }
}

@mixin desktop {
  @media (min-width: #{$breakpoint-tablet}) {
    @content;
  }
}
```

### Interactive States

**Focus Indicators**:

```css
:root {
  --focus-outline-color: var(--color-accent-primary);
  --focus-outline-width: 2px;
  --focus-outline-offset: 2px;
}

:focus-visible {
  outline: var(--focus-outline-width) solid var(--focus-outline-color);
  outline-offset: var(--focus-outline-offset);
}
```

**Touch Targets**:

```css
:root {
  --touch-target-min-size: 44px;
}

button, a, input, select {
  min-height: var(--touch-target-min-size);
  @media (hover: none) { /* Touch devices */
    min-height: var(--touch-target-min-size);
  }
}
```

### Animations

```css
:root {
  /* Duration */
  --duration-fast: 150ms;
  --duration-normal: 300ms;
  --duration-slow: 500ms;
  
  /* Easing */
  --easing-ease: ease;
  --easing-ease-in: ease-in;
  --easing-ease-out: ease-out;
  --easing-ease-in-out: ease-in-out;
}

/* Reduced Motion Support */
@media (prefers-reduced-motion: reduce) {
  *, *::before, *::after {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}
```

## CSS Architecture

### File Structure

```
_sass/
├── base/
│   ├── _reset.scss           # CSS reset/normalize
│   ├── _typography.scss      # Typography base styles
│   └── _layout.scss          # Layout utilities
├── tokens/
│   ├── _colors.scss          # Color design tokens
│   ├── _spacing.scss         # Spacing design tokens
│   ├── _typography.scss      # Typography design tokens
│   └── _breakpoints.scss     # Responsive breakpoints
├── components/
│   ├── _buttons.scss         # Button styles
│   ├── _cards.scss           # Card styles
│   ├── _navigation.scss      # Navigation styles
│   └── _forms.scss           # Form element styles
└── main.scss                 # Main import file
```

### Naming Conventions

**BEM Methodology** for components:

```css
.card { }                    /* Block */
.card__header { }            /* Element */
.card--featured { }          /* Modifier */
```

**Utility Classes**:

```css
.u-text-center { text-align: center; }
.u-mb-4 { margin-bottom: var(--spacing-4); }
.u-hidden { display: none; }
```

## Accessibility Requirements

### Color Contrast

All color combinations MUST meet WCAG 2.1 Level AA:

| Use Case | Minimum Ratio | Example |
|----------|---------------|---------|
| Normal text (< 18pt) | 4.5:1 | `--color-text-primary` on `--color-bg-default` |
| Large text (≥ 18pt or bold ≥ 14pt) | 3:1 | Headings on background |
| UI components (borders, icons) | 3:1 | Buttons, form inputs |

### Keyboard Navigation

**Focus Indicators**:
- All interactive elements MUST have visible focus state
- Focus outline MUST be at least 2px solid with sufficient contrast
- Focus outline MUST not be removed without providing alternative visual indicator

**Tab Order**:
- Tab order MUST follow logical reading order (top to bottom, left to right)
- Skip links MUST be provided to bypass navigation
- Keyboard traps MUST be avoided

### Touch Targets

On mobile devices (viewport < 768px):
- All interactive elements MUST be at least 44x44px
- Sufficient spacing between touch targets to prevent accidental activation
- Interactive elements MUST respond to tap without delay

## Testing Strategy

### Unit Tests

- Test design token export to CSS custom properties
- Test breakpoint utility functions
- Test color contrast calculation utilities

### Visual Regression Tests

- Capture screenshots of component library at each breakpoint
- Compare against baseline after design changes
- Flag any unintended visual regressions

### Accessibility Tests

- Automated contrast ratio validation (axe-core, pa11y)
- Focus indicator visibility testing
- Touch target size validation on mobile viewports
- Screen reader compatibility testing

### Cross-Browser Tests

- Test in Chrome, Firefox, Safari, Edge (latest 2 versions)
- Test responsive behavior at breakpoints
- Test CSS custom property support (fallbacks if needed)

## Edge Cases

**EC-1**: Browser doesn't support CSS custom properties → Provide fallback values  
**EC-2**: User sets browser zoom > 200% → Design scales proportionally with rem units  
**EC-3**: User sets dark mode preference → Dark theme is default (no light mode for MVP)  
**EC-4**: User sets high contrast mode → Ensure sufficient contrast remains  
**EC-5**: Touch and mouse devices (hybrid) → Support both interaction modes  

## Migration Notes

**From Jekyll**:

- Jekyll uses SCSS files in `_sass/` directory
- Current color variables defined in `_sass/_colors.scss`
- Current spacing defined in `_sass/_dx-space.scss`
- **MIGRATION**: Extract color values from current SCSS into CSS custom properties
- **MIGRATION**: Convert SCSS variables to CSS custom properties for runtime theming support
- **MIGRATION**: Preserve existing breakpoint values for visual consistency
- **PRESERVE**: Current dark theme color scheme (no light mode in MVP)

## Open Questions

None - design system is well-defined based on current Jekyll implementation

## References

- `_sass/_colors.scss` - Current color definitions
- `_sass/_dx-space.scss` - Current spacing system
- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/) - Accessibility standards
- [010-nlweb-semantic-html Spec](../010-nlweb-semantic-html/spec.md) - Semantic HTML patterns
