-- Migration 021: Improve GHC feature descriptions
-- Date: 2026-05-12
-- Purpose: Replace the auto-generated "This content/video demonstrates..." excerpt text
--          on ghc_features rows with concise, functional descriptions that explain
--          what each feature actually does and why it matters.
--          Max ~1000 characters each. No marketing intro language.

UPDATE ghc_features SET excerpt = 'Real-time AI code suggestions as you type — from single-line completions to entire function bodies. Uses your open files, comments, and cursor context to deliver language-aware completions across all major editors and languages. The foundational Copilot capability available since launch.', updated_at = NOW() WHERE slug = 'code-completion';

UPDATE ghc_features SET excerpt = 'A conversational AI assistant embedded directly in your IDE. Ask questions about your code, get explanations, generate tests, fix bugs, and refactor through natural language without leaving your editor. Supports multi-turn conversations with full awareness of your open files and workspace.', updated_at = NOW() WHERE slug = 'chat-in-ide';

UPDATE ghc_features SET excerpt = 'Automatically generates unit tests for your code — covering happy paths, edge cases, and error conditions. Reduces the time spent writing boilerplate test scaffolding and helps ensure test coverage for functions and classes you have already written.', updated_at = NOW() WHERE slug = 'unit-tests';

UPDATE ghc_features SET excerpt = 'Explains what a piece of code does in plain language — covers logic flow, algorithm intent, data transformations, and edge cases. Useful for understanding unfamiliar code, onboarding to a new codebase, or reviewing legacy implementations.', updated_at = NOW() WHERE slug = 'code-explanation';

UPDATE ghc_features SET excerpt = 'Generates documentation for your code — inline comments, JSDoc/XML doc blocks, README sections, and more. Explains what functions do, their parameters, return values, and side effects, reducing the documentation debt that builds up during fast development cycles.', updated_at = NOW() WHERE slug = 'code-documenting';

UPDATE ghc_features SET excerpt = 'Converts code from one programming language to another while preserving logic and intent. Useful for migrating legacy codebases, porting scripts between ecosystems, or learning how a pattern works in a different language.', updated_at = NOW() WHERE slug = 'code-translation';

UPDATE ghc_features SET excerpt = 'Analyzes your staged changes and proposes an appropriate commit message that summarizes what was changed and why. Saves time writing messages manually and produces more consistent, descriptive commit history.', updated_at = NOW() WHERE slug = 'commit-message-suggestions';

UPDATE ghc_features SET excerpt = 'Configure how Copilot generates commit messages by providing custom instructions — specify preferred format, required prefixes such as Conventional Commits, character limits, or project-specific conventions. Ensures generated messages consistently match your team''s standards.', updated_at = NOW() WHERE slug = 'commit-message-custom-instructions';

UPDATE ghc_features SET excerpt = 'Slash commands in Copilot Chat provide quick shortcuts for common operations — /explain, /fix, /tests, /doc, and more. Reduce the need to phrase natural-language requests for repetitive tasks and provide predictable, consistent entry points into Copilot''s capabilities.', updated_at = NOW() WHERE slug = 'slash-commands';

UPDATE ghc_features SET excerpt = 'Helps diagnose bugs by analyzing the call stack, variable values, and error messages in context. Suggests what went wrong and proposes fixes, reducing the time spent mentally tracing execution paths during debugging sessions.', updated_at = NOW() WHERE slug = 'code-debugging';

UPDATE ghc_features SET excerpt = 'The /fix slash command in Copilot Chat analyzes an error or problematic code selection and proposes a corrected version. Handles compilation errors, runtime exceptions, type mismatches, and logic issues — often resolving them in a single interaction.', updated_at = NOW() WHERE slug = 'code-fixing';

UPDATE ghc_features SET excerpt = 'Provides visibility into how Copilot is being used across your team or organization — acceptance rates, active users, most-used features, and more. Helps managers and admins understand ROI, identify adoption gaps, and make informed decisions about Copilot rollouts.', updated_at = NOW() WHERE slug = 'usage-metrics';

UPDATE ghc_features SET excerpt = 'Centralized controls for assigning, removing, and auditing Copilot licenses across an organization. Administrators can manage access per user, team, or all members, and review seat usage to optimize license allocation.', updated_at = NOW() WHERE slug = 'user-management';

UPDATE ghc_features SET excerpt = 'Prevents specific files, directories, or patterns from being sent to Copilot''s AI models. Useful for excluding files containing secrets, proprietary algorithms, or sensitive data you do not want included as context in completions or chat responses.', updated_at = NOW() WHERE slug = 'content-exclusions';

UPDATE ghc_features SET excerpt = 'Code, prompts, and suggestions are not used to train GitHub''s AI models by default for Business and Enterprise customers. Your private code remains private and does not contribute to model improvements that could surface in other users'' suggestions.', updated_at = NOW() WHERE slug = 'data-excluded-from-training-by-default';

UPDATE ghc_features SET excerpt = 'When a Copilot suggestion closely matches code in a public GitHub repository, Copilot shows a reference indicating the source. Helps you make informed decisions about using the suggestion and provides attribution information for compliance and legal awareness.', updated_at = NOW() WHERE slug = 'code-referencing';

UPDATE ghc_features SET excerpt = 'Copilot is embedded throughout GitHub.com — available in the code editor, issues, pull requests, and discussions. Lets you use AI assistance without switching to a separate IDE, useful for quick reviews, issue triage, and lightweight edits directly in the browser.', updated_at = NOW() WHERE slug = 'integration-in-web-ui';

UPDATE ghc_features SET excerpt = 'When a GitHub Actions job fails, Copilot analyzes the error logs and explains what went wrong in plain language. Suggests likely causes and remediation steps, reducing time spent digging through CI output to understand build and test failures.', updated_at = NOW() WHERE slug = 'explain-failed-action-jobs';

UPDATE ghc_features SET excerpt = 'Context-sensitive AI actions surfaced inline in the editor and on GitHub.com — such as explain, fix, document, or review. Provide one-click access to the most relevant Copilot operation for the code or context currently in focus.', updated_at = NOW() WHERE slug = 'smart-actions';

UPDATE ghc_features SET excerpt = 'Ask Copilot about the code in a specific pull request — get summaries, explanations of individual changes, suggestions for improvements, or answers about the intent behind the diff. Available directly on the PR page in GitHub.', updated_at = NOW() WHERE slug = 'chat-with-your-pull-request';

UPDATE ghc_features SET excerpt = 'Allows Copilot to draw context from multiple repositories simultaneously in GitHub Enterprise environments. Useful for answering questions about systems that span several repos, finding cross-repo patterns, and getting suggestions that account for shared libraries or APIs.', updated_at = NOW() WHERE slug = 'multi-repository-context';

UPDATE ghc_features SET excerpt = 'Lets Copilot answer questions grounded in your organization''s private repositories and wikis, indexed as Knowledge Bases. Results in contextually relevant answers about your internal codebase, architecture decisions, and domain-specific conventions rather than generic responses.', updated_at = NOW() WHERE slug = 'chat-with-knowledge-bases';

UPDATE ghc_features SET excerpt = 'When Code Scanning detects a security vulnerability, Copilot automatically generates a code fix and presents it as a suggested change in the pull request. Reduces the time between detection and remediation by making fixes immediately actionable without leaving the review flow.', updated_at = NOW() WHERE slug = 'code-scanning-ai-autofix';

UPDATE ghc_features SET excerpt = 'A REST API that exposes Copilot usage data programmatically — acceptance rates, active users, model usage, seat counts, and more. Lets teams integrate Copilot metrics into dashboards, reporting pipelines, and adoption tracking tools of their choice.', updated_at = NOW() WHERE slug = 'copilot-metrics-api';

UPDATE ghc_features SET excerpt = 'Generates plain-language summaries of Dependabot security alerts directly in GitHub. Explains the vulnerability, affected versions, and recommended remediation steps, making it faster to understand and act on security issues without consulting external documentation.', updated_at = NOW() WHERE slug = 'security-advisory-summaries';

UPDATE ghc_features SET excerpt = 'Enables Copilot to search the web for current information and include it in responses. Useful for questions about recent library updates, unfamiliar APIs, or anything that requires up-to-date context beyond the training data.', updated_at = NOW() WHERE slug = 'web-search-with-bing';

UPDATE ghc_features SET excerpt = 'AI models trained specifically on your organization''s private codebase to produce suggestions aligned with your internal patterns, APIs, and conventions. Results in higher-relevance completions than general-purpose models when working within your own code.', updated_at = NOW() WHERE slug = 'fine-tuned-models';

UPDATE ghc_features SET excerpt = 'Define different Copilot instructions for different parts of your codebase — for example, stricter security rules for authentication code or React conventions only for frontend files. Instructions are applied automatically based on which files are involved in the current context.', updated_at = NOW() WHERE slug = 'path-specific-custom-instructions';

UPDATE ghc_features SET excerpt = 'A marketplace of third-party extensions that plug directly into Copilot Chat. Lets you query, configure, and trigger external developer tools — databases, observability platforms, feature flags, CI systems, and more — through natural language without leaving your editor.', updated_at = NOW() WHERE slug = 'copilot-extensions-marketplace';

UPDATE ghc_features SET excerpt = 'Choose from multiple frontier AI models within Copilot Chat — including models from OpenAI, Anthropic, and Google. Different models have different strengths, letting you pick the best one for your specific task without switching tools.', updated_at = NOW() WHERE slug = 'multi-models';

UPDATE ghc_features SET excerpt = 'Automatically generates a pull request description from your diff and branch context on GitHub.com. Produces a summary of what changed and why, saving time writing PR bodies and ensuring reviewers have the context they need.', updated_at = NOW() WHERE slug = 'pr-body-generation-in-webui';

UPDATE ghc_features SET excerpt = 'Generates pull request descriptions directly from VS Code before you push. Analyzes your local branch changes and produces a structured summary, so you arrive at GitHub with a completed PR body rather than starting from scratch.', updated_at = NOW() WHERE slug = 'pr-body-generation-in-vs-code';

UPDATE ghc_features SET excerpt = 'Review code changes directly within your IDE before committing. Copilot highlights potential issues, suggests improvements, and provides inline feedback on your local diff — bringing AI code review into your normal editing workflow rather than only on pull requests.', updated_at = NOW() WHERE slug = 'local-in-editor-code-reviews';

UPDATE ghc_features SET excerpt = 'Describe a change in natural language and Copilot proposes coordinated edits across all relevant files simultaneously. You review each change as a diff per file, making it practical to execute refactors, renames, and feature additions that touch many parts of a codebase at once.', updated_at = NOW() WHERE slug = 'multi-file-edits';

UPDATE ghc_features SET excerpt = 'Brings Copilot Chat to the GitHub mobile app, letting you interact with your repositories on the go. Summarize issues, pull requests, and discussions, ask questions about a codebase, and get AI-assisted insights without needing to be at a computer.', updated_at = NOW() WHERE slug = 'chat-on-mobile';

UPDATE ghc_features SET excerpt = 'The @github chat participant brings repository-aware context into Copilot Chat on GitHub.com — ask about issues, pull requests, commits, code, and discussions across your repositories. Answers are grounded in your actual repo data rather than generic knowledge.', updated_at = NOW() WHERE slug = 'atgithub-chat-participant';

UPDATE ghc_features SET excerpt = 'Lets you write personal instructions that persistently guide Copilot''s behavior in VS Code — specify your preferred language version, coding style, libraries to avoid, or project context. Applied automatically to every chat interaction without repeating yourself each session.', updated_at = NOW() WHERE slug = 'user-instructions';

UPDATE ghc_features SET excerpt = 'Provides natural-language assistance for the command line via gh copilot. Use suggest to generate shell commands from a description, and explain to understand what a complex command does. Covers git, the GitHub CLI, and common shell tools — and can execute the generated command after your confirmation.', updated_at = NOW() WHERE slug = 'cli-assistance';

UPDATE ghc_features SET excerpt = 'A cloud-based environment where you describe a task and Copilot proposes a multi-file implementation plan, generates the changes, and runs tests without a local setup. An early exploration of plan-driven, agentic development that informed later features like the Coding Agent.', updated_at = NOW() WHERE slug = 'preview-for-copilot-workspace';

UPDATE ghc_features SET excerpt = 'Generates Playwright end-to-end tests from your existing application code. Copilot analyzes page structure and interactions to produce test scripts that cover key user flows, reducing the manual effort of writing browser automation from scratch.', updated_at = NOW() WHERE slug = 'playwright-test-generation';

UPDATE ghc_features SET excerpt = 'Rather than only completing the current line, Copilot predicts the next logical edit anywhere in the open file based on your recent changes. Particularly useful for repetitive refactors, renames, and pattern-following changes that would otherwise require manual fan-out across multiple locations.', updated_at = NOW() WHERE slug = 'next-edit-suggestions';

UPDATE ghc_features SET excerpt = 'Copilot independently plans and executes multi-step changes from a single natural-language prompt — edits files across the repository, runs terminal commands, executes tests, reads error output, and self-corrects. You review each proposed change before it is applied, staying in control throughout.', updated_at = NOW() WHERE slug = 'agent-mode';

UPDATE ghc_features SET excerpt = 'Include the content of any public web page as part of your Copilot Chat context. Useful for asking Copilot to implement something based on documentation, a spec, or an API reference — paste the URL and Copilot retrieves and reads the page as part of your prompt.', updated_at = NOW() WHERE slug = 'fetch-webpage';

UPDATE ghc_features SET excerpt = 'Connect MCP (Model Context Protocol) servers to Copilot Agent Mode to give agents access to live external data and actions — query Azure resources, create GitHub issues, read from databases, and more. Agents use the results directly in their reasoning, enabling real-world integrations beyond the local codebase.', updated_at = NOW() WHERE slug = 'mcp-with-azure-and-github';

UPDATE ghc_features SET excerpt = 'A monthly allowance of requests that unlock the most capable frontier models such as Claude Sonnet 4, GPT-5, and Gemini 2.5 Pro. Lighter models remain unlimited while top-tier ones draw from this pool, giving teams predictable access to high-capability AI without unbounded cost.', updated_at = NOW() WHERE slug = 'premium-requests';

UPDATE ghc_features SET excerpt = 'Upgrades the underlying model for inline code completions from GPT-3.5 Turbo to GPT-4o, resulting in more accurate, context-aware suggestions. Better at understanding complex codebases, handling multi-line completions, and producing idiomatic code in a wider range of languages.', updated_at = NOW() WHERE slug = 'gpt-4o-copilot-suggestions-model';

UPDATE ghc_features SET excerpt = 'Assign a GitHub Issue to Copilot and it works autonomously in the background — creating a branch, reading related code, implementing a solution, running CI checks, and opening a pull request for your review. Reduces context-switching by handling implementation work while you focus on other things.', updated_at = NOW() WHERE slug = 'copilot-coding-agent-in-pull-requests';

UPDATE ghc_features SET excerpt = 'Supply your own API key for Anthropic, OpenAI, Google, or any OpenAI-compatible provider inside VS Code Copilot Chat. Routes requests through your own contracts and quotas while keeping the full Copilot UX, and unlocks models not yet in the default catalog.', updated_at = NOW() WHERE slug = 'bring-your-own-llm';

UPDATE ghc_features SET excerpt = 'Lets you choose which AI model powers Copilot Chat — switch between OpenAI, Anthropic, Google, and other frontier models for different tasks. Use a faster model for quick questions and a more capable one for complex reasoning, all without leaving the editor.', updated_at = NOW() WHERE slug = 'model-selection';

UPDATE ghc_features SET excerpt = 'Create purpose-built AI agents enriched with specific repositories, files, and guidelines. Spaces let you scope Copilot''s knowledge to a particular project, team, or domain — resulting in more relevant answers and suggestions that reflect your organizational context.', updated_at = NOW() WHERE slug = 'copilot-spaces';

UPDATE ghc_features SET excerpt = 'Define custom coding standards that Copilot applies when reviewing pull requests. Specify style rules, security requirements, preferred patterns, and naming conventions — Copilot enforces them consistently across all reviews without relying on individual reviewers to catch them manually.', updated_at = NOW() WHERE slug = 'set-coding-guidelines-for-code-review';

UPDATE ghc_features SET excerpt = 'Generates project starter templates and boilerplate code to accelerate the beginning of new work. Instead of setting up structure from scratch, describe what you want to build and Copilot produces a ready-to-use starting point tailored to your stack and requirements.', updated_at = NOW() WHERE slug = 'ice-breakers';

UPDATE ghc_features SET excerpt = 'Lets administrators define Copilot instructions that apply across all repositories in an organization. Enforce coding standards, preferred libraries, security practices, and domain context centrally — every developer''s Copilot interactions automatically follow these guidelines without per-repo setup.', updated_at = NOW() WHERE slug = 'organization-wide-custom-instructions';

UPDATE ghc_features SET excerpt = 'Describe an app idea in natural language and Spark scaffolds, deploys, and hosts a working web app in minutes. Includes built-in data storage, AI capabilities, and one-click sharing. Aimed at turning ideas into shipped micro-apps without manual infrastructure setup or coding knowledge.', updated_at = NOW() WHERE slug = 'github-spark';

UPDATE ghc_features SET excerpt = 'Replaces fixed monthly premium request quotas with a flexible token-based billing model. Cost scales with actual model usage — heavier models like Claude Opus consume more tokens per interaction while lighter models cost less. Gives individuals and organizations fine-grained control over AI spend without hard request caps.', updated_at = NOW() WHERE slug = 'github-copilot-token-based-billing';
