# 016-visual-design-system

**Status**: TO CREATE  
**Priority**: MVP (11 of 13)  
**Category**: Foundation

## Description

Visual design system specification including:

- Color palette (GitHub brand colors)
- Typography system (fonts, sizes, weights, line heights)
- Spacing standards (margin, padding scale)
- CSS architecture (SCSS/CSS structure)
- Responsive breakpoints (mobile, tablet, desktop)
- Accessibility standards (color contrast 4.5:1, focus states, keyboard navigation)

## Why This Is Needed

- Defines visual foundation for all components
- Ensures consistent look and feel across site
- Provides design tokens for theming
- Establishes accessibility baselines (WCAG 2.1 AA)
- Guides CSS organization and maintainability

## Dependencies

- **Depends on**: 001-solution-structure (project setup)
- **Required by**: 012-page-components (uses design system)
- **Related to**: 010-nlweb-semantic-html (semantic structure)

## Implementation Notes

Must cover:

- All GitHub brand colors with semantic naming
- Typography hierarchy (h1-h6, body, code, etc.)
- Spacing scale (consistent margins/padding)
- Responsive breakpoints matching current site (< 768px, 768-1024px, > 1024px)
- Accessibility requirements:
  - Color contrast ratios (4.5:1 for normal text, 3:1 for large text/UI)
  - Focus indicator styling (visible, consistent)
  - Keyboard navigation patterns (tab order, arrow keys, escape)
  - Touch target sizes (44px minimum for mobile)

## References

- `/docs/migration/current-site-analysis.md` - Current site responsive breakpoints
- `_sass/` directory - Current SCSS architecture
- Root `AGENTS.md` - Accessibility standards section

## Next Steps

1. Use `/speckit.specify` with description: "Create visual design system with GitHub colors, typography, spacing standards, responsive breakpoints, and accessibility requirements including color contrast, focus states, and keyboard navigation"
2. Ensure spec covers all design tokens needed for components
3. Include CSS architecture patterns for maintainability
