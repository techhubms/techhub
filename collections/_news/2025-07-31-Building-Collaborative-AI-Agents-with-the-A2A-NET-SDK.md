---
external_url: https://devblogs.microsoft.com/foundry/building-ai-agents-a2a-dotnet-sdk/
title: Building Collaborative AI Agents with the A2A .NET SDK
author: Sergey Menshykh, Adam Sitnik, Brandon H
feed_name: Microsoft DevBlog
date: 2025-07-31 10:08:49 +00:00
tags:
- .NET
- A2A
- A2A .NET SDK
- Agent Capability Discovery
- Agent Collaboration
- Agent2Agent Protocol
- Agents
- AI Agents
- AI Applications
- ASP.NET Core
- Autonomous Agents
- Azure AI Foundry
- Message Based Communication
- Protocol
- Real Time Streaming
- Semantic Kernel
- Task Based Communication
section_names:
- ai
- azure
- coding
---
Written by Sergey Menshykh, Adam Sitnik, and Brandon H, this article introduces the A2A .NET SDK, demonstrating how developers can build collaborative AI agents leveraging the Agent2Agent protocol within the Azure AI Foundry environment.<!--excerpt_end-->

# Building Collaborative AI Agents with the A2A .NET SDK

*Authors: Sergey Menshykh, Adam Sitnik, Brandon H*

The evolution of AI has led to the proliferation of autonomous agents handling various specialized tasks, from customer service bots to workflow orchestration. However, enabling these agents to communicate and collaborate effectively is a challenge tackled by the Agent2Agent (A2A) protocol and its new .NET SDK implementation.

## Introduction: Agent Collaboration Challenges

AI agents are becoming increasingly sophisticated, yet many operate as isolated systems. There is a growing need for a standardized protocol that allows independent agents to discover, communicate, and collaborate, regardless of their underlying framework or vendor. The A2A protocol solves this by providing a common language for agent interoperability.

## What Is the Agent2Agent (A2A) Protocol?

The [Agent2Agent (A2A) Protocol](https://a2a-protocol.org) is an open standard enabling AI agents to:

- Discover each other’s capabilities
- Negotiate communication modalities (text, forms, media)
- Collaborate securely on long-running tasks
- Operate without exposing internal state, memory, or tools

A2A bridges vendor and technology divides, making agent ecosystems extensible and collaborative.

## Announcing the A2A .NET SDK

The newly released [A2A .NET SDK](https://github.com/a2aproject/a2a-dotnet) lets .NET developers build A2A servers and clients, allowing agents written in .NET (or any technology that supports the protocol) to participate in the agent ecosystem. The SDK strengthens the AI ecosystem, particularly in conjunction with [Azure AI Foundry](https://azure.microsoft.com/en-us/products/ai-foundry/) and Microsoft’s [Semantic Kernel](https://learn.microsoft.com/en-us/semantic-kernel/), both of which are adding and evolving their A2A capabilities.

**Note:** The SDK is in preview, so APIs may change as feedback is incorporated.

## Key Features of the A2A .NET SDK

### 🔍 Agent Capability Discovery

- Retrieve agent capabilities via standardized *Agent Cards* using the `A2ACardResolver` class
- Agent Cards describe host URLs, supported modalities, streaming support, notifications, and more

**Example:**

```csharp
A2ACardResolver cardResolver = new A2ACardResolver(new Uri("http://localhost:5100/"));
AgentCard agentCard = await cardResolver.GetAgentCardAsync();
```

### 💬 Flexible Communication Patterns

- **Message-based:** Synchronous messaging for immediate responses
- **Task-based:** Asynchronous tasks for longer-running operations

**Message Example:**

```csharp
A2AClient client = new A2AClient(new Uri(agentCard.Url));
Message response = (Message)await client.SendMessageAsync(new MessageSendParams {
  Message = new Message {
    Role = MessageRole.User,
    Parts = [new TextPart { Text = "What is the current weather in Settle" }]
  }
});
Console.WriteLine($"Received: {((TextPart)response.Parts[0]).Text}");
```

**Task Example:**

```csharp
AgentTask agentTask = (AgentTask)await client.SendMessageAsync(new MessageSendParams {
  Message = new Message {
    Role = MessageRole.User,
    Parts = [new TextPart { Text = "Generate an image of a sunset over the mountains" }]
  }
});
// Poll for task completion and handle result
```

For more, see [Message or a Task](https://github.com/a2aproject/A2A/blob/main/docs/topics/life-of-a-task.md#agent-message-or-a-task).

### 🌊 Real-time Streaming Support

- Supports Server-Sent Events (SSE) for real-time update scenarios

**Example:**

```csharp
await foreach (SseItem<A2AEvent> sseItem in client.SendMessageStreamAsync(new MessageSendParams { Message = userMessage })) {
  Message agentResponse = (Message)sseItem.Data;
  Console.WriteLine($"Received: {((TextPart)agentResponse.Parts[0]).Text}");
}
```

### 🏗️ ASP.NET Core Integration

Enables rapid development of A2A agents using ASP.NET Core extensions.

**Example:**

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();
TaskManager taskManager = new TaskManager();
agent.Attach(taskManager);
app.MapA2A(taskManager, "/agent");
app.Run();
```

## Tutorial: Building a Simple Echo Agent

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio/VS Code (or other .NET-compatible editor)
- Basic C# and ASP.NET Core knowledge

Verify .NET SDK installation:

```bash
dotnet --version
```

### Set Up the Agent Project

Initialize a new ASP.NET Core web app and add required A2A packages:

```bash
dotnet new webapp -n A2AAgent -f net9.0
cd A2AAgent
dotnet add package A2A --prerelease

# For ASP.NET Core integration

dotnet add package A2A.AspNetCore --prerelease
```

### Implement the Echo Agent

Create `EchoAgent.cs`:

```csharp
using A2A;
namespace A2AAgent;

public class EchoAgent {
  public void Attach(ITaskManager taskManager) {
    taskManager.OnMessageReceived = ProcessMessageAsync;
    taskManager.OnAgentCardQuery = GetAgentCardAsync;
  }

  private async Task<Message> ProcessMessageAsync(MessageSendParams messageSendParams, CancellationToken ct) {
    string request = messageSendParams.Message.Parts.OfType<TextPart>().First().Text;
    return new Message() {
      Role = MessageRole.Agent,
      MessageId = Guid.NewGuid().ToString(),
      ContextId = messageSendParams.Message.ContextId,
      Parts = [new TextPart() { Text = $"Echo: {request}" }]
    };
  }

  private async Task<AgentCard> GetAgentCardAsync(string agentUrl, CancellationToken cancellationToken) {
    return new AgentCard() {
      Name = "Echo Agent",
      Description = "An agent that will echo every message it receives.",
      Url = agentUrl,
      Version = "1.0.0",
      DefaultInputModes = ["text"],
      DefaultOutputModes = ["text"],
      Capabilities = new AgentCapabilities() { Streaming = true },
      Skills = [],
    };
  }
}
```

### Hosting the Agent

Update `Program.cs` to connect and expose the agent:

```csharp
using A2A;
using A2A.AspNetCore;
using A2AAgent;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();
EchoAgent agent = new EchoAgent();
TaskManager taskManager = new TaskManager();
agent.Attach(taskManager);
app.MapA2A(taskManager, "/agent");
await app.RunAsync();
```

Once running, your agent is discoverable and accessible at `http://localhost:5000/agent`.

## Testing the Agent With a Client

Set up a new client console app:

```bash
dotnet new console -n A2AClient -f net9.0
cd A2AClient
dotnet add package A2A --prerelease
dotnet add package System.Net.ServerSentEvents --prerelease
```

Create the client to discover and message the agent:

```csharp
A2ACardResolver cardResolver = new(new Uri("http://localhost:5000/"));
AgentCard echoAgentCard = await cardResolver.GetAgentCardAsync();
Console.WriteLine($"Connected to agent: {echoAgentCard.Name}");
Console.WriteLine($"Description: {echoAgentCard.Description}");
Console.WriteLine($"Streaming support: {echoAgentCard.Capabilities?.Streaming}");
A2AClient agentClient = new(new Uri(echoAgentCard.Url));
Message userMessage = new() {
  Role = MessageRole.User,
  MessageId = Guid.NewGuid().ToString(),
  Parts = [new TextPart { Text = "Hello from the A2A client!" }]
};
Console.WriteLine("\n=== Non-Streaming Communication ===");
Message agentResponse = (Message)await agentClient.SendMessageAsync(new MessageSendParams { Message = userMessage });
Console.WriteLine($"Received response: {((TextPart)agentResponse.Parts[0]).Text}");
Console.WriteLine("\n=== Streaming Communication ===");
await foreach (SseItem<A2AEvent> sseItem in agentClient.SendMessageStreamAsync(new MessageSendParams { Message = userMessage })) {
  Message streamingResponse = (Message)sseItem.Data;
  Console.WriteLine($"Received streaming chunk: {((TextPart)streamingResponse.Parts[0]).Text}");
}
```

## Explore Further

- **A2A Inspector Tool:** Web-based tool for debugging and experimenting with A2A agents ([A2A Inspector](https://github.com/a2aproject/a2a-inspector))
- **Samples:** [A2A .NET SDK Samples](https://github.com/a2aproject/a2a-dotnet/tree/main/samples)
- **Join the Community:** [AI Foundry Developer Forum](https://aka.ms/azureaifoundry/forum), [A2A Discussions](https://github.com/a2aproject/a2a-dotnet/discussions/landing)
- **Semantic Kernel Integration:** [Learn More](https://learn.microsoft.com/en-us/semantic-kernel/)

---
The A2A .NET SDK is open source (Apache 2.0). Community participation is encouraged.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/foundry/building-ai-agents-a2a-dotnet-sdk/)
