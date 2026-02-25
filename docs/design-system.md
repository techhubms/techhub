# Design System

The Tech Hub design system provides consistent visual styling across all pages through design tokens, CSS architecture, and standardized patterns.

## Design Tokens - Single Source of Truth

All colors, spacing, typography, and other design values are defined in `wwwroot/css/design-tokens.css`. This is the single source of truth for the design system.

### Core Principle

Never hardcode design values. Always use CSS custom properties (design tokens).

**Correct usage**:

```css
.card {
    background: var(--color-bg-default);
    border: 1px solid var(--color-purple-border-subtle);
    padding: var(--spacing-3);
    border-radius: var(--radius-md);
    color: var(--color-text-primary);
}
```

**Incorrect usage** (never do this):

```css
.card {
    background: #161b22;
    border: 1px solid rgba(189, 147, 249, 0.2);
    padding: 24px;
}
```

### Adding New Design Tokens

If you need a new color, spacing, or other design value:

1. Add to `design-tokens.css` in the appropriate section
2. Use semantic naming - name describes purpose, not value (e.g., `--color-bg-elevated`, not `--color-gray-800`)
3. Document usage with a comment explaining when to use it
4. Check for duplicates - verify similar tokens don't already exist

### Design Token Categories

- **Colors** - Backgrounds, text, borders, overlays, accents
- **Typography** - Font families, sizes, weights, line heights
- **Spacing** - Padding, margins, gaps (8px grid system: 0.5rem, 1rem, 1.5rem, 2rem, etc.)
- **Layout** - Breakpoints, container widths, z-index scale
- **Effects** - Shadows, transitions, border radius
- **Interactive** - Focus indicators, touch targets

## Color Palette

All colors use the `--color-` prefix for consistency. See `design-tokens.css` for the complete palette.

### Purple Accents (Tech Hub Brand)

```css
--color-purple-bright: #bd93f9;  /* Focus states, highlights */
--color-purple-medium: #9d72d9;  /* Links, icons */
--color-purple-dark: #7f56d9;    /* Borders, accents */
```

### Background Colors

```css
--color-bg-default: #161b22;     /* Standard component background */
--color-bg-emphasis: #21262d;    /* Highlighted areas, code blocks */
--color-bg-elevated: #252538;    /* Elevated sections */
```

### Text Colors

```css
--color-text-primary: #c9d1d9;   /* Main body text */
--color-text-secondary: #8b949e; /* Secondary text */
--color-text-emphasis: #f0f6fc;  /* Maximum contrast for headings */
```

## Typography

### Font Stack

```css
font-family: ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont,
             "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
```

### Font Sizes

Use relative units (rem/em) for accessibility. The design system defines standard sizes in design-tokens.css.

## Spacing

Use consistent spacing units based on an 8px grid system:

- `--spacing-0`: 0
- `--spacing-1`: 0.5rem (8px)
- `--spacing-2`: 1rem (16px)
- `--spacing-3`: 1.5rem (24px)
- `--spacing-4`: 2rem (32px)
- `--spacing-5`: 2.5rem (40px)
- `--spacing-6`: 3rem (48px)

## Breakpoints

| Name | Width | Usage |
|------|-------|-------|
| Mobile | < 768px | Single column, stacked layouts |
| Tablet | 768px - 1024px | Two column grids |
| Desktop | > 1024px | Full three column grids |

## CSS Architecture

Tech Hub uses a **dual CSS strategy**: Global CSS for reusable design system components and Component-Scoped CSS for page-specific layouts.

### Global CSS (wwwroot/css/)

Reusable design system components and site-wide styles:

| File | Purpose |
|------|---------|
| `design-tokens.css` | ALL colors, typography, spacing (single source of truth) |
| `base.css` | Reset, typography, links, focus states, utilities, forms |
| `sidebar.css` | Shared sidebar component (used across multiple pages) |
| `nav-helpers.css` | Navigation helpers (back to top, back to previous) |
| `loading.css` | Skeleton loaders, loading states |
| `page-container.css` | Page layout containers |
| `tag-dropdown.css` | Tag dropdown component |
| `date-slider.css` | Date range slider component |
| `article.css` | Article content styling |

### Component-Scoped CSS (.razor.css files)

Page and component-specific styles automatically isolated by Blazor. Styles are scoped to components using unique `b-{hash}` attributes.

### When to Use Global vs Component-Scoped CSS

**Use Component-Scoped CSS** (`.razor.css`) when:

- Page-specific layouts unique to one page
- Component-specific styling unique to that component
- One-off styles that will NOT be reused elsewhere
- Style isolation to prevent accidental conflicts

**Use Global CSS** (`wwwroot/css/`) when:

- Reusable design system components (cards, buttons, forms)
- Site-wide styles (header, footer, navigation)
- Design tokens (colors, typography, spacing)
- Multiple components use the same styles (any style repeated in 2+ components)

### CSS Bundle Configuration

CSS files are defined once in `TechHub.Web.Configuration.CssFiles.All` and referenced by both App.razor and Program.cs.

- **Development**: Individual files loaded separately for easy debugging
- **Production**: All global CSS bundled into `bundle.css` (generated by WebOptimizer)
- **Cache busting**: The bundle URL includes an assembly MVID query parameter (`?v=...`) that changes on every build, ensuring browsers fetch the new bundle after each deployment. This is necessary because WebOptimizer sets aggressive cache headers (`max-age=10 years`) on the bundle response.

## Hover Effects

Hover effects MUST NOT change element size or position. This prevents layout shifts and maintains a stable user experience.

### Allowed in Hover Effects

- `color` / `background-color` / `border-color` changes
- `opacity` changes
- `box-shadow` changes
- `filter` effects (brightness, blur, etc.)
- `rotate()` transforms (e.g., dropdown arrows)

### Forbidden in Hover Effects

- `translateY()` / `translateX()` - Moves elements
- `scale()` - Changes element size
- ANY transform that changes visual position or size

**Why**: Size/position changes on hover are disorienting, don't work on touch devices, and feel amateurish. Subtle color/shadow changes feel polished and professional.

## Focus & Keyboard Navigation

Focus outlines are **keyboard-only** — they appear only when the user navigates with the Tab key, not when clicking or tapping elements. This prevents lingering focus rings on mobile/touch devices while maintaining WCAG 2.1 AA accessibility for keyboard users.

### How It Works

1. **JavaScript detection** (`nav-helpers.js`): A `keydown` listener on `document` adds the `keyboard-nav` CSS class to `<html>` when the Tab key is pressed. A `pointerdown` listener removes it when mouse/touch input is detected. The `pointerdown` handler also **blurs non-input elements** (buttons, links) to prevent lingering focus states on mobile.
2. **CSS scoping** (`base.css`): All focus outline styles are scoped under `html.keyboard-nav :focus-visible`. Without the `keyboard-nav` class, `:focus` and `:focus-visible` outlines are suppressed globally via `outline: none !important`.
3. **Sticky hover prevention**: Interactive elements (buttons, tag cloud items, toolbar buttons) use `@media (hover: none)` to reset hover styles on touch-only devices. This prevents the `:hover` state from "sticking" after a tap, which would make deactivated buttons still appear highlighted.
4. **Design tokens**: `--focus-outline-width: 2px` and `--focus-outline-offset: 2px` in `design-tokens.css`.

### Rules for Focus Styles

- **Always use `:focus-visible`** — never use `:focus` for outline styles (`:focus` fires on click/tap too)
- **Never add outline styles without the `keyboard-nav` scope** — the global `base.css` handles this; component CSS should not duplicate it
- **Never use `box-shadow` as a focus ring** — keyboard users get the global outline from `base.css`; `box-shadow` cannot be suppressed globally and will show on mobile taps
- **Use `outline: none` on `:focus`** only when replacing the outline with a different visual indicator (e.g., `border-color` change on inputs)
- **Add `@media (hover: none)` resets for interactive buttons** — any button with `:hover` styling that changes background/border must have a `@media (hover: none)` rule resetting to the default state for non-selected/non-active items
- **Test on real mobile devices** — emulators may not reproduce touch focus behavior accurately

### Skip Link

The skip-to-content link (`.skip-link`) is visually hidden off-screen and slides into view when focused via keyboard. Its focus outline is also scoped under `html.keyboard-nav`.

## Image Handling

Images are resolved by naming convention, NOT stored in API models or configuration.

### Section Background Images

Section background images use a two-tier system:

1. **Thumbnails** (400x140px) for SectionCards - Fast page loads
2. **Full-size** images for SectionBanner - High quality hero images

### File Structure

```text
wwwroot/images/
├── section-backgrounds/         # Full-size images for banners
│   ├── ai.jxl                  # JPEG XL (best compression, modern browsers)
│   ├── ai.webp                 # WebP (good compression, wide support)
│   ├── ai.jpg                  # JPEG (fallback, universal support)
│   └── ...
└── section-thumbnails/          # Thumbnails for cards
    ├── ai.jxl
    ├── ai.webp
    ├── ai.jpg
    └── ...
```

### Adding a New Section

1. Create images: `{section-name}.webp` in both directories
2. Add CSS classes to both `.razor.css` files
3. NO API changes needed
4. NO configuration changes needed

## Implementation Reference

- **Design tokens file**: [src/TechHub.Web/wwwroot/css/design-tokens.css](../src/TechHub.Web/wwwroot/css/design-tokens.css)
- **CSS bundle config**: [src/TechHub.Web/Configuration/CssFiles.cs](../src/TechHub.Web/Configuration/CssFiles.cs)
- **Blazor styling patterns**: [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md)
