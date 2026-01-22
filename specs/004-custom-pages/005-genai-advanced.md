# Custom Page: GenAI Advanced

**Page**: GenAI Advanced  
**URL**: `/ai/genai-advanced`  
**Priority**: P2  
**Status**: Razor ❌ (Needs Update) | JSON ✅ | E2E Tests ❌  
**Estimated Effort**: 8-10 hours (remaining)

## Overview

GenAI Advanced JSON data is complete with all mermaid diagrams and resource links converted to simplified JSON structure with ID-based placeholders. This page has the most mermaid diagrams (6) and extensive resource links. Razor component needs update to use IMarkdownService for rendering.

**Live Site Reference**: <https://tech.hub.ms/ai/genai-advanced>

## Current Status

✅ **Complete**:

- **JSON structure**: `collections/_custom/genai-advanced.json` - **COMPLETE**
  - 13 sections (TOC excluded - generated dynamically)
  - 6 mermaid diagrams with ID-based placeholders
  - No FAQ blocks (none in source)
  - 8 More Info sections with resource links
  - All HTML converted to markdown
  - No `id` fields (generated dynamically from titles)
  - Document title extracted from frontmatter
- SidebarToc component integrated
- Conversion script: `.tmp/convert-genai-final.ps1` (same as genai-basics)

❌ **Needs Implementation**:

- **Razor component update**: `src/TechHub.Web/Components/Pages/GenAIAdvanced.razor`
  - Inject IMarkdownService
  - Load JSON from genai-advanced.json
  - Replace `{{mermaid:id}}` placeholders with actual diagrams
  - Render markdown to HTML using MarkdownService.RenderToHtml()
  - Generate TOC dynamically from section titles
  - Generate section IDs from titles

❌ **Missing Tests**:

- No E2E tests exist for GenAI Advanced page

## Acceptance Criteria

### JSON Content Completeness ✅ COMPLETE

**Source**: `https://raw.githubusercontent.com/techhubms/techhub/main/sections/ai/genai-advanced.md`

**Implementation Details**: Same simplified structure as genai-basics with ID-based mermaid placeholders and markdown-only content.

✅ **All 13 sections present** (TOC excluded - generated dynamically):

- Vectors and embeddings: How AI understands meaning ✅
- From embeddings to responses: The inference process ✅
- Neural networks and transformers ✅
- Attention mechanism ✅
- Context windows and model parameters ✅
- Fine-tuning a model ✅
- Function calling ✅
- Model Context Protocol (MCP) ✅
- Retrieval Augmented Generation (RAG) ✅
- Agents and agentic AI ✅
- Multi-agent solutions ✅
- Observability: Monitoring and evaluating AI applications ✅
- Scaling AI implementations ✅

✅ **6 Mermaid Diagrams** (validated):

- All diagrams present with ID-based placeholders ✅

✅ **No FAQ Blocks** (none in source) ✅

✅ **8 More Info sections** with resource links (validated)

- Observability: Monitoring and evaluating AI applications
- Scaling AI implementations

1. **Mermaid Diagrams** (8+ total):
   - Vectors and embeddings: 1 diagram (word-to-vector-to-space mapping)
   - Function calling: 1 diagram (sequence diagram for API calls)
   - MCP: 1 diagram (MCP architecture: Host/Client/Server)
   - RAG: 1 diagram (RAG workflow: Query → Embedding → Search → LLM)
   - Multi-agent solutions: 2 diagrams (orchestrator routing, sequence diagram)
   - Observability: 1 diagram (3-stage evaluation flowchart)
   - Additional technical diagrams in various sections

2. **"More information" Resource Links** (~35 total):
   - Context windows: ~4 links
   - Fine-tuning: ~3 links
   - Function calling: ~6 links
   - MCP: ~6 links
   - RAG: ~2 links
   - Agents: ~6 links
   - Multi-agent solutions: ~8 links
   - Observability: ~4 links

3. **Tables**:
   - Orchestration patterns table (5 patterns: Concurrent, Sequential, Group Chat, Handoff, Magentic)
   - Evaluator types table

### E2E Test Coverage

1. **Page Load & Rendering**
   - Navigate to `/ai/genai-advanced`
   - Verify content renders without errors
   - Verify all 13 sections display

2. **SidebarToc Functionality**
   - TOC auto-extracts all 13 h2 headings
   - TOC links scroll to correct sections
   - Scroll spy highlights active section
   - Last section can scroll to detection point

3. **Content Rendering**
   - Mermaid diagrams render correctly (8+ diagrams)
   - Resource link blocks render
   - Code examples format correctly
   - Tables display properly

4. **Interactive Elements**
   - Resource links are clickable
   - Keyboard navigation works
   - No console errors

## Implementation Tasks

### Task 1: JSON Content Update (REQUIRED)

**File**: `collections/_custom/genai-advanced.json`

**CRITICAL**: This is MANDATORY - JSON must match original markdown exactly

**Subtasks**:

1. **Fetch original markdown**:
   - Download from `https://raw.githubusercontent.com/techhubms/techhub/main/sections/ai/genai-advanced.md`
   - Parse complete structure (most complex of all GenAI pages)

2. **Add 8+ Mermaid Diagrams**:
   - Vectors and embeddings: word-to-vector mapping
   - Function calling: sequence diagram
   - MCP: architecture diagram (Host → Client → Server)
   - RAG: workflow diagram (Query → Embedding → Search → Combine → LLM → Response)
   - Multi-agent: orchestrator routing diagram
   - Multi-agent: sequence diagram showing agent collaboration
   - Observability: 3-stage evaluation flowchart
   - Additional technical diagrams
   - Format all as JSON strings with `\n`

3. **Add ~35 Resource Links** (MOST EXTENSIVE):
   - Context windows section: Azure model benchmarking, optimization guides
   - Fine-tuning section: Azure AI fine-tuning, CLU guides
   - Function calling: MCP development, Agent Framework guides
   - MCP: MCP courses, security guides, video tutorials
   - Multi-agent: Design patterns, orchestration docs, zero-trust guides
   - Observability: Evaluation guides, tracing docs
   - Each section needs comprehensive "moreInfo" arrays

4. **Add Tables**:
   - Orchestration patterns (5 patterns with control/topology/best-for columns)
   - Evaluator types (4 categories)

5. **Verify Technical Accuracy**:
   - Ensure all code examples are complete
   - Verify technical terminology is accurate
   - Check that architecture diagrams match descriptions

**Estimated Time**: 6-7 hours

### Task 2: Update Razor Component (if needed)

**File**: `src/TechHub.Web/Components/Pages/GenAIAdvanced.razor`

**Verify Rendering**:

- Complex mermaid diagrams render (architecture, sequences)
- Resource links display
- Tables render properly
- Code examples format with syntax highlighting
- Technical content displays correctly

**Estimated Time**: 2-3 hours

### Task 3: Create E2E Tests

**File**: `tests/TechHub.E2E.Tests/Web/GenAIAdvancedTests.cs`

**Reference Pattern**: `HandbookTests.cs`, `LevelsOfEnlightenmentTests.cs` for page-specific tests; `CustomPagesTests.cs` for general tests

**Test Methods**:

```csharp
[Fact]
public async Task GenAIAdvanced_OnPageLoad_ShouldRenderAllSections()

[Fact]
public async Task GenAIAdvanced_ShouldRender_WithSidebarToc()

[Fact]
public async Task GenAIAdvanced_TocLinks_ShouldScrollToSections()

[Fact]
public async Task GenAIAdvanced_Scrolling_ShouldUpdateActiveTocLink()

[Fact]
public async Task GenAIAdvanced_LastSection_ShouldScroll_ToDetectionPoint()

[Fact]
public async Task GenAIAdvanced_MermaidDiagrams_ShouldRender()

[Fact]
public async Task GenAIAdvanced_TechnicalContent_ShouldDisplay()

[Fact]
public async Task GenAIAdvanced_ResourceLinks_ShouldBeClickable()
```

**Estimated Time**: 2-3 hours

### Task 4: Run Tests & Validation

```powershell
Run -TestProject E2E.Tests -TestName GenAIAdvanced
```

**Validation**:

- All tests pass
- No console errors
- All 8+ mermaid diagrams render
- Complex architecture diagrams display correctly
- Resource links work

**Estimated Time**: 1 hour

## Success Metrics

- ✅ JSON contains all 8+ mermaid diagrams
- ✅ JSON contains all ~35 resource links (most extensive)
- ✅ JSON contains both tables
- ✅ All 13 sections have complete technical content
- ✅ All E2E tests pass (8 test methods)
- ✅ Page loads in < 2 seconds (despite complex content)
- ✅ No console errors
- ✅ All architecture diagrams render correctly
- ✅ Technical accuracy verified

## Dependencies

- ✅ SidebarToc component (already exists)
- ✅ GenAIAdvanced.razor (already exists)
- ❌ Complete JSON content (needs extensive update)
- ❌ E2E tests (need to create)
- ❌ Mermaid.js with support for complex diagrams

## Known Issues

Based on comparison report:

- Missing word-to-vector mapping diagram
- Missing function calling sequence diagram
- Missing MCP architecture diagram
- Missing RAG workflow diagram
- Missing 2 multi-agent diagrams
- Missing evaluation flowchart
- ~35 resource links missing across all sections
- Some sections may have abbreviated content

## Out of Scope

- Content authoring or rewriting technical concepts
- Visual design changes beyond standard rendering
- Mobile navigation (covered in spec 011)
- SEO optimization (covered in spec 005)
- Code playground or interactive demos

## Completion Checklist

- [ ] Original markdown downloaded and analyzed
- [ ] 8+ mermaid diagrams added to JSON
- [ ] ~35 resource links added to JSON
- [ ] 2 tables added to JSON
- [ ] All 13 sections verified complete and technically accurate
- [ ] Complex architecture diagrams tested
- [ ] Razor component updated for advanced content
- [ ] E2E test file created with 8 test methods
- [ ] All tests pass without failures
- [ ] No console errors
- [ ] Technical accuracy review completed
- [ ] Documentation updated
