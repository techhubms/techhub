---
external_url: https://techcommunity.microsoft.com/t5/azure/weird-problem-when-comparing-the-answers-from-chat-playground/m-p/4486090#M22407
title: Discrepancies Between Azure AI Foundry Playground and API Responses for GPT-4o-mini
author: Rakanid
feed_name: Microsoft Tech Community
date: 2026-01-15 20:34:31 +00:00
tags:
- AI Model Behavior
- API Integration
- Azure AI Foundry
- Azure OpenAI Service
- AzureChatOpenAI
- Chat Playground
- Chatbot Development
- Classification Logic
- GPT 4o Mini
- LangChain
- Prompt Engineering
- Python
- Response Discrepancy
- System Prompt
section_names:
- ai
- azure
primary_section: ai
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
