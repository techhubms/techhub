---
tags:
- .NET
- Agentic Coding
- Agents
- AI
- AI Agents
- Appium
- Azure DevOps
- CI Troubleshooting
- Code Review
- Cross Pollination Rounds
- DevOps
- GitHub Copilot
- GitHub Copilot CLI
- GitHub Issues
- Helix Test Logs
- Issue Reproduction
- MAUI
- Multi Model Workflows
- News
- Open Source Contribution
- Pull Request Automation
- Source Generators
- Syncfusion
- Test Automation
- Testing
- UI Tests
- XAML
- XamlC
section_names:
- ai
- devops
- dotnet
- github-copilot
title: Accelerating .NET MAUI Development with AI Agents
external_url: https://devblogs.microsoft.com/dotnet/accelerating-dotnet-maui-with-ai-agents/
date: 2026-03-19 21:12:42 +00:00
feed_name: Microsoft .NET Blog
author: David Ortinau
primary_section: github-copilot
---

David Ortinau (guest post from Syncfusion) explains how the .NET MAUI team’s custom AI agents—run via GitHub Copilot CLI—speed up issue reproduction, test creation, PR review, and fix exploration, cutting per-issue time by 50–70% while improving test coverage and review quality.<!--excerpt_end-->

# Accelerating .NET MAUI Development with AI Agents

*This is a guest blog from [Syncfusion](https://www.syncfusion.com/). Learn more about the free, open-source [Syncfusion Toolkit for .NET MAUI](https://aka.ms/syncfusion-maui-toolkit).* 

As a partner with the .NET MAUI team, Syncfusion shares how custom-built AI agents are improving the .NET MAUI contribution workflow and reducing time to resolve issues.

## The traditional contributor challenge

Contributing bug fixes to .NET MAUI can take significant time due to multi-platform complexity and test requirements. Common bottlenecks include:

- **Issue reproduction**: Set up the Sandbox app and reproduce platform-specific issues (30–60 minutes)
- **Root cause analysis**: Debug across platforms and handlers (1–3 hours)
- **Fix implementation**: Write and test the fix (30–120 minutes)
- **Test creation**: Build comprehensive coverage (1–2 hours)

For new contributors, this can stretch into multiple days.

## The solution: custom-built AI agents and skills for .NET MAUI

The .NET MAUI team built specialized agents and reusable “skills” that streamline the contribution lifecycle. Syncfusion uses these tools to accelerate PR work.

### pr-review skill: issue resolution with built-in quality gates

The [pr-review skill](https://github.com/dotnet/maui/blob/main/.github/skills/pr-review/SKILL.md) follows a 4-phase workflow.

#### Phase 1: Pre-Flight analysis

- Reads the GitHub issue and extracts reproduction steps
- Analyzes the codebase to identify affected components
- Flags platform-specific considerations (Android, iOS, Windows, Mac Catalyst)

#### Phase 2: Gate — test verification before fixing

- Checks whether tests exist for the issue/PR
- If tests are missing, tells you to create them first using **write-tests-agent**
- Validates that existing tests actually fail without a fix (proving they catch the bug)

Recommended workflow:

1. Use **write-tests-agent** to create tests
2. Use **pr-review** to verify tests and guide the fix

#### Phase 3: Try-Fix — multiple independent fix attempts

The pr-review skill uses the [try-fix skill](https://github.com/dotnet/maui/blob/main/.github/skills/try-fix/SKILL.md) with 4 AI models to explore different solutions:

- Proposes up to 4 independent fix strategies
- Runs the test suite after each attempt
- Records results for comparison

Example try-fix output:

> **Attempt 1:** Handler-level fix in CollectionViewHandler → Tests pass on iOS, fail on Android
> **Attempt 2:** Platform-specific fix in Items2 → Tests pass on all platforms, but causes regression
> **Attempt 3:** Core control fix with platform guards → All tests pass, no regressions
> ✓ **Attempt 3 selected as optimal solution**

#### Phase 4: report generation

Produces a PR-ready summary including:

- Fix description and rationale
- Test results (before/after)
- Alternatives attempted and why they weren’t selected
- Recommendation (approve or request changes)

### write-tests-agent: chooses the right test strategy

The [write-tests-agent](https://github.com/dotnet/maui/blob/main/.github/agents/write-tests-agent.md) acts as a test strategist.

#### Multi-strategy test creation

For **UI interaction bugs**, it can invoke [write-ui-tests](https://github.com/dotnet/maui/blob/main/.github/skills/write-ui-tests/SKILL.md):

- Creates Appium-based tests in `TestCases.HostApp` and `TestCases.Shared.Tests`
- Adds `AutomationId` attributes for locating elements
- Uses platform-appropriate assertions

For **XAML parsing/compilation bugs**, it can invoke [write-xaml-tests](https://github.com/dotnet/maui/blob/main/.github/skills/write-xaml-tests/SKILL.md):

- Adds tests to `Controls.Xaml.UnitTests`
- Covers parsing, XamlC compilation, and source generation
- Validates markup extensions and binding syntax
- Tests all inflators: Runtime, XamlC, SourceGen

Planned/future test types mentioned:

- Unit tests
- Device tests
- Integration tests

#### Test verification: Fail → Pass validation

The agent relies on [verify-tests-fail-without-fix](https://github.com/dotnet/maui/blob/main/.github/skills/verify-tests-fail-without-fix/SKILL.md) so tests prove the bug exists.

**Mode 1: verify failure only (tests first)**

1. Run tests on current buggy code
2. Verify tests **FAIL**
3. ✓ Confirms tests reproduce the issue

**Mode 2: full verification (fix + tests)**

1. Revert fix files (keep test files)
2. Run tests → should **FAIL**
3. Restore fix files
4. Run tests → should **PASS**
5. ✓ Confirms both tests and fix are valid

#### Layered coverage across test types

Example approach for a CollectionView scrolling bug:

- **UI test**: Appium scroll + visual/behavioral assertions
- **XAML test**: verifies `ItemTemplate` compiles across Runtime/XamlC/SourceGen

### sandbox-agent: manual testing and validation

The [sandbox-agent](https://github.com/dotnet/maui/blob/main/.github/agents/sandbox-agent.md) helps with hands-on validation:

- Creates scenarios in `Controls.Sample.Sandbox`
- Builds/deploys to iOS simulators, Android emulators, or Mac Catalyst
- Can generate Appium scripts

Use it for:

- Functional validation before merge
- Complex reproduction scenarios
- Visual layout/rendering checks
- Cases that are hard to automate

### learn-from-pr agent: continuous improvement loop

The [learn-from-pr agent](https://github.com/dotnet/maui/blob/main/.github/agents/learn-from-pr.md) analyzes completed PRs and improves instructions, skills, and documentation over time.

## How to use these tools (prompt examples)

These prompts are typed in the **GitHub Copilot CLI** terminal inside a cloned `dotnet/maui` repo. The tooling reads local files, runs builds/tests, and interacts with GitHub APIs.

### Fixing an issue with pr-review

```text
Fix issue #67890
```

With more context:

```text
Fix issue #67890. The issue appears related to async lifecycle events during CollectionView item recycling. Previous attempts may have failed because they didn't account for view recycling on Android.
```

For exploring different fix approaches:

```text
Fix issue #67890. Try a handler-level approach first. If that doesn't work, consider modifying the core control with platform guards.
```

### Reviewing a pull request

```text
Review PR #12345
```

Targeted review focus:

```text
Review PR #12345. Focus on thread safety in the async handlers and ensure Android platform-specific code follows conventions.
```

Test coverage validation:

```text
Review PR #12345. Verify that the tests actually reproduce the bug and cover all affected platforms (iOS and Mac Catalyst).
```

### Writing tests with write-tests-agent

```text
Write tests for issue #12345
```

Or request a specific test style:

```text
Write UI tests for issue #12345 that reproduce the button click behavior.
```

### Manual validation with sandbox-agent

```text
Test PR #12345 in Sandbox
```

Platform-specific:

```text
Test PR #12345 on iOS 18.5. Focus on the layout changes in SafeArea handling.
```

Reproduction-focused:

```text
Reproduce issue #12345 in Sandbox on Android. The user reported it happens when rotating the device while a dialog is open.
```

## Multi-model architecture for quality assurance

In Phase 3 (Try-Fix), the workflow runs 4 models sequentially:

| Order | Model | Purpose |
| --- | --- | --- |
| 1 | Claude Opus 4.6 | Deep analysis first attempt |
| 2 | Claude Sonnet 4.6 | Balanced second attempt |
| 3 | GPT-5.3-Codex | Code-specialized attempt |
| 4 | Gemini 3 Pro Preview | Different model family perspective |

Reasons given for **sequential (not parallel)** execution:

- Only one Appium session can control a device/emulator at a time
- All attempts modify the same source files; parallel edits would conflict
- Each model runs independently with no knowledge of others
- A cleanup step restores a clean baseline before the next attempt

**Cross-pollination rounds** (up to 3 rounds):

1. Round 1: each model proposes and tests one approach
2. Round 2: each model reviews prior results and either concludes “NO NEW IDEAS” or proposes “NEW IDEA: …”
3. Round 3: repeat until all models report “NO NEW IDEAS”

Claimed benefits:

- Broader solution exploration
- Reduced hallucination via empirical testing
- Data-driven selection of the best fix

## Measurable impact

Reported team improvements after adopting the agents:

| Task | Before (Manual) | After (Agents) | Time Saved |
| --- | --- | --- | --- |
| Issue reproduction | 30–60 min | 5–10 min | ~50 min |
| Root cause analysis | 1–3 hours | 20–40 min | ~1.5 hours |
| Implementing fix | 30–120 min | Automated | ~1 hour |
| Writing tests | 1–2 hours | 10–20 min | ~1.5 hours |
| Exploring alternatives | Not feasible | Built-in | “Priceless” |
| **Total per issue** | **4–8 hours** | **45 min–2.5 hours** | **~4–5 hours** |

Overall: **50–70% time reduction** per issue and **2–3x** more issues handled per week.

### Quality improvements

- Test coverage: 95%+ of PRs include comprehensive tests (up from ~60%)
- First-time fix rate: 80% (up from ~50%)
- Fewer review cycles / less back-and-forth

## Skills ecosystem (composable capabilities)

The post emphasizes modular “skills” that can be composed into different workflows.

### Core skills

- [try-fix](https://github.com/dotnet/maui/blob/main/.github/skills/try-fix/SKILL.md)
- [write-ui-tests](https://github.com/dotnet/maui/blob/main/.github/skills/write-ui-tests/SKILL.md)
- [write-xaml-tests](https://github.com/dotnet/maui/blob/main/.github/skills/write-xaml-tests/SKILL.md)
- [verify-tests-fail-without-fix](https://github.com/dotnet/maui/blob/main/.github/skills/verify-tests-fail-without-fix/SKILL.md)

### Supporting skills

- [azdo-build-investigator](https://github.com/dotnet/maui/blob/main/.github/skills/azdo-build-investigator/SKILL.md): queries Azure DevOps PR build info, downloads Helix logs
- [run-device-tests](https://github.com/dotnet/maui/blob/main/.github/skills/run-device-tests/SKILL.md)
- [pr-finalize](https://github.com/dotnet/maui/blob/main/.github/skills/pr-finalize/SKILL.md)

## Getting started as a contributor

1. **Set up GitHub Copilot CLI** and authenticate it: [GitHub Copilot CLI Documentation](https://docs.github.com/en/copilot/github-copilot-in-the-cli)
2. Pick an issue:
   - [Good First Issues](https://github.com/dotnet/maui/labels/good%20first%20issue)
   - [Help Wanted](https://github.com/dotnet/maui/labels/help%20wanted)
   - [Priority Issues](https://github.com/dotnet/maui/labels/p%2F0)
3. Use agents in the recommended order:
   1. `Write tests for issue #12345`
   2. `Fix issue #12345`
   3. Or review with `Review PR #12345`
4. Do human review:
   - Verify root cause and edge cases
   - Ensure conventions are followed
   - Add missing context
5. Submit the PR with clearer rationale, tests, and validated results

## Hypothetical example workflow

**Issue**: CollectionView items disappear on iOS when scrolling rapidly

Traditional (4–6 hours): setup sandbox, reproduce, debug handlers, implement fix.

Agent-assisted:

```text
Write tests for issue #12345
```

```text
Fix issue #12345
```

Then the pr-review skill runs:

- Pre-Flight (5–10 min)
- Gate (test existence + fail/pass validation)
- Try-Fix (20–40 min across multiple models)
- Report (select best approach)

Claimed result: PR ready in under an hour.

## Conclusion

The post argues that automating reproduction, testing, and fix exploration lets contributors focus on understanding problems and reviewing solutions, leading to faster cycles and higher-quality PRs.

## Resources

### Documentation

- [.NET MAUI Repository](https://github.com/dotnet/maui)
- [pr-review Skill Documentation](https://github.com/dotnet/maui/blob/main/.github/skills/pr-review/SKILL.md)
- [write-tests-agent Documentation](https://github.com/dotnet/maui/blob/main/.github/agents/write-tests-agent.md)
- [sandbox-agent Documentation](https://github.com/dotnet/maui/blob/main/.github/agents/sandbox-agent.md)
- [learn-from-pr Agent Documentation](https://github.com/dotnet/maui/blob/main/.github/agents/learn-from-pr.md)
- [Skills Directory](https://github.com/dotnet/maui/tree/main/.github/skills)
- [Contributing Guide](https://github.com/dotnet/maui/blob/main/.github/CONTRIBUTING.md)

### Getting started

- [GitHub Copilot CLI Documentation](https://docs.github.com/en/copilot/github-copilot-in-the-cli)
- [Good First Issues](https://github.com/dotnet/maui/labels/good%20first%20issue)
- [.NET MAUI Discussions](https://github.com/dotnet/maui/discussions)


[Read the entire article](https://devblogs.microsoft.com/dotnet/accelerating-dotnet-maui-with-ai-agents/)

