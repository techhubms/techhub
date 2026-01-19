---
external_url: https://devblogs.microsoft.com/visualstudio/next-edit-suggestions-available-in-visual-studio-github-copilot/
title: Next Edit Suggestions Now Available in Visual Studio with GitHub Copilot
author: Simona Liao
viewing_mode: external
feed_name: Microsoft DevBlog
date: 2025-06-09 15:00:39 +00:00
tags:
- AI Assisted Coding
- Code Editing
- Code Refactoring
- Copilot
- Copilot Completions
- Developer Tools
- NES
- Next Edit Suggestions
- Next Edits Suggestion
- Productivity
- Simona Liao
- VS
section_names:
- ai
- coding
- github-copilot
---
Authored by Simona Liao, this article details the launch of Next Edit Suggestions in Visual Studio, an AI-powered Copilot feature that enhances code editing and refactoring workflows.<!--excerpt_end-->

# Next Edit Suggestions Now Available in Visual Studio with GitHub Copilot

*By Simona Liao*

GitHub Copilot is well-known for its code completions (those familiar 'gray text' suggestions) that help autocomplete unfinished code and offer helpful templates. However, code writing is only part of development—editing and refactoring are equally crucial. What if Copilot could support these activities too?

We are excited to announce **Next Edit Suggestions (NES)**, now available in **Visual Studio 2022 17.14**. NES leverages your previous code edits to predict your next likely changes—be those insertions, deletions, or a mix. Unlike traditional Copilot completions, which are limited to your caret position, NES proactively surfaces suggestions anywhere in your file, pointing you to the most probable next edit.

## Example Usage Scenarios

Next Edit Suggestions are helpful in several scenarios. They go beyond repetitive tasks such as renaming by making logical code changes—like updating when you add new variables or alter a method’s purpose. Here are some examples:

### 1. Refactoring a 2D Point Class to 3D Point

- NES recognizes a pattern and suggests changes across the class as you refactor from 2D to 3D.
- [View demo video](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2025/06/Point3-26-1.mp4)

### 2. Updating Code Syntax to Modern C++ (Using STL)

- NES can update not just `printf()` to `std::cout` but also handles other syntax, such as refactoring `fgets()`.
- [View demo video](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2025/06/Migration3-28-1.mp4)

### 3. Making Logical Changes After Adding New Variables

- NES quickly adapts when a new variable is added, updating logic consistently throughout the code.
- [View demo video](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2025/06/AddingVariable3-28.mp4)

## Getting Started with Next Edit Suggestions

To enable NES:

1. Go to **Tools > Options > GitHub > Copilot > Copilot Completions > Enable Next Edit Suggestions** in Visual Studio 2022 17.14.
2. Once enabled, NES works as you code. When a suggestion is ready, it appears in a diff view:
    - **Red** highlights original code.
    - **Green** shows Copilot’s suggested changes.

If NES finds an edit on a different line, a navigation hint appears:

- The hint bar indicates you should press **Tab** to jump to the line containing the suggestion.
- An arrow in the gutter shows edit location.
- After landing on the edit line, **Tab** accepts the suggestion.

![NES navigation hint: Tab to jump to line with suggestion. Purple arrow in gutter.](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAoEAAAE2AQMAAAAkutqrAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAMElEQVR4nO3BMQEAAADCoPVPbQ0PoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAODDAGNMAAHkR73tAAAAAElFTkSuQmCC)

Once on the intended line:

- The gutter arrow changes color.
- The hint bar recommends **Tab to Accept**.

![NES: Inline diff, caret on suggestion line, gutter arrow changes to grey, hint says Tab accept.](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAArgAAAERAQMAAACq9UbXAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAALklEQVR4nO3BgQAAAADDoPlT3+AEVQEAAAAAAAAAAAAAAAAAAAAAAAAAAAAA8Axd2AABwLi9LQAAAABJRU5ErkJggg==)

## Mouse Interaction

- An arrow in the gutter signals edit suggestions.
- Clicking it brings up a menu: **Navigate to**, **Accept**, or **Dismiss** the suggestion.

![NES context menu on gutter arrow with options: Navigate to, Accept, Dismiss.](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAqMAAADIAQMAAADm/XqbAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAKElEQVR4nO3BAQ0AAADCoPdPbQ43oAAAAAAAAAAAAAAAAAAAAADgxwBDMAABPf3+DAAAAABJRU5ErkJggg==)

See NES step-by-step in the demo videos above.

## Feedback & Community

Try Next Edit Suggestions in Visual Studio today to see how it streamlines your logical code edits and refactoring. The development team is eager for your feedback—please share your thoughts or issues via the [Developer Community](https://developercommunity.visualstudio.com/VisualStudio) to help improve NES.

## Visual Studio Hub

For the latest on Visual Studio—including release notes, educational videos, social media updates, and discussions—visit the [Visual Studio Hub](https://visualstudio.microsoft.com/hub/).

## Appreciating Developer Feedback

Your insights continue to make Visual Studio the tool it is today. The team is grateful for your feedback and encourages ongoing contributions through [Developer Community](https://developercommunity.visualstudio.com/VisualStudio).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/next-edit-suggestions-available-in-visual-studio-github-copilot/)
