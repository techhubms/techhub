---
layout: post
title: Create Your Own Bicep Local Extension Using .NET
author: Sydney Smith
canonical_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/create-your-own-bicep-local-extension-using-net/ba-p/4439967
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-08-07 14:50:37 +00:00
permalink: /coding/community/Create-Your-Own-Bicep-Local-Extension-Using-NET
tags:
- .NET 9
- ARM
- Azure Bicep
- Bicep Extension
- Bicep Local
- C#
- CLI
- Custom Extension
- IaC
- Local Deployment
- Main.bicep
- Program.cs
- Project Scaffolding
- Resource Handler
- VS Code
section_names:
- azure
- coding
- devops
---
Sydney Smith provides a comprehensive walkthrough for creating Bicep Local Extensions with .NET, enabling local infrastructure as code workflows without Azure connectivity.<!--excerpt_end-->

# Create Your Own Bicep Local Extension Using .NET

Sydney Smith details a practical approach to building a custom extension for Bicep Local, designed to allow developers to manage resources locally with Bicep files—without needing an Azure connection. The guide covers every major phase, from environment setup to executing your extension and validating its output.

## Overview

Bicep Local enables fully local authoring and deployment of resources, with extensibility provided through extensions written in .NET. This guide outlines steps to create, build, and run such an extension.

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Bicep CLI v0.37.4 or higher](https://github.com/Azure/bicep/releases/tag/v0.37.4)
- VSCode Bicep extension

## Step-by-Step Guide

### 1. Project Scaffolding

- **Create `MyExtension.csproj`:**

  ```xml
  <Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
      <OutputType>Exe</OutputType>
      <RootNamespace>MyExtension</RootNamespace>
      <AssemblyName>my-extension</AssemblyName>
      <IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
      <PublishSingleFile>true</PublishSingleFile>
      <SelfContained>true</SelfContained>
      <InvariantGlobalization>true</InvariantGlobalization>
      <TargetFramework>net9.0</TargetFramework>
      <Nullable>enable</Nullable>
      <ImplicitUsings>enable</ImplicitUsings>
      <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
      <AppendRuntimeIdentifierToOutputPath>false</AppendRuntimeIdentifierToOutputPath>
    </PropertyGroup>
    <ItemGroup>
      <PackageReference Include="Azure.Bicep.Local.Extension" Version="0.37.4" />
    </ItemGroup>
  </Project>
  ```

- **Create `Program.cs`:**

  ```csharp
  using Microsoft.AspNetCore.Builder;
  using Bicep.Local.Extension.Host.Extensions;
  using Microsoft.Extensions.DependencyInjection;

  var builder = WebApplication.CreateBuilder();
  builder.AddBicepExtensionHost(args);
  builder.Services.AddBicepExtension(
    name: "MyExtension",
    version: "0.0.1",
    isSingleton: true,
    typeAssembly: typeof(Program).Assembly
  ).WithResourceHandler<MyResourceHandler>();
  var app = builder.Build();
  app.MapBicepExtension();
  await app.RunAsync();
  ```

- **Create `Models.cs`:**

  ```csharp
  using System.Text.Json.Serialization;
  using Azure.Bicep.Types.Concrete;
  using Bicep.Local.Extension.Types.Attributes;

  public enum OperationType { Uppercase, Lowercase, Reverse }
  public class MyResourceIdentifiers {
    [TypeProperty("The resource name", ObjectTypePropertyFlags.Identifier | ObjectTypePropertyFlags.Required)]
    public required string Name { get; set; }
  }
  [ResourceType("MyResource")]
  public class MyResource : MyResourceIdentifiers {
    [TypeProperty("The resource operation type", ObjectTypePropertyFlags.Required)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OperationType? Operation { get; set; }
    [TypeProperty("The text output")]
    public string? Output { get; set; }
  }
  ```

- **Create `Handlers/MyResourceHandler.cs`:**

  ```csharp
  using Bicep.Local.Extension.Host.Handlers;

  public class MyResourceHandler : TypedResourceHandler<MyResource, MyResourceIdentifiers> {
    protected override async Task<ResourceResponse> Preview(ResourceRequest request, CancellationToken cancellationToken) {
      await Task.CompletedTask;
      return GetResponse(request);
    }
    protected override async Task<ResourceResponse> CreateOrUpdate(ResourceRequest request, CancellationToken cancellationToken) {
      await Task.CompletedTask;
      request.Properties.Output = request.Properties.Operation switch {
        OperationType.Uppercase => request.Properties.Name.ToUpperInvariant(),
        OperationType.Lowercase => request.Properties.Name.ToLowerInvariant(),
        OperationType.Reverse => new string(request.Properties.Name.Reverse().ToArray()),
        _ => throw new InvalidOperationException(),
      };
      return GetResponse(request);
    }
    protected override MyResourceIdentifiers GetIdentifiers(MyResource properties) => new() { Name = properties.Name };
  }
  ```

---

### 2. Publishing the Extension

In your project directory, publish for all platforms:

```shell
dotnet publish --configuration release -r osx-arm64 .
dotnet publish --configuration release -r linux-x64 .
dotnet publish --configuration release -r win-x64 .

bicep publish-extension --bin-osx-arm64 ./bin/release/osx-arm64/publish/my-extension --bin-linux-x64 ./bin/release/linux-x64/publish/my-extension --bin-win-x64 ./bin/release/win-x64/publish/my-extension.exe --target ./bin/my-extension --force
```

### 3. Running Your Extension

- **Create `bicepconfig.json`:**

  ```json
  {
    "experimentalFeaturesEnabled": { "localDeploy": true },
    "extensions": { "myextension": "./bin/my-extension" },
    "implicitExtensions": []
  }
  ```

- **Create `main.bicep`:**

  ```bicep
  targetScope = 'local'
  extension myextension
  param inputText string
  resource foo 'MyResource' = {
    name: inputText
    operation: 'Reverse'
  }
  output outputText string = foo.output
  ```

- **Create `main.bicepparam`:**

  ```bicep
  using 'main.bicep'
  param inputText = 'Please reverse me!'
  ```

- **Run Local Deploy:**

  ```shell
  bicep local-deploy main.bicepparam
  ```

  Example output:

  ```
  Output outputText: "!em esrever esaelP"
  Resource foo (Create): Succeeded
  Result: Succeeded
  ```

---

## Support and Feedback

Bicep Local and its extension ecosystem are evolving. Feedback is welcome via the [Azure Bicep GitHub repository](https://github.com/Azure/bicep).

---

*Author: Sydney Smith*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/create-your-own-bicep-local-extension-using-net/ba-p/4439967)
