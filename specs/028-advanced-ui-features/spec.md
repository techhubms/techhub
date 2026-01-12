# GitHub Copilot Levels of Enlightenment Feature

## Overview

Dedicated educational landing page showcasing GitHub Copilot's progressive learning journey through distinct capability levels. This curated content page helps developers understand and advance their GitHub Copilot skills from beginner to expert.

## Functional Requirements

### FR-001: Levels of Enlightenment Landing Page

**Priority**: P2

Create a dedicated `/github-copilot/levels-of-enlightenment` page that curates and showcases content organized by GitHub Copilot skill progression levels.

**Page Structure**:

```text
/github-copilot/levels-of-enlightenment
├── Hero Section
│   └── "Journey Through GitHub Copilot Mastery"
├── Level 1: Awareness
│   └── Introduction to Copilot basics
├── Level 2: Experimentation  
│   └── First hands-on experiences
├── Level 3: Integration
│   └── Incorporating into workflow
├── Level 4: Optimization
│   └── Advanced techniques
└── Level 5: Mastery
    └── Expert patterns and best practices
```

**Content Organization**:

- Each level displays curated content (videos, blogs, community posts)
- Content filtered by custom `level` frontmatter field (e.g., `level: 1`, `level: 2`)
- Alternatively, manually specify content items via configuration
- Progressive disclosure: Show summary by default, expand for full content list

**Design Requirements**:

- Visual progression indicators (badges, icons, progress bar)
- Consistent color scheme per level for visual hierarchy
- Responsive grid layout for content cards
- Clear call-to-action for each level's resources

**Acceptance Criteria**:

1. ✅ Page accessible at `/github-copilot/levels-of-enlightenment`
2. ✅ Displays all 5 levels with clear visual hierarchy
3. ✅ Each level shows curated content items (videos, blogs, etc.)
4. ✅ Content cards link to individual content detail pages
5. ✅ Page is fully responsive (mobile, tablet, desktop)
6. ✅ Page follows Tech Hub visual design system
7. ✅ Accessibility: Proper heading structure, ARIA labels, keyboard navigation
8. ✅ SEO: Meta tags, structured data, og:image

## Technical Requirements

### Content Filtering Approach

#### Option 1: Frontmatter-Based (Recommended)

Add `level` field to content frontmatter:

```yaml
---
title: "Getting Started with GitHub Copilot"
date: 2025-01-15
categories: ["GitHub Copilot"]
tags: ["beginner", "tutorial"]
level: 1  # Levels of Enlightenment: 1-5
---
```

Filter content in Blazor component:

```csharp
// LevelsOfEnlightenment.razor.cs
private List<ContentItem> GetContentForLevel(int level)
{
    return AllContent
        .Where(c => c.Level == level)
        .OrderByDescending(c => c.Date)
        .Take(10)
        .ToList();
}
```

#### Option 2: Configuration-Based

Manually specify content items in `appsettings.json`:

```json
{
  "LevelsOfEnlightenment": {
    "Level1": ["2025-01-15-getting-started.md", "2025-01-10-intro-video.md"],
    "Level2": ["2025-01-12-first-project.md"],
    "Level3": ["2025-01-08-workflow-integration.md"],
    "Level4": ["2025-01-05-advanced-techniques.md"],
    "Level5": ["2025-01-01-expert-patterns.md"]
  }
}
```

### Blazor Component Structure

```text
src/TechHub.Web/Components/Pages/LevelsOfEnlightenment.razor
src/TechHub.Web/Components/LevelCard.razor
src/TechHub.Web/Components/LevelContentList.razor
```

### Styling

- Reuse existing `ContentItemCard` component for content display
- Create `LevelCard` component for level headers with visual badges
- Use CSS Grid for responsive layout
- Follow Tech Hub color palette for level differentiation

## Acceptance Criteria

### Overall Feature

1. ✅ Page fully renders with all 5 levels
2. ✅ Content correctly filtered by level
3. ✅ Visual design matches Tech Hub style
4. ✅ Fully responsive across all breakpoints
5. ✅ Accessibility standards met (WCAG 2.1 AA)
6. ✅ SEO optimized (meta tags, structured data)
7. ✅ Tests cover component rendering and content filtering
8. ✅ Documentation updated in relevant AGENTS.md files

## Priority

**P2** - Important educational feature for GitHub Copilot section

## Dependencies

- [016-visual-design-system](../016-visual-design-system/) - Visual design guidelines (documented in src/TechHub.Web/AGENTS.md)
- [018-content-rendering](../018-content-rendering/) - Content rendering (documented in src/TechHub.Web/AGENTS.md)
- [collections/markdown-guidelines.md](../../collections/markdown-guidelines.md) - Frontmatter field requirements

## References

- GitHub Copilot Learning Journey documentation
- Tech Hub visual design system
- Progressive disclosure UX patterns

## Notes

**Features Moved to Other Specs**:

- **Mobile Navigation** → [029-mobile-navigation](../029-mobile-navigation/)
- **Schema.org Markup** → [023-seo](../023-seo/) (Implementation Patterns section)
- **CORS Testing** → [005-integration-testing](../005-integration-testing/) (FR-002)
- **Middleware Testing** → [005-integration-testing](../005-integration-testing/) (FR-003)
