---
layout: "post"
title: "Efficient .NET Documentation with Astro, Starlight, and MarkdownSnippets"
description: "Khalid Abuhakmeh details how to streamline .NET documentation by combining Astro's Starlight and the MarkdownSnippets tool. The process ensures code samples stay synchronized with technical documentation, making it easier for developers to write, extract, and update code examples directly from the codebase."
author: "Khalid Abuhakmeh"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://khalidabuhakmeh.com/great-dotnet-documentation-with-astro-starlight-and-markdownsnippets"
viewing_mode: "external"
feed_name: "Khalid Abuhakmeh's Blog"
feed_url: "https://khalidabuhakmeh.com/feed.xml"
date: 2024-12-31 00:00:00 +00:00
permalink: "/2024-12-31-Efficient-NET-Documentation-with-Astro-Starlight-and-MarkdownSnippets.html"
categories: ["Coding"]
tags: [".NET", ".NET CLI", "Astro", "C#", "Code Samples", "Coding", "Documentation Automation", "JavaScript", "MarkdownSnippets", "Posts", "Starlight", "Technical Documentation", "Tooling"]
tags_normalized: ["dotnet", "dotnet cli", "astro", "csharp", "code samples", "coding", "documentation automation", "javascript", "markdownsnippets", "posts", "starlight", "technical documentation", "tooling"]
---

Khalid Abuhakmeh explains how developers can use Astro's Starlight in combination with the MarkdownSnippets tool to maintain accurate and up-to-date .NET documentation with live code samples.<!--excerpt_end-->

# Efficient .NET Documentation with Astro, Starlight, and MarkdownSnippets

*Author: Khalid Abuhakmeh*

![Great .NET Documentation with Astro, Starlight, and MarkdownSnippets](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/astro-starlight-dotnet-markdownsnippets-documentation.jpg)

Photo by [Starlight](https://starlight.astro.build/)

## Introduction

Maintaining high-quality documentation that evolves alongside your codebase can be challenging for developers. This post introduces a workflow using [Astro's Starlight](https://starlight.astro.build/) for documentation and the [MarkdownSnippets](https://github.com/SimonCropp/MarkdownSnippets) tool to pull live code samples directly from source code, ensuring your docs stay synchronized with your .NET projects.

## Directory Structure and Tools

Start by structuring your repository for streamlined collaboration between code and documentation. At your project root, create these folders:

```
- docs
- src
```

- The `docs` directory: Holds documentation powered by Starlight/Astro.
- The `src` directory: Contains your .NET code (C# projects, etc).

Initialize the dotnet tool manifest so you can install development tools just for this repo:

```bash
 dotnet new tool-manifest
```

Install the MarkdownSnippets CLI tool:

```bash
 dotnet tool install MarkdownSnippets.Tool
```

Add a configuration file for MarkdownSnippets in your root folder as `mdsnippets.json`:

```json
{
  "$schema": "https://raw.githubusercontent.com/SimonCropp/MarkdownSnippets/refs/heads/main/schema.json",
  "Convention": "InPlaceOverwrite",
  "WriteHeader": false,
  "ReadOnly": false,
  "LinkFormat": "None",
  "OmitSnippetLinks": true
}
```

Your directory now includes:

```
- docs
- src
- .config
- mdsnippets.json
```

## Writing .NET Code Samples

Switch to the `src` directory and create a new .NET console application:

```bash
 dotnet new console -o App
```

Use MarkdownSnippets conventions to mark code blocks with special comments for extraction, e.g. in `Program.cs`:

```csharp
// begin-snippet: App:HelloWorld
// Program.cs
Console.WriteLine("Hello, Again!");
// end-snippet
```

You may define as many code sample snippets as needed for your documentation.

## Generating Starlight Documentation

Switch to the `docs` directory. Create a new site using Astro with the Starlight template (requires Node and Yarn):

```bash
 yarn create astro --template starlight
```

Follow the Astro wizard's prompts. Your directory will resemble:

```
- docs
- .astro
- .vscode
- .yarn
- src
- public
- astro.config.mjs
- package.json
...
(src/App and .NET files, mdsnippets.json)
```

Update your `package.json` scripts (in docs) so running `npm run dev` extracts new code snippets and rebuilds docs:

```json
{
  "type": "module",
  "version": "0.0.1",
  "scripts": {
    "mdsnippets": "cd .. && dotnet mdsnippets",
    "dev": "npm run mdsnippets && astro dev",
    "start": "npm run dev",
    "build": "npm run mdsnippets && astro build"
  },
  "dependencies": {
    "@astrojs/starlight": "^0.29.3",
    "astro": "^4.16.10",
    "sharp": "^0.32.5"
  }
}
```

## Writing Example Documentation

Find the `example.md` (e.g., under `docs/src/content/docs/guides/`), and write documentation using the snippet reference:

```markdown
---
title: Hello, World!
description: Creating your first .NET Console Application
---

## Getting Started

Your first .NET application will be a console application. Create a new project:
```bash title="Terminal"

dotnet new console -o HelloWorld && cd ./HelloWorld

```
Add the following code in `Program.cs`:

snippet: App:HelloWorld

Run the app:
```bash title="Terminal"

dotnet run

```
Congratulations!
```

After running `npm run dev`, MarkdownSnippets replaces `snippet: App:HelloWorld` with the latest actual code:

```markdown
<!-- snippet: App:HelloWorld -->
```cs

// Program.cs
Console.WriteLine("Hello, Again!");

```
<!-- endSnippet -->
```

You can edit your source code and re-run the docs build to keep everything up to date.

## Conclusion & Recommendations

Combining MarkdownSnippets and Starlight provides an efficient way to generate code-heavy documentation. Establish naming conventions for code samples for easier management, and ensure code snippets are always valid, compiled, and consistent with your codebase.

As always, thanks for reading, and cheers.

---

![Khalid Abuhakmeh's Picture](/assets/images/authorimage.jpg)

## About the Author

Khalid Abuhakmeh is a developer advocate at JetBrains focusing on .NET technologies and tooling.

---

#### Read Next

- [Alpine.Js Polling ASP.NET Core APIs For Updates](/alpinejs-polling-aspnet-core-apis-for-updates)
- [Writing a String Numeric Comparer with .NET 9](/writing-a-string-numeric-comparer-with-dotnet-9)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/great-dotnet-documentation-with-astro-starlight-and-markdownsnippets)
