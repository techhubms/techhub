# RSS Feed Integration

## Overview

The Tech Hub automatically processes RSS feeds from Microsoft and technology sources to keep content current. This system combines automated feed processing with AI-powered content categorization and runs hourly via GitHub Actions.

## Feed Configuration

RSS feeds are configured in `.github/scripts/rss-feeds.json`:

```json
{
  "feeds": [
    {
      "name": "Microsoft AI Blog",
      "url": "https://blogs.microsoft.com/ai/feed/",
      "output_dir": "_news",
      "category": "AI",
      "enabled": true
    }
  ]
}
```

### Required Fields

- **name**: Human-readable feed identifier
- **url**: RSS or Atom feed URL
- **output_dir**: Target collection directory (`_news`, `_posts`, etc.)
- **category**: Primary category ("AI" or "GitHub Copilot")

### Optional Fields

- **enabled**: Boolean to enable/disable feed (default: true)
- **max_items**: Maximum items per processing run (default: 10)

## Adding New Feeds

### Using GitHub Copilot

```text
/new-rss-feeds
```

### Manual Addition

1. Edit `.github/scripts/rss-feeds.json`
2. Add new feed object to the feeds array
3. Validate JSON format
4. Commit changes

## Processing Pipeline

The RSS processing system has been enhanced with per-entry content fetching and dual AI provider support:

```text
RSS Download → Per-Entry Content Fetching → AI Analysis (GitHub Models or Azure AI Foundry) → Content Creation → Tag Enhancement → Commit
```

### Enhanced Download Phase

The download phase now includes comprehensive content fetching for each RSS entry:

#### **Per-Entry Processing**

- **Individual JSON Files**: Each RSS entry is saved as a separate JSON file in structured directories
- **Content Enrichment**: Actual web content is fetched and stored alongside RSS metadata
- **Batch Processing**: Optimized batch fetching for Reddit URLs using Playwright
- **Rate-Limited Fetching**: Individual URL fetching with rate limiting for non-Reddit sources

#### **Content Fetching Strategy**

1. **Reddit Content**: Batch processed using Playwright for JavaScript-enabled content
2. **YouTube Videos**: Metadata processing without content fetching
3. **Other URLs**: Individual HTTP requests with 10-second rate limiting
4. **Error Handling**: Graceful degradation when content fetching fails

### Dual AI Provider Support

The system now supports both GitHub Models and Azure AI Foundry for content processing:

#### **GitHub Models (Default)**

- **Endpoint**: `https://models.github.ai/inference/chat/completions`
- **Authentication**: GitHub Personal Access Token
- **Models**: `openai/gpt-4.1`, `microsoft/phi-3-5-mini-instruct`, etc.
- **Rate Limiting**: 15-second delays between API calls

#### **Azure AI Foundry**

- **Endpoint**: `https://<resource>.services.ai.azure.com/models/chat/completions`
- **Authentication**: Azure API Key
- **Models**: Deployment names configured in Azure resource
- **Enterprise Features**: Enhanced security, compliance, and governance

### Configuration Examples

#### Using GitHub Models (Default)

```powershell
./process-rss-to-markdown.ps1 "owner/repo" "ghp_token123"
```

#### Using Azure AI Foundry

```powershell
./process-rss-to-markdown.ps1 "owner/repo" "api_key123" -Endpoint "https://myresource.services.ai.azure.com/models/chat/completions" -Model "my-gpt4-deployment"
```

### Workflow Strategy

The RSS workflow uses a selective backup/recreate/restore strategy for branch synchronization:

1. **Diff Phase**: Compare main and rss-updates branches to identify only the files that have actually changed
2. **Backup Phase**: Selectively backup only the changed RSS-managed files (JSON tracking files and markdown content files)
3. **Recreate Phase**: The rss-updates branch is recreated to match the latest main branch exactly during the sync step
4. **Restore Phase**: Only the backed up changed files are restored, excluding any that were removed from main
5. **Processing Phase**: Enhanced RSS processing (download, per-entry content fetching, AI analysis, content creation)
6. **Copy Phase**: If markdown files changed, selective files are copied to main with intelligent filtering to distinguish NEW files vs deleted files

This selective approach is more efficient as it only handles files that have actually changed, reduces unnecessary file operations, and provides a predictable, clean-slate environment for each processing run while eliminating merge conflicts.

### Branch Strategy

The RSS processing workflow uses a two-branch strategy to ensure main branch stability:

- **rss-updates branch**: All RSS processing occurs here
  - Receives new RSS data (JSON files)
  - Processes content through AI analysis  
  - Creates markdown files from RSS entries
  - Applies tag enhancement and formatting fixes

- **main branch**: Updated only when new content is published
  - Receives commits only when markdown files are created or changed
  - Ensures main branch reflects actual published content
  - Keeps data-only changes isolated on rss-updates branch

**Key Behavior**: If RSS processing only updates tracking data (JSON files) without creating new markdown content, changes remain on rss-updates branch. The main branch is updated only when there are new or modified markdown files to publish.

**Branch Reset**: The rss-updates branch is automatically recreated from the latest main branch state at the start of every RSS processing run. This ensures each run starts with a clean slate, eliminating accumulated state and simplifying the sync process. The branch reset uses a selective backup/recreate/restore strategy that only handles files that have actually changed between branches, making the process more efficient than complex merge operations.

### Automatic Deployment Integration

When the RSS workflow successfully processes new content and commits markdown files to the main branch, the Azure Static Web App deployment is automatically triggered through an explicit workflow dispatch call. This ensures that new content is immediately available on the live site without manual intervention.

**Deployment Workflow Integration**:

- **Trigger**: RSS workflow explicitly triggers Azure Static Web App deployment via workflow_dispatch
- **Condition**: Only triggers when RSS workflow actually creates or modifies content files and syncs them to main
- **Timing**: Deployment is triggered immediately after RSS workflow commits new content to main branch
- **Automation**: No manual action required - fully automated content-to-production pipeline

**Key Benefits**:

- **Immediate Availability**: New RSS content appears on the live site within minutes
- **Zero Manual Intervention**: Fully automated from RSS processing to deployment
- **Optimal Efficiency**: Deployment only occurs when content is actually created or modified, eliminating unnecessary deployments
- **Resource Conservation**: No hourly deployments when RSS processing finds no new content
- **Reliable Triggering**: Explicit workflow dispatch bypasses GitHub's safety restrictions on automated workflow triggering

## Content Fetching Architecture

### Three-Phase Processing

#### Phase 1: Item Discovery and Planning

- Scan RSS feeds for new or updated entries
- Determine which items need content fetching
- Categorize URLs by type (Reddit, YouTube, other)
- Skip already processed items to avoid duplication

#### Phase 2: Batch Content Downloading

- **Reddit URLs**: Batch processed using Playwright for JavaScript rendering
- **Standard URLs**: Individual HTTP requests with exponential backoff
- **Rate Limiting**: Built-in delays to respect source website limits
- **Error Recovery**: Graceful handling of failed fetches

#### Phase 3: Enhanced Item Storage

```json
{
  "Title": "Article Title",
  "Link": "https://example.com/article",
  "PubDate": "2025-01-01T00:00:00Z",
  "Description": "RSS description",
  "Author": "Author Name",
  "EnhancedContent": "Full fetched content",
  "ProcessedDate": "2025-01-01 12:00:00",
  "FeedName": "Source Feed",
  "FeedUrl": "https://source.com/feed"
}
```

### Performance Optimizations

- **Concurrent Processing**: Batch operations for improved throughput
- **Smart Caching**: Skip re-processing of existing valid entries  
- **Selective Updates**: Only process changed items during incremental runs
- **Memory Management**: Efficient handling of large content sets

### AI Content Analysis

The system analyzes RSS content using a comprehensive AI prompt system with detailed categorization rules:

#### **Category Assignment Process**

- **Multi-Level Analysis**: Applies exclusion rules first, then inclusion rules for each category
- **Comprehensive Rule System**: Over 25 specific rules for categorizing content across 5 categories
- **Quality Filtering**: Automatic exclusion of low-quality, off-topic, or negative content
- **Multiple Category Support**: Can assign multiple categories when content spans topics

#### **Category Rules Overview**

**Exclusion Rules (Take Precedence)**:

- Biographical content about individuals
- Question-only or help-seeking posts
- Short community posts (<200 words)
- Sales pitches or tool announcements
- Non-English content
- Job postings or very negative content
- Non-development Microsoft business products

**Inclusion Categories**:

- **Microsoft AI**: Azure OpenAI, Copilot services, AI Foundry, Cognitive Services, Semantic Kernel
- **GitHub Copilot**: All Copilot editions, features, integrations, and best practices (always includes AI)
- **Coding**: Microsoft languages (C#, F#, TypeScript), .NET ecosystem, development tools
- **DevOps**: Azure DevOps, GitHub Actions, team practices, CI/CD, monitoring, developer experience
- **Azure**: All Azure services, ARM/Bicep templates, cloud architecture patterns

#### **Content Processing Outputs**

- **Categorize**: Assigns appropriate categories from the 5-category system based on comprehensive rules
- **Filter Quality**: Ensures content meets site standards using exclusion criteria
- **Generate Excerpts**: Creates 50-word introductions mentioning the author
- **Extract Tags**: Identifies 10+ relevant technology keywords from content
- **Create Summaries**: Produces detailed markdown content preserving all information
- **Provide Explanations**: Documents reasoning for category assignments for prompt refinement

### Content Output

Each RSS item creates a markdown file with:

- AI-generated excerpt and summary
- Proper categorization and tagging
- Canonical URL to original source
- Standardized front matter (see [Markdown Guidelines](markdown-guidelines.md))

### Deduplication

- **URL Tracking**: Prevents processing the same article twice
- **Content Similarity**: Detects and prevents duplicate content
- **File Existence**: Checks for existing files before creation

## AI Provider Configuration

### AI Service Selection

The RSS processing system supports both GitHub Models and Azure AI Foundry for content processing, providing flexibility in AI service selection based on organizational requirements.

#### GitHub Models (Default)

**Configuration**:

- **Endpoint**: `https://models.github.ai/inference/chat/completions`
- **Authentication**: GitHub Personal Access Token
- **Models**: `openai/gpt-4.1`, `microsoft/phi-3-5-mini-instruct`, etc.
- **Rate Limiting**: 15-second delays between API calls

**Setup Requirements**:

```powershell
# Using GitHub Models (default configuration)

./process-rss-to-markdown.ps1 "owner/repo" "ghp_token123"
```

**Features**:

- Integrated with GitHub ecosystem
- Simple token-based authentication
- Wide model selection
- Standard rate limiting

#### Azure AI Foundry

**Configuration**:

- **Endpoint**: `https://<resource>.services.ai.azure.com/models/chat/completions`
- **Authentication**: Azure API Key
- **Models**: Deployment names configured in Azure resource
- **Enterprise Features**: Enhanced security, compliance, and governance

**Setup Requirements**:

```powershell
# Using Azure AI Foundry

./process-rss-to-markdown.ps1 "owner/repo" "api_key123" -Endpoint "https://myresource.services.ai.azure.com/models/chat/completions" -Model "my-gpt4-deployment"
```

**Enterprise Features**:

- Enhanced security and compliance
- Custom model deployments
- Advanced governance controls
- Integration with Azure services

### Configuration Parameters

**Required Parameters**:

- **Repository**: `owner/repo` format
- **Authentication Token**: GitHub PAT or Azure API key

**Optional Parameters**:

- **Endpoint**: Custom AI service endpoint (defaults to GitHub Models)
- **Model**: Specific model or deployment name
- **Rate Limiting**: Configurable delays between requests

### Model Selection Guidelines

**GitHub Models**: Ideal for open-source projects and standard development workflows

**Azure AI Foundry**: Recommended for enterprise environments requiring enhanced security and compliance

## Tag Enhancement

After RSS processing, the PowerShell tag system (see [Filtering System](filtering-system.md)) automatically:

- **Normalizes Tags**: Converts variations to standard forms
- **Adds Technology Tags**: Detects and adds relevant Microsoft technology tags
- **Maintains Consistency**: Ensures tag format consistency across all content

## Processing Scripts

### Core Scripts

- **`complete-rss-workflow.ps1`**: Main orchestration script that handles the complete RSS workflow
- **`Get-FilteredTags.ps1`**: Tag enhancement and normalization
- Supporting functions in `.github/scripts/functions/` directory

### Execution

- **Automated**: Every hour via GitHub Actions (scheduled workflow)
- **Manual**: Can be triggered through GitHub Actions interface
- **Local**: Available for development testing

## Monitoring

### GitHub Actions Dashboard

- Monitor workflow status and execution history
- Review processing success rates
- Track error patterns and resolution

### Content Quality

- Items processed per run
- Duplicate detection effectiveness
- AI-generated content quality assessment

### Performance Monitoring

#### Content Fetching Metrics

- **Per-Entry Processing Times**: Track individual content fetching duration
- **Batch Processing Efficiency**: Monitor Reddit batch processing performance
- **Rate Limiting Compliance**: Verify adherence to source website limits
- **Memory Usage**: Track memory consumption during large content operations

#### AI Processing Metrics

- **GitHub Models Performance**: Monitor response times and rate limiting
- **Azure AI Foundry Performance**: Track deployment response metrics
- **Content Analysis Quality**: Assess AI categorization accuracy
- **Processing Throughput**: Measure items processed per hour

#### Workflow Optimization

- **Branch Synchronization**: Monitor selective backup/restore efficiency
- **Content Deduplication**: Track duplicate detection effectiveness
- **Error Recovery**: Measure resilience and fallback success rates
- **Overall Processing Times**: End-to-end workflow performance analysis

## Error Handling

### Common Issues

- **Feed Unavailability**: Automatic retry logic for temporary outages
- **Network Issues**: Built-in retry mechanisms with exponential backoff
- **API Limitations**: Respects rate limits and manages quotas for both GitHub Models and Azure AI Foundry
- **Content Fetching Failures**: Graceful degradation when individual URL content cannot be fetched
- **Playwright Processing Errors**: Robust handling of JavaScript rendering failures for Reddit content
- **Content Quality**: Filters out low-quality or irrelevant content through AI analysis

### Enhanced Error Recovery

- **Comprehensive Logging**: Detailed error tracking for troubleshooting across all processing phases
- **Graceful Handling**: Malformed feeds and individual item processing failures don't stop entire workflow
- **AI Processing Fallbacks**: Continues processing with RSS metadata when content fetching or AI analysis fails
- **Batch Processing Resilience**: Reddit batch processing continues even if individual items fail
- **Data Integrity**: Validates JSON structure and content before storage
- **Selective Processing**: Skip problematic items while continuing with successful ones

### Content Fetching Error Handling

- **Timeout Management**: Configurable timeouts for content fetching operations
- **Rate Limiting Compliance**: Automatic delays and retry logic to respect source website limits
- **Playwright Error Recovery**: Handles JavaScript-heavy site rendering failures gracefully
- **HTTP Error Codes**: Proper handling of 404, 403, and other HTTP response codes
- **Memory Management**: Prevents memory leaks during large content fetching operations

## Customization

### Processing Configuration

Environment variables for fine-tuning:

- `MAX_FEED_ITEMS`: Maximum items per processing run
- `CONTENT_QUALITY_THRESHOLD`: Minimum quality score for content inclusion

### Content Sources

To add new content types beyond RSS:

1. **API Integration**: Connect to content APIs directly
2. **Alternative Formats**: Support different feed formats
3. **Manual Processing**: Handle specially submitted content

For detailed content creation workflows, see [Content Management](content-management.md).
