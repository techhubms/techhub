---
external_url: https://devblogs.microsoft.com/dotnet/mcp-server-dotnet-nuget-quickstart/
title: Building Your First MCP Server with .NET 10 and Publishing to NuGet
author: Jon Douglas, Joel Verhagen, Jon Galloway
viewing_mode: external
feed_name: Microsoft .NET Blog
date: 2025-07-15 20:00:00 +00:00
tags:
- .NET
- .NET 10
- AI Assistants
- C#
- Community Tools
- Configuration
- MCP
- NuGet
- Open Standard
- Project Publishing
- SDK
- Server Development
- Templates
- VS Code
section_names:
- ai
- github-copilot
- coding
---
Written by Jon Douglas, Joel Verhagen, and Jon Galloway, this article demonstrates how to build, customize, and publish your first Model Context Protocol (MCP) server using .NET 10, enabling discoverable AI integrations via NuGet.<!--excerpt_end-->

## Building Your First MCP Server with .NET and Publishing to NuGet

**Authors:** Jon Douglas, Joel Verhagen, Jon Galloway

Want to extend AI assistants with custom capabilities? In this post, you’ll learn how to build a Model Context Protocol (MCP) server using .NET 10 and publish it to NuGet, making your AI tools discoverable and reusable by the entire .NET community. Additionally, the article covers new .NET 10 and NuGet features supporting MCP server development and introduces a new project template for easier setup.

---

### What is the Model Context Protocol (MCP)?

The **Model Context Protocol (MCP)** is an open standard enabling AI assistants to securely connect to external data sources and tools. MCP serves as a bridge between AI models and real-world systems—allowing assistants to access databases, APIs, file systems, and custom business logic. .NET 10's new MCP templates empower developers to create and publish these servers to NuGet.

---

### NuGet and MCP

NuGet.org now supports hosting and consuming MCP servers built with the [ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol) C# SDK. Key benefits:

- **Discoverability**: MCP servers are searchable via NuGet
- **Versioning**: Semantic versioning and dependency management
- **Simple Installation**: Easy configuration for VS Code and Visual Studio
- **Community Growth**: Expand the ecosystem of .NET AI tools

Search for MCP servers: [NuGet MCP Server Packages](https://www.nuget.org/packages?packagetype=McpServer)

---

## Creating an MCP Server: Step by Step

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (preview 6+)
- [Visual Studio Code](https://code.visualstudio.com/)
- [GitHub Copilot extension](https://marketplace.visualstudio.com/items?itemName=GitHub.copilot)
- [NuGet.org account](https://www.nuget.org/users/account/LogOn)

### Step 1: Install MCP Server Project Template

```bash
dotnet new install Microsoft.Extensions.AI.Templates
```

### Step 2: Create the Project

```bash
dotnet new mcpserver -n SampleMcpServer
cd SampleMcpServer
dotnet build
```

---

## Enhancing the MCP Server

The default template gives you a working MCP server with a sample `get_random_number` tool. To add a custom weather tool:

Create `WeatherTools.cs` in the `Tools` directory:

```csharp
[McpServerTool]
[Description("Describes random weather in the provided city.")]
public string GetCityWeather(
    [Description("Name of the city to return weather for")] string city)
{
    var weather = Environment.GetEnvironmentVariable("WEATHER_CHOICES");
    if (string.IsNullOrWhiteSpace(weather)) {
        weather = "balmy,rainy,stormy";
    }
    var weatherChoices = weather.Split(",");
    var selectedWeatherIndex = Random.Shared.Next(0, weatherChoices.Length);
    return $"The weather in {city} is {weatherChoices[selectedWeatherIndex]}.";
}
```

Update `Program.cs` to include your tool:

```csharp
.WithTools<WeatherTools>()
```

---

## Testing with GitHub Copilot

Configure GitHub Copilot by creating `.vscode/mcp.json`:

```json
{
  "servers": {
    "SampleMcpServer": {
      "type": "stdio",
      "command": "dotnet",
      "args": [ "run", "--project", "." ],
      "env": { "WEATHER_CHOICES": "sunny,humid,freezing,perfect" }
    }
  }
}
```

Test with prompts like:

- "What’s the weather in Seattle?"
- "Give me a random number between 1 and 100"

VS Code will reflect available MCP server tools in Copilot.

![VS Code screenshot with MCP server tools available in GitHub Copilot](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2025/07/mcp-tools-demo.png)

---

## Preparing for NuGet Publication

Update `.mcp/server.json`:

```json
{
  "description": "A sample MCP server with weather and random number tools",
  "name": "io.github.yourusername/SampleMcpServer",
  "packages": [
    {
      "registry_name": "nuget",
      "name": "YourUsername.SampleMcpServer",
      "version": "1.0.0",
      "package_arguments": [],
      "environment_variables": [
        {
          "name": "WEATHER_CHOICES",
          "description": "Comma separated list of weather descriptions",
          "is_required": true,
          "is_secret": false
        }
      ]
    }
  ],
  "repository": {
    "url": "https://github.com/yourusername/SampleMcpServer",
    "source": "github"
  },
  "version_detail": {
    "version": "1.0.0"
  }
}
```

Update your `.csproj`:

```xml
<PackageId>YourUsername.SampleMcpServer</PackageId>
```

---

## Publishing Your Project to NuGet

1. **Pack Your Project**:

   ```bash
   dotnet pack -c Release
   ```

2. **Publish to NuGet**:

   ```bash
   dotnet nuget push bin/Release/*.nupkg --api-key <your-api-key> --source https://api.nuget.org/v3/index.json
   ```

   **Tip:** For testing, use [int.nugettest.org](https://int.nugettest.org) before publishing live.

---

## Discoverability and Usage

Once published, your MCP server will show up under [NuGet MCP Server Packages](https://www.nuget.org/packages?packagetype=mcpserver).

- **Explore Package**: View docs and copy configuration
- **Install and Configure**: Add settings to `.vscode/mcp.json`

Example configuration for VS Code:

```json
{
  "inputs": [
    {
      "type": "promptString",
      "id": "weather-choices",
      "description": "Comma separated list of weather descriptions",
      "password": false
    }
  ],
  "servers": {
    "YourUsername.SampleMcpServer": {
      "type": "stdio",
      "command": "dnx",
      "args": [ "YourUsername.SampleMcpServer", "--version", "1.0.0", "--yes" ],
      "env": { "WEATHER_CHOICES": "${input:weather-choices}" }
    }
  }
}
```

VS Code will prompt for these environment settings the first time a user interacts.

![Screenshot showing MCP server package search and configuration](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2025/07/nuget-mcp-search.png)

---

## Future Directions

The .NET + MCP + NuGet integration paves the way for extensible AI systems. Potential use cases include:

- Enterprise database gateways (e.g., SQL Server)
- Cloud API orchestrators (Azure, AWS)
- Document intelligence with OCR
- DevOps pipelines and workflow tools
- Data analytics and reporting engines

---

## Learn More and Resources

- [Get started with .NET AI and the Model Context Protocol](https://learn.microsoft.com/dotnet/ai/get-started-mcp)
- [Model Context Protocol .NET samples](https://github.com/microsoft/mcp-dotnet-samples)
- [NuGet.org MCP Server Search](https://www.nuget.org/packages?packagetype=mcpserver)
- [MCP Registry Documentation](https://github.com/modelcontextprotocol/registry)
- [What’s new in .NET 10](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview)

> .NET + MCP + NuGet = The future of extensible AI

Happy building, and welcome to the growing community of MCP server creators!

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/mcp-server-dotnet-nuget-quickstart/)
