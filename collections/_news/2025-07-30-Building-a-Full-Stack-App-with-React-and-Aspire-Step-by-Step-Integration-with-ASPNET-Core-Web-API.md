---
external_url: https://devblogs.microsoft.com/dotnet/new-aspire-app-with-react/
title: 'Building a Full-Stack App with React and Aspire: Step-by-Step Integration with ASP.NET Core Web API'
author: Sayed Ibrahim Hashimi
feed_name: Microsoft .NET Blog
date: 2025-07-30 17:00:00 +00:00
tags:
- .NET
- .NET Aspire
- .NET CLI
- ASP.NET Core
- Aspire
- C#
- EF Core
- Full Stack Development
- JavaScript
- React
- REST API
- SQLite
- Vite
- VS Code
- Coding
- News
section_names:
- coding
primary_section: coding
---
In this guide, Sayed Ibrahim Hashimi demonstrates how to construct a full-stack TODO application using React and Aspire, coupling a modern front-end with an ASP.NET Core Web API and persistent SQLite storage.<!--excerpt_end-->

# Building a Full-Stack App with React and Aspire: Step-by-Step Integration with ASP.NET Core Web API

**Author:** Sayed Ibrahim Hashimi

## Overview

This guide walks through building a TODO application end-to-end using Aspire (a .NET orchestration tool) and React. The backend utilizes an ASP.NET Core Web API with SQLite database persistence, while the frontend is built with React (using Vite). The integration leverages the dotnet CLI, Aspire CLI, and C# Dev Kit in Visual Studio Code. The resulting app is fully orchestrated in Aspire, and can be deployed to any web host that supports ASP.NET Core.

---

## Prerequisites

- .NET 9.0
- Node.js
- [Visual Studio Code](https://code.visualstudio.com/) with [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
- Container runtime (Docker or similar)
- [Aspire CLI](https://www.nuget.org/packages/Aspire.Cli)

### Installing Aspire

- Use the provided PowerShell or bash script:
  - **Windows:**

      ```powershell
      iex "& { $(irm https://aspire.dev/install.ps1) }"
      ```

  - **Linux/macOS:**

      ```sh
      curl -sSL https://aspire.dev/install.sh | bash -s
      ```

- Starting with .NET 9, Aspire no longer requires a separate workload installation.

---

## Creating the Aspire Application

1. Open an empty folder in VS Code, create a subfolder `src` for source files.
2. Use VS Code's command palette (CTRL/CMD-SHIFT-P) and select “New Project”, then the Aspire Starter App template. Name the project “TodojsAspire”.
3. Optionally, you may use the terminal:

    ```sh
    dotnet new aspire-starter
    # or
    aspire new aspire-starter
    ```

4. Remove the default ASP.NET Core frontend (`TodojsAspire.Web`) since React will be used for the frontend. Update references and clean up the solution as shown in the guide.

---

## Configuring the Web API

- Add a `Todo` model to the `TodojsAspire.ApiService` project:

    ```csharp
    public class Todo {
      public int Id { get; set; }
      [Required]
      public string Title { get; set; } = default!;
      public bool IsComplete { get; set; } = false;
      [Required]
      public int Position { get; set; } = 0;
    }
    ```

- Scaffold API endpoints using the dotnet scaffold tool:

    ```sh
    dotnet tool install --global Microsoft.dotnet-scaffold
    dotnet scaffold
    # Follow the interactive steps for API, Minimal API, Model, SQLite provider etc.
    ```

- Add two additional endpoints for reordering tasks:
  - `MoveTaskUp`: swaps position with the prior task
  - `MoveTaskDown`: swaps position with next task

- Fix endpoint behaviors:
  - Ensure returned todos are sorted by `Position`.
  - Handle zero/missing `Position` on POST by assigning next available number.

---

## Configuring the Database

- Create an EF Core migration:

    ```sh
    dotnet ef migrations add TodoEndpointsInitialCreate
    ```

- Add SQLite database support to the AppHost with Aspire Community Toolkit:

    ```sh
    aspire add sqlite
    ```

- Update `AppHost.cs` to register the SQLite db and API service with appropriate references.
- Use `CommunityToolkit.Aspire.Microsoft.EntityFrameworkCore.Sqlite` NuGet package in ApiService project.
- Ensure the database is migrated at startup by updating `Program.cs` and removing obsolete connection strings.

---

## Verifying the API

- Use VS Code’s REST Client extension or another HTTP tool to test the API endpoints via a `.http` file.
- Sample HTTP requests include GET/POST/PUT/DELETE for todos, as well as move-up/move-down endpoints.
- Address issues if they arise (e.g., incorrect sorting, missing position values).

---

## Building the React Front-End

1. Scaffold the app with Vite:

    ```sh
    cd src
    npm create vite@latest todo-frontend -- --template react
    cd todo-frontend
    npm install
    npm run dev
    ```

2. Integrate the React frontend into AppHost using Aspire CLI and relevant extensions:

    ```sh
    aspire add nodejs
    aspire add ct-extensions
    # Add Vite app integration in AppHost.cs
    builder.AddViteApp(name: "todo-frontend", workingDirectory: "../todo-frontend")
           .WithReference(apiService)
           .WaitFor(apiService)
           .WithNpmPackageInstallation();
    ```

3. Configure `vite.config.js` to set up the development proxy linking React with the ASP.NET API backend; uses environment variables provided by AppHost.

### Components

- `TodoItem.jsx`: UI component for individual todos with buttons for delete, move up, move down.
- `TodoList.jsx`: Fetches and displays todo items, provides input for new tasks, and action handlers to call the API.
- `TodoList.css` and `index.css`: Styling for the UI.
- Update main entry points (`main.jsx`, `App.jsx`) and `index.html` for React integration.

---

## Using the Aspire Dashboard

- Start the AppHost project. The Aspire Dashboard will show the status of all components: the React frontend, ASP.NET Core API service, and SQLite database.
- The dashboard allows for log tracking, resource management, and integration with GitHub Copilot for debugging.

---

## Next Steps & Deployment

- The app runs locally under Aspire; it's ready for deployment to any ASP.NET Core supported host.
- Additional posts may cover production deployment.

---

## Recap

- Developed a full-stack TODO app using React for the frontend, ASP.NET Core Web API with Minimal APIs for the backend, and SQLite for persistence, all orchestrated by Aspire.
- Leveraged modern .NET tooling, including the dotnet CLI, Aspire CLI, C# Dev Kit, and community toolkits.
- Integrated everything into the Aspire Dashboard for monitoring, management, and potential AI-powered debugging.

## Feedback

- Aspire feedback/issues: [dotnet/aspire GitHub](https://github.com/dotnet/aspire)
- dotnet scaffold: [dotnet/Scaffolding GitHub](https://github.com/dotnet/scaffolding)
- C# Dev Kit: [microsoft/vscode-dotnettools GitHub](https://github.com/microsoft/vscode-dotnettools)
- Additional feedback can be left as blog comments.

---

**For full source code, visit [sayedihashimi/todojsaspire on GitHub](https://github.com/sayedihashimi/todojsaspire).**

---

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/new-aspire-app-with-react/)
