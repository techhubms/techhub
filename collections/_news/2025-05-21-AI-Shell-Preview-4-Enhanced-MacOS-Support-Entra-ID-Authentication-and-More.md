---
external_url: https://devblogs.microsoft.com/powershell/preview-4-ai-shell/
title: 'AI Shell Preview 4: Enhanced MacOS Support, Entra ID Authentication, and More'
author: Steven Bucher
feed_name: Microsoft PowerShell Blog
date: 2025-05-21 15:16:13 +00:00
tags:
- AI Integration
- AI Shell
- Authentication
- Azure OpenAI
- Command Line Tools
- Copilot in Azure
- Entra ID
- Invoke AIShell
- Macos
- OllamaAgent
- Phi Silica
- PowerShell
- Predictive IntelliSense
section_names:
- ai
- azure
- coding
---
Steven Bucher from the PowerShell Team introduces AI Shell Preview 4, diving into its new features: enhanced macOS integration, Entra ID authentication, improved shell commands, a new offline AI agent, and more.<!--excerpt_end-->

# AI Shell Preview 4 Is Here: Better macOS Support, Entra ID Authentication, and More

**Author**: Steven Bucher ([PowerShell Team](https://devblogs.microsoft.com/powershell))

The AI Shell Team has unveiled Preview 4 of AI Shell, bringing several key improvements and new features based on user feedback. This release continues to enhance the integration of AI-powered assistance within PowerShell by focusing on access to Azure OpenAI deployments, expanding the `Invoke-AIShell` command, and broadening compatibility with macOS.

## Improved macOS Support

- **Enhanced Side Car Experience with iTerm2**: The macOS side car experience is now more stable, enabling reliable support for the `/code post` command and achieving parity with the Windows experience.
- **Requirements**: Users must be running PowerShell 7 within iTerm2. [See installation instructions for PowerShell 7 on macOS.](https://learn.microsoft.com/powershell/scripting/install/installing-powershell-on-macos)
- ![AI Shell Preview 4 on mac](https://devblogs.microsoft.com/powershell/wp-content/uploads/sites/30/2025/05/macdemo.gif)

## Microsoft Entra ID Authentication for Azure OpenAI

- **Secure Authentication**: Support for Entra ID authentication enables secure access to Azure OpenAI resources without the need to store sensitive keys in configuration files.
- **Configuration Example**:

  ```jsonc
  {
    "GPTs": [
      {
        "Name": "ps-az-entraId",
        "Description": "A GPT instance with expertise in PowerShell scripting using Entra ID authentication.",
        "Endpoint": "<Your Endpoint>",
        "Deployment": "<Your Deployment Name>",
        "ModelName": "<Your Model Name>",
        "AuthType": "EntraID",
        "SystemPrompt": "You are a helpful and friendly assistant with expertise in PowerShell scripting and command line."
      }
    ],
    "Active": "ps-az-entraId"
  }
  ```

- **Credential Hierarchy**: AI Shell attempts authentication using these credentials, in order:
  - `EnvironmentCredential`
  - `WorkloadIdentityCredential`
  - `ManagedIdentityCredential`
  - `SharedTokenCacheCredential`
  - `VisualStudioCredential`
  - `AzureCliCredential`
  - `AzurePowerShellCredential`
  - `AzureDeveloperCliCredential`
  - `InteractiveBrowserCredential`
- [Learn about DefaultAzureCredential.](https://learn.microsoft.com/dotnet/api/azure.identity.defaultazurecredential)

## Enhanced Invoke-AIShell Command

- **Additional Parameters**:
  - `-PostCode`: Post code from the side pane directly into the PowerShell session, reducing the need to manually run the `/code post` command.
  - `-CopyCode`: Copy code from the side pane without running `/code copy`.
  - `-Exit`: Exit the side pane easily.
- **Workflow Improvements**: These features help users inject AI-generated commands seamlessly and maintain an uncluttered terminal experience, supporting more efficient shell workflows.
- **Integration with PSReadLine**: Pairing with Predictive IntelliSense further streamlines command entry and code generation.
- ![Invoke-AIShell Demo](https://devblogs.microsoft.com/powershell/wp-content/uploads/sites/30/2025/05/InvokeAIShellDemo.gif)

## Introducing Phi Silica Experimental Agent

- **Offline AI Experience**: Phi Silica is an experimental local agent leveraging the built-in Phi Silica model included with the latest Copilot+ PCs. This allows offline use of AI Shell.
- **Availability**: Not included by default; users must clone the repository and follow [local build instructions](https://github.com/PowerShell/AIShell?tab=readme-ov-file#locally-building-ai-shell).
- **Current Status**: Proof of concept—recommended only for testing, with possible breaking changes in future updates.
- ![Phi Silica Agent Demo](https://devblogs.microsoft.com/powershell/wp-content/uploads/sites/30/2025/05/PhiSilicaAgentDemo.gif)

## Additional Improvements

- Updated support for the latest OpenAI models ([#368](https://github.com/PowerShell/AIShell/pull/368))
- `/clear` command as alias for `/cls` ([#370](https://github.com/PowerShell/AIShell/pull/370))
- Installation script updated for macOS ([#374](https://github.com/PowerShell/AIShell/pull/374))
- Enhanced model management and integration with system prompts in OllamaAgent ([#351](https://github.com/PowerShell/AIShell/pull/351)), contributed by [@cnupy](https://github.com/cnupy)
- [Changelog available here](https://github.com/PowerShell/AIShell/releases/tag/v1.0.0-preview.4)

## Installation Instructions

Install the latest AI Shell with:

```powershell
Invoke-Expression "& { $(Invoke-RestMethod 'https://aka.ms/install-aishell.ps1') }"
```

Feedback is welcomed via the [GitHub repository](https://github.com/PowerShell/AIShell/issues).

**Thank you,**

The AI Shell Team  
Steven Bucher & Dongbo Wang

This post appeared first on "Microsoft PowerShell Blog". [Read the entire article here](https://devblogs.microsoft.com/powershell/preview-4-ai-shell/)
