# Site Terminology

This document defines the key terms and concepts used throughout the Tech Hub site architecture and development workflow.

## Core Concepts

### Sections

**Definition**: Top-level organizational units that group related content by topic or domain.

**Purpose**: Sections provide thematic organization and allow users to focus on specific areas of interest.

**Examples**:

- **AI Section** (`/ai`): General AI-related content including tools, news, and resources
- **GitHub Copilot Section** (`/github-copilot`): Dedicated content specifically about GitHub Copilot

**Configuration**: Defined in `_data/sections.json` as the single source of truth. Each section includes:

- Display title and description
- URL path and routing
- Associated category for filtering
- Background image for visual identity
- Collections that appear within the section

**Key Properties**:

- Sections are dynamic and configuration-driven
- New sections can be added without code changes
- Each section has its own index page and navigation

### Collections

**Definition**: Content types that represent different formats of information within sections.

**Purpose**: Collections organize content by format and purpose, allowing users to find specific types of resources.

**Examples**:

- **news**: Official product updates and announcements
- **videos**: Educational and informational video content
- **community**: Microsoft Tech community posts and other community-sourced content
- **events**: Official events and community meetups
- **posts**: Blog posts and articles
- **roundups**: Curated content summaries (special collection shown on homepage)

**Configuration**:

- Defined in `_config.yml` for Jekyll processing
- Associated with sections via `_data/sections.json`
- Can be marked as `custom: true` (manually created) or `custom: false` (auto-generated)

**Technical Details**:

- Each collection has its own directory (e.g., `_news/`, `_videos/`)
- Collections with `output: true` generate individual pages for each item
- Auto-generated collection pages are created by `section_pages_generator.rb`

### Items

**Definition**: Individual pieces of content within collections.

**Purpose**: Items are the actual content pieces that users consume - articles, videos, announcements, etc.

**Examples**:

- A news article in the `_news/` collection
- A video tutorial in the `_videos/` collection
- A community discussion post in the `_community/` collection
- A blog post in the `_posts/` collection

**Structure**: Items are typically Markdown files with YAML front matter containing:

- Metadata (title, date, author, categories, tags)
- Content body in Markdown format
- Optional custom fields specific to the collection type

**Processing**: Items are processed by Jekyll and can be:

- Listed on collection pages
- Filtered by date, tags, or categories
- Displayed on section index pages
- Included in RSS feeds and search functionality

## Filtering Systems

### Date Filters

**Definition**: Client-side filtering mechanism that allows users to narrow content by publication date ranges.

**Purpose**: Helps users find recent content or content from specific time periods.

**Functionality**:

- Filter items by date ranges (e.g., "Last 30 days", "Last 6 months")
- Dynamically update displayed content without page reload
- Combine with other filters for refined searches

**Implementation**: JavaScript-based filtering that works with the date metadata from item front matter.

**Usage Examples**:

- Show only news from the current month
- Display videos published in the last quarter
- Filter community posts from a specific date range

### Section Tag Filters

**Definition**: Client-side tag filtering system available only on the root index page that allows users to filter content by main site sections, implemented as normalized tags.

**Purpose**: Enables users to focus on content from specific topical areas (AI vs GitHub Copilot) using the unified tag-based architecture.

**Functionality**:

- Filter items using normalized section tags ("ai", "github copilot")
- Available only on the main index page (/)
- Uses the same tag matching logic as all other filters
- Dynamic content updates based on selected section tags
- Implements subset matching for tag-based filtering

**Implementation**: JavaScript-based system using pre-calculated tag relationships from server-side generation. Section names are converted to normalized tags and processed through the unified filtering system.

**Usage Examples**:

- Show only AI-related content across all collections using the "ai" tag
- Display only GitHub Copilot content from all sources using the "github copilot" tag
- Filter to see content from specific topical domains through normalized tag matching

### Collection Tag Filters

**Definition**: Client-side tag filtering system available on section index pages that allows users to filter content by content type within that section, implemented as normalized tags.

**Purpose**: Enables users to focus on specific content formats (News, Videos, Community, etc.) within a section using the unified tag-based architecture.

**Functionality**:

- Filter items using normalized collection tags ("news", "posts", "videos", etc.)
- Available only on section index pages (/ai, /github-copilot)
- Uses the same tag matching logic as all other filters
- Each collection type corresponds to a normalized tag
- Dynamic content updates based on selected collection tags
- Implements subset matching for tag-based filtering

**Implementation**: JavaScript-based system using pre-calculated tag relationships from server-side generation. Collection names are converted to normalized tags and processed through the unified filtering system.

**Usage Examples**:

- Show only News articles within the AI section using the "news" tag
- Display only Community content within GitHub Copilot section using the "community" tag
- Filter to see Videos from the current section using the "videos" tag

### Content Tag Filters

**Definition**: Client-side tag filtering system available on collection pages that allows users to filter content by keywords and topics using normalized content tags.

**Purpose**: Enables users to find content related to specific technologies, concepts, or themes within a collection using the unified tag-based architecture.

**Functionality**:

- Filter items using normalized content tags from front matter
- Support multiple tag selection for intersection filtering (AND logic)
- Available only on individual collection pages
- Uses the same tag matching logic as all other filters
- Dynamic content updates based on selected tags
- Implements subset matching for tag-based content discovery

**Implementation**: JavaScript-based system using pre-calculated tag relationships from server-side generation. Content tags are normalized and processed through the unified filtering system with subset matching support.

**Usage Examples**:

- Filter AI News by "machine learning", "neural networks", or "gpt" tags using subset matching
- Show GitHub Copilot Community content tagged with "visual studio code" or "productivity"
- Find Videos tagged with specific programming languages through normalized tag matching

## Jekyll/Liquid Processing

### Jekyll Filters (Liquid Filters)

**Definition**: Server-side template functions that transform and manipulate data during the Jekyll build process.

**Purpose**: Process content and data before the static site is generated, enabling dynamic content organization and formatting.

**Types**:

#### Built-in Liquid Filters

- **Date filters**: `| date: "%Y-%m-%d"` - Format dates for display
- **String filters**: `| upcase`, `| downcase`, `| strip` - Text manipulation
- **Array filters**: `| sort`, `| reverse`, `| limit` - Collection manipulation
- **URL filters**: `| relative_url`, `| absolute_url` - Link generation

#### Custom Jekyll Filters

Located in `_plugins/` directory and extend Jekyll's functionality:

- **`date_filters.rb`**: Custom date formatting and manipulation
- **`tag_filters.rb`**: Advanced tag processing and organization
- **`string_filters.rb`**: String processing and validation utilities

**Usage Examples**:

```liquid
<!-- Format publication date -->
{{ post.date | date: "%B %d, %Y" }}

<!-- Limit and sort items -->
{{ site.posts | sort: 'date' | reverse | limit: 5 }}

<!-- Generate proper URLs -->
<a href="{{ page.url | relative_url }}">{{ page.title }}</a>

<!-- Custom filter for tag organization -->
{{ site.tags | organize_by_category }}
```

**Key Differences from Client-Side Filters**:

- **Processing Time**: Jekyll filters run during build time, not in the browser
- **Data Access**: Can access all site data and metadata
- **Output**: Generate static HTML content
- **Performance**: No runtime performance impact on users
- **Scope**: Can manipulate data structure and organization before client-side filters receive it

## Relationship Between Concepts

1. **Sections** contain multiple **Collections**
2. **Collections** contain multiple **Items**
3. **Items** have metadata (dates, tags) used by **Date Filters** and **Tag Filters**
4. **Jekyll Filters** process all data during build time to prepare content for client-side consumption
5. Client-side **Date Filters** and **Tag Filters** provide interactive content discovery

This hierarchical organization enables both powerful content management and flexible user experience through multiple layers of filtering and organization.

## Content Structure Terminology

### Excerpt

**Definition**: An introduction that summarizes the main points of an article and mentions the author.

**Purpose**: Serves as a logical introduction to the main content and provides users with a quick overview of what to expect.

**Requirements**:

- Must appear directly after the frontmatter
- Maximum 200 words
- Must be immediately followed by the `<!--excerpt_end-->` code
- Should be informative and engaging
- Must mention the author's name

### Content

**Definition**: The main content section that provides a detailed, well-structured markdown summary of the article.

**Purpose**: Delivers the full information following logically from the excerpt.

**Requirements**:

- Always comes after the excerpt and the `<!--excerpt_end-->` marker
- Can be as long as needed
- Must follow the excerpt logically
- Should be well-structured using proper markdown formatting

## Content Type Terminology

### Understanding "Post" vs "Blog" vs "Article" vs "Item" in the Codebase

The codebase uses terminology that requires careful understanding:

#### Content Type Definitions

- **`site.posts`**: Jekyll's default collection containing **blogs** (not all content)
- **"Blog"**: Refers specifically to content in the `_posts/` folder (`site.posts` collection)
- **"Item"**: General term for any content from any collection (news, videos, blogs, etc.)
- **"Article"**: Same meaning as Item. Prefer Item over Article!
- **"Post"**: Generic term in variable names and CSS classes, does NOT refer specifically to `site.posts`

#### Variable and CSS Class Naming Conventions

- **Variables named "post"**: Usually refer to ANY collection item, not just blogs from `site.posts`
- **CSS classes with "post"**: Style ANY collection content, not just blogs
- **Functions with "post"**: Process ANY collection content, not just blogs

**Example of generic "post" usage:**

```liquid
{%- for post in filtered_articles -%}  <!-- "post" = any collection item -->
  <div class="post-title">{{ post.title }}</div>  <!-- CSS for any article type -->
{%- endfor -%}
```
