---
layout: "post"
title: "Enterprise BYOK for GitHub Copilot Now in Public Preview"
description: "This news update announces the public preview of Bring Your Own Key (BYOK) for GitHub Copilot, allowing enterprise customers to connect API keys from providers like Anthropic, Microsoft Foundry, OpenAI, and xAI. It describes centralized model management, support for various IDEs, billing changes, configuration options for admins, and highlights a current limitation. The update is aimed at enterprise and business customers leveraging GitHub Copilot in development workflows."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-11-20-enterprise-bring-your-own-key-byok-for-github-copilot-is-now-in-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-11-20 15:46:54 +00:00
permalink: "/2025-11-20-Enterprise-BYOK-for-GitHub-Copilot-Now-in-Public-Preview.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["Admin Controls", "AI", "Anthropic", "API Key", "Billing", "BYOK", "Copilot", "Copilot Chat", "DevOps", "Enterprise", "GitHub Copilot", "IDE Integration", "LLM Providers", "Microsoft Foundry", "Model Management", "News", "OpenAI", "Organization Admin", "VS Code", "Xai"]
tags_normalized: ["admin controls", "ai", "anthropic", "api key", "billing", "byok", "copilot", "copilot chat", "devops", "enterprise", "github copilot", "ide integration", "llm providers", "microsoft foundry", "model management", "news", "openai", "organization admin", "vs code", "xai"]
---

Allison presents a release announcement detailing the public preview of Bring Your Own Key (BYOK) support for GitHub Copilot, empowering enterprises to use their own LLM provider keys and centrally manage model access within organizations.<!--excerpt_end-->

# Enterprise Bring Your Own Key (BYOK) for GitHub Copilot: Public Preview

GitHub Copilot now supports Bring Your Own Key (BYOK) for enterprise customers, making it possible for organizations to link their own API keys from providers such as Anthropic, Microsoft Foundry, OpenAI, and xAI. This update provides greater flexibility and centralized control over model selection in development environments.

## Key Capabilities

- **Flexible Model Choice**: Teams can connect their preferred LLM provider's API key and access all associated models through Copilot in supported environments.
- **Centralized Administration**:
  - Enterprise admins can add and configure API keys at the enterprise level.
  - Organization admins can control access within specific organizations.
  - Centralized settings make management and configuration efficient and consistent.
- **IDE Integration**: BYOK is supported in multiple development environments including Visual Studio Code, JetBrains IDEs, Eclipse, and Xcode in Agent, Plan, Ask, and Edit modes.
- **Flexible Billing**: API usage through BYOK is billed directly by the chosen provider and bypasses GitHub Copilot usage quotas. Enterprises can leverage existing provider credits or contracts.

## Limitations

- **OpenAI Responses API Unsupported**: The BYOK feature does not support the OpenAI Responses API. Models must utilize the OpenAI Completions API to function within Copilot.

## Getting Started

To enable BYOK for your organization:

1. Go to enterprise or organization settings in GitHub.
2. Add and configure your preferred LLM provider's API key.
3. Assign access to specific organizations or teams as needed.

More detailed documentation can be found [here](https://docs.github.com/en/copilot/how-tos/administer-copilot/manage-for-enterprise/use-your-own-api-keys).

## Feedback and Community

GitHub invites feedback on this new feature via the [GitHub Community discussion](https://github.com/orgs/community/discussions/179954), helping shape future updates.

*Disclaimer: Features in public preview may receive UI or functionality changes.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-20-enterprise-bring-your-own-key-byok-for-github-copilot-is-now-in-public-preview)
