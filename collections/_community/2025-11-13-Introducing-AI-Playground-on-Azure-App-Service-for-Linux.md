---
layout: "post"
title: "Introducing AI Playground on Azure App Service for Linux"
description: "This post details the new AI Playground feature in Azure App Service for Linux, designed to help developers interact with Small Language Models (SLMs) via a user-friendly UI. It provides live prompt testing, latency measurement, code integration examples, and troubleshooting tips—all accessible through the Kudu endpoint. The walkthrough includes setup steps, supported models, and planned roadmap enhancements."
author: "TulikaC"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-ai-playground-on-azure-app-service-for-linux/ba-p/4469497"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-13 04:36:41 +00:00
permalink: "/2025-11-13-Introducing-AI-Playground-on-Azure-App-Service-for-Linux.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "AI Playground", "App Performance", "App Service Extensions", "Azure", "Azure App Service", "Azure Portal", "BitNet", "C#", "Code Integration", "Coding", "Community", "Kudu", "Linux App Service", "Model Latency", "Node.js", "Performance Metrics", "Phi", "Prompt Engineering", "Python", "SLM Sidecar", "Small Language Model", "Web App Deployment"]
tags_normalized: ["ai", "ai playground", "app performance", "app service extensions", "azure", "azure app service", "azure portal", "bitnet", "csharp", "code integration", "coding", "community", "kudu", "linux app service", "model latency", "nodedotjs", "performance metrics", "phi", "prompt engineering", "python", "slm sidecar", "small language model", "web app deployment"]
---

TulikaC introduces AI Playground for Azure App Service on Linux, a built-in tool that lets developers test prompt interactions with Small Language Models, measure performance, and easily grab code samples for integration.<!--excerpt_end-->

# Introducing AI Playground on Azure App Service for Linux

AI Playground is a new, lightweight built-in tool designed for developers using Azure App Service (Linux) who want to experiment easily with Small Language Models (SLMs) running as a sidecar with their web apps.

## What is AI Playground?

AI Playground provides a simple UI accessible directly from the Kudu endpoint of your Linux App Service. It allows you to:

- Send system and user prompts to an attached SLM (such as Phi or BitNet)
- View immediate responses in-line
- Examine performance metrics including Time to First Token (TTFT), total response time, and tokens per second
- Copy ready-to-use code snippets (C#, Python, Node.js, etc.) for direct integration into your application
- Receive configuration guidance if no sidecar SLM is detected

Learn more about setting up sidecar SLMs: [Set up an SLM as a Sidecar](https://learn.microsoft.com/en-us/azure/app-service/tutorial-ai-slm-dotnet)

## How to Access AI Playground

1. In the Azure portal, open your App Service (Linux).
2. Choose **Advanced Tools (Kudu)** and click **Go**.
3. Use the left navigation to select **AI Playground**.

> **Pre-requisite**: SLM sidecar must be configured with your application. See [setup tutorial](https://learn.microsoft.com/en-us/azure/app-service/tutorial-ai-slm-dotnet).

## Core Features

### Prompts Panel

- Enter a **System Prompt** to modulate SLM behavior (e.g., “You speak like a pirate.”).
- Send a **User Prompt** and hit **Send to SLM**.

### Performance Metrics

- **TTFT**: Monitors how quickly the first token is generated for responsiveness checks.
- **Total**: Overall time to complete the response.
- **Tokens/sec**: Throughput metric for text generation.

### Code Integration

- The right sidebar presents minimal working code snippets in C#, Python, and Node.js, ready to drop into your app without leaving the Kudu interface.

> _Tip: To optimize SLM performance, keep prompts concise. If responses slow, reduce prompt size or output length._

### SLM Detection and Guidance

- If no SLM sidecar is detected, AI Playground displays a notice with setup steps and links.
- Find the full tutorial here: [AI SLM Sidecar setup](https://learn.microsoft.com/en-us/azure/app-service/tutorial-ai-slm-dotnet)

## Troubleshooting

- **No responses/timeouts:** Ensure your sidecar container is running (Deployment Center → Containers), and check its endpoint/port.
- **Slow TTFT or tokens/sec:**
  - Try several short prompts to warm up.
  - Consider scaling to a Premium plan for better resources.
  - Keep prompts and response requests brief for optimal speed.

## Roadmap

- Support for custom, bring-your-own LLMs
- Expanded evaluation features (prompt presets, saved sessions, exportable traces)
- Improved observability (per-call logs, quick Log Stream access)

## Conclusion

AI Playground helps you deliver and test AI-powered features quickly on Azure App Service for Linux. You can prompt, measure, and integrate code directly—all within your web app environment. Continuous improvements are planned, including broader model support and enhanced evaluation tooling.

For detailed configuration and ongoing updates, check the official [Azure App Service documentation](https://learn.microsoft.com/en-us/azure/app-service/tutorial-ai-slm-dotnet).

*Author: TulikaC*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-ai-playground-on-azure-app-service-for-linux/ba-p/4469497)
