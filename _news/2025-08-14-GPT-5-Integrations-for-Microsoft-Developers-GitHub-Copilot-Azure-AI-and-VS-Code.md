---
layout: "post"
title: "GPT-5 Integrations for Microsoft Developers: GitHub Copilot, Azure AI, and VS Code"
description: "This news post provides a developer-oriented overview of how OpenAI's GPT-5 model is integrated across Microsoft’s developer tools and platforms, including GitHub Copilot, Visual Studio Code, Azure AI Foundry, and Copilot Studio. It covers new GPT-5 capabilities, quick-start integration steps, model access details, and real-world usage examples for coding, DevOps, and building AI-powered applications."
author: "Jon Galloway, Pamela Fox, Dan Wahlin"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/blog/gpt-5-for-microsoft-developers"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/feed"
date: 2025-08-14 17:00:24 +00:00
permalink: "/2025-08-14-GPT-5-Integrations-for-Microsoft-Developers-GitHub-Copilot-Azure-AI-and-VS-Code.html"
categories: ["AI", "Azure", "Coding", "DevOps", "GitHub Copilot"]
tags: ["AI", "AI Toolkit", "API Integration", "Azure", "Azure AI Foundry", "Azure OpenAI Service", "C#", "Coding", "Copilot Studio", "DevOps", "GitHub Copilot", "GitHub Models", "GPT 5", "JavaScript", "LLM", "Model Evaluation", "News", "OpenAI .NET SDK", "Python", "RAG", "Reasoning Models", "VS", "VS Code"]
tags_normalized: ["ai", "ai toolkit", "api integration", "azure", "azure ai foundry", "azure openai service", "csharp", "coding", "copilot studio", "devops", "github copilot", "github models", "gpt 5", "javascript", "llm", "model evaluation", "news", "openai dotnet sdk", "python", "rag", "reasoning models", "vs", "vs code"]
---

Jon Galloway, Pamela Fox, and Dan Wahlin deliver a deep dive into the integration of GPT-5 across GitHub Copilot, Azure AI, and core Microsoft developer tools, offering practical code samples and guidance for immediate adoption.<!--excerpt_end-->

# GPT-5 Integrations for Microsoft Developers: GitHub Copilot, Azure AI, and VS Code

**Authors:** Jon Galloway, Pamela Fox, Dan Wahlin

OpenAI’s GPT-5 model has landed, and Microsoft’s developer ecosystem now features GPT-5 integrations across GitHub Copilot, Visual Studio Code, Azure AI Foundry, Copilot Studio, and more. This article provides a dev-focused roundup: how to access GPT-5, what’s new, and how to start building with it immediately.

## GPT-5 Overview

- **Improved reasoning and accuracy** for complex workflows
- **Unified capabilities** (chat, agents, coding, multimodal, advanced math)
- **Faster response times** and larger context handling
- **Available in API and developer tools**

## Where You Can Use GPT-5 Today

### GitHub Copilot

- Leverages GPT-5 for richer, longer code completions and intelligent suggestions
- Integrates with VS Code, Visual Studio, JetBrains, Xcode, and Eclipse (preview availability varies)
- Accessible in all Copilot plans, with [GPT-5 mini](https://aka.ms/vscode-github-changelog-gpt5-mini) as a starting point

### Visual Studio Code (AI Toolkit)

- Experiment with GPT-5 via the AI Toolkit
- Connects to GitHub Models, Azure AI Foundry, and supports both cloud and open-source/local backends
- [Official announcement](https://techcommunity.microsoft.com/blog/azuredevcommunityblog/gpt-5-family-of-models--gpt-oss-are-now-available-in-ai-toolkit-for-vs-code/4441394)

### Azure AI Foundry

- Enterprise-grade, secure access to GPT-5 (requires registration for main model; mini/nano/chat variants are available without registration)
- Regional availability: East US 2, Sweden Central
- Supports long-running agents and structured outputs ([details](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/concepts/models#gpt-5))

### GitHub Models Marketplace

- Supports multiple GPT-5 variants for rapid experiment and workflow integration
- [Marketplace link](https://github.com/marketplace/models)

### Microsoft Copilot Studio

- Makers can use GPT-5 models for orchestrating agents and building advanced chat/automation flows

### OpenAI .NET SDK, Python, JavaScript

- Official SDKs for .NET, Python, and JavaScript enable GPT-5 integration in C#, Python, and JS/TypeScript apps with new API features

#### Sample: C# (Streaming, Reasoning)

```csharp
OpenAIResponseClient client = new(model: "gpt-5", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
await foreach (var update in client.CreateResponseStreamingAsync(
    userInputText: "Explain beta-reduction in lambda calculus.",
    new ResponseCreationOptions { ReasoningOptions = new ResponseReasoningOptions { ReasoningEffortLevel = ResponseReasoningEffortLevel.High } }))
{
    if (update is StreamingResponseContentPartDeltaUpdate delta) {
        Console.Write(delta.Text);
    }
}
```

#### Sample: Python (Controllable Reasoning & Verbosity)

```python
import os
import openai
from azure.identity import DefaultAzureCredential, get_bearer_token_provider

client = openai.AzureOpenAI(
    api_version=os.environ["AZURE_OPENAI_VERSION"],
    azure_endpoint=os.environ["AZURE_OPENAI_ENDPOINT"],
    azure_ad_token_provider=get_bearer_token_provider(DefaultAzureCredential(), "https://cognitiveservices.azure.com/.default")
)
response = client.chat.completions.create(
    model=os.environ["AZURE_OPENAI_DEPLOYMENT"],
    messages=[{"role": "user", "content": "Explain beta-reduction in lambda calculus."}],
    reasoning_effort="minimal",
    verbosity="low"
)
print(response.choices[0].message.content)
```

#### Sample: JavaScript (Structured Output, Azure Foundry Integration)

```javascript
import { AzureOpenAI } from "openai";
import dotenv from "dotenv";
dotenv.config();
const client = new AzureOpenAI({
    endpoint: process.env.AZURE_INFERENCE_ENDPOINT,
    apiKey: process.env.AZURE_INFERENCE_KEY,
    apiVersion: "2025-01-01-preview",
    deployment: process.env.AZURE_OPENAI_DEPLOYMENT || "gpt-5",
});
const schema = { /* custom JSON schema as in full article */ };
const result = await client.chat.completions.create({
    model: deployment,
    messages: [{ role: "system", content: "Return JSON only." }, { role: "user", content: "What is 23 * 7? Show your steps." }],
    response_format: { type: "json_schema", json_schema: schema },
});
const data = JSON.parse(result.choices[0].message?.content ?? "{}" );
console.log("Steps:", data.steps);
console.log("Answer:", data.answer);
```

### DevOps & Evaluation

- Use the [Azure AI Evaluation SDK](https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/evaluation/azure-ai-evaluation) for model quality and cost assessment
- Try model variants in side-by-side playgrounds via Azure AI Foundry or GitHub Models
- .NET apps: Ensure evaluation consistency with [Microsoft.Extensions.AI Evaluation](https://learn.microsoft.com/en-us/dotnet/ai/conceptual/evaluation-libraries)

## Real-World Examples & Community

- **Pamela Fox**: Deep dive into GPT-5 for RAG (retrieval-augmented generation)
- **Anthony Shaw**: Updates GitHub Models CLI for GPT-5 family and explores LLM use in GitHub Actions for automated PR summaries, code review, and CI workflows
- **Burke Holland**: “Vibe coding” experiments in VS Code—GPT-5 builds a fully working website and game

## Resources & Quick Links

- [GPT-5 availability for Visual Studio Code](https://techcommunity.microsoft.com/blog/azuredevcommunityblog/gpt-5-family-of-models--gpt-oss-are-now-available-in-ai-toolkit-for-vs-code/4441394)
- [GPT-5 integration notes for Visual Studio](https://github.blog/changelog/2025-08-12-openai-gpt-5-is-now-available-in-public-preview-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [Official OpenAI GPT-5 announcement](https://openai.com/index/gpt-5-new-era-of-work/)
- [Azure OpenAI model docs](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/concepts/models#gpt-5)

---

Microsoft’s dev ecosystem is GPT-5 ready, enabling fast, scalable adoption for coding, DevOps, AI applications, and more.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/blog/gpt-5-for-microsoft-developers)
