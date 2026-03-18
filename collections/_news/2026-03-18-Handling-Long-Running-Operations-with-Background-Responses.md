---
date: 2026-03-18 16:37:00 +00:00
tags:
- .NET
- A2AAgent
- Agent Framework
- AgentRunOptions
- AgentSession
- AI
- Async Processing
- Azure OpenAI
- AzureOpenAIClient
- Background Responses
- C#
- ChatClientAgent
- Continuation Token
- DefaultAzureCredential
- Fault Tolerance
- Long Running Operations
- Microsoft Agent Framework
- Network Interruptions
- News
- OpenAI Provider
- Polling
- Python
- Reasoning Models
- Responses API
- Streaming
author: Sergey Menshykh, Eduard van Valkenburg
title: Handling Long-Running Operations with Background Responses
feed_name: Microsoft Semantic Kernel Blog
section_names:
- ai
- dotnet
primary_section: ai
external_url: https://devblogs.microsoft.com/agent-framework/handling-long-running-operations-with-background-responses/
---

Sergey Menshykh and Eduard van Valkenburg explain how Microsoft Agent Framework “background responses” help run long agent tasks without tying up a client connection, using continuation tokens for polling and stream resumption in both .NET and Python.<!--excerpt_end-->

# Handling Long-Running Operations with Background Responses

AI agents powered by reasoning models can take minutes to work through complex problems—deep research, multi-step analysis, lengthy content generation. In a traditional request-response pattern, that means your client sits idle waiting for a connection that may time out, or worse, fails silently and loses all progress.

**Background responses** in **Microsoft Agent Framework** let you offload these long-running operations so your application stays responsive and resilient, regardless of how long the agent takes to think.

With background responses, you start an agent task and get back a **continuation token** instead of blocking until completion. Your application can poll for results on its own schedule, resume interrupted streams from exactly where they left off, and handle network hiccups without restarting work from scratch. Background responses are available in both **.NET** and **Python**.

## How It Works

When you enable background responses and send a request to an agent, what happens depends on whether the underlying agent and model support background processing:

1. **Immediate completion**: if the agent does not support background processing, it completes the task inline and returns the final response directly (no continuation token).
2. **Background processing**: if the agent supports background processing, it starts working in the background and returns a continuation token that you use to check progress or resume streaming.

The continuation token captures the current state of the operation. When it’s `null` (.NET) or `None` (Python), the operation is complete—either the response is ready, the operation failed, or further input is required.

## When to Use Background Responses

Background responses are a good fit when your agent tasks involve:

- **Complex reasoning**: models like `o3` or `GPT-5.2` that take time to reason through multi-step problems
- **Long content generation**: producing detailed reports, extensive analysis, or multi-part documents
- **Unreliable network conditions**: mobile clients, edge deployments, or any environment where connections may drop
- **Async user experiences**: “fire and forget” patterns where users submit a task and come back later for results

## Non-Streaming: Poll for Completion

The simplest approach is to start a background run, then poll until the result is ready.

### .NET

```csharp
AIAgent agent = new AzureOpenAIClient(
    new Uri("https://<myresource>.openai.azure.com"),
    new DefaultAzureCredential())
    .GetResponsesClient("<deployment-name>")
    .AsAIAgent();

AgentRunOptions options = new()
{
    AllowBackgroundResponses = true // Enable background responses
};

AgentSession session = await agent.CreateSessionAsync();

// Start the run— may complete immediately or return a continuation token
AgentResponse response = await agent.RunAsync(
    "Write a detailed market analysis for the Q4 product launch.",
    session,
    options);

// Poll until complete
while (response.ContinuationToken is not null)
{
    await Task.Delay(TimeSpan.FromSeconds(2));

    options.ContinuationToken = response.ContinuationToken;
    response = await agent.RunAsync(session, options);
}

Console.WriteLine(response.Text);
```

### Python

```python
import asyncio
from agent_framework.openai import OpenAIResponsesClient

agent = OpenAIResponsesClient(model_id="o3").as_agent(
    name="researcher",
    instructions="You are a helpful research assistant.",
)

session = await agent.create_session()

# Start a background run
response = await agent.run(
    messages="Write a detailed market analysis for the Q4 product launch.",
    session=session,
    options={"background": True},
)

# Poll until the operation completes
while response.continuation_token is not None:
    await asyncio.sleep(2)
    response = await agent.run(
        session=session,
        options={"continuation_token": response.continuation_token},
    )

print(response.text)
```

**Key points:**

- If no continuation token is returned, the operation completed immediately—no polling needed.
- Pass the continuation token from each response into the next polling call.
- When the token is `null` / `None`, you have the final result.

## Streaming with Resumption

For a more responsive experience, you can stream results in real time while still benefiting from background processing. Each streamed update carries a continuation token, so if the connection drops, you can pick up right where you left off.

### .NET

```csharp
AgentRunOptions options = new()
{
    AllowBackgroundResponses = true // Enable background responses
};

AgentSession session = await agent.CreateSessionAsync();

AgentResponseUpdate? latestUpdate = null;

await foreach (var update in agent.RunStreamingAsync(
    "Write a detailed market analysis for the Q4 product launch.",
    session,
    options))
{
    Console.Write(update.Text);
    latestUpdate = update;

    // Simulate a network interruption
    break;
}

// Resume from exactly where we left off
options.ContinuationToken = latestUpdate?.ContinuationToken;
await foreach (var update in agent.RunStreamingAsync(session, options))
{
    Console.Write(update.Text);
}
```

### Python

```python
session = await agent.create_session()

last_token = None

# Start streaming with background enabled
async for update in agent.run(
    messages="Write a detailed market analysis for the Q4 product launch.",
    stream=True,
    session=session,
    options={"background": True},
):
    last_token = update.continuation_token
    if update.text:
        print(update.text, end="", flush=True)

    # Simulate a network interruption
    break

# Resume the stream using the last continuation token
if last_token is not None:
    async for update in agent.run(
        stream=True,
        session=session,
        options={"continuation_token": last_token},
    ):
        if update.text:
            print(update.text, end="", flush=True)
```

**Key points:**

- Each update includes a continuation token—save the most recent one.
- If the stream breaks, pass the saved token to resume from that exact point.
- The agent continues processing on the server even if the client disconnects.

## Built-In Fault Tolerance

One of the practical benefits of background responses is resilience to connectivity issues. Because the agent continues processing server-side regardless of what happens to the client connection, you get fault tolerance without building it yourself:

- **Network interruptions**: the client can reconnect and resume using the last continuation token.
- **Client restarts**: persist the continuation token to storage and pick up the operation from a new process.
- **Timeout protection**: long-running tasks won’t fail because a connection was held open too long.

For production applications, consider storing continuation tokens persistently (in a database or cache) so operations can survive not just network blips but full application restarts. This is especially valuable for enterprise scenarios where agent tasks may run for several minutes.

## Use Cases

### Document Generation and Review

An enterprise compliance team uses an agent to generate regulatory filings. These documents require the model to reason through complex guidelines and can take several minutes. With background responses, the application submits the request, shows the user a progress indicator, and polls for the result—no risk of the request timing out mid-generation.

### Research and Analysis

A financial services agent performs deep analysis across market data, SEC filings, and news. The reasoning model needs time to synthesize information from multiple sources. Background responses let the application kick off the analysis, free up the UI, and notify the user when results are ready.

### Batch Processing Pipelines

A data engineering team runs agents over large datasets to extract insights. Each item may require extended reasoning. Background responses allow the pipeline to submit tasks in parallel, poll for completions, and handle individual failures without losing progress on the rest.

## Best Practices

- **Use reasonable polling intervals**: start with 2-second intervals and consider exponential backoff for longer-running tasks.
- **Always check for null continuation tokens**: this is your signal that processing is complete.
- **Persist continuation tokens** for operations that may span user sessions or survive application restarts.

## Supported Agents

Background responses are fully supported when used with the Responses API for **OpenAI** and **Azure OpenAI** providers:

- OpenAI provider: https://learn.microsoft.com/en-us/agent-framework/agents/providers/openai
- Azure OpenAI provider: https://learn.microsoft.com/en-us/agent-framework/agents/providers/azure-openai

This is done via `ChatClientAgent` in .NET and `Agent` in Python.

`A2AAgent` has limited support for background responses, with improvements planned for an upcoming release.

The amount of time you can access response data is subject to the data retention policy of the underlying service.

## Learn More

- Background responses docs: https://learn.microsoft.com/en-us/agent-framework/agents/background-responses
- Microsoft Agent Framework on GitHub: https://github.com/microsoft/agent-framework
- Discussion boards: https://github.com/microsoft/agent-framework/discussions


[Read the entire article](https://devblogs.microsoft.com/agent-framework/handling-long-running-operations-with-background-responses/)

