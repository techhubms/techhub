---
external_url: https://devblogs.microsoft.com/dotnet/microsoft-testing-platform-azure-retry/
title: Azure DevOps Gains Full Support for Microsoft.Testing.Platform
author: Youssef Fahmy
feed_name: Microsoft .NET Blog
date: 2025-12-16 18:05:00 +00:00
tags:
- .NET
- .NET 10 SDK
- Azure DevOps
- C#
- CI/CD Pipelines
- DotNetCoreCLI
- F#
- Microsoft.Testing.Platform
- PublishTestResults
- Retry Extension
- Test Automation
- Test Frameworks
- Test Results
- Testing
- TRX Files
- Unit Testing
- Visual Basic
- VSTest
- YAML Pipelines
section_names:
- azure
- coding
- devops
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
