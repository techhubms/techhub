---
layout: "post"
title: "Deprecation Notice: Gemini 3 Pro and GPT-5.1 Models in GitHub Copilot"
description: "This news update informs GitHub Copilot users and administrators about the upcoming deprecation of several AI models, including Gemini 3 Pro and GPT-5.1, across all Copilot experiences. It outlines deprecation dates, suggested replacement models, steps for updating workflows, and guidance for Copilot Enterprise administrators."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-03-02-upcoming-deprecation-of-gemini-3-pro-and-gpt-5-1-models"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-03-02 20:47:00 +00:00
permalink: "/2026-03-02-Deprecation-Notice-Gemini-3-Pro-and-GPT-51-Models-in-GitHub-Copilot.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "AI Integration", "AI Models", "Copilot", "Copilot Chat", "Copilot Enterprise", "Copilot Settings", "Enterprise Administration", "Gemini 3 Pro", "GitHub Copilot", "GPT 5.1", "GPT 5.3 Codex", "Model Deprecation", "Model Selector", "News", "Retired", "VS Code", "Workflow Update"]
tags_normalized: ["ai", "ai integration", "ai models", "copilot", "copilot chat", "copilot enterprise", "copilot settings", "enterprise administration", "gemini 3 pro", "github copilot", "gpt 5dot1", "gpt 5dot3 codex", "model deprecation", "model selector", "news", "retired", "vs code", "workflow update"]
---

Allison announces the planned deprecation of Gemini 3 Pro and GPT-5.1 models in GitHub Copilot, providing key dates, alternatives, and guidance for developers and administrators.<!--excerpt_end-->

# Deprecation Notice: Gemini 3 Pro and GPT-5.1 Models in GitHub Copilot

GitHub Copilot will deprecate several AI models across all Copilot experiences, including Copilot Chat, inline edits, ask and agent modes, and code completions.

## Models and Deprecation Dates

| Model                 | Deprecation Date | Suggested Alternative   |
|-----------------------|------------------|------------------------|
| Gemini 3 Pro          | 2026-03-26       | Gemini 3.1 Pro         |
| GPT-5.1               | 2026-04-01       | GPT-5.3-Codex          |
| GPT-5.1-Codex         | 2026-04-01       | GPT-5.3-Codex          |
| GPT-5.1-Codex-Mini    | 2026-04-01       | GPT-5.3-Codex          |
| GPT-5.1-Codex-Max     | 2026-04-01       | GPT-5.3-Codex          |

The accelerated deprecation of Gemini 3 Pro is due to [provider deprecation](https://docs.cloud.google.com/vertex-ai/generative-ai/docs/models/gemini/3-pro).

## Action Required

- **Update workflows** and integrations to utilize supported models before the listed deprecation dates.
- **Copilot Enterprise administrators** should enable access to alternative models through model policies in Copilot settings.
- Confirm model availability in the Copilot Chat model selector in Visual Studio Code or on github.com.
- No manual action is needed to remove deprecated models after their retirement.

## For Enterprise Customers

GitHub Enterprise users with questions or concerns are encouraged to contact their account manager for support.

## Learn More and Provide Feedback

- Find additional details in the [Copilot model documentation](https://docs.github.com/copilot/reference/ai-models/supported-models?utm_source=model-release-announcement&utm_medium=changelog&utm_campaign=newmodels).
- Join the [GitHub Community Copilot Conversations](https://github.com/orgs/community/discussions/categories/copilot-conversations?utm_source=model-release-announcement&utm_medium=changelog&utm_campaign=newmodels) to share your feedback and experiences.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-02-upcoming-deprecation-of-gemini-3-pro-and-gpt-5-1-models)
