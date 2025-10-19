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
- Apply proper formatting according to [Markdown Guidelines](markdown-guidelines.md)

### 2. Manual File Creation

For direct file creation:

1. **Choose Content Type**: Select the appropriate collection directory (see [Site Overview](site-overview.md) for complete list with descriptions)
2. **Create the File**: Use naming convention `YYYY-MM-DD-title-slug.md`
3. **Add Content**: Follow the structure and formatting rules in [Markdown Guidelines](markdown-guidelines.md)

### 3. GitHub Copilot Features Content

GitHub Copilot feature demonstration videos are managed through a special collection structure:

**Location**: `_videos/ghc-features/` subfolder

**Special Requirements**:

- Must include `section: "github-copilot"` in frontmatter
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

- Must include `section: "github-copilot"` in frontmatter
- Must include `alt-collection: "vscode-updates"` to highlight the Visual Studio Code Updates tab instead of the Videos tab
- Should include `youtube_id` for embedded video playback

**Automatic Integration**:

- Videos in this folder automatically appear on the `/github-copilot/vscode-updates.md` page
- The latest video is featured prominently at the top of the page
- Older videos are listed below with links and descriptions

### 5. Automated RSS Content Creation

The site automatically processes RSS feeds:

- **Scheduled Processing**: GitHub Actions check for new content periodically
- **AI Categorization**: Content is automatically categorized and tagged
- **Automatic Publishing**: New content appears without manual intervention

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

- [Site Overview](site-overview.md)
- [Site Terminology](terminology.md)

## Troubleshooting

### Common Issues

- **Missing Frontmatter**: Check requirements in [Markdown Guidelines](markdown-guidelines.md)
- **File Naming**: Use `YYYY-MM-DD-title.md` pattern
- **Categories/Tags**: Verify against site configuration in [Site Overview](site-overview.md)
- **Date Formats**: Use ISO 8601 format: `YYYY-MM-DD HH:MM:SS +00:00`

### Repair Tools

Use the PowerShell repair script to fix common issues:

```powershell
# Fix all markdown files in the repository

.\.github\scripts\fix-markdown-files.ps1

# Fix a specific file only

.\.github\scripts\fix-markdown-files.ps1 -FilePath "docs/example-file.md"
```

This script automatically fixes formatting, dates, frontmatter, and other common issues.

## Alternative Collection Tab Highlighting

### Overview

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
section: "github-copilot"
alt-collection: "features"  # or "vscode-updates"
---
```

### Supported Values

- `"features"` - Highlights the Features tab
- `"vscode-updates"` - Highlights the Visual Studio Code Updates tab

### Implementation Details

The header template (`_includes/header.html`) checks for the `alt-collection` frontmatter field and uses it to determine which collection tab should be highlighted. This overrides the default collection-based highlighting logic.

The `alt-collection` value is matched against the URL path of collection links in the navigation. For example, `"features"` matches `/github-copilot/features.html`, and `"vscode-updates"` matches `/github-copilot/vscode-updates.html`.

### Testing

End-to-end tests verify that the correct tabs are highlighted for:

- GitHub Copilot Features videos
- Visual Studio Code Updates videos
- Regular videos (control group)
- Collection pages themselves

Tests are located in `spec/e2e/tests/alt-collection-highlighting.spec.js`.
