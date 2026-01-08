# Section System Specification

> **Feature**: Configuration-driven section and collection management system

## Overview

The section system organizes all content into logical topic areas (sections) and content types (collections). All configuration is driven by a single JSON file (`sections.json`), ensuring consistency and maintainability. Content is loaded dynamically using infinite scroll pagination for optimal performance.

## Requirements

### Functional Requirements

**FR-1**: The system MUST read all section and collection definitions from `sections.json`  
**FR-2**: The system MUST generate section index pages automatically from configuration  
**FR-3**: The system MUST support multi-section content access (same item in multiple sections)  
**FR-4**: The system MUST filter content by category for section-specific views  
**FR-5**: The system MUST provide navigation between sections and collections  
**FR-6**: The system MUST generate RSS feeds per section  
**FR-7**: The system SHOULD use clean, SEO-friendly URL structure (redirects from old URLs acceptable)  
**FR-8**: The system MUST implement infinite scroll pagination (20 items per batch)  
**FR-9**: The system MUST prefetch next batch at 80% scroll position  
**FR-10**: The system MUST preserve pagination state in URL parameters  

### Non-Functional Requirements

**NFR-1**: Section changes MUST require only configuration updates, not code changes  
**NFR-2**: Page generation MUST complete in < 5 seconds for all sections  
**NFR-3**: Navigation MUST be keyboard accessible  
**NFR-4**: Section pages MUST be mobile-responsive  
**NFR-5**: Pagination API requests MUST complete in < 200ms (p95)  
**NFR-6**: Cache hit rate MUST exceed 90% for repeated requests  
**NFR-7**: System MUST support 1000+ concurrent users without degradation  

## Configuration Schema

### `sections.json` Structure

```json
{
  "section-key": {
    "title": "Section Display Title",
    "description": "Section description for meta tags and cards",
    "url": "/section-url",
    "section": "section-key",
    "image": "/assets/section-backgrounds/image.jpg",
    "category": "Category Name",
    "collections": [
      {
        "title": "Collection Display Title",
        "url": "/section/collection.html",
        "collection": "collection-name",
        "description": "Collection description"
      }
    ]
  }
}
```

### Field Definitions

| Field | Type | Required | Description |
| ------- | ------ | ---------- | ------------- |
| `section-key` | string | Yes | Unique identifier for the section (used in URLs) |
| `title` | string | Yes | Display name for the section |
| `description` | string | Yes | SEO and card description |
| `url` | string | Yes | Absolute URL path for the section |
| `section` | string | Yes | Section key (matches parent key) |
| `image` | string | Yes | Path to section background image |
| `category` | string | Yes | Filter value for content categorization |
| `collections` | array | Yes | List of collections in this section |
| `collections[].title` | string | Yes | Display name for the collection |
| `collections[].url` | string | Yes | Absolute URL path for the collection page |
| `collections[].collection` | string | Yes | Collection identifier (e.g., "news", "videos") |
| `collections[].description` | string | Yes | Collection description |

## Use Cases

### UC-1: Display Home Page Section Grid

**Actor**: User  
**Precondition**: `sections.json` is loaded  
**Trigger**: User visits home page (`/`)  

**Flow**:

1. System reads `sections.json`
2. System extracts all section objects
3. System renders section cards in grid layout
4. Each card displays: title, description, background image
5. Each card links to section URL
6. System renders latest roundups below section grid

**Postcondition**: User sees all available sections

### UC-2: Navigate to Section Index

**Actor**: User  
**Precondition**: User is on home page  
**Trigger**: User clicks section card  

**Flow**:

1. User clicks "GitHub Copilot" section card
2. System navigates to `/github-copilot`
3. System loads section configuration from `sections.json`
4. System loads all collections for this section
5. System filters content by `category: "GitHub Copilot"`
6. System displays section header, collections tabs, and filtered content
7. System shows first collection by default (e.g., "News")

**Postcondition**: User sees GitHub Copilot section with all collections

### UC-3: Switch Between Collections

**Actor**: User  
**Precondition**: User is on section index page  
**Trigger**: User clicks collection tab  

**Flow**:

1. User clicks "Videos" tab in GitHub Copilot section
2. System navigates to `/github-copilot/videos.html`
3. System loads video collection items filtered by `category: "GitHub Copilot"`
4. System applies default date filter (Last 7 days)
5. System displays filtered videos with filter controls
6. System updates URL and active tab state

**Postcondition**: User sees videos for GitHub Copilot section

### UC-4: Add New Section via Configuration

**Actor**: Developer  
**Precondition**: `sections.json` exists  
**Trigger**: Need to add new section (e.g., "Web Development")  

**Flow**:

1. Developer opens `sections.json`
2. Developer adds new section object:

   ```json
   "web-dev": {
     "title": "Web Development",
     "description": "Modern web development resources",
     "url": "/web-dev",
     "section": "web-dev",
     "image": "/assets/section-backgrounds/web-dev.jpg",
     "category": "Web Development",
     "collections": [
       {
         "title": "News",
         "url": "/web-dev/news.html",
         "collection": "news",
         "description": "Web development news"
       }
     ]
   }
   ```

3. Developer adds background image to `/assets/section-backgrounds/`
4. Developer tags content items with `categories: ["Web Development"]`
5. System automatically generates section pages on next build
6. New section appears in home page grid

**Postcondition**: Web Development section is live with no code changes

## Section Categories

### Current Sections

| Section Key | Title | Category | URL |
| ------------- | ------- | ---------- | ----- |
| `all` | Everything | All | `/all` |
| `github-copilot` | GitHub Copilot | GitHub Copilot | `/github-copilot` |
| `ai` | AI | AI | `/ai` |
| `ml` | ML | ML | `/ml` |
| `azure` | Azure | Azure | `/azure` |
| `dotnet` | .NET/Coding | Coding | `/dotnet` |
| `devops` | DevOps | DevOps | `/devops` |
| `security` | Security | Security | `/security` |

### Special Sections

**"Everything" Section**:

- Category: "All" (no filtering)
- Shows ALL content regardless of category
- Includes "Roundups" collection (exclusive to this section)
- Acts as global content view

## Collection Types

### Standard Collections

| Collection | Description | Output Pages |
| ------------ | ------------- | -------------- |
| `news` | Official product updates | Yes |
| `blogs` | Blogs | Yes |
| `videos` | Video content and tutorials | Yes |
| `community` | Microsoft Tech Community posts | Yes |

### Special Collections

**Roundups**:

- Only appears in "Everything" section
- Weekly curated content summaries
- Has dedicated RSS feed (`/roundups/feed.xml`)
- Not filtered by category

## Content Filtering Logic

### Category Filtering

**Rule**: Content items are displayed in a section if ANY of their categories match the section's category.

**Example**:

```yaml
# Content item frontmatter

categories:
  - GitHub Copilot
  - AI
```

This item appears in:

- GitHub Copilot section (category match)
- AI section (category match)
- Everything section (no filter)

Does NOT appear in:

- Azure section (no category match)
- .NET section (no category match)

### Multi-Section Access

**Principle**: Same content item accessible from multiple section contexts.

**URLs**:

- `/2025-01-02-chat-in-ide.html` (canonical)
- `/github-copilot/videos/chat-in-ide.html` (section context)
- `/ai/videos/chat-in-ide.html` (section context)

All URLs show the same content, but:

- Navigation reflects current section context
- Related content filtered by section
- Breadcrumbs show section path
- Canonical URL specified in metadata

## Page Generation

### Auto-Generated Pages

The system MUST automatically generate these pages from `sections.json`:

1. **Section Index Pages**: `/{section}/index.html`
   - Layout: Section header + collection tabs + content list
   - Data: Filtered by section category
   - Navigation: Links to collections

2. **Collection Pages**: `/{section}/{collection}.html`
   - Layout: Section header + single collection view
   - Data: Filtered by section category AND collection type
   - Navigation: Collection tabs with active state

### Manual Pages

These pages are NOT generated (custom content):

- Custom section pages (e.g., `/github-copilot/features.html`)
- About page (`/about.html`)
- Home page (`/index.html`)

## Navigation Structure

### Primary Navigation (Header)

```text
[Logo/Home] [Section Menu ▼] [RSS] [About]
```

**Section Menu Dropdown**:

- Everything
- GitHub Copilot
- AI
- ML
- Azure
- .NET/Coding
- DevOps
- Security

### Section Sub-Navigation

```text
[Section Header]
[News] [Blogs] [Videos] [Community]
     ↑ Active tab highlighted
```

**Rules**:

- Horizontal tab list for collections
- Active collection highlighted
- Sticky header on scroll (mobile)
- Keyboard navigable (arrow keys)

### Footer Navigation

- All sections linked
- Collection links within "Everything"
- RSS feed link
- About link

## RSS Feed Structure

### Feed URLs

| Feed Type | URL | Content |
| ----------- | ----- | --------- |
| Everything | `/feed.xml` | All content |
| Section | `/{section}/feed.xml` | Section-filtered content |
| Roundups | `/roundups/feed.xml` | Roundups only |

### Feed Generation

**Rules**:

1. Read `sections.json` to determine section categories
2. Filter content items by category
3. Generate RSS 2.0 XML with:
   - Channel title: `{Section Title} | Tech Hub`
   - Channel description: Section description
   - Items: Filtered by category, sorted by date (newest first)
   - Item categories: All categories for the item

## Acceptance Criteria

**AC-1**: Given `sections.json` with 8 sections, when home page loads, then 8 section cards are displayed  
**AC-2**: Given a section with 4 collections, when section page loads, then 4 collection tabs are shown  
**AC-3**: Given content tagged "GitHub Copilot", when visiting `/github-copilot`, then content is visible  
**AC-4**: Given content tagged "AI" and "GitHub Copilot", when visiting `/ai` OR `/github-copilot`, then content appears in both  
**AC-5**: Given a new section added to `sections.json`, when site rebuilds, then new section appears automatically  
**AC-6**: Given section RSS feed URL, when fetched, then only that section's content is included  
**AC-7**: Given content list with 100+ items, when user scrolls to 80%, then next 20 items load automatically  
**AC-8**: Given user at page 3 of results, when sharing URL, then recipient sees same page state  
**AC-9**: Given user filtering by tag, when infinite scroll loads more items, then new items respect current filter  

## Technical Approach

### Data Access Layer

**Repository Interface**:

```csharp
public interface ISectionRepository
{
    Task<IEnumerable<Section>> GetAllSectionsAsync(CancellationToken ct);
    Task<Section?> GetSectionByUrlAsync(string url, CancellationToken ct);
    Task<Section?> GetSectionByKeyAsync(string key, CancellationToken ct);
}
```

### Domain Models

```csharp
public record Section(
    string Key,
    string Title,
    string Description,
    string Url,
    string Image,
    string Category,
    IEnumerable<CollectionInfo> Collections
);

public record CollectionInfo(
    string Title,
    string Url,
    string Collection,
    string Description
);
```

### API Endpoints

```csharp
// GET /api/sections
app.MapGet("/api/sections", async (ISectionRepository repo) =>
    Results.Ok(await repo.GetAllSectionsAsync()));

// GET /api/sections/{url}
app.MapGet("/api/sections/{url}", async (string url, ISectionRepository repo) =>
    await repo.GetSectionByUrlAsync(url) is Section section
        ? Results.Ok(section)
        : Results.NotFound());
```

### Blazor Components

**SectionCard.razor**:

```razor
<div class="section-card" style="background-image: url('@Section.Image')">
    <a href="@Section.Url">
        <h3>@Section.Title</h3>
        <p>@Section.Description</p>
    </a>
</div>

@code {
    [Parameter, EditorRequired]
    public Section Section { get; set; } = null!;
}
```

**SectionIndex.razor**:

```razor
@page "/{sectionKey}"

<PageTitle>@Section?.Title | Tech Hub</PageTitle>

<PageHeader Section="@Section" />

<nav class="collection-tabs">
    @foreach (var collection in Section?.Collections ?? [])
    {
        <a href="@collection.Url" class="@(IsActive(collection) ? "active" : "")">
            @collection.Title
        </a>
    }
</nav>

<ContentList Items="@FilteredItems" Filters="@Filters" />

@code {
    [Parameter]
    public string SectionKey { get; set; } = string.Empty;
    
    private Section? Section { get; set; }
    private IEnumerable<ContentItem> FilteredItems { get; set; } = [];
    
    // Load data logic...
}
```

## Testing Strategy

### Unit Tests

- Test section repository data access
- Test content filtering by category
- Test multi-category matching logic
- Test section/collection URL generation

### Component Tests (bUnit)

- Test SectionCard component rendering
- Test SectionIndex component with mock data
- Test collection tab navigation
- Test responsive layout

### Integration Tests

- Test API endpoints return correct sections
- Test section pages render correctly
- Test RSS feeds filter by category
- Test navigation between sections

### E2E Tests (Playwright)

- Test home page displays all sections
- Test clicking section card navigates correctly
- Test collection tab switching
- Test content appears in multiple sections
- Test adding new section via config

## Edge Cases

**EC-1**: Section with no collections → Display message "No collections available"  
**EC-2**: Collection with no items → Display "No content available"  
**EC-3**: Duplicate section keys → Log error, use first occurrence  
**EC-4**: Invalid category in content → Item not shown in any section (except "Everything")  
**EC-5**: Missing background image → Use default placeholder  
**EC-6**: Section URL conflicts with existing route → Log error, prioritize existing route  

## Performance Considerations

**Caching Strategy**:

- Cache `sections.json` in memory (singleton pattern)
- Use Output Caching for section/collection pages (5 minute TTL)
- Use Redis distributed cache for paginated content (15 minute TTL)
- Lazy-load and cache section background images
- Preload critical sections (Everything, GitHub Copilot)

**API Performance**:

- Pagination requests MUST complete in < 200ms (p95)
- Initial 20 items rendered in < 500ms
- Next batch loads within 300ms of scroll trigger
- Cache hit rate MUST exceed 90%

**Scalability**:

- Pagination API MUST support horizontal scaling
- Use stateless API design for load balancing
- Cache invalidation strategy for content updates
- Monitor cache hit rates and adjust TTL accordingly
- Generate RSS feeds asynchronously

## Migration Notes

**From Jekyll**:

- Replace `_plugins/section_pages_generator.rb` with C# page generation
- Convert Liquid section templates to Blazor components (SSR + WASM)
- Ensure `sections.json` structure remains identical
- URLs can be modernized (set up redirects from old paths if needed)
- **REMOVED**: Jekyll "20 + same-day" limiting rule - use infinite scroll instead
- **ADDED**: Infinite scroll pagination with prefetching
- **ADDED**: Output caching and Redis distributed cache
- **ADDED**: Client-side state management with URL parameters

## Open Questions

1. Should sections be stored in a database instead of JSON file?
2. Should we support dynamic section ordering?
3. Should we allow nested sections (sub-sections)?
4. How to handle section deprecation/archival?

## References

- [Filtering System Spec](/specs/019-filtering-system/spec.md)
- [RSS Feeds Spec](/specs/021-rss-feeds/spec.md)
