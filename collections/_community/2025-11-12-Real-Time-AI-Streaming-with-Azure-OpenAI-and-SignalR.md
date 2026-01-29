---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/real-time-ai-streaming-with-azure-openai-and-signalr/ba-p/4468833
title: Real-Time AI Streaming with Azure OpenAI and SignalR
author: pranav_pratik
feed_name: Microsoft Tech Community
date: 2025-11-12 09:38:46 +00:00
tags:
- .NET 8
- AI Integration
- Angular
- ASP.NET Core
- Azure OpenAI
- Azure SignalR Service
- Cloud Scale
- Content Filtering
- DefaultAzureCredential
- Entra ID
- Frontend Integration
- Live Chat
- Real Time UX
- SignalR
- Streaming Responses
- WebSockets
- AI
- Azure
- Coding
- DevOps
- Community
section_names:
- ai
- azure
- coding
- devops
primary_section: ai
---
pranav_pratik demonstrates how to deliver a ChatGPT-style streaming AI experience in your own apps by integrating Azure OpenAI Service with real-time SignalR communication and Angular frontend, covering the end-to-end architecture and best practices.<!--excerpt_end-->

# Real-Time AI Streaming with Azure OpenAI and SignalR

This guide outlines how to deliver incremental, live-streamed AI responses in your own applications—similar to ChatGPT—by combining Azure OpenAI Service, ASP.NET Core SignalR, and an Angular frontend.

## Why Stream AI Responses?

- **Reduces perceived latency:** Users see the answer as it's generated, improving experience and engagement.
- **Mimics ChatGPT:** The typing effect keeps users interested, especially for long-form answers.
- **Scales for enterprise:** Azure SignalR Service manages high connection loads and simplifies scale-out.

## Solution Overview

- A SignalR Hub in ASP.NET Core calls Azure OpenAI with streaming enabled, sending partial results to clients.
- The Angular client receives and renders partial output as it's produced, optionally showing typing indicators.
- Azure SignalR Service can be used to manage scale, removing the need for sticky sessions on the backend.

## Architecture

- **Backend:** .NET 8 API using ASP.NET Core SignalR and Azure OpenAI.
- **Frontend:** Angular 16+ using @microsoft/signalr client to receive real-time messages.
- **Optional:** Azure SignalR Service for scalability.

## Prerequisites

- Azure OpenAI resource (e.g., with gpt-4o or gpt-4o-mini model)
- .NET 8 and ASP.NET Core
- Angular 16+

## Backend Implementation

### 1. Install Required Packages

```sh
dotnet add package Microsoft.AspNetCore.SignalR
 dotnet add package Azure.AI.OpenAI --prerelease
 dotnet add package Azure.Identity
 dotnet add package Microsoft.Extensions.AI
 dotnet add package Microsoft.Extensions.AI.OpenAI --prerelease
 dotnet add package Microsoft.Azure.SignalR
```

### 2. Use DefaultAzureCredential for Authentication

Leveraging Entra ID with DefaultAzureCredential means you don't store secrets in code.

### 3. Program.cs Snippet

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
// builder.Services.AddSignalR().AddAzureSignalR(); // Uncomment for Azure SignalR scaling
builder.Services.AddSingleton<AiStreamingService>();
var app = builder.Build();
app.MapHub<ChatHub>("/chat");
app.Run();
```

### 4. AiStreamingService.cs (AI Streaming Logic)

Streams content from Azure OpenAI using modern .NET AI extensions:

```csharp
public class AiStreamingService {
  private readonly IChatClient _chatClient;
  public AiStreamingService(IConfiguration config) {
    var endpoint = new Uri(config["AZURE_OPENAI_ENDPOINT"]);
    var deployment = config["AZURE_OPENAI_DEPLOYMENT"];
    var azureClient = new AzureOpenAIClient(endpoint, new DefaultAzureCredential());
    _chatClient = azureClient.GetChatClient(deployment).AsIChatClient();
  }
  public async IAsyncEnumerable<string> StreamReplyAsync(string userMessage) {
    var messages = new List<ChatMessage> {
      ChatMessage.CreateSystemMessage("You are a helpful assistant."),
      ChatMessage.CreateUserMessage(userMessage)
    };
    await foreach (var update in _chatClient.CompleteChatStreamingAsync(messages)) {
      var chunk = string.Join("", update.Content
        .Where(p => p.Kind == ChatMessageContentPartKind.Text)
        .Select(p => ((TextContent)p).Text));
      if (!string.IsNullOrEmpty(chunk))
        yield return chunk;
    }
  }
}
```

### 5. ChatHub.cs (SignalR Hub)

Forwards incremental AI responses to the final client:

```csharp
public class ChatHub : Hub {
  private readonly AiStreamingService _ai;
  public ChatHub(AiStreamingService ai) => _ai = ai;
  public async Task AskAi(string prompt) {
    var messageId = Guid.NewGuid().ToString("N");
    await Clients.Caller.SendAsync("typing", messageId, true);
    await foreach (var partial in _ai.StreamReplyAsync(prompt)) {
      await Clients.Caller.SendAsync("partial", messageId, partial);
    }
    await Clients.Caller.SendAsync("typing", messageId, false);
    await Clients.Caller.SendAsync("completed", messageId);
  }
}
```

## Frontend Implementation (Angular)

### 1. Install SignalR Package

```sh
npm i @microsoft/signalr
```

### 2. Create ai-stream.service.ts

```typescript
@Injectable({ providedIn: 'root' })
export class AiStreamService {
  private connection?: signalR.HubConnection;
  private typing$ = new BehaviorSubject<boolean>(false);
  private partial$ = new BehaviorSubject<string>('');
  private completed$ = new BehaviorSubject<boolean>(false);

  get typing(): Observable<boolean> { return this.typing$.asObservable(); }
  get partial(): Observable<string> { return this.partial$.asObservable(); }
  get completed(): Observable<boolean> { return this.completed$.asObservable(); }

  async start(): Promise<void> {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/chat')
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.connection.on('typing', (_id: string, on: boolean) => this.typing$.next(on));
    this.connection.on('partial', (_id: string, text: string) => {
      this.partial$.next((this.partial$.value || '') + text);
    });
    this.connection.on('completed', (_id: string) => this.completed$.next(true));
    await this.connection.start();
  }
  async ask(prompt: string): Promise<void> {
    this.partial$.next('');
    this.completed$.next(false);
    await this.connection?.invoke('AskAi', prompt);
  }
}
```

### 3. ai-chat.component.ts

```typescript
@Component({ selector: 'app-ai-chat', templateUrl: './ai-chat.component.html', styleUrls: ['./ai-chat.component.css'] })
export class AiChatComponent implements OnInit {
  prompt = '';
  output = '';
  typing = false;
  done = false;
  constructor(private ai: AiStreamService) {}
  async ngOnInit() {
    await this.ai.start();
    this.ai.typing.subscribe(on => this.typing = on);
    this.ai.partial.subscribe(text => this.output = text);
    this.ai.completed.subscribe(done => this.done = done);
  }
  async send() {
    this.output = '';
    this.done = false;
    await this.ai.ask(this.prompt);
  }
}
```

### 4. ai-chat.component.html

```html
<div class="chat">
  <div class="prompt">
    <input [(ngModel)]="prompt" placeholder="Ask me anything…" />
    <button (click)="send()">Send</button>
  </div>
  <div class="response">
    <pre>{{ output }}</pre>
    <div class="typing" *ngIf="typing">Assistant is typing…</div>
    <div class="done" *ngIf="done">✓ Completed</div>
  </div>
</div>
```

## Streaming Modes & Content Filtering

- **Default**: Buffers and filters output by chunks before sending.
- **Async Filter**: Immediate token streaming, moderation handled asynchronously (needs client-side handling for delayed moderation actions).

## Best Practices

- Batch updates to the DOM for efficiency.
- Only log full responses server-side after completion.
- Store connection strings in Azure Key Vault and rotate regularly. Authenticate with Entra ID (DefaultAzureCredential).

## Scaling & Security

- Use Azure SignalR Service for scale; avoid sticky sessions in distributed setups.
- Configure CORS if hosting frontend and backend on different origins.

## Learn More

- [AI‑Powered Group Chat sample (ASP.NET Core)](https://learn.microsoft.com/en-us/aspnet/core/tutorials/ai-powered-group-chat/ai-powered-group-chat?view=aspnetcore-9.0)
- [Azure OpenAI .NET client (auth & streaming)](https://learn.microsoft.com/en-us/dotnet/api/overview/azure/ai.openai-readme?view=azure-dotnet)
- [SignalR JavaScript Client](https://learn.microsoft.com/en-us/aspnet/core/signalr/javascript-client?view=aspnetcore-9.0)

---

*Author: pranav_pratik*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/real-time-ai-streaming-with-azure-openai-and-signalr/ba-p/4468833)
