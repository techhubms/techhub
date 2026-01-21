---
external_url: https://andrewlock.net/creating-your-first-sample-game-with-monogame/
title: Creating your first sample game with MonoGame
author: Andrew Lock
viewing_mode: external
feed_name: Andrew Lock's Blog
date: 2025-06-03 09:00:00 +00:00
tags:
- .NET
- .NET 8
- .NET CLI
- C#
- Content Pipeline
- Cross Platform
- DesktopGL
- Game Development
- JetBrains Rider
- MGCB
- Microsoft XNA
- MonoGame
- MonoGame Templates
- MSBuild
- NuGet
- OpenGL
- Project Setup
- Sample Application
- VS
- XNA 4.0 API
section_names:
- coding
---
Andrew Lock offers a step-by-step introduction to MonoGame, explaining its XNA roots, .NET-based setup, essential tools, and sample project structure for modern cross-platform game development.<!--excerpt_end-->

# Creating your first sample game with MonoGame

*By Andrew Lock*

## Introduction

This post introduces the [MonoGame](https://monogame.net/) framework—an open-source implementation of the Microsoft XNA 4.0 API—targeted at .NET developers aiming to build cross-platform games. You'll discover the history of XNA and MonoGame, setup requirements, and how to scaffold a sample project.

## Background: A Brief History of Microsoft XNA and MonoGame

- **Microsoft XNA**: Released in 2006 to simplify game development on Windows and Xbox 360, providing lower-level building blocks for games, content pipelines, and cross-platform possibilities.
- **Open-Source Evolution**: Community-led projects like Mono.XNA and SilverSprite emerged to extend XNA across platforms. XNA.Touch and later MonoGame further broadened reach to mobile and consoles.
- **MonoGame**: Launched in 2011, MonoGame has evolved to target Windows, Mac, Linux, current-generation consoles, and leverages OpenGL/DirectX.

## Getting Started with MonoGame

### Prerequisites

- .NET 8 SDK or newer
- Install MonoGame templates via CLI:

  ```bash
  dotnet new install MonoGame.Templates.CSharp
  ```

- Templates include options for 2D/3D games, various platforms (Windows, Linux, Mac, Android, iOS), and content pipeline extensions.

### IDE Support

- Works with Visual Studio, VS Code, and JetBrains Rider

## Project Setup: Sample Game

1. **Create Solution with new .slnx Format**

   ```bash
   dotnet new sln
   dotnet sln migrate
   rm *.sln
   ```

2. **Add MonoGame Cross-Platform Project**

   ```bash
   dotnet new mgdesktopgl --output MyGame
   dotnet sln add ./MyGame/
   ```

3. **Examine Project Structure**
   - Target is `.NET 8`
   - NuGet references:
     - `MonoGame.Framework.DesktopGL`: Main runtime for DesktopGL
     - `MonoGame.Content.Builder.Task`: MSBuild integration for processing game assets
   - Embedded resources: Icons, graphics
   - Custom MSBuild target `RestoreDotnetTools` ensures MonoGame tools (MGCB) are available

4. **MonoGame Tools in dotnet-tools.json**
   - `dotnet-mgcb` (CLI content builder)
   - `dotnet-mgcb-editor` (graphical content pipeline editor)
   - Platform-specific editor binaries

## Core App Code: Minimal MonoGame Example

- *Program.cs* runs the main game loop:

  ```csharp
  using var game = new MyGame.Game1();
  game.Run();
  ```

- *Game1.cs* sets up graphics device, sprite batch, and core override methods:
  - `Initialize()`, `LoadContent()`, `Update()`, `Draw()`
  - Responds to Escape key or Back button to exit
  - Fills the screen with Cornflower Blue as the default background

## Running the Sample

- Build and run with `dotnet run` or F5 in your IDE
- The app displays a blue screen—basic, but serves as a clean project foundation

![The default MonoGame sample 'game'](/content/images/2025/monogame_00.png)

## Summary

- MonoGame enables .NET developers to build cross-platform games using modern tools.
- Setup requires installing templates, creating new projects, and integrating essential NuGet packages.
- The sample demonstrates the absolute basics—serving as a launchpad for more advanced game features, custom content, and porting older XNA projects.

---

For more in-depth tutorials, visit the [MonoGame documentation](https://docs.monogame.net/articles/getting_started/index.html).

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/creating-your-first-sample-game-with-monogame/)
