# RSS Feed Generation Specification

> **Feature**: RSS/Atom feed generation with optimal feed URLs and modern caching

## Overview

The RSS feed system generates RSS 2.0 and Atom 1.0 feeds for all sections and collections with clean URLs. Feed generation uses aggressive caching strategies (Output Caching + Redis) for optimal performance. Where practical, main feed URLs should be preserved from Jekyll site (redirects acceptable).

## Requirements

### Functional Requirements

**FR-1**: The system MUST generate RSS 2.0 and Atom 1.0 compliant feeds  
**FR-2**: The system SHOULD preserve main feed URLs where practical (redirects acceptable if needed)  
**FR-3**: The system MUST filter feed content by section category  
**FR-4**: The system MUST include proper feed metadata (title, description, language, last build date)  
**FR-5**: The system MUST convert Markdown content to HTML for feed items  
**FR-6**: The system MUST use absolute URLs for all links and images  
**FR-7**: The system MUST respect timezone settings (Europe/Brussels)  
**FR-8**: The system MUST exclude future-dated content (beyond current date)  
**FR-9**: The system MUST properly escape HTML content in feed items  
**FR-10**: The system MUST limit feeds to most recent 100 items for performance  

### Non-Functional Requirements

**NFR-1**: Feed generation MUST complete in < 50ms per feed (p95, cached)  
**NFR-2**: Generated feeds MUST validate against RSS 2.0 and Atom 1.0 specifications  
**NFR-3**: Feed responses MUST include proper Content-Type headers  
**NFR-4**: Feeds MUST use Output Caching (15 min TTL) + Redis distributed cache  
**NFR-5**: Feeds MUST support HTTP caching with ETag/Last-Modified headers  

## Use Cases

### UC-1: Generate Section Feed

**Actor**: Content System  
**Precondition**: Content items exist with valid frontmatter  
**Trigger**: Feed URL request (e.g., `/ai/feed.xml`)  

**Flow**:

1. System receives feed request for section (e.g., `/ai/feed.xml`)
2. System checks Output Cache for cached feed (15 min TTL)
3. If cache miss, system loads sections.json to determine section category
4. System queries all content items with matching category
5. System filters items by publish date (exclude future dates)
6. System sorts items by date descending, limits to 100 items
7. System converts Markdown to HTML for each item
8. System converts relative URLs to absolute URLs
9. System generates RSS 2.0 XML with proper namespaces
10. System caches generated feed (Output Cache + Redis)
11. System sets Content-Type: application/rss+xml header
12. System returns generated feed

**Postcondition**: Valid RSS feed returned to client

### UC-2: Generate Everything Feed

**Actor**: Content System  
**Precondition**: Content items exist across all sections  
**Trigger**: Request to `/feed.xml`  

**Flow**:

1. System receives request for main feed
2. System checks Output Cache for cached feed
3. If cache miss, system queries all content items (no category filter)
4. System filters items by publish date
5. System sorts by date descending, limits to 100 items
6. System converts content and generates feed
7. System caches generated feed
8. System returns feed

**Postcondition**: Feed with content from all sections returned

### UC-3: Generate Roundups Feed

**Actor**: Content System  
**Precondition**: Roundup collection items exist  
**Trigger**: Request to `/roundups/feed.xml`  

**Flow**:

1. System receives request for roundups feed
2. System checks Output Cache for cached feed
3. If cache miss, system queries all roundup collection items
4. System filters by publish date, sorts descending, limits to 100 items
5. System generates feed with roundup-specific metadata
6. System caches generated feed
7. System returns feed

**Postcondition**: Roundups-only feed returned

### UC-4: Handle Image URLs in Feed Content

**Actor**: Content System  
**Precondition**: Content contains relative image URLs  
**Trigger**: Feed generation process  

**Flow**:

1. System parses HTML content from Markdown
2. System identifies all `<img>` tags
3. System converts relative `src` attributes to absolute URLs
4. System ensures images use `https://tech.hub.ms` domain
5. System includes images in feed item content

**Postcondition**: All image URLs are absolute and accessible

## Acceptance Criteria

### AC-1: Feed URL Functionality

**Given** the site has content in AI section  
**When** a user requests `/ai/feed.xml` from .NET site  
**Then** the feed is returned with valid RSS 2.0 format  
**And** the feed validates as RSS 2.0  
**And** the Content-Type header is `application/rss+xml; charset=utf-8`  

### AC-2: Feed Content Accuracy

**Given** 25 news items exist in AI section  
**When** the AI feed is generated  
**Then** up to 100 most recent items are included  
**And** items are sorted by publication date (newest first)  
**And** items are sorted newest first  
**And** all items have title, link, description, pubDate  

### AC-3: Markdown to HTML Conversion

**Given** a content item with Markdown including code blocks  
**When** the feed is generated  
**Then** Markdown is converted to HTML  
**And** code blocks have syntax highlighting preserved  
**And** HTML is properly escaped for XML  

### AC-4: Absolute URL Conversion

**Given** content with relative image `/assets/images/photo.jpg`  
**When** feed is generated  
**Then** image URL becomes `https://tech.hub.ms/assets/images/photo.jpg`  
**And** all content links are absolute URLs  

### AC-5: YouTube Embed Handling

**Given** content with `{% youtube VIDEO_ID %}` tag  
**When** feed is generated  
**Then** YouTube tag is converted to link `https://www.youtube.com/watch?v=VIDEO_ID`  
**Or** embedded player HTML with absolute URL  

### AC-6: Timezone Handling

**Given** current time is 2026-01-01 23:00 in Europe/Brussels  
**When** feed is generated  
**Then** items with date 2026-01-02 00:00 UTC are excluded (future in Brussels)  
**And** items with date 2026-01-01 23:00 UTC are included (past in Brussels)  

### AC-7: Feed Metadata

**Given** AI section feed  
**When** feed is generated  
**Then** feed title is "Tech Hub - AI"  
**And** feed description matches section description from sections.json  
**And** feed link is `https://tech.hub.ms/ai/`  
**And** feed language is `en-us`  

### AC-8: Caching Headers

**Given** a feed request  
**When** feed is returned  
**Then** response includes `ETag` header  
**And** response includes `Last-Modified` header  
**And** subsequent request with matching `If-None-Match` returns 304 Not Modified  

## Technical Approach

### Feed URLs

**Section Feeds** (filter by category):

- `/feed.xml` - All content (no category filter)
- `/ai/feed.xml` - AI category
- `/github-copilot/feed.xml` - GitHub Copilot category
- `/ml/feed.xml` - ML category
- `/azure/feed.xml` - Azure category
- `/coding/feed.xml` - Coding (.NET) category
- `/devops/feed.xml` - DevOps category
- `/security/feed.xml` - Security category

**Collection Feeds**:

- `/roundups/feed.xml` - Roundups collection only

### Feed Format

**RSS 2.0 Structure**:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<rss version="2.0" 
     xmlns:atom="http://www.w3.org/2005/Atom"
     xmlns:dc="http://purl.org/dc/elements/1.1/">
  <channel>
    <title>Tech Hub - [Section Name]</title>
    <description>[Section Description]</description>
    <link>https://tech.hub.ms/[section]/</link>
    <atom:link href="https://tech.hub.ms/[section]/feed.xml" rel="self" type="application/rss+xml"/>
    <language>en-us</language>
    <lastBuildDate>[RFC 2822 Date]</lastBuildDate>
    <generator>Tech Hub .NET</generator>
    
    <item>
      <title>[Item Title]</title>
      <link>https://tech.hub.ms/[item-url]</link>
      <description><![CDATA[[HTML Content]]]></description>
      <pubDate>[RFC 2822 Date]</pubDate>
      <dc:creator>[Author]</dc:creator>
      <category>[Category]</category>
      <category>[Tag 1]</category>
      <category>[Tag 2]</category>
      <guid isPermaLink="true">https://tech.hub.ms/[item-url]</guid>
    </item>
  </channel>
</rss>
```

### Content Processing Pipeline

1. **Check Cache**: Check Output Cache + Redis for cached feed
2. **Query Content**: If cache miss, use repository to fetch items by category
3. **Filter by Date**: Apply timezone-aware date filter (Europe/Brussels)
4. **Sort & Limit**: Order by date descending, limit to 100 items
5. **Convert Markdown**: Use Markdig to convert to HTML
6. **Process URLs**: Convert all relative URLs to absolute
7. **Escape Content**: Wrap in CDATA or properly escape HTML entities
8. **Generate XML**: Use SyndicationFeed or custom generator
9. **Cache**: Store generated XML (Output Cache 15 min + Redis)
10. **Return**: Serve with proper headers (ETag, Last-Modified)

### Implementation Notes

**Technology**:

- Use `System.ServiceModel.Syndication.SyndicationFeed` for RSS generation
- Or use lightweight library like `WilderMinds.RssSyndication`
- Integrate with Markdig pipeline for Markdown conversion
- Use same date handling as content rendering (Europe/Brussels timezone)

**Caching Strategy**:

- Cache generated feed XML in memory
- Invalidate cache when content changes
- Use ETag based on content hash
- Use Last-Modified based on newest item date

**URL Resolution**:

- Base URL: `https://tech.hub.ms`
- Use configuration for base URL (environment-specific)
- Resolve all relative URLs during content processing
- Apply to `<img src>`, `<a href>`, and other references

## Testing Strategy

### Unit Tests

**Test Cases**:

- Feed generation with empty content list
- Feed generation with single item
- Feed generation with 25 items (verify limiting)
- Markdown to HTML conversion in feed
- Relative URL to absolute URL conversion
- YouTube tag conversion
- Timezone-aware date filtering
- Category filtering for section feeds
- Feed metadata population from sections.json
- HTML escaping in CDATA sections

**Mocking**:

- Mock content repository
- Mock Markdig pipeline
- Mock system clock for date tests

### Integration Tests

**Test Cases**:

- Generate feed for each section
- Validate XML against RSS 2.0 schema
- Verify Content-Type headers
- Test caching with ETag/Last-Modified
- Test 304 Not Modified responses
- Verify absolute URLs in generated feeds
- Test feed with various content types (news, videos, blogs)

### E2E Tests

**Test Cases**:

- Request `/feed.xml` and verify valid RSS
- Request `/ai/feed.xml` and verify category filtering
- Verify feed reader can parse generated feeds
- Test feed subscription in feed readers (Feedly, RSS readers)
- Verify main feed URLs work correctly (301 redirects acceptable if structure changed)

## Edge Cases

1. **Empty Section**: What if section has no content?
   - Return valid feed with empty `<channel>` (no items)
   - Include feed metadata (title, description, link)

2. **All Future Dates**: What if all content is future-dated?
   - Return empty feed (no items)
   - Feed metadata still present

3. **Large Images**: What if content has very large embedded images?
   - Include images as-is (feed readers handle)
   - Consider max content size per item (if needed)

4. **Invalid Markdown**: What if content has malformed Markdown?
   - Log error but continue feed generation
   - Include raw content or skip item (configurable)

5. **Missing Frontmatter**: What if item lacks required metadata?
   - Skip item (log warning)
   - Require title and date minimum

6. **Special Characters**: What if title/description has XML special chars?
   - Properly escape: `&lt;`, `&gt;`, `&amp;`, `&quot;`
   - Use CDATA for content when possible

7. **Very Long Content**: What if item content exceeds reasonable size?
   - Include full content (no truncation for feeds)
   - Feed readers handle long content

8. **Multiple Categories**: What if item has multiple categories?
   - Item appears in multiple section feeds
   - All categories included as `<category>` tags

## Performance Considerations

**Feed Generation**:

- Cache generated XML (invalidate on content change)
- Pre-generate feeds during build process
- Use streaming XML writer for large feeds
- Limit feed size to 100 items maximum

**Content Processing**:

- Reuse Markdig pipeline instance
- Process items in parallel when possible
- Cache Markdown→HTML conversion results
- Minimize allocations during URL resolution

**Response Time**:

- Target: < 50ms for cached feeds
- Target: < 500ms for uncached feeds
- Use compression (gzip) for XML responses
- Implement HTTP caching (ETag, Last-Modified)

## Migration Notes

**Current Jekyll Implementation**:

- Main feed: `jekyll-feed` plugin generates `/feed.xml`
- Custom feeds: Liquid templates in `rss/` directory
- Feed format: Atom 1.0 for main feed, RSS 2.0 for custom feeds
- Content: Full HTML content in feed items

**Migration Path**:

1. Generate feeds using `System.ServiceModel.Syndication`
2. Use Output Caching (15 min TTL) + Redis distributed cache
3. Validate generated feeds against RSS 2.0/Atom 1.0 specifications
4. Test with feed readers to ensure compatibility
5. Verify subscriber continuity (maintain feed URLs)
6. **REMOVED**: Jekyll "20 + same-day" limiting rule - use 100 item limit instead
7. **ADDED**: Aggressive caching strategy for performance

**URL Preservation**:

Main feed URLs SHOULD be preserved where practical (redirects acceptable):

- `/feed.xml` → Recommended to preserve (everything feed)
- `/ai/feed.xml` → Recommended to preserve (section feed)
- `/github-copilot/feed.xml` → Recommended to preserve (section feed)
- `/roundups/feed.xml` → Recommended to preserve (collection feed)

If URL structure changes, use 301 redirects to maintain subscriber continuity.  
**Note**: Exact URL matching is NOT mandatory - optimal structure takes priority.

**Content Compatibility**:

- Markdown→HTML conversion using Markdig (no Rouge matching required)
- Modern syntax highlighting (not tied to Jekyll Rouge output)
- Keep same excerpt generation (200 words + `<!--excerpt_end-->`)
- Use same timezone handling (Europe/Brussels)

## Open Questions

None - specification is complete based on existing Jekyll implementation documentation.
