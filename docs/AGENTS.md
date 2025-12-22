# Documentation Agent

## Overview

You are a documentation specialist for the Tech Hub project. This directory contains comprehensive technical documentation covering all aspects of the site architecture, development practices, and operational procedures.

## Documentation Files

```text
docs/
├── content-management.md       # Content creation and organization
├── datetime-processing.md      # Date handling and timezone configuration
├── documentation-guidelines.md # Documentation structure and hierarchy
├── filtering-system.md         # Tag-based filtering implementation
├── frontend-guidelines.md      # Frontend development standards
├── github-token-setup.md       # GitHub token configuration
├── javascript-guidelines.md    # JavaScript development standards
├── jekyll-development.md       # Jekyll-specific patterns
├── markdown-guidelines.md      # Markdown formatting for AI models
├── performance-guidelines.md   # Performance optimization strategies
├── plugins.md                  # Jekyll plugin development
├── powershell-guidelines.md    # PowerShell syntax and standards
├── rss-feeds.md               # RSS feed processing
├── ruby-guidelines.md          # Ruby development standards
├── site-overview.md            # Site structure overview
├── terminology.md              # Project vocabulary and concepts
├── testing-guidelines.md       # Testing strategy and frameworks
└── writing-style-guidelines.md # Content writing standards
```

## Documentation Philosophy

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

### Testing Guidelines

**[testing-guidelines.md](testing-guidelines.md)** - Testing strategy:
- Test pyramid structure
- Framework selection (Pester, Jest, RSpec, Playwright)
- Critical testing rules
- Test organization

### Jekyll Development

**[jekyll-development.md](jekyll-development.md)** - Jekyll-specific patterns:
- Build process
- Plugin development
- Liquid templates
- Performance considerations

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

## Integration with AGENTS.md

Documentation and AGENTS.md files serve different purposes:

**Documentation** (`docs/*.md`):
- Comprehensive technical details
- Historical context
- Complete feature coverage
- Human-readable reference

**AGENTS.md**:
- Focused on specific domains
- Action-oriented guidance
- Critical rules and patterns
- AI assistant instructions

### Division of Content

| Content Type | Location | Purpose |
|--------------|----------|---------|
| Complete feature spec | `docs/` | Full details |
| Development patterns | `AGENTS.md` | Quick reference |
| Architecture overview | `docs/` | Understanding |
| Critical rules | `AGENTS.md` | Prevent errors |
| Historical decisions | `docs/` | Context |
| Current best practices | `AGENTS.md` | Daily use |

## Resources

### Style Guides

- [writing-style-guidelines.md](writing-style-guidelines.md) - Writing standards
- [markdown-guidelines.md](markdown-guidelines.md) - Markdown formatting
- [documentation-guidelines.md](documentation-guidelines.md) - Doc structure

### Technical References

- [terminology.md](terminology.md) - Vocabulary reference
- [site-overview.md](site-overview.md) - Architecture overview

### Development

- Language-specific AGENTS.md files for code examples
- [testing-guidelines.md](testing-guidelines.md) for test coverage

## Never Do

- Never let documentation diverge from code
- Never use outdated examples
- Never skip cross-reference updates
- Never assume readers know context
- Never use vague language ("might", "probably")
- Never include untested code examples
- Never duplicate content (link instead)
- Never use relative dates ("last week", "recently")
