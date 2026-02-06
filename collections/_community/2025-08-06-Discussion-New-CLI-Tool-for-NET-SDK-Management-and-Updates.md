---
external_url: https://www.reddit.com/r/dotnet/comments/1mjgvq3/want_to_make_it_easier_to_get_startedstay_up_to/
title: 'Discussion: New CLI Tool for .NET SDK Management and Updates'
author: chusk3
feed_name: Reddit DotNet
date: 2025-08-06 21:13:04 +00:00
tags:
- .NET
- .NET Install
- .NET Runtime
- .NET SDK
- CLI Tools
- Cross Platform Development
- Developer Experience
- Dnup
- Linux
- Package Management
- Scoop
- Self Contained Deployment
- Software Installation
- Tool Updates
- Ubuntu
- VS
- Windows
- Winget
- Community
section_names:
- dotnet
primary_section: dotnet
---
chusk3 shares an invitation from the .NET SDK team, led by PM Chet, for developers to discuss a new CLI tool proposal aimed at streamlining .NET SDK and Runtime installations and updates.<!--excerpt_end-->

# Discussion: New CLI Tool for .NET SDK Management and Updates

**Summary:**

chusk3 relays a request from the .NET SDK team (PM: Chet) for community feedback on a draft [dotnet/designs spec](https://github.com/dotnet/designs/pull/339) proposing a new command-line tool that simplifies downloading, installing, and updating .NET SDKs and Runtimes. The goal is to provide a consistent, cross-platform experience for managing .NET installations—something currently complicated by the diversity in package managers, OS distributions, and IDE integrations.

## Key Points from the Post and Community Discussion

- **Motivation:**
  - Current methods for managing .NET SDKs are inconsistent and can be error-prone, especially across different OS platforms (e.g., Linux distributions vs. Windows).
  - Existing solutions include platform-specific managers (scoop, winget), scripts (dotnet-install), and IDE-driven mechanisms (Visual Studio config), all with their own limitations.

- **Proposed Solution:**
  - Introduce a new CLI tool (tentatively: `dnup`) with commands like `dnup init`, `dnup install`, and `dnup update` to simplify and standardize SDK installation and updating.
  - Intention is to remove friction regardless of platform or development environment.

- **Community Feedback:**
  - Pain points with existing methods, especially on Linux where keeping up to date or installing specific versions can require workarounds or cause conflicts with system repositories.
  - Some feel that a new CLI tool might duplicate the functionality of existing solutions like winget or scoop, though winget is Windows-only and some Linux package managers lag in updates.
  - Strong preference from some users for integration directly into the dotnet CLI, or at least careful consideration to avoid wrapping/sugar-coating existing tools without added benefit.
  - Use cases cited include custom runtime installs (as in [DotnetRuntimeBootstrapper](https://github.com/Tyrrrz/DotnetRuntimeBootstrapper)) and the desire for administration-free installations and architecture flexibility.
  - Concerns that a new tool needs to offer genuine improvements and not just wrap existing scripts.

## Additional Notes

- The post includes a request from the subreddit bot reminding users to avoid spam and to review sidebar rules.
- Some off-topic discussion and minor moderation reminders are present, but technical discussion is the focus.

## Links

- [dotnet/designs spec PR #339](https://github.com/dotnet/designs/pull/339)
- [DotnetRuntimeBootstrapper](https://github.com/Tyrrrz/DotnetRuntimeBootstrapper)

## Conclusion

Developers are encouraged to review the proposed spec and provide feedback, both on GitHub and in the community, about the design and potential impact of a dedicated CLI tool for .NET SDK management.

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mjgvq3/want_to_make_it_easier_to_get_startedstay_up_to/)
