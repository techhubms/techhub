---
external_url: https://devblogs.microsoft.com/devops/tfvc-remove-existing-obsolete-policies-asap/
title: TFVC Remove Existing Obsolete Policies ASAP
author: Dan Hellem
primary_section: dotnet
feed_name: Microsoft DevOps Blog
date: 2026-02-18 16:35:23 +00:00
tags:
- .NET Framework
- '#devops'
- Automation
- Azure
- Azure & Cloud
- Azure DevOps
- Azure Repos
- C#
- Check in Policies
- DevOps
- Legacy Systems
- Microsoft.TeamFoundationServer.ExtendedClient
- News
- Policy Migration
- Project Administration
- Repository Management
- Source Control
- Team Explorer
- TFVC
- Version Control
- VS
- .NET
section_names:
- azure
- dotnet
- devops
---
Dan Hellem explains the deprecation of legacy TFVC check-in policies in Azure DevOps, detailing how to update or remove policies using Visual Studio or a C# app to maintain repository compliance.<!--excerpt_end-->

# TFVC Remove Existing Obsolete Policies ASAP

In April 2025, Microsoft announced the deprecation schedule for legacy Team Foundation Version Control (TFVC) check-in policies in Azure DevOps. This transition is required due to limitations in the original policy implementation and storage methods. The obsolete policies are now being phased out and need to be replaced with their updated equivalents to maintain compliance and avoid blocking repository changes.

## Transition Phases

- **Phase II (Current):**
  - Obsolete TFVC policies can still be replaced using Visual Studio Team Explorer.
  - Users attempting to check in code with outdated policies will receive compliance notifications.
  - Replace policies now to avoid repository issues.

- **Final Phase (Upcoming):**
  - Remaining legacy policies will block all check-ins.
  - Obsolete policies will become invisible and unmanageable through Team Explorer.
  - Manual removal is required if policies remain.

## How to Remove or Replace Policies

### Using Visual Studio

1. Open Visual Studio and connect to your Azure DevOps project.
2. Navigate to Team Explorer > Settings > Source Control.
3. Click the **Check-in Policy** tab.
4. Add updated policies and remove obsolete ones.

### Using a C# Console Application

If policy removal via Visual Studio is not possible, use the following approach:

1. Open Visual Studio and create a new C# Console App (.NET Framework).
2. In Solution Explorer, right click the project and select **Manage NuGet Packages**.
3. Search for and install `Microsoft.TeamFoundationServer.ExtendedClient`.
4. Update `Program.cs` in the Main method with the code below:

```csharp
var collectionUri = "https://contoso.visualstudio.com/";
var currentProjectName = "fabrikam";

using (TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(new Uri(collectionUri)))
{
    var versionControlServer = tpc.GetService<VersionControlServer>();
    TeamProject teamProject = versionControlServer.GetTeamProject(currentProjectName);
    teamProject.SetCheckinPolicies(null);
}
```

1. Update `collectionUri` and `currentProjectName` to match your environment.
2. Run the project (you will sign into Azure DevOps if prompted). The script will remove obsolete policies.

After cleanup, reopen Visual Studio to reapply any required policies.

## Additional Resources

- [Migration guide](https://learn.microsoft.com/en-us/azure/devops/repos/tfvc/tfvc-check-in-policy-migrate-guide): Detailed steps for migrating and managing custom policies.
- [Video walkthrough](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/02/tfvc-steps-to-remove-obsolete-checkin-policies.mp4): Visual instructions for removing obsolete policies.

## Key Recommendations

- Replace obsolete policies immediately to prevent repository access disruption.
- Project administrators are required to perform policy changes via code.
- Understand the risks associated with legacy systems and plan transitions to supported approaches.

For questions or more details, refer to the official Azure DevOps Blog post or consult the linked resources.

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/tfvc-remove-existing-obsolete-policies-asap/)
