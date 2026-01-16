---
layout: "post"
title: "Discrepancies Between Azure AI Foundry Playground and API Responses for GPT-4o-mini"
description: "A developer encounters unexpected inconsistencies between results from Azure AI Foundry's Chat Playground and the official API when classifying user messages with GPT-4o-mini. This post investigates potential hidden system prompts or differences in pre-processing that might explain divergent outputs, calling for input on how to replicate Playground accuracy through API or LangChain."
author: "Rakanid"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure/weird-problem-when-comparing-the-answers-from-chat-playground/m-p/4486090#M22407"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-01-15 20:34:31 +00:00
permalink: "/2026-01-15-Discrepancies-Between-Azure-AI-Foundry-Playground-and-API-Responses-for-GPT-4o-mini.html"
categories: ["AI", "Azure"]
tags: ["AI", "AI Model Behavior", "API Integration", "Azure", "Azure AI Foundry", "Azure OpenAI Service", "AzureChatOpenAI", "Chat Playground", "Chatbot Development", "Classification Logic", "Community", "GPT 4o Mini", "LangChain", "Prompt Engineering", "Python", "Response Discrepancy", "System Prompt"]
tags_normalized: ["ai", "ai model behavior", "api integration", "azure", "azure ai foundry", "azure openai service", "azurechatopenai", "chat playground", "chatbot development", "classification logic", "community", "gpt 4o mini", "langchain", "prompt engineering", "python", "response discrepancy", "system prompt"]
---

Rakanid describes issues with inconsistent results when using Azure AI Foundry's GPT-4o-mini in the Chat Playground versus the API, and seeks advice from the community on understanding and bridging this gap.<!--excerpt_end-->

# Discrepancies Between Azure AI Foundry Playground and API Responses for GPT-4o-mini

**Author:** Rakanid

## Problem Overview

Rakanid is developing a chatbot using Azure AI Foundry with the GPT-4o-mini model. The chatbot classifies each user message as a follow-up, repeat, or a new query. However, a puzzling behavior has emerged:

- **In the Azure AI Foundry Chat Playground**, the classification logic works *perfectly*.
- **Using the same prompt via Python** (through LangChain's `AzureChatOpenAI()` or the official Azure OpenAI SDK with `client.chat.completions.create()`), the results are "totally different and often wrong."

## Troubleshooting Steps Taken

Rakanid has already ruled out the obvious configuration mismatches:

- Same deployment name (`gpt-4o-mini`)
- Same temperature, top_p, and max_tokens
- Identical system and user messages
- System prompt copied directly from the Playground

## Observed Differences

Despite matching all visible parameters, the API version doesn’t match the Playground's response quality. The Playground outputs are consistently more accurate than the API or LangChain outputs.

## Theory: Hidden Prompts or Scaffolding?

Rakanid suspects that the Chat Playground may be applying additional, potentially hidden system prompts, pre-processing steps, or output formatting behind the scenes—elements that are not visible to users and are not included in the provided "View Code" snippet.

## Community Questions

Rakanid asks:

- Does the Chat Playground use hidden instructions or pre-processing that isn’t exposed?
- Is there any way to view those hidden prompts or precisely replicate Playground behavior through the API or LangChain?

## Call for Help

Has anyone in the community experienced similar discrepancies, or does anyone have suggestions for achieving identical results between the Azure AI Foundry Playground and direct API usage? Any insights or workarounds would be greatly appreciated.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure/weird-problem-when-comparing-the-answers-from-chat-playground/m-p/4486090#M22407)
