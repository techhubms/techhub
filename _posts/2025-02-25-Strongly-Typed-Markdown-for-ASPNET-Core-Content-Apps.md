---
layout: "post"
title: "Strongly-Typed Markdown for ASP.NET Core Content Apps"
description: "Khalid Abuhakmeh demonstrates how to process Markdown files with YAML frontmatter in ASP.NET Core applications. By leveraging Markdig and YamlDotNet, he shows how to create a strongly-typed C# object for easily parsing content and metadata, and rendering in Razor Pages. Includes complete code samples."
author: "Khalid Abuhakmeh"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://khalidabuhakmeh.com/strongly-typed-markdown-for-aspnet-core-content-apps"
viewing_mode: "external"
feed_name: "Khalid Abuhakmeh's Blog"
feed_url: "https://khalidabuhakmeh.com/feed.xml"
date: 2025-02-25 00:00:00 +00:00
permalink: "/2025-02-25-Strongly-Typed-Markdown-for-ASPNET-Core-Content-Apps.html"
categories: ["Coding"]
tags: ["ASP.NET", "ASP.NET Core", "C#", "Coding", "Content Management", "Markdig", "Markdown", "Metadata", "Posts", "Razor Pages", "Strongly Typed Objects", "YAML", "YamlDotNet"]
tags_normalized: ["aspdotnet", "aspdotnet core", "csharp", "coding", "content management", "markdig", "markdown", "metadata", "posts", "razor pages", "strongly typed objects", "yaml", "yamldotnet"]
---

In this post, Khalid Abuhakmeh guides developers through building strongly-typed content management solutions by parsing Markdown with YAML frontmatter into C# objects for ASP.NET Core apps.<!--excerpt_end-->

# Strongly-Typed Markdown for ASP.NET Core Content Apps

*By Khalid Abuhakmeh*

![Strongly-Typed Markdown for ASP.NET Core Content Apps](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/strongly-typed-markdown-aspnetcore-content-apps.jpg)

Photo by [Charles Gaudreault](https://unsplash.com/@dcdg)

Every development career has milestone moments. One many developers share is building a custom content management system (CMS). A widely-used approach for combining metadata and content is the Markdown format, which enables YAML frontmatter embedded with simple textual content. While YAML is flexible for metadata, using that data directly within ASP.NET Core applications can be challenging.

In this post, I’ll share a quick experiment: processing Markdown files and their YAML metadata into a strongly-typed C# object. This technique allows you to easily update content while accessing it programmatically in a type-safe way.

## The Magnificent Markdown

[Markdown](https://www.markdownguide.org/) is a widely-used, simple format, great for authoring documentation or CMS content. Here’s an example of a Markdown document that defines a person's profile:

```markdown
---
name: "Khalid Abuhakmeh"
profession: "Software Developer"
hobbies: ["video games", "movies", "boxing"]
---

## Summary

I am writing a little about myself here and this should appear in the page. Cool! Check me out at my [personal blog](https://khalidabuhakmeh.com).
```

The YAML frontmatter at the top defines three properties: `Name`, `Profession`, and `Hobbies`. To model this data in C#, you might create the following class:

```csharp
public class Asset {
    public string Name { get; set; } = "";
    public string Profession { get; set; } = "";
    public string[] Hobbies { get; set; } = [];
}
```

## Parsing Markdown Frontmatter into C#

The goal is to parse a Markdown file, extract its YAML metadata, and make both the content and its properties available as a strongly-typed object. For this, a `MarkdownObject<T>` class can be used, generically handling any frontmatter structure you define.

### Required Packages

Make sure you add the following NuGet packages:

```xml
<ItemGroup>
  <PackageReference Include="Markdig" Version="0.40.0" />
  <PackageReference Include="YamlDotNet" Version="16.3.0" />
</ItemGroup>
```

### Implementation of MarkdownObject

```csharp
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using Microsoft.AspNetCore.Html;
using YamlDotNet.Serialization;
using Md = Markdig.Markdown;

namespace SuperContent.Models;

public class MarkdownObject<T>
{
    private static readonly MarkdownPipeline MarkdownPipeline =
        new MarkdownPipelineBuilder()
            .UseYamlFrontMatter()
            .UseAdvancedExtensions()
            .Build();

    private static readonly IDeserializer Deserializer =
        new DeserializerBuilder()
            .WithYamlFormatter(new YamlFormatter())
            .WithCaseInsensitivePropertyMatching()
            .Build();

    public MarkdownObject(string content)
    {
        var doc = Md.Parse(content, MarkdownPipeline);
        FrontMatter = default;

        if (doc.Descendants<YamlFrontMatterBlock>().FirstOrDefault() is { } fm)
        {
            var yaml = fm.Lines.ToSlice();
            FrontMatter = Deserializer.Deserialize<T>(yaml.Text);

            // Remove the YAML frontmatter from the markdown document
            doc.Remove(fm);
        }

        // Convert the rest of the markdown content to HTML
        Html = new HtmlString(doc.ToHtml());
    }

    public T? FrontMatter { get; private set; }
    public IHtmlContent Html { get; private set; }
}
```

## Using MarkdownObject in a Razor Page

Assume all Markdown files are in a `Data` directory, one per profile. The Razor Page will use the file name as a `slug` parameter in the route. The model outputs both the strongly-typed metadata and the formatted HTML content:

```razor
@page "/profile/{slug}"
@model SuperContent.Pages.Profile

<div class="row">
  <div class="col-12">
    <h1>@Model.Asset.FrontMatter?.Name</h1>
  </div>
</div>

<div class="row">
  <div class="col-3">
    <dl>
      <dt>Profession</dt>
      <dd>@Model.Asset.FrontMatter?.Profession</dd>
      <dt>Hobbies</dt>
      <dd>
        <ul>
          @if (Model.Asset is { FrontMatter.Hobbies : { } hobbies }) {
              @foreach (var hobby in hobbies) {
                  <li>@hobby</li>
              }
          }
        </ul>
      </dd>
    </dl>
  </div>
  <div class="col-9">
    @Model.Asset.Html
  </div>
</div>
```

### The Page Model

```csharp
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperContent.Models;

namespace SuperContent.Pages;

public partial class Profile : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Slug { get; set; } = "";

    public MarkdownObject<Asset> Asset { get; set; } = null!;

    public IActionResult OnGet()
    {
        // Sanitize slug to prevent directory traversal and other attacks
        var sanitizedSlug = SlugRegex.Replace(Slug, "");
        var path = Path.Combine("ML", $"{sanitizedSlug}.md");

        if (System.IO.File.Exists(path))
        {
            var content = System.IO.File.ReadAllText($"Data/{sanitizedSlug}.md");
            Asset = new(content);
            return Page();
        }
        return NotFound();
    }

    [GeneratedRegex("[^a-zA-Z0-9_-]")]
    private static partial Regex SlugRegex { get; }
}

public class Asset
{
    public string Name { get; set; } = "";
    public string Profession { get; set; } = "";
    public string[] Hobbies { get; set; } = [];
}
```

With this setup, navigating to `/profile/Khalid` will display a profile page, rendering both the structured data and the Markdown content safely as HTML.

## Source and Further Exploration

You can find the full demo code in Khalid’s [GitHub repository](https://github.com/khalidabuhakmeh/SuperContent). Try it out and adapt it for your own content-driven ASP.NET Core apps.

---

*About the author*: Khalid Abuhakmeh is a developer advocate at JetBrains focusing on .NET technologies and tooling.

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/strongly-typed-markdown-for-aspnet-core-content-apps)
