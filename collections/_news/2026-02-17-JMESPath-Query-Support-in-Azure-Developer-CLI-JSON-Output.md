---
external_url: https://devblogs.microsoft.com/azure-sdk/azd-jmespath-query-support/
title: JMESPath Query Support in Azure Developer CLI JSON Output
author: Scott Addie
primary_section: dotnet
feed_name: Microsoft Azure SDK Blog
date: 2026-02-17 18:19:18 +00:00
tags:
- Automation
- Azd
- Azure
- Azure Developer CLI
- Azure Development
- Azure SDK
- Command Line Tools
- Config Management
- Developer Productivity
- DevOps
- Error Handling
- JMESPath
- JSON Output
- News
- Script Automation
- Scripting
- Terminal Tools
- .NET
section_names:
- azure
- dotnet
- devops
---
Scott Addie explains how JMESPath queries are now supported in the Azure Developer CLI’s JSON output, showing developers how to extract and filter data efficiently within their terminal workflows.<!--excerpt_end-->

# JMESPath Query Support in Azure Developer CLI JSON Output

**Author:** Scott Addie

Azure Developer CLI (azd) now includes support for [JMESPath](https://jmespath.org/) queries directly on JSON output, beginning with version 1.23.4. This enhancement lets you filter, select, and transform `azd` command JSON results—including error messages—right from your terminal, streamlining scripting and automation tasks.

## What’s New?

- Pass `--query` with `-o json` to any compatible `azd` command.
- Extract exactly the parts of the JSON output you need, removing noise and manual parsing.
- Supports use cases ranging from configuration inspection to automated error reporting.

## Examples

1. **Viewing a configuration section:**

   ```sh
   azd config show -o json --query "template.sources"
   ```

   **Result:**

   ```json
   {
     "awesome-azd": {
       "key": "awesome-azd",
       "location": "https://aka.ms/awesome-azd/templates.json",
       "name": "Awesome AZD",
       "type": "awesome-azd"
     }
   }
   ```

2. **Extracting the default environment name in a script:**

   ```sh
   ENV_NAME=$(azd env list -o json --query "[?IsDefault].Name | [0]")
   echo "Deploying to $ENV_NAME"
   ```

   **Result:**

   ```
   production
   ```

3. **Extracting error messages (azd 1.23.5+):**

   ```sh
   azd auth token -o json --query "data.message"
   ```

   **When not authenticated, output:**

   ```
   "\nERROR: not logged in, run `azd auth login` to login\n"
   ```

For additional JMESPath syntax and capabilities, see the [JMESPath documentation](https://jmespath.org/tutorial.html).

## Why It Matters

- Eliminates the need for extra parsing utilities in automation and scripts.
- Makes extracting information from complex or verbose JSON responses straightforward.
- Increases productivity for Azure developers working with scripts, pipelines, or automation tasks.

## Feedback & Resources

- [File an issue](https://github.com/Azure/azure-dev/issues) or [start a discussion](https://github.com/Azure/azure-dev/discussions) for questions and feature requests.
- [Sign up for user research](https://aka.ms/azd-user-research-signup) if you want to help shape azd's future.
- Feature introduced in [PR #6664](https://github.com/Azure/azure-dev/pull/6664) and expanded in [PR #6735](https://github.com/Azure/azure-dev/pull/6735).

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azd-jmespath-query-support/)
