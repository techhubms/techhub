-- Migration 015: Add timelineFeatures to the GitHub Copilot Features custom page
-- Date: 2026-05-08
-- Purpose: Update the features custom page JSON to include a chronological
--          timeline of GitHub Copilot feature releases (new timelineFeatures field).
--          Uses UPSERT so this is safe to run on databases that already have the key.

INSERT INTO custom_page_data (key, description, json_data, updated_at)
VALUES (
    'features',
    'GitHub Copilot Features',
    $JSON${
  "title": "GitHub Copilot Features",
  "description": "Explore the powerful features of GitHub Copilot across all subscription tiers.",
  "intro": "This page provides a comprehensive overview of GitHub Copilot plans, combining official features with example videos. For the most current pricing, visit GitHub's official pricing page.",
  "note": "GitHub Copilot features like the Coding Agent are not currently available in GitHub Enterprise Server. For the most recent plan details view the official documentation.",
  "links": {
    "pricing": "https://github.com/features/copilot/plans",
    "planDetails": "https://docs.github.com/en/copilot/get-started/plans"
  },
  "subscriptionTiers": [
    {
      "id": "free",
      "name": "Free",
      "tagline": "A fast way to get started with GitHub Copilot",
      "price": null,
      "features": [
        "50 premium requests per month",
        "2,000 completions per month",
        "Access to Claude Haiku 4.5, GPT-4.1, GPT-5 mini, and more"
      ],
      "videoAnchor": "videos-free"
    },
    {
      "id": "student",
      "name": "Student",
      "tagline": "Free access for verified students",
      "price": null,
      "features": [
        "Everything in Free",
        "Coding agent",
        "Unlimited agent mode and chats with GPT-5 mini",
        "Unlimited code completions",
        "Access to code review, Claude Sonnet 4/4.5, GPT-5, Gemini 2.5 Pro, and more",
        "300 premium requests per month",
        "Free for verified students"
      ],
      "videoAnchor": "videos-pro"
    },
    {
      "id": "pro",
      "name": "Pro",
      "tagline": "Accelerate workflows with GitHub Copilot ($10 USD per month or $100 per year)",
      "price": {
        "monthly": 10,
        "yearly": 100,
        "currency": "USD"
      },
      "features": [
        "Everything in Free",
        "Coding agent",
        "Unlimited agent mode and chats with GPT-5 mini",
        "Unlimited code completions",
        "Access to code review, Claude Sonnet 4/4.5, GPT-5, Gemini 2.5 Pro, and more",
        "300 premium requests to use latest models, with option to buy more",
        "Free for verified teachers and maintainers of popular open source projects"
      ],
      "videoAnchor": "videos-pro"
    },
    {
      "id": "business",
      "name": "Business",
      "tagline": "For teams and organizations ($19 USD per user per month)",
      "price": {
        "monthly": 19,
        "perUser": true,
        "currency": "USD"
      },
      "features": [
        "Everything in Pro",
        "Coding agent for organizations",
        "Organization custom instructions",
        "Usage metrics and analytics",
        "Data excluded from training by default",
        "User management and policies",
        "Content exclusions",
        "Audit logs",
        "300 premium requests per user per month"
      ],
      "videoAnchor": "videos-pro"
    },
    {
      "id": "proplus",
      "name": "Pro+",
      "tagline": "Scale with agents and more models ($39 USD per month or $390 per year)",
      "price": {
        "monthly": 39,
        "yearly": 390,
        "currency": "USD"
      },
      "features": [
        "Everything in Pro",
        "Access to all models, including Claude Opus 4.5/4.6 and more",
        "1,500 premium requests per month, with option to buy more",
        "Third-party coding agents",
        "Access to GitHub Spark",
        "Increased GitHub Models rate limits"
      ],
      "videoAnchor": "videos-proplus"
    },
    {
      "id": "enterprise",
      "name": "Enterprise",
      "tagline": "Full enterprise features and controls ($39 USD per user per month)",
      "price": {
        "monthly": 39,
        "perUser": true,
        "currency": "USD"
      },
      "features": [
        "Everything in Business",
        "1,000 premium requests per user per month",
        "Third-party coding agents",
        "Access to GitHub Spark",
        "Copilot knowledge bases",
        "Advanced compliance and security features"
      ],
      "videoAnchor": "videos-proplus"
    }
  ],
  "featureSections": [
    {
      "id": "videos-free",
      "title": "Free Features",
      "plans": [
        "Free"
      ]
    },
    {
      "id": "videos-pro",
      "title": "Student, Pro & Business Features",
      "plans": [
        "Student",
        "Pro",
        "Business"
      ]
    },
    {
      "id": "videos-proplus",
      "title": "Pro+ & Enterprise Features",
      "plans": [
        "Pro+",
        "Enterprise"
      ]
    }
  ],
  "videoCollection": "ghc-features",
  "timelineFeatures": [
    {
      "id": "token-based-billing",
      "name": "Copilot Token-Based Billing",
      "releaseDate": "2025-05",
      "description": "GitHub Copilot moves from a fixed-count 'premium request' model to a flexible token-based billing system — giving individuals and organizations more predictable costs and fine-grained control over model usage.",
      "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
      "ghesSupport": false,
      "videoSlug": null
    },
    {
      "id": "coding-agent",
      "name": "Coding Agent",
      "releaseDate": "2025-05",
      "description": "An autonomous AI agent that independently implements features, fixes bugs, and resolves GitHub issues — creating branches, writing code, running tests, and opening pull requests with minimal human intervention.",
      "plans": ["Pro", "Business", "Pro+", "Enterprise", "Student"],
      "ghesSupport": false,
      "videoSlug": null
    },
    {
      "id": "mcp-server-support",
      "name": "MCP Server Support",
      "releaseDate": "2025-04",
      "description": "Model Context Protocol (MCP) integration lets Copilot connect to external tools and data sources, enabling richer context-aware completions and enabling agents to take actions in external services.",
      "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
      "ghesSupport": false,
      "videoSlug": null
    },
    {
      "id": "agent-mode",
      "name": "Agent Mode",
      "releaseDate": "2025-02",
      "description": "Agentic multi-step coding in VS Code — Copilot iterates autonomously through planning, editing, running tests, and fixing errors across your entire codebase without constant prompting.",
      "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
      "ghesSupport": false,
      "videoSlug": null
    },
    {
      "id": "copilot-free",
      "name": "GitHub Copilot Free",
      "releaseDate": "2024-12",
      "description": "GitHub Copilot is now available for free to all GitHub users — no credit card required. Includes 2,000 code completions and 50 chat messages per month with access to leading AI models.",
      "plans": ["Free"],
      "ghesSupport": false,
      "videoSlug": null
    },
    {
      "id": "code-review",
      "name": "Copilot Code Review",
      "releaseDate": "2024-10",
      "description": "AI-powered pull request reviews that identify bugs, security issues, and code quality improvements — commenting directly on diffs with actionable, context-aware feedback.",
      "plans": ["Pro", "Business", "Pro+", "Enterprise", "Student"],
      "ghesSupport": false,
      "videoSlug": null
    },
    {
      "id": "custom-instructions",
      "name": "Custom Instructions",
      "releaseDate": "2024-09",
      "description": "Define persistent instructions that shape every Copilot response — include your coding standards, preferred libraries, and project conventions so Copilot always follows your team's practices.",
      "plans": ["Pro", "Business", "Pro+", "Enterprise", "Student"],
      "ghesSupport": false,
      "videoSlug": null
    },
    {
      "id": "github-models",
      "name": "GitHub Models",
      "releaseDate": "2024-07",
      "description": "Experiment with, compare, and integrate frontier AI models directly from the GitHub Marketplace — including OpenAI, Anthropic, Google, and Meta models — using a unified API compatible with OpenAI's SDK.",
      "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
      "ghesSupport": false,
      "videoSlug": null
    },
    {
      "id": "copilot-for-cli",
      "name": "Copilot in the CLI",
      "releaseDate": "2024-03",
      "description": "Natural-language assistance in your terminal — explain shell commands, suggest flags, and generate complex command-line pipelines without leaving your workflow.",
      "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
      "ghesSupport": false,
      "videoSlug": null
    },
    {
      "id": "copilot-enterprise",
      "name": "GitHub Copilot Enterprise",
      "releaseDate": "2024-02",
      "description": "Enterprise-grade Copilot with knowledge bases that index your private repositories, organization-wide custom instructions, deeper GitHub.com integration, and advanced admin controls for large organizations.",
      "plans": ["Enterprise"],
      "ghesSupport": false,
      "videoSlug": null
    },
    {
      "id": "copilot-chat",
      "name": "Copilot Chat",
      "releaseDate": "2023-09",
      "description": "An AI chat assistant embedded in your IDE — ask questions about your code, get explanations, debug errors, write tests, and refactor code through a conversational interface without leaving your editor.",
      "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
      "ghesSupport": true,
      "videoSlug": null
    },
    {
      "id": "copilot-for-business",
      "name": "GitHub Copilot for Business",
      "releaseDate": "2023-02",
      "description": "Copilot extended for teams — centralized license management, organization-wide policies, IP indemnity, and data exclusions from model training, giving IT and security teams the controls they need.",
      "plans": ["Business", "Enterprise"],
      "ghesSupport": true,
      "videoSlug": null
    },
    {
      "id": "code-completions",
      "name": "Code Completions",
      "releaseDate": "2022-06",
      "description": "Real-time AI-powered code suggestions in your editor — from single-line completions to entire function bodies — based on your code context, comments, and the patterns of millions of open-source projects.",
      "plans": ["Free", "Pro", "Business", "Pro+", "Enterprise", "Student"],
      "ghesSupport": true,
      "videoSlug": null
    }
  ]
}$JSON$,
    NOW()
)
ON CONFLICT (key) DO UPDATE
SET json_data  = EXCLUDED.json_data,
    updated_at = NOW();
