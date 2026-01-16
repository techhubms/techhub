# Feature Specification: NLWeb Semantic HTML Standard

**Feature Branch**: `001-nlweb-semantic-html`  
**Created**: 2026-01-02  
**Status**: Draft  
**Input**: User description: "Create comprehensive NLWeb semantic HTML standard defining exact HTML output for all Blazor components, element usage rules, Schema.org JSON-LD patterns, prohibited wrapper patterns, accessibility integration with WCAG 2.1 AA, and validation testing criteria. Must support pragmatic approach: semantic elements prioritized, maximum 1 wrapper div per component, system fonts, 8px grid spacing, GitHub color palette with accessibility auditing."

## User Scenarios & Testing

### User Story 1 - Screen Reader Content Navigation (Priority: P1)

Screen reader users navigate Tech Hub content using semantic HTML landmarks and structure, enabling them to jump between sections, skip to main content, and understand document hierarchy without visual cues.

**Why this priority**: Accessibility is a core requirement (WCAG 2.1 AA). Screen reader users represent significant portion of developer community.

**Independent Test**: Can be fully tested by navigating site with NVDA/JAWS screen reader and verifying all content is accessible through semantic landmarks and delivers complete content understanding.

**Acceptance Scenarios**:

1. **Given** a screen reader user on home page, **When** they query page landmarks, **Then** they hear distinct `<main>`, `<nav>`, `<header>`, `<footer>` regions
2. **Given** screen reader user on section index page, **When** they navigate by headings, **Then** they can jump between sections using proper `<h1>`-`<h6>` hierarchy
3. **Given** screen reader user viewing item detail, **When** they navigate by article regions, **Then** each content item is wrapped in `<article>` element with clear boundaries

---

### User Story 2 - Search Engine Content Indexing (Priority: P1)

Search engines crawl Tech Hub and extract structured data using Schema.org JSON-LD markup, properly categorizing content types (articles, videos, blog posts) and understanding publication dates, authors, and categories.

**Why this priority**: SEO drives majority of traffic. Proper structured data improves search rankings and rich snippet display.

**Independent Test**: Can be fully tested by running Google Rich Results Test on generated pages and verifying Schema.org validation passes with no errors.

**Acceptance Scenarios**:

1. **Given** a content item page, **When** Google bot crawls it, **Then** it extracts Article schema with headline, datePublished, author, and publisher
2. **Given** a video content page, **When** search engine indexes it, **Then** it recognizes VideoObject schema with name, description, uploadDate, and thumbnail
3. **Given** section index page, **When** crawler processes it, **Then** it understands WebPage or CollectionPage schema with breadcrumb navigation

---

### User Story 3 - Developer Component Implementation (Priority: P1)

Developers building Blazor components reference HTML templates and usage rules to generate consistent, semantic markup without needing to make architectural decisions about element choice or wrapper usage.

**Why this priority**: Developer productivity and consistency depend on clear standards. Without templates, every developer makes different semantic choices.

**Independent Test**: Can be tested by providing developer with component requirements and verifying generated HTML matches template specifications exactly.

**Acceptance Scenarios**:

1. **Given** developer needs to create ItemCard component, **When** they reference specification, **Then** they know exactly which HTML elements to use (`<article>`, `<header>`, `<time>`) and can implement without questions
2. **Given** developer implementing filtering UI, **When** they check prohibited patterns, **Then** they avoid div-itis and use semantic form elements (`<form>`, `<fieldset>`, `<label>`)
3. **Given** developer adding new page type, **When** they review Schema.org patterns, **Then** they implement correct JSON-LD structure without trial and error

---

### User Story 4 - Keyboard-Only Navigation (Priority: P2)

Keyboard-only users navigate entire Tech Hub site using Tab, Enter, Escape, and Arrow keys, accessing all functionality without mouse including filters, navigation, and content browsing.

**Why this priority**: Accessibility requirement for motor-impaired users. Also improves power user experience.

**Independent Test**: Can be tested by unplugging mouse and completing all user tasks using only keyboard, verifying logical tab order and activation.

**Acceptance Scenarios**:

1. **Given** keyboard user on section page, **When** they Tab through page, **Then** they reach all interactive elements in logical order (nav → filters → content → footer)
2. **Given** keyboard user activating filter, **When** they press Space on checkbox, **Then** filter activates and focus remains on control
3. **Given** keyboard user in navigation menu, **When** they press Escape, **Then** menu closes and focus returns to trigger button

---

### User Story 5 - Mobile Screen Reader Experience (Priority: P2)

Mobile screen reader users (VoiceOver on iOS, TalkBack on Android) navigate Tech Hub content effectively using swipe gestures and semantic landmarks optimized for touch interfaces.

**Why this priority**: Mobile traffic represents majority of users. Mobile screen readers have different interaction patterns than desktop.

**Independent Test**: Can be tested by navigating site on iPhone with VoiceOver enabled and verifying all content accessible through swipe gestures.

**Acceptance Scenarios**:

1. **Given** VoiceOver user on mobile, **When** they swipe through page, **Then** semantic landmarks are announced ("Navigation", "Main content", "Article")
2. **Given** TalkBack user viewing content list, **When** they navigate by headings, **Then** each article heading is announced with proper level
3. **Given** mobile screen reader user activating filter, **When** they double-tap, **Then** ARIA live region announces filter result count

---

### User Story 6 - Content Readability Without CSS (Priority: P3)

Users viewing Tech Hub with CSS disabled (browser reading mode, text-only browsers, poor network conditions) access complete content in logical order through semantic HTML structure alone.

**Why this priority**: Progressive enhancement principle. Ensures content is accessible even when stylesheets fail to load.

**Independent Test**: Can be tested by disabling all stylesheets in browser and verifying content remains readable and logically ordered.

**Acceptance Scenarios**:

1. **Given** user disables CSS, **When** they view any page, **Then** content appears in logical reading order (header → main → footer)
2. **Given** CSS is disabled, **When** user views content list, **Then** each article is clearly separated and hierarchy is maintained through heading levels
3. **Given** reading mode activated, **When** browser extracts content, **Then** semantic article structure enables clean extraction without navigation/footer

---

### Edge Cases

- What happens when Blazor component absolutely requires wrapper div for rendering (e.g., event handling on parent)? **Answer**: One wrapper div allowed per component - document in component specification
- How to handle nested interactive elements that violate HTML5 spec (e.g., button inside link)? **Answer**: Prohibited - use separate elements or delegate events
- What if Schema.org type doesn't exist for content format? **Answer**: Use closest parent type (e.g., Article for new content types, fallback to Thing)
- How to maintain semantic HTML when using CSS frameworks that require extra divs? **Answer**: Avoid CSS frameworks with excessive wrappers, use utility-first CSS on semantic elements
- What happens when content is dynamically loaded via infinite scroll? **Answer**: Use ARIA live regions to announce new content, maintain semantic structure in loaded HTML

## Requirements

### Functional Requirements

**HTML Element Selection**:

- **FR-001**: System MUST use semantic HTML5 elements (`<article>`, `<section>`, `<nav>`, `<main>`, `<header>`, `<footer>`, `<aside>`, `<time>`, `<figure>`, `<figcaption>`) for page structure before considering `<div>`
- **FR-002**: System MUST use `<article>` for self-contained content that could be syndicated (news items, blog posts, video entries, community posts)
- **FR-003**: System MUST use `<section>` for thematic groupings of content with a heading (collection of articles, category groupings)
- **FR-004**: System MUST use `<nav>` for navigation blocks (primary header nav, section tabs, footer links, breadcrumbs)
- **FR-005**: System MUST use `<time>` with `datetime` attribute for all dates (publication dates, last modified, event dates)
- **FR-006**: System MUST use `<button>` for actions and `<a>` for navigation - never mix purposes
- **FR-007**: System MUST use `<h1>`-`<h6>` heading hierarchy with no skipped levels (e.g., `<h2>` after `<h1>`, not `<h3>`)

**Component Wrapper Rules**:

- **FR-008**: Blazor components MAY include maximum ONE wrapper `<div>` per component if required for Blazor rendering or event handling
- **FR-009**: Wrapper divs MUST include semantic CSS class indicating component purpose (e.g., `class="item-card"`, `class="filter-panel"`)
- **FR-010**: Nested components MUST NOT each add wrapper divs - maximum one wrapper div per component, not per nesting level
- **FR-011**: If wrapper div is not required for Blazor functionality, component MUST output semantic element directly (no wrapper)

**Schema.org Structured Data**:

- **FR-012**: System MUST include Schema.org structured data using JSON-LD format in `<script type="application/ld+json">` tags
- **FR-013**: Content item pages (news, blogs, videos) MUST include Article or specialized type (NewsArticle, BlogPosting, VideoObject) schema
- **FR-014**: Article schema MUST include: `headline`, `datePublished`, `dateModified`, `author`, `publisher`, `image`, `description` properties
- **FR-015**: VideoObject schema MUST include: `name`, `description`, `uploadDate`, `thumbnailUrl`, `contentUrl` (or `embedUrl`) properties
- **FR-016**: Section and collection pages MUST include WebPage or CollectionPage schema with `name`, `description`, `breadcrumb` properties
- **FR-017**: Home page MUST include WebSite schema with `name`, `url`, `description`, `publisher`, `potentialAction` (SearchAction) properties
- **FR-018**: All pages MUST include Organization schema in footer with `name`, `url`, `logo`, `sameAs` (social media links)

**Accessibility Integration**:

- **FR-019**: System MUST use ARIA attributes ONLY when semantic HTML is insufficient (e.g., `aria-expanded` for collapsed sections, `aria-live` for dynamic updates)
- **FR-020**: Interactive elements MUST be keyboard accessible with visible focus indicators meeting 3:1 contrast ratio
- **FR-021**: Form inputs MUST have associated `<label>` elements using `for`/`id` or wrapping pattern
- **FR-022**: Images MUST have `alt` attributes - descriptive for content images, empty for decorative images
- **FR-023**: ARIA landmarks MUST match semantic HTML elements (e.g., `<nav>` implies `role="navigation"` - don't duplicate)

**Typography and Spacing**:

- **FR-024**: System MUST use system font stack (e.g., `-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif`) for performance and native appearance
- **FR-025**: System MUST use 8px grid spacing system for margins, padding, and gaps (multiples of 8: 8px, 16px, 24px, 32px, etc.)
- **FR-026**: Font sizes MUST be defined in rem units for accessibility (user can scale text)
- **FR-027**: Line height MUST be minimum 1.5 for body text for readability (WCAG 2.1 requirement)

**Color and Contrast**:

- **FR-028**: System MUST use Tech Hub color palette (dark theme with Microsoft brand colors)
- **FR-029**: System MUST audit all color combinations to ensure 4.5:1 contrast ratio for normal text (WCAG 2.1 AA)
- **FR-030**: System MUST ensure 3:1 contrast ratio for large text (18pt+) and UI components
- **FR-031**: System MUST NOT rely on color alone to convey information (use icons, labels, or patterns in addition)

**Prohibited Patterns**:

- **FR-032**: System MUST NOT use div-itis (excessive nested divs without semantic purpose)
- **FR-033**: System MUST NOT use `<div class="button">` - use `<button>` element
- **FR-034**: System MUST NOT use `<span>` or `<div>` for dates - use `<time>` element
- **FR-035**: System MUST NOT nest interactive elements (e.g., `<button>` inside `<a>` or vice versa)
- **FR-036**: System MUST NOT use generic class names on semantic elements that duplicate element name (e.g., `<article class="article">` - just use `<article>`)
- **FR-037**: System MUST NOT include more than 2 wrapper divs total in any component output (preferably 0 or 1)

**Markdown to HTML Conversion**:

- **FR-039**: Markdown headings (`#`, `##`, etc.) MUST convert to `<h1>`-`<h6>` maintaining hierarchy
- **FR-040**: Markdown lists MUST convert to `<ul>`/`<ol>` with proper `<li>` elements
- **FR-041**: Markdown code blocks MUST convert to `<pre><code class="language-*">` with language class
- **FR-042**: Markdown links MUST convert to `<a>` elements with proper `href` attributes
- **FR-043**: Markdown emphasis MUST convert to `<em>` (italic) and `<strong>` (bold) elements

### Key Entities

**Semantic Element** represents HTML5 semantic elements with defined usage rules:

- Element name (article, section, nav, etc.)
- Purpose and when to use
- Required/optional attributes
- Content model (what can be inside)
- ARIA implications (implicit roles)

**Component Template** represents expected HTML output for each Blazor component:

- Component name
- Input parameters
- Expected HTML structure
- Semantic elements used
- Schema.org integration points
- Accessibility attributes
- Wrapper div justification (if present)

**Schema Type** represents Schema.org structured data types:

- Schema.org type name (Article, VideoObject, WebSite, etc.)
- Required properties
- Optional properties
- JSON-LD template
- Which pages use this schema

## Success Criteria

### Measurable Outcomes

- **SC-001**: 100% of pages pass HTML5 semantic validation with zero errors
- **SC-002**: 100% of pages pass Schema.org validation (Google Rich Results Test) with zero errors
- **SC-003**: 100% of pages achieve Lighthouse Accessibility score of 95 or higher
- **SC-004**: 100% of pages achieve WCAG 2.1 Level AA compliance (0 violations in axe DevTools)
- **SC-005**: All interactive elements reachable via keyboard navigation in logical tab order
- **SC-006**: Screen reader testing passes with NVDA and JAWS on Windows, VoiceOver on macOS/iOS
- **SC-007**: All color combinations meet 4.5:1 contrast ratio for normal text (automated audit)
- **SC-008**: Maximum 1 wrapper div per component (code review verification)
- **SC-009**: Zero prohibited patterns detected in generated HTML (automated linting)
- **SC-010**: Content remains readable and logically ordered with CSS disabled

## Assumptions

1. **Blazor SSR**: Initial page render is server-side, semantic HTML in initial response
2. **Progressive Enhancement**: JavaScript enhances but doesn't create initial content
3. **Modern Browsers**: Target browsers support HTML5 semantic elements (IE11 not supported)
4. **Tech Hub Colors**: Dark theme with Microsoft brand colors (primary blue, dark navy, etc.)
5. **System Fonts Available**: Target OS includes modern system font stack
6. **Screen Reader Testing**: Team has access to NVDA, JAWS, VoiceOver for testing
7. **No Dark Mode**: Site color scheme is already dark, no light/dark toggle needed

## Out of Scope

1. **Dark Mode**: Not implementing theme switching (site uses dark theme only)
2. **CSS Framework**: Not using Bootstrap, Tailwind, or similar (utility-first custom CSS only)
3. **Backward Compatibility**: No support for IE11 or legacy browsers requiring polyfills
4. **Microdata or RDFa**: Only JSON-LD for Schema.org (not mixing encoding methods)
5. **Breadcrumb Schema**: Deferred to URL Routing specification (depends on URL structure decisions)
6. **Print Stylesheets**: Not defining print-specific semantic HTML patterns
7. **Email Templates**: Not defining HTML email formatting (web pages only)
8. **RSS Feed HTML**: Deferred to RSS Feeds specification

## Dependencies

1. **Visual Design System Spec** (to be created) - Provides exact GitHub color hex values and spacing scale
2. **Blazor Components Spec** (exists: `/specs/blazor-components/spec.md`) - Defines which components need HTML templates
3. **Content Rendering Spec** (exists: `/specs/content-rendering/spec.md`) - Defines markdown processing requirements
4. **URL Routing Spec** (to be created) - Provides URL structure for canonical links and breadcrumbs

## Appendix: Component HTML Templates

### ItemCard Component

**Purpose**: Display content item in section/collection lists

**Expected HTML Output**:

```html
<article class="item-card">
  <header>
    <h3>
      <a href="/section/collection/2026-01-02-item-slug">Item Title</a>
    </h3>
    <time datetime="2026-01-02T00:00:00+01:00">January 2, 2026</time>
  </header>
  <p>Item excerpt text...</p>
</article>
```

**Wrapper Justification**: None - component outputs `<article>` directly

**Schema.org**: Page-level JSON-LD includes ItemList with references to articles

**Accessibility**: Heading level (h3) must be configurable based on page context

---

### SectionNav Component

**Purpose**: Display section sub-navigation tabs

**Expected HTML Output**:

```html
<nav class="section-nav" aria-label="Section collections">
  <ul role="tablist">
    <li role="presentation">
      <a href="/section/collection" role="tab" aria-selected="true">Collection Name</a>
    </li>
    <li role="presentation">
      <a href="/section/other" role="tab" aria-selected="false">Other Collection</a>
    </li>
  </ul>
</nav>
```

**Wrapper Justification**: None - component outputs `<nav>` directly

**ARIA**: Uses tablist pattern for horizontal navigation tabs

**Accessibility**: `aria-selected` indicates active tab, `aria-label` distinguishes from primary nav

---

### FilterPanel Component

**Purpose**: Date/tag/text filtering controls

**Expected HTML Output**:

```html
<form class="filter-panel" role="search">
  <fieldset>
    <legend>Filter by Date</legend>
    <select name="date" aria-label="Date range">
      <option value="7">Last 7 days</option>
      <option value="30">Last 30 days</option>
    </select>
  </fieldset>
  <fieldset>
    <legend>Filter by Tags</legend>
    <div class="tag-list">
      <label>
        <input type="checkbox" name="tag" value="azure" />
        Azure
      </label>
    </div>
  </fieldset>
  <fieldset>
    <legend>Search</legend>
    <label for="search-input" class="visually-hidden">Search content</label>
    <input type="search" id="search-input" name="q" placeholder="Search..." />
  </fieldset>
</form>
```

**Wrapper Justification**: None for component, `.tag-list` div groups checkboxes for layout

**Semantic Elements**: `<form>`, `<fieldset>`, `<legend>`, `<label>`, `<input>`

**Accessibility**: Labels for all inputs, fieldset grouping, search role

---

### MainLayout Component

**Purpose**: Site-wide page structure

**Expected HTML Output**:

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <!-- meta tags, title, schema.org JSON-LD -->
</head>
<body>
  <header class="site-header">
    <nav aria-label="Primary navigation">
      <!-- primary nav -->
    </nav>
  </header>
  
  <main id="main">
    @Body
  </main>
  
  <footer class="site-footer">
    <nav aria-label="Footer navigation">
      <!-- footer links -->
    </nav>
  </footer>
</body>
</html>
```

**Wrapper Justification**: None - uses semantic page structure elements

**Landmarks**: `<header>`, `<main>`, `<footer>`, `<nav>` create ARIA landmarks

**Accessibility**: Labeled navigation regions, keyboard-accessible focus states

---

## Appendix: Validation Testing Strategy

### Automated Tests (CI/CD)

1. **HTML5 Validation**: Run W3C validator on generated pages (zero errors)
2. **Schema.org Validation**: Run Google Rich Results Test API (zero errors)
3. **Accessibility Audit**: Run axe-core via Playwright (zero violations)
4. **Color Contrast**: Run automated contrast checker on all text (4.5:1 minimum)
5. **Semantic Lint**: Custom rules checking for prohibited patterns (div-itis, nested buttons, etc.)

### Manual Testing (Pre-Release)

1. **Screen Reader**: NVDA on Windows - navigate home, section, item pages
2. **Screen Reader**: JAWS on Windows - test filtering and navigation
3. **Screen Reader**: VoiceOver on macOS - verify landmark navigation
4. **Screen Reader**: VoiceOver on iOS - test mobile experience
5. **Keyboard Navigation**: Tab through site, verify logical order and focus indicators
6. **CSS Disabled**: Disable stylesheets, verify content remains readable
7. **Browser Reading Mode**: Activate reader view, verify clean content extraction

### Code Review Checklist

For each component:

- [ ] Uses semantic elements before considering `<div>`
- [ ] Maximum 1 wrapper div (if any)
- [ ] Wrapper div has semantic class name (if present)
- [ ] Proper heading hierarchy maintained
- [ ] Schema.org JSON-LD included (if page component)
- [ ] ARIA only where semantic HTML insufficient
- [ ] All interactive elements keyboard accessible
- [ ] Color contrast meets 4.5:1 ratio
- [ ] System fonts used (no custom font loading)
- [ ] 8px grid spacing applied

## Appendix: HTML Element Decision Tree

**When selecting HTML element for component output**:

1. Is it self-contained content that could be syndicated?
   - **YES** → Use `<article>`
   - **NO** → Continue to 2

2. Is it a thematic grouping of content with a heading?
   - **YES** → Use `<section>`
   - **NO** → Continue to 3

3. Is it navigation (links to other pages/sections)?
   - **YES** → Use `<nav>`
   - **NO** → Continue to 4

4. Is it supplementary/complementary content?
   - **YES** → Use `<aside>`
   - **NO** → Continue to 5

5. Is it a date or time?
   - **YES** → Use `<time datetime="...">`
   - **NO** → Continue to 6

6. Is it an action (not navigation)?
   - **YES** → Use `<button>`
   - **NO** → Continue to 7

7. Is it navigation to another page?
   - **YES** → Use `<a href="...">`
   - **NO** → Continue to 8

8. Is it a form control?
   - **YES** → Use `<input>`, `<select>`, `<textarea>` with `<label>`
   - **NO** → Continue to 9

9. Is it grouping for styling/layout only with no semantic meaning?
   - **YES** → Use `<div>` (document why semantic element doesn't fit)
   - **NO** → Re-evaluate, likely missing semantic use case

## Appendix: Implementation Notes

1. **Start with Semantic Element**: Default to semantic HTML, justify `<div>` usage
2. **One Wrapper Max**: If Blazor requires wrapper, limit to ONE per component
3. **Test Without CSS**: Ensure content readable with stylesheets disabled
4. **Keyboard First**: Test all interactions with keyboard before mouse
5. **Screen Reader Validate**: Run NVDA on every new component
6. **Schema.org Separate**: Keep JSON-LD in Razor code-behind or separate partial
7. **ARIA Last Resort**: Exhaust semantic HTML options before adding ARIA
8. **GitHub Colors**: Reference design system spec for exact hex values
9. **System Fonts**: Never load custom web fonts, use system font stack
10. **8px Grid**: All spacing must be multiple of 8px (no 5px, 12px, 15px, etc.)
