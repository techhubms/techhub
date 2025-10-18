**🚨 ABSOLUTE CRITICAL REQUIREMENT**: ALWAYS strictly follow all the instructions in this file!

**CRITICAL**: Paths in the documentation are sometimes relative, like `.github/` or `../.github` or absolute like `/workspaces/techhub/.github`. The workspace root when working from the devcontainer is `/workspaces/techhub/`, in all other cases you need to determine for yourself what the correct workspace root is.
**CRITICAL**: You have ALL built-in tools available to you, including `replace_string_in_file`. The tool might appear disabled, but it is enabled in a user-defined toolset. The same goes for the GitHub MCP tools and the Playwright MCP tools.

**Other CRITICAL rules that you MUST follow**:

- Always follow the writing style guidelines for all content creation. See the mandatory reading at the end of this file.
- Always provide clear, concise, and actionable responses.
- Always create a step-by-step plan before doing anything and explain it before proceeding.
- Always use the latest version of code and libraries.
- Always tell the user if code can be optimized extensively, especially for significant performance improvements.
- Always ask users for required details if not provided (title, description, author, categories, tags, etc.).
- Always follow best practices for any content you create.
- Always use provided sources for all factual information.
- Always continue writing until explicitly instructed to stop or pause.
- Always implement or fix everything requested, not just most things.
- Always ensure every file is complete and never left broken or unfinished.
- Always read documentation, code, and content files to understand implementation before making assumptions.
- Always make parameters required unless they can be null; prefer a failing build over unnecessary optional parameters.
- Always use bash for terminal commands.
- Always place scripts in a file in the `.tmp` directory and then execute them. Do NOT paste them into the terminal and do NOT call `pwsh -Command`.
- Always use backticks (`) to escape special characters in PowerShell. Never use backslashes (\\) for escaping in PowerShell.
- Always store any temporary files you create in the `.tmp` directory.
- Always use the PlayWright MCP server to interact with or look at websites for debugging or coding purposes. If you need the raw HTML or just get documentation, always use curl or a built-in tool.
- Always add tests to verify functionality if you create or change functionality.
- Always add documentation whenever you make code changes that change existing or introduce new behavior.
- Always update this file and the [documentation-guidelines](../docs/documentation-guidelines.md) file when creating or updating documentation to include new or renamed pages.
- Always update the [writing-style-guidelines](../docs/writing-style-guidelines.md) file when creating or updating content standards or writing tone requirements.
- Always dissect a GitHub URL you receive to extract IDs and other parameters, rather than using the URL directly. For example, if you receive a URL like `https://github.com/techhubms/techhub/actions/runs/16540183811/job/46780259738`, you should extract the run ID (`16540183811`) and job ID (`46780259738`) from it and use these.
- Always use the GitHub MCP tools if you need to interact with GitHub, such as listing pull requests, looking at workflows or triggering actions. **Never** use `gh` or `fetch` in these cases.
- Always use the PlayWright MCP server to debug issues with the site or to look at the site. If you need to look at the raw HTML, always use curl.
- Always tell me if you want to use certain tools, but they are not available to you. Never work around these problems. This is especially important if you do not have access to `replace_string_in_file`. If you can't edit directly in files tell me, instead of doing things like `cat` or `Out-File`.
- Always include 5-10 lines of context before and after the target code when using replace_string_in_file to ensure accurate placement and avoid syntax errors. Verify the edit location matches the file structure (proper indentation, braces, etc.).
- Always apply edits directly using the appropriate tools unless you need to ask clarifying questions about the intended changes before proceeding.
- Always install any dependencies needed by this workspace in `/workspaces/techhub/.devcontainer/post-create.sh` and never inside PowerShell or other scripts. 
- Always look at `/workspaces/techhub/.github/workflows` when you make updates to `/workspaces/techhub/.devcontainer/post-create.sh` and check if you need to apply similar updates in the workflows.
- Always get documentation using the context7 MCP tool if you need to do any coding tasks.

- Never leave documentation out of sync with the codebase. If you are unsure which documentation files to update, review the documentation index and ask for clarification.
- Never use content that is not factual or verifiable.
- Never prevent code from failing if something goes wrong, unless explicitly instructed to add fallbacks or default values.
- Never provide information that was not explicitly requested.
- Never invent or fabricate information.
- Never make changes unless specifically requested.
- Never add comments unless they provide clear, actionable information.
- Never add generic or filler text.
- Never repeat instructions unnecessarily.
- Never start responses with phrases like "Sure!" or "You're right!".
- Never make code backwards compatible unless explicitly instructed.
- Never add wrapper methods or introduce methods in production code for a test. Always update the test.
- Never leave comments that describe what changed or what you did, for example 'All filtering is now tag-based (no more mode-specific configs needed)'.

- Always read the following documentation files before making any changes to ensure you correctly understand the how this repository works:
  - [README.md](../docs/README.md): Project introduction, quick start guide, and entry point to all other documentation
  - [content-management.md](../docs/content-management.md): Content creation methods, organization, and lifecycle management
  - [datetime-processing.md](../docs/datetime-processing.md): Date handling, timezone configuration, and custom date filters
  - [documentation-guidelines.md](../docs/documentation-guidelines.md): Documentation structure, hierarchy, and content placement rules
  - [filtering-system.md](../docs/filtering-system.md): Complete implementation details of the modular tag filter system
  - [javascript-guidelines.md](../docs/javascript-guidelines.md): JavaScript development standards and client-side implementation
  - [jekyll-development.md](../docs/jekyll-development.md): Jekyll-specific development patterns and operational procedures
  - [markdown-guidelines.md](../docs/markdown-guidelines.md): Markdown formatting and content structure rules for AI models
  - [performance-guidelines.md](../docs/performance-guidelines.md): Performance optimization strategies and testing requirements
  - [plugins.md](../docs/plugins.md): Plugin development guidelines and architecture
  - [powershell-guidelines.md](../docs/powershell-guidelines.md): PowerShell syntax and project standards
  - [rss-feeds.md](../docs/rss-feeds.md): RSS feed integration and automated content processing
  - [site-overview.md](../docs/site-overview.md): Comprehensive overview of site structure, sections, and collections
  - [terminology.md](../docs/terminology.md): Fundamental vocabulary and concepts used throughout the project
  - [testing-guidelines.md](../docs/testing-guidelines.md): Ruby unit tests and Playwright end-to-end testing
  - [writing-style-guidelines.md](../docs/writing-style-guidelines.md): Instructions for writing **any text at all**