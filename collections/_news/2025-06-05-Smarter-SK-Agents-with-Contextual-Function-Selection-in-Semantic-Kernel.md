---
external_url: https://devblogs.microsoft.com/semantic-kernel/smarter-sk-agents-with-contextual-function-selection/
title: Smarter SK Agents with Contextual Function Selection in Semantic Kernel
author: Sergey Menshykh
feed_name: Microsoft DevBlog
date: 2025-06-05 09:28:47 +00:00
tags:
- .NET
- Agents
- AI Agents
- Azure OpenAI
- C#
- ChatCompletionAgent
- Contextual Function Selection
- Customer Review Summarization
- Function Filtering
- Plugin Integration
- Retrieval Augmented Generation
- Semantic Kernel
- Token Efficiency
- Tools
section_names:
- ai
- coding
primary_section: ai
---
Sergey Menshykh discusses the new Contextual Function Selection capability in Semantic Kernel, illustrating its practical benefits for AI agent development with dynamic function filtering and performance optimization.<!--excerpt_end-->

# Smarter SK Agents with Contextual Function Selection

**Author:** Sergey Menshykh

In today’s fast-paced AI landscape, developers are constantly seeking ways to make AI interactions more efficient and relevant. The new **Contextual Function Selection** feature in the Semantic Kernel Agent Framework helps achieve this by dynamically selecting and advertising only the most relevant functions based on the current conversation context. This results in smarter, faster, and more effective AI agents.

## Why Contextual Function Selection Matters

When AI models have access to a large number of available functions, it can be challenging to select the most appropriate one, potentially leading to confusion and inefficiency. Contextual Function Selection addresses this challenge by leveraging **Retrieval-Augmented Generation (RAG)** to filter and present only the most relevant functions for each interaction. This approach improves response accuracy, reduces token consumption, and enhances overall system performance.

## Key Benefits of Contextual Function Selection

- **Dynamic Function Filtering:** Automatically select the top relevant functions according to the conversation context.
- **Enhanced AI Performance:** Narrow function choices to reduce confusion and improve accuracy.
- **Token Efficiency:** Advertise only necessary functions to minimize input token usage.

## Example Use Case: Summarizing Customer Reviews

Suppose you are building an AI agent to summarize customer reviews. With Contextual Function Selection, the agent chooses the most relevant functions—such as retrieving reviews, summarizing them, and performing sentiment analysis—dynamically based on the user's request.

### Implementation Sample (C#)

```csharp
// Create an embedding generator for function vectorization
var embeddingGenerator = new AzureOpenAIClient(new Uri("<endpoint>"), new ApiKeyCredential("<api-key>"))
    .GetEmbeddingClient("<deployment-name>")
    .AsIEmbeddingGenerator();

// Create a chat completion agent
ChatCompletionAgent agent = new()
{
    Name = "ReviewGuru",
    Instructions = "You are a friendly assistant that summarizes key points and sentiments from customer reviews. For each response, list available functions",
    Kernel = kernel,
    Arguments = new(new PromptExecutionSettings
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    })
};

// Register the contextual function provider
agentThread.AIContextProviders.Add(
    new ContextualFunctionProvider(
        vectorStore: new InMemoryVectorStore(new InMemoryVectorStoreOptions()
        {
            EmbeddingGenerator = embeddingGenerator
        }),
        vectorDimensions: 1536,
        functions: GetAvailableFunctions(),
        maxNumberOfFunctions: 3
    )
);

// Invoke the agent
ChatMessageContent message = await agent
    .InvokeAsync("Get and summarize customer review.", agentThread)
    .FirstAsync();
Console.WriteLine(message.Content);

private IReadOnlyList<AIFunction> GetAvailableFunctions()
{
    // Returns 16 functions across 6 categories:
    // customer reviews (1), sentiment analysis (2),
    // summaries (2), communication (3), date/time (2), and Azure services (6)
}
```

### Example Output

```
Customer Reviews:
-----------------
1. John D. - ★★★★★
   Comment: Great product and fast shipping! Date: 2023-10-01

2. Jane S. - ★★★★
   Comment: Good quality, but delivery was a bit slow. Date: 2023-09-28

3. Mike J. - ★★★
   Comment: Average. Works as expected. Date: 2023-09-25

Summary:
--------
The reviews indicate overall customer satisfaction, with highlights on product quality and shipping efficiency. While some customers experienced excellent service, others mentioned areas for improvement, particularly regarding delivery times.

Available functions:
--------------------
- Tools-GetCustomerReviews
- Tools-Summarize
- Tools-CollectSentiments
```

*Note:* From the 16 available functions, only 3 relevant ones were selected and advertised to the model, based on the conversation’s context.

## Why This Matters to Developers

For developers, the Contextual Function Selection feature streamlines the creation of intelligent, context-sensitive agents. Whether you are handling customer interactions, automating tasks, or analyzing data, this approach ensures your AI agents stay relevant and efficient.

## Learn More

- [Official Microsoft Learn documentation](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/agent-contextual-function-selection?pivots=programming-language-csharp)
- [Sample code on GitHub](https://github.com/microsoft/semantic-kernel/blob/main/dotnet/samples/Concepts/Agents/ChatCompletion_ContextualFunctionSelection.cs)

Stay up to date with innovations in AI agent development via [Semantic Kernel’s blog](https://devblogs.microsoft.com/semantic-kernel/).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/smarter-sk-agents-with-contextual-function-selection/)
