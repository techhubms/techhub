---
external_url: https://andrewlock.net/converting-a-docker-compose-file-to-aspire/
title: 'Converting a docker-compose File to .NET Aspire: A Practical Implementation'
author: Andrew Lock
viewing_mode: external
feed_name: Andrew Lock's Blog
date: 2025-05-27 09:00:00 +00:00
tags:
- .NET Aspire
- .NET Core
- App Modeling
- Aspire
- Aspire App Host
- CI/CD
- Containerization
- Distributed Application
- Docker
- Docker Compose
- IaC
- JetBrains Rider
- Listmonk
- NuGet Packages
- PostgreSQL
section_names:
- coding
---
In this post, Andrew Lock demonstrates how to convert a docker-compose-based deployment for the mailing list manager listmonk into a .NET Aspire app host, leveraging Aspire's modeling tools for improved local development and publish workflows.<!--excerpt_end-->

# Converting a docker-compose File to .NET Aspire: A Practical Implementation

*By Andrew Lock*

---

## Introduction

This post describes the process of converting the deployment method for the open-source mailing-list manager listmonk from a docker-compose.yml file to a .NET Aspire app host project. Andrew Lock explores how this transition, while yielding a functionally similar application, brings several benefits, particularly regarding local development and automated publish artifact generation for deployments.

> *Note: The author has only experimented with Aspire and invites feedback for improvements or corrections.*

## High-Level Overview

### What is .NET Aspire?

.NET Aspire provides tools, templates, and packages to ease building observable, production-ready apps. Key benefits include:

- Simplification of application modeling via NuGet packages
- Focused on enhancing local development
- Streamlined configuration and interconnections between services

With .NET Aspire, you model your system’s dependencies (databases, services, etc.) directly in a .NET app host project. This approach helps manage configuration (like connection strings) that would otherwise be scattered and potentially error-prone in other systems.

### Overview of listmonk

[listmonk](https://listmonk.app/) is a self-hosted newsletter and mailing list manager that utilizes:

- Go for backend
- Vue + Buefy for UI frontend

It's open source under AGPLv3 and can be run as a single binary or, more commonly, via a docker-compose.yml file. Its most common stack consists of:

1. **App** - The listmonk service itself (docker container)
2. **Database** - A PostgreSQL instance (docker container)

The docker-compose.yml connects them, specifies environment variables, and sets up volumes and ports.

## The Conversion Process

The goal is to model this entire stack in a .NET Aspire app host and verify that the generated deployment artifacts (including a docker-compose.yml) replicate the original setup.

### 1. Prerequisites

Install the following tools:

- .NET 9 SDK (or .NET 8)
- Docker Desktop (or another OCI runtime)
- .NET Aspire project templates:

  ```bash
  dotnet new install Aspire.ProjectTemplates
  ```

### 2. Create the Aspire App Host Project

Create a folder and initialize the app host:

```bash
mkdir LismonkAspire
cd LismonkAspire
dotnet new aspire-apphost
```

This sets up a minimal .NET 9 Aspire 9.3 app host project, initially with just:

```csharp
var builder = DistributedApplication.CreateBuilder(args);
builder.Build().Run();
```

### 3. Modeling the Database

.NET Aspire allows you to model resources with built-in or plugin integrations. For PostgreSQL, install the relevant integration:

```bash
dotnet add package Aspire.Hosting.PostgreSQL
```

The database section in docker-compose defines three main environment variables for credentials and declares the service with volumes, port, and healthcheck.

Convert this in Aspire as follows:

```csharp
// Store credentials as secrets
var postgresUser = builder.AddParameter("db-user", secret: true);
var postgresPassword = builder.AddParameter("db-password", secret: true);
var postgresDbName = builder.AddParameter("db-name", "listmonk", publishValueAsDefault: true);

var dbPort = 5432;
var dbContainerName = "listmonk_db";

var db = builder.AddPostgres("db", postgresUser, postgresPassword, port: dbPort)
    .WithImage("postgres", "17-alpine")
    .WithContainerName(dbContainerName)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("listmonk-data")
    .WithEnvironment("POSTGRES_DB", postgresDbName);
```

*Benefits*: Using parameters and secrets simplifies variable usage and enhances security and maintainability compared to YAML anchors.

### 4. Modeling the listmonk App

Since there is no Aspire integration for listmonk, model it as a generic container:

```csharp
var listmonkSuperUser = builder.AddParameter("listmonk-admin-user", secret: true);
var listmonkSuperUserPassword = builder.AddParameter("listmonk-admin-password", secret: true);
var publicPort = 9000;

builder.AddContainer(name: "listmonk", image: "listmonk/listmonk", tag: "latest")
    .WaitFor(db)
    .WithHttpEndpoint(port: publicPort, targetPort: 9000)
    .WithExternalHttpEndpoints()
    .WithArgs("sh", "-c", "./listmonk --install --idempotent --yes --config '' && ./listmonk --upgrade --yes --config '' && ./listmonk --config ''")
    .WithBindMount(source: "./uploads", target: "/listmonk/uploads")
    .WithEnvironment("LISTMONK_app__address", $"0.0.0.0:{publicPort}")
    .WithEnvironment("LISTMONK_db__user", postgresUser)
    .WithEnvironment("LISTMONK_db__password", postgresPassword)
    .WithEnvironment("LISTMONK_db__database", postgresDbName)
    .WithEnvironment("LISTMONK_db__host", dbContainerName)
    .WithEnvironment("LISTMONK_db__port", dbPort.ToString())
    .WithEnvironment("LISTMONK_db__ssl_mode", "disable")
    .WithEnvironment("LISTMONK_db__max_open", "25")
    .WithEnvironment("LISTMONK_db__max_idle", "25")
    .WithEnvironment("LISTMONK_db__max_lifetime", "300s")
    .WithEnvironment("TZ", "Etc/UTC")
    .WithEnvironment("LISTMONK_ADMIN_USER", listmonkSuperUser)
    .WithEnvironment("LISTMONK_ADMIN_PASSWORD", listmonkSuperUserPassword);
```

*Note*: Some hardcoded variables reflect the docker-compose file for parity. Flowing shared values as variables is much easier than in YAML.

### 5. Running and Testing

Set secrets for local development (preferably via user-secrets):

```bash
dotnet user-secrets set "Parameters:db-user" "listmonk"
dotnet user-secrets set "Parameters:db-password" "listmonk"
dotnet user-secrets set "Parameters:listmonk-admin-user" "admin-user"
dotnet user-secrets set "Parameters:listmonk-admin-password" "admin-password"
```

Run the Aspire app:

- Using IDE (F5), or
- `dotnet run`

Aspire’s dashboard provides visibility into logs, environment details, and endpoint links, including the listmonk web app for login.

### 6. Exporting as docker-compose.yml

To compare Aspire’s artifact output with the original docker-compose.yml:

1. Install the publishing package:

   ```bash
   dotnet add package Aspire.Hosting.Docker --version 9.3.0-preview.1.25265.20
   ```

2. Enable the Docker Compose publisher:

   ```csharp
   builder.AddDockerComposeEnvironment("docker-compose");
   ```

3. Add publishing metadata for services:

   - For the app:

     ```csharp
     .PublishAsDockerComposeService((resource, service) => {
         service.Restart = "unless-stopped";
     });
     ```

   - For the database:

     ```csharp
     .PublishAsDockerComposeService((resource, service) => {
         service.Restart = "unless-stopped";
         service.Healthcheck = new() {
             Interval = "10s",
             Timeout = "5s",
             Retries = 6,
             StartPeriod = "0s",
             Test = ["CMD-SHELL", "pg_isready -U listmonk"]
         };
     });
     ```

4. Install Aspire CLI and generate artifacts:

   ```bash
   dotnet tool install --global aspire.cli --prerelease
   aspire publish -o publish
   ```

   - Outputs a .env and docker-compose.yml file.
   - The generated files closely mirror the original and are functionally equivalent, with only minor cosmetic differences.

## Summary

Andrew Lock demonstrates the practical conversion of a docker-compose deployment to a .NET Aspire app host, documenting the modeling of both the listmonk app and its database. The .NET Aspire approach streamlines configuration, enhances local development workflows, and offers modern publish capabilities—producing artifacts, including a docker-compose.yml, nearly identical to hand-authored sources. This experiment underscores Aspire’s promise for both .NET and non-.NET distributed application stacks.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/converting-a-docker-compose-file-to-aspire/)
