---
title: From Playwright Automation to Agent Driven Testing (GHCP in Action)
section_names:
- ai
- devops
- github-copilot
author: syedarshad
date: 2026-04-21 05:00:00 +00:00
primary_section: github-copilot
tags:
- .github/agents
- Agent Driven Testing
- AI
- CI/CD
- Community
- Confidence Scoring
- Copilot Agents
- Developer Workflow
- DevOps
- Element Discovery
- End To End Testing
- Fallback Strategies
- GitHub Copilot
- GitHub Copilot Chat
- GitHub Repository
- MCP
- Natural Language Prompts
- npm
- Page Object Model
- Playwright
- Playwright.config.ts
- Selectors
- Self Healing Tests
- Test Automation
- VS Code
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-playwright-automation-to-agent-driven-testing-ghcp-in/ba-p/4507395
feed_name: Microsoft Tech Community
---

syedarshad walks through moving from brittle Playwright selector-based automation to agent-driven testing using GitHub Copilot (GHCP) agents and Model Context Protocol (MCP), including a practical setup flow in VS Code, confidence-scored element discovery, and fallback strategies for more resilient E2E tests.<!--excerpt_end-->

# From Playwright Automation to Agent Driven Testing (GHCP in Action)

Agent-driven testing shifts browser automation from hardcoded selectors and static scripts to AI-assisted discovery and adaptive execution. In this approach, **GitHub Copilot (GHCP) agents** use **Model Context Protocol (MCP)** to analyze pages, identify elements with **confidence scoring**, and apply **self-healing** strategies when the UI changes.

## What is agent-driven testing?

Agent-driven testing replaces brittle automation patterns (fixed selectors + predetermined workflows) with an agent that can:

- Analyze the current page structure
- Discover relevant elements dynamically
- Score element candidates for reliability (confidence)
- Choose interaction strategies based on context
- Fall back to alternate strategies when the first attempt fails

## Traditional Playwright vs MCP-enhanced agent-driven testing

| Traditional Playwright | Agent-driven (MCP-enhanced) |
| --- | --- |
| Hardcoded selectors | AI-discovered elements |
| Static test scripts | Dynamic, adaptive tests |
| Breaks with UI changes | Self-healing automation |
| Manual element analysis | Intelligent page exploration |
| Rule-based logic | Context-aware decisions |
| Limited fallback options | Intelligent cascading strategies |

### Example: hardcoded selector vs MCP tool call

Traditional approach (brittle):

```ts
const searchInput = page.locator('input[name="q"]');
```

Agent-driven approach (adaptive):

```ts
const searchResult = await this.mcpClient.callTool({
  name: 'playwright_find_element',
  arguments: {
    element_type: 'search_input',
    page_url: await this.page.url(),
    confidence_threshold: 0.8,
    generate_multiple_selectors: true
  }
});
```

## How agent-driven testing works with MCP

The workflow described uses MCP to imitate a human tester’s approach: explore, understand, then act.

### 1) Intelligent page analysis

The agent explores the target website and gathers page context to inform later decisions.

### 2) Dynamic element discovery with confidence scoring

Instead of relying on one selector, MCP-based discovery identifies candidates and scores them.

### 3) MCP server page analysis

The MCP server analyzes page content and returns insights that help the agent choose robust selectors and interaction paths.

### 4) Adaptive fallback strategies

When the primary interaction fails, the agent cascades through alternatives.

Example fallback outline from the post:

```ts
async getSearchInput() {
  console.log('🔎 MCP: Using intelligently discovered search input...');

  // 1. Try MCP-discovered element first (highest reliability)
  // 2. Real-time dynamic discovery (adaptive)
  // 3. Traditional selectors (fallback safety)
}
```

## Implementation guide (step-by-step)

### Step 1: Create GitHub agent configuration

1. Under your project directory:

```bash
mkdir -p .github/agents
```

2. Create this file:

- `.github/agents/playwright-agent.md`

(Use the exact configuration described in your setup.)

### Step 2: Select the agent in GitHub Copilot Chat

1. Open **GitHub Copilot Chat** in **VS Code**
2. Click the **agent selector** at the top of the chat
3. Choose **Playwright Tester Mode**
4. The agent will now use MCP-enhanced capabilities

### Step 3: Create a test using natural language prompts

Example prompt:

> Create a test case that navigates to www.google.com, searches for 'playwright tutorial', and navigates to the Playwright homepage. Use MCP analysis to discover elements intelligently.

### Step 4: Execute and monitor the agent behavior

Install and run Playwright:

```bash
npm install @playwright/test
npx playwright install
npx playwright test --headed
```

Example console output flow shown in the post:

- `🧠 MCP: Analyzing target URL: https://www.google.com`
- `📸 MCP: Taking page snapshot for element analysis...`
- `🔍 MCP: Analyzing page elements dynamically...`
- `🎯 MCP: Discovered search input: input[name="q"]`
- `✅ MCP: Using discovered selector: input[name="q"]`

## Sample execution flow (Google → Playwright)

### MCP page analysis

- Analyze URL
- Take snapshot
- Analyze elements dynamically

### Intelligent element discovery

- Discover search input: `input[name="q"]`
- Discover search button: `input[value="Google Search"]`

### Confidence-based execution

- Use discovered elements to execute the search

### Adaptive link detection

Example scoring logic mentioned:

```ts
if (href.includes('playwright.dev')) confidence += 50;
if (fullText.includes('playwright')) confidence += 20;
```

## Outcome details

### Performance comparison (as reported)

| Metric | Traditional Approach | MCP-Enhanced Approach |
| --- | --- | --- |
| Element Discovery | Static, breaks easily | 95% success with confidence scoring |
| Maintenance Effort | High (manual updates) | 90% reduction** (self-healing) |
| Bot Detection Handling | Basic fallback | Intelligent adaptive strategies |
| Test Reliability | 60–70% (UI changes) | 85–90%** (AI adaptation) |
| Debugging Time | 2–4 hours per failure | 20–30 minutes** (intelligent insights) |

## Benefits highlighted

### Self-healing tests

- Tests adapt to UI changes automatically
- Confidence scoring helps prevent false positives
- Intelligent fallback strategies improve reliability

### Intelligent element discovery

Avoid hardcoded selectors; use scoring signals such as:

```ts
if (name === 'q') score += 10;
if (role === 'combobox') score += 7;
if (placeholder?.includes('search')) score += 5;
```

### Enhanced debugging & insights

Examples shown:

- `✅ MCP: Using discovered selector: input[name="q"]`
- `🧠 MCP: Found 18 potential Playwright links`

### Natural language test creation

- Write prompts instead of building every step manually
- Agent generates automation with built-in handling patterns

## Implementation checklist

- Create `.github/agents/playwright-agent.md`
- Select **Playwright Tester Mode** in GitHub Copilot Chat
- Install Playwright: `npm install @playwright/test`
- Build an MCP-enhanced Page Object Model with confidence scoring
- Configure `playwright.config.ts` (reporting, etc.)
- Write tests using natural language prompts
- Run tests: `npx playwright test --headed`
- Review MCP insights in console output and reports

## Success metrics to look for

- Logs like: `MCP: Analyzing page elements dynamically...`
- Confidence indicators like: `MCP: Found 18 potential Playwright links`
- Adaptive behavior like: `MCP: Using discovered selector: input[name='q']`
- Tests passing despite UI changes
- Reduced maintenance cycles (as measured in your environment)


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-playwright-automation-to-agent-driven-testing-ghcp-in/ba-p/4507395)

