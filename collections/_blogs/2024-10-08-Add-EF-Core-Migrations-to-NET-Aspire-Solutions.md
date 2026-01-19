---
layout: post
title: Add EF Core Migrations to .NET Aspire Solutions
author: Khalid Abuhakmeh
canonical_url: https://khalidabuhakmeh.com/add-ef-core-migrations-to-dotnet-aspire-solutions
viewing_mode: external
feed_name: Khalid Abuhakmeh's Blog
feed_url: https://khalidabuhakmeh.com/feed.xml
date: 2024-10-08 00:00:00 +00:00
permalink: /coding/blogs/Add-EF-Core-Migrations-to-NET-Aspire-Solutions
tags:
- .NET
- .NET Aspire
- .NET CLI
- Aspire
- AspireSandbox
- Database Schema
- DbContext
- Distributed Applications
- EF Core
- EF Core Migrations
- IDesignTimeDbContextFactory
- PostgreSQL
- VS
section_names:
- coding
---
In this detailed guide, Khalid Abuhakmeh walks through bridging the development/runtime gap for managing EF Core migrations within .NET Aspire distributed solutions. The post explores solution structure, code snippets, and essential CLI commands.<!--excerpt_end-->

# Add EF Core Migrations to .NET Aspire Solutions

*By Khalid Abuhakmeh*

![Add EF Core Migrations to .NET Aspire Solutions](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/dotnet-aspire-ef-core-migrations-approach.jpg)

Photo by [NIPYATA!](https://unsplash.com/@nipyata)

Working with Entity Framework Core (EF Core) brings powerful migration features which are essential for managing schema changes in .NET solutions. In distributed solutions enabled by .NET Aspire, bridging the gap between development-time actions (like managing migrations) and runtime execution (applying migrations to a database) is crucial.

While the [Microsoft documentation on Aspire and EF Core migrations](https://learn.microsoft.com/en-us/dotnet/aspire/database/ef-core-migrations) focuses on runtime concerns, it omits important development-time tasks. This post explains how to manage EF Core migrations during development in .NET Aspire solutions.

## Solution Structure

Understanding your solution layout is the first step. For a distributed Aspire app, you might have the following design:

```
AspireSandbox
├─ AspireSandbox.AppHost
├─ AspireSandbox.Data
├─ AspireSandbox.ServiceDefaults
└─ AspireSandbox.Web
```

The `AspireSandbox.Data` project contains the `DbContext` implementation and is referenced by the application host. Here is a sample (simplified) implementation:

```csharp
using Microsoft.EntityFrameworkCore;

namespace AspireSandbox.Data;

public class Database(DbContextOptions<Database> options) : DbContext(options)
{
    public DbSet<Count> Counts => Set<Count>();
}

public class Count
{
    public int Id { get; set; }
    public DateTimeOffset CountedAt { get; set; } = DateTimeOffset.UtcNow;
}
```

### Dependencies

The project requires Entity Framework Core and Npgsql for PostgreSQL integration. Example snippet from `AspireSandbox.Data.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.8" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.8">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.8" />
  </ItemGroup>
</Project>
```

Reference this project from `AspireSandbox.AppHost`, making sure to add the attribute `IsAspireProjectResource="false"` to the project reference:

```xml
<ItemGroup>
  <ProjectReference Include="..\AspireSandbox.Data\AspireSandbox.Data.csproj" IsAspireProjectResource="false" />
</ItemGroup>
```

This prevents the project from being included as a resource in Aspire source generation.

## Entity Framework Core Design-Time Factory

EF Core uses a design-time DbContext factory for tooling scenarios. When using Aspire for distributed solutions, you want your design-time tooling to spin up dependencies, such as databases.

Here’s an example of implementing a `DataContextDesignTimeFactory` in `AspireSandbox.AppHost`:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AspireSandbox.AppHost;

public sealed class DataContextDesignTimeFactory : IDesignTimeDbContextFactory<Data.Database>
{
    public Data.Database CreateDbContext(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var postgres = builder
            .AddPostgres("postgres")
            .AddDatabase("migrations", databaseName: "migrations");

        var optionsBuilder = new DbContextOptionsBuilder<Data.Database>();
        optionsBuilder.UseNpgsql("migrations");
        return new Data.Database(optionsBuilder.Options);
    }
}
```

This factory initializes necessary dependencies (PostgreSQL) and enables EF Core tooling to create or update migrations. Since EF Core uses a model snapshot as a C# file, database persistence is not required between migration runs.

## Running EF Core CLI Commands

To create or update migrations, use the `dotnet ef` CLI, specifying both the data and apphost projects:

```bash
dotnet ef migrations --project ./AspireSandbox.Data --startup-project ./AspireSandbox.AppHost add Initial
```

Adapt `--project` and `--startup-project` parameters to match your solution structure.

If executed correctly, a new database migration will be generated, reflecting your model changes.

## Conclusion

With a proper `IDesignTimeDbContextFactory` implementation and adjusted CLI workflows, you can fluidly develop with Aspire and Entity Framework Core—managing new migrations during development as well as execution at runtime.

Thanks to James Hancock for his insights in the Aspire GitHub issues that inspired this post.

---

![Khalid Abuhakmeh's Picture](/assets/images/authorimage.jpg)

## About Khalid Abuhakmeh

Khalid is a developer advocate at JetBrains focusing on .NET technologies and tooling.

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/add-ef-core-migrations-to-dotnet-aspire-solutions)
