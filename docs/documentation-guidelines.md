# Documentation Guidelines

This document defines the documentation structure, hierarchy, and content placement guidelines for the Tech Hub project.

For documentation philosophy, principles, and maintenance procedures, see [docs/AGENTS.md](AGENTS.md).

**Key Hierarchy**: Foundation → Functionality → Task → Language → Performance

## Documentation Structure

The Tech Hub uses a two-tier documentation system:

### 1. AGENTS.md Files (Domain-Specific)

Located in each major directory, AGENTS.md files provide focused guidance for AI assistants working in specific domains:

- **`AGENTS.md`** (root): General project overview and navigation to domain-specific agents
- **`scripts/AGENTS.md`**: PowerShell development and automation scripts
- **`_plugins/AGENTS.md`**: Jekyll Ruby plugin development
- **`assets/js/AGENTS.md`**: JavaScript client-side development
- **`_sass/AGENTS.md`**: SCSS styling and CSS architecture
- **`rss/AGENTS.md`**: RSS feed management and syndication
- **`docs/AGENTS.md`**: Documentation guidelines and maintenance
- **`spec/AGENTS.md`**: Testing strategies and frameworks

**Purpose**: Action-oriented, domain-specific guidance with critical rules and patterns for daily development work.

### 2. Documentation Files (Comprehensive Reference)

Located in `docs/`, these files provide complete technical details:

#### Foundation Files

- **`docs/documentation-guidelines.md`**: Documentation structure and hierarchy (this file)
- **`docs/terminology.md`**: Definitions, concepts, and basic understanding
- **`docs/site-overview.md`**: High-level architecture and system structure
- **Purpose**: Establish fundamental understanding before diving into specifics

#### Functionality-Oriented Files

- **`docs/filtering-system.md`**: Complete implementation of date and tag filtering systems
- **`docs/datetime-processing.md`**: Date handling, timezone configuration, and custom date filters
- **`docs/content-management.md`**: Content creation, organization, lifecycle management, and RSS feed processing
- **Purpose**: Explain **what** the system does and **how** it works

#### Task-Oriented Files

- **`docs/jekyll-development.md`**: Jekyll-specific development patterns and practices
- **`_plugins/AGENTS.md`**: Plugin development guidelines and patterns
- **Purpose**: Explain **how to do** specific development tasks

#### Content Standards

- **`docs/markdown-guidelines.md`**: Markdown formatting and content standards
- **`docs/writing-style-guidelines.md`**: Content writing style and tone guidelines
- **Purpose**: Language and content standards

#### Performance and Optimization

- **`docs/performance-guidelines.md`**: Optimization strategies and performance considerations

## Documentation vs AGENTS.md

| Aspect | Documentation (`docs/*.md`) | AGENTS.md Files |
| ------ | --------------------------- | --------------- |
| **Purpose** | Comprehensive reference | Action-oriented guidance |
| **Audience** | Humans and AI | AI assistants primarily |
| **Scope** | Complete feature coverage | Domain-specific focus |
| **Detail Level** | In-depth explanations | Critical rules and patterns |
| **Content** | Historical context, architecture | Current best practices |
| **Updates** | When features change | When development patterns change |

### When to Update Each

**Update Documentation** when:

- Feature specifications change
- Architecture evolves
- New components added
- Historical context needed

**Update AGENTS.md** when:

- Development patterns change
- Critical rules added
- Common errors identified
- Domain-specific guidance needed

## Documentation Hierarchy

When creating or updating documentation, follow this strict hierarchy to ensure content is placed in the most appropriate location. **Always check files in this order** and place content where it best fits according to the file's purpose:

### 1. Generic AI Instructions

- **`AGENTS.md`** (root): Only for high-level project overview and navigation to domain agents
- **Not for**: Specific implementation details or technical procedures

### 2. Domain-Specific Agent Instructions

Check the appropriate domain AGENTS.md file for action-oriented, development-focused content:

- PowerShell development → `scripts/AGENTS.md`
- Ruby plugins → `_plugins/AGENTS.md`
- JavaScript → `assets/js/AGENTS.md`
- SCSS styling → `_sass/AGENTS.md`
- RSS feeds → `rss/AGENTS.md`
- Documentation → `docs/AGENTS.md`
- Testing → `spec/AGENTS.md`

### 3. Foundation Files

- **`docs/documentation-guidelines.md`**: Documentation structure, hierarchy, and content placement rules
- **`docs/terminology.md`**: Definitions, concepts, and basic understanding
- **`docs/site-overview.md`**: High-level architecture and system structure
- **Purpose**: Establish fundamental understanding before diving into specifics

### 3. Functionality-Oriented Files

- **`docs/filtering-system.md`**: Complete implementation of date and tag filtering systems
- **`docs/datetime-processing.md`**: Date handling, timezone configuration, and custom date filters
- **`docs/content-management.md`**: Content creation, organization, lifecycle management, and RSS feed processing

## Content Placement Strategy

### Before Adding New Content

1. **Check existing files in hierarchy order** - Don't duplicate existing information
2. **Choose the most specific applicable file** - More specific files take precedence
3. **Reference related files** - Use cross-references to maintain connections
4. **Avoid fragmentation** - Keep related concepts together

### Examples of Proper Placement

#### ✅ **Correct Placements**

- **Tag filtering logic** → `docs/filtering-system.md` (functionality-oriented)
- **Date and timezone processing** → `docs/datetime-processing.md` (functionality-oriented)
- **How to create a plugin** → `_plugins/AGENTS.md` (domain-specific)
- **Development patterns** → Domain-specific `AGENTS.md` files
- **Basic site concepts** → `docs/terminology.md` (foundation)

#### ❌ **Incorrect Placements**

- **Plugin implementation details** → `AGENTS.md` (root) (too generic, should be in `_plugins/AGENTS.md`)
- **Code examples** → `docs/site-overview.md` (wrong level of detail)
- **Specific API usage** → `docs/terminology.md` (too detailed for foundation)

## Cross-Reference Requirements

### Linking Standards

- **Always reference more detailed files** from higher-level files
- **Use consistent linking format**: Use standard markdown link syntax with square brackets and parentheses
- **Provide context** when referencing: Example - "See [_plugins/AGENTS.md](../_plugins/AGENTS.md) for implementation details"
- **Avoid circular references** - maintain clear hierarchy flow

### Reference Patterns

```markdown
# Good: Contextual reference

For specific implementation details, see [Jekyll Development](jekyll-development.md).

# Good: Cross-reference with purpose


# Bad: Vague reference

See other docs for more info.

# Bad: Circular reference

This file references another file that references back to this file.
```

## File Naming Conventions

### Consistent Naming

- Use **descriptive names** that clearly indicate content: `content-management.md` not `content.md`
- Use **specific suffixes** when appropriate: `-guidelines.md`, `-development.md`

### Directory Structure

For the complete documentation file list and descriptions, see [docs/AGENTS.md](AGENTS.md#documentation-files).

For framework-specific development guidelines (PowerShell, JavaScript, Ruby, etc.), see the respective AGENTS.md files in each directory.

## Content Organization Within Files

### Standard File Structure

```markdown
# Title

Brief description of what this document covers.

## Overview

High-level summary of the topic.

## Main Sections

### Subsection 1

Content organized logically.

### Subsection 2

More content with clear headings.

### Heading Guidelines

- Use **descriptive headings** that clearly indicate content
- Follow **logical hierarchy**: H1 → H2 → H3 → H4
- Avoid **duplicate headings** within the same file
- Use **consistent formatting**: Title case for main headings

## Documentation Maintenance

### Regular Reviews

- **Quarterly reviews** of all documentation for accuracy
- **Update cross-references** when files are added or restructured
- **Remove outdated content** that no longer applies
- **Consolidate** fragmented information when appropriate

### Version Control

- **Commit documentation changes** with descriptive messages
- **Review documentation updates** as part of code review process
- **Track documentation debt** in issues when updates are needed

## Quality Standards

### Writing Standards

Follow the comprehensive guidelines in [Writing Style Guidelines](writing-style-guidelines.md) for:

- **Tone and voice**: Down-to-earth, authentic, and professional
- **Language standards**: Clear, concise, and actionable content
- **Content to avoid**: Exaggerated language, buzzwords, and marketing speak
- **Content to embrace**: Specific examples, practical benefits, and honest assessments

**Key principles from the writing style guidelines:**

- **Clear, professional English** without jargon when possible
- **Consistent terminology** across all documentation
- **Active voice** preferred over passive voice
- **Concise explanations** that get to the point quickly

### Technical Accuracy

- **Test all code examples** before including them
- **Verify all links** work correctly
- **Update screenshots** when UI changes
- **Maintain consistency** with actual implementation

### Accessibility

- **Descriptive link text** rather than "click here"
- **Alt text for images** when they convey important information
- **Logical heading structure** for screen readers
- **Clear table headers** when using tables

### Index Update Process

1. **Add new files** to the appropriate section in this file (`documentation-guidelines.md`)
2. **Update file descriptions** to reflect current content
3. **Maintain reading order** recommendations for new users
4. **Cross-reference** related files appropriately

For the current documentation index structure, see the [Directory Structure](#directory-structure) section in this file.

## Current Documentation Index

### 📚 Start Here (Recommended Reading Order)

- **[documentation-guidelines.md](documentation-guidelines.md)** - *Documentation structure, hierarchy, and content placement rules*
- **[terminology.md](terminology.md)** - *Essential concepts and definitions*
- **[site-overview.md](site-overview.md)** - *Site architecture and how everything fits together*

### 🛠️ Development & Technical

- **[jekyll-development.md](jekyll-development.md)** - *Jekyll patterns, Liquid templating, development setup*
- **[filtering-system.md](filtering-system.md)** - *Tag and date filtering implementation*
- **[datetime-processing.md](datetime-processing.md)** - *Date handling, timezone configuration, and custom date filters*

### 📝 Content & Management

- **[content-management.md](content-management.md)** - *Creating and managing content*
- **[markdown-guidelines.md](markdown-guidelines.md)** - *Markdown formatting standards*

### ⚡ Performance & Standards

- **[performance-guidelines.md](performance-guidelines.md)** - *Performance optimization and best practices*
- **[writing-style-guidelines.md](writing-style-guidelines.md)** - *Writing tone, style, and language standards*

## File Descriptions

| File | Purpose | When to Read |
|------|---------|-------------|
| **documentation-guidelines.md** | Documentation structure and content placement rules | Understanding how to organize and place documentation |
| **terminology.md** | Definitions and basic concepts | Understanding the system fundamentals |
| **site-overview.md** | High-level architecture and structure | Getting oriented with the site |
| **jekyll-development.md** | Jekyll/Liquid development patterns | Writing Jekyll templates and code |
| **filtering-system.md** | Tag/date filtering implementation | Working with content filtering |
| **datetime-processing.md** | Date handling, timezone configuration, and custom date filters | Working with dates and timezone processing |
| **content-management.md** | Content creation workflows and RSS feed processing | Adding and managing content |
| **markdown-guidelines.md** | Markdown formatting standards | Writing and formatting content |
