# Custom Page: GenAI Applied

**Page**: GenAI Applied  
**URL**: `/ai/genai-applied`  
**Priority**: P2  
**Status**: Razor ✅ | JSON ❌ (Incomplete) | E2E Tests ❌  
**Estimated Effort**: 8-10 hours

## Overview

GenAI Applied builds on GenAI Basics with practical applications and real-world use cases. The JSON file is missing several mermaid diagrams, FAQ blocks, and resource links.

**Live Site Reference**: <https://tech.hub.ms/ai/genai-applied>

## Current Status

✅ **Complete**:

- Razor component exists: `src/TechHub.Web/Components/Pages/GenAIApplied.razor`
- SidebarToc component integrated
- Basic JSON structure: `collections/_custom/genai-applied.json`

❌ **Critical Missing Content** (from comparison report):

- **3+ mermaid diagrams**
- **2+ FAQ blocks** with multiple questions
- **~20 "More information" resource links**
- Potentially missing subsections

❌ **Missing Tests**:

- No E2E tests exist for GenAI Applied page

## Acceptance Criteria

### JSON Content Completeness

**Source**: `https://raw.githubusercontent.com/techhubms/techhub/main/sections/ai/genai-applied.md`

1. **All 6 TOC sections** must be present in JSON with complete content:
   - What You Can Build Today
   - Integrating AI into Your Applications
   - Building Multi-Agent Systems
   - Monitoring and Evaluating AI Applications
   - The AI-Native Web
   - Learning Resources

2. **Mermaid Diagrams** (3+ total):
   - Integrating AI section: 1 diagram (3-layer architecture: Experience/Orchestration/Data)
   - Building Multi-Agent Systems: 1 diagram (orchestrator routing pattern)
   - Monitoring: 1 diagram (3-stage evaluation flowchart)

3. **FAQ Blocks** (2+ blocks):
   - GitHub Copilot section: 1 FAQ block (when to refactor IDE vs Copilot, which mode for features)
   - .NET AI section: 1 FAQ block (Semantic Kernel vs Agent Framework, Azure OpenAI vs OpenAI)

4. **"More information" Resource Links** (~20 total):
   - GitHub Copilot section: ~7 links
   - Azure AI Services section: ~6 links
   - Building Multi-Agent Systems: ~8 links
   - Monitoring: ~4 links

5. **Tables**:
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

### Task 1: JSON Content Update (CRITICAL)

**File**: `collections/_custom/genai-applied.json`

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
