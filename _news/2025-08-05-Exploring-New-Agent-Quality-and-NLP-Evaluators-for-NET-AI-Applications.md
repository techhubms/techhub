---
layout: "post"
title: "Exploring New Agent Quality and NLP Evaluators for .NET AI Applications"
description: "This post introduces enhancements to the Microsoft.Extensions.AI.Evaluation libraries, including new agent quality and NLP evaluators for .NET AI applications. It covers evaluator functionality, setup instructions, code samples, and guidance on generating reports for robust AI system evaluation."
author: "Shyam Namboodiripad"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/exploring-agent-quality-and-nlp-evaluators/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-08-05 17:05:00 +00:00
permalink: "/2025-08-05-Exploring-New-Agent-Quality-and-NLP-Evaluators-for-NET-AI-Applications.html"
categories: ["AI", "Azure", "Coding"]
tags: [".NET", "Agent Quality", "AI", "AI Evaluation", "Azure", "Azure OpenAI", "BLEU", "C#", "Coding", "Evaluations", "F1 Evaluator", "GLEU", "MEAI", "MEAI.Evaluation", "Microsoft.Extensions.AI", "Microsoft.Extensions.AI.Evaluation", "News", "NLP Metrics", "Testing"]
tags_normalized: ["net", "agent quality", "ai", "ai evaluation", "azure", "azure openai", "bleu", "c", "coding", "evaluations", "f1 evaluator", "gleu", "meai", "meai dot evaluation", "microsoft dot extensions dot ai", "microsoft dot extensions dot ai dot evaluation", "news", "nlp metrics", "testing"]
---

In this post, Shyam Namboodiripad details the latest agent quality and NLP evaluators in the Microsoft.Extensions.AI.Evaluation libraries, offering a comprehensive toolkit for .NET AI application assessment.<!--excerpt_end-->

## Overview

When building AI applications, comprehensive evaluation plays a vital role in ensuring systems respond accurately, reliably, and in the appropriate context. The Microsoft.Extensions.AI.Evaluation library has been enhanced with new evaluators for both AI agent quality assessment and natural language processing (NLP) metrics, empowering .NET developers to more deeply analyze and validate their AI-powered applications. This post covers the new capabilities and guides you through their usage.

---

## Agent Quality Evaluators

The `Microsoft.Extensions.AI.Evaluation.Quality` NuGet package introduces three evaluators for assessing AI agents in conversational scenarios involving tool use:

- **IntentResolutionEvaluator**: Evaluates the AI agent’s ability to understand and address user intent.
- **TaskAdherenceEvaluator**: Assesses if the agent maintains focus on the assigned conversational task.
- **ToolCallAccuracyEvaluator**: Measures the correctness and suitability of tool calls invoked by the agent.

These evaluators help developers systematically judge if their conversational AI solutions perform to expectations, especially where interaction with tools/APIs is required.

---

## NLP (Natural Language Processing) Evaluators

A new package, `Microsoft.Extensions.AI.Evaluation.NLP`, supplies evaluators that implement standard NLP algorithms for text similarity:

- **BLEUEvaluator**: Implements the BLEU metric to measure alignment between generated and reference texts.
- **GLEUEvaluator**: Implements GLEU, Google's sentence-level BLEU variant.
- **F1Evaluator**: Calculates F1-scores for evaluating text similarity and information retrieval.

**Note:** NLP evaluators work without an AI model; they depend on basic NLP techniques such as tokenization and n-gram analysis, letting you compare output text to reference text using established metrics.

These NLP evaluators complement the existing toolkit for evaluation quality and safety, providing developers flexibility suited to various assessment needs.

---

## Integrating with Azure OpenAI

Agent quality evaluators require connection to a large language model (LLM). The recommended approach is to use Azure OpenAI, supporting powerful models like GPT-4o and GPT-4.1, both tuned and tested against the included evaluation prompts. You can utilize other providers via the `IChatClient` abstraction, though results may vary.

### Setting Required Environment Variables

You need to set two variables:

```sh
SET EVAL_SAMPLE_AZURE_OPENAI_ENDPOINT=https://<your azure openai resource>.openai.azure.com/
SET EVAL_SAMPLE_AZURE_OPENAI_MODEL=<your model deployment name (e.g., gpt-4o)>
```

Authentication uses `DefaultAzureCredential`, allowing integration with developer tooling such as Visual Studio or Azure CLI.

---

## Setting Up a Test Project

You can use Visual Studio, Visual Studio Code (with C# Dev Kit), or the .NET CLI for your project:

### Visual Studio

1. Open Visual Studio
2. File > New > Project…
3. Choose **MSTest Test Project**
4. Name your project and create it

### VS Code

1. Open VS Code
2. Command Palette: **.NET: New Project…**
3. Pick **MSTest Test Project**
4. Name and create

### CLI

```sh
dotnet new mstest -n EvaluationTests
cd EvaluationTests
```

### Add Dependencies

```sh
dotnet add package Azure.AI.OpenAI
dotnet add package Azure.Identity

# Core evaluation libraries

dotnet add package Microsoft.Extensions.AI.Evaluation

# Quality and NLP evaluators

dotnet add package Microsoft.Extensions.AI.Evaluation.Quality

# (NLP is prerelease at the time of writing)

dotnet add package Microsoft.Extensions.AI.Evaluation.NLP --prerelease

# Reporting and OpenAI interaction

dotnet add package Microsoft.Extensions.AI.Evaluation.Reporting
dotnet add package Microsoft.Extensions.AI.OpenAI --prerelease
```

---

## Example: Using the Evaluators in Practice

Here is a sample structure for your test class:

```csharp
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.AI.Evaluation;
using Microsoft.Extensions.AI.Evaluation.NLP;
using Microsoft.Extensions.AI.Evaluation.Quality;
using Microsoft.Extensions.AI.Evaluation.Reporting;
using Microsoft.Extensions.AI.Evaluation.Reporting.Storage;

namespace EvaluationTests;

[TestClass]
public class Test1
{
    // Configuration for agent quality and NLP evaluation
    private static readonly ReportingConfiguration s_agentQualityConfig = CreateAgentQualityReportingConfiguration();
    private static readonly ReportingConfiguration s_nlpConfig = CreateNLPReportingConfiguration();

    [TestMethod]
    public async Task EvaluateAgentQuality()
    {
        // Run agent quality evaluators on a simulated conversation
        await using ScenarioRun scenarioRun = await s_agentQualityConfig.CreateScenarioRunAsync("Agent Quality");
        // Simulate customer service conversation
        (List<ChatMessage> messages, ChatResponse response, List<AITool> toolDefinitions) = await GetCustomerServiceConversationAsync(chatClient: scenarioRun.ChatConfiguration!.ChatClient);
        var additionalContext = new List<EvaluationContext>
        {
            new ToolCallAccuracyEvaluatorContext(toolDefinitions),
            new TaskAdherenceEvaluatorContext(toolDefinitions),
            new IntentResolutionEvaluatorContext(toolDefinitions)
        };
        EvaluationResult result = await scenarioRun.EvaluateAsync(messages, response, additionalContext);
        NumericMetric intentResolution = result.Get<NumericMetric>(IntentResolutionEvaluator.IntentResolutionMetricName);
        Assert.IsFalse(intentResolution.Interpretation!.Failed);
    }

    [TestMethod]
    public async Task EvaluateNLPMetrics()
    {
        // Evaluate text similarity metrics
        await using ScenarioRun scenarioRun = await s_nlpConfig.CreateScenarioRunAsync("NLP");
        const string Response = "Paris is the capital of France. It's famous for the Eiffel Tower, Louvre Museum, and rich cultural heritage";
        List<string> referenceResponses = new List<string>
        {
            "Paris is the capital of France. It is renowned for the Eiffel Tower, Louvre Museum, and cultural traditions.",
            "Paris, the capital of France, is famous for its landmarks like the Eiffel Tower and vibrant culture.",
            "The capital of France is Paris, known for its history, art, and iconic landmarks like the Eiffel Tower."
        };
        var additionalContext = new List<EvaluationContext>
        {
            new BLEUEvaluatorContext(referenceResponses),
            new GLEUEvaluatorContext(referenceResponses),
            new F1EvaluatorContext(groundTruth: referenceResponses.First())
        };
        EvaluationResult result = await scenarioRun.EvaluateAsync(Response, additionalContext);
        NumericMetric f1 = result.Get<NumericMetric>(F1Evaluator.F1MetricName);
        Assert.IsFalse(f1.Interpretation!.Failed);
    }

    // (CreateAgentQualityReportingConfiguration, CreateNLPReportingConfiguration, and GetCustomerServiceConversationAsync implementations omitted for brevity; see main post for full code.)
}
```

---

## Running the Tests & Generating Reports

You can run these tests using Visual Studio/Test Explorer or via the CLI (`dotnet test`). After execution, generate an HTML report with:

```sh
dotnet tool install Microsoft.Extensions.AI.Evaluation.Console --create-manifest-if-needed
dotnet aieval report -p <path to 'eval-results' folder under build output directory> -o ./report.html --open
```

The report will open automatically, letting you analyze detailed metric results for each evaluation scenario, including agent intent resolution and NLP similarity scores.

---

## Additional Resources & Feedback

- [API Usage Examples (GitHub)](https://github.com/dotnet/ai-samples/tree/main/src/microsoft-extensions-ai-evaluation)
- [Microsoft.Extensions.AI.Evaluation Documentation](https://learn.microsoft.com/dotnet/ai/conceptual/evaluation-libraries)
- [Report issues or suggestions on GitHub](https://github.com/dotnet/extensions/issues?q=is%3Aissue%20state%3Aopen%20label%3Aarea-ai-eval)

You’re encouraged to try these evaluators in your own .NET AI projects and to provide feedback for improving these tools for the AI developer community.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/exploring-agent-quality-and-nlp-evaluators/)
