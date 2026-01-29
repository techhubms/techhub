---
external_url: https://devblogs.microsoft.com/visualstudio/bring-your-own-model-visual-studio-chat/
title: Bring Your Own Language Model to Visual Studio Chat
author: Rhea Patel, Samruddhi Khandale
feed_name: Microsoft DevBlog
date: 2025-08-20 15:40:20 +00:00
tags:
- AI Providers
- Anthropic
- API Integration
- BYOK
- BYOM
- Chat
- Claude
- Copilot
- Customization
- Developer Tools
- Gemini
- Google
- GPT
- Key
- Large Language Models
- Models
- OpenAI
- VS
- AI
- Coding
- News
section_names:
- ai
- coding
primary_section: ai
---
Rhea Patel and Samruddhi Khandale introduce a new Visual Studio Chat feature allowing developers to bring their own language models from providers like OpenAI, Anthropic, and Google, enabling customization, flexibility, and direct API integration.<!--excerpt_end-->

# Bring Your Own Language Model to Visual Studio Chat

_Authors: Rhea Patel, Samruddhi Khandale_

Developers working in Visual Studio Chat can now connect their own large language models by providing API keys from providers such as OpenAI, Anthropic, and Google. This enhancement allows users to:

- **Select from a wider range of language models** by integrating third-party or latest releases immediately
- **Customize workflows** to match organizational security, infrastructure, or performance needs
- **Directly manage usage, quotas, and billing** with chosen providers
- **Switch between Copilot’s built-in models and external models** for enhanced flexibility

## How to Enable

1. Open the **Chat Window** in Visual Studio.
2. Click on **Manage Models** from the model picker.
3. Choose your preferred provider and enter your API key.
4. Select from preset models or enter a custom model name.
5. The connected model will appear in your model picker for use in chat.

## Key Considerations

- **Feature Scope:** This applies strictly to Chat within Visual Studio, not completions, commit messages, or other AI features.
- **Model Capability Differences:** Not all external models offer features like tool use or vision support.
- **Copilot Business/Enterprise Restriction:** This integration is unavailable for Copilot Business and Copilot Enterprise users.
- **Partial Integration:** Embeddings, repo indexing, and intent detection still use Copilot APIs.
- **Responsible AI:** Output from third-party models may bypass Copilot’s responsible AI filters—use with awareness.

Further support for additional AI providers is anticipated. For more details or to try it out, visit [Visual Studio](https://visualstudio.microsoft.com/).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/bring-your-own-model-visual-studio-chat/)
