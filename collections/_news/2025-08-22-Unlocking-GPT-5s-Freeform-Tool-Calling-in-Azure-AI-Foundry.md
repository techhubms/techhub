---
external_url: https://devblogs.microsoft.com/foundry/unlocking-gpt-5s-freeform-tool-calling-a-new-era-of-seamless-integration/
title: Unlocking GPT-5’s Freeform Tool Calling in Azure AI Foundry
author: Ananya Bishnoi
feed_name: Microsoft DevBlog
date: 2025-08-22 17:29:26 +00:00
tags:
- API Key
- Authentication
- Automation
- Azure AI Foundry
- Azure OpenAI
- Code Integration
- Developer Tools
- Entra ID
- Freeform Tool Calling
- Function Chaining
- GPT 5
- Prompt Engineering
- Python
- SQL
- Tool Calling
section_names:
- ai
- azure
- coding
primary_section: ai
---
Ananya Bishnoi demonstrates how GPT-5’s freeform tool calling in Azure AI Foundry enables developers to chain Python and SQL tools for seamless automation and dynamic workflows.<!--excerpt_end-->

# Unlocking GPT-5’s Freeform Tool Calling in Azure AI Foundry

*Author: Ananya Bishnoi*

## Introduction

GPT-5 models are now live in Azure AI Foundry through Azure OpenAI, bringing advanced reasoning and generative capabilities. One standout feature is **freeform tool calling**: the model can now send raw code—like Python scripts or SQL queries—directly to your dev tools, making integrations smoother than ever.

## What is Freeform Tool Calling in GPT-5?

Traditional tool calling with AI often requires strict JSON formatting for every payload. Freeform tool calling in GPT-5 changes the game by letting the model generate and send code or configuration files in whatever format your tool expects—including unstructured text. This streamlines development, allowing:

- Flexible interaction with a wide range of tools
- More expressive and intuitive workflows
- Support for complex, multi-step task automation

## Demo: Chaining SQL and Python Tools

Liam Cavanagh’s demo shows real-world usage by chaining together a SQL execution tool and a Python code runner:

### Step 1: Setup

- Load environment variables from a `.env` file
- Initialize Azure OpenAI client (supports API key and Entra ID authentication)

### Step 2: Define Custom Tools

- `sql_exec_sqlite`: Executes SQL, returns final SELECT as CSV
- `code_exec_python`: Executes raw Python, returns stdout

### Step 3: Prompt GPT-5

The model is prompted to:

- Generate SQL to create a sales table and compute revenue
- Call `sql_exec_sqlite` to execute it
- Call `code_exec_python` to format the result

### Step 4: Run the Conversation Loop

- Send prompt to GPT-5
- Detect and execute tool calls (locally)
- Feed output back to GPT-5 as `function_call_output`
- Print the assistant’s final response

This design lets GPT-5 maintain full context and chain multiple tool calls as part of a single workflow.

## Key Takeaways

- **No JSON Required:** Send raw code/text to tools
- **Improved Usability:** Output is natural and easier for devs to use
- **Multi-Tool Orchestration:** Seamless chaining of different processing steps
- **Developer-Centric:** Enhances automation, scripting, and dynamic code execution

## Learn More

- [What is Azure OpenAI in Azure AI Foundry Models? | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/overview)
- [GPT-5 in Azure AI Foundry: The future of AI apps and agents starts here | Microsoft Azure Blog](https://azure.microsoft.com/en-us/blog/gpt-5-in-azure-ai-foundry-the-future-of-ai-apps-and-agents-starts-here/?msockid=0030611da9e260e628f5742ca8586188)

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/foundry/unlocking-gpt-5s-freeform-tool-calling-a-new-era-of-seamless-integration/)
