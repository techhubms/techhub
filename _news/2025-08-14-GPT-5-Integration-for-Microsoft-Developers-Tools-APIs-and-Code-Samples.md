---
layout: "post"
title: "GPT-5 Integration for Microsoft Developers: Tools, APIs, and Code Samples"
description: "An in-depth overview of how Microsoft developers can leverage OpenAI's GPT-5 across Microsoft products and services. This article summarizes GPT-5's rollout into GitHub Copilot, Azure AI Foundry, Copilot Studio, Visual Studio Code, and .NET SDK, with hands-on integration tips, code samples in C#, Python, and JavaScript, evaluation approaches, and resource links. Includes real-world usage examples and guidance for starting with GPT-5 in the Microsoft ecosystem."
author: "Jon Galloway, Pamela Fox, Dan Wahlin"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/blog/gpt-5-for-microsoft-developers"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/feed"
date: 2025-08-14 17:00:24 +00:00
permalink: "/2025-08-14-GPT-5-Integration-for-Microsoft-Developers-Tools-APIs-and-Code-Samples.html"
categories: ["AI", "Azure", "Coding", "GitHub Copilot"]
tags: [".NET SDK", "Agentic Reasoning", "AI", "AI Toolkit", "API Integration", "Azure", "Azure AI Foundry", "Azure API", "Azure OpenAI", "C#", "Coding", "Copilot Studio", "GitHub Copilot", "GPT 5", "JavaScript", "Model Evaluation", "Multimodal AI", "News", "Python", "RAG", "Structured Output", "Visual Studio", "Visual Studio Code"]
tags_normalized: ["net sdk", "agentic reasoning", "ai", "ai toolkit", "api integration", "azure", "azure ai foundry", "azure api", "azure openai", "c", "coding", "copilot studio", "github copilot", "gpt 5", "javascript", "model evaluation", "multimodal ai", "news", "python", "rag", "structured output", "visual studio", "visual studio code"]
---

Jon Galloway, Pamela Fox, and Dan Wahlin present a comprehensive guide for developers on using GPT-5 within Microsoft platforms. The post covers integration in GitHub Copilot, Azure AI Foundry, Copilot Studio, VS Code, and .NET SDK, including technical examples and best practices.<!--excerpt_end-->

# GPT-5 Integration for Microsoft Developers: Tools, APIs, and Code Samples

Stay at the forefront of AI development with a detailed rundown of GPT-5 support and integration options across Microsoft's developer ecosystem, brought to you by Jon Galloway, Pamela Fox, and Dan Wahlin.

## Announcement Overview

- OpenAI introduced GPT-5, focusing on improved reasoning, context handling, unified capabilities, and multimodal features.
- Microsoft rolled out GPT-5 support immediately in tools and services relevant to developers.

## Where to Use GPT-5 in the Microsoft Ecosystem

### GitHub Copilot

- Richer suggestions and enhanced code completion using GPT-5, especially on larger codebases or multifile refactoring.
- Integrated into VS Code, Visual Studio, JetBrains IDEs, Xcode, Eclipse.
- "GPT-5 mini" available in all Copilot plans.

### AI Toolkit in Visual Studio Code

- Experiment with GPT-5 via AI Toolkit: connect to GitHub Models or Azure AI Foundry, run playgrounds, and scaffold new integrations.
- Supports both cloud and OSS/local backends.

### Azure AI Foundry

- Enterprise-grade GPT-5 model hosting.
- Advanced features: structured output, model routing, agentic, and reasoning tasks.
- Initially available in East US 2 and Sweden Central (requires registration for some models).

### GitHub Models Marketplace

- Browse, experiment, and use the GPT-5 family (full, mini, nano, chat variants), including CLI access for prompt engineering and workflow automation.

### Copilot Studio

- Makers can select GPT-5 or GPT-5 chat/reasoning models for advanced agent orchestration.

### OpenAI .NET SDK

- Official .NET SDK supports GPT-5 via Responses API with features like streaming and configurable reasoning efforts.
- NuGet package available.

## Code Examples with GPT-5

### C# Example – Streaming with Reasoning Effort

```csharp
using OpenAI.Responses;
OpenAIResponseClient client = new(
    model: "gpt-5",
    apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY")
);
await foreach (var update in client.CreateResponseStreamingAsync(
    userInputText: "Explain beta-reduction in lambda calculus.",
    new ResponseCreationOptions {
        ReasoningOptions = new ResponseReasoningOptions {
            ReasoningEffortLevel = ResponseReasoningEffortLevel.High,
        },
    })) {
    if (update is StreamingResponseContentPartDeltaUpdate delta) {
        Console.Write(delta.Text);
    }
}
```

### Python Example – Fine-tuned Reasoning and Verbosity

```python
import os
import openai
from azure.identity import DefaultAzureCredential, get_bearer_token_provider
client = openai.AzureOpenAI(
    api_version=os.environ["AZURE_OPENAI_VERSION"],
    azure_endpoint=os.environ["AZURE_OPENAI_ENDPOINT"],
    azure_ad_token_provider=get_bearer_token_provider(DefaultAzureCredential(), "https://cognitiveservices.azure.com/.default"),
)
response = client.chat.completions.create(
    model=os.environ["AZURE_OPENAI_DEPLOYMENT"],
    messages=[{"role": "user", "content": "Explain beta-reduction in lambda calculus."}],
    reasoning_effort="minimal",
    verbosity="low"
)
print(response.choices[0].message.content)
```

### JavaScript Example – Structured Output

```javascript
import { AzureOpenAI } from "openai";
import dotenv from "dotenv";
dotenv.config();
const endpoint = process.env.AZURE_INFERENCE_ENDPOINT;
const key = process.env.AZURE_INFERENCE_KEY;
const deployment = process.env.AZURE_OPENAI_DEPLOYMENT || "gpt-5";
const client = new AzureOpenAI({
  endpoint,
  apiKey: key,
  apiVersion: "2025-01-01-preview",
  deployment,
});
const schema = {
  name: "math_explanation",
  schema: {
    type: "object",
    properties: {
      steps: { type: "array", items: { type: "string" } },
      answer: { type: "number" },
    },
    required: ["steps", "answer"],
    additionalProperties: false,
  },
  strict: true,
};
const result = await client.chat.completions.create({
  model: deployment,
  messages: [
    { role: "system", content: "Return JSON only." },
    { role: "user", content: "What is 23 * 7? Show your steps." },
  ],
  response_format: { type: "json_schema", json_schema: schema },
});
const content = result.choices[0].message?.content ?? "{}";
const data = JSON.parse(content);
console.log("Steps:", data.steps);
console.log("Answer:", data.answer);
```

## Evaluation and Model Comparison

- Use Azure AI Evaluation SDK and Microsoft.Extensions.AI.Evaluation for benchmarking and regression tests.
- Prefer structured output (JSON schemas) for automated quality checks.
- Explore the [RAG chat app with Azure OpenAI and Azure AI Search (Python)](https://github.com/Azure-Samples/azure-search-openai-demo/) for end-to-end deployment and evaluation workflows.

## Community and Reference Projects

- Pamela Fox: [GPT-5: Will it RAG?](https://blog.pamelafox.org/2025/08/gpt-5-will-it-rag.html)
- Anthony Shaw: [GitHub Models CLI, LLM in GitHub Actions](https://github.com/tonybaloney/llm-github-models)
- Burke Holland: [Vibe coding with GPT-5 in VS Code](https://www.youtube.com/watch?v=wqc85X2rpEY)

## Quick Start

- [Use GPT-5 in VS Code](https://techcommunity.microsoft.com/blog/azuredevcommunityblog/gpt-5-family-of-models--gpt-oss-are-now-available-in-ai-toolkit-for-vs-code/4441394)
- [Use GPT-5 in Visual Studio](https://github.blog/changelog/2025-08-12-openai-gpt-5-is-now-available-in-public-preview-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- Access all API and model docs directly from Microsoft product links above.

Microsoft's early rollout of GPT-5 unlocks powerful AI tools and deep integration for developers across its ecosystem. Use the above code samples and resources to start building and evaluating GPT-5 applications today.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/blog/gpt-5-for-microsoft-developers)
