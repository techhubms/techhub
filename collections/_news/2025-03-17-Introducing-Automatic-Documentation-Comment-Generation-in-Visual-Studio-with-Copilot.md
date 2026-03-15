---
external_url: https://devblogs.microsoft.com/visualstudio/introducing-automatic-documentation-comment-generation-in-visual-studio/
title: Introducing Automatic Documentation Comment Generation in Visual Studio with Copilot
author: Sinem Akinci, Allie Barry
feed_name: Microsoft DevBlog
date: 2025-03-17 15:00:10 +00:00
tags:
- AI Generated Comments
- Automatic Doc Comments
- C#
- C++
- Copilot
- Developer Tools
- Docs
- Documentation
- Doxygen
- Editor Integration
- Preview Feature
- Productivity
- Settings
- Subscription
- VS
- XML Comments
- AI
- GitHub Copilot
- News
- .NET
section_names:
- ai
- dotnet
- github-copilot
primary_section: github-copilot
---
In this post, authors Sinem Akinci and Allie Barry introduce how GitHub Copilot now auto-generates documentation comments directly in the Visual Studio editor for C# and C++, providing developers with AI-powered assistance in their daily workflow.<!--excerpt_end-->

# Introducing Automatic Documentation Comment Generation in Visual Studio

**By Sinem Akinci and Allie Barry**

We are excited to announce that Copilot is now automatically integrated into Visual Studio’s editor to help you generate documentation comments (doc comments) for functions. This new feature aims to streamline your documentation process and enhance your coding experience.

To access these updates, you will need a GitHub Copilot subscription. If you don’t already have GitHub Copilot, you can sign up for free [here](https://github.com/settings/copilot).

## AI Doc Comment Generation

With automatic doc comment generation enabled in your settings, you can type the trigger for your chosen doc comment format (such as `///` in C#) and Copilot will automatically provide suggestions to fill out the function’s description. These suggestions are generated based on the function's content and typically include:

- A summary of the function
- Parameter information
- Return type descriptions (if available)

You can accept these suggestions simply by pressing the Tab key, just as you would when using regular Copilot code completions.

---

![Gif showcasing Copilot generating a summary in the comments](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAwAAAAEnAQMAAAAkebZqAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAMklEQVR4nO3BAQEAAACCIP+vbkhAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAwI0Bb8cAAVAQnJ8AAAAASUVORK5CYII=)
---

## How to Enable Doc Comments

To take advantage of this feature, automatic doc comment generation must be enabled in Visual Studio settings.

### For C++ Projects

You can configure the generated documentation comment styles by navigating to **Tools > Options**. Both XML and Doxygen style comments are supported:

![Tools > Option, Coding Style, General to enable for C++](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAA2cAAAJJAQMAAAAUVN3UAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAVUlEQVR4nO3BMQEAAADCoPVPbQsvoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA+Bn7XgABUZXTQAAAAABJRU5ErkJggg==)

### For C# Projects

Enable the feature via **Tools > Options** under the Advanced Comments settings for C#:

![Tools > Option, C#, Advanced, Comments for C#](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAzEAAAInAQMAAACSoEXxAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAATklEQVR4nO3BgQAAAADDoPlTn+AGVQEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADAN9/YAAFhJtQ3AAAAAElFTkSuQmCC)

## Feature Availability

- This feature is currently available in Visual Studio 17.14 Preview 2.
- Supported for both C# and C++ project files.

## Share Your Feedback

We encourage you to explore this new workflow and see how GitHub Copilot can simplify and transform your coding experience in Visual Studio.

We deeply appreciate the ongoing [feedback](https://developercommunity.microsoft.com/VisualStudio) from our users, which is invaluable as we continue to improve and innovate. Your input helps us make Visual Studio better with each update.

Happy coding!

---
*Original post on [Visual Studio Blog](https://devblogs.microsoft.com/visualstudio/introducing-automatic-documentation-comment-generation-in-visual-studio/) by Sinem Akinci and Allie Barry.*

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/introducing-automatic-documentation-comment-generation-in-visual-studio/)
