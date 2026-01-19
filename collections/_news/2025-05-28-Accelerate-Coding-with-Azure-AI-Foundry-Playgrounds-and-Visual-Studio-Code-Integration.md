---
external_url: https://devblogs.microsoft.com/foundry/open-in-vscode/
title: Accelerate Coding with Azure AI Foundry Playgrounds and Visual Studio Code Integration
author: Thasmika Gokal
viewing_mode: external
feed_name: Microsoft DevBlog
date: 2025-05-28 19:01:04 +00:00
tags:
- Agent Workflow
- AI Development
- AI Playground
- API Endpoint
- Azd Commands
- Azure AI Foundry
- C#
- Code Samples
- Entra ID Authentication
- GitHub Integration
- Model Deployment
- Open in VS Code
- Prototyping
- Python
- VS Code
- VS Code Extension
section_names:
- ai
- azure
- coding
---
Authored by Thasmika Gokal, this article delves into the enhanced AI developer experience provided by Azure AI Foundry playgrounds and the 'Open in VS Code' workflow, detailing integration strategies for seamless prototyping and deployment in Visual Studio Code.<!--excerpt_end-->

# Accelerate Coding with Azure AI Foundry Playgrounds and Visual Studio Code Integration

**Author: Thasmika Gokal**

## Introduction

In today’s fast-paced development environment, speed and clarity are essential. For those working with state-of-the-art AI agents and models, **Azure AI Foundry playgrounds** provide a low-friction, on-demand environment tailored for rapid prototyping, API exploration, and technical validation—serving as a technical sketchpad for developers.

Modern application development typically involves juggling APIs, services, SDKs, and data models before committing to a framework or infrastructure. The rise in software ecosystem complexity heightens demand for safe, lightweight environments to validate ideas efficiently. Azure AI Foundry playgrounds meet this need by preloading comprehensive tooling, native API support, and advanced features, including code generation, chain of thought summaries, and parameter tuning.

## The 'Open in VS Code' Workflow

Announced at [Microsoft Build 2025](https://www.youtube.com/watch?v=HfgMrIuM1Ng&t=2115s), the **'Open in VS Code' workflow** enhances developer productivity by allowing users to:

- Transfer agent/model API endpoints, keys, and code samples from Azure AI Foundry into a fresh workspace in Visual Studio Code for Web.
- Reduce friction and speed up time-to-code for experimentation with AI agents and models.
- Seamlessly move from rapid prototyping to deployment, all within a familiar development environment.

### One-Click Workflow Highlights

- With a **single click**, agent/model details and sample code are imported into a VS Code Web workspace via the Azure AI Foundry extension.
- Developers can run agents/models locally, leverage `azd` commands to provision and deploy a complete web app, and even push their workspace structure directly to GitHub for continued work in VS Code Desktop.
- The process supports flexible handoff across Azure AI Foundry, Visual Studio Code, and GitHub, streamlining the developer journey.

## Step-by-Step Guide — Open in VS Code

1. **Model Selection:**
   - Use the [Azure AI Foundry portal](https://ai.azure.com/?cid=devblogs) to select the best model (e.g., o3, o4-mini, MAI-DS-R1, or gpt-4o-mini).
   - Example: Select *gpt-4o-mini* for an agent workflow.

2. **Model Provisioning:**
   - Deploy the endpoint using the model card in the portal.

3. **Agent Playground:**
   - Navigate to the agents playground to adjust generation controls (e.g., max responses, past messages).
   - Add external knowledge, tools, and actions as needed.

4. **Iterate and Experiment:**
   - Refine prompts and continue experimentation directly in the playground environment.

5. **View Code Samples:**
   - Click the **View Code** button to reveal sample code tailored to your interaction—available in Python, C#, and JavaScript, as well as JSON, cURL, and Go for models.
   - Entra ID authentication can be set up for agents with "Key Authorization" now available for models.

6. **Open in VS Code:**
   - Press **Open in VS Code** to launch a new workspace in VS Code for Web under the `/azure` environment.
   - Agent/model API endpoint, key, and code samples are automatically imported.

7. **Workspace Setup:**
   - The terminal automatically sets the API key as an environment variable.
   - Files are downloaded, including README instructions, environment variable scripts, a requirements.txt file, and `agent_run.py` for sample execution.

8. **Run Locally:**
   - Execute the model locally using `python agent_run.py`. Successful responses appear within seconds.

9. **Provision and Deploy with `azd`:**
   - Use `azd init` to initialize a git repo and set up an Azure workspace.
   - Run `azd up` to provision Azure resources and deploy app code.
   - After deployment, `azd show` lets you inspect running Azure services and deployed web applications.

10. **Continue on Desktop:**
    - Use the **Continue on Desktop** option to publish the workspace to GitHub, enabling seamless transitions to VS Code Desktop or configured dev containers.

## Getting Started

- **Sign-in or sign-up to [Azure AI Foundry](https://ai.azure.com?cid=devblogs).**
- **Create a Foundry Hub and/or Project.**
- **Deploy models such as o4-mini, o3, or gpt-4.1 via Foundry Models or the playground interfaces.**
- **Click “Open in VS Code” in code samples and follow the guided workflow.**

## Resources to Dive Deeper

- [Azure AI Foundry Portal](https://ai.azure.com?cid=devblogs)
- [Visual Studio Code Extension for Foundry](https://marketplace.visualstudio.com/items?itemName=TeamsDevApp.vscode-ai-foundry)
- [Azure AI Foundry SDK](https://aka.ms/aifoundrysdk)
- [Learn courses for Azure AI Foundry](https://aka.ms/CreateAgenticAISolutions)
- [Azure AI Foundry Documentation](https://learn.microsoft.com/azure/ai-foundry/)
- Join the conversation on [GitHub](https://aka.ms/azureaifoundry/forum) and [Discord](https://aka.ms/azureaifoundry/discord)

## Conclusion

The "Open in VS Code" workflow within the Azure AI Foundry environment dramatically decreases the time from model selection and experimentation to prototyping and deployment. Leveraging tight integrations with Visual Studio Code and GitHub, developers can iterate rapidly and operationalize AI solutions in minutes—focusing on innovation, not setup.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/foundry/open-in-vscode/)
