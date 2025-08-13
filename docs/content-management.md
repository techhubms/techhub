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

### 3. Automated RSS Content Creation

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
