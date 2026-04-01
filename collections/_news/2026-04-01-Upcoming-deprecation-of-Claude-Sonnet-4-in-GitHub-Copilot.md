---
feed_name: The GitHub Blog
date: 2026-04-01 00:14:43 +00:00
tags:
- Agent Mode
- AI
- Ask Mode
- Claude Sonnet 4
- Claude Sonnet 4.6
- Code Completions
- Copilot
- Copilot Chat
- Copilot Enterprise
- GitHub Copilot
- GitHub Enterprise
- Inline Edits
- Model Deprecation
- Model Policies
- Model Selector
- News
- Supported Models
- VS Code
primary_section: github-copilot
external_url: https://github.blog/changelog/2026-03-31-upcoming-deprecation-of-claude-sonnet-4-in-github-copilot
author: Allison
section_names:
- ai
- github-copilot
title: Upcoming deprecation of Claude Sonnet 4 in GitHub Copilot
---

Allison announces that GitHub Copilot will deprecate the Claude Sonnet 4 model on 2026-05-01 and explains what Copilot Enterprise admins may need to change in model policies so users can switch to a supported alternative.<!--excerpt_end-->

# Upcoming deprecation of Claude Sonnet 4 in GitHub Copilot

GitHub will deprecate a model across all GitHub Copilot experiences on **2026-05-01**.

## Deprecation details

| Model | Deprecation date | Suggested alternative |
| --- | --- | --- |
| Claude Sonnet 4 | 2026-05-01 | Claude Sonnet 4.6 |

This deprecation applies across GitHub Copilot experiences including:

- Copilot Chat
- Inline edits
- Ask mode
- Agent mode
- Code completions

## What you need to do

- Update any workflows and integrations to use supported models before **2026-05-01**.
- **Copilot Enterprise administrators** may need to enable access to alternative models via **model policies** in Copilot settings.

## Admin verification steps

As an administrator, you can confirm model availability by:

1. Checking your individual Copilot settings.
2. Confirming the policy is enabled for the specific model.
3. After enabling, verifying the model appears in the Copilot Chat model selector:
   - In **VS Code**
   - On **github.com**

No action is required to remove models once they have been deprecated.

## Questions and feedback

- GitHub Enterprise customers with concerns can contact their account manager.
- Supported models documentation: https://docs.github.com/copilot/reference/ai-models/supported-models?utm_source=model-release-announcement&utm_medium=changelog&utm_campaign=newmodels
- Share feedback in GitHub Community: https://github.com/orgs/community/discussions/categories/copilot-conversations?utm_source=model-release-announcement&utm_medium=changelog&utm_campaign=newmodels


[Read the entire article](https://github.blog/changelog/2026-03-31-upcoming-deprecation-of-claude-sonnet-4-in-github-copilot)

