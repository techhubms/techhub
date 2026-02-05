# Content Processing

## Overview

The Tech Hub supports both manual and automated content creation. Content is organized into collections and automatically categorized using AI-powered processing.

## Content Creation Methods

### Database Synchronization

**Important**: Tech Hub uses a **database-backed** content system. Markdown files in the `collections/` folder are the **source of truth**, but the application serves content from a database (SQLite or PostgreSQL) for performance.

**How it works**:

1. **First startup**: Database syncs from all markdown files (~30-60s for 4000+ files)
2. **Subsequent startups**: Hash-based diff detects changes (<1s if no changes)
3. **Configuration**: Set `ContentSync:Enabled = false` in appsettings.json to skip sync for faster local dev

For details on supported database providers and their configuration, see [database.md](database.md).

### Manual Content Creation with GitHub Copilot

The easiest way to add a single contentitem is using the built-in GitHub Copilot commands:

```text
/new-article
```

This command will:

- Ask for the source URL and content type
- Automatically extract and process the content
- Generate appropriate frontmatter and structure
- Create the markdown file in the correct location
- Apply proper formatting (use `npx markdownlint-cli2 --fix` to fix issues)

### Automated RSS Content Creation

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

- [Terminology](terminology.md) - Site terminology, sections, and standard values
- [Repository Structure](repository-structure.md) - Code and content organization
- [Collections Guide](../collections/AGENTS.md) - Content management overview
- [Custom Pages](custom-pages.md) - Custom pages and specialized collections
- [Terminology](terminology.md) - Site terminology and standard values

## Troubleshooting

### Common Issues

- **Missing Frontmatter**: Check requirements in [collections/AGENTS.md](../collections/AGENTS.md#frontmatter-schema)
- **File Naming**: Use `YYYY-MM-DD-title.md` pattern
- **Sections/Tags**: Verify against site configuration in [terminology.md](terminology.md). Note: frontmatter uses `section_names` field with normalized section identifiers ("ai", "github-copilot")
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
- **Error Handling**: Graceful degradation when content fetching fails

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
- Standardized front matter (see [collections/AGENTS.md](../collections/AGENTS.md#frontmatter-schema))

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
