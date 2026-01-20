# Feature Specification: Custom Pages Production Quality

**Feature Branch**: `004-custom-pages`  
**Created**: 2026-01-16  
**Status**: Draft  
**Input**: Bring all 8 custom pages to production quality with proper SidebarToc component usage, 50vh scroll spacer, styling matching live site reference examples, and complete content structure including cards, timelines, pricing grids, and interactive elements

## User Scenarios & Testing

### User Story 1 - Browse GitHub Copilot Features (Priority: P1)

Users can explore GitHub Copilot features organized into feature groups with visual cards showing feature icons, descriptions, and links to documentation.

**Live Site Reference**: <https://tech.hub.ms/github-copilot/features>

**Why this priority**: Most visited custom page, serves as primary entry point for developers learning GitHub Copilot capabilities.

**Independent Test**: Navigate to `/github-copilot/features`, verify all feature groups display with cards, icons are visible, links navigate to correct documentation.

**Acceptance Scenarios**:

1. **Given** I visit `/github-copilot/features`, **When** the page loads, **Then** I see the GitHub Copilot Features header with background image
2. **Given** I'm on the features page, **When** I scroll down, **Then** I see feature groups (Core, Advanced, Enterprise, etc.) with icon-based cards
3. **Given** I see a feature card, **When** I click the card or link, **Then** I navigate to the feature documentation
4. **Given** the page has multiple sections, **When** I use the sidebar TOC, **Then** I can jump to any section and the TOC highlights the current section as I scroll

---

### User Story 2 - Read VS Code Updates (Priority: P1)

Users can browse GitHub Copilot updates for Visual Studio Code organized by video entries with embedded players, descriptions, and table of contents navigation.

**Live Site Reference**: <https://tech.hub.ms/github-copilot/vscode-updates>

**Why this priority**: Already implemented as reference example, validates the pattern works correctly.

**Independent Test**: Navigate to `/github-copilot/vscode-updates`, verify video list displays, clicking videos loads content with sidebar TOC, scroll spy works.

**Acceptance Scenarios**:

1. **Given** I visit `/github-copilot/vscode-updates`, **When** the page loads, **Then** I see a list of video updates with thumbnails and titles
2. **Given** I click a video entry, **When** the content loads, **Then** I see the video player, description, and sidebar TOC
3. **Given** I'm reading video content, **When** I scroll through sections, **Then** the TOC highlights the current section
4. **Given** I reach the last content section, **When** I continue scrolling, **Then** I can scroll the last section high enough to trigger its TOC highlight (50vh spacer)

---

### User Story 3 - Learn GenAI Basics/Applied/Advanced (Priority: P2)

Users can progress through GenerativeAI learning paths from basics to advanced topics with structured content, code examples, and visual diagrams.

**Live Site References**:

- GenAI Basics: <https://tech.hub.ms/ai/genai-basics>
- GenAI Applied: <https://tech.hub.ms/ai/genai-applied>
- GenAI Advanced: <https://tech.hub.ms/ai/genai-advanced>

**Why this priority**: Educational progression path, supports learning journey from beginner to expert.

**Independent Test**: Navigate to `/ai/genai-basics`, `/ai/genai-applied`, `/ai/genai-advanced`, verify content hierarchy, code examples render correctly, sidebar TOC works.

**Acceptance Scenarios**:

1. **Given** I visit any GenAI page, **When** the page loads, **Then** I see structured content with headings, paragraphs, and code examples
2. **Given** I'm on GenAI Basics, **When** I scroll through content, **Then** I see concepts explained with diagrams and examples
3. **Given** I complete GenAI Basics, **When** I navigate to GenAI Applied, **Then** I see practical applications building on basic concepts
4. **Given** I'm on any GenAI page, **When** I use the sidebar TOC, **Then** it auto-extracts headings and highlights active sections

---

### User Story 4 - Access GitHub Copilot Handbook (Priority: P2)

Users can access the GitHub Copilot Handbook as a comprehensive reference with book information, purchase links, and content preview.

**Live Site Reference**: <https://tech.hub.ms/github-copilot/handbook>

**Why this priority**: Promotes official Microsoft Press book, drives awareness and sales.

**Independent Test**: Navigate to `/github-copilot/handbook`, verify book cover displays, purchase links work, content sections are organized.

**Acceptance Scenarios**:

1. **Given** I visit `/github-copilot/handbook`, **When** the page loads, **Then** I see the book cover, title, author information, and purchase links
2. **Given** I see purchase options, **When** I click a retailer link, **Then** I navigate to the correct purchase page
3. **Given** the page has content sections, **When** I scroll, **Then** the sidebar TOC tracks my position through the content

---

### User Story 5 - Explore GitHub Copilot Levels of Enlightenment (Priority: P2)

Users can understand their GitHub Copilot proficiency progression through levels with clear criteria, examples, and next steps for each level.

**Live Site Reference**: <https://tech.hub.ms/github-copilot/levels-of-enlightenment>

**Why this priority**: Gamification encourages skill development and engagement.

**Independent Test**: Navigate to `/github-copilot/levels-of-enlightenment`, verify all levels display with icons, descriptions, and progression indicators.

**Acceptance Scenarios**:

1. **Given** I visit the levels page, **When** the page loads, **Then** I see all proficiency levels listed with visual indicators
2. **Given** I see a level, **When** I read its description, **Then** I understand the criteria and skills for that level
3. **Given** I want to progress, **When** I review the next level, **Then** I see clear actionable steps to advance

---

### User Story 6 - Discover AI in SDLC (Priority: P3)

Users can explore how AI integrates into software development lifecycle phases with timeline visualization and phase-specific guidance.

**Live Site Reference**: <https://tech.hub.ms/ai/sdlc>

**Why this priority**: Strategic content for understanding AI adoption across development processes.

**Independent Test**: Navigate to `/ai/sdlc`, verify timeline displays, each phase has content, visual elements render correctly.

**Acceptance Scenarios**:

1. **Given** I visit `/ai/sdlc`, **When** the page loads, **Then** I see a timeline visualization of SDLC phases
2. **Given** I see SDLC phases, **When** I read each phase, **Then** I understand how AI assists in that phase
3. **Given** the page has visual elements, **When** they render, **Then** they enhance understanding without blocking content

---

### User Story 7 - Explore Developer Experience Space (Priority: P3)

Users can discover the Developer Experience Space framework with pillars, metrics, and implementation guidance organized visually.

**Live Site Reference**: <https://tech.hub.ms/devops/dx-space>

**Why this priority**: Niche enterprise content for DevOps/Platform Engineering leaders.

**Independent Test**: Navigate to `/devops/dx-space`, verify framework pillars display, metrics are organized, content structure is clear.

**Acceptance Scenarios**:

1. **Given** I visit `/devops/dx-space`, **When** the page loads, **Then** I see the DX Space framework overview
2. **Given** I explore framework pillars, **When** I read each pillar, **Then** I see associated metrics and guidance
3. **Given** the page uses cards or grids, **When** they render, **Then** content is organized and scannable

---

### Edge Cases

- What happens when a custom page has no content sections (fewer than 2 headings)? → TOC should not render
- What happens when embedded videos fail to load? → Show placeholder with error message, don't block page content
- What happens when sidebar TOC has many nested levels? → Limit depth to h2/h3, collapse deeper nesting
- What happens when content is very short (less than viewport height)? → TOC still renders but scroll spy may not activate
- What happens when user navigates directly to a heading anchor (#section-id)? → Page scrolls to that heading, TOC highlights it
- What happens on mobile/narrow viewports? → Sidebar TOC collapses or moves to different position (handled by 011-mobile-navigation spec)

## Requirements

### Functional Requirements

- **FR-001**: All 8 custom pages MUST use the `<SidebarToc>` component for table of contents generation
- **FR-002**: All custom pages MUST wrap content in `.article-body` container for scroll spy functionality
- **FR-003**: All custom pages MUST include `.article-body::after { height: 50vh }` spacer (via global CSS)
- **FR-004**: TOC component MUST auto-extract headings from HTML content (h2 and h3 tags)
- **FR-005**: TOC component MUST highlight the active section when corresponding heading crosses the detection point (~40% from viewport top)
- **FR-006**: Clicking a TOC link MUST smooth-scroll to the corresponding heading and update browser URL with anchor
- **FR-007**: Custom pages MUST match visual styling from reference examples (cards, grids, timelines where applicable) BUT adapt to Tech Hub color scheme and design system
- **FR-008**: Custom pages MUST apply Tech Hub visual identity (section background images, color palette, typography matching existing components)
- **FR-009**: Sidebar MUST be used creatively to improve navigation, content discovery, and user experience (not limited to TOC only)
- **FR-010**: Feature cards MUST display with icons, titles, descriptions, and clickable links using Tech Hub styling
- **FR-011**: Video content pages MUST support embedded video players with descriptions
- **FR-012**: Book/handbook pages MUST display cover images, purchase links, and author information
- **FR-013**: Timeline/framework pages MUST organize content visually with appropriate layout components
- **FR-014**: All interactive elements (cards, links, buttons) MUST be keyboard accessible
- **FR-015**: All images MUST have alt text for screen readers
- **FR-016**: All custom pages MUST have proper semantic HTML (nav, main, aside, article)
- **FR-017**: Page load performance MUST be under 2 seconds on standard connections
- **FR-018**: Sidebar MAY include additional components beyond TOC (page info, RSS links, quick links, related content) where they enhance UX

### Key Entities

- **CustomPage**: Represents a feature-specific page with unique layout and content structure (features, handbook, levels, GenAI, SDLC, DX Space)
- **SidebarToc**: Component that parses HTML content and generates hierarchical navigation with scroll spy
- **ContentSection**: Logical grouping of content under a heading (h2 or h3) that appears in TOC
- **FeatureCard**: Visual element with icon, title, description, and link used on features page
- **VideoEntry**: Content item with embedded player, description, and metadata for VS Code updates
- **TimelinePhase**: Visual element representing a stage in SDLC or DX framework

## Success Criteria

### Measurable Outcomes

- **SC-001**: Users can navigate to any custom page and see fully rendered content within 2 seconds
- **SC-002**: All 8 custom pages have functional TOC with scroll spy that highlights active sections
- **SC-003**: Users can scroll the last content section to detection point on all pages (50vh spacer working)
- **SC-004**: All interactive elements (cards, links, TOC items) are keyboard accessible (Tab/Enter navigation works)
- **SC-005**: All custom pages pass accessibility validation (WCAG AA compliance for semantic HTML, alt text, ARIA labels)
- **SC-006**: Zero console errors or warnings on any custom page load
- **SC-007**: Visual design matches reference examples for card layouts, grids, and component styling BUT uses Tech Hub color scheme consistently
- **SC-008**: TOC auto-extraction works correctly for all pages (extracts all h2/h3 headings with proper nesting)
- **SC-009**: Sidebar components are used effectively to enhance navigation and content discovery beyond basic TOC functionality

## Implementation Notes

### Reference Documentation

- [docs/toc-component.md](/docs/toc-component.md) - SidebarToc component architecture and usage
- [src/TechHub.Web/AGENTS.md](/src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [src/TechHub.Web/Components/Pages/GitHubCopilotVSCodeUpdates.razor](/src/TechHub.Web/Components/Pages/GitHubCopilotVSCodeUpdates.razor) - Reference implementation

### Current Status

**Completed**:

- VS Code Updates page (reference implementation with SidebarToc component, 50vh spacer, styling)

**Needs Migration** (manual TOC → SidebarToc component):

- GitHubCopilotFeatures.razor
- GitHubCopilotHandbook.razor
- GitHubCopilotLevels.razor
- GenAIBasics.razor
- GenAIApplied.razor
- GenAIAdvanced.razor
- AISDLC.razor
- DXSpace.razor

### Component Requirements

**All pages must**:

1. Replace manual `<nav class="sidebar-toc">` with `<SidebarToc HtmlContent="@renderedHtml" />` component
2. Ensure content is wrapped in `<div class="article-body">` for scroll spy
3. Verify 50vh spacer is present (global CSS, no per-page changes needed)
4. Add proper page metadata (title, description, canonical URL)
5. Implement proper semantic HTML structure
6. Test keyboard navigation and screen reader compatibility

### Visual Design Elements

**Design Philosophy**:

- **Reference live site structure and layout** (card patterns, grids, timelines, spacing) BUT adapt to new Tech Hub color scheme
- **Apply Tech Hub visual identity**: Use section background images, Tech Hub color palette, modern typography from existing components (SectionCard, ContentItemCard)
- **Maximize sidebar utility**: Use sidebar creatively beyond just TOC - add page metadata, quick links, related resources, navigation aids, or any element that improves content discoverability and page navigation
- **Modern, scannable layout**: Prioritize clear visual hierarchy, whitespace, responsive design, and mobile-first thinking
- **Enhance content presentation**: Use visual elements (cards, icons, timelines, grids) where they add clarity and improve user understanding

**Sidebar Usage Guidelines**:

The sidebar is not limited to table of contents. Consider these additional uses:

- **SidebarPageInfo**: Display page metadata (author, last updated, reading time, related links)
- **SidebarRssLinks**: Provide RSS feed subscription options
- **Quick Links**: Jump to specific tools, resources, or documentation mentioned in content
- **Related Content**: Link to related custom pages or sections
- **Progress Indicators**: For learning paths (GenAI Basics → Applied → Advanced)
- **Navigation Aids**: Complementary navigation beyond TOC where it improves UX
- **Visual Summaries**: Key takeaways, glossary terms, or reference materials

**Cards** (GitHub Copilot Features page):

- Icon at top (SVG or font icon) - use Tech Hub icon style
- Title in bold/heading
- Description text below
- Clickable card or link
- Hover states for interactivity matching existing component patterns
- Tech Hub colors for backgrounds, borders, and accent elements

**Timelines** (AI in SDLC page):

- Horizontal or vertical flow
- Phase markers/icons
- Content for each phase
- Visual connectors between phases
- Tech Hub color scheme for timeline elements

**Grids** (DX Space page):

- Responsive column layout (CSS Grid or Flexbox)
- Equal-height cards
- Consistent spacing matching Tech Hub design system
- Clear visual hierarchy
- Tech Hub colors for card backgrounds and borders

**Book Display** (Handbook page):

- Book cover image
- Title, author, publisher info
- Purchase links (multiple retailers) styled as Tech Hub buttons
- Content preview or description
- Consider sidebar for book details, purchase options, or related resources

### Testing Strategy

**Component Tests** (bUnit):

- SidebarToc extracts headings correctly
- TOC renders with proper hierarchy
- Component accepts HtmlContent parameter

**E2E Tests** (Playwright):

- Navigate to each custom page URL
- Verify content renders
- Test TOC click → scroll to heading
- Test scroll → TOC highlight updates
- Verify last section can scroll to detection point
- Test keyboard navigation (Tab through TOC, Enter to activate)
- Verify no console errors on page load

**Accessibility Tests**:

- Lighthouse audit for WCAG AA compliance
- Screen reader navigation (nvda/JAWS simulation)
- Keyboard-only navigation verification
- Color contrast validation

## Dependencies

- **Completed**: SidebarToc component (already exists and works correctly)
- **Completed**: Global scroll spacer CSS (already in article.css)
- **Completed**: Blazor routing for custom page URLs (already configured)
- **Optional**: New visual components if cards/timelines need custom implementation (can reuse existing or create new)

## Out of Scope

- Mobile navigation layout (covered in 011-mobile-navigation spec)
- Content creation or authoring (content already exists, just needs proper component usage)
- Search/filtering integration (covered in 001-filtering-system and 002-search specs)
- Analytics tracking (covered in 006-google-analytics spec)
- SEO metadata beyond canonical URLs (covered in 005-seo spec)
