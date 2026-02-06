---
external_url: https://code-maze.com/efcore-testing-database-connectivity/
title: Testing Database Connectivity with EF Core
author: Georgi Georgiev
feed_name: Code Maze Blog
date: 2025-01-23 11:08:23 +00:00
tags:
- .NET
- ASP.NET Core
- C#
- Connectivity
- Container Orchestration
- Database
- Database Connectivity
- DbContext
- EF Core
- Health Checks
- Kubernetes
- SQL Server
- Testcontainers
- DevOps
- Blogs
section_names:
- dotnet
- devops
primary_section: dotnet
---
In this article, Georgi Georgiev explores essential techniques for validating database connectivity with EF Core, covering both application and infrastructure-level health checks for resilient .NET services.<!--excerpt_end-->

# Testing Database Connectivity with EF Core

*Author: Georgi Georgiev*

Ensuring reliable database connectivity is a critical aspect of application development—whether during application startup, within multi-step workflows, or as part of active application health monitoring. This reliability improves user experience and operational stability. Modern containerized environments, especially those orchestrated with Kubernetes (K8S), further underscore the importance of health reporting for effective service management.

This article details two practical approaches for testing database availability using Entity Framework Core (EF Core): the `CanConnectAsync()` method for application-level connectivity checks, and using DbContext with the ASP.NET Core health check framework for infrastructure-level monitoring. These methods support not only robust application behavior but also integrate with service orchestrators in cloud-native scenarios.

---

## Prerequisites for Running the Database Connectivity Checks

You’ll need a running SQL Server database. To streamline setup and ensure automation, the article utilizes the [Testcontainers](https://code-maze.com/csharp-testing-using-testcontainers-for-net-and-docker/) NuGet package. This package allows you to create and manage a SQL Server instance within a Docker container, provided you have Docker installed and running on your system.

After starting the SQL Server Test Container, configure your `ApplicationDbContext` to connect to the containerized SQL Server. Example setup in C#:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString), contextLifetime: ServiceLifetime.Scoped);
```

---

## Using CanConnectAsync for Database Connectivity Checks

There are numerous scenarios where proactively verifying database connectivity is valuable:

- **Multi-step processes** relying on repeated database access can validate connectivity before performing complex operations.
- **Offline-capable applications** that reduce features and notify users when the database is unreachable.
- **Failover strategies**, where applications may switch to backup infrastructure if the main database is down.
- **Routine monitoring and alerting** for operations teams to detect and resolve downtime swiftly.

EF Core’s `CanConnectAsync()` method checks whether a database connection can be established by executing a simple `SELECT 1` query. It returns `true` if successful; otherwise, it returns `false`.

### Example: Minimal API Implementation

Here’s a minimal API endpoint in ASP.NET Core utilizing `CanConnectAsync()`:

```csharp
app.MapGet("/can-connect", async (ApplicationDbContext applicationDbContext, CancellationToken cancellationToken) => {
    var canConnect = await applicationDbContext.Database.CanConnectAsync(cancellationToken);
    return canConnect ? Results.Ok("Connected successfully") : Results.Problem("Cannot connect", statusCode: StatusCodes.Status503ServiceUnavailable);
});
```

- A successful connection attempt returns `Connected successfully` (HTTP 200).
- A failed attempt returns `Cannot connect` with HTTP 503 (Service Unavailable).

To test, send a GET request to `/can-connect` while the Docker container is running. Stopping the container will trigger the `Service Unavailable` response.

---

## Using HealthChecks for Infrastructure-Level and Container Orchestration Monitoring

Complex applications often consist of multiple interacting services within a private network. For orchestrators such as Kubernetes, actively reporting service health is essential for routing traffic and maintaining system reliability.

### ASP.NET Core Health Checks and EF Core

ASP.NET Core’s built-in health check framework integrates with EF Core, making it suitable for monitoring database connectivity and enabling orchestrators to manage application instances effectively.

### Setup Steps

1. **Install the HealthChecks Package:**

   ```
   dotnet add package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
   ```

2. **Register the Health Check in Service Collection:**

   ```csharp
   builder.Services.AddHealthChecks()
       .AddDbContextCheck<ApplicationDbContext>("Database Health", failureStatus: HealthStatus.Unhealthy);
   ```

3. **Map Health Check Endpoint:**

   ```csharp
   app.MapHealthChecks("/health");
   ```

Once configured, you can access real-time health information for your database via the `/health` endpoint. A healthy connection returns HTTP 200; a failed connection returns HTTP 503 (Unhealthy).

## Conclusion

This article examined two straightforward yet powerful approaches to database connectivity verification using EF Core:

- Application-level checks with `CanConnectAsync()`
- Infrastructure-level health monitoring with the ASP.NET Core health checks framework

Using these techniques can improve your application's resilience and the ability for modern orchestrators (like Kubernetes) to manage your services intelligently. This is vital for handling cases of database unavailability and for maintaining overall system health.

For further resources, downloadable source code, or in-depth exploration of related patterns, refer to the [Code Maze GitHub repository](https://github.com/CodeMazeBlog/CodeMazeGuides/tree/main/dotnet-efcore/TestForValidDbConnection) and connected learning materials.

This post appeared first on "Code Maze Blog". [Read the entire article here](https://code-maze.com/efcore-testing-database-connectivity/)
