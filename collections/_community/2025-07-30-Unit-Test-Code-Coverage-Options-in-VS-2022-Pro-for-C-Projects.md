---
layout: post
title: Unit Test Code Coverage Options in VS 2022 Pro for C Projects
author: jamawg
canonical_url: https://www.reddit.com/r/VisualStudio/comments/1md4xq8/how_to_get_unit_test_code_coverage_using_vs_2022/
viewing_mode: external
feed_name: Reddit Visual Studio
feed_url: https://www.reddit.com/r/VisualStudio/.rss
date: 2025-07-30 12:12:49 +00:00
permalink: /devops/community/Unit-Test-Code-Coverage-Options-in-VS-2022-Pro-for-C-Projects
tags:
- C Programming
- Clang
- Code Coverage
- Continuous Integration
- Coverlet
- Eclipse IDE
- Fine Code Coverage
- GCC
- Gcov
- Makefile
- MSBuild
- MSTest
- Static Analysis
- Test Automation
- Unit Testing
- VS
section_names:
- devops
---
jamawg shares practical strategies for obtaining unit test code coverage in Visual Studio 2022 Professional when working with C code, focusing on viable DevOps approaches.<!--excerpt_end-->

# Unit Test Code Coverage Options in VS 2022 Pro for C Projects

**Author: jamawg**

Visual Studio 2022 Enterprise includes built-in code coverage, but the Professional edition does not. This poses a challenge for teams wanting code coverage without upgrading to Enterprise. Here’s a summary of the considerations and approaches discussed for obtaining coverage in a C-based project on a Windows pipeline:

## Situation

- Project involves existing C code with a non-Windows target.
- Development preference is Visual Studio (VS) Pro, avoiding the upgrade to Enterprise for budget reasons.
- Need build, static code analysis, unit tests, and code coverage running on Windows pipelines.

## Native (C/C++) Code Coverage Options

1. **MSBuild & MSTest**
   - Used mostly for .NET projects; for native C/C++ code, MSTest integration can be limited.
2. **Alternate Compilers**
   - Consider building with Clang or GCC, both on Windows and cross-compiling as needed.
   - Use `gcov` to collect code coverage when compiling with GCC.
3. **Alternate Project/Build Systems**
   - Converting to a Makefile-based project could open up more options for custom build/test integrations.
4. **Static Analysis**
   - Make use of static analysis tools in the pipeline even if native code coverage options are limited.

## Visual Studio Extensions & Tools

- **Coverlet**: Mainly for .NET projects; not suitable for native C. Helpful mention for those using C# or mixed codebases.
  - [Coverlet GitHub](https://github.com/coverlet-coverage/coverlet)
- **Fine Code Coverage**: Also geared toward .NET, may not support C code but could be worth investigating further.
  - [Fine Code Coverage for VS](https://marketplace.visualstudio.com/items?itemName=FortuneNgwenya.FineCodeCoverage)

## Development Flow Suggestions

- Consider mix-and-match setups: Develop in VS, but build/test outside using CLI tools, Makefiles, or CMake to automate code coverage steps.
- If using Eclipse-based IDEs (from board suppliers), focus on using their CLI tools in build scripts or directly in the CI pipeline, bypassing the IDE UI when possible.

## Takeaways

- **.NET codebases have easier code coverage options in VS Pro via extensions.**
- **Native C/C++ projects require creativity—use alternate compilers and coverage tools, or adjust pipelines for coverage integration.**
- **Community/third-party extensions are improving but not always sufficient for C.**

## Additional Notes

- Be prepared to actively test and possibly troubleshoot third-party extension compatibility for code coverage features.
- Engaging with extension authors and experimenting in test projects can clarify tool limitations.

This post appeared first on "Reddit Visual Studio". [Read the entire article here](https://www.reddit.com/r/VisualStudio/comments/1md4xq8/how_to_get_unit_test_code_coverage_using_vs_2022/)
