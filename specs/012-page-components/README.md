# 012-page-components

**Status**: TO CREATE  
**Priority**: MVP (12 of 13)  
**Category**: Frontend

## Description

Page-level Blazor component specifications including:

- Home page (section grid, latest roundups)
- Section Index pages (collection tabs, filtering controls)
- Collection pages (filtered content lists)
- Item detail pages (markdown content, metadata, related items)
- Custom pages (GitHub Copilot Features, Levels of Enlightenment, A(i) to Z)

## Why This Is Needed

- Defines user-facing page structure and behavior
- Specifies custom page requirements (Features, Levels of Enlightenment, A(i) to Z)
- Documents special frontmatter handling (`plans`, `ghes_support`, `alt-collection`)
- Establishes responsive layout patterns
- Guides Blazor component composition

## Dependencies

- **Depends on**: 009-blazor-components (reusable components), 011-visual-design-system (styling)
- **Required by**: 014-filtering-system (page integration), 017-search (search UI)
- **Related to**: 004-url-routing (page routes), 013-content-rendering (markdown display)

## Implementation Notes

Must cover:

**Home Page**:

- Section cards grid (8 sections: All, GitHub Copilot, AI, ML, Azure, .NET, DevOps, Security)
- Latest roundups section
- Responsive layout (grid → stacked on mobile)

**Section Index Pages**:

- Collection tabs navigation
- Filtering controls (date, tags, search)
- Content list with "20 + same-day" pagination
- Server-side render, client-side enhancement

**Custom Pages**:

- `/github-copilot/features.html` - Feature showcase with subscription tier filtering
  - Special frontmatter: `plans: ["Free"|"Pro"|"Business"|"Pro+"|"Enterprise"]`
  - Special frontmatter: `ghes_support: true|false`
  - Date-based filtering: Future dates = non-clickable features
  - `alt-collection: "features"` to highlight Features tab
- `/github-copilot/handbook.html` - GitHub Copilot Handbook reference
- `/github-copilot/levels-of-enlightenment.html` - Learning path with embedded videos
- `/github-copilot/vscode-updates.html` - VS Code updates collection
- `/ai/genai-basics.html` - GenAI fundamentals
- `/ai/genai-applied.html` - Applied GenAI use cases  
- `/ai/genai-advanced.html` - Advanced GenAI topics
- `/ai/sdlc.html` - Software Development Lifecycle with AI
- `/devops/dx-space.html` - Developer Experience space
- Other custom markdown pages

**Responsive Patterns**:

- Breakpoints: < 768px (mobile), 768-1024px (tablet), > 1024px (desktop)
- Hamburger menu on mobile
- Stacked layouts on mobile
- Touch-friendly controls (44px minimum)

## References

- `/docs/migration/current-site-analysis.md` - Page types and structure
- `/docs/content-management.md` - GitHub Copilot features special frontmatter
- `/github-copilot/features.md` - Current features page implementation
- `/github-copilot/levels-of-enlightenment.md` - Current learning path page

## Next Steps

1. Use `/speckit.specify` with description: "Create page components for Home, Section Index, Collection, and Item detail pages, plus custom pages including GitHub Copilot Features with subscription tier filtering (plans, ghes_support frontmatter), Levels of Enlightenment, and A(i) to Z guides"
2. Ensure spec covers all special frontmatter handling
3. Include responsive layout requirements
4. Document server-side render + client-side enhancement pattern

