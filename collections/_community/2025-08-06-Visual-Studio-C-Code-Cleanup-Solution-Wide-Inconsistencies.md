---
external_url: https://www.reddit.com/r/VisualStudio/comments/1mjk2n7/analyze_code_cleanup_broken_cc/
title: 'Visual Studio C++ Code Cleanup: Solution-Wide Inconsistencies'
author: kohuept
feed_name: Reddit Visual Studio
date: 2025-08-06 23:21:36 +00:00
tags:
- Analyze Menu
- Bug Report
- C++
- Code Cleanup
- Code Quality
- Developer Experience
- Documentation
- Feature Request
- IDE
- Microsoft
- Solution Explorer
- VS
- Coding
- Community
section_names:
- coding
primary_section: coding
---
kohuept describes persistent problems with running Code Cleanup for C++ solutions in Visual Studio, questioning the inconsistent behavior and Microsoft's support response.<!--excerpt_end-->

# Visual Studio C++ Code Cleanup: Solution-Wide Inconsistencies

**User:** kohuept

## Overview

This thread raises concerns regarding Visual Studio's Code Cleanup tool for C++ development. The author reports that while file-level Code Cleanup works through the editor toolbar or on-save actions, running Code Cleanup for the entire solution from the 'Analyze' menu or Solution Explorer menu does not function for C++ projects.

## Reported Issue

- Code Cleanup works on individual C++ files using the icon at the bottom of the editor or with the save event.
- Attempting to perform Code Cleanup on the whole C++ solution via the Analyze menu or context menu in Solution Explorer yields no result.
- This behavior seems unsupported but is counter to what is stated in Microsoft's official documentation.

## User Findings and References

- [Official Microsoft documentation](https://learn.microsoft.com/en-us/visualstudio/ide/code-styles-and-code-cleanup?view=vs-2022&pivots=programming-language-cpp) claims Code Cleanup works for C++.
- A related [DevDiv suggestion ticket (#2284151)](https://devdiv.visualstudio.com/DevDiv/_workitems/edit/2284151) has also been raised, but was reportedly converted into a feature request with little apparent traction.
- The user is frustrated that the UI gives the impression that solution-wide Code Cleanup should work for C++ developers.

## Discussion Points

- Confusion over conflicting documentation and actual Visual Studio behavior
- Desire for improved support and transparency from Microsoft
- Suggests upvoting the feedback ticket to gain more attention from the development team

## Key Quotes

> "Doing Code Cleanup on a single file from the little icon at the bottom works, and Code Cleanup on save also works. Just not from the Analyze menu or from right clicking and selecting Code Cleanup in the Solution Explorer."

> "But the documentation says it works for C++? And it works fine from the file health bar thing and on save. I don't get why they would make it not work only for the whole solution but leave the button there and say it works."

## Recommendations

- C++ developers encountering the same issue should upvote the suggestion ticket
- Monitor documentation and Visual Studio updates for any changes in Code Cleanup support for C++ projects

This post appeared first on "Reddit Visual Studio". [Read the entire article here](https://www.reddit.com/r/VisualStudio/comments/1mjk2n7/analyze_code_cleanup_broken_cc/)
