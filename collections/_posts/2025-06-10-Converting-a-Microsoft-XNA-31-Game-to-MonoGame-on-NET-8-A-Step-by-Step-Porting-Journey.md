---
layout: "post"
title: "Converting a Microsoft XNA 3.1 Game to MonoGame on .NET 8: A Step-by-Step Porting Journey"
description: "Andrew Lock shares his experience updating a 15-year-old Microsoft XNA 3.1 game to run on MonoGame with .NET 8. The article covers practical porting steps, compatibility issues between XNA versions, solutions for content pipeline challenges, and adjustments for modern MonoGame development."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/converting-an-xna-game-to-monogame/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-06-10 09:00:00 +00:00
permalink: "/posts/2025-06-10-Converting-a-Microsoft-XNA-31-Game-to-MonoGame-on-NET-8-A-Step-by-Step-Porting-Journey.html"
categories: ["Coding"]
tags: [".NET 8", "Coding", "Content Pipeline", "Game Development", "Microsoft XNA", "MonoGame", "Porting", "Posts", "RasterizerState", "SoundEffect", "SpriteFont", "XNA 3.1"]
tags_normalized: ["dotnet 8", "coding", "content pipeline", "game development", "microsoft xna", "monogame", "porting", "posts", "rasterizerstate", "soundeffect", "spritefont", "xna 3dot1"]
---

In this article, Andrew Lock details the process of porting a classic Microsoft XNA 3.1 game from 2009 to MonoGame running on .NET 8. He covers technical steps, challenges, and solutions encountered during the migration.<!--excerpt_end-->

# Converting a Microsoft XNA 3.1 Game to MonoGame on .NET 8: A Step-by-Step Porting Journey

**Author:** Andrew Lock

---

## Introduction

This article explores the process of porting a Microsoft XNA Framework 3.1 game—originally written in 2009—to MonoGame on .NET 8. Andrew Lock shares his hands-on experience, documenting the steps, issues, and solutions involved in bringing an old game back to life with modern tooling.

## Background: Why Port an XNA Game?

Andrew was inspired by a Merge Conflict podcast episode discussing MonoGame, which implements Microsoft XNA APIs for contemporary platforms. Reminiscing about his own XNA-based clone of the game “Trash” (itself inspired by Nintendo's Dr. Mario), Andrew wondered how feasible it would be to run his old codebase on modern frameworks. The original code targeted XNA 3.1 and .NET Framework 3.5, while MonoGame is built around XNA 4.0 and latest versions of .NET.

## Approach Overview

The upgrade plan involved:

1. Creating a MonoGame sample application.
2. Replacing sample code with original project files.
3. Iteratively resolving build and runtime issues.

The focus was to make the game run on current technology, not to refine or distribute it.

## Detailed Porting Steps

### 1. Setting Up the New Project

- Used the updated `.slnx` solution format for .NET projects.
- Created a cross-platform MonoGame desktop app using `mgdesktopgl` template.
- Added the project to the solution and verified that the default app (the classic Cornflower Blue screen) runs.

```bash
# Create and convert solution

dotnet new sln
dotnet sln migrate
rm *.sln

# Create MonoGame project in Trash subfolder

dotnet new mgdesktopgl --output Trash
dotnet sln add ./Trash/
```

### 2. Transferring Game Files

- Copied all C# code files and content (such as .wav and .png assets) except:
    - The legacy `.csproj` project file (used MonoGame’s instead).
    - The old `AssemblyInfo.cs` (attributes handled by SDK now).

#### Compiling: Version Issues

- Only one initial build error: Use of `GraphicsDevice.RenderState` not found in MonoGame/XNA 4.0.
    - Solution: Replace `RenderState` with `RasterizerState` as per XNA 4.0 changes.
    - Example fix:

      ```csharp
      // XNA 3.1:
      bool defaultUseScissorTest = spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable;
      // MonoGame/XNA 4.0:
      bool defaultUseScissorTest = spriteBatch.GraphicsDevice.RasterizerState.ScissorTestEnable;
      ```

- MonoGame maintains impressive compatibility with XNA APIs, enabling mostly seamless compilation.

- Notable limitation: MonoGame omits some XNA namespaces (like `.Storage` and `.Net`), for cross-platform reasons.

### 3. Content Pipeline Migration

#### Handling Audio Content (XACT to MonoGame)

- On first run, the game crashed due to missing `.xgs` (XACT Game Studio) files.
- Instead of porting XACT directly (unsupported by MonoGame), switched to MonoGame’s sound APIs:
    - Used `SoundEffect`, `SoundEffectInstance`, and `Song` types for audio.
    - Relied on the MonoGame Content Builder (MGCB) to import and process assets.

- Ran the MGCB content editor:

   ```bash
   dotnet mgcb-editor
   ```

- Most assets (e.g., .wav, images) processed fine, but .xap (XACT Audio Project) was unsupported.
- Examined .xap content manually, rewrote usage to work with direct sound files.

    ```csharp
    SoundEffect effect = Content.Load<SoundEffect>("file.wav");
    effect.Play();
    ```

### 4. Solving Font Issues

- Build failed on a missing font: `Narkisim`, referenced in a SpriteFont.
- Discovered `Narkisim` is part of the Windows Hebrew language pack.
- Solution: Installed the required font using Windows Update, restoring successful content pipeline builds.

### 5. Handling RasterizerState and Scissor Test Changes

- The next runtime error: attempting to set `ScissorTestEnable` on the default `RasterizerState` object, which is immutable in XNA 4.0/MonoGame.
- Correction: Create a new `RasterizerState` object and assign it to `GraphicsDevice`.

    ```csharp
    spriteBatch.GraphicsDevice.RasterizerState = new RasterizerState { ScissorTestEnable = true };
    ```

- Additional fix: Must set the `RasterizerState` *before* calling `SpriteBatch.Begin()`, or better yet, pass it as a parameter.

    ```csharp
    spriteBatch.Begin(rasterizerState: new RasterizerState { ScissorTestEnable = true });
    ```

### 6. Verifying the Final Result

- With all above issues resolved, the game launched and functioned correctly in MonoGame under .NET 8, including proper scissor testing and working gameplay.
- The source was made available on GitHub, though Andrew notes it’s not actively maintained or polished.

## Summary and Takeaways

Porting an old Microsoft XNA Framework game (targeting .NET Framework 3.5 and XNA 3.1) to MonoGame running on .NET 8 proved surprisingly straightforward. Key migration challenges revolved around breaking changes in the XNA API, content pipeline differences, sound file handling, and font requirements. MonoGame’s strong compatibility with the XNA structure enabled most original code to work with limited updates. The project highlights the durability and adaptability of Microsoft’s developer platforms over the years.

## Resources

- [MonoGame](https://monogame.net/)
- [Microsoft XNA Framework](https://en.wikipedia.org/wiki/Microsoft_XNA)
- [GitHub repository for "Trash" game by Andrew Lock](https://github.com/andrewlock/Trash)
- [Merge Conflict Podcast Episode #457](https://www.mergeconflict.fm/457)
- [MonoGame Content Pipeline Documentation](https://docs.monogame.net/articles/getting_to_know/howto/content_pipeline/)

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/converting-an-xna-game-to-monogame/)
