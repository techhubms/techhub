---
layout: post
title: 'Announcing AI Shell Preview 2: Enhanced Azure PowerShell Integration and More'
author: Steven Bucher
canonical_url: https://devblogs.microsoft.com/powershell/ai-shell-preview-2/
viewing_mode: external
feed_name: Microsoft PowerShell Blog
feed_url: https://devblogs.microsoft.com/powershell/feed/
date: 2025-02-28 20:49:30 +00:00
permalink: /ai/news/Announcing-AI-Shell-Preview-2-Enhanced-Azure-PowerShell-Integration-and-More
tags:
- AI
- AI Shell
- Automation
- Azure
- Azure OpenAI
- Azure PowerShell
- Bicep
- CLI Tools
- Coding
- Copilot in Azure
- Deployment
- Developer Tools
- DevOps
- Error Handling
- Microsoft
- News
- OllamaSharp
- OpenAI Compatible Models
- PowerShell
- Windows Terminal
section_names:
- ai
- azure
- coding
- devops
---
In this update, Steven Bucher and the AI Shell Team share details on AI Shell Preview 2, including enhanced Azure PowerShell support, improved third-party AI model integrations, deployment tools, and native error handling.<!--excerpt_end-->

## Announcing AI Shell Preview 2

### What’s New in AI Shell Preview 2?

The PowerShell Team has released the **Preview 2** version of **AI Shell**, focusing on elevated Azure and AI integration for developers working with command-line automation. This release includes:

- **Improved support for Azure PowerShell**: Enhanced integration for managing Azure resources via both Azure CLI and PowerShell. Notably, you can now authenticate using `Connect-AzAccount`, and the `/replace` command more fluidly works with generated PowerShell scripts, allowing interactive parameter replacement.
- **Broader support for third-party OpenAI-compatible models**: The `openai-gpt` agent now connects to a variety of non-Microsoft, OpenAI API-compliant models, such as Ollama, LM Studio, Deepseek, LocalAI, Google Gemini, and Grok. These models can be easily added via configuration.
- **Refactored Ollama agent code**: Example code for the 'ollama' agent now uses the OllamaSharp library. The agent is distributed as a build-it-yourself example, updated with settings file configuration for more flexible integration.
- **Improved native command error handling**: `Resolve-Error` (`fixit`) uses Windows screen scraping for better diagnostic automation of non-PowerShell command errors, improving troubleshooting.
- **Easy deployment of Azure OpenAI instances**: Release includes a [Bicep template](https://raw.githubusercontent.com/PowerShell/AIShell/refs/heads/main/docs/development/AzureOAIDeployment/main.bicep) and [step-by-step documentation](https://learn.microsoft.com/powershell/utility-modules/aishell/developer/deploy-azure-openai?view=ps-modules) for quickly deploying Azure OpenAI without manual Azure portal steps.
- **Additional bug fixes**: Numerous bug fixes have been implemented; full details are available in the [changelog](https://github.com/PowerShell/AIShell/releases/tag/v1.0.0-preview.2).

### AI Shell Preview 3 Notice

Preview 3 followed quickly after Preview 2 to address a backend issue impacting the Azure Agent reliability, ensuring solid Azure service interactions.

### Native Command Error Handling Details

The new screen scraper API for `Resolve-Error` means AI Shell can now interpret and help fix errors from native applications, not just PowerShell scripts. Example output and workflow are provided in the official documentation.

### Third-Party Model Configuration Example

To configure new models, properties like `endpoint`, `key`, and `model name` are added to the agent's config file. Example:

```json
{
  "GPTs": [
    {
      "Name": "gpt-deepseek",
      "Description": "A GPT instance using DeepSeek v3.",
      "Endpoint": "https://api.deepseek.com",
      "ModelName": "deepseek-chat",
      "Key": "<your-deepseek-api-key>",
      "SystemPrompt": "You are a helpful assistant."
    },
    {
      "Name": "gpt-gemini",
      "Description": "A GPT instance using Google Gemini.",
      "Endpoint": "https://generativelanguage.googleapis.com/v1beta/openai/",
      "ModelName": "gemini-1.5-flash",
      "Key": "<your-gemini-api-key>",
      "SystemPrompt": "You are a helpful assistant."
    }
  ],
  "Active": "gpt-deepseek"
}
```

### Ollama Agent Updates

The example 'ollama' agent serves as a template for users wanting to build custom agents leveraging OllamaSharp and configuration via settings files. Full instructions are available in the project README on GitHub.

### Quick-Start Deployment for Azure OpenAI

Deployment is simplified with a reusable Bicep template, enabling rapid provisioning of Azure OpenAI. Detailed documentation provides step-wise guidance suitable for developers and IT professionals.

### Installation and Automation

You can install or upgrade AI Shell using PowerShell 7 with this command:

```powershell
Invoke-Expression "& { $(Invoke-RestMethod 'https://aka.ms/install-aishell.ps1') }"
```

For always-on integration, add `Start-AIShell` to your PowerShell profile script so that AI Shell starts automatically when using Windows Terminal (requires PowerShell 7.4.6+):

```powershell
if ($PSVersionTable.PSVersion -ge ([version]'7.4.6') -and (Get-Process -Id $pid).Parent.Name -eq 'WindowsTerminal') {
    Start-AIShell
}
```

See [about_Profiles](https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about/about_profiles#how-to-create-a-profile) for profile script setup help.

### Feedback and Community

Feedback is encouraged via [GitHub Issues](https://github.com/PowerShell/AIShell/issues). The team will continue evolving the project to bring new AI-powered workflow enhancements to the CLI experience.

**Thanks from Steven Bucher and Dongbo Wang, AI Shell Team**

This post appeared first on "Microsoft PowerShell Blog". [Read the entire article here](https://devblogs.microsoft.com/powershell/ai-shell-preview-2/)
