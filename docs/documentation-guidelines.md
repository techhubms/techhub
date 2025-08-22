# Documentation Guidelines

This document defines the documentation structure, hierarchy, and content placement guidelines for the Tech Hub project.

## Documentation Philosophy

**CRITICAL**: Whenever code changes any behavior, you MUST update the relevant documentation files to reflect those changes. This requirement applies to all contributors and to Copilot automation. Never leave documentation out of sync with the codebase. If you are unsure which documentation files to update, review the documentation index and ask for clarification.

**CRITICAL**: When creating or updating documentation, always update this file and the `.github/coding-instructions.md` file to include new content or make existing content up-to-date.

The documentation follows a hierarchical approach designed to:

- **Minimize duplication**: Content appears in one authoritative location
- **Maximize discoverability**: Clear navigation paths and cross-references
- **Optimize for different needs**: AI models first, human developers second
- **Maintain consistency**: Standardized formatting and reference patterns

Key principles:

- Follow the established documentation hierarchy (Foundation → Functionality → Task → Language → Performance)
- Place content in the most specific applicable file
- Use cross-references to connect related information
- Avoid duplication across documentation files
- Keep the amount of levels to a maximum of 3, so #, ## and ###

## Documentation Hierarchy

When creating or updating documentation, follow this strict hierarchy to ensure content is placed in the most appropriate location. **Always check files in this order** and place content where it best fits according to the file's purpose:

### 1. Generic AI Instructions

- **`.github/copilot-instructions.md`**: Only for generic AI model instructions that apply universally
- **Not for**: Specific implementation details, project-specific guidance, or technical procedures

### 2. Foundation Files

- **`docs/documentation-guidelines.md`**: Documentation structure, hierarchy, and content placement rules
- **`docs/terminology.md`**: Definitions, concepts, and basic understanding
- **`docs/site-overview.md`**: High-level architecture and system structure
- **Purpose**: Establish fundamental understanding before diving into specifics

### 3. Functionality-Oriented Files

- **`docs/filtering-system.md`**: Complete implementation of date and tag filtering systems
- **`docs/datetime-processing.md`**: Date handling, timezone configuration, and custom date filters
- **`docs/content-management.md`**: Content creation, organization, and lifecycle management
- **`docs/rss-feeds.md`**: RSS feed integration and automated content processing
- **`docs/testing-guidelines.md`**: Ruby unit tests and Playwright end-to-end testing
- **Purpose**: Explain **what** the system does and **how** it works

### 4. Task-Oriented Files

- **`docs/plugins.md`**: Creating and maintaining Jekyll plugins
- **`docs/jekyll-development.md`**: Jekyll-specific development patterns and practices
- **`docs/github-token-setup.md`**: GitHub token configuration and authentication setup
- **Purpose**: Explain **how to do** specific development tasks

### 5. Language-Oriented Files

- **`docs/javascript-guidelines.md`**: JavaScript development standards and patterns
- **`docs/powershell-guidelines.md`**: PowerShell scripting conventions
- **`docs/ruby-guidelines.md`**: Ruby development standards and best practices
- **`docs/markdown-guidelines.md`**: Markdown formatting and content standards
- **`docs/frontend-guidelines.md`**: Frontend development standards and practices
- **`docs/writing-style-guidelines.md`**: Content writing style and tone guidelines
- **Purpose**: Language-specific syntax, conventions, and best practices

### 6. Performance and Optimization

- **`docs/performance-guidelines.md`**: Optimization strategies and performance considerations

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
- **How to create a plugin** → `docs/plugins.md` (task-oriented)
- **JavaScript coding standards** → `docs/javascript-guidelines.md` (language-oriented)
- **Basic site concepts** → `docs/terminology.md` (foundation)
- **Test execution workflows** → `docs/testing-guidelines.md` (functionality-oriented)

#### ❌ **Incorrect Placements**

- **Plugin implementation details** → `copilot-instructions.md` (too specific for generic instructions)
- **JavaScript code examples** → `docs/site-overview.md` (wrong level of detail)
- **Specific API usage** → `docs/terminology.md` (too detailed for foundation)

## Cross-Reference Requirements

### Linking Standards

- **Always reference more detailed files** from higher-level files
- **Use consistent linking format**: Use standard markdown link syntax with square brackets and parentheses
- **Provide context** when referencing: Example - "See [Plugins documentation](plugins.md) for implementation details"
- **Avoid circular references** - maintain clear hierarchy flow

### Reference Patterns

```markdown
# Good: Contextual reference

For specific implementation details, see [Jekyll Development](jekyll-development.md).

# Good: Cross-reference with purpose

Testing procedures are covered in [Testing Guidelines](testing-guidelines.md).

# Bad: Vague reference

See other docs for more info.

# Bad: Circular reference

This file references another file that references back to this file.
```

## File Naming Conventions

### Consistent Naming

- Use **kebab-case** for all documentation files: `testing-guidelines.md`
- Use **descriptive names** that clearly indicate content: `content-management.md` not `content.md`
- Use **specific suffixes** when appropriate: `-guidelines.md`, `-development.md`

### Directory Structure

```text
docs/
├── terminology.md               # Foundation: Definitions and concepts
├── site-overview.md            # Foundation: Architecture overview
├── documentation-guidelines.md # Foundation: Documentation structure and hierarchy
├── content-management.md       # Functionality: Content workflows
├── datetime-processing.md      # Functionality: Date and timezone handling
├── filtering-system.md         # Functionality: Filtering implementation
├── rss-feeds.md               # Functionality: RSS processing
├── testing-guidelines.md      # Functionality: Testing infrastructure
├── plugins.md                 # Task: Plugin development
├── jekyll-development.md      # Task: Jekyll development
├── javascript-guidelines.md   # Language: JavaScript standards
├── powershell-guidelines.md   # Language: PowerShell standards
├── ruby-guidelines.md         # Language: Ruby standards
├── markdown-guidelines.md     # Language: Markdown standards
├── frontend-guidelines.md     # Language: Frontend development standards
├── writing-style-guidelines.md # Language: Content writing standards
├── performance-guidelines.md  # Performance: Optimization strategies
└── github-token-setup.md      # Task: GitHub token configuration
```

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

1. **Add new files** to the appropriate section in `docs/README.md`
2. **Update file descriptions** to reflect current content
3. **Maintain reading order** recommendations for new users
4. **Cross-reference** related files appropriately

For the current documentation index structure, see [docs/README.md](README.md).

## Current Documentation Index

### 📚 Start Here (Recommended Reading Order)

- **[documentation-guidelines.md](documentation-guidelines.md)** - *Documentation structure, hierarchy, and content placement rules*
- **[terminology.md](terminology.md)** - *Essential concepts and definitions*
- **[site-overview.md](site-overview.md)** - *Site architecture and how everything fits together*

### 🛠️ Development & Technical

- **[jekyll-development.md](jekyll-development.md)** - *Jekyll patterns, Liquid templating, development setup*
- **[plugins.md](plugins.md)** - *Complete plugin architecture and development guide*
- **[filtering-system.md](filtering-system.md)** - *Tag and date filtering implementation*
- **[datetime-processing.md](datetime-processing.md)** - *Date handling, timezone configuration, and custom date filters*
- **[testing-guidelines.md](testing-guidelines.md)** - *Ruby unit tests and Playwright end-to-end testing*

### 📝 Content & Management

- **[content-management.md](content-management.md)** - *Creating and managing content*
- **[markdown-guidelines.md](markdown-guidelines.md)** - *Markdown formatting standards*

### ⚡ Performance & Standards

- **[performance-guidelines.md](performance-guidelines.md)** - *Performance optimization and best practices*
- **[javascript-guidelines.md](javascript-guidelines.md)** - *JavaScript development standards*
- **[powershell-guidelines.md](powershell-guidelines.md)** - *PowerShell scripting conventions*
- **[ruby-guidelines.md](ruby-guidelines.md)** - *Ruby development and Jekyll plugin standards*
- **[frontend-guidelines.md](frontend-guidelines.md)** - *Frontend development, CSS standards, and E2E testing*
- **[writing-style-guidelines.md](writing-style-guidelines.md)** - *Writing tone, style, and language standards*

### 🤖 AI & Development Tools

- **[rss-feeds.md](rss-feeds.md)** - *RSS feed configuration and management*

## File Descriptions

| File | Purpose | When to Read |
|------|---------|-------------|
| **documentation-guidelines.md** | Documentation structure and content placement rules | Understanding how to organize and place documentation |
| **terminology.md** | Definitions and basic concepts | Understanding the system fundamentals |
| **site-overview.md** | High-level architecture and structure | Getting oriented with the site |
| **jekyll-development.md** | Jekyll/Liquid development patterns | Writing Jekyll templates and code |
| **plugins.md** | Plugin system and development | Creating or modifying site functionality |
| **filtering-system.md** | Tag/date filtering implementation | Working with content filtering |
| **datetime-processing.md** | Date handling, timezone configuration, and custom date filters | Working with dates and timezone processing |
| **testing-guidelines.md** | Ruby unit tests and Playwright end-to-end testing | Running tests and writing new tests |
| **content-management.md** | Content creation workflows | Adding and managing content |
| **markdown-guidelines.md** | Markdown formatting standards | Writing and formatting content |
| **writing-style-guidelines.md** | Writing tone, style, and language standards | Creating any written content |
| **performance-guidelines.md** | Performance optimization rules | Optimizing site performance |
| **javascript-guidelines.md** | JavaScript development standards | Client-side development |
| **powershell-guidelines.md** | PowerShell scripting standards | Automation and scripting |
| **ruby-guidelines.md** | Ruby development and Jekyll plugin standards | Ruby/Jekyll plugin development |
| **frontend-guidelines.md** | Frontend development, CSS standards, and E2E testing | Frontend development and testing |
| **rss-feeds.md** | RSS feed management | Managing automated content |
