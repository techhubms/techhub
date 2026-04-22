---
author: Rick Strahl
external_url: https://weblog.west-wind.com/posts/2026/Apr/20/Revisiting-C-Scripting-with-the-WestwindScripting-Templating-Library-Part-1
date: 2026-04-20 20:35:37 +00:00
tags:
- .NET
- Assembly References
- Blogs
- C#
- Caching
- Code Generation
- Dynamic Code Execution
- Error Handling
- ExecuteScript
- ExecuteScriptFile
- Handlebars Syntax
- Layout Pages
- Microsoft.CodeAnalysis.CSharp
- Namespace References
- NuGet
- Partials
- Razor Comparison
- Roslyn
- Runtime Compilation
- ScriptParser
- Sections
- Template Engine
- Templating
- Westwind.Scripting
title: Revisiting C# Scripting with the Westwind.Scripting Templating Library, Part 1
primary_section: dotnet
feed_name: Rick Strahl's Blog
section_names:
- dotnet
---

Rick Strahl walks through Westwind.Scripting’s ScriptParser: a C# (raw .NET) templating engine that parses Handlebars-like templates into generated C# code, compiles them with Roslyn at runtime, caches the results, and (new in this update) supports file-based layout pages and sections.<!--excerpt_end-->

![Scripting banner](https://weblog.west-wind.com/images/2026/Revisiting-C-Scripting-with-the-Westwind-Scripting-Templating-Library,-Part-1/Scripting-Banner.jpg)

> This is a two part series that discusses the Westwind.Scripting Template library
> - Part 1: An introduction to Template Scripting and how it works (this post)
> - Part 2: Real world integration for Local Rendering and Web Site Generation (coming soon)

## What this post is about

Rick Strahl recently updated his C# template scripting engine built around the `ScriptParser` class in [Westwind.Scripting](https://github.com/RickStrahl/Westwind.Scripting), adding support for **Layout Pages** and **Sections**.

`ScriptParser` is a small, self-contained template rendering engine that merges a text document with embedded expressions and code. It uses a Handlebars-like syntax but lets you embed **raw C#** expressions and code blocks. Templates are parsed into C# source, compiled to IL with Roslyn, then executed at runtime.

The library has two major pieces:

- **C# Script Execution Engine**
  - Compiles and runs C# from source at runtime.
  - Can execute snippets, complete methods, or full class definitions.
  - Generates wrapper code, compiles via Roslyn, loads dependencies, caches compiled assemblies, and executes.
  - Background article: [Runtime C# Code Compilation Revisited for Roslyn](https://weblog.west-wind.com/posts/2022/Jun/07/Runtime-C-Code-Compilation-Revisited-for-Roslyn)

- **Template Scripting Engine (ScriptParser)**
  - Parses a Handlebars-style template into generated C#.
  - Compiles and executes that generated code.
  - Templates can run from strings or files.
  - Supports partials, and now supports **layout pages and sections** (file templates only).

## ScriptParser template features (overview)

- Handlebars-like syntax using raw C# code
- String-based template execution
- File-based script execution
- Partials loaded from disk (string and file scripts)
- Layout and Section directives (file scripts only)
- Expressions and code blocks use raw C# syntax
- Supports latest C# syntax (based on Roslyn version)
- No “pseudo” scripting language constructs—control flow is just C#

Supported tag forms:

- `{{ C# expression }}`
- `{{% C# code block }}`
- `{{: html encoded Expression }}`
- `{{! raw expression }}`
- `{{@ commented block @}}`

Code blocks can be:

- Self-contained code blocks
- Any structured C# statement (`for`, `while`, `if`, `using`, etc.) split across multiple code blocks with literal text between them

## Why build this instead of Razor?

Strahl explains that Razor is excellent in ASP.NET web apps with tooling support, but he ran into pain points using Razor in non-web/desktop or non-ASP.NET contexts:

- Running Razor outside ASP.NET adds complexity
- HTML formatting oddities for plain text generation
- ASP.NET runtime requirements
- Risk/concern about runtime compilation availability
- Poor syntax highlighting outside .NET-focused editors
- Constant churn in internals

He wanted a self-contained, embeddable solution that is stable and can keep up with new C# by updating Roslyn dependencies.

## Getting started

### Install

```ps
 dotnet add package Westwind.Scripting
```

This includes Westwind.Scripting and the Roslyn compiler dependencies.

> #### Roslyn runtime dependency
> `Microsoft.CodeAnalysis.CSharp` is not part of the distributed .NET runtime, so you must ship Roslyn DLLs (adds several MB to your distribution).

## Template basics: expressions and code blocks

The template logic is made of:

- **Expressions**: `{{ expr }}`

```html
<i>{{ DateTime.Now.ToString("d") }}</i>
```

- **Code blocks**: `{{% C# code }}`

```html
{{% for(int x; x<10; x++ { }}
  <div>{{ x }}. Hello World</div>
{{% } }}

{{% // declare variables
   var album = new Album() { Title = "Album Title", Band = "Rocka Rolla" };
   var message = "Rock on Garth.";
}}

<!-- use declared variables -->
<div>{{ album.title }} by {{ album.band }}</div>
<div>{{ message }}</div>
```

### How templates execute

Templates are compiled into a generated class method. Literal text becomes `writer.Write("...")`, while expressions and code blocks are injected into the method body.

- Literal text → emitted as string literals
- Expressions → written via `writer.Write(<expression>)` (auto `ToString()` for non-strings)
- Code blocks → emitted as raw C# (no `writer.Write()` unless you call it yourself)

## Executing a script string (example)

```csharp
var model = new TestModel {Name = "rick", DateTime = DateTime.Now.AddDays(-10)};

string script = @"
Hello World. Date is: {{ Model.DateTime.ToString(""d"") }}!
{{% for(int x=1; x<3; x++) { }}
  {{ x }}. Hello World {{Model.Name}}
{{% } }}

And we're done with this!
";

var scriptParser = new ScriptParser();

// add dependencies
scriptParser.AddAssembly(typeof(ScriptParserTests));
scriptParser.AddNamespace("Westwind.Scripting.Test");

var result = scriptParser.ExecuteScript(script, model);

Console.WriteLine(result);
Console.WriteLine(scriptParser.ScriptEngine.GeneratedClassCodeWithLineNumbers);
Console.WriteLine(scriptParser.ErrorType);
```

## New feature: layout pages and sections (file scripts)

Partials (`Script.RenderPartial(...)`) can be repetitive for larger sites or documentation projects. The new release adds:

- **Layout Pages**: master page referenced by content pages
- **Sections**: placeholders defined in the layout and filled by content pages

### Running a file template

Layout pages only work with files on disk. You typically set a base path so `/` or `~/` can resolve related scripts.

```csharp
[TestMethod]
public void LayoutFileScriptTest()
{
    var scriptParser = new ScriptParser();

    // auto-encode {{ expr }}
    scriptParser.ScriptingDelimiters.HtmlEncodeExpressionsByDefault = true;

    var result = scriptParser.ExecuteScriptFile(
        "website/Views/Detail.html",
        new TestModel { Name = "Rick" },
        basePath: "website/Views/");

    Console.WriteLine(result);
    Console.WriteLine(scriptParser.ScriptEngine.GeneratedClassCodeWithLineNumbers);

    Assert.IsNotNull(result, scriptParser.ErrorMessage);
}
```

Notes from the post:

- `basePath` is used to resolve **Layout** and `RenderPartial()` paths.
- It does not automatically resolve other document URLs.

### Content page example (concept)

A content page sets the layout via code:

- `Script.Layout = "Layout.html";`

It can also define sections:

```html
{{ Script.Section("Headers") }}
  <!-- header content, css, etc -->
{{ Script.EndSection("Headers") }}
```

### Layout page example (concept)

The layout pulls sections and content:

```html
{{ Script.RenderSection("StartDocument") }}
{{ Script.RenderSection("Headers") }}

{{ Script.RenderContent() }}

{{ Script.RenderSection("Scripts") }}
```

#### Important behavior: content + layout merge

- Layout and content pages are **merged into a single generated method** at parse time.
- The layout renders first; `{{ Script.RenderContent() }}` is where the content is inserted.
- If the layout changes, content pages that use it need rebuilding (because the merged code changes).

## Real-world uses described

- **Markdown Monster snippet expansion** and automation
- **Documentation Monster** HTML topic generation and site generation (2.5k+ topics in ~10 seconds in one project)
- Bulk email generation / mail merge
- Code generation (replacing old templating approaches)

## References and dependency loading

Because templates are compiled, all assemblies/namespaces used by executed code must be resolvable by the compiler.

The engine adds common runtime references by default, but if you use external types (models, utility libraries), you must add references explicitly.

Helpers described:

- `AddCommonDefaultReferences()`
- `AddLoadedReferences()` (only picks up assemblies actually loaded)
- `AddAssembly()` / `AddAssemblies()`
- Optionally add a full set of runtime meta references (bigger footprint)

Example:

```csharp
var scriptParser = new ScriptParser();

// explicit references
scriptParser.AddAssembly(typeof(ScriptParserTests));
scriptParser.AddAssembly("./Westwind.Utilities.dll");

// forward loaded references from host (be careful)
scriptParser.ScriptEngine.AddLoadedReferences();

// meta reference assemblies (large)
scriptParser.AddAssemblies(
    metaAssemblies: Basic.Reference.Assemblies.Net100.References.All.ToArray());

// namespaces
scriptParser.AddNamespace("Westwind.Scripting.Test");
```

## Generated code inspection

You can inspect generated code:

- `script.ScriptEngine.GeneratedClassCode`
- `script.ScriptEngine.GeneratedClassCodeWithLineNumbers`

The post shows a snippet of generated source where literal text becomes `writer.Write(...)` and expressions/code blocks are embedded in the method.

## Caching

- Templates compile into assemblies.
- Compiled output is cached (static collection) based on generated code.
- First execution is slower (loading Roslyn and compiling).
- Re-running the same template is much faster (cached assembly).
- Assemblies generally can’t be unloaded by default; you can provide a custom `AssemblyLoadContext` if you need unload behavior.

## Error handling

- Errors are exposed via `ScriptEngine.ErrorMessage`.
- Error types include:
  - Compilation
  - Runtime
- Compilation errors can include line numbers referencing generated code (useful for mapping back to templates).

## Summary

Part 1 explains how ScriptParser works (raw C# in Handlebars-style tags), how it compiles templates into code via Roslyn, how to manage references/namespaces, and how the new **layout pages and sections** feature works for file-based templating. Part 2 will cover a real-world integration scenario for HTML output generation.

## Resources

- [Westwind.Scripting Library on GitHub](https://github.com/RickStrahl/Westwind.Scripting)
- [Westwind.Scripting Templating Features](https://github.com/RickStrahl/Westwind.Scripting/blob/master/ScriptAndTemplates.md)
- [Previous Post: Runtime Compilation with Roslyn and Building Westwind.Scripting](https://weblog.west-wind.com/posts/2022/Jun/07/Runtime-C-Code-Compilation-Revisited-for-Roslyn)


[Read the entire article](https://weblog.west-wind.com/posts/2026/Apr/20/Revisiting-C-Scripting-with-the-WestwindScripting-Templating-Library-Part-1)

