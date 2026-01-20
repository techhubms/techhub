# Content Management

## Overview

The Tech Hub supports both manual and automated content creation. Content is organized into collections and automatically categorized using AI-powered processing.

## Content Creation Methods

### 1. Manual Content Creation with GitHub Copilot (Recommended)

The easiest way to create content is using the built-in GitHub Copilot commands:

```text
/new-article
```

This command will:

- Ask for the source URL and content type
- Automatically extract and process the content
- Generate appropriate frontmatter and structure
- Create the markdown file in the correct location
- Apply proper formatting according to [Markdown Guidelines](../collections/markdown-guidelines.md)

### 2. Manual File Creation

For direct file creation:

1. **Choose Content Type**: Select the appropriate collection directory (see root [AGENTS.md](../AGENTS.md) Site Terminology section for complete list with descriptions)
2. **Create the File**: Use naming convention `YYYY-MM-DD-title-slug.md`
3. **Add Content**: Follow the structure and formatting rules in [Markdown Guidelines](../collections/markdown-guidelines.md)

### 3. GitHub Copilot Features Content

GitHub Copilot feature demonstration videos are managed through a special collection structure:

**Location**: `_videos/ghc-features/` subfolder

**Special Requirements**:

- Must include `section_names: ["github-copilot", "ai"]` in frontmatter
- Must include `plans: ["Free"|"Pro"|"Business"|"Pro+"|"Enterprise"]` array to specify which subscription tiers support the feature
- Must include `ghes_support: true|false` to indicate GitHub Enterprise Server support
- Must include `alt-collection: "features"` to highlight the Features tab instead of the Videos tab
- **Date-based filtering**: Videos with future dates are still shown as features but are not clickable as this is the mechanism to distinguish between features that have a demo video or not. So if you create a video for a feature, make sure to set update the date in the filename, frontmatter AND permalink!

**Automatic Integration**:

- Videos in this folder automatically populate the `/github-copilot/features.md` page
- Features are organized by subscription tier based on the `plans` frontmatter
- The page dynamically filters features based on publication date and subscription level
- Replaces the previous `_data/copilot_plans.json` system with a more maintainable video-based approach

### 4. Visual Studio Code Updates Videos

Visual Studio Code update videos are managed through a special collection structure:

**Location**: `_videos/vscode-updates/` subfolder

**Special Requirements**:

- Must include `section_names: ["github-copilot", "ai"]` in frontmatter
- Must include `alt-collection: "vscode-updates"` to highlight the Visual Studio Code Updates tab instead of the Videos tab
- Should include `youtube_id` for embedded video playback

**Automatic Integration**:

- Videos in this folder automatically appear on the `/github-copilot/vscode-updates.md` page
- The latest video is featured prominently at the top of the page
- Older videos are listed below with links and descriptions

### 5. Automated RSS Content Creation

The site automatically processes RSS feeds from Microsoft and technology sources. This system combines automated feed processing with AI-powered content categorization.

For full details on the RSS processing pipeline, see the [RSS Feed Processing](#rss-feed-processing) section below.

## Publishing Content

Use the GitHub Copilot command for publishing:

```text
/pushall
```

This command will:

- Stage all changes
- Create a commit with descriptive message
- Handle branch protection and rebasing
- Push changes to remote repository
- Optionally create pull request

For detailed information about site structure and terminology, see:

- [Root AGENTS.md](../AGENTS.md) - Site Terminology and Repository Structure sections
- [Collections Guide](../collections/AGENTS.md) - Content management overview

## Troubleshooting

### Common Issues

- **Missing Frontmatter**: Check requirements in [Markdown Guidelines](../collections/markdown-guidelines.md)
- **File Naming**: Use `YYYY-MM-DD-title.md` pattern
- **Sections/Tags**: Verify against site configuration in root [AGENTS.md](../AGENTS.md) Site Terminology section. Note: frontmatter uses `section_names` field with normalized section identifiers ("ai", "github-copilot")
- **Date Formats**: Use ISO 8601 format: `YYYY-MM-DD HH:MM:SS +00:00`

### Repair Tools

Use the PowerShell repair script to fix AI-generated markdown formatting issues:

```powershell
# Fix all markdown files in the repository
scripts/content-processing/fix-markdown-files.ps1

# Fix a specific file only
scripts/content-processing/fix-markdown-files.ps1 -FilePath "docs/example-file.md"
```

This script automatically fixes markdown formatting issues like missing blank lines, heading spacing, and list formatting.

**Note**: New content from RSS feeds already has correct frontmatter format (section_names). This script does NOT modify frontmatter - it only fixes AI-generated markdown formatting problems.

## Alternative Collection Tab Highlighting

The `alt-collection` frontmatter field allows content items to highlight a different collection tab in the navigation bar than their default collection would suggest. This is particularly useful for specialized subcollections like GitHub Copilot Features and Visual Studio Code Updates.

### How It Works

By default, videos in the `_videos` collection highlight the "Videos" tab in the navigation. However, videos in the `ghc-features` and `vscode-updates` subfolders should highlight their respective tabs instead:

- Videos in `_videos/ghc-features/` highlight the "Features" tab
- Videos in `_videos/vscode-updates/` highlight the "Visual Studio Code Updates" tab

### Usage

Add the `alt-collection` field to the frontmatter of your content:

```yaml
---
layout: "post"
title: "Your Video Title"
section_names:
- github-copilot
- ai
alt-collection: "features"  # or "vscode-updates"
---
```

### Supported Values

- `"features"` - Highlights the Features tab
- `"vscode-updates"` - Highlights the Visual Studio Code Updates tab

### Implementation Details

The navigation component checks for the `alt-collection` frontmatter field and uses it to determine which collection tab should be highlighted in the GitHub Copilot section. This overrides the default collection-based highlighting logic.

The `alt-collection` value is matched against the URL path of collection links in the navigation. For example, `"features"` matches `/github-copilot/features`, and `"vscode-updates"` matches `/github-copilot/vscode-updates`.

### Testing

End-to-end tests verify that the custom pages work correctly:

- GitHub Copilot Features page (`/github-copilot/features`)
- Visual Studio Code Updates page (`/github-copilot/vscode-updates`)
- Other custom pages

Tests are located in [tests/TechHub.E2E.Tests/Web/CustomPagesTests.cs](../tests/TechHub.E2E.Tests/Web/CustomPagesTests.cs).

## RSS Feed Processing

The Tech Hub automatically processes RSS feeds from Microsoft and technology sources to keep content current. This system combines automated feed processing with AI-powered content categorization and runs hourly via GitHub Actions.

### Feed Configuration

RSS feeds are configured in `scripts/data/rss-feeds.json`:

```json
{
  "feeds": [
    {
      "name": "Microsoft AI Blog",
      "url": "https://blogs.microsoft.com/ai/feed/",
      "output_dir": "_news",
      "section": "AI",
      "enabled": true
    }
  ]
}
```

**Required Fields**:

- **name**: Human-readable feed identifier
- **url**: RSS or Atom feed URL
- **output_dir**: Target collection directory (`_news`, `_blogs`, etc.)
- **section**: Section display title for categorization (e.g., "AI", "GitHub Copilot") - RSS processing converts these to normalized `section_names` array in frontmatter (e.g., ["ai"], ["github-copilot", "ai"])

**Optional Fields**:

- **enabled**: Boolean to enable/disable feed (default: true)
- **max_items**: Maximum items per processing run (default: 10)

### Adding New Feeds

**Using GitHub Copilot**:

```text
/new-rss-feeds
```

**Manual Addition**:

1. Edit `scripts/data/rss-feeds.json`
2. Add new feed object to the feeds array
3. Validate JSON format
4. Commit changes

### Processing Pipeline

The RSS processing system uses per-entry content fetching and Azure AI Foundry integration:

```text
RSS Download → Per-Entry Content Fetching → AI Analysis (Azure AI Foundry) → Content Creation → Tag Enhancement → Commit
```

**Download Phase**:

- **Individual JSON Files**: Each RSS entry is saved as a separate JSON file in structured directories
- **Content Enrichment**: Actual web content is fetched and stored alongside RSS metadata
- **Rate-Limited Fetching**: Individual URL fetching with rate limiting between requests

**Content Fetching Strategy**:

1. **YouTube Videos**: Metadata processing without content fetching
2. **Other URLs**: Individual HTTP requests with 10-second rate limiting
3. **Error Handling**: Graceful degradation when content fetching fails

### Azure AI Foundry Integration

The system uses Azure AI Foundry for content processing:

- **Endpoint**: `https://<resource>.services.ai.azure.com/models/chat/completions`
- **Authentication**: Azure API Key
- **Models**: Deployment names configured in Azure resource
- **Rate Limiting**: 15-second delays between API calls

**Configuration Example**:

```powershell
./scripts/content-processing/process-rss-to-markdown.ps1 "owner/repo" "api_key123" -Endpoint "https://myresource.services.ai.azure.com/models/chat/completions" -Model "gpt-4.1"
```

### Branch Strategy

The RSS processing workflow uses a two-branch strategy:

- **rss-updates branch**: All RSS processing occurs here (JSON files, AI analysis, markdown creation)
- **main branch**: Updated only when new markdown content is published

**Key Behavior**: If RSS processing only updates tracking data (JSON files), changes remain on rss-updates branch. Main branch is updated only with new/modified markdown files.

### AI Content Analysis

The system analyzes RSS content using comprehensive categorization rules:

**Exclusion Rules** (Take Precedence):

- Biographical content, question-only posts, short posts (<200 words)
- Sales pitches, non-English content, job postings
- Non-development Microsoft business products

**Inclusion Categories**:

- **Microsoft AI**: Azure OpenAI, Copilot services, AI Foundry, Semantic Kernel
- **GitHub Copilot**: All Copilot editions, features, integrations (always includes AI)
- **Coding**: Microsoft languages (C#, F#, TypeScript), .NET ecosystem
- **DevOps**: Azure DevOps, GitHub Actions, CI/CD, monitoring
- **Azure**: All Azure services, ARM/Bicep templates, cloud architecture

### Content Output

Each RSS item creates a markdown file with:

- AI-generated excerpt and summary
- Proper categorization and tagging
- External URL to original source (stored in frontmatter `external_url` field)
- Viewing mode varies by collection:
  - Videos and roundups: `"internal"` (content opens on site)
  - All other collections: `"external"` (links open in new tab)
- Standardized front matter (see [Markdown Guidelines](../collections/markdown-guidelines.md))

### Processing Scripts

**Core Scripts**:

- **`scripts/content-processing/download-rss-feeds.ps1`**: Downloads RSS feeds and saves entries as JSON files
- **`scripts/content-processing/process-rss-to-markdown.ps1`**: Processes JSON entries through AI to create markdown files
- **`scripts/content-processing/functions/Get-FilteredTags.ps1`**: Tag enhancement and normalization

**Execution**:

- **Automated**: Every hour via GitHub Actions (scheduled workflow)
- **Manual**: Can be triggered through GitHub Actions interface
- **Local**: Available for development testing

### Error Handling

**Common Issues**:

- **Feed Unavailability**: Automatic retry logic for temporary outages
- **Network Issues**: Built-in retry mechanisms with exponential backoff
- **API Limitations**: Respects rate limits and manages quotas for Azure AI Foundry
- **Content Fetching Failures**: Graceful degradation when individual URL content cannot be fetched

### Automatic Deployment

When RSS workflow commits markdown files to main branch, deployment to Azure Container Apps is automatically triggered via CI/CD pipeline. New content appears on the live site within minutes.
