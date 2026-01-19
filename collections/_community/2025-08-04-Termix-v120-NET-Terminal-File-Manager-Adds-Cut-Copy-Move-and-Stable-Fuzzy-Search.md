---
external_url: https://www.reddit.com/r/dotnet/comments/1mhgcod/beautiful_terminal_based_file_manager_now/
title: 'Termix v1.2.0: .NET Terminal File Manager Adds Cut, Copy, Move, and Stable Fuzzy Search'
author: amrohann
viewing_mode: external
feed_name: Reddit DotNet
date: 2025-08-04 15:20:46 +00:00
tags:
- .NET
- .NET Tool
- Copy
- File Manager
- Fuzzy Search
- Keyboard Shortcuts
- Move
- Open Source
- Paste
- Progress Bar
- Terminal
- Termix
section_names:
- coding
---
In this post, amrohann presents Termix v1.2.0, a .NET-powered terminal file manager, highlighting major new features and installation tips.<!--excerpt_end-->

## About Termix v1.2.0

**Termix** is a terminal-based file navigator built using .NET. The latest release, version 1.2.0, brings several usability enhancements and new features designed to improve productivity for developers and terminal users.

### New Features in v1.2.0

- **Cut, Copy, Move, and Paste Workflows**: Perform file operations quickly with keyboard shortcuts:
  - Press `C` to copy the selected file or directory.
  - Press `X` to move the selected file or directory.
  - Press `P` to paste the pending copy or move operation into the current directory. The common workflow is to mark items for copy or move, navigate to the destination, and then paste.
- **Status Bar Indicators**: The status bar now displays a clear indicator ("Copy" or "Move") before completing your paste, making operations transparent.
- **Progress Bar for Large Operations**: A progress bar appears when handling large files, providing feedback on operation status.
- **More Stable Fuzzy Search**: Improved fuzzy search allows users to find files more reliably within the terminal UI.

### Installation and Updates

Get Termix installed or updated on your system using the dotnet global tool command:

- To install Termix for the first time:

  ```shell
  dotnet tool install --global Termix
  ```

- To update an existing Termix installation:

  ```shell
  dotnet tool update --global Termix
  ```

To build from source or contribute to development, you can clone the GitHub repository:
[https://github.com/amrohan/termix](https://github.com/amrohan/termix)

### Additional References

- Release notes: [v1.2.0 Release](https://github.com/amrohan/termix/releases/tag/v1.2.0)
- Original posts explaining project history and earlier features:
  - [Post One](https://www.reddit.com/r/dotnet/comments/1mcetzz/i_built_a_beautiful_modern_file_navigator_for_the/)
  - [Post Two](https://www.reddit.com/r/dotnet/comments/1mf1szg/termix_v090_add_rename_delete_write_file_ops/)

### Community and Contributions

Huge thanks to all contributors and users who helped resolve issues and provided feedback. Suggestions, bug reports, and participation in future development are all encouraged via GitHub or Reddit discussions.

---
Submitted by [amrohann](https://www.reddit.com/user/amrohann) ([Reddit Thread](https://www.reddit.com/r/dotnet/comments/1mhgcod/beautiful_terminal_based_file_manager_now/))

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mhgcod/beautiful_terminal_based_file_manager_now/)
