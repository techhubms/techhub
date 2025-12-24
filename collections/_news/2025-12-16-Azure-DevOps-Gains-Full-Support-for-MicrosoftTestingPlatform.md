---
layout: "post"
title: "Azure DevOps Gains Full Support for Microsoft.Testing.Platform"
description: "This news post highlights the seamless integration of Microsoft.Testing.Platform in Azure DevOps, making it easier for developers to run, retry, and publish .NET test results in CI/CD pipelines. It explains updates to Azure DevOps tasks, migration steps, retry handling, and improved publishing of results with clear technical configuration guidance. Developers working with .NET frameworks can now take advantage of enhanced testing capabilities and automation within Azure DevOps."
author: "Youssef Fahmy"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/microsoft-testing-platform-azure-retry/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-12-16 18:05:00 +00:00
permalink: "/news/2025-12-16-Azure-DevOps-Gains-Full-Support-for-MicrosoftTestingPlatform.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET", ".NET 10 SDK", "Azure", "Azure DevOps", "C#", "CI/CD Pipelines", "Coding", "DevOps", "DotNetCoreCLI", "F#", "Microsoft.Testing.Platform", "News", "PublishTestResults", "Retry Extension", "Test Automation", "Test Frameworks", "Test Results", "Testing", "TRX Files", "Unit Testing", "Visual Basic", "VSTest", "YAML Pipelines"]
tags_normalized: ["dotnet", "dotnet 10 sdk", "azure", "azure devops", "csharp", "cislashcd pipelines", "coding", "devops", "dotnetcorecli", "fsharp", "microsoftdottestingdotplatform", "news", "publishtestresults", "retry extension", "test automation", "test frameworks", "test results", "testing", "trx files", "unit testing", "visual basic", "vstest", "yaml pipelines"]
---

Youssef Fahmy presents an in-depth look at Azure DevOps' new full support for Microsoft.Testing.Platform, outlining how developers can now run, retry, and publish .NET test results effortlessly.<!--excerpt_end-->

# Azure DevOps Gains Full Support for Microsoft.Testing.Platform

Azure DevOps now offers comprehensive support for Microsoft.Testing.Platform, streamlining test execution and publishing in your CI/CD pipelines. This update removes previous workarounds and provides a smoother experience for developers working with .NET test frameworks.

## Key Improvements

- **Run Tests with DotNetCoreCLI**: Use the familiar DotNetCoreCLI task to execute tests without needing custom scripts or additional configuration.
- **Intelligent Retry Handling**: Azure DevOps can now publish multiple TRX files from retry attempts, correctly grouping results and setting accurate exit codes.

## Migration and Usage

If you're moving from VSTest or starting with Microsoft.Testing.Platform, follow these steps for a seamless experience:

1. **Use DotNetCoreCLI Task**: Start running tests using the updated DotNetCoreCLI@2 task. Sample YAML:

   ```yml
   - task: DotNetCoreCLI@2
     displayName: 'Run tests'
     inputs:
       command: 'test'
       projects: '**/*Tests.csproj'
       arguments: '--no-build --report-trx'
   ```

   Microsoft.Testing.Platform introduces new command-line flags, like `--report-trx` instead of `--logger trx`.

2. **Direct Script Usage**: For flexibility, run the `dotnet test` command directly using a command-line task:

   ```yml
   - task: CmdLine@2
     displayName: 'Run tests'
     inputs:
       script: 'dotnet test --no-build --report-trx'
   ```

## Retry Extension and Publishing Results

Microsoft.Testing.Platform works with the [Retry extension](https://www.nuget.org/packages/Microsoft.Testing.Extensions.Retry), automatically re-running failed tests and generating separate TRX files for each attempt. The new Azure DevOps behavior handles these files intelligently:

- **Grouped Results**: Results from retries are grouped, making UI review easier.
- **Accurate Exit Codes**: The `PublishTestResults` task only fails when all retry attempts fail, reflecting true suite status.

Enable retry-aware publishing by setting the following variable at the pipeline level:

```yml
variables:
  AllowPtrToDetectTestRunRetryFiles: true
```

Example pipeline for running tests and publishing results:

```yml
trigger:
- main

jobs:
- job: MyJob
  variables:
    AllowPtrToDetectTestRunRetryFiles: true
  steps:
  # Install SDK, restore, build steps...
  - task: CmdLine@2
    displayName: 'Run tests'
    inputs:
      script: 'dotnet test --no-build --report-trx --retry-failed-tests 3 --results-directory TestResults'
  - task: PublishTestResults@2
    displayName: 'Publish test results'
    inputs:
      testResultsFormat: 'VSTest'
      testResultsFiles: '**/*.trx'
      mergeTestResults: true
      failTaskOnFailedTests: true
      condition: succeededOrFailed()
```

## UI Scenarios

- **All Tests Pass**: Results appear as standard success.
- **Test Passes After Retry**: Failure attempts are grouped, and the task succeeds if a retry passes.
- **All Retries Fail**: Failures are grouped for detailed review.

## Important Notes

- Microsoft.Testing.Platform uses the TRX report format.
- Use the Retry extension for automatic failed test re-execution.
- Always configure `AllowPtrToDetectTestRunRetryFiles` per pipeline.

## Resources

- [Microsoft.Testing.Platform overview](https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro)
- [DotNetCoreCLI Azure DevOps task](https://learn.microsoft.com/azure/devops/pipelines/tasks/reference/dotnet-core-cli-v2)
- [PublishTestResults task](https://learn.microsoft.com/azure/devops/pipelines/tasks/reference/publish-test-results-v2)
- [GitHub Discussions for support](https://github.com/microsoft/testfx/discussions/new/choose)

Developers should update to .NET 10 SDK, add the Retry extension, set the required pipeline variables, and leverage these enhanced Azure DevOps features for more robust automated testing.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/microsoft-testing-platform-azure-retry/)
