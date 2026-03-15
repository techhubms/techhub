---
external_url: https://github.blog/changelog/2026-03-12-copilot-auto-model-selection-is-generally-available-in-jetbrains-ides
title: GitHub Copilot Auto Model Selection Launches for JetBrains IDEs
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-03-12 15:54:33 +00:00
tags:
- AI
- AI Model Selection
- Billing
- Copilot
- Copilot Enterprise
- Developer Tools
- GitHub Copilot
- GPT 5.3 Codex
- GPT 5.4
- Haiku 4.5
- IDEs
- JetBrains
- Model Multiplier
- News
- Premium Requests
- Software Release
- Sonnet 4.6
section_names:
- ai
- github-copilot
---
Allison presents the general availability of GitHub Copilot’s auto model selection in JetBrains IDEs, highlighting new flexibility and billing improvements for developers.<!--excerpt_end-->

# GitHub Copilot Auto Model Selection for JetBrains IDEs

GitHub Copilot’s auto model selection feature is now generally available in JetBrains IDEs across all Copilot plans. This enhancement allows Copilot to automatically choose the best-fitting AI model for each code completion based on real-time availability and performance factors.

## Key Features

- **Dynamic Model Routing:** Copilot dynamically selects from models like GPT-5.4, GPT-5.3-Codex, Sonnet 4.6, and Haiku 4.5, depending on your subscription and administrator policies. The routing logic adapts over time as models evolve.
- **Transparency:** Developers can easily see which AI model generated a suggestion by hovering over the model response within their IDE.
- **Control:** Users retain the ability to manually select specific models or switch to auto mode at any time.
- **Policy Compliance:** The auto selection system respects any admin-defined model policies and restrictions, aligning with organizational requirements.

## Premium Request Billing

- **Dynamic Billing:** Billing for premium requests aligns with the selected model’s multiplier, offering a 10% discount on the multiplier for requests routed through auto mode (e.g., a 1x multiplier model only consumes 0.9 premium requests).
- **Supported Models:** Currently, auto routes to models with multipliers from 0x to 1x, which include GPT-5.4, GPT-5.3-Codex, Sonnet 4.6, and Haiku 4.5, but this may expand in the future.

## Roadmap and Feedback

- **Enhancements:** Copilot auto selection will become even more intelligent soon, factoring in task complexity to select models best matched to the work developers are doing.
- **Community Discussion:** Users are encouraged to share feedback and questions in the Copilot Conversations on GitHub’s Community forums.

For more information on Copilot premium billing and model multipliers, see the [official documentation](https://docs.github.com/copilot/concepts/billing/copilot-requests#model-multipliers).

---
*Author: Allison*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-12-copilot-auto-model-selection-is-generally-available-in-jetbrains-ides)
