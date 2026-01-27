---
external_url: https://devblogs.microsoft.com/visualstudio/better-models-smarter-defaults-claude-sonnet-4-gpt-4-1-and-more-control-in-visual-studio/
title: 'Better Models, Smarter Defaults: Enhanced Copilot Model Selection and Controls in Visual Studio'
author: Rhea Patel
feed_name: Microsoft DevBlog
date: 2025-06-25 23:24:03 +00:00
tags:
- AI Models
- Billing Updates
- Claude Sonnet 4
- Copilot
- Developer Tools
- Gemini Pro
- GPT 4.1
- Inline Suggestions
- Model Selection
- Models
- Premium Requests
- Productivity
- Usage Management
- VS
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Rhea Patel presents substantial updates to GitHub Copilot in Visual Studio, including smarter default AI models, more options for model customization, and tools to monitor and manage usage, making the Copilot experience more responsive and transparent for developers.<!--excerpt_end-->

## Major Improvements to Copilot in Visual Studio

### Smarter Default Model

GitHub Copilot in Visual Studio now uses **GPT-4.1** as its default model, replacing the prior default (GPT-4o). Based on internal testing, GPT-4.1 provides significantly better performance, with faster responses, higher quality suggestions, and greater overall efficiency for developers.

### Expanded Model Selection

Users can now choose from a wider range of AI models in Visual Studio:

- **Claude Sonnet 4**
- **Claude Opus 4**
- **Claude Sonnet 3.5**
- **Claude 3.7** ("non-thinking" and "thinking" variants)
- **OpenAI o3 mini**
- **Gemini 2.0 Flash**
- **Gemini 2.5 Pro**

Selected models are now "sticky": your choice persists across threads, providing a smoother workflow. Unsure which model to use? [Documentation is available](https://docs.github.com/copilot/using-github-copilot/ai-models/choosing-the-right-ai-model-for-your-task) describing the strengths and ideal use cases for each model.

To streamline the workflow, model enablement and switching are now simpler. If your subscription provides access to a model but it isn’t yet enabled, a prompt now appears directly within the model picker—eliminating the need to navigate to GitHub settings separately.

### Usage Management and Billing Updates

With GitHub’s new [consumptive billing model](https://github.blog/changelog/2025-06-18-update-to-github-copilot-consumptive-billing-experience/), Visual Studio adds new features to keep users informed and in control:

- **Copilot Consumptions Panel:** Monitor your usage by clicking the Copilot badge in Visual Studio and selecting "Copilot Consumptions". See how many premium requests have been used across features like chat and inline suggestions.
- **Plan Management:** From the same interface, quickly access your subscription to update or adjust GitHub Copilot settings.
- **Automatic Model Switch:** If your premium request quota runs out, Copilot switches automatically to the standard GPT-4.1 model at no added cost.
- **Transparent Quota Usage:** Some models count more heavily toward your quota; the usage impact for each is clearly indicated in the model picker before selecting.

### Additional Resources

- **Visual Studio Hub:** Stay up to date by visiting the [Visual Studio Hub](https://visualstudio.microsoft.com/hub/) for latest release notes, videos, social updates, and community conversations.
- **Feedback Opportunities:** Developers are encouraged to provide feedback through [Developer Community](https://developercommunity.visualstudio.com/VisualStudio), helping to further enhance Visual Studio and Copilot experiences.

---

These improvements aim to make Copilot in Visual Studio both more powerful and more transparent for developers, allowing for tailored model selection, easy tracking of requests and usage, and more control over the AI-powered coding experience.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/better-models-smarter-defaults-claude-sonnet-4-gpt-4-1-and-more-control-in-visual-studio/)
