# SectionPriorityHelper Migration Plan

## Current State

**SectionPriorityHelper** is a runtime helper that determines the "primary section" for content items based on section priority rules.

### Current Usage

1. **FileBasedContentRepository** - Computes `PrimarySectionName` at runtime
2. **API Endpoints** (`ContentEndpoints.cs`, `SectionsEndpoints.cs`) - Computes `PrimarySectionName` for API responses
3. **Runtime overhead** - Logic executed on every content item load

### Problems

- Runtime computation on every request
- Logic duplicated across codebase
- Section priority hardcoded in C# (should be in content)
- Harder to test and validate

## Migration Strategy

### Phase 1: TechHub.ContentFixer Updates

Add `primary_section` frontmatter field generation to TechHub.ContentFixer:

```csharp
// In content processing logic
var primarySection = SectionPriorityHelper.GetPrimarySectionName(categories, collectionName);

// Add to frontmatter
frontmatter.Add("primary_section", primarySection);
```

**Result**: All markdown files get `primary_section` field

### Phase 2: Update Models

Change `ContentItem.PrimarySectionName` from computed to stored:

```csharp
// Before (computed)
PrimarySectionName = SectionPriorityHelper.GetPrimarySectionName(item.SectionNames, item.CollectionName)

// After (stored from frontmatter)
PrimarySectionName = frontmatter["primary_section"]
```

### Phase 3: Remove Runtime Computation

Delete:

- `SectionPriorityHelper.cs`
- `SectionPriorityHelperTests.cs`
- All runtime calls to `SectionPriorityHelper`

Update:

- `FileBasedContentRepository.cs` - Read `primary_section` from frontmatter
- `ContentEndpoints.cs` - Remove runtime computation
- `SectionsEndpoints.cs` - Remove runtime computation

### Phase 4: Validation

- Run TechHub.ContentFixer on all collections
- Verify all files have `primary_section` field
- Test that content loads correctly
- Verify primary section sorting works

## Benefits After Migration

✅ **Zero runtime overhead** - Primary section pre-computed during content processing
✅ **Single source of truth** - Frontmatter contains all metadata
✅ **Easier testing** - Just validate frontmatter, no runtime logic
✅ **Simpler codebase** - Remove helper class and all its usages
✅ **Content-driven** - Section priority rules in TechHub.ContentFixer where content processing happens

## Rollback Plan

If migration causes issues:

1. Keep `SectionPriorityHelper` as fallback
2. Use `frontmatter["primary_section"] ?? SectionPriorityHelper.GetPrimarySectionName(...)`
3. Fix content files incrementally
4. Remove fallback once all files migrated

## Timeline

- **Week 1**: Update TechHub.ContentFixer
- **Week 2**: Run content fixer on all collections, verify output
- **Week 3**: Update models and repositories
- **Week 4**: Remove SectionPriorityHelper and cleanup
