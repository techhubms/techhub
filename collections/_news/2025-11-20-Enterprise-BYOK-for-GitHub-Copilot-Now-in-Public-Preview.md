---
external_url: https://github.blog/changelog/2025-11-20-enterprise-bring-your-own-key-byok-for-github-copilot-is-now-in-public-preview
title: Enterprise BYOK for GitHub Copilot Now in Public Preview
author: Allison
feed_name: The GitHub Blog
date: 2025-11-20 15:46:54 +00:00
tags:
- Admin Controls
- Anthropic
- API Key
- Billing
- BYOK
- Copilot
- Copilot Chat
- Enterprise
- IDE Integration
- LLM Providers
- Microsoft Foundry
- Model Management
- OpenAI
- Organization Admin
- VS Code
- Xai
section_names:
- ai
- devops
- github-copilot
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
