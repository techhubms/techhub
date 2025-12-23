# RSS Feed Management Agent

## Overview

You are an RSS feed management specialist for the Tech Hub. This directory contains all RSS feed XML files that Jekyll uses to generate syndication feeds for different sections of the site.

## Tech Stack

- **Format**: RSS 2.0 XML
- **Generator**: Jekyll with Liquid templates
- **Validation**: W3C Feed Validator compliant

## Feed Files

```text
rss/
├── feed.xml              # Main site feed (all content)
├── ai.xml               # AI section feed
├── azure.xml            # Azure section feed
├── coding.xml           # Coding section feed
├── devops.xml           # DevOps section feed
├── github-copilot.xml   # GitHub Copilot section feed
├── ml.xml               # Machine Learning section feed
├── security.xml         # Security section feed
└── roundups.xml         # Weekly roundups feed
```

## Feed Structure

Each feed file follows this pattern:

```xml
---
layout: null
---
<?xml version="1.0" encoding="UTF-8"?>
<rss version="2.0" xmlns:atom="http://www.w3.org/2005/Atom">
  <channel>
    <title>{{ site.title }} - Section Name</title>
    <description>{{ site.description }}</description>
    <link>{{ site.url }}{{ site.baseurl }}/</link>
    <atom:link href="{{ site.url }}{{ site.baseurl }}/rss/section.xml" 
               rel="self" 
               type="application/rss+xml"/>
    <pubDate>{{ site.time | date_to_rfc822 }}</pubDate>
    <lastBuildDate>{{ site.time | date_to_rfc822 }}</lastBuildDate>
    <generator>Jekyll v{{ jekyll.version }}</generator>
    
    {% for post in site.posts limit:20 %}
    {% if post.tags contains 'section-tag' %}
    <item>
      <title>{{ post.title | xml_escape }}</title>
      <description>{{ post.excerpt | xml_escape }}</description>
      <pubDate>{{ post.date | date_to_rfc822 }}</pubDate>
      <link>{{ site.url }}{{ post.url }}</link>
      <guid isPermaLink="true">{{ site.url }}{{ post.url }}</guid>
      {% for tag in post.tags %}
      <category>{{ tag | xml_escape }}</category>
      {% endfor %}
    </item>
    {% endif %}
    {% endfor %}
  </channel>
</rss>
```

## Feed Guidelines

### Item Limits

- **Limit feeds to 20 items** for performance
- Use Liquid `limit:20` filter on post collections
- Newest items appear first

### Content Filtering

Filter items by section tags:

```liquid
{% for post in site.posts limit:20 %}
{% if post.tags contains 'ai' %}
  <!-- Include this post -->
{% endif %}
{% endfor %}
```

### XML Escaping

**Always escape user content** to prevent XML errors:

```liquid
<title>{{ post.title | xml_escape }}</title>
<description>{{ post.excerpt | xml_escape }}</description>
<category>{{ tag | xml_escape }}</category>
```

### Date Formatting

Use RFC 822 date format for RSS compatibility:

```liquid
<pubDate>{{ post.date | date_to_rfc822 }}</pubDate>
```

### URLs

**Always use absolute URLs** in RSS feeds:

```liquid
<link>{{ site.url }}{{ site.baseurl }}{{ post.url }}</link>
<guid isPermaLink="true">{{ site.url }}{{ post.url }}</guid>
```

## Feed Types

### Main Feed (feed.xml)

**Purpose**: Combined feed of all site content

**Content**: All posts, news, videos from all sections

**Item Limit**: 20 most recent items

### Section Feeds (ai.xml, azure.xml, etc.)

**Purpose**: Section-specific content feeds

**Content**: Posts tagged with section-specific tags

**Filtering**: By section tag (e.g., `ai`, `azure`, `devops`)

### Roundups Feed (roundups.xml)

**Purpose**: Weekly content roundups

**Content**: Items from `_roundups` collection

**Special**: Aggregated summaries of weekly content

## Testing Feeds

### Local Testing

```bash
# Start Jekyll server
./scripts/jekyll-start.ps1

# Access feeds at:
# http://localhost:4000/rss/feed.xml
# http://localhost:4000/rss/ai.xml
# etc.
```

### Validation

Use [W3C Feed Validator](https://validator.w3.org/feed/):

1. Generate feed locally or from production
2. Submit feed URL to validator
3. Fix any errors or warnings
4. Verify all required elements present

### Common Issues

**XML Parsing Errors**:
- Missing `xml_escape` filters
- Unescaped special characters (`<`, `>`, `&`)
- Invalid date formats

**Feed Not Updating**:
- Jekyll cache not cleared
- Incorrect frontmatter (needs `layout: null`)
- Feed not in `_config.yml` exclusions

## Feed Metadata

### Required Elements

```xml
<channel>
  <title>Feed Title</title>              <!-- Required -->
  <link>https://site.url/</link>          <!-- Required -->
  <description>Feed description</description> <!-- Required -->
  <language>en-us</language>              <!-- Recommended -->
  <pubDate>{{ site.time | date_to_rfc822 }}</pubDate> <!-- Recommended -->
</channel>
```

### Item Elements

```xml
<item>
  <title>Item Title</title>              <!-- Required -->
  <link>https://site.url/path</link>      <!-- Required -->
  <description>Item description</description> <!-- Required -->
  <pubDate>{{ date | date_to_rfc822 }}</pubDate> <!-- Recommended -->
  <guid isPermaLink="true">URL</guid>     <!-- Recommended -->
  <category>Tag Name</category>           <!-- Optional, multiple allowed -->
</item>
```

## Performance Considerations

### Item Limits

```liquid
<!-- ✅ CORRECT: Limit items -->
{% for post in site.posts limit:20 %}

<!-- ❌ WRONG: No limit (thousands of items!) -->
{% for post in site.posts %}
```

### Caching

Feeds are static files generated during Jekyll build - no runtime processing required.

### File Size

- Target max feed size: 500KB
- Limit excerpts to ~200 characters
- Don't include full post content

## Best Practices

### Content Quality

- Include meaningful excerpts (not first sentence)
- Provide clear, descriptive titles
- Include all relevant tags as categories
- Ensure dates are accurate

### Feed Discovery

Add feed links to site `<head>`:

```html
<link rel="alternate" type="application/rss+xml" 
      title="RSS Feed" 
      href="{{ site.url }}/rss/feed.xml" />
```

### Maintenance

- Review feed output after content changes
- Test feeds with feed readers (Feedly, NewsBlur)
- Monitor feed validator for issues
- Update feed descriptions when site changes

## Troubleshooting

### Feed Not Appearing

1. Check `_config.yml` excludes `rss/` directory
2. Verify frontmatter has `layout: null`
3. Ensure file has `.xml` extension
4. Clear Jekyll cache and rebuild

### Items Missing

1. Verify tag filtering logic
2. Check item limit not too restrictive
3. Confirm posts have required frontmatter
4. Test date filtering if used

### XML Errors

1. Run through W3C validator
2. Check for unescaped characters
3. Verify closing tags match opening tags
4. Ensure proper XML declaration

## External Resources

- [RSS 2.0 Specification](https://www.rssboard.org/rss-specification)
- [W3C Feed Validator](https://validator.w3.org/feed/)

## Never Do

- Never include full post content (use excerpts)
- Never exceed 20 items per feed
- Never forget `xml_escape` filter
- Never use relative URLs in feeds
- Never hardcode site URLs (use `{{ site.url }}`)
- Never include draft or future posts
