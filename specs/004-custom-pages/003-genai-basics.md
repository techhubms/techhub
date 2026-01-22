# Custom Page: GenAI Basics

**Page**: GenAI Basics  
**URL**: `/ai/genai-basics`  
**Priority**: P2  
**Status**: Razor ❌ (Needs Update) | JSON ✅ | E2E Tests ❌  
**Estimated Effort**: 8-10 hours (remaining)

## Overview

GenAI Basics JSON data is complete with all mermaid diagrams, FAQ blocks, and resource links converted to simplified JSON structure with ID-based placeholders. Razor component needs update to use IMarkdownService for rendering.

**Live Site Reference**: <https://tech.hub.ms/ai/genai-basics>

## Current Status

✅ **Complete**:

- **JSON structure**: `collections/_custom/genai-basics.json` - **COMPLETE**
  - 13 sections (TOC excluded - generated dynamically)
  - 11 mermaid diagrams with ID-based placeholders (`{{mermaid:semantic-id}}`)
  - 2 FAQ blocks (8 Q&A pairs total) - markdown format
  - 10 More Info sections with resource links
  - All HTML converted to markdown
  - No `id` fields (generated dynamically from titles)
  - Document title extracted from frontmatter
- SidebarToc component integrated
- Conversion script: `.tmp/convert-genai-final.ps1` (parameterized, reusable)

❌ **Needs Implementation**:

- **Razor component update**: `src/TechHub.Web/Components/Pages/GenAIBasics.razor`
  - Inject IMarkdownService
  - Load JSON from genai-basics.json
  - Replace `{{mermaid:id}}` placeholders with actual diagrams
  - Render markdown to HTML using MarkdownService.RenderToHtml()
  - Generate TOC dynamically from section titles
  - Generate section IDs from titles

❌ **Missing Tests**:

- No E2E tests exist for GenAI Basics page

## Acceptance Criteria

### JSON Content Completeness ✅ COMPLETE

**Source**: `https://raw.githubusercontent.com/techhubms/techhub/main/sections/ai/genai-basics.md`

**Implementation Details**:

- **JSON Structure**: Simplified approach with single `content` field containing raw markdown + structured data
- **Mermaid Diagrams**: Stored as array with `{id, title, diagram}` objects, referenced via ID-based placeholders
- **Placeholder Pattern**: `{{mermaid:semantic-id}}` (e.g., `{{mermaid:foundations-1950s-1990s}}`)
- **FAQ Format**: Markdown-only (no HTML) - converted from HTML using `ConvertTo-Markdown` function
- **More Info Links**: Extracted as separate array with `{text, url}` objects
- **HTML Removal**: All HTML removed from content (only remains in mermaid syntax where appropriate)
- **ID Generation**: Semantic IDs from titles (lowercase, special chars removed, spaces to hyphens)

✅ **All 13 sections present** (TOC excluded - generated dynamically):

- History ✅
- ML vs AI vs GenAI ✅
- About Generative AI ✅
- Vendors ✅
- Models ✅
- Providers ✅
- Prompts & messages ✅
- Tokens & Tokenization ✅
- Next-token prediction: How LLMs generate text ✅
- Costs ✅
- Problems with models ✅
- When not to use AI ✅
- Societal impacts and risks ✅

✅ **11 Mermaid Diagrams** (validated):

- History section: 3 diagrams ✅
- ML vs AI vs GenAI: 1 diagram ✅
- Vendors: 1 diagram ✅
- Providers: 1 diagram ✅
- Tokens & Tokenization: 1 diagram ✅
- Next-token prediction: 2 diagrams ✅
- Costs: 1 diagram ✅
- Problems with models: 1 diagram ✅

✅ **2 FAQ Blocks** (8 Q&A pairs total):

- Models section: 3 questions ✅
- Providers section: 5 questions ✅

✅ **10 More Info sections** with resource links (validated)

### E2E Test Coverage

1. **Page Load & Rendering**
   - Navigate to `/ai/genai-basics`
   - Verify content renders without errors
   - Verify all 13 sections display

2. **SidebarToc Functionality**
   - TOC auto-extracts all 13 h2 headings
   - TOC links scroll to correct sections
   - Scroll spy highlights active section
   - Last section can scroll to detection point

3. **Content Rendering**
   - Mermaid diagrams render correctly (10 diagrams)
   - FAQ blocks display with questions/answers
   - Resource link blocks render
   - Code examples format correctly

4. **Interactive Elements**
   - Resource links are clickable
   - FAQ blocks are readable
   - Keyboard navigation works
   - No console errors

## Implementation Tasks

### Task 1: JSON Content Update (REQUIRED)

**File**: `collections/_custom/genai-basics.json`

**CRITICAL**: This is MANDATORY - JSON must match original markdown exactly

**Subtasks**:

1. **Fetch original markdown**:
   - Download from `https://raw.githubusercontent.com/techhubms/techhub/main/sections/ai/genai-basics.md`
   - Parse complete structure

2. **Add 10 Mermaid Diagrams**:
   - Extract mermaid code blocks from markdown
   - Format as JSON strings with `\n` for line breaks
   - Add to appropriate section objects
   - Example structure:

     ```json
     {
       "id": "history",
       "title": "History",
       "subsections": [
         {
           "title": "Foundations (1950s-1990s)",
           "mermaid": "graph LR\n    A[\"1956<br/>AI term coined\"] --> B[\"1966<br/>ELIZA chatbot\"]",
           "timeline": [...]
         }
       ]
     }
     ```

3. **Add 7 FAQ Blocks**:
   - Models section: Add FAQ object with 3 questions
   - Providers section: Add FAQ object with 4 questions
   - Structure:

     ```json
     {
       "id": "models",
       "faq": [
         {
           "question": "What is a GPT and why are not all models GPT?",
           "answer": "GPT stands for 'Generative Pre-trained Transformer'..."
         }
       ]
     }
     ```

4. **Add ~30 Resource Links**:
   - Add "moreInfo" arrays to each section
   - Structure:

     ```json
     {
       "id": "vendors",
       "moreInfo": [
         {
           "text": "Azure OpenAI and Microsoft Foundry models",
           "url": "https://learn.microsoft.com/..."
         }
       ]
     }
     ```

5. **Verify Complete Content**:
   - Compare section-by-section with markdown
   - Ensure no abbreviated summaries
   - Preserve all subsections and nested content

**Estimated Time**: 8-10 hours

### Task 2: Update Razor Component (if needed)

**File**: `src/TechHub.Web/Components/Pages/GenAIBasics.razor`

**Verify Rendering**:

- Mermaid diagrams render via `<script>` tag
- FAQ blocks display with proper styling
- Resource links display in "More information" sections
- All content sections use proper semantic HTML

**May Need**:

- Add mermaid.js script reference
- Add FAQ block rendering logic
- Add "More information" section rendering

**Estimated Time**: 2-3 hours

### Task 3: Create E2E Tests

**File**: `tests/TechHub.E2E.Tests/Web/GenAIBasicsTests.cs`

**Test Methods**:

```csharp
[Fact]
public async Task GenAIBasics_OnPageLoad_ShouldRenderAllSections()

[Fact]
public async Task GenAIBasics_ShouldRender_WithSidebarToc()

[Fact]
public async Task GenAIBasics_TocLinks_ShouldScrollToSections()

[Fact]
public async Task GenAIBasics_Scrolling_ShouldUpdateActiveTocLink()

[Fact]
public async Task GenAIBasics_LastSection_ShouldScroll_ToDetectionPoint()

[Fact]
public async Task GenAIBasics_MermaidDiagrams_ShouldRender()

[Fact]
public async Task GenAIBasics_FaqBlocks_ShouldDisplay()

[Fact]
public async Task GenAIBasics_ResourceLinks_ShouldBeClickable()
```

**Reference Pattern**: `HandbookTests.cs`, `LevelsOfEnlightenmentTests.cs` for page-specific tests; `CustomPagesTests.cs` for general tests

**Estimated Time**: 3-4 hours

### Task 4: Run Tests & Validation

```powershell
Run -TestProject E2E.Tests -TestName GenAIBasics
```

**Validation**:

- All tests pass
- No console errors
- Mermaid diagrams render correctly
- FAQ blocks display properly
- Resource links work

**Estimated Time**: 1 hour

## Success Metrics

- ✅ JSON contains all 10 mermaid diagrams
- ✅ JSON contains all 7 FAQ questions with answers
- ✅ JSON contains all ~30 resource links
- ✅ All 13 sections have complete content (not abbreviated)
- ✅ All E2E tests pass (8 test methods)
- ✅ Page loads in < 2 seconds
- ✅ No console errors
- ✅ Mermaid diagrams render without errors
- ✅ FAQ blocks are readable and styled

## Dependencies

- ✅ SidebarToc component (already exists)
- ✅ GenAIBasics.razor (already exists)
- ❌ Complete JSON content (needs update)
- ❌ Mermaid.js integration (may need to add)
- ❌ E2E tests (need to create)

## Known Issues

Based on comparison report (`.tmp/json-comparison-report.md`):

- History section missing 3 mermaid timelines
- Vendors section missing mermaid vendor flow diagram
- Models section missing FAQ block (3 questions)
- Providers section missing FAQ block (4 questions)
- Multiple sections missing "More information" link blocks
- All mermaid diagrams missing from JSON

## Out of Scope

- Content authoring or rewriting
- Visual design changes beyond standard rendering
- Mobile navigation (covered in spec 011)
- SEO optimization (covered in spec 005)

## Completion Checklist

- [ ] Original markdown downloaded and analyzed
- [ ] 10 mermaid diagrams added to JSON
- [ ] 7 FAQ questions added to JSON
- [ ] ~30 resource links added to JSON
- [ ] All sections verified complete (no abbreviations)
- [ ] Razor component updated for new content types
- [ ] E2E test file created with 8 test methods
- [ ] All tests pass without failures
- [ ] No console errors during page load
- [ ] Mermaid diagrams render correctly
- [ ] Documentation updated
