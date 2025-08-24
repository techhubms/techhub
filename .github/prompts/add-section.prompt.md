---
mode: 'agent'
description: 'Add a new section to the Tech Hub website following established patterns.'
---

# Add New Section Implementation

## Prerequisites

**CRITICAL**: Read `.github/copilot-instructions.md` and understand the project structure before starting.

This prompt guides you through adding a complete new section to the Tech Hub. The system uses a data-driven architecture where most pages are auto-generated from configuration.

## Step 1: Gather Requirements

Ask the user for these required inputs:

- **Section Name**: Display name (e.g., "Azure", "DevOps")
- **Section Key**: URL-friendly identifier (e.g., "azure", "devops") 
- **Description**: Engaging section description
- **Category**: Primary category for content filtering
- **Related Tags**: Tags that should categorize content into this section

## Step 2: Update Configuration

### Update sections.json

Add the new section to `_data/sections.json` following this pattern:

```json
"{section-key}": {
    "title": "{Section Name}",
    "description": "{Description with shortcut mention if applicable}",
    "url": "/{section-key}",
    "section": "{section-key}",
    "image": "/assets/section-backgrounds/{section-key}.jpg",
    "category": "{Category}",
    "collections": [
        {
            "title": "News",
            "url": "/{section-key}/news.html",
            "collection": "news",
            "description": "News articles from official sources."
        },
        {
            "title": "Blogs", 
            "url": "/{section-key}/posts.html",
            "collection": "posts",
            "description": "Curated blog posts."
        }
        // Add other collections as needed
    ]
}
```

**Note**: The Ruby plugin `section_pages_generator.rb` automatically generates all markdown files for entries with a "collection" field.

### Create Directory Structure

1. **Create section directory**: `mkdir /{section-key}/` if it doesn't exist
2. **Auto-generated files**: The `section_pages_generator.rb` plugin automatically creates:
   - `/{section-key}/index.md` - Main section page
   - Collection pages for entries with "collection" field (news.html, posts.html, etc.)
3. **Custom pages**: Manually create any collections without "collection" field (like "A(i) to Z")

## Step 3: Update Tag Processing (If Needed)

Check if `Get-FilteredTags.ps1` needs updates for new section tags:

- Review existing tag mappings for AI, Azure, DevOps, etc.
- Add new tag mappings only if they fall outside existing categories
- Focus on ensuring content gets properly categorized

## Step 4: Content Integration

### Review Existing Content

1. **Scan all collections** for content that should be in the new section:
   - `_news/`, `_posts/`, `_videos/`, `_community/`, `_events/`

2. **Provide categorization suggestions** to the user for relevant articles

3. **Update article frontmatter** by adding the new category to relevant content:
   ```yaml
   categories: [AI, New Section, GitHub Copilot]  # Multi-section support
   ```

**Important**: Content can appear in multiple sections. GitHub Copilot content must always include AI category.

### **CRITICAL: Content Categorization Process**

**Essential Step**: After adding the new section configuration, you MUST categorize existing content before testing:

1. **Create categorization script**: Build a PowerShell script to automatically add the new category to relevant markdown files
2. **Execute categorization**: Run the script to update all matching content files
3. **Run markdown formatters**: Execute `pwsh .github/scripts/fix-markdown-files.ps1` to ensure proper formatting
4. **Verify changes**: Confirm content now appears in the new section

**Why This Is Critical**: Without this step, the new section will be empty even if relevant content exists, because Jekyll filters content by categories in frontmatter. The section pages won't display any content until existing articles are properly categorized.

**Example Categorization Script**:
```powershell
# Scan all markdown files for the new section's keywords
$files = Get-ChildItem -Path "_*" -Filter "*.md" -Recurse
foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    if ($content -match "keyword1|keyword2|keyword3") {
        # Use Set-FrontMatterValue to add the new category
        $categories = Get-FrontMatterValue -Content $content -Key "categories"
        if ($categories -notcontains "New Section") {
            $updatedCategories = $categories + "New Section"
            $newContent = Set-FrontMatterValue -Content $content -Key "categories" -Value $updatedCategories
            Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8
        }
    }
}
```

## Step 5: Documentation and RSS

### Update Documentation

Update these files to include the new section:

- `.github/copilot-instructions.md` - Add categorization rules

### RSS Feeds (Optional)

Consider adding section-specific RSS feeds to `.github/scripts/rss-feeds.json` if relevant sources exist.
Suggest these sources to the user and let the user decide if they should be added.

## Step 6: Testing and Validation

### Build and Test

1. **Format content FIRST**: `pwsh .github/scripts/fix-markdown-files.ps1` - **CRITICAL: Run this after categorization**
2. **Jekyll build**: `pwsh jekyll-start.ps1` - verify no errors (startup can take 2-3 minutes)
3. **Run all tests**: `pwsh run-all-tests.ps1` - **Note: Jekyll startup can take 2-3 minutes for E2E tests**

**Important Order**: Always run markdown formatters before Jekyll build and testing to ensure proper frontmatter formatting.

### Manual Validation

1. **Test locally**: Navigate to the new section and verify:
   - Section appears in main navigation
   - Collection pages display content correctly
   - Filtering works properly
   - Mobile responsive design

2. **Cross-section functionality**: Ensure content appears in multiple sections when categorized

## Step 7: Final Steps

### Complete Implementation

1. **Add section background image**: Place image at `/assets/section-backgrounds/{section-key}.jpg`
2. **Review content categorization**: Apply user's selections to article frontmatter
3. **Final testing**: Verify complete functionality

### Summary

Provide:
- Files created/modified count
- Articles categorized into new section
- Test results (all 4 test types: PowerShell, JavaScript, Ruby, E2E)
- Any issues encountered and resolutions

## Key Points

- **Auto-generation**: Most pages are created automatically by Jekyll plugins
- **Multi-categorization**: Content can appear in multiple sections
- **Data-driven**: Configuration in `sections.json` drives page generation
- **Content categorization is ESSENTIAL**: New sections will be empty until existing content is categorized and formatted
- **Testing**: Use comprehensive 4-tier testing approach
- **GitHub Copilot rule**: Always include AI category for Copilot content
- **Process order matters**: Configure → Categorize → Format → Build → Test