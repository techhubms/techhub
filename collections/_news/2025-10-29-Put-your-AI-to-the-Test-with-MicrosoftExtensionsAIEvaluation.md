---
external_url: https://devblogs.microsoft.com/blog/put-your-ai-to-the-test-with-microsoft-extensions-ai-evaluation
title: Put your AI to the Test with Microsoft.Extensions.AI.Evaluation
author: McKenna Barlow, Shyam Namboodiripad
feed_name: Microsoft Blog
date: 2025-10-29 17:00:39 +00:00
tags:
- .NET
- AI Evaluation
- Azure Blob Storage
- Azure DevOps
- Azure OpenAI
- CI/CD
- Content Safety
- Custom Evaluators
- Evaluations
- LLM
- Meai
- Microsoft.Extensions.AI.Evaluation
- MSTest
- Natural Language Processing
- NuGet
- NUnit
- Quality Metrics
- Reporting
- Responsible AI
- Test Automation
- VS
- xUnit
- AI
- Azure
- DevOps
- News
section_names:
- ai
- azure
- dotnet
- devops
primary_section: ai
---
McKenna Barlow and Shyam Namboodiripad detail how developers can leverage Microsoft.Extensions.AI.Evaluation to systematically assess and improve the quality, safety, and relevance of AI outputs in .NET applications.<!--excerpt_end-->

# Put your AI to the Test with Microsoft.Extensions.AI.Evaluation

*Authors: McKenna Barlow, Shyam Namboodiripad*

AI is reshaping software development, but it brings new challenges around evaluating model outputs. This article introduces the Microsoft.Extensions.AI.Evaluation libraries—a set of tools allowing .NET developers to systematically evaluate AI application quality, safety, and relevance with familiar workflows.

## Why Evaluate AI?

AI-generated outputs need testing just as traditional code does. Evals act as “unit tests for AI,” assessing outputs for criteria such as correctness, coherence, relevance, intent, and safety—including domain-specific configurations as needed. Integrating evals enables developers to:

- Benchmark different models and prompts
- Catch issues before users are affected
- Continuously improve AI feature quality

## Microsoft.Extensions.AI.Evaluation Libraries: Key Features

- **Seamless .NET Integration**: Plug directly into existing .NET projects and use with MSTest, xUnit, NUnit, Visual Studio Test Explorer, dotnet test, and CI/CD pipelines.
- **Flexible Evaluation**: Run evaluations offline during development or online in production, and upload results to telemetry dashboards for live monitoring.
- **Rich, Built-in Evaluators**:
  - **Content Safety**: Built atop the Azure AI Foundry Evaluation service for issues like protected material, hate, unfairness, violence, and code vulnerability.
  - **Quality Evaluators**: Measure relevance, coherence, completeness, agent task handling, intent resolution, and tool usage.
  - **NLP**: Implements mainstream NLP evaluation metrics (BLEU, GLEU, F1) for language tasks.
- **Reporting and Visualization**: Includes an interactive CLI reporting tool for HTML visualizations and integration options for Azure DevOps and custom dashboards.
- **Incremental, Fast Testing**: Response caching saves time and cost during repeated evals by avoiding redundant LLM calls.
- **Scalable Storage**: Supports Azure Blob Storage as well as local disk and custom providers for persisting evaluation results.
- **Extensibility**: Create your own evaluators, metrics, and storage/reporting logic to suit bespoke needs.

## Integration Scenarios

- **CI/CD Automation**: Integrate into Azure DevOps pipelines to treat AI evaluations as first-class testing steps.
- **Azure and OpenAI**: Use with Azure OpenAI deployments (with default support for GPT-4o or newer, and custom models via IChatClient).

## Example: Running Quality Evaluators

1. **Set Up Azure OpenAI**:
   - Deploy a model in Azure OpenAI
   - Set required environment variables (endpoints, deployment names)
2. **Create a Test Project in Visual Studio**:
   - Add relevant NuGet packages (`Azure.AI.OpenAI`, `Microsoft.Extensions.AI.Evaluation`, etc.)
   - Write and run unit tests using evaluators (e.g., `CoherenceEvaluator`, `GroundednessEvaluator`)
3. **Evaluate and Report**:
   - Run tests via Test Explorer for evaluation and output persisting
   - Use the `dotnet aieval` CLI tool to generate HTML reports and visualize results

```csharp
// (Sample code excerpt demonstrating evaluation setup and usage in a .NET MSTest project is given in the post)
```

## Advanced Usage and Customization

- **Define Custom Evaluators**: Extend the libraries with domain-specific metrics
- **Custom Reporting/Storage**: Use built-in integrations for Azure Blob Storage, or implement your own provider
- **CLI and Automation**: Generate reports with `dotnet aieval` for interactive analysis
- **Monitor Over Time**: Track historical performance and trends of your AI features

## Resources and Further Reading

- [Microsoft.Extensions.AI.Evaluation on Learn](https://learn.microsoft.com/en-us/dotnet/ai/conceptual/evaluation-libraries)
- [API Usage Examples](https://github.com/dotnet/ai-samples/tree/main/src/microsoft-extensions-ai-evaluation)
- [Azure AI Foundry Documentation](https://learn.microsoft.com/azure/ai-foundry/openai/)
- [Responsible AI practices](https://learn.microsoft.com/en-us/dotnet/ai/evaluation/responsible-ai)
- [Azure DevOps Plugin](https://marketplace.visualstudio.com/items?itemName=pbw.microsoft-extensions-ai-evaluation-report)

The Microsoft.Extensions.AI.Evaluation libraries provide a step-change in building reliable, transparent, and high-quality AI-enabled .NET apps. Explore the linked samples and documentation to bring advanced evals to your workflow.

This post appeared first on "Microsoft Blog". [Read the entire article here](https://devblogs.microsoft.com/blog/put-your-ai-to-the-test-with-microsoft-extensions-ai-evaluation)
