---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/gpt-5-freeform-tool-calling-in-typescript-on-azure-ai-foundry/ba-p/4469682
title: Implementing GPT-5 FreeForm Tool Calling with Azure AI Foundry
author: Julia_Muiruri
feed_name: Microsoft Tech Community
date: 2025-11-19 18:29:45 +00:00
tags:
- Agentic Workflow
- API Integration
- Azure AI Foundry
- Bar Chart
- CSV
- Custom Tools
- Data Analysis
- FreeForm Tool Calling
- Function Calling
- GPT 5
- Iris Dataset
- JavaScript
- SQL
- TypeScript
- VM Sandbox
- AI
- Azure
- Coding
- Machine Learning
- Community
section_names:
- ai
- azure
- coding
- ml
primary_section: ai
---
In this tutorial, Julia Muiruri illustrates how developers can leverage GPT-5’s FreeForm tool calling within Azure AI Foundry to execute SQL and JavaScript workflows for advanced analytic tasks, highlighting agentic automation and custom tool design.<!--excerpt_end-->

# GPT-5 FreeForm Tool Calling in TypeScript on Azure AI Foundry

Julia Muiruri demonstrates a practical, code-heavy workflow where a Data Analyst automates species-level mean calculations and visualization for the Iris dataset using a GPT-5 agent.

## Scenario Overview

Carl, a Data Analyst, avoids manual SQL and chart logic. Instead, he prompts an internal GPT-5 Agent: *"Compute species-level means, then visualize petal length."*

- **Custom Tool 1:** `sql_exec_csv` – Runs raw SQL on an Iris dataset, returns CSV.
- **Custom Tool 2:** `code_exec_javascript` – Executes raw JavaScript in a secure VM, parses CSV, generates and saves an SVG chart.

GPT-5 orchestrates multi-stage execution:

1. Generates SQL, receives CSV results.
2. Emits JavaScript code for table and chart display.
3. Chains these steps without JSON schema wrapping.

## What Is FreeForm Tool Calling?

FreeForm tool calling allows GPT-5 to invoke registered tools with arbitrary, unstructured text payloads—SQL, JavaScript, Python scripts, Bash, and more—avoiding JSON argument schemas.

### Key Benefits

- **No Schema Friction:** Use the tool’s native language (SQL, JS), skip JSON wrapping/parsing.
- **Improved Reasoning:** Mixed output (code, natural language) facilitates better AI workflows.
- **Multi-Step Chaining:** Each tool’s output re-enters the workflow as plain text for further model reasoning.

### Key Differences

| Dimension               | Structured Function (JSON Schema)         | FreeForm Custom Tools           |
|-------------------------|--------------------------------------------|---------------------------------|
| Payload Shape           | JSON arguments                            | Raw text (code/script)          |
| Validation              | Automatic via schema                      | Semantic, at execution          |
| Parsing Overhead        | JSON mapping                              | Minimal (direct string)         |
| Ease of Evolution       | Update schema/code                        | Update prompt/tool description  |
| Observability           | Nested JSON hides code                    | Natural language/code visible   |
| Chaining Complexity     | Must parse JSON per step                  | Direct string passing           |
| Errors                  | JSON structure errors                     | Runtime execution errors        |

## When *Not* To Use FreeForm

- Strict validation (financial, PII)
- Complex, deeply nested data
- Mass extraction (needs consistent JSON for parsing)

Hybrid designs can mix structured and custom tools for validation and execution tasks.

## Implementation Breakdown

### Prerequisites

- Azure AI Foundry project
- GPT-5 deployment (v1 API)

### User Workflow

1. Prompt GPT-5 to write SQL (means on Iris species) for CSV output.
2. Prompt to generate JavaScript for parsing the CSV, printing a table, and rendering a bar chart (mean petal length by species).

**Tool Registry:**

- `sql_exec_csv`: Executes SQL, returns CSV string.
- `code_exec_javascript`: Executes JavaScript, produces chart and console table.

**Execution Flow:**

- Model emits raw SQL (not JSON).
- Tool executes query, returns CSV.
- Model emits JavaScript (not schema-wrapped).
- Tool executes code, prints table, saves SVG chart.

**Benefits for Developers:**

- Eliminates friction of JSON parsing; tools work with direct code.
- Enables creative, expressive, and rapid prototyping using agentic patterns.
- Azure AI Foundry supports governance and scaling for production deployments.

## Reference Links & Resources

- [Unlocking GPT-5’s Freeform Tool Calling: Microsoft DevBlog](https://devblogs.microsoft.com/foundry/unlocking-gpt-5s-freeform-tool-calling-a-new-era-of-seamless-integration/)
- [OpenAI Cookbook: Free-Form Function Calling](https://cookbook.openai.com/examples/gpt-5/gpt-5_new_params_and_tools#2-freeform-function-calling)
- [Python & TypeScript Code Samples on GitHub](https://github.com/Azure-Samples/insideAIF/tree/main/Samples/Freeform-Tool-Calling)

---

**Author:** Julia Muiruri

**Published:** Nov 14, 2025

**Platform:** Microsoft Developer Community Blog

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/gpt-5-freeform-tool-calling-in-typescript-on-azure-ai-foundry/ba-p/4469682)
