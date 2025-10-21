# Markdown and YAML Parsers

## Purpose

This folder contains parsers for processing Jekyll-style markdown files with YAML frontmatter.

## Parser Implementations

### YamlFrontmatterParser

**Purpose**: Parses YAML frontmatter from markdown files following Jekyll conventions.

**Functionality**:
- Extracts YAML between `---` delimiters at file start
- Deserializes YAML to strongly-typed objects
- Handles Jekyll naming conventions (underscore_case)
- Provides type-safe value extraction methods

**Usage**:
```csharp
var parser = new YamlFrontmatterParser();
var (frontmatter, content) = parser.Parse(markdownText);

// Extract typed values
var title = parser.GetString(frontmatter, "title");
var tags = parser.GetStringList(frontmatter, "tags");
var date = parser.GetDate(frontmatter, "date");
```

**Error Handling**:
- Returns `(null, originalContent)` if no frontmatter found
- Throws `InvalidOperationException` for malformed YAML
- Returns `default` for missing or invalid type conversions

### MarkdownParser

**Purpose**: Processes markdown content and extracts structured information.

**Functionality**:
- Extracts excerpts before `<!--excerpt_end-->` separator
- Separates main content from excerpts
- Strips markdown formatting for search indexing
- Creates searchable content strings

**Usage**:
```csharp
var parser = new MarkdownParser();

// Extract structured content
var excerpt = parser.ExtractExcerpt(content);
var mainContent = parser.ExtractMainContent(content);

// Create searchable text
var searchable = parser.CreateSearchableContent(
    title, description, author, content, tags);

// Strip formatting
var plainText = parser.StripMarkdown(markdown);
```

**Markdown Stripping**:
- Removes HTML tags
- Removes headers (#, ##, ###)
- Removes bold/italic markers (*_, **, _*)
- Extracts link text `[text](url)` → `text`
- Removes code blocks and inline code
- Normalizes whitespace

## Integration with MarkdownContentRepository

These parsers are used by `MarkdownContentRepository` to:

1. **Read markdown files** from collection directories
2. **Parse frontmatter** to extract metadata (title, date, author, categories, tags)
3. **Extract content** separating excerpt from main content
4. **Create ContentItem** objects with proper type (NewsArticle, BlogPost, Video, etc.)
5. **Build search index** using stripped markdown content

## Testing Strategy

### Unit Tests

Test parsers in isolation:
- Frontmatter parsing with various YAML formats
- Excerpt extraction with different separator patterns
- Markdown stripping with complex formatting
- Type conversion edge cases

### Integration Tests

Test with real markdown files:
- Parse actual content from `_news/`, `_posts/`, etc.
- Verify all frontmatter fields extracted correctly
- Ensure content structure preserved

## Dependencies

- **YamlDotNet**: YAML parsing library (v16.3.0+)
- **System.Text.RegularExpressions**: Pattern matching for content extraction

## Performance Considerations

**Compiled Regex**:
- All regex patterns use `RegexOptions.Compiled` for performance
- Patterns compiled once and reused across many files

**Memory Efficiency**:
- Parsers are stateless - no caching needed
- Can be used as singletons via DI
- Minimal allocations during parsing

**Caching Strategy**:
- Parsing results cached in `ContentCache` (not in parsers)
- Parsers focus on single responsibility: parsing
- Caching handled at repository level

## Registration in DI Container

```csharp
// Register as singletons - they're stateless
builder.Services.AddSingleton<YamlFrontmatterParser>();
builder.Services.AddSingleton<MarkdownParser>();
```

## Error Handling Best Practices

**YamlFrontmatterParser**:
- Invalid YAML: Throw `InvalidOperationException` with context
- Missing frontmatter: Return `(null, content)` - not an error
- Type mismatch: Return `default` - allow caller to handle

**MarkdownParser**:
- Invalid markdown: Strip what we can, ignore rest
- No excerpt separator: Return empty excerpt, full content as main
- Null/empty input: Return empty strings - don't throw

## Future Enhancements

Potential improvements for Phase 6:

1. **Custom YAML Types**: Support custom deserializers for complex frontmatter
2. **Markdown Extensions**: Parse GitHub Flavored Markdown features
3. **Validation**: Add frontmatter schema validation
4. **Performance**: Benchmark and optimize for large file sets
5. **Streaming**: Support streaming parsing for very large files
