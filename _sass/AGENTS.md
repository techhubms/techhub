# SCSS Styling Agent

> **AI CONTEXT**: This is a **LEAF** context file for the `_sass/` directory. It complements the [Root AGENTS.md](../AGENTS.md).
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Overview

You are a styling specialist focused on the Tech Hub's SCSS (Sass) architecture. The styling system uses modular SCSS files compiled by Jekyll, with the main entry point in `assets/css/main.scss`.

## When to Use This Guide

**Read this file when**:

- Writing or modifying SCSS files in `_sass/` directory
- Creating new UI components or layouts
- Updating color schemes or design tokens
- Implementing responsive design patterns
- Debugging style-related issues

**Related Documentation**:

- Jekyll compilation → [.github/agents/fullstack.md](../.github/agents/fullstack.md)
- JavaScript interactions → [assets/js/AGENTS.md](../assets/js/AGENTS.md)
- Testing → [spec/AGENTS.md](../spec/AGENTS.md)

## Tech Stack

- **CSS Preprocessor**: SCSS (Sass)
- **Compiler**: Jekyll's built-in Sass compiler
- **Architecture**: Modular partials with a single entry point
- **Methodology**: Component-based with BEM-influenced naming

## Directory Structure

```text
_sass/                          # SCSS partial files (compiled by Jekyll)
├── _base.scss                  # Base styles and resets
├── _colors.scss                # Color variables and theme
├── _features.scss              # Feature sections styling
├── _filters.scss               # Filter UI components
├── _layout.scss                # Overall page layout
├── _navigation-collections.scss # Collection navigation
├── _navigation-posts.scss      # Post navigation
├── _navigation-sections.scss   # Section navigation
├── _navigation.scss            # Main navigation bar
├── _settings.scss              # Global variables and settings
├── _subheader.scss            # Subheader component
└── _syntax-highlighting.scss   # Code syntax highlighting

assets/css/
└── main.scss                   # Main entry point (imports all partials)
```

## Critical Rule: Two-Directory Structure

**SCSS files live in TWO locations**:

1. **`_sass/` directory**: Contains all SCSS partials (`_*.scss`)
2. **`assets/css/main.scss`**: Single entry point that imports partials

**Jekyll compiles** `assets/css/main.scss` → `assets/css/main.css`

### main.scss Structure

```scss
---
# Front matter required for Jekyll processing
---

// Import all partials in dependency order
@import 'settings';     // Variables first
@import 'colors';       // Color scheme
@import 'base';         // Base styles
@import 'layout';       // Layout structure
@import 'navigation';   // Navigation components
// ... other imports
```

## SCSS File Organization

### _settings.scss

Global variables and configuration:

```scss
// Breakpoints
$breakpoint-mobile: 768px;
$breakpoint-tablet: 1024px;
$breakpoint-desktop: 1440px;

// Spacing scale
$spacing-xs: 0.25rem;
$spacing-sm: 0.5rem;
$spacing-md: 1rem;
$spacing-lg: 1.5rem;
$spacing-xl: 2rem;

// Typography
$font-family-sans: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
$font-family-mono: 'SFMono-Regular', Consolas, 'Liberation Mono', monospace;
```

### _colors.scss

Color system and theme:

```scss
// Primary palette
$color-primary: #0078d4;      // Microsoft blue
$color-secondary: #107c10;    // Success green
$color-accent: #5c2d91;       // Purple accent

// Semantic colors
$color-background: #ffffff;
$color-text: #323130;
$color-text-secondary: #605e5c;
$color-border: #edebe9;

// State colors
$color-success: #107c10;
$color-warning: #ffb900;
$color-error: #d13438;
$color-info: #0078d4;
```

### _base.scss

Base styles and resets:

```scss
// CSS Reset
*,
*::before,
*::after {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

// Base typography
body {
  font-family: $font-family-sans;
  font-size: 16px;
  line-height: 1.6;
  color: $color-text;
  background-color: $color-background;
}

// Responsive images
img {
  max-width: 100%;
  height: auto;
}
```

### _layout.scss

Page layout and grid:

```scss
.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 $spacing-md;
  
  @media (min-width: $breakpoint-tablet) {
    padding: 0 $spacing-lg;
  }
}

.main-content {
  display: grid;
  grid-template-columns: 1fr;
  gap: $spacing-lg;
  
  @media (min-width: $breakpoint-desktop) {
    grid-template-columns: 250px 1fr;
  }
}
```

### _filters.scss

Filter UI components:

```scss
.filter-section {
  margin-bottom: $spacing-lg;
}

.filter-btn {
  padding: $spacing-sm $spacing-md;
  border: 1px solid $color-border;
  background: $color-background;
  cursor: pointer;
  transition: all 0.2s ease;
  
  &:hover {
    background: $color-primary;
    color: white;
    border-color: $color-primary;
  }
  
  &.active {
    background: $color-primary;
    color: white;
    border-color: $color-primary;
  }
  
  .count-badge {
    margin-left: $spacing-xs;
    font-size: 0.875em;
    opacity: 0.8;
  }
}
```

### _navigation.scss

Main navigation components:

```scss
.main-nav {
  background: $color-primary;
  color: white;
  padding: $spacing-md 0;
  
  .nav-list {
    display: flex;
    list-style: none;
    gap: $spacing-md;
  }
  
  .nav-item a {
    color: white;
    text-decoration: none;
    padding: $spacing-sm $spacing-md;
    
    &:hover,
    &.active {
      background: rgba(255, 255, 255, 0.2);
      border-radius: 4px;
    }
  }
}
```

## SCSS Best Practices

### Nesting

**Limit nesting to 3 levels max**:

```scss
// ✅ CORRECT: Shallow nesting
.card {
  padding: $spacing-md;
  
  &__header {
    font-size: 1.25rem;
  }
  
  &__body {
    margin-top: $spacing-sm;
  }
}

// ❌ WRONG: Deep nesting
.card {
  .header {
    .title {
      .text {
        font-size: 1.25rem; // Too deep!
      }
    }
  }
}
```

### Variables

```scss
// ✅ CORRECT: Semantic names
$color-primary: #0078d4;
$spacing-md: 1rem;

// ❌ WRONG: Non-semantic names
$blue: #0078d4;
$medium: 1rem;
```

### Mixins

```scss
// Reusable mixins
@mixin responsive($breakpoint) {
  @media (min-width: $breakpoint) {
    @content;
  }
}

// Usage
.component {
  padding: $spacing-sm;
  
  @include responsive($breakpoint-tablet) {
    padding: $spacing-lg;
  }
}
```

### BEM-Influenced Naming

```scss
// Block__Element--Modifier pattern
.filter-section {          // Block
  &__header {             // Element
    font-size: 1.25rem;
  }
  
  &__button {             // Element
    padding: $spacing-sm;
    
    &--active {           // Modifier
      background: $color-primary;
    }
  }
}
```

## Component Patterns

### Card Component

```scss
.card {
  background: white;
  border: 1px solid $color-border;
  border-radius: 8px;
  padding: $spacing-md;
  transition: box-shadow 0.2s ease;
  
  &:hover {
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  }
  
  &__header {
    border-bottom: 1px solid $color-border;
    padding-bottom: $spacing-sm;
    margin-bottom: $spacing-sm;
  }
  
  &__title {
    font-size: 1.25rem;
    font-weight: 600;
  }
  
  &__body {
    color: $color-text-secondary;
  }
}
```

### Button Component

```scss
.btn {
  display: inline-block;
  padding: $spacing-sm $spacing-md;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s ease;
  
  &--primary {
    background: $color-primary;
    color: white;
    
    &:hover {
      background: darken($color-primary, 10%);
    }
  }
  
  &--secondary {
    background: $color-secondary;
    color: white;
    
    &:hover {
      background: darken($color-secondary, 10%);
    }
  }
  
  &--outline {
    background: transparent;
    border: 1px solid $color-primary;
    color: $color-primary;
    
    &:hover {
      background: $color-primary;
      color: white;
    }
  }
}
```

## Responsive Design

### Mobile-First Approach

```scss
// Start with mobile styles
.content {
  padding: $spacing-sm;
  font-size: 0.875rem;
  
  // Add tablet styles
  @media (min-width: $breakpoint-tablet) {
    padding: $spacing-md;
    font-size: 1rem;
  }
  
  // Add desktop styles
  @media (min-width: $breakpoint-desktop) {
    padding: $spacing-lg;
    font-size: 1.125rem;
  }
}
```

### Grid Layouts

```scss
.grid {
  display: grid;
  gap: $spacing-md;
  
  // Mobile: 1 column
  grid-template-columns: 1fr;
  
  // Tablet: 2 columns
  @media (min-width: $breakpoint-tablet) {
    grid-template-columns: repeat(2, 1fr);
  }
  
  // Desktop: 3 columns
  @media (min-width: $breakpoint-desktop) {
    grid-template-columns: repeat(3, 1fr);
  }
}
```

## Accessibility Considerations

### Focus States

```scss
.interactive-element {
  &:focus {
    outline: 2px solid $color-primary;
    outline-offset: 2px;
  }
  
  &:focus:not(:focus-visible) {
    outline: none; // Remove for mouse clicks
  }
  
  &:focus-visible {
    outline: 2px solid $color-primary;
    outline-offset: 2px;
  }
}
```

### Color Contrast

Ensure WCAG AA compliance (4.5:1 for normal text):

```scss
// ✅ CORRECT: Sufficient contrast
.text {
  color: #323130;          // Dark gray on white: 12.63:1
  background: white;
}

// ❌ WRONG: Insufficient contrast
.text {
  color: #cccccc;          // Light gray on white: 1.61:1
  background: white;
}
```

## Performance

### Minimize CSS Output

```scss
// ✅ CORRECT: Efficient selectors
.card {
  padding: $spacing-md;
}

// ❌ WRONG: Over-specific
div.container > div.card > div.content {
  padding: $spacing-md;
}
```

### Avoid @extend

```scss
// ✅ CORRECT: Use mixins or utility classes
@mixin card-styles {
  background: white;
  border-radius: 8px;
  padding: $spacing-md;
}

.card {
  @include card-styles;
}

// ❌ WRONG: @extend creates bloated CSS
.base-card {
  background: white;
  border-radius: 8px;
  padding: $spacing-md;
}

.card {
  @extend .base-card;
}
```

## File Compilation

Jekyll automatically compiles `assets/css/main.scss` to `assets/css/main.css` during build.

**No build tools needed** - Jekyll handles Sass compilation natively.

## Common Issues

### Import Order Matters

Variables must be imported before they're used:

```scss
// ✅ CORRECT
@import 'settings';  // Define variables
@import 'colors';    // Use variables from settings
@import 'base';      // Use variables from settings & colors

// ❌ WRONG
@import 'base';      // Uses undefined variables!
@import 'settings';  // Too late!
```

### Partial Files Must Start with Underscore

```text
✅ _settings.scss   - Correct partial file name
❌ settings.scss    - Will be compiled as separate CSS file
```

## Never Do

- Never create SCSS files without underscore prefix in `_sass/`
- Never import partials without the leading underscore (`@import 'settings'` not `@import '_settings'`)
- Never nest selectors more than 3 levels deep
- Never use ID selectors for styling
- Never use `!important` unless absolutely necessary
- Never create separate CSS files (use SCSS partials imported into main.scss)
