# Documentation Agent

## Overview

You are a documentation specialist for the Tech Hub project. This directory contains comprehensive technical documentation covering all aspects of the site architecture, development practices, and operational procedures.

## Documentation Files

```text
docs/
├── AGENTS.md                   # Documentation guidelines (this file)
├── content-management.md       # Content creation, organization, and RSS processing
├── datetime-processing.md      # Date handling and timezone configuration
├── documentation-guidelines.md # Documentation structure and hierarchy
├── filtering-system.md         # Tag-based filtering implementation
├── jekyll-development.md       # Jekyll-specific patterns
├── markdown-guidelines.md      # Markdown structure for AI models
├── performance-guidelines.md   # Performance optimization
├── site-overview.md            # Site structure overview
├── terminology.md              # Project vocabulary
└── writing-style-guidelines.md # Content writing standards
```

## Documentation Philosophy

**CRITICAL**: Whenever code changes any behavior, you MUST update the relevant documentation files to reflect those changes. Never leave documentation out of sync with the codebase.

**CRITICAL**: When creating or updating documentation, update [documentation-guidelines.md](documentation-guidelines.md) to include new content. For code-specific domain guidance, update the relevant AGENTS.md file in the appropriate directory.

### Audience

Documentation serves multiple audiences:

1. **Developers** - Contributing to the codebase
2. **AI Assistants** - Understanding project structure
3. **Content Creators** - Adding articles and posts
4. **DevOps Engineers** - Managing infrastructure

### Principles

- **Accuracy**: Always reflect actual implementation
- **Completeness**: Cover all aspects of topic
- **Clarity**: Use clear, concise language
- **Examples**: Include practical code examples
- **Maintenance**: Keep in sync with codebase
- **Minimize duplication**: Content appears in one authoritative location
- **Cross-references**: Use links to connect related information
- **Heading levels**: Maximum 3 levels (#, ##, ###)

## Documentation Standards

### File Structure

Each documentation file should follow this structure:

```markdown
# Title

Brief overview paragraph explaining the topic.

## Section 1

Content with examples and explanations.

### Subsection

More detailed information.

## Section 2

Additional topics.

## Resources

Links to related documentation.

## Never Do

Anti-patterns and common mistakes to avoid.
```

### Writing Style

Follow [writing-style-guidelines.md](writing-style-guidelines.md):

- Use active voice
- Be concise and direct
- Use present tense for current features
- Include code examples
- Use consistent terminology (see [terminology.md](terminology.md))

### Code Examples

```markdown
<!-- Use syntax highlighting -->
```javascript
// ✅ CORRECT: Good example with comments
const result = processData(input);
```

```javascript
// ❌ WRONG: Anti-pattern with explanation
var data = undefined; // Never use var or undefined
```
```

### Cross-References

Link related documentation:

```markdown
For more details, see [filtering-system.md](filtering-system.md).
```

## Key Documentation Files

### Site Overview

**[site-overview.md](site-overview.md)** - Comprehensive site structure:
- Collections and sections
- Content organization
- Navigation structure
- Data files

### Filtering System

**[filtering-system.md](filtering-system.md)** - Complete filtering implementation:
- Tag-based architecture
- Pre-calculated relationships
- Client-side filtering
- Performance optimization

### Content Management

**[content-management.md](content-management.md)** - Content lifecycle:
- Creating content
- Frontmatter structure
- Collection organization
- Publication workflow

## Documentation Maintenance

### When to Update

Update documentation when:

1. **Code changes** - Behavior modifications
2. **New features** - Additional functionality
3. **Deprecations** - Removing old features
4. **Bug fixes** - Correcting documented behavior
5. **Architecture changes** - Structural modifications

### Update Process

1. Identify affected documentation files
2. Update relevant sections
3. Verify cross-references still valid
4. Test code examples still work
5. Commit with descriptive message

### Review Checklist

- [ ] Accuracy: Reflects current implementation
- [ ] Completeness: Covers all aspects
- [ ] Examples: Code examples tested
- [ ] Links: Cross-references valid
- [ ] Style: Follows writing guidelines
- [ ] Terminology: Consistent vocabulary

## Common Documentation Patterns

### Feature Documentation

```markdown
## Feature Name

**Purpose**: Brief description of what this feature does.

**Location**: `path/to/implementation`

**Key Concepts**:
- Concept 1
- Concept 2

### Usage

```language
// Example code
```

### Configuration

Explain configuration options.

### Best Practices

List recommended approaches.

## Never Do

Anti-patterns to avoid.
```

### API Documentation

```markdown
### Function Name

**Purpose**: What it does

**Parameters**:
- `param1` (type): Description
- `param2` (type): Description

**Returns**: Return value description

**Example**:

```language
result = functionName(param1, param2);
```

**Throws**: Error conditions
```

### Process Documentation

```markdown
## Process Name

**Overview**: What this process accomplishes.

**Steps**:

1. **Step 1**: Action description
   ```bash
   # Command example
   ```

2. **Step 2**: Next action
   ```bash
   # Command example
   ```

**Expected Output**:

Describe successful completion.

**Troubleshooting**:

Common issues and solutions.
```

## Documentation Tools

### Markdown Linting

Follow guidelines in [markdown-guidelines.md](markdown-guidelines.md):
- Use ATX-style headers (`#` not underlines)
- Consistent list formatting
- Code blocks with language tags
- No trailing whitespace

### Link Validation

Verify internal links:
```bash
# Check for broken links
grep -r "\[.*\](.*.md)" docs/
```

### Example Testing

Test code examples periodically:
- PowerShell: Run examples in scripts
- JavaScript: Test in browser console
- Ruby: Execute in IRB
- Liquid: Test in Jekyll templates

## Content Placement

For detailed guidelines on where to place documentation content, see [documentation-guidelines.md](documentation-guidelines.md).

**Quick Reference**:

- **Generic rules** stay in `docs/`
- **Language-specific code** goes to domain AGENTS.md files
- **Date/timezone concepts** → `docs/datetime-processing.md`
- **JavaScript date code** → `assets/js/AGENTS.md`
- **Ruby date code** → `_plugins/AGENTS.md`

## Never Do

- Never let documentation diverge from code
- Never use outdated examples
- Never skip cross-reference updates
- Never assume readers know context
- Never use vague language ("might", "probably")
- Never include untested code examples
- Never duplicate content (link instead)
- Never use relative dates ("last week", "recently")
