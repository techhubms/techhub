---
external_url: https://devblogs.microsoft.com/powershell/enhanced-authoring-with-dsc-v3/
title: Authoring Enhancements in Microsoft Desired State Configuration v3.0.0
author: Jason Helmick
feed_name: Microsoft PowerShell Blog
date: 2025-03-12 18:00:43 +00:00
tags:
- Authoring
- Azure Resource Manager
- Bicep
- Configuration as Code
- Desired State Configuration
- DSC V3.0.0
- IaC
- PowerShell
- PowerShell DSC
- Schema Validation
- Shell Completion
- VS Code
- Azure
- Coding
- DevOps
- News
section_names:
- azure
- coding
- devops
primary_section: coding
---
Authored by Jason Helmick, this post introduces key authoring enhancements in Microsoft Desired State Configuration v3.0.0, designed to streamline the configuration and deployment processes for PowerShell and Azure environments.<!--excerpt_end-->

# Authoring Enhancements in Microsoft Desired State Configuration v3.0.0

*This is the third post in a multi-part series about the new release of DSC.*

Microsoft Desired State Configuration (DSC) v3.0.0 delivers several notable features to enhance the configuration authoring experience:

- **Shell completion**
- **Schema-based validation**
- **Support for modern DSLs like Azure Bicep**

## Terminology

- **DSC:** Refers to Desired State Configuration v3.0.0.
- **PSDSC:** Refers to PowerShell Desired State Configuration v1.1 and v2.

## DSC Command Completer

The DSC command completer returns a shell script that, when executed, registers shell completions for DSC. This helps users benefit from command auto-completion and improved workflows.

DSC can generate completion scripts for the following shells:

- [Bash](https://www.gnu.org/software/bash/)
- [Elvish](https://elv.sh/)
- [Fish](https://fishshell.com/)
- [PowerShell](https://learn.microsoft.com/powershell/scripting/overview)
- [Z Shell](https://support.apple.com/102360)

Learn more in the [`dsc completer` command reference documentation](https://learn.microsoft.com/powershell/dsc/reference/cli/completer/command?view=dsc-3.0&preserveView=true).

## Enhanced Authoring with Schemas

When authoring configuration documents and resource manifests with DSC, JSON schemas are used to validate these files. Enhanced schemas are provided for use in Visual Studio Code. These schemas offer:

- Improved contextual help (especially for hovering/selecting properties)
- Contextual help for enumerated values
- Improved error messages for invalid values
- Default value snippets to autocomplete values

This improves productivity, reduces errors, and provides richer context for users within their development environment.

To learn more, see [Authoring with Enhanced Schemas](https://learn.microsoft.com/powershell/dsc/concepts/enhanced-authoring?view=dsc-3.0&preserveView=true).

## Authoring with Bicep (Coming Soon)

[Bicep](https://learn.microsoft.com/azure/azure-resource-manager/bicep/overview?tabs=bicep) is Microsoft's domain-specific language (DSL) for ARM (Azure Resource Manager) deployments. Bicep introduces an intuitive syntax, strong validation, modularity, and seamless integration with Azure-native tooling, enhancing the authoring of DSC configurations.

### Key Features of Using Bicep with DSC

- Clean, maintainable syntax relative to JSON or YAML
- Local invocation or integration with Azure deployments
- Combines Infrastructure as Code (IaC) with Configuration as Code (CaC)

Example Bicep configuration invoking the classic `WindowsFeature` PowerShell resource to install IIS on Windows Server:

```bicep
targetScope = 'dsc'
resource powershellAdapter 'Microsoft.Windows/WindowsPowerShell@2025-01-07' = {
  name: 'Web server install'
  properties: {
    resources: [
      {
        name: 'Run WindowsFeature'
        type: 'PSDesiredStateConfiguration/WindowsFeature'
        properties: {
          Name: 'Web-server'
          Ensure: 'Present'
        }
      }
    ]
  }
}
```

> **Note:** By default, Bicep files are scoped to a Bicep resource. To leverage DSC authoring with Bicep, set the scope to `dsc`. Bicep support is currently under development, and further announcements will follow.

## For More Information

- Review [DSCv3 documentation](https://learn.microsoft.com/powershell/dsc/overview?view=dsc-3.0&preserveView=true) for additional details.
- Share feedback or issues on the [PowerShell DSC GitHub repository](https://github.com/PowerShell/DSC).

---

*Authored by Jason Helmick, Sr. Product Manager, PowerShell.*

This post appeared first on "Microsoft PowerShell Blog". [Read the entire article here](https://devblogs.microsoft.com/powershell/enhanced-authoring-with-dsc-v3/)
