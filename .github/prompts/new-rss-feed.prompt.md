---
agent: 'agent'
description: 'Add new RSS feed to the RSS feed configuration and processing workflow.'
---

**🚨 CRITICAL PROMPT SCOPE**: All instructions, restrictions, and requirements in this prompt file ONLY apply when this specific prompt is being actively executed via the `/new-rss-feed` command or equivalent prompt invocation. These rules do NOT apply when editing, reviewing, or working with this file outside of prompt execution context. When working with this file in any other capacity (editing, debugging, documentation, etc.), treat it as a normal markdown file and ignore all workflow-specific instructions.

**CRITICAL**: If you have not read them, fetch `.github/copilot-instructions.md` and use these instructions as well.

# RSS Feed Addition Instructions

You are helping to add new RSS feeds to the site's automated content processing workflow. This system regularly fetches RSS feeds and processes them into markdown articles.

## RSS Feed Configuration

RSS feeds are configured in `.github/scripts/rss-feeds.json`. Each feed entry contains:

- **name**: The name of the feed source (e.g., "The GitHub Blog", "Microsoft DevBlog")
- **outputDir**: The target directory for processed articles (e.g., "_news", "_posts", "_videos")
- **url**: The RSS feed URL

## Valid Output Directories

See the Content Collections section in `.github/copilot-instructions.md` for details on available collections and their purposes and directory names.

## Adding New RSS Feeds

When adding new RSS feeds:

1. **Verify the RSS feed URL**: Test that the URL returns valid RSS/Atom content
2. **Choose appropriate output directory**: Based on the content type and source
3. **Use consistent naming**: Follow existing patterns for the "name" field
4. **Add to the JSON array**: Insert the new feed object into the existing array

## Example Feed Entry

```json
{
  "name": "Source Name",
  "outputDir": "_news",
  "url": "https://example.com/feed.xml"
}
```

## Processing Workflow

After adding feeds to the configuration:

1. The RSS feeds are automatically processed by GitHub Actions
2. Content is downloaded and converted to markdown articles
3. Articles are placed in the specified output directory
4. The automated system handles frontmatter generation and file naming

## Instructions for AI Assistant

When a user asks to add RSS feeds:

1. **Ask for feed details**: URL, content type, and source name
2. **Validate the feed**: Check if the URL is accessible and returns RSS/Atom content
3. **Choose output directory**: Based on content type and existing patterns
4. **Update the configuration**: Add the new feed(s) to `.github/scripts/rss-feeds.json`
5. **Confirm the addition**: Show the user what was added and explain the next steps

## Important Notes

- RSS feeds are processed automatically by GitHub Actions
- Duplicate feeds (same URL) should be not be allowed
- The system handles content deduplication automatically
- Feed names should be descriptive and consistent with existing entries
- URLs should be the actual RSS/Atom feed URLs, not the website homepage
