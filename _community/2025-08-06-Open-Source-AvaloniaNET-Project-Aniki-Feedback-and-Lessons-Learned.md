---
layout: "post"
title: "Open Source Avalonia/.NET Project 'Aniki': Feedback and Lessons Learned"
description: "This community discussion centers on the release of 'Aniki', a cross-platform MyAnimeList client built with Avalonia and .NET, designed to streamline anime tracking and management. The conversation includes valuable technical feedback, recommendations for improving C# code quality, and important considerations for open source project branding."
author: "Loiuy123_"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/csharp/comments/1mji0tp/i_just_released_my_first_real_open_source_project/"
viewing_mode: "external"
feed_name: "Reddit CSharp"
feed_url: "https://www.reddit.com/r/csharp/.rss"
date: 2025-08-06 21:57:29 +00:00
permalink: "/2025-08-06-Open-Source-AvaloniaNET-Project-Aniki-Feedback-and-Lessons-Learned.html"
categories: ["Coding"]
tags: [".NET", "Application Development", "Avalonia", "BASE64", "Branding", "C#", "Code Quality", "Coding", "Community", "Community Feedback", "Cross Platform", "Encryption", "File Scoped Namespaces", "GitHub", "Global Usings", "Implicit Usings", "MyAnimeList", "Nullable Reference Types", "Open Source", "Regex Source Generator", "TreatWarningsAsErrors", "UI Framework"]
tags_normalized: ["net", "application development", "avalonia", "base64", "branding", "c", "code quality", "coding", "community", "community feedback", "cross platform", "encryption", "file scoped namespaces", "github", "global usings", "implicit usings", "myanimelist", "nullable reference types", "open source", "regex source generator", "treatwarningsaserrors", "ui framework"]
---

Loiuy123_ shares insights and receives technical feedback on their first significant open source project, Aniki, a MyAnimeList client built with Avalonia and .NET.<!--excerpt_end-->

# Open Source Avalonia/.NET Project 'Aniki': Feedback and Lessons Learned

**Author:** Loiuy123_

## Project Overview

Loiuy123_ introduces **Aniki**, an open source desktop client for MyAnimeList (MAL) users, built using Avalonia (a cross-platform UI framework) and .NET. Aniki enables users to manage their MAL accounts, browse content, and even watch anime with integrated torrent search from Nyaa. The author emphasizes a commitment to ongoing development and welcomes community feedback on both coding practices and usability.

GitHub repo: [TrueTheos/Aniki](https://github.com/TrueTheos/Aniki)

## Community Feedback and Recommendations

### Code Quality and Modern C# Practices

- **Nullable Reference Types:** Strongly recommended to guard against null-related bugs. Enable this feature to improve code correctness and reliability. [Learn more](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references)
- **File Scoped Namespaces:** Adopt C# 10's file-scoped namespaces to reduce nesting and improve code readability. [Details here](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-10.0/file-scoped-namespaces)
- **Treat Warnings as Errors:** Set `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` in your `.csproj` file to ensure all warnings are addressed diligently and not ignored or suppressed. This encourages thorough code quality.
- **Regex Source Generators:** For static or unchanging regular expressions, leverage [regex source generators](https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-source-generators) to optimize and validate regex usage at compile time.
- **Global and Implicit Usings:** Reduce repetitive code and improve project organization by utilizing global and implicit usings. [Overview](https://devblogs.microsoft.com/dotnet/welcome-to-csharp-10/#global-and-implicit-usings)

### Project Configuration

- Ensure `Resources\CLIENTID.txt` exists in source control, or improve documentation on how to create this file for local/self-hosted setups referencing the note in the Readme's `Self-Hosting` section.

### Branding and Usability

- **Branding:** Consider the name "Aniki" carefully, as it may create confusion with "Anki", a well-known flashcard tool, particularly in overlapping communities (anime/Japanese learners). Exploring alternative names may help your project stand out.
- **Screenshots & Demos:** Add visual aids and demos to your repository's README to improve usability and attract more users.

### Security Considerations

- **Sensitive Data Storage:** Avoid storing sensitive data, such as tokens, using only base64 encoding. Implement proper encryption practices for user safety.

### Encouragement and Community Spirit

- Community members congratulate Loiuy123_ for successfully bringing a useful project to life and seeking improvement. These supportive interactions foster healthy open source collaboration.

## Example Security Concern

```csharp
private static string EncryptData(string data) {
    byte[] bytes = Encoding.UTF8.GetBytes(data);
    return Convert.ToBase64String(bytes);
}
```

> **Note:** Base64 is not encryption. Do not use this approach for storing sensitive data like access tokens.

## Author's Responses and Action Items

- Plans to adopt nullable reference types and enforce warnings-as-errors
- Intends to standardize use of file-scoped namespaces across the project
- Will investigate the regex source generator feature for improved pattern handling
- Committed to improving documentation and adding screenshots
- Acknowledges branding feedback and is open to renaming if needed

---

For more details or to contribute feedback/code, visit the GitHub repo [here](https://github.com/TrueTheos/Aniki).

This post appeared first on "Reddit CSharp". [Read the entire article here](https://www.reddit.com/r/csharp/comments/1mji0tp/i_just_released_my_first_real_open_source_project/)
