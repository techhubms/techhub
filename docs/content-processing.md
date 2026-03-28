# Content Processing

## Overview

The Tech Hub supports both manual and automated content creation. Content is organized into collections and automatically categorized using AI-powered processing. All content is stored exclusively in the PostgreSQL database.

## Content Architecture

### Database as Source of Truth

**Important**: The PostgreSQL database is the **single source of truth** for all content. Content is written directly to the database by:

1. **`ContentProcessingBackgroundService`** (production) — Background service running inside `TechHub.Api` on a configurable schedule. Downloads RSS feeds, categorizes content with Azure OpenAI, and writes directly to the database.

### Environment Strategy

| Environment | Content Source |
|-------------|---------------|
| Production | `ContentProcessingBackgroundService` (in `TechHub.Api`) writes RSS content directly to the database |
| Staging | Database restore from production snapshot (`scripts/Restore-Database.ps1`) |
| Local development | Database restore from production snapshot |

For local development, see [running-and-testing.md](running-and-testing.md) for setup instructions.

### Database Restore (Staging/Local)

To populate a non-production environment with real data:

```powershell
# Restore production data to local Docker Compose database
./scripts/Restore-Database.ps1 -Target local

# Restore production data to staging
./scripts/Restore-Database.ps1 -Target staging
```

Requires VPN access to the production environment and PostgreSQL client tools installed.

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

The Tech Hub automatically processes RSS feeds from Microsoft and technology sources to keep content current. The processing pipeline is implemented in C# as `ContentProcessingBackgroundService` inside `TechHub.Api` and runs on a configurable interval (default: every 15 minutes) in production.

### Content Processing Pipeline

The `ContentProcessingBackgroundService` (`IHostedService`) runs inside the API on a configurable schedule and performs:

1. **Feed ingestion** — Downloads and parses RSS/Atom XML from configured feed URLs
2. **Content fetching** — Fetches the full article HTML from each item's source URL, or YouTube closed captions (transcripts) for video items
3. **AI categorization** — Sends article content to Azure OpenAI to determine collection, sections, tags, title, and excerpt
4. **Deduplication** — Checks the database for existing items by `external_url` before writing
5. **Database write** — Inserts or updates the content item and tag expansions directly in PostgreSQL

Items that the AI determines are off-topic or low quality are skipped (not written to the database).

### Feed Configuration

RSS feeds are stored in the PostgreSQL database and managed via the admin UI at `/admin/feeds`. On first startup, the API seeds the database from `scripts/data/rss-feeds.json` if no feeds exist yet.

**Required Fields**:

- **name**: Human-readable feed identifier
- **url**: RSS or Atom feed URL
- **output_dir**: Target collection directory (`_news`, `_blogs`, etc.)

**Optional Fields**:

- **enabled**: Boolean to enable/disable feed (default: true)

### Adding New Feeds

**Via Admin UI** (recommended):

1. Navigate to `/admin/feeds` (requires Azure AD authentication in deployed environments)
2. Click "Add Feed"
3. Fill in name, URL, output directory
4. Save

**Via JSON seed file** (initial setup only):

1. Edit `scripts/data/rss-feeds.json`
2. On next startup (if database has no feeds), the API seeds from this file

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

1. **YouTube Videos**: Closed captions (transcripts) are fetched via **YoutubeExplode**, preferring English tracks. The transcript text is sent to the AI as context for generating structured video summaries. Transcript fetching is resilient — failures are non-fatal and the pipeline continues with metadata only.
2. **Other URLs**: Individual HTTP requests with rate limiting between requests
3. **Error Handling**: Graceful degradation when content or transcript fetching fails

### Azure AI Foundry Integration

The system uses Azure AI Foundry for content processing:

- **Endpoint**: `https://<resource>.cognitiveservices.azure.com/openai/deployments/<model>/chat/completions`
- **Authentication**: Azure API Key
- **Models**: Deployment names configured in Azure resource
- **Rate Limiting**: 15-second delays between API calls

**Configuration Example**:

```powershell
./scripts/content-processing/process-rss-to-markdown.ps1 "owner/repo" "api_key123" -Endpoint "https://myresource.cognitiveservices.azure.com/openai/deployments/gpt-4.1/chat/completions" -Model "gpt-4.1"
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
- **.NET (dotnet)**: Microsoft languages (C#, F#, TypeScript), .NET ecosystem
- **DevOps**: Azure DevOps, GitHub Actions, CI/CD, monitoring
- **Azure**: All Azure services, ARM/Bicep templates, cloud architecture

### Content Output

Each RSS item creates a markdown file with:

- AI-generated excerpt and summary
- Proper categorization and tagging
- External URL to original source (stored in frontmatter `external_url` field)
- **Future date protection**: If the RSS feed's publication date is in the future (e.g., scheduled release dates), the current datetime is used instead for both the filename and frontmatter date
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
