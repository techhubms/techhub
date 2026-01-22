# Custom Page: GenAI Applied

**Page**: GenAI Applied  
**URL**: `/ai/genai-applied`  
**Priority**: P2  
**Status**: Razor ❌ (Needs Update) | JSON ✅ | E2E Tests ❌  
**Estimated Effort**: 6-8 hours (remaining)

## Overview

GenAI Applied JSON data is complete with all mermaid diagrams, FAQ blocks, and resource links converted to simplified JSON structure with ID-based placeholders. Razor component needs update to use IMarkdownService for rendering.

**Live Site Reference**: <https://tech.hub.ms/ai/genai-applied>

## Current Status

✅ **Complete**:

- **JSON structure**: `collections/_custom/genai-applied.json` - **COMPLETE**
  - 6 sections (TOC excluded - generated dynamically)
  - 1 mermaid diagram with ID-based placeholder
  - 1 FAQ block (2 Q&A pairs) - markdown format
  - 5 More Info sections with resource links
  - All HTML converted to markdown
  - No `id` fields (generated dynamically from titles)
  - Document title extracted from frontmatter
- SidebarToc component integrated
- Conversion script: `.tmp/convert-genai-final.ps1` (same as genai-basics)

❌ **Needs Implementation**:

- **Razor component update**: `src/TechHub.Web/Components/Pages/GenAIApplied.razor`
  - Inject IMarkdownService
  - Load JSON from genai-applied.json
  - Replace `{{mermaid:id}}` placeholders with actual diagrams
  - Render markdown to HTML using MarkdownService.RenderToHtml()
  - Generate TOC dynamically from section titles
  - Generate section IDs from titles

❌ **Missing Tests**:

- No E2E tests exist for GenAI Applied page

## Acceptance Criteria

### JSON Content Completeness ✅ COMPLETE

**Source**: `https://raw.githubusercontent.com/techhubms/techhub/main/sections/ai/genai-applied.md`

**Implementation Details**: Same simplified structure as genai-basics with ID-based mermaid placeholders and markdown-only content.

✅ **All 6 sections present** (TOC excluded - generated dynamically):

- What You Can Build Today ✅
- Integrating AI into Your Applications ✅
- Building Multi-Agent Systems ✅
- Monitoring and Evaluating AI Applications ✅
- The AI-native web: NLWeb, llms.txt, and semantic search ✅
- Learning Resources ✅

✅ **1 Mermaid Diagram** (validated):

- Building Multi-Agent Systems: 1 diagram ✅

✅ **1 FAQ Block** (2 Q&A pairs - markdown format):

- Integrating AI section: 2 questions ✅

✅ **5 More Info sections** with resource links (validated)

- Monitoring: 1 diagram (3-stage evaluation flowchart)

1. **FAQ Blocks** (2+ blocks):
   - GitHub Copilot section: 1 FAQ block (when to refactor IDE vs Copilot, which mode for features)
   - .NET AI section: 1 FAQ block (Semantic Kernel vs Agent Framework, Azure OpenAI vs OpenAI)

2. **"More information" Resource Links** (~20 total):
   - GitHub Copilot section: ~7 links
   - Azure AI Services section: ~6 links
   - Building Multi-Agent Systems: ~8 links
   - Monitoring: ~4 links

3. **Tables**:
   - Orchestration patterns table (Pattern | Use when)
   - Evaluator types table (Category | What it measures)

### E2E Test Coverage

1. **Page Load & Rendering**
   - Navigate to `/ai/genai-applied`
   - Verify content renders without errors
   - Verify all 6 sections display

2. **SidebarToc Functionality**
   - TOC auto-extracts all 6 h2 headings
   - TOC links scroll to correct sections
   - Scroll spy highlights active section
   - Last section can scroll to detection point

3. **Content Rendering**
   - Mermaid diagrams render correctly (3 diagrams)
   - FAQ blocks display with questions/answers
   - Resource link blocks render
   - Tables display properly
   - Project cards render (What You Can Build Today)

4. **Interactive Elements**
   - Resource links are clickable
   - Project links navigate correctly
   - Keyboard navigation works
   - No console errors

## Implementation Tasks

### Task 1: JSON Content Update (REQUIRED)

**File**: `collections/_custom/genai-applied.json`

**CRITICAL**: This is MANDATORY - JSON must match original markdown exactly

**Subtasks**:

1. **Fetch original markdown**:
   - Download from `https://raw.githubusercontent.com/techhubms/techhub/main/sections/ai/genai-applied.md`
   - Parse complete structure

2. **Add 3 Mermaid Diagrams**:
   - Integrating AI: 3-layer architecture diagram
   - Multi-Agent Systems: Orchestrator routing diagram
   - Monitoring: 3-stage evaluation flowchart
   - Format as JSON strings with `\n`

3. **Add 2+ FAQ Blocks**:
   - GitHub Copilot section: FAQ with 2 questions
   - .NET AI section: FAQ with 2 questions
   - Structure:

     ```json
     {
       "id": "github-copilot",
       "faq": [
         {
           "question": "When should I refactor using my IDE versus using Copilot?",
           "answer": "Use IDE refactoring tools for standard operations..."
         }
       ]
     }
     ```

4. **Add ~20 Resource Links**:
   - Add "moreInfo" arrays to sections
   - Include all documentation links from markdown

5. **Add Tables**:
   - Orchestration patterns (4 patterns: Sequential, Concurrent, Group Chat, Handoff)
   - Evaluator types (4 categories: General quality, RAG quality, Safety, Agent-specific)

6. **Verify Project Cards** (What You Can Build Today):
   - Ensure all 5 projects have complete data:
     - Build an MCP server
     - Create a coding agent
     - Deploy a RAG chatbot
     - Automate document processing
     - Build a multi-agent system

**Estimated Time**: 5-6 hours

### Task 2: Update Razor Component (if needed)

**File**: `src/TechHub.Web/Components/Pages/GenAIApplied.razor`

**Verify Rendering**:

- Mermaid diagrams render
- FAQ blocks display
- Resource links display
- Tables render properly
- Project cards display with links

**Estimated Time**: 1-2 hours

### Task 3: Create E2E Tests

**File**: `tests/TechHub.E2E.Tests/Web/GenAIAppliedTests.cs`

**Reference Pattern**: `HandbookTests.cs`, `LevelsOfEnlightenmentTests.cs` for page-specific tests; `CustomPagesTests.cs` for general tests

**Test Methods**:

```csharp
[Fact]
public async Task GenAIApplied_OnPageLoad_ShouldRenderAllSections()

[Fact]
public async Task GenAIApplied_ShouldRender_WithSidebarToc()

[Fact]
public async Task GenAIApplied_TocLinks_ShouldScrollToSections()

[Fact]
public async Task GenAIApplied_Scrolling_ShouldUpdateActiveTocLink()

[Fact]
public async Task GenAIApplied_LastSection_ShouldScroll_ToDetectionPoint()

[Fact]
public async Task GenAIApplied_MermaidDiagrams_ShouldRender()

[Fact]
public async Task GenAIApplied_ProjectCards_ShouldDisplay()

[Fact]
public async Task GenAIApplied_ResourceLinks_ShouldBeClickable()
```

**Estimated Time**: 2-3 hours

### Task 4: Run Tests & Validation

```powershell
Run -TestProject E2E.Tests -TestName GenAIApplied
```

**Validation**:

- All tests pass
- No console errors
- Mermaid diagrams render
- FAQ blocks display
- Tables display correctly

**Estimated Time**: 1 hour

## Success Metrics

- ✅ JSON contains all 3 mermaid diagrams
- ✅ JSON contains all FAQ blocks (2+ blocks)
- ✅ JSON contains all ~20 resource links
- ✅ JSON contains both tables (patterns, evaluators)
- ✅ All 6 sections have complete content
- ✅ All E2E tests pass (8 test methods)
- ✅ Page loads in < 2 seconds
- ✅ No console errors
- ✅ Project cards display correctly

## Dependencies

- ✅ SidebarToc component (already exists)
- ✅ GenAIApplied.razor (already exists)
- ❌ Complete JSON content (needs update)
- ❌ E2E tests (need to create)

## Known Issues

Based on comparison report:

- Missing 3-layer architecture mermaid diagram
- Missing orchestrator routing diagram
- Missing evaluation flowchart
- GitHub Copilot FAQ block missing
- .NET AI FAQ block missing
- Multiple "More information" sections missing

## Out of Scope

- Content authoring or rewriting
- Visual design changes
- Mobile navigation (covered in spec 011)
- SEO optimization (covered in spec 005)

## Completion Checklist

- [ ] Original markdown downloaded and analyzed
- [ ] 3 mermaid diagrams added to JSON
- [ ] 2+ FAQ blocks added to JSON
- [ ] ~20 resource links added to JSON
- [ ] 2 tables added to JSON
- [ ] All sections verified complete
- [ ] Razor component updated for new content
- [ ] E2E test file created with 8 test methods
- [ ] All tests pass without failures
- [ ] No console errors
- [ ] Documentation updated
