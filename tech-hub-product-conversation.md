# Tech Hub - Product Discovery Conversation

**Date:** Early 2025  
**Participants:**
- **Matthijs** - Product Owner
- **Reinier** - Developer

---

## Session 1: Initial Pitch

**Matthijs:** Hey Reinier, I've been thinking about this problem we keep running into. Every week, I spend hours manually curating Microsoft tech content from dozens of different sources - blogs, YouTube channels, RSS feeds, Reddit posts. And I know Rob and others on the team do the same. We're all looking at the same DevBlogs, the same GitHub announcements, the same community discussions. It's incredibly inefficient.

**Reinier:** Yeah, I've noticed that too. Everyone's got their own bookmarks and their own process for staying current.

**Matthijs:** Exactly! So here's my idea: what if we built a centralized content hub that automatically aggregates all this Microsoft tech content into one place? I'm calling it the "Tech Hub." It would pull from RSS feeds, process content with AI to categorize it properly, and present everything in a clean, filterable interface.

**Reinier:** Interesting. So like a smart news aggregator specifically for Microsoft technologies?

**Matthijs:** Yes, but with more structure. I'm thinking we organize content into distinct sections - AI, GitHub Copilot, Azure, .NET, DevOps, Security, Machine Learning. Each section would have different content types: news, blogs, videos, community posts, events. And everything would be automatically tagged and categorized.

**Reinier:** That's ambitious. Let me understand the scope better. When you say "automatically," you mean the entire pipeline from RSS feed to published content happens without manual intervention?

**Matthijs:** Exactly! Hourly, ideally. GitHub Actions runs a workflow that downloads RSS feeds, fetches the actual content from each URL - not just the RSS metadata - uses AI to analyze and categorize it, generates markdown files with proper frontmatter, enhances the tags, and commits everything to the repository.

**Reinier:** Wait, you want to fetch the full content from URLs? That's more complex than just parsing RSS feeds.

**Matthijs:** Yes, because RSS feeds often have truncated content or poor descriptions. We need the real article text for proper AI analysis. For Reddit posts, we'd need to handle JavaScript rendering. For YouTube, we extract metadata. For other URLs, standard HTTP fetching with rate limiting.

**Reinier:** Okay, so that's a multi-phase processing pipeline. What about the AI categorization? What exactly should it do?

**Matthijs:** The AI needs to be smart about this. We have specific rules - like, if something mentions GitHub Copilot, it should be tagged with both "GitHub Copilot" AND "AI" categories. There should be exclusion rules too - no biographical content, no help-seeking questions, no sales pitches, no job postings. And it needs to generate a proper excerpt, extract relevant tags, and create a well-formatted summary.

**Reinier:** So the AI isn't just categorizing - it's creating the actual content for the site?

**Matthijs:** Right. It takes the fetched content and generates markdown files with frontmatter following our standards. Things like proper date formatting in Europe/Brussels timezone, canonical URLs without query parameters, proper tag arrays, normalized tags for programmatic use.

**Reinier:** You mentioned multiple AI providers?

**Matthijs:** Yeah, I want flexibility. By default, use GitHub Models with their API, but also support Azure AI Foundry for enterprises that need enhanced compliance and security. Same interface, different backends.

**Reinier:** Okay, that's the automation side. What about the user-facing site? You mentioned filtering?

**Matthijs:** This is critical. The site needs a sophisticated filtering system. Users should be able to filter by date ranges - not just "last 30 days" but configurable options like "last 2 days," "last 7 days," "last 90 days," etc. And tag-based filtering where multiple tags work with AND logic, not OR.

**Reinier:** AND logic? So selecting "Azure" and "DevOps" only shows content that has both tags?

**Matthijs:** Exactly. It progressively narrows results. And here's a key feature: tag subset matching. If someone selects "AI," it should also show content tagged with "Generative AI," "Azure AI," "AI Agents" - anything where "AI" appears as a complete word in the tag.

**Reinier:** That's clever. What about performance with all this filtering?

**Matthijs:** That's why we need server-side optimization. I'm thinking a "20 + Same-Day" rule - show the 20 most recent posts, plus any additional posts from the same day as the 20th post. This ensures users never miss content from today while keeping the initial load manageable.

**Reinier:** And the client-side filtering operates on this limited dataset?

**Matthijs:** Exactly. Plus a 7-day recency filter - automatically exclude anything older than 7 days before even applying the 20+ rule. This keeps content fresh and performance snappy.

**Reinier:** Makes sense. What about the technology stack? Static site or dynamic?

**Matthijs:** Static site generated with Jekyll. I want it fast, SEO-friendly, and easy to host on Azure Static Web Apps. All the dynamic filtering happens client-side with JavaScript after the server renders the complete HTML.

**Reinier:** Why Jekyll specifically?

**Matthijs:** Ruby ecosystem, powerful templating with Liquid, excellent plugin system, and it's what GitHub Pages uses so there's good support. Plus we can write custom plugins in Ruby for complex logic.

**Reinier:** Alright, so Jekyll generates the static site, but we need custom plugins. What kind of plugins?

**Matthijs:** Several key ones. A section pages generator that reads a JSON configuration file and automatically creates all the section index pages and collection pages. Date filters for timezone-aware date processing - everything in Europe/Brussels timezone. Tag filters for the sophisticated filtering system with pre-calculated tag relationships. And utility plugins for things like YouTube embeds and string processing.

**Reinier:** The tag relationships being pre-calculated is smart for performance. So the server generates JavaScript data structures that the client uses for instant filtering?

**Matthijs:** Exactly! No API calls, no delays. The filtering should feel instantaneous.

**Reinier:** What about the content structure? You mentioned sections and collections. How does that hierarchy work?

**Matthijs:** Think of it like this: sections are topical areas - AI, GitHub Copilot, Azure, etc. Collections are content types - news, blogs, videos, community, events. Each section can have multiple collections. For example, the GitHub Copilot section has news, blogs, videos, community, AND custom pages like a features showcase and a "Levels of Enlightenment" learning guide.

**Reinier:** Custom pages alongside generated ones?

**Matthijs:** Yes. Some pages are auto-generated from the configuration, others are manually created for special purposes. The configuration file marks which is which with a "custom" flag.

**Reinier:** And this configuration file is the single source of truth?

**Matthijs:** Absolutely. `_data/sections.json` defines everything - sections, their URLs, their collections, descriptions, background images. Adding a new section means editing one JSON file, and the plugins handle the rest.

**Reinier:** I like that approach. What about mobile and responsive design?

**Matthijs:** Critical. Touch-friendly filters, responsive layouts, no dependency on hover states. The filtering system needs to work perfectly on mobile - large touch targets, collapsible tag lists, smooth interactions.

**Reinier:** Speaking of tag lists, how do we handle hundreds of tags without overwhelming the UI?

**Matthijs:** Good question. Collapsed view shows 30 tags, expanded view shows up to 100, all alphabetically sorted. Tags are dynamically selected based on relevance and count. There's a "More" button to expand, "Less" to collapse.

**Reinier:** What about search? Can users search content beyond filtering?

**Matthijs:** Yes! Real-time text search with debounced input. As users type, it searches across titles, descriptions, metadata, and tags. Search works alongside date and tag filters - everything is AND logic. So you can filter to "last 7 days" + "Azure" tag + search for "containers."

**Reinier:** That's powerful. What about URL state management?

**Matthijs:** Essential. All filter states - date filters, tag selections, search queries - should be preserved in URL parameters. This enables bookmarking and sharing specific filtered views. When someone shares a URL with `?filters=azure,copilot&search=vscode`, it should restore exactly that state.

**Reinier:** Smart. Now let's talk about the RSS workflow. You mentioned hourly processing. Walk me through the complete flow.

**Matthijs:** Okay, here's the workflow. Every hour, GitHub Actions triggers. It runs on a separate `rss-updates` branch, not `main`. First, it syncs with the latest `main` branch using a selective backup/restore strategy to avoid merge conflicts. Then it downloads RSS feeds from our configured sources - could be 20+ feeds from Microsoft blogs, dev blogs, GitHub, community sources.

**Reinier:** And then the per-entry content fetching?

**Matthijs:** Right. For each RSS entry, we check if it's already been processed by looking at JSON tracking files. If it's new, we categorize by URL type. Reddit URLs get batched together and processed with Playwright for JavaScript rendering. YouTube videos get metadata extraction. Other URLs get individual HTTP requests with 10-second rate limiting to be respectful to source sites.

**Reinier:** Then AI analysis?

**Matthijs:** Yes. Each entry's content goes to the AI - either GitHub Models or Azure AI Foundry. The AI applies our comprehensive categorization rules, generates the excerpt, extracts tags, creates the summary, and provides reasoning for its decisions. This outputs JSON that we convert to markdown with proper frontmatter.

**Reinier:** And after markdown generation?

**Matthijs:** PowerShell scripts enhance the tags - normalizing variations, adding Microsoft technology tags, ensuring consistency. Then we repair any markdown formatting issues with another script. Everything gets committed to the `rss-updates` branch.

**Reinier:** When does content move to `main`?

**Matthijs:** Only when there are actual new or modified markdown files to publish. If the RSS processing only updated tracking JSON without creating content, it stays on `rss-updates`. This keeps `main` clean and ensures it only reflects published content.

**Reinier:** And deployment?

**Matthijs:** When `main` gets new content, the RSS workflow explicitly triggers the Azure Static Web App deployment workflow via workflow_dispatch. This bypasses GitHub's restrictions on workflow chains and ensures new content goes live immediately - fully automated from RSS to production.

**Reinier:** Nice. What about testing? This is a complex system.

**Matthijs:** Multi-tier testing strategy following the testing pyramid. PowerShell unit tests with Pester for tag processing and content manipulation. JavaScript unit tests with Jest for client-side filtering logic. Ruby integration tests with RSpec for Jekyll plugins. And Playwright end-to-end tests for complete user workflows in real browsers.

**Reinier:** That's comprehensive. How do the tests run?

**Matthijs:** We have wrapper scripts for each test suite - `run-powershell-tests.ps1`, `run-javascript-tests.ps1`, `run-plugin-tests.ps1`, `run-e2e-tests.ps1`. Plus a `run-all-tests.ps1` that executes everything in the optimal order. Fast tests first, then slower integration tests, finally E2E.

**Reinier:** What about the development workflow? How do developers work locally?

**Matthijs:** Dev container setup with VS Code. Everything pre-configured - Ruby, Jekyll, Node.js, PowerShell, all dependencies. Developers open the repo in VS Code, reopen in container, run `jekyll-start.ps1`, and they're up and running at localhost:4000.

**Reinier:** Jekyll start script?

**Matthijs:** Yeah, PowerShell script that handles stopping any existing Jekyll processes, cleaning caches, and starting Jekyll with proper flags - livereload, force polling, incremental builds off for consistency, host set to 0.0.0.0 for container access.

**Reinier:** What about debugging?

**Matthijs:** Multiple approaches. For quick debugging, there's a build-only mode that skips serving - fastest for inspecting generated HTML. For tests, there are detailed output modes, coverage analysis, and specific test file targeting. For Playwright tests, headed mode with slowMo for watching test execution.

**Reinier:** Let me ask about the filtering system architecture. You mentioned three filter modes?

**Matthijs:** Yes! Root index has date filters plus section tag filters - so users can filter to "AI" or "GitHub Copilot" sections. Section indexes have date filters plus collection tag filters - "News," "Videos," "Community," etc. Collection pages have date filters plus content tag filters - the actual technology tags like "Azure," "Visual Studio Code," "Machine Learning."

**Reinier:** All implemented as tags?

**Matthijs:** Exactly. Unified tag-based architecture. Sections are tags, collections are tags, content tags are tags. Same subset matching logic, same client-side processing, same URL parameter handling. This dramatically simplifies the code.

**Reinier:** How does the date filtering work with timezones?

**Matthijs:** Server-side dates are always Europe/Brussels timezone. All epoch timestamp calculations use Brussels time for consistency. But client-side, we use the visitor's local timezone for better UX. So "Today" means today for them, not today in Brussels.

**Reinier:** That's user-friendly. What about the "Today (0)" button issue?

**Matthijs:** Ah yes. Date filter buttons should only be hidden if there are truly zero posts for that date range in the user's timezone. If the count is zero due to active tag filters, the button stays visible with (0) so users understand the effect of their selections.

**Reinier:** What about special content types? You mentioned GitHub Copilot features?

**Matthijs:** Right! There's a special subfolder `_videos/ghc-features/` for GitHub Copilot feature demonstration videos. These have special frontmatter with subscription plan tiers, GHES support flags, and an `alt-collection` field that makes them highlight the Features tab instead of Videos tab.

**Reinier:** And these automatically populate a features page?

**Matthijs:** Yes. The `/github-copilot/features.md` page dynamically pulls from that subfolder and organizes features by subscription tier - Free, Pro, Business, Enterprise. Videos with future dates are shown but not clickable - that's how we distinguish features that have demo videos from those that don't yet.

**Reinier:** Clever. What about Visual Studio Code updates?

**Matthijs:** Similar approach. `_videos/vscode-updates/` subfolder with `alt-collection: "vscode-updates"`. These appear on `/github-copilot/vscode-updates.html` with the latest video featured prominently.

**Reinier:** Let me ask about RSS feed configuration. How do we add new feeds?

**Matthijs:** Two ways. There's a `/new-rss-feeds` GitHub Copilot command that makes it easy. Or manually edit `_data/rss-feeds.json` - add an object with name, URL, output directory, category, and enabled flag.

**Reinier:** What RSS feed sources are we starting with?

**Matthijs:** All the major Microsoft blogs - AI Blog, Dev Blog, Visual Studio Blog, Azure Blog. GitHub's blog. Specific feeds like the Copilot tag feed. Community sources like certain Reddit communities, tech YouTubers covering Microsoft tech, prominent Microsoft MVPs. We're probably looking at 20-30 feeds initially.

**Reinier:** That's a lot of content. Storage concerns?

**Matthijs:** Git repository can handle it. Markdown files are small, and we're only keeping the last 7 days visible in the limited dataset. Older content remains in the repo for SEO and archives but doesn't impact performance.

**Reinier:** What about content quality control?

**Matthijs:** The AI has exclusion rules built in. Biographical content, question-only posts, very short posts under 200 words, sales pitches, job postings, very negative content - all automatically filtered out. The AI also assigns a quality score, and we can set a threshold.

**Reinier:** Can content be manually added?

**Matthijs:** Absolutely. There's a `/new-article` GitHub Copilot command. You provide a URL and content type, and it extracts the content, generates proper frontmatter and structure, creates the markdown file in the right location, applies formatting rules.

**Reinier:** What about publishing manually created content?

**Matthijs:** `/pushall` command. It stages changes, creates a descriptive commit message, handles branch protection and rebasing, pushes to remote, optionally creates a PR.

**Reinier:** Let's talk about performance requirements. What's the target?

**Matthijs:** Initial page load under 2 seconds on 3G. Filtering should feel instant - under 100ms. The "20 + Same-Day" rule with 7-day recency keeps initial dataset small enough for this. All JavaScript operations should be sub-100ms except the initial index building which can take 300-500ms on page load.

**Reinier:** How do we achieve that with potentially thousands of tags?

**Matthijs:** Pre-calculation is key. Server-side Ruby plugins calculate all tag relationships during build - which posts match which tags, what tags appear together, subset matching indices. This generates JavaScript data structures that client code uses for O(1) lookups instead of O(n) filtering.

**Reinier:** Binary search for date filtering?

**Matthijs:** Exactly! Pre-sorted epoch timestamps with binary search instead of linear scanning. Multi-core parallel processing during build for large datasets. String operation caching, normalization caching, date calculation caching - multiple cache layers in the Ruby plugins.

**Reinier:** What about the content limiting rule implementation?

**Matthijs:** Custom Liquid filter `limit_with_same_day` in `_plugins/date_filters.rb`. It's collection-aware - groups items by collection first, applies the configurable limit per collection (default 20), includes same-day items, then merges results sorted by date. This ensures fair representation across collections.

**Reinier:** Why collection-aware?

**Matthijs:** Without it, if the News collection had 50 recent items and Videos had 5, the 20-item limit would only show News. Collection-aware limiting gives each collection a fair chance to appear.

**Reinier:** Makes sense. What about error handling in the RSS workflow?

**Matthijs:** Comprehensive logging at every step. Graceful degradation - if content fetching fails for one item, continue with others. If AI analysis fails, fall back to RSS metadata. Reddit batch processing continues even if individual items fail. Invalid JSON gets validated before storage. Rate limiting prevents overwhelming source sites.

**Reinier:** What if feeds are unavailable?

**Matthijs:** Retry logic with exponential backoff. Temporary outages get automatically retried. Persistent failures get logged but don't stop the entire workflow. Each feed is processed independently.

**Reinier:** Monitoring and observability?

**Matthijs:** GitHub Actions dashboard shows workflow status and history. Success rates tracked per feed. Processing times monitored. Memory usage checked for leaks. We can see exactly which feeds succeed, which fail, how long each step takes.

**Reinier:** What about the documentation approach?

**Matthijs:** Comprehensive docs in the `/docs` folder. There's a documentation guidelines file that defines hierarchy and content placement rules. Foundation docs first - terminology, site overview. Then functionality docs - filtering system, datetime processing, content management. Then task-oriented docs - Jekyll development, plugin development. Then language-specific guidelines - JavaScript, PowerShell, Ruby, Markdown. Finally performance and testing guidelines.

**Reinier:** That's well organized. What about AI instructions?

**Matthijs:** Critical! There's a `.github/copilot-instructions.md` file with absolute requirements for AI models working on this repo. Things like always following writing style guidelines, never duplicating information, always reading documentation before changes, always adding tests, always updating docs. It's quite extensive.

**Reinier:** Writing style guidelines?

**Matthijs:** Yeah, in `docs/writing-style-guidelines.md`. Down-to-earth tone, avoid buzzwords and marketing speak, no exaggerated language like "revolutionary" or "game-changing." Specific examples good, vague language bad. Clear professional English, active voice preferred.

**Reinier:** Why is this so important?

**Matthijs:** Content quality. We're aggregating from many sources, and we generate excerpts and summaries with AI. Consistent, authentic language ensures users trust the content and don't feel like they're reading marketing fluff.

**Reinier:** Fair enough. Let me ask about the infrastructure. You mentioned Azure Static Web Apps?

**Matthijs:** Yes. The repo has infrastructure as code in the `/infra` folder. We can deploy the Azure resources with scripts. Static Web App provides CDN, SSL, custom domains, authentication if needed, and integrates perfectly with GitHub Actions for automatic deployment.

**Reinier:** What about analytics?

**Matthijs:** Google Analytics configured in `_config.yml`. Tracks page views, popular content, user journeys through filters. We can see which sections get the most traffic, which filters are most used.

**Reinier:** Let's talk about the roundups feature. What's that?

**Matthijs:** Weekly curated content summaries. There's a special `_roundups` collection that appears on the homepage but not in sections. These are manually or semi-automatically created compilations of the week's best content, organized by theme or topic.

**Reinier:** Semi-automatically?

**Matthijs:** There's an `iterative-roundup-generation.ps1` script that can help generate roundups using AI. It looks at the week's content, groups by theme, writes summaries. But someone reviews and publishes it.

**Reinier:** What about magazines? I saw references to XPRT Magazine.

**Matthijs:** That's community content. Historical XPRT Magazine issues are in the `_community` collection. These are bi-annual publications from Xebia, covering Microsoft tech topics. They're preserved for reference and searchable like other content.

**Reinier:** Okay, let me ask about edge cases. What happens if someone manually deletes a file that was RSS-generated?

**Matthijs:** The RSS workflow tracks processed entries in JSON files. If a file is deleted from main, the next RSS run won't restore it unless that entry appears again in the RSS feed. The selective sync strategy respects deletions in main.

**Reinier:** What about duplicate content detection?

**Matthijs:** Multiple layers. URL tracking prevents processing the same article twice. Content similarity detection catches duplicates with different URLs. File existence checks before creation. The `skipped-entries.json` file tracks items that were skipped and why.

**Reinier:** What if a Reddit post gets deleted after we've processed it?

**Matthijs:** There's a `remove-deleted-reddit-posts.ps1` script that can clean those up. It checks Reddit URLs and removes content for posts that no longer exist.

**Reinier:** What about the formatting repair script?

**Matthijs:** `fix-markdown-files.ps1` automatically fixes common issues - date format standardization, frontmatter quoting consistency, tag array formatting, filename and permalink consistency, duplicate frontmatter keys, whitespace cleanup. It can process all files or target specific ones.

**Reinier:** Can I run that locally during development?

**Matthijs:** Yes, that's the idea. If you create content manually and the formatting isn't perfect, run the script and it'll fix everything to match standards.

**Reinier:** What about the tag enhancement process specifically?

**Matthijs:** PowerShell functions in `scripts/functions/` directory. `Get-FilteredTags` normalizes common tag variations, adds related technology tags based on content analysis. For example, if content mentions "VS Code," it adds "Visual Studio Code" tag. If it mentions "Copilot," it adds both "GitHub Copilot" and "AI."

**Reinier:** This runs after AI processing?

**Matthijs:** Yes. AI generates initial tags, then PowerShell enhancement adds missing obvious tags and normalizes formatting. This two-stage approach combines AI semantic understanding with rule-based normalization.

**Reinier:** Let's talk about the plugin architecture. You mentioned section pages generator. How does that work?

**Matthijs:** It's a Jekyll Generator plugin with highest priority. During Jekyll build, it reads `_data/sections.json`, then programmatically creates PageWithoutAFile objects for each section index and collection page marked as non-custom. These become part of the site.pages collection as if they were physical files.

**Reinier:** So the pages are completely dynamic?

**Matthijs:** The structure is dynamic, the content comes from includes. The generator creates pages with frontmatter and includes the appropriate templates. The templates use Liquid to render sections, collections, filters, content lists - all dynamically based on the configuration.

**Reinier:** What about the date filters plugin?

**Matthijs:** Provides custom Liquid filters for date operations. `date_to_epoch` converts dates to Unix timestamps in Brussels timezone. `now_epoch` gets current timestamp. `normalize_date_format` handles timezone abbreviations. `limit_with_same_day` applies the content limiting rule. All using shared utilities from `date_utils.rb` for consistency.

**Reinier:** And the tag filters plugin?

**Matthijs:** The most complex one. `generate_all_filters` is the main entry point. It processes all items, builds tag relationships with subset matching, calculates tag counts, optimizes date filtering with binary search, generates date filter configurations, and returns complete filter data for JavaScript.

**Reinier:** This is where the pre-calculation happens?

**Matthijs:** Exactly. It creates `window.tagRelationships` with post indices for each tag, `window.dateFilterMappings` with post indices for each date filter, and tag metadata with counts and display names. All generated server-side during build.

**Reinier:** What about the YouTube plugin?

**Matthijs:** Simple Liquid tag. `{% youtube VIDEO_ID %}` generates a responsive iframe with proper lazy loading, privacy-respecting youtube-nocookie.com embed, and all the necessary attributes for security and performance.

**Reinier:** Let's discuss the testing strategy more. You mentioned four tiers. Walk me through what each tests.

**Matthijs:** Phase 2 is PowerShell unit tests - they test tag normalization algorithms, content cleaning functions, frontmatter extraction, string manipulation in isolation. No file I/O, just logic. These are fast, run first, provide immediate feedback on preprocessing correctness.

**Reinier:** Phase 4?

**Matthijs:** JavaScript unit tests with Jest and jsdom. Test client-side filtering logic without browser dependencies - filter state management, tag relationships, date calculations, URL parameter handling, text search logic. Mock DOM elements, focus on pure logic. Also fast, run early.

**Reinier:** Phase 3?

**Matthijs:** Ruby integration tests with RSpec. Test Jekyll plugins actually work with Jekyll - Liquid filters behave correctly, generators create proper pages, data files are processed right, server-side transformations happen as expected. Slower because they involve Jekyll, run after unit tests.

**Reinier:** Phase 5?

**Matthijs:** Playwright end-to-end tests. Real browser, real Jekyll-generated HTML, complete user workflows. Test filtering interactions, mobile responsive behavior, cross-browser compatibility, performance, accessibility. Slowest tests, run last, validate the complete system.

**Reinier:** How do you keep E2E tests fast enough?

**Matthijs:** Parallelization where possible. Target specific scenarios instead of testing every combination. Use fixtures for consistent test data. Start Jekyll once and reuse the server for multiple tests. Tests have max failure limits to stop early if something's fundamentally broken.

**Reinier:** What about test data management?

**Matthijs:** Prefer in-memory test data over external files. For PowerShell tests, create markdown strings in the test. For JavaScript tests, mock factories provide consistent data. For Ruby tests, minimal fixtures. For Playwright, use actual Jekyll-generated content when possible for realistic testing.

**Reinier:** What does a typical development session look like?

**Matthijs:** Open repo in VS Code, reopen in dev container. Run `jekyll-start.ps1` to start the site. Make changes to code. Jekyll auto-regenerates with livereload enabled, browser refreshes automatically. For plugin changes, stop and restart Jekyll. Run relevant test suite - `run-powershell-tests.ps1` for PowerShell changes, `run-javascript-tests.ps1` for JS changes, etc.

**Reinier:** When do you run all tests?

**Matthijs:** Before committing, run `run-all-tests.ps1` to ensure everything works. CI runs all tests on PR. If tests fail, the PR can't merge. This ensures main branch always has passing tests.

**Reinier:** What about the PR validation workflow?

**Matthijs:** GitHub Actions workflow that runs on every PR. Executes all test suites in order, builds the Jekyll site to ensure it generates correctly, checks for errors in the build output, validates links and references. Only PRs that pass can merge.

**Reinier:** What other workflows do we have?

**Matthijs:** `download-and-process-rss-data.yml` runs hourly for RSS processing. `weekly-roundup-generation.yml` assists with roundup creation. `deploy-static-web-app.yml` handles Azure deployment. `deploy-infrastructure.yml` manages Azure resource provisioning.

**Reinier:** The deploy workflow is triggered automatically?

**Matthijs:** Yes, by the RSS workflow when content is published to main. Also on direct pushes to main and manual triggers. It builds the Jekyll site and deploys to Azure Static Web App with the Azure CLI.

**Reinier:** Let me ask about security. Any concerns with automated content processing?

**Matthijs:** Several mitigations. AI categorization filters malicious content. URLs are validated before fetching. Rate limiting prevents abuse. Content comes from trusted sources. Generated markdown is sanitized. GitHub token and Azure credentials are stored as secrets, never in code.

**Reinier:** What about costs?

**Matthijs:** GitHub Actions has generous free tier for public repos. Azure Static Web Apps free tier handles our traffic. AI API calls are the main cost - GitHub Models has rate limits but is free with GitHub account, Azure AI Foundry has per-request costs but we control the volume with feed configuration.

**Reinier:** How do we control volume?

**Matthijs:** `max_items` setting per feed limits how many entries are processed per run. We can disable feeds with the `enabled: false` flag. Hourly runs mean we process small batches frequently rather than large batches rarely.

**Reinier:** What about the client-side JavaScript architecture? You mentioned filters.js being unified. What does that mean?

**Matthijs:** Before there were mode-specific files. Now `assets/js/filters.js` handles all filtering modes - sections, collections, tags - with the same unified tag-based logic. It reads window.currentFilterData to determine the mode and configures accordingly. This dramatically reduced code duplication.

**Reinier:** What about sections.js?

**Matthijs:** That's the one exception. Only JavaScript file allowed to modify content on page load. It handles section collection activation based on URL parameters. This is necessary for proper navigation state restoration. All other JavaScript must wait for user interaction.

**Reinier:** Why that restriction?

**Matthijs:** Performance and SEO. Server-side rendering means content is immediately visible and crawlable. If JavaScript created initial content, there'd be a flash of empty page, worse SEO, and broken experience if JavaScript fails. Section state is architectural, so it's the exception.

**Reinier:** What about the text search implementation?

**Matthijs:** Pre-index all content on page load. Extract title, description, metadata, tags into a searchable string, lowercase it, cache in window.cachedPosts array. When user types, debounce input 300ms, then filter array with simple string.includes(). Super fast because it's pre-indexed and uses native string matching instead of regex.

**Reinier:** Clear button behavior?

**Matthijs:** Appears automatically when text is entered, hidden when empty. Provides instant clearing. Escape key also clears search. Search terms persist in URL with `?search=term` parameter so you can bookmark searches.

**Reinier:** How does search interact with other filters?

**Matthijs:** AND logic. If you select "Last 7 days" + "Azure" tag + search "functions," you get content that matches all three criteria. Filter counts update dynamically to show how many items match each filter given the current search and selections.

**Reinier:** That's sophisticated. What about accessibility?

**Matthijs:** Keyboard navigation works throughout. Screen reader support with proper ARIA labels. Logical heading structure. Descriptive link text. Touch targets sized appropriately. No hover-dependent interactions. High contrast support. Focus indicators visible.

**Reinier:** Mobile-specific features?

**Matthijs:** Touch-friendly filter buttons with large tap targets. Collapsible sections to save screen space. Responsive grid layouts that stack on mobile. Text search input optimized for mobile keyboards. No horizontal scrolling. Smooth scrolling and transitions.

**Reinier:** What about very old browsers?

**Matthijs:** Progressive enhancement. Core content loads even without JavaScript. Filters gracefully degrade. Modern browsers get the full experience, older ones get functional but simpler experience. No polyfills that bloat the bundle.

**Reinier:** Let's talk about content types. What's in each collection?

**Matthijs:** News collection has official announcements and product updates from Microsoft blogs. Videos collection has YouTube content and tutorials. Community collection has Microsoft Tech community posts, Reddit discussions, Q&A content. Events collection has conferences, meetups, webinars. Posts collection has blog posts and long-form articles. Roundups are curated summaries.

**Reinier:** How do we determine which collection something goes in?

**Matthijs:** The RSS feed configuration specifies `output_dir` - like `_news` or `_posts`. For manually added content, the creator chooses based on content type. Community content might be Reddit or Microsoft Tech Community. Events are typically calendar-based. Videos have `youtube_id` in frontmatter.

**Reinier:** What about content that fits multiple collections?

**Matthijs:** Content only goes in one collection based on format, but it can have multiple category tags. A video about Azure DevOps would be in the Videos collection but tagged with both "Azure" and "DevOps" categories, making it appear in both sections when users filter.

**Reinier:** So the categories determine visibility in sections, collections determine format?

**Matthijs:** Exactly! Separation of concerns. Collections = format (news, video, blog). Categories = topic (AI, Azure, DevOps). Tags = specific technologies (GitHub Copilot, Visual Studio Code, Azure Functions).

**Reinier:** What about the frontmatter field order? Is that standardized?

**Matthijs:** Yes, documented in markdown guidelines. Layout first, then title, description, author, excerpt separator, canonical URL, viewing mode, feed metadata, custom fields, date, permalink, and finally categories, tags, tags_normalized at the bottom. This reduces diff noise when the repair script processes files.

**Reinier:** Viewing mode?

**Matthijs:** Either "internal" for self-contained content displayed fully on site - like videos and roundups. Or "external" for content that links to original sources - like news, posts, community. This affects the layout and whether we show full content or excerpt plus link.

**Reinier:** What determines the permalink structure?

**Matthijs:** For `layout: "post"` files, the permalink is auto-generated from the filename as `/YYYY-MM-DD-Title-Slug.html`. For page layouts, it's set explicitly. The date in the filename must match the date in frontmatter.

**Reinier:** What about canonical URLs?

**Matthijs:** Always point to the original source. Remove query parameters to keep them clean. This helps with SEO and gives credit to the original authors. It's also required for copyright compliance.

**Reinier:** Speaking of copyright, how do we handle that?

**Matthijs:** We're aggregating and summarizing, not republishing full content without permission. Excerpts and summaries fall under fair use. Canonical URLs and author credits give proper attribution. For community content like magazine articles we have permission to host.

**Reinier:** What about RSS feeds we generate? I saw multiple XML files.

**Matthijs:** Each section has its own RSS feed - `ai.xml`, `github-copilot.xml`, `azure.xml`, etc. There's `feed.xml` for everything combined. And `roundups.xml` for the roundups collection. These are Jekyll-generated RSS feeds that people can subscribe to.

**Reinier:** So users can subscribe to just the sections they care about?

**Matthijs:** Exactly! Someone only interested in GitHub Copilot can subscribe to `github-copilot.xml` and ignore everything else. The feeds include proper metadata, publication dates, content, and links.

**Reinier:** Where are these RSS links in the UI?

**Matthijs:** Each section page has a subtle RSS link in the header area. The footer has a main "Subscribe via RSS" link pointing to the everything feed. Not prominently featured but available for power users who want it.

**Reinier:** What about robots.txt and sitemap?

**Matthijs:** Jekyll's sitemap plugin generates `sitemap.xml` automatically with all pages. We can configure robots.txt to control crawler behavior. SEO plugin adds proper meta tags to all pages.

**Reinier:** The SEO plugin?

**Matthijs:** `jekyll-seo-tag` provides Open Graph tags, Twitter cards, JSON-LD structured data, canonical URLs, meta descriptions. This makes content shareable on social media with proper previews and helps search engines understand the structure.

**Reinier:** What about Google Analytics?

**Matthijs:** Configured in `_config.yml` with the tracking ID. The theme includes the Analytics snippet. We can see traffic patterns, popular content, user flow through sections, which filters are used most.

**Reinier:** Privacy considerations?

**Matthijs:** Analytics is Google's responsibility. Our embeds use youtube-nocookie.com for privacy. We don't collect personal data. No cookies except Analytics. No user accounts or authentication needed for basic use.

**Reinier:** Could we add authentication later?

**Matthijs:** Azure Static Web Apps supports authentication with GitHub, Azure AD, Twitter, etc. If we wanted premium features or personalization, we could add auth and still keep the core site public and static.

**Reinier:** What about API access for the content?

**Matthijs:** Not initially planned, but the static site architecture makes it easy. We could generate JSON files during build with the same content structures and expose them as an API. Or use Azure Functions for dynamic API endpoints.

**Reinier:** Let me ask about future expansion. How hard is it to add a new section?

**Matthijs:** Trivial. Edit `_data/sections.json`, add a new section object with title, description, URL, category, collections array. Optionally add a custom background image. Commit and deploy. The plugins generate all the pages automatically.

**Reinier:** What about adding a new collection type?

**Matthijs:** Add to `_config.yml` collections section with `output: true`. Create the directory like `_newcollection/`. Update `_data/sections.json` to include this collection in relevant sections. That's it - the system handles the rest.

**Reinier:** What if we want a new filter type?

**Matthijs:** The unified tag architecture makes this easier now. New filter types are just new ways of generating tags. Add the logic to `tag_filters.rb` to generate the tag relationships, update the client JavaScript to handle the mode, and it works.

**Reinier:** What about internationalization? Different languages?

**Matthijs:** Not in initial scope, but Jekyll supports i18n. We'd need to duplicate content in multiple languages, add language switcher, configure Jekyll to generate language-specific paths. Content would need translation - could use AI for that.

**Reinier:** Accessibility testing?

**Matthijs:** Playwright E2E tests include accessibility checks. We can use tools like axe-core to scan pages for violations. Manual testing with screen readers. Keyboard-only navigation testing. Color contrast validation.

**Reinier:** Performance monitoring in production?

**Matthijs:** Azure Static Web Apps provides analytics on bandwidth and requests. Google Analytics shows page load times. We can add custom JavaScript to track filtering performance. Lighthouse CI for automated performance testing on every deploy.

**Reinier:** What about error tracking?

**Matthijs:** For the build process, GitHub Actions logs capture everything. For client-side JavaScript, we could add error tracking like Sentry. For RSS processing failures, detailed logs in workflow runs.

**Reinier:** Okay, let me summarize to make sure I understand. We're building a Jekyll-based static site that automatically aggregates Microsoft tech content from RSS feeds using AI categorization. The site is organized into sections and collections with a sophisticated tag-based filtering system. Everything is optimized for performance with server-side pre-calculation and client-side instant filtering. The entire pipeline from RSS to production is automated with GitHub Actions. Content can also be manually added via GitHub Copilot commands. Multi-tier testing ensures quality. Dev container makes local development easy. Is that right?

**Matthijs:** That's exactly right! And the key architectural principles are: configuration-driven for easy extension, static generation for performance and SEO, unified tag-based filtering for simplicity, comprehensive testing for reliability, and full automation for maintainability.

**Reinier:** What's the timeline for building this?

**Matthijs:** Initial MVP with basic sections, RSS processing, filtering, and manual content creation - maybe 6-8 weeks. Then iterative improvements - better AI categorization, more RSS feeds, additional features, performance optimization. It's the kind of project that can launch quickly and improve continuously.

**Reinier:** What about team size?

**Matthijs:** Could be done by one person but better with 2-3. One focused on Jekyll/Ruby plugins, one on JavaScript/frontend, one on PowerShell/automation. Or with good T-shaped skills, 2 people sharing responsibilities.

**Reinier:** Documentation while building?

**Matthijs:** Absolutely. Documentation-first approach. Write the docs as we design and build. The comprehensive docs we discussed aren't aspirational - they should exist before or alongside the code. This forces clear thinking about architecture and makes onboarding new contributors easy.

**Reinier:** Testing approach?

**Matthijs:** Test-driven where it makes sense. PowerShell functions definitely TDD. Jekyll plugins with tests before shipping. JavaScript unit tests early. E2E tests as features are complete. Fix tests immediately when they fail - don't accumulate test debt.

**Reinier:** Git workflow?

**Matthijs:** Main branch protected, requires PR and passing tests. Feature branches for development. RSS automation on `rss-updates` branch with selective sync to main. Clear commit messages following conventional commits. Squash merge to keep main history clean.

**Reinier:** Code review process?

**Matthijs:** All PRs reviewed before merge. Check for correctness, performance, test coverage, documentation updates. Use GitHub Copilot for initial reviews, then human review. No "LGTM" without actually reviewing.

**Reinier:** CI/CD pipeline?

**Matthijs:** PR validation runs all tests and builds. Merge to main triggers deployment to staging if we have one. Manual approval or automatic deployment to production. Rollback capability if something breaks. Monitor production after each deploy.

**Reinier:** Okay, I think I have a complete picture now. Let me think about the technical challenges and design decisions...

**Matthijs:** Take your time. This is a substantial project with a lot of moving parts. The good news is we can build incrementally - start with basic site structure and filtering, add RSS automation, enhance with AI, improve filtering sophistication, add features like search and special content types.

**Reinier:** Yeah, incremental makes sense. What's the absolute minimum viable product?

**Matthijs:** Jekyll site with manual content creation, basic section/collection structure, simple date filtering, RSS feeds generated for subscriptions. No automation, no AI, no sophisticated filtering yet. Just a well-structured content site.

**Matthijs:** From there, add RSS automation without AI - just pull feeds and create basic markdown. Then add AI categorization. Then sophisticated filtering. Then text search. Each step adds value independently.

**Reinier:** That's a good approach. What's the biggest technical risk?

**Matthijs:** Probably the AI categorization accuracy and cost. If the AI doesn't categorize well, we have a lot of miscategorized content. If the API costs are too high, it's not sustainable. We need to validate both early.

**Reinier:** How do we validate categorization?

**Matthijs:** Process a sample of 50-100 articles, manually review the results, measure accuracy. Iterate on the AI prompt - the system message and categorization rules - until accuracy is acceptable. 80%+ would be good, 90%+ would be great.

**Reinier:** And cost?

**Matthijs:** Track API usage and costs for a week of hourly processing. Calculate monthly costs. Compare GitHub Models (free but rate limited) vs Azure AI Foundry (paid but flexible). Choose based on cost and reliability.

**Reinier:** What about the filtering performance? How do we validate that?

**Matthijs:** Lighthouse performance testing. Chrome DevTools profiling. Test with 1000+ items in the dataset. Measure filter operation times. Aim for sub-100ms filtering even with large datasets. The pre-calculation approach should handle this, but we need to prove it.

**Reinier:** What about edge cases in the RSS processing?

**Matthijs:** Malformed RSS feeds, duplicate entries, feeds that go offline, rate limiting from source sites, content that changes after processing, deleted content, RSS feeds that don't follow standards. Robust error handling and logging for all of these.

**Reinier:** Content that changes after processing?

**Matthijs:** That's tricky. If someone publishes an article, we process it, then they update the article significantly, our version is stale. We'd need to track modification dates and reprocess changed content. Maybe a future enhancement.

**Reinier:** Deleted content?

**Matthijs:** Reddit posts specifically. We have the cleanup script. For other sources, if the URL returns 404, we could flag it for review. But we probably keep the content as an archive even if the source is gone.

**Reinier:** What about legal considerations? Terms of service for RSS feeds?

**Matthijs:** RSS feeds are inherently for syndication - that's their purpose. As long as we provide attribution, canonical URLs, and excerpts rather than full reproduction, we're in compliance. But we should review TOS for each source to be sure.

**Reinier:** What about Microsoft's stance on this kind of aggregation?

**Matthijs:** Microsoft encourages content sharing and attribution. DevBlogs specifically have RSS feeds for syndication. As long as we're not claiming authorship and we link back to originals, they're supportive of community initiatives like this.

**Reinier:** Okay, I'm feeling good about this. Let me start sketching out a technical architecture document and a development plan. I'll break it into phases and identify dependencies and risks.

**Matthijs:** Perfect! And remember, the goal is to make Microsoft tech content accessible in one place with excellent discoverability. Everything else serves that goal. If a feature doesn't help users find the content they need, it's not essential.

**Reinier:** Agreed. User experience first, technical elegance second. Though with good architecture, we can have both.

**Matthijs:** Exactly. I'm excited to see this come together!

---

## End of Conversation

**Status:** Reinier has a complete understanding of the Tech Hub project requirements and is ready to begin architecture and development planning. All major features, technical decisions, and implementation details have been discussed.
