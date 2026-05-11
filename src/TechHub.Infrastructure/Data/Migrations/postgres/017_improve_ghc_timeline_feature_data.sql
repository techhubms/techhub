-- Migration 017: Comprehensive GHC timeline feature data
-- Date: 2026-05-11
-- Purpose: Replace the timelineFeatures array on the features custom page with a
--          comprehensive, chronologically-accurate list of GitHub Copilot feature
--          milestones. Dates are best-effort based on official GitHub Changelog,
--          GitHub Blog announcements, and Build/Universe keynotes.
--
-- Notes on dates:
--   - Most dates reflect public preview / first public release at month granularity.
--   - "Plans" lists current availability (not historical), so e.g. Code Completions
--     lists Free even though Free didn't exist when Code Completions launched.
--
-- Strategy: PostgreSQL JSONB merge (||) replaces only the timelineFeatures key
--           while preserving every other key in the JSON document.

UPDATE custom_page_data
SET
    json_data  = (
        json_data::jsonb || '{"timelineFeatures": [
  {
    "id": "code-completions",
    "name": "Code Completions",
    "releaseDate": "2022-06",
    "description": "GitHub Copilot launches as a paid product on June 21, 2022 — real-time AI code suggestions as you type, from single-line completions to entire function bodies. Trained on billions of lines of public code, it draws on your open files, comments, and cursor context to deliver precise, language-aware completions across all major editors and languages.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": true,
    "videoSlug": null
  },
  {
    "id": "copilot-for-business",
    "name": "GitHub Copilot for Business",
    "releaseDate": "2023-02",
    "description": "Announced on February 14, 2023 — GitHub Copilot extended for organizations with centralized license management, organization-wide policy controls, IP indemnity protection, and guaranteed exclusion of your code from model training. Gives IT, legal, and security teams the compliance and governance controls needed to deploy Copilot at scale.",
    "plans": ["Business", "Enterprise"],
    "ghesSupport": true,
    "videoSlug": null
  },
  {
    "id": "copilot-chat-preview",
    "name": "Copilot Chat (preview)",
    "releaseDate": "2023-09",
    "description": "Copilot Chat enters public beta in Visual Studio Code and Visual Studio in September 2023 — an in-editor conversational AI assistant that can answer questions about your code, generate tests, fix bugs, and refactor through natural language, without leaving your editor.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": true,
    "videoSlug": null
  },
  {
    "id": "copilot-chat-ga",
    "name": "Copilot Chat — General Availability",
    "releaseDate": "2023-12",
    "description": "Copilot Chat reaches general availability on December 29, 2023 across Visual Studio, VS Code, JetBrains IDEs, and GitHub Mobile — bringing production-ready conversational coding assistance to every Copilot subscriber with deeper IDE context awareness, slash commands, and chat participants.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": true,
    "videoSlug": null
  },
  {
    "id": "copilot-enterprise",
    "name": "GitHub Copilot Enterprise",
    "releaseDate": "2024-02",
    "description": "GA on February 27, 2024 — GitHub Copilot''s most advanced tier for large organizations. Extends Business with Copilot Knowledge Bases that index your private repositories and wikis, organization-wide custom instructions, AI-assisted pull request summaries, and richer GitHub.com integrations across issues, commits, and code search.",
    "plans": ["Enterprise"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "copilot-for-cli",
    "name": "Copilot in the CLI (gh copilot)",
    "releaseDate": "2024-03",
    "description": "GA on March 21, 2024 as a gh CLI extension — natural-language shell assistance via gh copilot suggest to generate commands and gh copilot explain to demystify complex pipelines. Covers git, the GitHub CLI, and common shell tools, and can execute the generated command directly after your confirmation.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "copilot-workspace",
    "name": "Copilot Workspace (technical preview)",
    "releaseDate": "2024-04",
    "description": "Technical preview on April 29, 2024 — a Copilot-native developer environment where you describe a task and Copilot proposes a multi-file plan, generates the changes, and runs tests in a cloud workspace. An early exploration of fully agentic, plan-driven development that informed the later Coding Agent.",
    "plans": ["Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "copilot-extensions",
    "name": "Copilot Extensions",
    "releaseDate": "2024-05",
    "description": "Public preview at Build / GitHub keynote in May 2024 — a marketplace of third-party extensions (DataStax, Docker, LaunchDarkly, Sentry, and many more) that plug directly into Copilot Chat. Lets you query, configure, and trigger external developer tools through natural language without leaving your editor.",
    "plans": ["Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "github-models",
    "name": "GitHub Models",
    "releaseDate": "2024-08",
    "description": "Launched on August 1, 2024 — a model playground and API hub built into GitHub. Experiment with and compare frontier models from OpenAI, Anthropic, Google, Meta, and more with zero setup. The unified REST API is fully OpenAI SDK-compatible, so switching from GPT-4.1 to Claude Sonnet or Gemini Pro requires only a model name change.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "multi-model-picker",
    "name": "Multi-Model Picker in Copilot",
    "releaseDate": "2024-10",
    "description": "Announced at GitHub Universe (October 29, 2024) — Copilot Chat gains a model picker letting users choose between OpenAI GPT-4o, Anthropic Claude 3.5 Sonnet, and Google Gemini 1.5 Pro. The first time Copilot supported multiple frontier model providers natively, ending OpenAI exclusivity.",
    "plans": ["Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "custom-instructions",
    "name": "Custom Instructions (.github/copilot-instructions.md)",
    "releaseDate": "2024-10",
    "description": "Announced at GitHub Universe in October 2024 — teach Copilot your project''s conventions once by storing preferred languages, frameworks, coding standards, and domain context in a .github/copilot-instructions.md file. Every Copilot chat response and code suggestion automatically respects them, with no need to repeat yourself each session.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "code-review",
    "name": "Copilot Code Review (public preview)",
    "releaseDate": "2024-10",
    "description": "Public preview at GitHub Universe (October 29, 2024) — request Copilot as a reviewer on any pull request and it reads your diff, flags logic errors, security concerns, and style issues, and posts inline comments with concrete fix suggestions you can accept as a commit with a single click.",
    "plans": ["Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "copilot-edits",
    "name": "Copilot Edits",
    "releaseDate": "2024-11",
    "description": "Public preview in November 2024 — describe a multi-file change in natural language and Copilot Edits proposes coordinated edits across all relevant files in a single review surface. You see every change as a diff and approve or revise per file, bridging single-suggestion completions and full agentic editing.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "copilot-free",
    "name": "GitHub Copilot Free",
    "releaseDate": "2024-12",
    "description": "Released on December 18, 2024 — GitHub Copilot made available at no cost to every GitHub user, no credit card required. Includes 2,000 code completions and 50 premium chat requests per month, with access to frontier models including GPT-4.1, Claude Sonnet 4.5, and Gemini 2.5 Pro right out of the box.",
    "plans": ["Free"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "agent-mode",
    "name": "Agent Mode (preview)",
    "releaseDate": "2025-02",
    "description": "Public preview in VS Code on February 6, 2025 — Copilot independently plans changes, edits files across your repository, runs terminal commands, executes tests, reads error output, and self-corrects from a single natural-language instruction. You stay in control by reviewing each proposed change before it is applied.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "next-edit-suggestions",
    "name": "Next Edit Suggestions",
    "releaseDate": "2025-02",
    "description": "Preview in VS Code in February 2025 — instead of only completing the current line, Copilot predicts the next logical edit anywhere in the open file based on your recent changes. Perfect for repetitive refactors, renames, and pattern-following changes that previously required manual fan-out.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "premium-requests",
    "name": "Premium Requests Model",
    "releaseDate": "2025-04",
    "description": "Introduced in April 2025 — a per-plan monthly allowance of premium requests that unlock the most capable models (Claude Sonnet 4, GPT-5, Gemini 2.5 Pro). Lighter models stay unlimited, while frontier models draw from this pool, giving teams predictable access to top-tier intelligence without unbounded cost.",
    "plans": ["Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "mcp-server-support",
    "name": "MCP Server Support",
    "releaseDate": "2025-04",
    "description": "GA in VS Code in April 2025 — first-class support for the open Model Context Protocol (MCP) standard. Copilot connects to any compatible server: databases, REST APIs, search engines, issue trackers, and more. Agents can query live data and trigger actions in external systems, using the results directly within their reasoning.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "code-review-ga",
    "name": "Copilot Code Review — General Availability",
    "releaseDate": "2025-04",
    "description": "Copilot Code Review reaches GA in April 2025 — automatic AI reviews on every pull request opened in enrolled repositories, with org-level enablement controls, custom review instructions, and full integration into branch protection rules and required-checks workflows.",
    "plans": ["Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "proplus-tier",
    "name": "Copilot Pro+ Tier",
    "releaseDate": "2025-05",
    "description": "Launched at Microsoft Build 2025 — a new $39/month individual tier sitting above Pro. Includes 1,500 premium requests, access to all frontier models (including Claude Opus), GitHub Spark, third-party coding agents, and increased GitHub Models rate limits for power users and indie developers.",
    "plans": ["Pro+"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "coding-agent",
    "name": "Coding Agent",
    "releaseDate": "2025-05",
    "description": "Announced at Microsoft Build on May 19, 2025 — assign any GitHub Issue to Copilot and it works autonomously in the background, creating a branch, reading related code, implementing a solution, running CI checks, and opening a pull request for your review. Frees you from context-switching by handling implementation work while you stay focused on higher-level problems.",
    "plans": ["Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "github-spark",
    "name": "GitHub Spark",
    "releaseDate": "2025-10",
    "description": "GitHub Spark reaches general availability around GitHub Universe 2025 — describe an app idea in natural language and Spark scaffolds, deploys, and hosts a fully working web app in minutes, with built-in data storage, AI features, and one-click sharing. Aimed at turning ideas into shipped micro-apps without manual setup.",
    "plans": ["Pro+", "Enterprise"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "copilot-cli",
    "name": "GitHub Copilot CLI (standalone)",
    "releaseDate": "2025-11",
    "description": "A standalone Copilot CLI launched in late 2025, separate from the older gh copilot extension — a fully agentic terminal experience with multi-step planning, MCP server integration, BYOK support, and the ability to read, edit, and run code in your local repo from a single shell session.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "org-custom-instructions",
    "name": "Organization Custom Instructions",
    "releaseDate": "2026-04",
    "description": "GA on April 2, 2026 — administrators define organization-wide custom instructions that apply to every Copilot interaction across all repositories: enforce coding standards, security practices, preferred libraries, and domain context centrally instead of per-repo, with audit-friendly version history.",
    "plans": ["Business", "Enterprise"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "byok-vscode",
    "name": "Bring Your Own Key (BYOK) in VS Code",
    "releaseDate": "2026-04",
    "description": "GA on April 22, 2026 — supply your own API key for Anthropic, OpenAI, Google, or any OpenAI-compatible provider directly inside VS Code Copilot Chat. Lets enterprises route through their own contracts and quotas while keeping the full Copilot UX, and gives individuals access to models not yet in the default catalog.",
    "plans": ["Pro+", "Enterprise"],
    "ghesSupport": false,
    "videoSlug": null
  },
  {
    "id": "token-based-billing",
    "name": "Copilot Token-Based Billing",
    "releaseDate": "2026-06",
    "description": "Effective June 1, 2026 — replaces fixed ''premium request'' quotas with a flexible token-based billing model where cost scales with actual model usage. Heavier models like Claude Opus consume more tokens per interaction while lighter models cost less, giving individuals and organizations fine-grained control over their AI spend without artificial request caps.",
    "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
    "ghesSupport": false,
    "videoSlug": null
  }
]}'::jsonb
    )::text,
    updated_at = NOW()
WHERE key = 'features';
