---
applyTo: "**/*.md"
---

# Instructions for AI models when generating markdown files

**CRITICAL**: Always follow the writing style guidelines in [writing-style-guidelines.md](writing-style-guidelines.md) for tone, language, and content standards.

## Automated Processing

The repository includes a PowerShell script (`pwsh /workspaces/techhub/.github/scripts/fix-markdown-files.ps1`) that automatically fixes common markdown and frontmatter issues including:

- Date format standardization
- Frontmatter quoting consistency
- Tag and category array formatting
- Filename and permalink consistency for post layout files
- Duplicate frontmatter key detection
- Whitespace and formatting cleanup

**Usage:**

```powershell
# Process all markdown files in the repository

pwsh /workspaces/techhub/.github/scripts/fix-markdown-files.ps1

# Process a specific file only

pwsh /workspaces/techhub/.github/scripts/fix-markdown-files.ps1 -FilePath "docs/javascript-guidelines.md"
```

**CRITICAL**: Do not automatically run this script!

While following these instructions carefully is important, minor formatting issues will be automatically corrected by the script during processing.

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

**Content Type Specific Rules:**

- If the file is in the `_videos` directory **never include closing lines like `[Read the entire article here](URL)` for YouTube videos**
- In all other cases, end the content with a line that looks like this: `[Read the entire article here]([canonical_url]).` Replace `[canonical_url]` with the canonical URL

## Front-Matter Field Rules

**Required Fields:**

- Include all relevant front-matter fields found in other markdown files in the same folder such as: layout, title, description, author, categories, tags, date, canonical_url, permalink, **and any others present**
- Do not start descriptions with phrases like "In this article", "In this post", etc. Start with the main topic or key point

**Field-Specific Rules:**

- `layout`: Set to `post` for content files (those in collection folders like `_posts`, `_news`, `_community`, etc.). Set to `page` for section directory files (those in `/ai`, `/github-copilot`, etc.)
- `title`: Extract from the article or ask the user if not clear
- `description`: Write a concise summary of the article's main points and topics (max 100 words). Do not start with phrases like "In this article" or similar
- `author`: Extract from the article or ask the user if not clear. **For YouTube videos, always use the actual presenter/demonstrator as the author, not the channel owner**. If there is no presenter/demonstrator, use the channel owner.
- `excerpt_separator`: Always set to `<!--excerpt_end-->`
- `canonical_url`: Use the provided URL, but remove any querystring parameters. Do not change the rest of the URL
- `tags`: Array of relevant keywords from the article formatted as `["Tag 1", "Tag 2", "Tag 3", "Tag++", "-Tag", "A . Tag", "C#", "Hyphen-Tag"]`. Do not use generic terms like 'news' or 'update'. At least 10 if possible, but only if they really fit. Don't make up things. Tags can contain spaces and will be automatically filtered and formatted. **Never include names, GitHub handles, or anything not directly related to the actual content of the video/article as tags**
- `tags_normalized`: Array of lowercase, normalized versions of the tags for consistent programmatic processing. Generated automatically by PowerShell scripts. Format as `["tag 1", "tag 2", "tag 3", "tagplusplus", "mintag", "a dot tag", "csharp", "hyphen-tag"]` with all lowercase and standardized formatting. Order should be identical to tags.
- `categories`: Array of strings formatted as `["AI", "GitHub Copilot"]`. Use the exact category names defined in the `site.sections` configuration in `_config.yml`. If you include 'GitHub Copilot', always include 'AI' as well. If unsure, leave empty
- `feed_name`: Use provided feed name. Omit if not provided
- `feed_url`: Use provided feed url. Omit if not provided
- `permalink`: Is `/filename.html`. Replace `.md` with `.html` from the filename. The section is the target directory, without an underscore (e.g. `roundups`, `news`, `videos`, etc)
- `page`: Automatically set based on directory structure. For `_posts` directory, use `"blogs"`. For other directories, use the directory name without underscore (e.g., `_news` becomes `"news"`, `_videos` becomes `"videos"`)
- `viewing_mode`: Determines how content is displayed on the site:
  - `"internal"`: Self-contained content displayed fully on site (videos, roundups)
  - `"external"`: Content that links to original sources (news, posts, community, etc)

**Date and Filename Rules:**

- Dates should be formatted as `yyyy-MM-dd HH:mm:ss +00:00` (with the colon in the timezone offset, not `+0000`). Use the appropriate timezone if different from UTC. The script will automatically repair common date format issues
- The filename format is: `yyyy-MM-dd-Article-Title.md`. For `layout: "post"` files, the filename must match the date in the frontmatter, and the permalink will be automatically generated as `/yyyy-MM-dd-Article-Title.html`

**Frontmatter Quoting Rules:**

- Most frontmatter values should be quoted with double quotes, except for: `excerpt_separator` and `date` which should remain unquoted
- Array values like `tags` and `categories` use square brackets with individual values quoted: `["Value 1", "Value 2", "Value 3"]`

**Frontmatter Field Order:**

Follow this specific field order to ensure consistency across all content files:

1. `layout`
2. `title`
3. `description`
4. `author`
5. `excerpt_separator`
6. `canonical_url` (if applicable)
7. `viewing_mode`
8. `feed_name` (if applicable)
9. `feed_url` (if applicable)
10. Other custom fields (e.g., `magazine-description-ai`, `download-url`)
11. `date`
12. `permalink`
13. `categories`
14. `tags`
15. `tags_normalized`

**Placement Rules:**

- `categories`, `tags`, and `tags_normalized` should always be placed at the bottom of the frontmatter (before the closing `---`)
- This reduces diff noise when the repair script processes files and ensures consistent formatting
- Custom fields should be placed after standard fields but before `date`

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

```yaml
---
layout: "post"
title: "Introducing automatic documentation comment generation in Visual Studio"
description: "GitHub Copilot now integrates with Visual Studio to auto-generate function doc comments, streamlining the documentation process for subscribers."
author: "Sinem Akinci, Allie Barry"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/visualstudio/introducing-automatic-documentation-comment-generation-in-visual-studio/"
viewing_mode: "external"
categories: ["AI", "GitHub Copilot"]
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/visualstudio/tag/copilot/feed/"
date: 2025-03-17 15:00:10 +00:00
permalink: "/2025-03-17-Introducing-automatic-documentation-comment-generation-in-Visual-Studio.html"
tags: ["AI", "Code Comments", "Developer Tools", "Docs", "Documentation", "GitHub Copilot", "News", "Productivity", "Visual Studio"]
tags_normalized: ["ai", "code comments", "developer tools", "docs", "documentation", "github copilot", "news", "productivity", "visual studio"]
---
```
