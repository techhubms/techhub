---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/automated-test-framework-missing-tests-in-test-explorer/ba-p/4490186
title: Fixing Disappearing Logic Apps Standard Test Framework Tests in VS Code
author: WSilveira
primary_section: coding
feed_name: Microsoft Tech Community
date: 2026-01-28 19:39:04 +00:00
tags:
- Automated Test Framework
- Azure
- C# DevKit
- Coding
- Community
- Csproj
- Integration Services
- Logic Apps Standard
- Microsoft Azure
- MSTest
- Package Management
- Test Explorer
- VS Code
section_names:
- azure
- coding
---
WSilveira outlines the solution for Logic Apps Standard Automated Test Framework tests disappearing from the VS Code Test Explorer. The post provides step-by-step guidance for updating MSTest package references to restore test discovery.<!--excerpt_end-->

# Fixing Disappearing Logic Apps Standard Test Framework Tests in VS Code

Have you noticed that your tests created using the Logic Apps Standard Automated Test Framework have suddenly vanished from the Test Explorer in Visual Studio Code? This issue is most likely due to a mismatch between the MSTest versions used in your project and the requirements introduced by the latest C# DevKit extension update.

## What Happened?

A recent update to the C# DevKit extension increased the minimum required version for the MSTest library to 3.7.*. However, the Logic Apps Standard extension scaffolds new projects with MSTest version 3.0.2, which no longer meets the updated requirement. As a result, your tests stop appearing in the Test Explorer.

## How to Fix It

You can resolve this problem by updating the MSTest package versions in your project's `.csproj` file. Below are the steps to do this:

1. **Open the `.csproj` file** that the Logic Apps Standard extension created for your tests (e.g., `LogicApps.csproj`).
2. **Identify your MSTest package references** inside the `<ItemGroup>`. They might look like this:

   ```xml
   <ItemGroup>
     <PackageReference Include="MSTest" Version="3.2.0" />
     <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
     <PackageReference Include="MSTest.TestAdapter" Version="3.2.0" />
     <PackageReference Include="MSTest.TestFramework" Version="3.2.0" />
     <PackageReference Include="Microsoft.Azure.Workflows.WebJobs.Tests.Extension" Version="1.0.0" />
     <PackageReference Include="coverlet.collector" Version="3.1.2" />
   </ItemGroup>
   ```

3. **Replace the package references** with updated versions as shown below:

   ```xml
   <ItemGroup>
     <PackageReference Include="Microsoft.Azure.Workflows.WebJobs.Tests.Extension" Version="1.*" />
     <PackageReference Include="MSTest" Version="4.0.2" />
     <PackageReference Include="coverlet.collector" Version="3.2.0" />
   </ItemGroup>
   ```

   > _Note_: Some of the package references are now handled as transitive dependencies and do not need to be declared explicitly.

4. **Restart your VS Code window** and rebuild the project. The Test Explorer should now rediscover and list your tests as expected.

## Upcoming Extension Updates

The Logic Apps Standard team is updating their extension to ensure new projects are generated with the correct MSTest versions. In the meantime, manual updates are necessary for existing projects.

---

For more integration service updates, follow the [Azure Integration Services Blog](https://techcommunity.microsoft.com/category/azure/blog/integrationsonazureblog).

*Updated Jan 28, 2026 by WSilveira*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/automated-test-framework-missing-tests-in-test-explorer/ba-p/4490186)
