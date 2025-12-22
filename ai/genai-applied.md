---
layout: "page"
title: "GenAI Applied"
description: "Practical ways to apply GenAI, from development workflows to product integration, plus resources to keep learning."
image: "/assets/ai-to-z/logo.png"
category: "AI"
page-name: "genai-applied"
---

This page focuses on using GenAI in the real world: spotting where it already shows up, deciding what to build, and integrating it into products and developer workflows.

Related pages:

- [GenAI Basics](/ai/genai-basics.html)
- [GenAI Advanced](/ai/genai-advanced.html)

## Table of Contents

1. [Inspiration](#inspiration)
2. [Integrating AI into Your Applications](#integrating-ai-into-your-applications)
3. [Want to Know More?](#want-to-know-more)
4. [Conclusion](#conclusion)

## Inspiration

What can you actually build with AI? Beyond feature lists, this page collects practical patterns and project ideas that you can implement and iterate on.

### Where You Find AI Today

AI isn’t just in dedicated “AI apps.” It’s embedded everywhere you already work and live. Knowing where it shows up helps you spot opportunities to use it in your own projects.

**In Your Development Workflow**
Your code editor probably already has AI. GitHub Copilot suggests code as you type. Chat interfaces help debug problems. Pull requests get AI-generated summaries. Testing tools can create test cases automatically.

**In Everyday Software**
Your email client drafts responses. Your browser summarizes articles. Your phone transcribes voicemails. Your photo app organizes pictures by faces and locations. These started as separate research projects and became “normal” features.

**In Business Applications**
CRM systems score leads automatically. Accounting software categorizes expenses. Customer service platforms route inquiries. The AI runs in the background, making existing workflows smarter.

**Behind the Scenes**
Search engines understand natural language. Translation works in real time. Streaming services recommend content. Social platforms detect harmful content. The AI infrastructure is invisible but essential.

**What This Means for Builders**
AI success comes from integration, not replacement. The most useful AI features enhance existing workflows rather than creating entirely new ones.

**Start Where You Are**
Look at repetitive tasks in your current work. What takes time but doesn’t require creativity? What patterns could be automated? What decisions could be supported with better summaries or better data?

**More information:**

- [Chat in IDE](https://youtu.be/TorMsn9bjLY)
- [Copilot vs Chat: Sidekick Showdown - When to Use Each Coding Sidekick](https://cooknwithcopilot.com/blog/copilot-vs-chat-sidekick-showdown.html)
- [What's new with the GitHub Copilot coding agent: A look at the updates](https://www.youtube.com/watch?v=vgPl6sK6rQo)
- [AI Challenger: Loft Orbital - Building Smarter Satellites with AI](https://www.youtube.com/watch?v=lGtTnFlI6yA)

### Real Projects You Can Build Today

AI shows up across the stack even when you don’t open a chat app.

**Build Your Own Satellite Ground Station**
AI can help you process telemetry, predict orbital paths, and analyze satellite imagery and space weather.
See [From Space to Subsurface: Predicting Gold Zones with Azure AI and Machine Learning](https://techcommunity.microsoft.com/t5/ai-machine-learning/from-space-to-subsurface-using-azure-ai-to-predict-gold-rich/m-p/4441134#M258).

**Create Multi-Agent Systems That Actually Work**
Move beyond single assistants to systems where multiple agents collaborate.
See [Building a multi-agent system with Semantic Kernel](https://www.reddit.com/r/dotnet/comments/1ltr8tf/building_a_multiagent_system_with_semantic_kernel/).

**Connect AI to Real-World Data with MCP**
Stop limiting AI to training data. Build Model Context Protocol servers that give models access to live databases, APIs, and services.
See [Model Context Protocol Development Best Practices](https://www.youtube.com/watch?v=W56H9W7x-ao).

**Zero-Trust AI Agents for Enterprise**
Build agentic systems that can operate in secure environments with proper identity and access controls.
See [Zero Trust Agents: Adding Identity and Access to Multi-Agent Workflows](https://techcommunity.microsoft.com/t5/ai-azure-ai-services-blog/zero-trust-agents-adding-identity-and-access-to-multi-agent/ba-p/4427790).

### Beyond Chat: AI as Infrastructure

The most interesting AI applications aren’t chatbots. They’re systems that make everything else smarter.

**AI-Powered Development Workflows**
Go beyond code completion and build workflows that use AI to understand the system you are shipping.
See [From Vibe Coding to Vibe Engineering: It's Time to Stop Riffing with AI](https://thenewstack.io/from-vibe-coding-to-vibe-engineering-its-time-to-stop-riffing-with-ai/).

**Scientific Research Acceleration**
AI can analyze papers, generate hypotheses, and even control lab workflows.
See [Introducing Microsoft Discovery: An Agentic AI Platform for Scientific Research](https://www.youtube.com/watch?v=k3S4lPbUWng).

**Visual Data Processing at Scale**
Build systems that can analyze thousands of images, extract insights from video, or process satellite imagery. Computer vision capabilities are now accessible through APIs.

### Where to Start Building

**Start Small, Think Big**
Begin with a problem you face regularly. Can AI help you analyze data faster, draft better documentation, or triage incoming requests?

**Use Existing Building Blocks**
You usually don’t need to train models from scratch. Start with pre-trained models through APIs, then build your differentiation in how you connect context, constraints, and workflows.

**Learn by Doing**
Build something small, test it with real examples, then iterate. When you change prompts or tool choices, you should see measurable changes in output quality.

**Use an AI-native SDLC**
If you’re shipping AI features, treat prompts, evaluations, and guardrails as first-class artifacts and build them into your delivery process.
See [AI Native SDLC](/ai/sdlc.html).

## Integrating AI into Your Applications

Most practical integrations come down to three layers:

- **Experience**: how users interact (chat, inline suggestions, forms, copilots)
- **Orchestration**: prompts, tool use, retrieval, evaluations, and guardrails
- **Data and systems**: your sources of truth (docs, DBs, APIs) and authorization

### Tools and IDEs

Modern development environments have integrated AI capabilities to enhance productivity and streamline workflows.

**Visual Studio Code**

- GitHub Copilot integration: code suggestions and chat in the editor
- AI-powered extensions for specific languages and frameworks
- Context-aware completions based on your repository

**Visual Studio**

- AI-assisted IntelliSense, suggestions, and review support
- Debugging help with explanations and next-step suggestions

**JetBrains Rider**

- AI assistant support for code generation and explanation
- Refactoring suggestions and test generation workflows

**Benefits of AI-integrated development tools**

- Faster coding with contextual suggestions
- Better code quality through review support
- Learning support through explanations
- Reduced context switching

### Copilot

Microsoft’s Copilot family represents AI assistance across different tools and workflows.

**GitHub Copilot**

- Code completion: suggests functions and code blocks based on context
- Chat interface: conversational help for reasoning about code and architecture
- Multi-language support

**Interaction modes**

**Ask** is best for explanations, options, and quick checks.

**Edit** is best for refining existing code, docs, and configs.

**Agent** is best for multi-step tasks where the AI plans and executes changes.

#### Copilot FAQ

**When should I refactor using my IDE versus using Copilot?**

- Use IDE refactoring tools for standard operations like renaming symbols or extracting methods.
- Use Copilot for refactors that require understanding intent, architecture, and cross-file reasoning.

Lots of examples and detailed information can be found in the [GitHub Copilot Hub](/github-copilot/).

**More information:**

- [What's new with the GitHub Copilot coding agent: A look at the updates](https://www.youtube.com/watch?v=vgPl6sK6rQo)
- [Copilot vs Chat: Sidekick Showdown - When to Use Each Coding Sidekick](https://cooknwithcopilot.com/blog/copilot-vs-chat-sidekick-showdown.html)
- [Modernizing Legacy COBOL to Cloud with GitHub Copilot](https://www.youtube.com/watch?v=xWA0xYttWMo)
- [GitHub Copilot Helps One Acre Fund Scale Farming Impact](https://www.youtube.com/watch?v=ol_un2Nam2E)
- [Introducing automatic documentation comment generation in Visual Studio](https://devblogs.microsoft.com/visualstudio/introducing-automatic-documentation-comment-generation-in-visual-studio/)
- [VS Code June 2025 (version 1.102)](https://code.visualstudio.com/updates/v1_102)
- [GitHub Copilot in 2025: More Intelligent, More Accessible, More Productive](https://github.blog/news-insights/product-news/github-copilot-in-2025-more-intelligent-more-accessible-more-productive/)
- [Building AI Agents with Ease: Function Calling in VS Code AI Toolkit](https://techcommunity.microsoft.com/blog/educatordeveloperblog/building-ai-agents-with-ease-function-calling-in-vs-code-ai-toolkit/4362419)

### Azure AI Services

Microsoft Azure provides AI services that range from end-to-end platforms to specialized APIs.

**Azure AI Foundry**
AI Foundry is a unified platform for building, evaluating, and deploying AI applications.

**Azure OpenAI**
Enterprise access to OpenAI models with Azure security and compliance features.

**Do you need to use low-level services directly?**
For many applications, you can start with Azure OpenAI or AI Foundry, then add specialized services when needed.

**More information:**

- [Azure Update: 20th June 2025](https://www.youtube.com/watch?v=2L4cSig9Y4Y)
- [Azure Updates: August 2025 Highlights](https://www.youtube.com/watch?v=-8sH0QFhvkQ)
- [What's New in Azure AI Foundry: July 2025 Releases and Updates](https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-july-2025/)
- [Introducing Deep Research in Azure AI Foundry Agent Service](https://azure.microsoft.com/en-us/blog/introducing-deep-research-in-azure-ai-foundry-agent-service/)
- [Agent Factory: Building Your First AI Agent with Azure AI Foundry](https://techcommunity.microsoft.com/blog/azure-ai-services-blog/agent-factory-building-your-first-ai-agent-with-azure-ai-foundry/4295871)

### Languages and SDKs

Most languages have solid SDKs and libraries for integrating AI.

**Python**

- OpenAI SDK
- LangChain
- Hugging Face Transformers
- LlamaIndex

**JavaScript/TypeScript**

- OpenAI Node.js SDK
- LangChain.js
- Vercel AI SDK

**C#/.NET**

- Microsoft.Extensions.AI
- Agent Framework (successor to Semantic Kernel)
- Azure AI SDKs

**More information:**

- [.NET AI Community Standup: AI in .NET - What's New, What's Next](https://www.youtube.com/watch?v=pt4CJKm-2ZI)
- [Connecting to a Local MCP Server Using Microsoft.Extensions.AI](https://www.youtube.com/watch?v=iYHh5n-6ez4)

### Semantic Kernel

Semantic Kernel is an SDK for orchestrating prompts, tools, and workflows in application code. Microsoft also highlights Microsoft Agent Framework for higher-level agentic systems.

**More information:**

- [Semantic Kernel documentation](https://learn.microsoft.com/en-us/semantic-kernel/)
- [Semantic Kernel GitHub repository](https://github.com/microsoft/semantic-kernel)
- [Semantic Kernel and Microsoft.Extensions.AI: Better Together, Part 2](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-and-microsoft-extensions-ai-better-together-part-2/)
- [Migration guide from Semantic Kernel to Agent Framework](https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-semantic-kernel)

### Microsoft Agent Framework

Microsoft Agent Framework is a framework for building, orchestrating, and deploying AI agents.

**More information:**

- [Microsoft Agent Framework documentation](https://learn.microsoft.com/en-us/agent-framework/)
- [Agent Framework GitHub repository](https://github.com/microsoft/agent-framework)
- [Semantic Kernel and Microsoft Agent Framework announcement](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-and-microsoft-agent-framework/)
- [Introducing Microsoft Agent Framework blog post](https://devblogs.microsoft.com/foundry/introducing-microsoft-agent-framework-the-open-source-engine-for-agentic-ai-apps/)
- [Agent Framework introduction video (30 min)](https://www.youtube.com/watch?v=AAgdMhftj8w)

#### Semantic Kernel FAQ

**Adding Azure OpenAI vs OpenAI in C# - what are the differences?**

- Azure OpenAI offers enterprise security, data residency control, and Azure integration.
- Direct OpenAI can be simpler to start with but gives less control over data handling.

## Want to Know More?

### Learning resources

- [Microsoft Learn](https://learn.microsoft.com/en-us/training/paths/introduction-to-ai-on-azure/)
- [Hugging Face](https://huggingface.co/)
- [Rob Bos' LinkedIn Learning Course on AI development with GitHub models and Azure](https://www.linkedin.com/learning/enterprise-ai-development-with-github-models-and-azure)
- [GitHub Skills](https://skills.github.com/)
- [Let's build GPT: from scratch, in code, spelled out](https://www.youtube.com/watch?v=kCc8FmEb1nY)

### More content from this site

- [AI-focused videos and tutorials](/ai/videos.html)
- [Blog posts about AI implementation](/ai/posts.html)
- [Microsoft and Azure updates](/all/posts.html)

### Hands-on learning

- Start with a simple chat interface to understand AI interactions.
- Try GitHub Copilot or similar tools in your development environment.
- Experiment with prompt changes and measure output quality.
- Build a small application using Azure OpenAI or GitHub Models.
- Join AI communities and forums and learn from examples.

## Conclusion

GenAI becomes useful when it sits inside real workflows, uses trusted context, and has clear guardrails. Start small, measure quality, and expand as you gain confidence.

**Key takeaways:**

- Start simple and keep scope bounded.
- Treat evaluation and safety as part of the build, not as an afterthought.
- Prefer integrations that make existing workflows faster and more reliable.

**Next steps:**

1. Try existing tools (GitHub Copilot, Microsoft Copilot) to build intuition.
2. Pick one workflow you know well and improve one step with a prototype.
3. Add basic evaluation before you scale usage.
