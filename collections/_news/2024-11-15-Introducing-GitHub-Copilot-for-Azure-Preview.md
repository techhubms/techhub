---
layout: "post"
title: "Introducing GitHub Copilot for Azure (Preview)"
description: "This news article introduces the preview of GitHub Copilot for Azure, a new tool that integrates GitHub Copilot Chat with Azure management directly in VS Code. The content covers features for learning about Azure services, deploying and operating resources, troubleshooting issues, and optimizing costs— all from within the coding environment using natural language and slash commands."
author: "Chris Harris"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/blogs/2024/11/15/introducing-github-copilot-for-azure"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2024-11-15 00:00:00 +00:00
permalink: "/news/2024-11-15-Introducing-GitHub-Copilot-for-Azure-Preview.html"
categories: ["AI", "Azure", "Coding", "DevOps", "GitHub Copilot"]
tags: ["AI", "Application Deployment", "Azure", "Azure AI Search", "Azure Developer CLI", "Azure OpenAI", "Azure Portal", "Azure SQL", "CI/CD", "Coding", "Container Apps", "DevOps", "Diagnostics", "GitHub Copilot", "GitHub Copilot For Azure", "GPT 4o", "Kubernetes", "News", "RAG", "Resource Management", "Slash Commands", "VS Code", "YAML"]
tags_normalized: ["ai", "application deployment", "azure", "azure ai search", "azure developer cli", "azure openai", "azure portal", "azure sql", "cislashcd", "coding", "container apps", "devops", "diagnostics", "github copilot", "github copilot for azure", "gpt 4o", "kubernetes", "news", "rag", "resource management", "slash commands", "vs code", "yaml"]
---

Chris Harris presents the preview of GitHub Copilot for Azure, guiding developers on integrating Azure resource management and troubleshooting into their workflow using GitHub Copilot Chat in VS Code.<!--excerpt_end-->

# Introducing GitHub Copilot for Azure (Preview)

_By Chris Harris, Product Manager_

GitHub Copilot for Azure is now available in preview. This extension integrates seamlessly with GitHub Copilot Chat inside Visual Studio Code, allowing developers to interact with Azure resources, deploy applications, and resolve issues—all directly from their coding environment. No need to switch between your IDE and the Azure portal or documentation.

## Key Features

- **In-Editor Azure Assistance:** Use `@azure` in Copilot Chat to get up-to-date documentation, learn about Azure services (e.g., Azure OpenAI models, Azure AI Search, Azure SQL), and ask questions without leaving VS Code.
- **Seamless Deployment:** Streamline setup and deployments, whether you're building a RAG chat app with GPT-4o, managing CI/CD pipelines, or working with the Azure Developer CLI (`azd`). Copilot guides you through app templates, commands, and configuration syntax (including YAML).
- **Troubleshooting & Diagnostics:** Quickly identify and resolve performance or resource issues using suggested prompts. Copilot for Azure facilitates diagnosis via log searches, resource recommendations, and optimization tips. For example, diagnosing slow Kubernetes clusters or application errors becomes more efficient.
- **Resource Operations:** Effortlessly manage and query Azure resources. Find out how many storage accounts or web app plans you have, segregate resources by region, or get cost breakdowns directly in your editor. Gain operational insights and optimize setup without additional context-switching.
- **Command Shortcuts:** Use slash commands (like `/help`, `/learn`, `/resources`, `/diagnose`, `/changeTenant`) for quick access to frequent tasks and information.

## How To Get Started

Download the extension from the [Visual Studio Marketplace](https://aka.ms/GetGitHubCopilotForAzure). After installing, prompt `@azure` or use slash commands inside Copilot Chat in VS Code to interact with your Azure resources and streamline your cloud workflow.

## Example Prompts

- `@azure Give me a detailed description of Azure AI Search`
- `@azure Can you help me build an RAG chat app with GPT-4o?`
- `@azure Are there any errors in the logs of my [SuperCoolDemo] Container App?`
- `/diagnose` to identify app issues
- `/learn` to get Azure learning resources

## Feedback

User feedback is encouraged via thumbs up/down or by opening issues on the [GitHub repo](https://aka.ms/GitHubCopilotForAzureRepo). The project team welcomes suggestions and bug reports.

---

With GitHub Copilot for Azure, developers can focus on coding while leveraging the full power of Azure—reducing context switches and increasing productivity.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2024/11/15/introducing-github-copilot-for-azure)
