---
author: Emanuele Bartolesi
tags:
- Activity Bar
- AI
- Blogs
- Command Center
- Command Palette
- Cursor Blinking
- Development
- DevOps
- Editor Layout
- GitHub Copilot
- JetBrains Rider Dark Theme
- Menu Bar
- PowerToys LightSwitch
- Primary Sidebar
- Productivity
- Quick Input
- Settings JSON
- Theme Switching
- VS Code
- VS Code Extensions
- VS Code Pets
- VS Code Profiles
- Windows 11
- Windows PowerToys
title: How I created a Cozy Workspace in VS Code
feed_name: Emanuele Bartolesi's Blog
section_names:
- devops
date: 2025-12-11 15:54:26 +00:00
external_url: https://dev.to/playfulprogramming/how-i-created-a-cozy-workspace-in-vs-code-4bf0
primary_section: devops
---

Emanuele Bartolesi shares the VS Code layout and settings he uses to create a cleaner, less distracting “Productivity” profile—freeing up editor space (especially when chat panels are open) and making long coding sessions more comfortable.<!--excerpt_end-->

# How I created a Cozy Workspace in VS Code

I love VS Code. It is fast, flexible, and works for almost any stack. But the default layout is not great if you want a clean space to think. The editor area is smaller than it should be, and the UI is packed with elements that pull attention away from the code. With the new Copilot Chat panel taking even more space, the problem becomes obvious: too much noise, not enough room to work.

[![VS Code default layout with Copilot opened on the right](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fa5r5wccr9kd6xos8y0l1.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fa5r5wccr9kd6xos8y0l1.png)

This article explains the setup I finally arrived at: a dedicated *Productivity* profile built around a few small layout changes:

- Move the primary sidebar to the right
- Put the Activity Bar at the top
- Center Quick Input
- Hide the Command Center
- Move the Menu Bar into compact mode
- Switch themes automatically based on daylight (Windows theme)
- Improve cursor visibility
- Optional: add a “fun” extension (VS Code Pets)

These changes are minimal but have a clear impact. They give space back to the code instead of the UI.

## Creating a dedicated “Productivity” profile

VS Code profiles let you build isolated setups for different workflows. This helps when you don’t want extensions or layout settings for one task to affect everything else.

Bartolesi uses this profile only for focused work sessions.

Steps:

1. Open the Command Palette.
2. Run **Profiles: Create Profile**.
3. Choose **Empty**.
4. Name it **Productivity**.
5. Customize settings only for this profile.

You can also click the **Gear** icon (bottom left) and select **Profiles**.

[![Create a new VS Code profile](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fbjlckxu0kf9gwem00ny2.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fbjlckxu0kf9gwem00ny2.png)

## Moving the primary sidebar to the right

By default, VS Code puts the sidebar on the left. Bartolesi finds that this competes with the natural reading flow and pulls attention away from the editor.

He also notes that when you close the sidebar, the code shifts (because the sidebar is no longer present). Moving it to the right keeps the editor feeling more centered.

Setting:

```json
"workbench.sideBar.location": "right"
```

UI alternative: right-click the sidebar and select **Move Primary Sidebar Right**.

[![Move Primary Sidebar Right option in VS Code](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Frd8dwljxzizkerijyrk5.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Frd8dwljxzizkerijyrk5.png)

## Activity Bar at the top

The Activity Bar is very visible. Keeping it on the side creates an extra vertical column that eats into editor width. Moving it to the top uses horizontal space instead.

Setting:

```json
"workbench.activityBar.location": "top"
```

UI alternative: click at the top of the sidebar and choose **Top** from the **Activity Bar Position** menu.

[![Activity Bar position menu](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fov7dptqftxgdsl1bzxhq.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fov7dptqftxgdsl1bzxhq.png)

## Centering Quick Input for better focus

The Command Palette and file switcher typically appear at the top, which forces your eyes upward. Centering Quick Input keeps it closer to where you’re already focused.

Setting:

```json
"workbench.quickInput.location": "center"
```

## Hiding the Command Center

If you rely on keyboard shortcuts and Quick Input, Bartolesi considers the Command Center redundant UI noise.

Setting:

```json
"window.commandCenter": false
```

## Move the menu bar (compact)

If you rarely use the menu, you can reclaim vertical space and reduce visual distraction by switching it to compact mode.

Setting:

```json
"window.menuBarVisibility": "compact"
```

UI alternative: click **Customize Layout** (top right of the VS Code window) and change the Menu Bar visibility.

[![Menu Bar layout customization](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fukui96lv23b2mmlgk0xy.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fukui96lv23b2mmlgk0xy.png)

## Automatic light/dark theme switching with PowerToys LightSwitch

[PowerToys LightSwitch](https://learn.microsoft.com/en-us/windows/powertoys/) can change the Windows theme based on sunrise/sunset. VS Code can detect the OS color scheme and switch themes automatically.

Settings:

```json
"workbench.preferredDarkColorTheme": "JetBrains Rider Dark Theme",
"workbench.preferredLightColorTheme": "Default Light+",
"window.autoDetectColorScheme": true
```

Bartolesi uses **JetBrains Rider Dark Theme** to keep things consistent when switching between VS Code and Rider.

## Improving cursor visibility with “expand” blinking

The default cursor blink can be subtle. The “expand” style makes it more noticeable without (in his view) being distracting.

Setting:

```json
"editor.cursorBlinking": "expand"
```

## Bonus: a bit of fun with VS Code Pets

The **VS Code Pets** extension adds a small animated pet to your workspace. It’s optional, but intended to make long sessions feel a bit more pleasant.

[![VS Code Pets extension in action](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2F8q65fseydgmjk0ug12gk.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2F8q65fseydgmjk0ug12gk.png)

## Final result

A cozy development environment is not about adding more tools—it is about removing friction. By tweaking VS Code layout settings, you can reclaim space, reduce UI noise, and make the editor feel more purpose-built for deep work.

[![Final VS Code layout result](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fkjny7tun8120fq5lx4ya.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fkjny7tun8120fq5lx4ya.png)

If you want all settings in one place, Bartolesi links his repo:

- GitHub: [kasuken/Windows11-Setup](https://github.com/kasuken/Windows11-Setup)

The repo includes Windows 11 setup automation and configuration, including PowerShell scripts, Windows Terminal settings, and VS Code configuration.

## Notes on non-core page content

The source text also contains DEV Community UI elements and comments (profile cards, “Create template”, “Dismiss”, community boilerplate). Those are page/navigation elements rather than part of the article’s core instructions, so they are omitted here.


[Read the entire article](https://dev.to/playfulprogramming/how-i-created-a-cozy-workspace-in-vs-code-4bf0)

