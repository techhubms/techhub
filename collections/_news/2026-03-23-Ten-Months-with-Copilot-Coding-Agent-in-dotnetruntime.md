---
feed_name: Microsoft .NET Blog
author: Stephen Toub - MSFT
section_names:
- ai
- devops
- dotnet
- github-copilot
external_url: https://devblogs.microsoft.com/dotnet/ten-months-with-cca-in-dotnet-runtime/
title: Ten Months with Copilot Coding Agent in dotnet/runtime
primary_section: github-copilot
date: 2026-03-23 14:55:00 +00:00
tags:
- .NET
- .NET Runtime
- AI
- AI Assisted Development
- BenchmarkDotNet
- Build Automation
- CI
- Code Review
- Copilot CLI
- Copilot Code Review
- Copilot Coding Agent
- Copilot Instructions.md
- Cross Platform Builds
- Developer Stories
- DevOps
- Dotnet/runtime
- EgorBot
- GitHub Copilot
- GitHub Skills
- Linux Build Environment
- NativeAOT
- News
- Open Source
- Performance Benchmarking
- Pull Requests
- Repository Instructions
- System.Text.Json
- System.Text.RegularExpressions
- Test Coverage
- xUnit
---

Stephen Toub - MSFT shares what the .NET team learned after 10 months using GitHub Copilot Coding Agent in dotnet/runtime, including PR throughput data, success and revert rates, review burden, and why good repo instructions and task selection matter more than model hype.<!--excerpt_end-->

## Overview

GitHub’s **Copilot Coding Agent (CCA)** became available in **May 2025**, and the .NET team started using it immediately in the **dotnet/runtime** repository to test whether a cloud-based AI coding agent could contribute responsibly to a highly critical, heavily reviewed open-source codebase.

Key point repeated throughout: **humans remain accountable** for everything that ships. CCA is treated as a tool in the workflow, not a replacement for engineering judgment.

Relevant links:

- Copilot Coding Agent concept doc: https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent
- dotnet/runtime repo: https://github.com/dotnet/runtime

## The numbers (May 19, 2025 → March 22, 2026)

### dotnet/runtime pull request population

| Category | PRs | Merged | Closed | Open | Success rate |
| --- | --- | --- | --- | --- | --- |
| Human (Microsoft) | 3,082 (50%) | 2,556 | 377 | 149 | 87.1% |
| Human (Community) | 1,411 (23%) | 1,029 | 262 | 120 | 79.7% |
| CCA | 878 (14%) | 535 | 253 | 90 | 67.9% |
| Bot (e.g., dependabot) | 810 (13%) | 666 | 109 | 35 | 85.9% |
| Total | 6,181 | 4,786 | 1,001 | 394 | 82.7% |

Notes on the dataset:

- **CCA cannot open PRs on its own**; every CCA PR was created at the explicit request of a maintainer.
- “Success rate” is `merged / (merged + closed)` (open PRs excluded).
- The team emphasizes selection bias: CCA is assigned a different class of tasks than humans.

### Revert rate (quality signal)

- CCA merged PRs: **3/535 reverted (0.6%)**
- Non-CCA merged PRs: **33/4,251 reverted (0.8%)**

The post notes the sample is small, but at least there were “no red flags”.

### AI review is now default

Beyond CCA authoring PRs, the repo also uses **Copilot Code Review (CCR)** so that:

- Every PR (human, CCA, external contributor) automatically gets AI review feedback.
- Many “human-authored” PRs still involve AI via Copilot in VS or Copilot CLI.

Screenshot referenced in the post:

![Five concurrent Copilot CLI sessions in terminal tabs, each working on a different task](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2026/03/copilot-cli-terminals.webp)

## Comparison across other .NET repos

Over the same period, the team collected data for other repos where CCA was used:

| Repository | Total PRs | CCA PRs | Merged CCA | Closed CCA | Open CCA | CCA success rate | Why it’s interesting |
| --- | --- | --- | --- | --- | --- | --- | --- |
| microsoft/aspire | 3527 | 1130 | 717 | 390 | 23 | 64.8% | Greenfield cloud-native stack; early/aggressive CCA adopter |
| dotnet/roslyn | 2784 | 263 | 165 | 56 | 42 | 74.7% | Deep domain complexity comparable to runtime |
| dotnet/aspnetcore | 1804 | 254 | 151 | 61 | 42 | 71.2% | Large mature framework codebase |
| dotnet/efcore | 1136 | 129 | 83 | 37 | 9 | 69.2% | Specialized query translation domain |
| dotnet/extensions | 569 | 127 | 98 | 25 | 4 | 79.7% | Contains AI-focused libraries (Microsoft.Extensions.AI) |
| modelcontextprotocol/csharp-sdk | 528 | 182 | 136 | 40 | 6 | 77.3% | Greenfield project; less legacy burden |

Across all seven repos:

- **2,963** CCA PRs
- **1,885 merged** (68.6%)
- ~**392k lines added**, ~**121k deleted**

## Change size distribution (dotnet/runtime)

CCA PR sizes vary widely:

- 59 PRs (6.7%) had **0 lines changed** (agent invoked but no change produced)
- Nearly half are **< 50 lines**
- A meaningful share are **101–500 lines**
- Some are **5,000+ lines** (large mechanical updates, refactors, generator-driven changes)

Success rate by size (decided PRs):

- Best: **1–50 lines (76–80%)**
- Drop: **101–500 lines (64%)**
- Rebound: **1,001+ lines (71.9%)** (often carefully-scoped mechanical tasks)

Key lesson: **scope clarity matters more than raw size**.

## Where the code goes: tests vs production

In merged CCA PRs:

- **65.7%** of added lines were **test code**
- **29.6%** production code
- **4.7%** other

For comparison, a random sample of merged human PRs:

- **49.9%** test code
- **38.5%** production
- **11.6%** other

The team attributes this partly to task selection: CCA is frequently assigned test-writing tasks.

## Success rate improved over time

Runtime success rate climbed from early volatility to ~71% in recent months:

- May 2025: **41.7%**
- Recent quarter: about **~71%**

The post frames this as a learning curve in:

- better task selection
- better prompts
- better repo setup (instructions + environment access)

## Two “throughput” stories

### 1) The Birthday Party Experiment

Assigning 20+ “help wanted” issues from a phone produced many PRs quickly. Examples:

- **Thread safety fix** in `System.Text.Json` (CCA implemented and added regression test; quick review)
- **Intentionally closed PR**: regex anchor quantifier syntax where existing behavior was intentional; CCA’s changes + test updates revealed this
- **Debugging win**: non-backtracking regex engine bug fixed with a one-line fix + tests
- **Struggle**: Windows BCrypt interop attempt for `Sha1ForNonSecretPurposes` became a long, messy PR, largely due to platform/testing/architecture constraints; eventually closed

Summary mental model:

- Excellent at well-scoped implementation and investigation
- Weak at architectural judgment in large, cross-platform codebases

### 2) The Redmond Flight Experiment

On Jan 6, 2026, nine PRs were opened from a phone by assigning work to CCA.

Notable examples:

- Large mechanical cleanup across **112 files** removing always-true preprocessor constants (`NET9_0_OR_GREATER`, `NET10_0_OR_GREATER`)
- Porting regex optimizations with complex IL emit logic (`System.Reflection.Emit`)

Important trade-off:

- PR creation becomes cheap, but **review capacity becomes the bottleneck**.

The post argues the sustainable response is improving review efficiency using AI-assisted review tooling:

- Copilot Code Review (CCR)
- A custom repo-aware **code-review skill**: https://github.com/dotnet/runtime/blob/main/.github/skills/code-review/SKILL.md

## The biggest lever: instructions and setup

Early failure mode:

- No `.github/copilot-instructions.md`
- Firewall blocked access to required package feeds
- Agent couldn’t reliably build/test

Result:

- Early success rate ~**41.7%**

After changes:

- Firewall access configured
- Added `copilot-instructions.md` with build commands, expectations, and testing guidance
- Adjusted setup steps to reduce timeouts

Correlation reported:

- Before first setup changes: **38.1%**
- After: **69%**

The post gives concrete examples of what to document:

- Exact build commands (`build.cmd` / `build.sh`, inner-loop vs outer-loop)
- Testing conventions (e.g., don’t assert localized exception messages)
- Platform constraints (CCA environment runs on Linux)
- Architectural boundaries (avoid `InternalsVisibleTo` as a testing shortcut)

## What works well vs what struggles

### Sweet spots (by category)

Success rate by task category (runtime):

- Removal/Cleanup: **84.7%**
- Testing: **75.6%**
- Refactoring: **69.7%**
- Bug Fix: **69.4%**
- Performance: **54.5%**

Key themes:

- Mechanical, well-scoped work tends to succeed.
- Open-ended work and architectural judgment tends to fail.

### Native code is hard

Reasons cited:

- CCA runs on Linux only (at least for much of the period in the report)
- Platform-specific code paths and toolchains (MSVC vs clang) make validation hard
- C++ has fewer guardrails than C#; memory management mistakes are more likely

### Performance work needs benchmarks

Failure mode:

- Agent makes performance claims without validation.

Mitigation:

- “No performance PR merges without empirical evidence.”
- A custom skill to invoke **EgorBot** to run **BenchmarkDotNet** on dedicated hardware, then have CCA analyze results.

Referenced tools:

- EgorBot: https://github.com/EgorBo/EgorBot
- BenchmarkDotNet: https://github.com/dotnet/BenchmarkDotNet

## Autonomy and human intervention

### Commit patterns

Average merged CCA PR: **6.9 commits** (median 5), but this includes:

- initial autonomous commits (often 2)
- post-review iteration commits

### Human commits dramatically increase success

Across all 878 CCA PRs:

| Category | PRs | Merged | Closed | Success rate |
| --- | --- | --- | --- | --- |
| With human commits on CCA branch | 396 | 280 | 45 | 86.2% |
| Without human commits | 482 | 255 | 208 | 55.1% |

The post argues the “without intervention” set includes exploratory attempts where maintainers chose not to salvage the PR.

## Code review load and time-to-merge

Merged CCA PRs:

- Average comments: **16.5**
- Median comments: **10**

Merged human PRs (same period, illustrative comparison):

- Average comments: **12.4**
- Median comments: **7**

Time to merge for merged CCA PRs (runtime):

- Median: ~**2 days**
- Average: just over a **week** (long tail)
- 58% merged within **3 days**
- 16% took **2+ weeks**

## Closed PRs can still be “value delivered”

The post argues merge-rate alone is misleading. Examples of valuable closures:

- Prototype that failed perf validation
- Issue already fixed (CCA discovers and demonstrates)
- Design discussions that end with a “don’t do this” conclusion

Closed PR reasons (253 closed CCA PRs), categorized via an AI model:

- Auto-closed draft / abandoned: 112 (44%)
- Wrong approach: 41 (16%)
- Superseded: 32 (13%)
- Design rejected: 20 (8%)
- Valuable exploration: 20 (8%)
- Too complex for CCA: 10 (4%)
- CI / infrastructure issues: 7 (3%)
- Already addressed: 6 (2%)
- Duplicate: 5 (2%)

They estimate a “value delivery” rate rising from **67.9%** to **73.7%** if you count certain closed outcomes as valuable.

## Practical lessons for teams adopting CCA

### Treat it as pairing

You are the senior engineer responsible for:

- scoping
- direction
- quality control

CCA provides implementation bandwidth.

### Match tasks to strengths

Good CCA tasks:

- clear reproduction + expected behavior
- bounded scope
- existing patterns to follow
- minimal cross-platform/platform-specific testing requirements

### Expect iteration

Review and iteration is normal. Good prompts and explicit follow-ups matter.

### Use instructions as leverage

Encoding lessons in `.github/copilot-instructions.md` reduces repeated failures and improves throughput over time.

## Main takeaway

The team’s closing summary:

- **Preparation matters more than the model.**
- Better repo setup, instructions, skills, and task selection drove a large improvement in outcomes.
- AI can increase throughput, but it can also shift bottlenecks to review capacity.


[Read the entire article](https://devblogs.microsoft.com/dotnet/ten-months-with-cca-in-dotnet-runtime/)

