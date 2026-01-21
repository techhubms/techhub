# TechHub.Core Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Core/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).
> **RULE**: All rules from parent AGENTS.md files apply. This file adds Core layer-specific patterns.

## Overview

This project contains domain models, DTOs, and interfaces with **zero external dependencies** (pure .NET). It defines the core business entities and contracts that all other layers depend on.

**When to read this file**: When creating or modifying domain models, DTOs, repository interfaces, or understanding the core domain.

**Testing this code**: See [tests/TechHub.Core.Tests/AGENTS.md](../../tests/TechHub.Core.Tests/AGENTS.md) for unit testing patterns.

## Project Structure

```text
TechHub.Core/
├── Models/                       # Domain entities
│   ├── Section.cs               # Section aggregate
│   ├── ContentItem.cs           # Content item entity
│   ├── Collection.cs            # Collection reference
│   └── FrontMatter.cs           # Markdown frontmatter
├── DTOs/                         # Data transfer objects
│   ├── SectionDto.cs            # Section DTO
│   ├── ContentItemDto.cs        # Content item DTO
│   └── CollectionDto.cs         # Collection DTO
├── Interfaces/                   # Repository contracts
│   ├── ISectionRepository.cs    # Section data access
│   ├── IContentRepository.cs    # Content data access
│   ├── IMarkdownService.cs      # Markdown processing
│   └── IRssService.cs           # RSS feed generation
└── TechHub.Core.csproj          # Project file (no dependencies!)
```

## Core Principles

### No External Dependencies

**CRITICAL**: TechHub.Core must remain framework-agnostic:

- ✅ **Use**: `System.*` namespaces only
- ✅ **Use**: Standard .NET types (string, int, DateTime, etc.)
- ✅ **Use**: Collections (`IReadOnlyList<T>`, `List<T>`, `Dictionary<K,V>`)
- 🚫 **NEVER**: Add NuGet packages or framework dependencies
- 🚫 **NEVER**: Reference ASP.NET Core, Entity Framework, or other frameworks

**Why**: Keeps domain logic clean, testable, and reusable across any application type.

### Immutability by Default

**Use `init` accessors and `required` properties** for all domain models:

- Properties use `init` instead of `set`
- Mark mandatory fields with `required`
- Use `IReadOnlyList<T>` for collections

**Benefits**:

- Thread-safe by design
- Prevents accidental mutations
- Clear intent (values set once at creation)
- Works perfectly with record types

**See**: [Models/Section.cs](Models/Section.cs) for implementation

## Domain Model Patterns

### Entity Design

**Use classes for entities with identity** (can be mutated over lifetime).

**Example**: `Section`, `ContentItem`

**Key Properties**:

- `required` for mandatory fields
- `init` for immutability after construction
- Validation methods for business rules
- Avoid public setters

### Value Object Pattern

**Use records for value objects** (identity defined by values, not reference).

**Example**: `Collection`, `TagCloudItem`

**Benefits**: Value equality, immutability, concise syntax, built-in methods.

### Content Item Model

**Complex domain entity** with many properties representing content from markdown files.

**Critical Properties**:

- `Slug`: URL-friendly identifier
- `DateEpoch`: Unix timestamp for dates
- `SectionNames`: Lowercase section identifiers (mapped from frontmatter `categories`)
- `Tags`: Normalized lowercase tags
- `ViewingMode`: "internal" or "external"

See [Markdown Frontmatter Mapping](#markdown-frontmatter-mapping) for complete field mappings.

## Markdown Frontmatter Mapping

**Critical reference** for understanding how markdown frontmatter maps to domain properties:

**Markdown File Example**:

```markdown
---
title: "Getting Started with GitHub Copilot"
author: "John Doe"
date: 2026-01-07
categories: [ai, github-copilot]
tags: [machine-learning, productivity, vscode]
external_url: "https://example.com/original-article"
viewing_mode: "external"
video_id: "dQw4w9WgXcQ"
---

This is the excerpt that appears in content lists and RSS feeds.

<!--excerpt_end-->

# Full Article Content

The rest of the markdown content...
```

**Property Mappings**:

| Frontmatter Field           | Domain Property  | Type                    | Notes                                                                                   |
| --------------------------- | ---------------- | ----------------------- | --------------------------------------------------------------------------------------- |
| `title`                     | `Title`          | `string`                | Required                                                                                |
| `author`                    | `Author`         | `string?`               | Optional                                                                                |
| `date`                      | `DateEpoch`      | `long`                  | Converted to Unix timestamp in Europe/Brussels timezone                                 |
| `categories`                | `SectionNames`   | `IReadOnlyList<string>` | Frontmatter contains Section Titles ("AI"), mapped to lowercase section names ("ai")    |
| `tags`                      | `Tags`           | `IReadOnlyList<string>` | Normalized to lowercase, hyphen-separated                                               |
| `external_url`              | `ExternalUrl`    | `string?`               | Original source URL                                                                     |
| `viewing_mode`              | `ViewingMode`    | `string`                | "internal" or "external" (default: "external")                                          |
| `video_id`                  | `VideoId`        | `string?`               | YouTube video identifier                                                                |
| Filename                    | `Slug`           | `string`                | `2025-01-15-article.md` → `2025-01-15-article`                                          |
| Before `<!--excerpt_end-->` | `Excerpt`        | `string`                | Plain text, max 200 words                                                               |
| Full markdown               | `RenderedHtml`   | `string`                | Processed with Markdig                                                                  |

**See [src/TechHub.Infrastructure/AGENTS.md](../TechHub.Infrastructure/AGENTS.md)** for implementation details of frontmatter parsing.

## DTO Patterns

**Use DTOs for API responses and client communication**:

**DTO Structure**:

- Use `record` types for value equality and immutability
- Mark mandatory fields with `required`
- Use `init` accessors
- Add XML documentation comments

**Key DTOs**:

- `SectionDto` - Section data with collections ([DTOs/SectionDto.cs](DTOs/SectionDto.cs))
- `ContentItemDto` - Content item with metadata ([DTOs/ContentItemDto.cs](DTOs/ContentItemDto.cs))
- `CollectionDto` - Collection reference ([DTOs/CollectionDto.cs](DTOs/CollectionDto.cs))

**DTO vs Domain Model**:

- **Domain Model**: Rich behavior, validation, business rules
- **DTO**: Data transfer only, no behavior, optimized for serialization
- **Mapping**: Use extension methods or mapping libraries (AutoMapper, Mapster)

## Repository Interfaces

**Define contracts for data access**:

**Key Repository Interfaces**:

- `ISectionRepository` - Section data access ([Interfaces/ISectionRepository.cs](Interfaces/ISectionRepository.cs))
- `IContentRepository` - Content item data access ([Interfaces/IContentRepository.cs](Interfaces/IContentRepository.cs))
- `IMarkdownService` - Markdown processing ([Interfaces/IMarkdownService.cs](Interfaces/IMarkdownService.cs))
- `IRssService` - RSS feed generation ([Interfaces/IRssService.cs](Interfaces/IRssService.cs))

**Repository Method Patterns**:

- All methods async with `CancellationToken ct = default`
- Return `Task<IReadOnlyList<T>>` for collections
- Return `Task<T?>` for single items (null if not found)
- Include XML documentation for each method

**CRITICAL Repository Contract**:

- All `IContentRepository` methods **MUST** return content sorted by `DateEpoch` descending (newest first)
- This sorting happens at repository layer, before caching
- Clients should never need to sort content themselves

**See**: [Interfaces/](Interfaces/) for complete interface definitions

## URL Generation Methods

**ContentItem includes URL generation methods**:

**`GetUrlInSection(string sectionName)`**:

- Generates full URL path for content item
- Pattern: `/{sectionName}/{collectionName}/{slug}`
- Validates section name exists in item's `SectionNames`
- Throws `ArgumentException` if section not found

**Example**: `GetUrlInSection("github-copilot")` → `"/github-copilot/blogs/2024-01-15-article"`

**See**: [Models/ContentItem.cs](Models/ContentItem.cs) for implementation

## Unix Epoch Timestamp Usage

**Always use Unix epoch for date storage and manipulation**:

**Storage**:

```csharp
public required long DateEpoch { get; init; }  // Seconds since Unix epoch
```

**Conversion Examples**:

- **From DateTime**: `new DateTimeOffset(date).ToUnixTimeSeconds()`
- **To DateTime**: `DateTimeOffset.FromUnixTimeSeconds(epochSeconds).DateTime`
- **From frontmatter**: See [src/TechHub.Infrastructure/AGENTS.md](../TechHub.Infrastructure/AGENTS.md) for parsing logic

**Why Unix Epoch?**:

- Timezone-agnostic storage (always UTC internally)
- Efficient comparison and sorting
- Compact representation (single long value)
- Standard format across all platforms

## DTO Conversion Extensions

**Pattern**: Use extension methods for converting domain models to DTOs

**Extension Method Structure**:

- `ToDto(this ContentItem)` - Convert single item
- `ToDtos(this IEnumerable<ContentItem>)` - Convert collection
- Map all properties from domain model to DTO
- Handle nullable properties appropriately

**Usage Examples**:

- **In repositories**: `return item?.ToDto();`
- **In API endpoints**: `Results.Ok(item.ToDto())`
- **For collections**: `items.ToDtos()`

**See**: [Extensions/ContentItemExtensions.cs](Extensions/ContentItemExtensions.cs) for implementation

## Validation Patterns

**Add validation methods to domain models**:

**Validation Method Pattern**:

- Method name: `Validate()`
- Throws `ArgumentException` for invalid state
- Validates required fields are not empty
- Validates numeric fields are in valid range
- Validates enum-like fields have allowed values

**Common Validations**:

- String fields: Check `IsNullOrWhiteSpace`
- Numeric fields: Check positive values
- Collections: Check minimum count
- Enums: Check allowed values

**See**: [Models/ContentItem.cs](Models/ContentItem.cs) `Validate()` method for implementation

## Testing

**See [tests/TechHub.Core.Tests/AGENTS.md](../../tests/TechHub.Core.Tests/AGENTS.md)** for comprehensive unit testing patterns for domain models and business logic.

## Related Documentation

- **[src/AGENTS.md](../AGENTS.md)** - Shared .NET patterns and code quality standards
- **[src/TechHub.Infrastructure/AGENTS.md](../TechHub.Infrastructure/AGENTS.md)** - Repository implementations that use these interfaces
- **[src/TechHub.Api/AGENTS.md](../TechHub.Api/AGENTS.md)** - API endpoints that consume these models
- **[tests/TechHub.Core.Tests/AGENTS.md](../../tests/TechHub.Core.Tests/AGENTS.md)** - Unit testing patterns
- **[Root AGENTS.md](../../AGENTS.md)** - Complete workflow and principles

---

**Remember**: Core layer stays clean and framework-agnostic. No dependencies, just pure domain logic.
