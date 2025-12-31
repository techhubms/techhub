---
layout: "post"
title: "Maximize GitHub Copilot Free Tier: Using Inline Completions for More Answers"
description: "Jesse Houwing explains how to get more out of GitHub Copilot's free tier by using inline completions as a workaround when you exhaust your chat message quota. The post includes practical prompts and examples, benefitting users of VS Code, NeoVim, and editors without dedicated Chat features."
author: "Jesse Houwing"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://jessehouwing.net/github-copilot-free-use-inline-completions-for-more-answers/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: "https://jessehouwing.net/rss/"
date: 2025-02-10 16:03:31 +00:00
permalink: "/blogs/2025-02-10-Maximize-GitHub-Copilot-Free-Tier-Using-Inline-Completions-for-More-Answers.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "Chat Messages", "Code Completions", "Coding Assistance", "Copilot Usage", "Developer Workflow", "Editor Plugins", "Free Tier", "GitHub", "GitHub Copilot", "Inline Completions", "Neovim", "Posts", "Productivity Tips", "VS Code"]
tags_normalized: ["ai", "chat messages", "code completions", "coding assistance", "copilot usage", "developer workflow", "editor plugins", "free tier", "github", "github copilot", "inline completions", "neovim", "posts", "productivity tips", "vs code"]
---

Jesse Houwing presents practical techniques for maximizing the GitHub Copilot free tier by leveraging inline completions, especially for users facing chat message limits or working in editors without chat integration.<!--excerpt_end-->

# Maximize GitHub Copilot Free Tier: Using Inline Completions for More Answers

*By Jesse Houwing*

GitHub has recently introduced a limited free tier for GitHub Copilot, allowing users 2000 inline completions and 50 chat messages. While the free offering provides valuable opportunities to explore Copilot’s capabilities, many users have noted that 50 chat messages is a rather limited number, especially if you rely on Copilot Chat for coding queries and guidance.

However, there’s an effective workaround: you can use your completion quota for many of the tasks typically handled in Chat. This is especially valuable for NeoVim users and editors like Qt Explorer and XCode, which do not natively support Copilot Chat but do allow inline suggestions. Inline completions are also significantly faster than chat-based responses.

## Using Inline Completions for Q&A

You can prompt Copilot by writing comments or small cues in your code. For instance:

```csharp
// Q: Explain the key usage scenarios for the Singleton pattern
// A:
```

Often, simply typing `// A:` will provide a helpful response. If the answer seems too brief, add more context:

```csharp
// Q: Explain the key usage scenarios for the Singleton pattern. A detailed description
// A: The Singleton pattern is used when we want to ensure that a class has only one instance and provide a global point of access to that instance. Some key usage scenarios for the Singleton pattern include:
```

To further expand the answer, continue typing, such as `// -` on the next line or open the completions panel (`ctrl-enter`), which will yield an in-depth breakdown:

```csharp
// 1. Global Access: Provides a single point of access to the class instance.
// 2. Resource Management: Manages shared resources (e.g. database connections, file systems).
// 3. Caching: Implements in-memory data or object caches.
// 4. Logging: Centralizes logging functionality through a single logger instance.
// 5. Configuration Settings: Manages application-wide settings consistently.
// 6. State Management: Maintains shared state or context.
// 7. Thread Safety: Provides safe concurrent access to shared resources.
```

## Suggesting Test Cases

Copilot can help brainstorm additional test cases. For example:

```csharp
// Suggest additional test cases to add to this test fixture, signatures only:
//
```

Typical resulting suggestions might include:

```csharp
// public void GetById_WhenFlightDoesNotExist_ReturnsNull()
// public void Add_WhenFlightIsNull_DoesNotAddFlightToRepository()
// public void Add_WhenFlightHasNullFlightNumber_ThrowsException()
// public void Add_WhenFlightHasEmptyFlightNumber_ThrowsException()
// public void Add_WhenFlightHasNullOrigin_ThrowsException()
// public void Add_WhenFlightHasEmptyOrigin_ThrowsException()
// ...
```

This is an efficient way to enhance your test coverage based on Copilot’s understanding of your code.

## Suggesting Code Improvements

You can also ask Copilot for security or improvement suggestions:

```csharp
// To improve the security of this REST API I can:
//
```

Copilot can then suggest:

```csharp
// add an authorization filter to this controller
// require authentication for all actions using the [Authorize] attribute
```

Enhance the suggestions further by adding cues such as `// and`, or more specific requests: `// something else I could do is`.

## Conclusion

If you’ve reached your chat limit in the GitHub Copilot free tier, inline completions are an excellent fallback and can essentially serve as lightweight chat, especially in editors that don’t support Chat natively. With these techniques, you may be able to extend Copilot’s usefulness and productivity value considerably. And if you find yourself regularly surpassing these limits, you may want to consider a subscription.

This approach is particularly beneficial to users of NeoVim, Qt Explorer, XCode, and similar environments that support inline completions but not Copilot Chat.

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/github-copilot-free-use-inline-completions-for-more-answers/)
