---
layout: post
title: 'Microsoft and LangChain: Advancing AI Security for Open Source on Azure'
author: Marlene Mhangami
canonical_url: https://devblogs.microsoft.com/blog/microsoft-and-langchain-leading-the-way-in-ai-security-for-open-source-on-azure
viewing_mode: external
feed_name: Microsoft DevBlog
feed_url: https://devblogs.microsoft.com/feed
date: 2025-06-05 17:05:24 +00:00
permalink: /ai/news/Microsoft-and-LangChain-Advancing-AI-Security-for-Open-Source-on-Azure
tags:
- AI Security
- Autogen
- Azure AI Foundry
- Continuous Integration
- Information Leakage
- Langchain
- LangChain4J
- LangGraph
- Large Language Models
- Open Source
- Privilege Escalation
- Python
- Semantic Kernel
- Trust
section_names:
- ai
- azure
- security
---
Authored by Marlene Mhangami, this article examines Microsoft's partnership with LangChain to bolster AI security for open-source developers on Azure. The collaboration addresses integration risks while ensuring enterprises can safely accelerate innovation in the AI landscape.<!--excerpt_end-->

## Microsoft and LangChain: Advancing AI Security for Open Source on Azure

**Author: Marlene Mhangami**

### Introduction

Rapid developments in the field of artificial intelligence mean developers and enterprises often prioritize speed and execution. This fast pace has led many to adopt open-source AI tools such as LangChain—a widely used framework for building various AI applications. In response to the imperative for not just fast but also secure innovation, Microsoft has been actively collaborating to strengthen security practices within the open-source AI community, particularly for apps running on Azure.

### Background: Secure Future Initiative

Just over a year ago, Microsoft introduced the [Secure Future Initiative (SFI)](https://www.microsoft.com/trust-center/security/secure-future-initiative), aiming to enhance security for Microsoft, its customers, and the broader technology ecosystem, including open-source software. The initiative guides developers to innovate securely while moving rapidly.

### The Rise of LangChain

LangChain has emerged as one of the most downloaded Python AI frameworks, reportedly surpassing the OpenAI SDK in monthly downloads on PyPI, according to data from the LangChain Interrupt 2025 Keynote.

LangChain's popularity is driven by:

- Its building blocks for **multi-agent architectures**
- Support for a range of **large language models (LLMs)**
- Numerous third-party integrations available via its expansive community ecosystem

However, these advantages also introduce challenges, especially around ensuring the security of partner integrations and experimental technologies.

### Microsoft’s AI Security Guidance

Microsoft has published guidance that outlines two main risks associated with LLM-driven applications:

- **Information Leakage:** Unintended exposure of sensitive data to unauthorized parties, leading to possible data breaches.
- **Privilege Escalation:** Unauthorized users gaining more access than intended, posing risks to secure operations.

Building AI applications with agentic flows also typically involves code execution, evaluation, and data processing, all of which must be considered within the context of security risks.

### Open Source Security: Collaboration with LangChain

Microsoft’s commitment to advancing open source security is long-standing. In AI, the company maintains frameworks such as [Semantic Kernel](https://github.com/microsoft/semantic-kernel/) and [Autogen](https://github.com/microsoft/autogen). Continuing this mission, the Microsoft security team audited the LangChain codebase—focusing on

- [langchain-community](https://github.com/langchain-ai/langchain-community/tree/main/libs/community): third-party integrations
- [langchain-experimental](https://github.com/langchain-ai/langchain-experimental/tree/main/libs/experimental): packages for experimental and research-oriented features

Several potential security issues were identified, especially with the broader LangChain ecosystem—encompassing both official and community-contributed integrations.

#### Statement from Microsoft

> “When we examined LangChain and its associated projects, we identified several areas for security improvement to address before using it in our production systems. Microsoft is committed to making Azure the most secure place for running AI workloads, and our Developer Relations team is working with LangChain to improve security and make it easier for organizations to use safely.”  
> — Michael Scovetta, Microsoft Principal Security Assurance Manager

#### Statement from LangChain

> “Over the past year and a half we’ve taken steps to make LangChain enterprise ready. Step one in this was rearchitecting the ecosystem to make packages like langchain-community and langchain-experimental optional and separate packages. As a next step, we’re excited to work with Microsoft to support more enterprises in their journey to leverage AI safely and effectively.”  
> — Harrison Chase, LangChain Co-Founder and CEO

### Security Improvements and Engineering Collaboration

In response to these findings, Microsoft is:

- Providing engineering support, including continuous integration tools and workflows, to help detect and prevent insecure code from entering the LangChain project
- Supporting the project through [Alpha-Omega](https://alpha-omega.dev/), assisting with documentation and best practices to help organizations avoid security pitfalls
- Collaborating on the [LangChain-Azure](https://github.com/langchain-ai/langchain-azure) mono-repo, aiming to make Azure a secure platform for AI development

### LangChain Integration on Azure

To facilitate access to secure AI models and services on Azure, Microsoft provides the following resources:

- **Python:** Use the [Azure AI](https://python.langchain.com/docs/integrations/providers/microsoft/#azure-ai) package within LangChain for seamless integration
  - _Installation:_  

    ```
    pip install langchain-azure-ai
    ```

  - _Sample code:_  

    ```python
    from langchain_azure_ai.chat_models import AzureAIChatCompletionsModel
    from langchain_core.messages import HumanMessage, SystemMessage

    model = AzureAIChatCompletionsModel(
        endpoint="https://{your-resource-name}.services.ai.azure.com/models",
        credential="your-api-key",
        model="deepseek/DeepSeek-R1-0528"
    )

    messages = [HumanMessage(content="Translate the following from English into Italian: 'hi!'")]
    message_stream = model.stream(messages)
    print("".join(chunk.content for chunk in message_stream))
    ```

- **JavaScript:** The [langchain-azure-js](https://github.com/langchain-ai/langchain-azure-js) package enables Azure integration for JavaScript developers
- **Java:** The [LangChain4J](https://docs.langchain4j.dev/integrations/language-models/azure-open-ai/) package has community-maintained support for Azure OpenAI integrations

### Looking Ahead

Microsoft and LangChain see this collaborative effort as a pioneering model for securing open-source AI applications at scale. Microsoft’s dedication to transparency, partnership, and trust is intended to provide enterprise developers with secure, innovative tools as they build on Azure.

### References

- [Secure Future Initiative](https://www.microsoft.com/trust-center/security/secure-future-initiative)
- [LangChain](https://www.langchain.com/)
- [Microsoft AI Security Guidance](https://www.microsoft.com/en-us/microsoft-cloud/blog/2025/04/23/securing-ai-navigating-risks-and-compliance-for-the-future/?msockid=1f49e2daf44a6b4d3ec0f7c8f5da6a30)
- [Semantic Kernel](https://github.com/microsoft/semantic-kernel/)
- [Autogen](https://github.com/microsoft/autogen/)
- [Alpha-Omega](https://alpha-omega.dev/)
- [LangChain-Azure](https://github.com/langchain-ai/langchain-azure)

---

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/blog/microsoft-and-langchain-leading-the-way-in-ai-security-for-open-source-on-azure)
