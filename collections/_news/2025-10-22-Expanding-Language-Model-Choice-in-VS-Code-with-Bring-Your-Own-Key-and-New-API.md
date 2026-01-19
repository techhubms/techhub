---
external_url: https://code.visualstudio.com/blogs/2025/10/22/bring-your-own-key
title: Expanding Language Model Choice in VS Code with Bring Your Own Key and New API
author: Olivia Guzzardo McVicker, Pierce Boggan
viewing_mode: external
feed_name: Visual Studio Code Releases
date: 2025-10-22 00:00:00 +00:00
tags:
- AI Extensions
- AI Toolkit
- Azure AI Foundry
- BYOK
- Cerebras
- Code Generation
- Custom LLM
- Extension Development
- GitHub Copilot Chat
- Hugging Face
- Language Model Chat Provider API
- LLM Integration
- Model Management
- Ollama
- OpenAI Compatible
- VS Code
- VS Code Extensions
section_names:
- ai
- coding
- github-copilot
---
Olivia Guzzardo McVicker and Pierce Boggan outline how new APIs and BYOK in Visual Studio Code allow developers to integrate and manage a broader range of AI models—including custom and third-party LLMs—enriching code chat and generation tools.<!--excerpt_end-->

# Expanding Language Model Choice in VS Code with Bring Your Own Key and New API

**Authors:** Olivia Guzzardo McVicker, Pierce Boggan  
**Published:** October 22, 2025

Visual Studio Code (VS Code) has significantly advanced its AI integration by launching the Language Model Chat Provider API and enhancing the Bring Your Own Key (BYOK) experience. These features are designed to give developers greater control and flexibility over the AI models used for chat, code generation, and productivity in VS Code.

## Key Highlights

- **Bring Your Own Key (BYOK):** Enables users to connect any supported large language model (LLM) to VS Code by supplying their own API key. Providers include OpenAI, OpenRouter, Ollama, Google, Hugging Face, and more.
- **Language Model Chat Provider API:** Allows extension developers to contribute their own models by building VS Code extensions, further extending the range of available models.
- **Extensible Ecosystem:** Users can now access a variety of specialized models (e.g., for code, chat, or local experimentation) directly from VS Code through extensions or custom providers.

## Bring Your Own Key (BYOK) — What Is It?

BYOK empowers you to use virtually any supported language model by configuring your provider API key within VS Code. This opens up the possibility to:

- Use specific or specialized models for coding or chat.
- Experiment with cloud or local LLMs, such as those available via Ollama or custom-tuned models from Azure AI Foundry.
- Set up these connections using the **Chat: Manage Language Models** command in VS Code.

## Language Model Chat Provider API: Enabling an Open, Extensible AI Model Ecosystem

This API lets model providers deliver their models through VS Code extensions. The result is an open landscape where developers can:

- Install extensions to access additional LLMs.
- Leverage built-in and community-contributed providers for both general and specialized use cases.

> *Currently, models provided through this API are available to users on GitHub Copilot Individual plans (Free, Pro, Pro+).*

### Notable Extensions

- [AI Toolkit for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-windows-ai-studio.windows-ai-studio): Access models, including those tuned in Azure AI Foundry or run via Foundry Local.
- [Cerebras Inference](https://marketplace.visualstudio.com/items?itemName=cerebras.cerebras-chat): Provides access to high-performance code generation models (such as Qwen3 Coder, GPT OSS 120B) optimized for speed.
- [Hugging Face Provider for GitHub Copilot Chat](https://marketplace.visualstudio.com/items?itemName=HuggingFace.huggingface-vscode-chat): Enables use of frontier open LLMs (e.g., Kimi K2, DeepSeek V3.1, GLM 4.5) from Hugging Face directly inside VS Code.

## For Extension Developers

Developers can integrate their own models by referencing the [Language Model Chat Provider API documentation](https://code.visualstudio.com/api/extension-guides/ai/language-model-chat-provider) and building from the [sample extension](https://github.com/microsoft/vscode-extension-samples/tree/main/chat-model-provider-sample).

## OpenAI-Compatible Models

Support is available for custom OpenAI-compatible providers using the dedicated OpenAI Compatible provider within VS Code Insiders. This allows configuration for any compatible endpoint and explicit model/tool mapping via the `github.copilot.chat.customOAIModels` setting.

![Managing OpenAI-Compatible Models in VS Code](/assets/blogs/2025/10/22/manage-openai-compatible.png)

## Looking Ahead

Future enhancements under consideration include:

- Richer UI for model management and discovery.
- Streamlined installation/activation of model provider extensions.
- Improved integration with built-in tools and smarter prompts.

Recent updates feature improved edit tools for custom models, but users should note that BYOK does not yet support code completions. As VS Code evolves, feedback is welcomed on [GitHub](https://github.com/microsoft/vscode) to help shape these capabilities.

## Conclusion

The new Language Model Chat Provider API and BYOK significantly extend the flexibility of AI-powered development in VS Code, empowering users to select, integrate, and manage LLMs tailored to their specific coding needs.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/10/22/bring-your-own-key)
