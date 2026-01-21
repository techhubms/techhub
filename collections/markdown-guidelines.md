# Instructions for AI models when generating markdown files

**CRITICAL**: Always follow the writing style guidelines in [writing-style-guidelines.md](writing-style-guidelines.md) for tone, language, and content standards.

**CRITICAL**: For complete frontmatter field definitions and examples, see [frontmatter-schema.md](frontmatter-schema.md).

## Automated Processing

The repository includes automated scripts that validate and repair markdown formatting:

- **markdownlint-cli2**: Industry-standard markdown linting and auto-fixing tool (headings, lists, whitespace, code blocks)
- **fix-markdown-files.ps1**: Batch processes files using markdownlint-cli2 to fix markdown formatting issues

**NOTE**: Frontmatter repair is NOT needed - templates already generate correct .NET format.

**CRITICAL**: Do not automatically run processing scripts!

While following these instructions carefully is important, minor formatting issues may be automatically corrected by automated tooling during processing.

## Quick Reference

**For detailed frontmatter field definitions**, see [frontmatter-schema.md](frontmatter-schema.md).

This document provides high-level guidance for content creation. For complete field-by-field documentation:

- **Universal fields** (all content): [frontmatter-schema.md#universal-fields-all-content](frontmatter-schema.md#universal-fields-all-content)
- **Collection-specific fields** (videos, features, roundups): [frontmatter-schema.md#collection-specific-fields](frontmatter-schema.md#collection-specific-fields)
- **Deprecated fields** (no longer used): [frontmatter-schema.md#deprecatedremoved-fields](frontmatter-schema.md#deprecatedremoved-fields)

## Content Structure Rules

- Never assume the type of content. Always ask the user if not provided.
- Do not create tags or sections or categories unless the provided content is about these topics.
- Do not mix content between files. Each file must be based only on the content provided with the instructions.
- If you cannot determine any required field from the URL or content, ask the user for it.

**Content Organization:**

- Start with the Front-Matter, then follow with the rest of the file
- Follow the Front-Matter with an excerpt of the article (max 200 words), mentioning the main points and the author's name. End the excerpt with `<!--excerpt_end-->`. It should serve as a logical introduction to the main content
- Write excerpts that are informative and engaging
- After the excerpt and a blank line, provide a summary of the article. This is the main content and it should be a logical followup to the excerpt
- Never use the standard markdown section separator `---`. Headings automatically get a small underline applied through CSS.

**Specific Rules per Collection:**

- If the file is in the `_videos` directory **never include closing lines like `[Read the entire article here](URL)` for YouTube videos**
- In all other cases, end the content with a line that looks like this: `[Read the entire article here]([canonical_url]).` Replace `[canonical_url]` with the canonical URL

## Front-Matter Field Rules

**Required Fields:**

- Include all relevant front-matter fields found in other markdown files in the same folder such as: layout, title, description, author, categories, tags, date, canonical_url, permalink, **and any others present**
- Do not start descriptions with phrases like "In this article", "In this post", etc. Start with the main topic or key point

**Field-Specific Rules:**

- `layout`: Always set to `"post"` for collection content (_blogs,_news, _community,_videos, _roundups)
- `title`: Extract from the article or ask the user if not clear
- `author`: Extract from the article or ask the user if not clear. **For YouTube videos, always use the actual presenter/demonstrator as the author, not the channel owner**
- `canonical_url`: Use the provided URL, but remove any querystring parameters
- `viewing_mode`: `"internal"` for self-contained content (videos, roundups), `"external"` for content that links to original sources (news, blogs, community)
- `tags`: Array of relevant keywords. Format as `["Tag 1", "Tag 2", "Tag 3"]`. At least 10 if possible, but only if they really fit
- `section_names`: Array of normalized section identifiers (lowercase, hyphenated). Format as `["ai", "github-copilot"]`. Valid values: `["ai", "azure", "github-copilot", "dotnet", "devops", "security", "coding", "cloud"]`
- `permalink`: Format is `/section/collection/slug` where section is primary section, collection is the collection name without underscore
- `feed_name`: Use provided feed name. Omit if not provided
- `feed_url`: Use provided feed url. Omit if not provided

**Deprecated/Removed Fields** (DO NOT USE):

- `categories` - REPLACED by `section_names` (which uses normalized identifiers, not display names)
- `tags_normalized` - REMOVED (normalization happens in .NET code, not frontmatter)
- `excerpt_separator` - REMOVED (excerpts detected by `<!--excerpt_end-->` marker in content)
- `description` - REMOVED (excerpt serves this purpose)
- `page` - REMOVED (derived from file path in .NET)

For complete field definitions and examples, see [frontmatter-schema.md](frontmatter-schema.md).

**Frontmatter Field Order:**

Follow this order for consistency (see [frontmatter-schema.md](frontmatter-schema.md) for details):

1. `layout`
2. `title`
3. `author`
4. `canonical_url` (if applicable)
5. `viewing_mode`
6. `feed_name` (if applicable)
7. `feed_url` (if applicable)
8. `date`
9. `permalink`
10. `tags`
11. `section_names`
12. Collection-specific fields (plans, `ghes_support`, `coming_soon`)

## Markdown Formatting Rules

1. Use well-structured markdown.
2. For lists:
    - Place a single blank line before and after the entire list.
    - Do not add blank lines between list items.
    - List items must start with a dash and a space (e.g., `- item`).
3. For headings:
    - Place a single blank line before and after each heading.
    - Headings must start with one or more hash symbols followed by a space (e.g., `# Heading`, `## Subheading`).
4. End the file with a single blank line.
5. Do not leave trailing spaces at the end of any line.
6. Do not use tab characters; use spaces for indentation.
7. Ensure there is exactly one blank line between blocks (e.g., between headings, lists, paragraphs, and code blocks).
8. Do not use more than one consecutive blank line anywhere in the file.
9. All headings must increment by only one level at a time (e.g., do not jump from `#` to `###`).
10. Do not use inline HTML unless absolutely necessary.
11. Do not end headings with a colon (e.g., use `## Key Points` not `## Key Points:`).

## Code Examples

When including code examples in content, use proper syntax highlighting and clear labels:

```javascript
// ✅ CORRECT: Good example with comments
const result = processData(input);
```

```
// ❌ WRONG: Anti-pattern with explanation
var data = undefined; // Never use var or undefined
```

**Code Example Best Practices:**

- Always specify the language for syntax highlighting
- Use comments to explain what makes examples good or bad
- Mark good examples with ✅ and bad examples with ❌
- Include explanatory comments within code blocks
- Test all code examples to ensure they work

## Examples

Lists:

```markdown
# Heading

1. Ordered list item 1
2. Ordered list item 2
   Some text about list item 2
   
   - The first unordered list inside item 2
     - The first item of an even deeper nested list
     - The second item of an even deeper nested list
       More text
   - The second unordered list inside item 2
   - The thrid unordered list inside item 2
     Even here can be some text
3. Ordered list item 3

## Subheading that increases with 1 level

- Unordered list
- Unordered list
  1. First subitem
  2. Second subitem
     
     - Etc
     - Etc

## Another subheading

Some text here

### Another level increase

#### And another

Always end with a single blank line.
```

## Front-Matter Formatting Example

For complete examples and field definitions, see [frontmatter-schema.md](frontmatter-schema.md).

**News article example:**

```yaml
---
layout: "post"
title: "GPT-5 Comes to GitHub Copilot in Visual Studio"
author: "Rhea Patel"
canonical_url: "https://devblogs.microsoft.com/visualstudio/gpt-5-now-available-in-visual-studio/"
viewing_mode: "external"
feed_name: "Microsoft VisualStudio Blog"
feed_url: "https://devblogs.microsoft.com/visualstudio/feed/"
date: 2025-08-12 17:10:52 +00:00
permalink: "/github-copilot/news/GPT-5-Comes-to-GitHub-Copilot-in-Visual-Studio"
tags: ["AI", "GitHub Copilot", "GPT-5", "Visual Studio", "News"]
section_names: ["ai", "github-copilot"]
---
```

**Video example:**

```yaml
---
layout: "post"
title: "AI-Powered Email Sorting with n8n and OpenAI"
author: "Alireza Chegini"
canonical_url: "https://www.youtube.com/watch?v=21HSDwtkHNk"
viewing_mode: "internal"
date: 2025-06-04 19:00:03 +00:00
permalink: "/ai/videos/AI-Powered-Email-Sorting-with-n8n-and-OpenAI"
tags: ["AI", "Automation", "Email Management", "n8n", "OpenAI", "Videos"]
section_names: ["ai"]
---
```

**See [frontmatter-schema.md](frontmatter-schema.md) for more examples** including blogs, roundups, and GitHub Copilot features.
