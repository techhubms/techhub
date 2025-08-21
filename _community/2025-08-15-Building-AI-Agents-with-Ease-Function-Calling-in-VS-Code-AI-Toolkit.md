---
layout: "post"
title: "Building AI Agents with Ease: Function Calling in VS Code AI Toolkit"
description: "This guide by shreyanfern explains how developers can leverage the VS Code AI Toolkit to create actionable AI agents using function calling. It covers the core technical process for enabling Large Language Models (LLMs) to interact with external APIs and data, specifically within Microsoft's AI Toolkit extension. The tutorial includes hands-on agent building steps, configuration tips, sample Python code using the Azure AI Inference SDK, and insights into extending agent capabilities through the Model Context Protocol (MCP). Suitable for developers aiming to apply advanced AI tooling in real-world applications."
author: "shreyanfern"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/educator-developer-blog/building-ai-agents-with-ease-function-calling-in-vs-code-ai/ba-p/4442637"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-15 17:00:24 +00:00
permalink: "/2025-08-15-Building-AI-Agents-with-Ease-Function-Calling-in-VS-Code-AI-Toolkit.html"
categories: ["AI", "Azure", "Coding"]
tags: ["Agent Development", "AI", "AI Toolkit", "API Integration", "Azure", "Azure AI Inference SDK", "Coding", "Community", "Custom Tool Development", "Function Calling", "GitHub", "GPT", "Large Language Models", "LLM Integration", "MCP", "OpenWeather API", "Prompt Engineering", "Python", "Tool Calling", "VS Code"]
tags_normalized: ["agent development", "ai", "ai toolkit", "api integration", "azure", "azure ai inference sdk", "coding", "community", "custom tool development", "function calling", "github", "gpt", "large language models", "llm integration", "mcp", "openweather api", "prompt engineering", "python", "tool calling", "vs code"]
---

shreyanfern details how to build actionable AI agents using function calling in the VS Code AI Toolkit, including real examples and code for Azure integration.<!--excerpt_end-->

# Building AI Agents with Ease: Function Calling in VS Code AI Toolkit

## Overview

Function calling empowers Large Language Models (LLMs) to execute real-world actions by interacting with external data sources such as APIs and databases. This guide outlines the practical steps to build and test AI agents in Visual Studio Code using Microsoft’s AI Toolkit, highlighting both conceptual foundations and hands-on techniques.

---

## What is Function Calling?

Function calling is a mechanism that enables an AI agent—the intelligent “brain” powered by an LLM—to take action through external tools, such as APIs or code execution. Analogous to hands for the human brain, function calls allow an agent to dynamically fetch information, process requests, and generate useful, context-driven outputs.

- **Client and LLM Interaction**: User requests are processed by the LLM, which analyzes available tools and determines the correct function call (e.g., querying weather data).
- **Tool Definition Components**:
  - *Name*: Unique identifier for the tool
  - *Description*: Explains what the tool does and when to use it
  - *Input Parameters*: Required inputs for execution

This process allows AI to go beyond its training data and perform meaningful, up-to-date actions.

---

## Step-by-Step: Developing an Agent with AI Toolkit

### 1. Setting Up

- **Install VS Code AI Toolkit**: [Get the extension](https://aka.ms/AIToolkit) and complete installation per [this blog](https://techcommunity.microsoft.com/blog/educatordeveloperblog/visual-studio-code-ai-toolkit-run-llms-locally/4163192) or the [AI Sparks YouTube Series](https://www.youtube.com/watch?v=crFcWa_9hK0&list=PLmsFUfdnGr3yysvu8fPA9ka5gW2fkJci1).
- **Launch Agent Builder**: (Previously 'Prompt Builder')

### 2. Creating a Custom Tool

- Click on the “+” icon and select **Custom Tool**.
- Choose the example tool (e.g., `get_weather`) or upload a custom schema.
- Example tool schema:

  ```json
  {
    "type": "object",
    "properties": {
      "location": { "type": "string", "description": "The city and state e.g. San Francisco, CA" },
      "unit": { "type": "string", "enum": [ "c", "f" ] }
    },
    "additionalProperties": false,
    "required": [ "location" ]
  }
  ```

- The example tool populates fields automatically; custom tools require manual schema definition.

### 3. Building and Testing the Agent

- After adding your tool, test the agent using a prompt like: “Do I need to carry an umbrella today in Bangalore?”
- Run the model via the Toolkit. The LLM determines:
  - Whether external data is required
  - Which tool/function to call and with what parameters
- Simulate/enter a mock weather API response (e.g., "rainy").
- Run again to get a final conversational answer from the agent: e.g., “Yes, you should carry an umbrella today in Bangalore, as it is rainy. Stay dry!”

### 4. Deployment & Code Generation

- The AI Toolkit supports code generation in multiple languages.
- For Python with Azure integration, select Python and use the Azure Inference SDK.
- Example Python code snippet:

  ```python
  '''Run this model in Python
  > pip install azure-ai-inference
  '''
  import os
  from azure.ai.inference import ChatCompletionsClient
  from azure.ai.inference.models import AssistantMessage, SystemMessage, UserMessage, ToolMessage
  from azure.core.credentials import AzureKeyCredential

  # Authenticate with a GitHub Personal Access Token (PAT)
  client = ChatCompletionsClient(
      endpoint = "https://models.github.ai/inference",
      credential = AzureKeyCredential(os.environ["GITHUB_TOKEN"]),
      api_version = "2024-08-01-preview",
  )

  def get_weather():
      return "rainy"

  messages = [
      SystemMessage(content = "You are a helpful AI Assistant."),
      UserMessage(content = [TextContentItem(text = "Do I need to carry an umbrella today in Bangalore?")]),
  ]

  tools = [
      {
          "type": "function",
          "function": {
              "name": "get_weather",
              "description": "Determine weather in my location",
              "parameters": {
                  "type": "object",
                  "properties": {
                      "location": {"type": "string", "description": "The city and state e.g. San Francisco, CA"},
                      "unit": {"type": "string", "enum": [ "c", "f" ]}
                  },
                  "additionalProperties": False,
                  "required": ["location"]
              }
          }
      }
  ]

  response_format = "text"

  while True:
      response = client.complete(
          messages=messages,
          model="openai/gpt-4o",
          tools=tools,
          response_format=response_format,
          temperature=1,
          top_p=1,
      )

      if response.choices[0].message.tool_calls:
          print(response.choices[0].message.tool_calls)
          messages.append(response.choices[0].message)
          for tool_call in response.choices[0].message.tool_calls:
              messages.append(ToolMessage(
                  content=locals()[tool_call.function.name](),
                  tool_call_id=tool_call.id,
              ))
      else:
          print(f"[Model Response] {response.choices[0].message.content}")
          break
  ```

---

## Further Learning

- The blog covers only basic agent creation; more advanced features and multi-step agentic systems will be showcased in future posts.
- For more, explore the Model Context Protocol (MCP) and AI Sparks series.

---

*Author: shreyanfern*

---

## References

- [AI Toolkit Documentation](https://aka.ms/AIToolkit)
- [Model Context Protocol Blog](https://techcommunity.microsoft.com/blog/educatordeveloperblog/unlocking-ai-potential-exploring-the-model-context-protocol-with-ai-toolkit/4411198)
- [Visual Studio Code AI Toolkit](https://techcommunity.microsoft.com/blog/educatordeveloperblog/visual-studio-code-ai-toolkit-run-llms-locally/4163192)
- [AI Sparks YouTube Playlist](https://www.youtube.com/playlist?list=PLmsFUfdnGr3yysvu8fPA9ka5gW2fkJci1)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/educator-developer-blog/building-ai-agents-with-ease-function-calling-in-vs-code-ai/ba-p/4442637)
