---
external_url: https://medium.com/@davidfowl/model-run-ship-the-new-way-to-build-distributed-apps-48d67286a665?source=rss-8163234c98f0------2
title: Model. Run. Ship. The New Way to Build Distributed Apps with Aspire
author: David Fowler
viewing_mode: external
feed_name: David Fowler's Blog
date: 2025-04-27 22:27:35 +00:00
tags:
- .NET
- Application Modeling
- Aspire
- Automation
- C#
- Ci/cd
- Cloud Native
- Deployment
- Distributed Applications
- Docker Compose
- GitHub Actions
- IaC
- Microservices
- Pulumi
- Software Development
- Terraform
- Web Development
section_names:
- coding
- devops
---
In this post, David Fowler discusses how Aspire streamlines the modeling and deployment of distributed applications, making connections between services explicit and repeatable.<!--excerpt_end-->

# Model. Run. Ship. The New Way to Build Distributed Apps with Aspire

*Written by David Fowler*

## Introduction

Modern application development often involves assembling several components—a frontend, an API, supporting services like Redis or Postgres—and managing the “glue” between them through Docker Compose or custom scripts. This indirect structure often proves brittle, particularly at deployment time.

Aspire is introduced as a way to:  

- Make the glue between services explicit  
- Serve as an **app host** (not a frontend, API, or infrastructure-as-code tool)  
- Bring together projects, services, and environment variables into a coherent, repeatable system

This article explores how Aspire enables the evolution from a collection of isolated projects to a unified, deployable product.

---

## Composing Applications: The Aspire Model

Aspire’s guiding principle is to **model your entire application as a system**—not merely as individual components.

- Aspire does not care about the internal tech stack (Blazor, MVC, Minimal APIs, React, etc.), only about:
  - What are the parts?
  - How do those parts connect?
  - What do they need to run?
- Developers describe their app *to* Aspire (not build *in* Aspire)

**Sample Code:**

```csharp
var builder = DistributedApplication.CreateBuilder(args);
var redis = builder.AddRedis("cache");
var db = builder.AddPostgres("db");
var api = builder.AddProject("api")
    .WithReference(redis)
    .WithReference(db);
var ui = builder.AddNpmApp("frontend", "../frontend")
    .WithNpmPackageInstallation()
    .WithHttpEndpoint(env: "PORT")
    .WithReverseProxy(api.GetEndpoint("http"))
    .WithExternalHttpEndpoints();
```

- This code does not replace Dockerfiles or infrastructure scripts. It offers a **model** that can drive both.

To see a working example, check out [aspire-ai-chat-demo](https://github.com/davidfowl/aspire-ai-chat-demo), illustrating multi-service orchestration in a single system.

---

## One Model, Multiple Environments

Aspire enables a single model to serve both local development and production deployments.

**Environment Switching Pattern:**

```csharp
if (builder.ExecutionContext.IsPublishMode)
{
    ui.PublishAsDockerFile(config => {
        config.WithReverseProxy(api.GetEndpoint("http"));
    });
}
else
{
    ui.WithEnvironment("BACKEND_URL", api.GetEndpoint("http"));
}
```

**Local Development:**

```shell
aspire run
```

- Runs containers, launches services, wires everything up automatically.

**Production or CI:**

```shell
aspire publish -p docker-compose -o artifacts
```

- Produces Docker-compose artifacts and deployable configurations with no code changes.

---

## Publishing to Production (VPS Workflow Example)

Aspire converts the application model into deployment artifacts. The deployment target can be any VPS capable of running containers (e.g., DigitalOcean, not limited to Azure or Kubernetes).

**Key Points:**

- Aspire models deployment, agnostic to the underlying infrastructure
- Suitable for home labs, hobby projects, or small teams seeking ease of repeatable deployments
- The [aspire-ai-chat-demo](https://github.com/davidfowl/aspire-ai-chat-demo) uses a GitHub Actions workflow for automation:  

**CI/CD Example (GitHub Actions):**

```yaml
name: Aspire Publish Pipeline
on:
  push:
    branches: ['*']
  pull_request:
    branches: ['*']
permissions:
  contents: read
  packages: write
jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout code
        uses: actions/checkout@v3
      - name: Set up .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '9.x'
      - name: Install Aspire CLI
        run: dotnet tool install --global aspire.cli --prerelease
      - name: Run Aspire Publish
        run: aspire publish -p docker-compose -o artifacts
        working-directory: AIChat.AppHost
      - name: Log in to GitHub Container Registry
        run: echo "${{ secrets.GITHUB_TOKEN }}" | docker login ghcr.io -u ${{ github.actor }} --password-stdin
      - name: Tag and Push Container Images
        run: |
          BUILD_NUMBER=${{ github.run_number }}
          BRANCH_NAME=${{ github.ref_name }}
          SANITIZED_BRANCH_NAME=$(echo "$BRANCH_NAME" | sed 's#[^a-zA-Z0-9._-]#-#g')
          for image in chatui chatapi; do
            docker tag $image:latest ghcr.io/${{ github.repository_owner }}/$image:${SANITIZED_BRANCH_NAME}-${BUILD_NUMBER}
            docker push ghcr.io/${{ github.repository_owner }}/$image:${SANITIZED_BRANCH_NAME}-${BUILD_NUMBER}
          done
```

**Manual Deployment via SSH:**

```shell
scp -r artifacts user@your-server.com:/home/user/aspire-app
ssh user@your-server.com << 'EOF'
cd /home/user/aspire-app
docker login ghcr.io -u $GITHUB_USER -p $GITHUB_TOKEN
docker compose down && docker compose up -d
EOF
```

Aspire simplifies, but does not replace, infrastructure management. It produces repeatable, portable outputs.

---

## The System is the Product

- A successful deployment is more than the frontend or the API—it's the **entire optimized system**, including service orchestration, infrastructure, configuration, and secrets.
- Aspire enables you to model, validate, mutate, generate, deploy, and reason about this system as a whole.

---

## Broader Applicability Beyond Microservices

- Aspire targets *distributed applications*—not just those following a microservices architecture.
- Whether you have two or five services (frontend, backend, queue, database, external API), Aspire provides the glue between components, reducing complexity and config drift.

---

## What’s Next for Aspire

The team is working toward:

- **Terraform and Pulumi publishers** for infrastructure provisioning
- **Reusable CI/CD pipeline generation**, allowing faster, less error-prone deployments directly from the modeled system

Goal: To establish Aspire as a foundational tool for distributed app delivery—suitable for everything from personal projects to production-grade systems.

---

## Try Out Aspire

If Aspire seems unfamiliar, start by asking how your app fits into Aspire—rather than the other way around.  
Remember, what you actually ship is a *system*, not just individual projects.

---

*For more details and real-world samples, visit [aspire-ai-chat-demo](https://github.com/davidfowl/aspire-ai-chat-demo).*

This post appeared first on "David Fowler's Blog". [Read the entire article here](https://medium.com/@davidfowl/model-run-ship-the-new-way-to-build-distributed-apps-48d67286a665?source=rss-8163234c98f0------2)
