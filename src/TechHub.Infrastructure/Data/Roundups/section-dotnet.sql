-- Section-specific roundup content: dotnet
-- Generated: 2026-05-19 11:07
-- Heading aliases matched: .NET, Coding
-- Updates backfilled duplicates so each roundup only contains dotnet content.
-- Usage: psql -U techhub -d techhub -f section-dotnet.sql

-- weekly-dotnet-roundup-2025-07-07  (2025-07-07)
UPDATE content_items
SET  content      = '.NET sees continued push toward frictionless, cross-platform development—condensing mobile/web convergence, AI enhancements, and rapid scripting into actionable productivity boosters.

<!--excerpt_end-->

## Coding

### Single-File Apps and Modern Scripting in .NET 10

.NET 10 Preview enables running full C# apps from a single `.cs` file using `dotnet run app.cs`. Andrew Lock’s analysis shows how to include in-file NuGet/SDK directives, leverage shebang for Unix-like environments, and notes forthcoming improvements such as multi-file support. This drastically lowers entry barriers for prototyping, sharing, and experimenting without the need for complex project setups.

- [Exploring Single-File .NET Apps with dotnet run app.cs in .NET 10 Preview](https://andrewlock.net/exploring-dotnet-10-preview-features-1-exploring-the-dotnet-run-app.cs/)

### Blazor, MAUI, and AI-Enhanced Mobile Experiences

Through the .NET MAUI standup, Beth Massi and David Ortinau demonstrate how Blazor web apps can be rapidly adapted for native mobile platforms with full device feature access, while Azure AI Foundry tooling streamlines embedding AI. Community-driven resources and best practices further empower web/mobile/AI hybrid programming.

- [.NET MAUI Community Standup - Blazor for Mobile with AI? Here''s how.](/ai/videos/net-maui-community-standup-blazor-for-mobile-with-ai-heres-how)',
     excerpt      = '.NET sees continued push toward frictionless, cross-platform development—condensing mobile/web convergence, AI enhancements, and rapid scripting into actionable productivity boosters.',
     content_hash = md5('.NET sees continued push toward frictionless, cross-platform development—condensing mobile/web convergence, AI enhancements, and rapid scripting into actionable productivity boosters.

<!--excerpt_end-->

## Coding

### Single-File Apps and Modern Scripting in .NET 10

.NET 10 Preview enables running full C# apps from a single `.cs` file using `dotnet run app.cs`. Andrew Lock’s analysis shows how to include in-file NuGet/SDK directives, leverage shebang for Unix-like environments, and notes forthcoming improvements such as multi-file support. This drastically lowers entry barriers for prototyping, sharing, and experimenting without the need for complex project setups.

- [Exploring Single-File .NET Apps with dotnet run app.cs in .NET 10 Preview](https://andrewlock.net/exploring-dotnet-10-preview-features-1-exploring-the-dotnet-run-app.cs/)

### Blazor, MAUI, and AI-Enhanced Mobile Experiences

Through the .NET MAUI standup, Beth Massi and David Ortinau demonstrate how Blazor web apps can be rapidly adapted for native mobile platforms with full device feature access, while Azure AI Foundry tooling streamlines embedding AI. Community-driven resources and best practices further empower web/mobile/AI hybrid programming.

- [.NET MAUI Community Standup - Blazor for Mobile with AI? Here''s how.](/ai/videos/net-maui-community-standup-blazor-for-mobile-with-ai-heres-how)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-07-07';

-- weekly-dotnet-roundup-2025-07-14  (2025-07-14)
UPDATE content_items
SET  content      = 'Tooling and best practices saw key advances for TypeScript, .NET, and MVVM, all supporting more productive, maintainable workflows.

<!--excerpt_end-->

## Coding

### TypeScript and .NET: Configuration, Tooling, and Performance

TypeScript 5.9 Beta streamlines `tsc --init` for rapid, default-rich starts. New features—ECMAScript ''import defer'', Node.js v20 stable modules, and enhanced DOM types—ease full-stack development. Editor refinements aid navigation and debugging, while type system optimizations yield quicker builds. .NET’s July releases fix vulnerabilities and boost reliability. Andrew Lock’s dive into .NET 10’s single-file run experience (`dotnet run app.cs`) modernizes scripting, supporting dependency management and build properties within C# for fast prototyping.

This extends last week’s coverage on .NET scripting innovations and dev productivity initiatives.

- [Announcing TypeScript 5.9 Beta: New Features, Improvements, and Optimizations](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9-beta/)
- [Behind the Scenes of dotnet run app.cs: Deep Dive into .NET 10 Single-File Run Experience](https://andrewlock.net/exploring-dotnet-10-preview-features-2-behind-the-scenes-of-dotnet-run-app.cs/)
- [.NET and .NET Framework July 2025 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-july-2025-servicing-updates/)

### Secure Dependency Management and MVVM Decoupling

A .NET-focused guide clarifies upgrade strategies: to remediate vulnerabilities, update NuGet packages at the project level, not just system runtimes. This is vital for accurate scanning and secure CI/CD. For WPF/MVVM, new advice details decoupling Views from ViewModels using ViewModelLocator patterns and modern DI, making UI logic more modular and testable.

- [Upgrading NuGet Packages vs. .NET Runtime: Addressing SCA Vulnerabilities in Microsoft.AspNetCore.*](https://techcommunity.microsoft.com/t5/net-runtime/do-i-need-to-upgrade-microsoft-aspnetcore-nuget-packages-after/m-p/4431436#M752)
- [Decoupling Views and ViewModels in CommunityToolkit.Mvvm](https://techcommunity.microsoft.com/t5/app-development/how-to-decouple-views-from-view-models-using-communitytoolkit/m-p/4432591#M1261)',
     excerpt      = 'Tooling and best practices saw key advances for TypeScript, .NET, and MVVM, all supporting more productive, maintainable workflows.',
     content_hash = md5('Tooling and best practices saw key advances for TypeScript, .NET, and MVVM, all supporting more productive, maintainable workflows.

<!--excerpt_end-->

## Coding

### TypeScript and .NET: Configuration, Tooling, and Performance

TypeScript 5.9 Beta streamlines `tsc --init` for rapid, default-rich starts. New features—ECMAScript ''import defer'', Node.js v20 stable modules, and enhanced DOM types—ease full-stack development. Editor refinements aid navigation and debugging, while type system optimizations yield quicker builds. .NET’s July releases fix vulnerabilities and boost reliability. Andrew Lock’s dive into .NET 10’s single-file run experience (`dotnet run app.cs`) modernizes scripting, supporting dependency management and build properties within C# for fast prototyping.

This extends last week’s coverage on .NET scripting innovations and dev productivity initiatives.

- [Announcing TypeScript 5.9 Beta: New Features, Improvements, and Optimizations](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9-beta/)
- [Behind the Scenes of dotnet run app.cs: Deep Dive into .NET 10 Single-File Run Experience](https://andrewlock.net/exploring-dotnet-10-preview-features-2-behind-the-scenes-of-dotnet-run-app.cs/)
- [.NET and .NET Framework July 2025 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-july-2025-servicing-updates/)

### Secure Dependency Management and MVVM Decoupling

A .NET-focused guide clarifies upgrade strategies: to remediate vulnerabilities, update NuGet packages at the project level, not just system runtimes. This is vital for accurate scanning and secure CI/CD. For WPF/MVVM, new advice details decoupling Views from ViewModels using ViewModelLocator patterns and modern DI, making UI logic more modular and testable.

- [Upgrading NuGet Packages vs. .NET Runtime: Addressing SCA Vulnerabilities in Microsoft.AspNetCore.*](https://techcommunity.microsoft.com/t5/net-runtime/do-i-need-to-upgrade-microsoft-aspnetcore-nuget-packages-after/m-p/4431436#M752)
- [Decoupling Views and ViewModels in CommunityToolkit.Mvvm](https://techcommunity.microsoft.com/t5/app-development/how-to-decouple-views-from-view-models-using-communitytoolkit/m-p/4432591#M1261)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-07-14';

-- weekly-dotnet-roundup-2025-07-21  (2025-07-21)
UPDATE content_items
SET  content      = 'A host of releases and community solutions empower .NET, VS Code, and Visual Studio users, delivering faster, modernized APIs, automation, cross-language extensibility, and practical tooling.

<!--excerpt_end-->

## Coding

### .NET 10 Features and Extension Member Revolution

.NET 10 Preview and C# 14’s extension members allow seamless type extension—supporting methods, properties, and soon operators—without source modification, yielding discoverable, idiomatic APIs. Preview 6 adds post-quantum cryptography, performance boosts, stateful Blazor, passkey auth, and more, plus faster build and deployment tooling.

- [C# 14 Extension Members in .NET 10 Preview: How to Use Extension Everything](https://andrewlock.net/exploring-dotnet-10-preview-features-3-csharp-14-extensions-members/)
- [.NET 10 Preview 6: New Features Across Runtime, SDK, Libraries, ASP.NET Core, Blazor, and More](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-6/)
- [.NET 10 Preview 6 Unboxed - Blazor State Persistence, Passkey, & What is DNX?](/coding/videos/net-10-preview-6-unboxed-blazor-state-persistence-passkey-and-what-is-dnx)

### AI Tooling, MCP, and .NET Interoperability

New templates and workflows make creating MCP servers in .NET 10 and publishing on NuGet simple, seamlessly blending AI extension and modern extensibility. Python.NET enables embedding Python in C#, expanding .NET’s reach into data science.

- [Building Your First MCP Server with .NET 10 and Publishing to NuGet](https://devblogs.microsoft.com/dotnet/mcp-server-dotnet-nuget-quickstart/)
- [MCP C# SDK: What’s New and Upcoming for .NET Developers](/ai/videos/mcp-c-sdk-whats-new-and-upcoming-for-net-developers)
- [Writing and Running Python in .NET](/coding/videos/writing-and-running-python-in-net)

### Developer Tools, Documentation, and Community Solutions

VS Code’s Markdown Mermaid Viewer solves diagram previewing; a Visual Studio extension adds git worktree support for better branch management. Build-time OpenAPI specs in .NET 9 empower CI/CD and automation. Community standup sessions unpack modern Minimal APIs and database integration, while reusable WPF helpers improve desktop UI. Scott Hanselman spotlights .NET’s broad relevance and stability.

- [VS Code Extension: Preview Mermaid Diagrams in Markdown for Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1m42lmi/vs_code_extension_preview_mermaid_diagrams_in/)
- [Visual Studio Has Most Git Features I Need—Except Worktree, So I Built an Extension for It](https://www.reddit.com/r/VisualStudio/comments/1m1l1lc/visual_studio_has_most_git_features_i_need_except/)
- [Build-Time OpenAPI Documentation in .NET 9: A OneDevQuestion with Mike Kistler](/coding/videos/build-time-openapi-documentation-in-net-9-a-onedevquestion-with-mike-kistler)
- [.NET Data Community Standup: Couchbase EF Core Provider Discussion](/coding/videos/net-data-community-standup-couchbase-ef-core-provider-discussion)
- [ASP.NET Community Standup - Why Aren''t You Using Minimal APIs?](/coding/videos/aspnet-community-standup-why-arent-you-using-minimal-apis)
- [Centering a WPF TreeViewItem in the TreeView ScrollViewer](https://weblog.west-wind.com/posts/2025/Jul/15/Centering-a-WPF-TreeViewItem-in-the-TreeView-ScrollViewer)
- [Is .NET Legacy Tech? Scott Hanselman Explores the Modern .NET Platform](/coding/videos/is-net-legacy-tech-scott-hanselman-explores-the-modern-net-platform)',
     excerpt      = 'A host of releases and community solutions empower .NET, VS Code, and Visual Studio users, delivering faster, modernized APIs, automation, cross-language extensibility, and practical tooling.',
     content_hash = md5('A host of releases and community solutions empower .NET, VS Code, and Visual Studio users, delivering faster, modernized APIs, automation, cross-language extensibility, and practical tooling.

<!--excerpt_end-->

## Coding

### .NET 10 Features and Extension Member Revolution

.NET 10 Preview and C# 14’s extension members allow seamless type extension—supporting methods, properties, and soon operators—without source modification, yielding discoverable, idiomatic APIs. Preview 6 adds post-quantum cryptography, performance boosts, stateful Blazor, passkey auth, and more, plus faster build and deployment tooling.

- [C# 14 Extension Members in .NET 10 Preview: How to Use Extension Everything](https://andrewlock.net/exploring-dotnet-10-preview-features-3-csharp-14-extensions-members/)
- [.NET 10 Preview 6: New Features Across Runtime, SDK, Libraries, ASP.NET Core, Blazor, and More](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-6/)
- [.NET 10 Preview 6 Unboxed - Blazor State Persistence, Passkey, & What is DNX?](/coding/videos/net-10-preview-6-unboxed-blazor-state-persistence-passkey-and-what-is-dnx)

### AI Tooling, MCP, and .NET Interoperability

New templates and workflows make creating MCP servers in .NET 10 and publishing on NuGet simple, seamlessly blending AI extension and modern extensibility. Python.NET enables embedding Python in C#, expanding .NET’s reach into data science.

- [Building Your First MCP Server with .NET 10 and Publishing to NuGet](https://devblogs.microsoft.com/dotnet/mcp-server-dotnet-nuget-quickstart/)
- [MCP C# SDK: What’s New and Upcoming for .NET Developers](/ai/videos/mcp-c-sdk-whats-new-and-upcoming-for-net-developers)
- [Writing and Running Python in .NET](/coding/videos/writing-and-running-python-in-net)

### Developer Tools, Documentation, and Community Solutions

VS Code’s Markdown Mermaid Viewer solves diagram previewing; a Visual Studio extension adds git worktree support for better branch management. Build-time OpenAPI specs in .NET 9 empower CI/CD and automation. Community standup sessions unpack modern Minimal APIs and database integration, while reusable WPF helpers improve desktop UI. Scott Hanselman spotlights .NET’s broad relevance and stability.

- [VS Code Extension: Preview Mermaid Diagrams in Markdown for Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1m42lmi/vs_code_extension_preview_mermaid_diagrams_in/)
- [Visual Studio Has Most Git Features I Need—Except Worktree, So I Built an Extension for It](https://www.reddit.com/r/VisualStudio/comments/1m1l1lc/visual_studio_has_most_git_features_i_need_except/)
- [Build-Time OpenAPI Documentation in .NET 9: A OneDevQuestion with Mike Kistler](/coding/videos/build-time-openapi-documentation-in-net-9-a-onedevquestion-with-mike-kistler)
- [.NET Data Community Standup: Couchbase EF Core Provider Discussion](/coding/videos/net-data-community-standup-couchbase-ef-core-provider-discussion)
- [ASP.NET Community Standup - Why Aren''t You Using Minimal APIs?](/coding/videos/aspnet-community-standup-why-arent-you-using-minimal-apis)
- [Centering a WPF TreeViewItem in the TreeView ScrollViewer](https://weblog.west-wind.com/posts/2025/Jul/15/Centering-a-WPF-TreeViewItem-in-the-TreeView-ScrollViewer)
- [Is .NET Legacy Tech? Scott Hanselman Explores the Modern .NET Platform](/coding/videos/is-net-legacy-tech-scott-hanselman-explores-the-modern-net-platform)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-07-21';

-- weekly-dotnet-roundup-2025-07-28  (2025-07-28)
UPDATE content_items
SET  content      = 'This week in coding brought advances in developer ergonomics, automation, and strong community learning—especially for .NET and TypeScript teams.

<!--excerpt_end-->

## Coding

### Source Generator and Protocol Enhancements

.NET 10’s `AddEmbeddedAttributeDefinition()` API eradicates old source generator pain by letting authors embed marker attributes directly—eliminating type conflicts for projects on current SDKs and resulting in cleaner, more maintainable metaprogramming. This follows last week’s .NET preview coverage.

The MCP C# SDK update (protocol 2025-06-18) delivers better OAuth2 support, structured outputs, user information elicitation, and richer metadata—streamlining secure authentication and human-in-the-loop AI workflows.

- [Solving the Source Generator ''Marker Attribute'' Problem in .NET 10: AddEmbeddedAttributeDefinition() Explained](https://andrewlock.net/exploring-dotnet-10-preview-features-4-solving-the-source-generator-marker-attribute-problem-in-dotnet-10/)
- [MCP C# SDK Updated: Protocol 2025-06-18 Brings Elicitation, Structured Output, and Enhanced Security](https://devblogs.microsoft.com/dotnet/mcp-csharp-sdk-2025-06-18-update/)

### Language and Tooling Modernization

TypeScript 5.9 RC introduces ECMAScript `import defer`, Node.js 20 module compatibility, and major speedups, as well as editor goodies like improved tooltips—enabling safer, faster onboarding for teams updating dependencies.

- [Announcing the Release Candidate of TypeScript 5.9: What''s New and Improved](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9-rc/)

### Documentation and Project Standards

PowerShell’s PlatyPS 1.0.0 swaps XML for fast, cross-platform Markdown doc authoring, making up-to-date, source-controlled help a reality for large teams. In .NET Aspire, new name/constant centralization patterns prevent errors and speed up large-scale project refactoring.

- [Announcing Microsoft.PowerShell.PlatyPS 1.0.0: PowerShell Help Authoring Simplified](https://devblogs.microsoft.com/powershell/announcing-platyps-100/)
- [.NET Aspire: Centralizing Project Names and Constants](/coding/videos/net-aspire-centralizing-project-names-and-constants)

### IDE Experience and Community Engagement

A proposed Visual Studio web browser/console extension could cut context-switching and boost web/React/.NET development. Community sessions like ‘Rubber Duck Thursdays’ facilitate hands-on peer learning, reinforcing last week’s focus on collaborative, real-time growth.

- [Developing a Web Browser and Console Log Extension for Visual Studio](https://www.reddit.com/r/VisualStudio/comments/1m5pxx2/possible_new_web_browserconsole_extension/)
- [Rubber Duck Thursdays - Build for the Love of Code](/coding/videos/rubber-duck-thursdays-build-for-the-love-of-code)',
     excerpt      = 'This week in coding brought advances in developer ergonomics, automation, and strong community learning—especially for .NET and TypeScript teams.',
     content_hash = md5('This week in coding brought advances in developer ergonomics, automation, and strong community learning—especially for .NET and TypeScript teams.

<!--excerpt_end-->

## Coding

### Source Generator and Protocol Enhancements

.NET 10’s `AddEmbeddedAttributeDefinition()` API eradicates old source generator pain by letting authors embed marker attributes directly—eliminating type conflicts for projects on current SDKs and resulting in cleaner, more maintainable metaprogramming. This follows last week’s .NET preview coverage.

The MCP C# SDK update (protocol 2025-06-18) delivers better OAuth2 support, structured outputs, user information elicitation, and richer metadata—streamlining secure authentication and human-in-the-loop AI workflows.

- [Solving the Source Generator ''Marker Attribute'' Problem in .NET 10: AddEmbeddedAttributeDefinition() Explained](https://andrewlock.net/exploring-dotnet-10-preview-features-4-solving-the-source-generator-marker-attribute-problem-in-dotnet-10/)
- [MCP C# SDK Updated: Protocol 2025-06-18 Brings Elicitation, Structured Output, and Enhanced Security](https://devblogs.microsoft.com/dotnet/mcp-csharp-sdk-2025-06-18-update/)

### Language and Tooling Modernization

TypeScript 5.9 RC introduces ECMAScript `import defer`, Node.js 20 module compatibility, and major speedups, as well as editor goodies like improved tooltips—enabling safer, faster onboarding for teams updating dependencies.

- [Announcing the Release Candidate of TypeScript 5.9: What''s New and Improved](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9-rc/)

### Documentation and Project Standards

PowerShell’s PlatyPS 1.0.0 swaps XML for fast, cross-platform Markdown doc authoring, making up-to-date, source-controlled help a reality for large teams. In .NET Aspire, new name/constant centralization patterns prevent errors and speed up large-scale project refactoring.

- [Announcing Microsoft.PowerShell.PlatyPS 1.0.0: PowerShell Help Authoring Simplified](https://devblogs.microsoft.com/powershell/announcing-platyps-100/)
- [.NET Aspire: Centralizing Project Names and Constants](/coding/videos/net-aspire-centralizing-project-names-and-constants)

### IDE Experience and Community Engagement

A proposed Visual Studio web browser/console extension could cut context-switching and boost web/React/.NET development. Community sessions like ‘Rubber Duck Thursdays’ facilitate hands-on peer learning, reinforcing last week’s focus on collaborative, real-time growth.

- [Developing a Web Browser and Console Log Extension for Visual Studio](https://www.reddit.com/r/VisualStudio/comments/1m5pxx2/possible_new_web_browserconsole_extension/)
- [Rubber Duck Thursdays - Build for the Love of Code](/coding/videos/rubber-duck-thursdays-build-for-the-love-of-code)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-07-28';

-- weekly-dotnet-roundup-2025-08-04  (2025-08-04)
UPDATE content_items
SET  content      = 'This week brought strong advances to .NET Aspire, TypeScript, and C# tooling, deeper open-source integration, and community-driven productivity patterns. New releases and workflows point to a modern, flexible Microsoft developer stack supporting both rapid prototyping and scalable, production-grade delivery.

<!--excerpt_end-->

## Coding

### .NET Aspire Ecosystem: Distributed App Orchestration

Aspire 9.4 introduced a GA CLI with fast scaffolding, dashboard notifications, and AI model hosting support—simplifying distributed service orchestration and boosting workflow speed. New APIs make adding and monitoring external services easier, while the dashboard and hosting enhancements align Aspire with broader MCP-driven architectures.

- [Aspire 9.4: CLI, Interactive Dashboard, AI Integrations, and New Features](https://devblogs.microsoft.com/dotnet/announcing-aspire-9-4/)
- [.NET Aspire 9.4 New Features: ExternalService Resource, Interaction Service, Aspire CLI, and GitHub Model Integration](/coding/videos/net-aspire-94-new-features-externalservice-resource-interaction-service-aspire-cli-and-github-model-integration)
- [.NET Aspire – Adding Custom Commands to the Dashboard](/coding/videos/net-aspire-adding-custom-commands-to-the-dashboard)

### Full-Stack Development: Modern Patterns and Open Source

Integration tutorials for React with Aspire/ASP.NET Core APIs show how modern front/back workflows, DB migrations, and AI debugging now operate in an all-in-one, frictionless pipeline. Open-source stacks and templates (e.g., Xams) fuel collaborative modernization for .NET teams.

- [Building a Full-Stack App with React and Aspire: Step-by-Step Integration with ASP.NET Core Web API](https://devblogs.microsoft.com/dotnet/new-aspire-app-with-react/)
- [Open-sourced the ASP.NET + React Stack for Internal Business App Development](https://www.reddit.com/r/dotnet/comments/1mgk8cw/opensourced_the_aspnet_react_stack_we_use_to/)
- [Templates for MVC / Razor Pages with a Modern Frontend Build System](https://www.reddit.com/r/dotnet/comments/1mf10vc/templates_for_mvc_razor_pages_with_a_modern/)

### C# Language Evolution: Type Union Advancements

The C# language team adopted type union proposals, marking a move toward safer, more expressive APIs and code. This aligns with .NET’s ongoing shift to ergonomic, maintainable codebases and responds to long-standing community requests for language flexibility.

- [More Type Union Proposals Adopted by the C# Language Design Team](https://www.reddit.com/r/dotnet/comments/1mf2ylu/more_type_union_proposals_adopted_by_the_c/)
- [More type union proposals adopted by the language design team!](https://www.reddit.com/r/csharp/comments/1mf2xll/more_type_union_proposals_adopted_by_the_language/)
- [C# 15 Wishlist: What Features Do Developers Hope For?](https://www.reddit.com/r/csharp/comments/1meqqrk/c_15_wishlist/)

### TypeScript 5.9: JavaScript Tooling Modernization

TS 5.9 delivers streamlined config, ECMAScript import enhancements, improved DOM typings, stronger type inference, and speed boosts. This builds on last week’s RC and signals robust, forward-compatible JavaScript tooling for future upgrades.

- [Announcing TypeScript 5.9: New Features, Improvements, and What’s Next](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9/)

### Expanding .NET and C# Tooling

.NET 10’s `dnx` CLI enables quick-use tools without installs; Aspire Event Hub Live Explorer improves event-driven debugging locally; open-source utilities make Windows service hosting and Spotify command integration more approachable for everyday devs.

- [Running One-Off .NET Tools with dnx: Exploring the .NET 10 Preview](https://andrewlock.net/exploring-dotnet-10-preview-features-5-running-one-off-dotnet-tools-with-dnx/)
- [Introducing .NET Aspire Event Hub Live Explorer](https://www.reddit.com/r/dotnet/comments/1mgm401/introducing_net_aspire_event_hub_live_explorer/)
- [C# Tool to Run Any App as a Windows Service: Managed Alternative to NSSM](https://www.reddit.com/r/dotnet/comments/1mgewsc/just_built_a_tool_that_turns_any_app_into_a/)
- [SpotifyLikeButton: Global Hotkey Utility to Like/Unlike Songs on Spotify](https://www.reddit.com/r/csharp/comments/1mfyig0/spotifylikebutton/)

### Best Practices: Architecture, Validation, and Cross-Platform

Discussions ranged from integrating ASP.NET Core Identity in Clean Architecture, to best practices for FluentValidation centralization. Cross-platform and legacy support using Mono drew caution, while C# inheritance/constructor gotchas resurfaced as valuable reminders for maintainability.

- [How to integrate ASP.NET Core Identity in Clean Architecture (DDD) without breaking domain independence?](https://www.reddit.com/r/dotnet/comments/1meuo7l/how_to_integrate_aspnet_core_identity_in_clean/)
- [Model Validation Best Practices in .NET Using FluentValidation](https://www.reddit.com/r/dotnet/comments/1mg49nf/model_validation_best_practices/)
- [Sanity Check On .NET Framework / Mono / MacOS](https://www.reddit.com/r/dotnet/comments/1mfy7yk/sanity_check_on_net_framework_mono_macos/)
- [C# Inheritance Puzzle](https://www.reddit.com/r/csharp/comments/1mfzryw/c_inheritance_puzzle/)

### UI Modernization and Open Source Community

New C# bindings for Rust’s egui UI, WinUI’s OSS transition, and a wave of community projects (from dashboards to side projects) reflect a robust drive toward desktop UX modernization and continued open source culture in the .NET ecosystem.

- [Egui.NET: unofficial C# bindings for the easy-to-use Rust UI library](https://www.reddit.com/r/csharp/comments/1mgwnvs/eguinet_unofficial_c_bindings_for_the_easytouse/)
- [WinUI OSS Update: Phased Rollout Toward Open Collaboration](https://www.reddit.com/r/dotnet/comments/1mfx9wm/winui_oss_update_phased_rollout_toward_open/)
- [August 2025 Community Project Showcase: C# Side Projects](https://www.reddit.com/r/csharp/comments/1memjhl/come_discuss_your_side_projects_august_2025/)
- [Unlocking Hidden Acrylic/Mica Style UI in Visual Studio 2022](https://www.reddit.com/r/VisualStudio/comments/1mbzcis/visual_studio_2022_has_hidden_acrylicmica_style_ui/)
- [Termix v0.9.0 – Add Rename, Delete, Write File Ops & Fuzzy Search (Preview)](https://www.reddit.com/r/dotnet/comments/1mf1szg/termix_v090_add_rename_delete_write_file_ops/)
- [[Looking for Feedback]: I Made this StateMachine Lib!](https://www.reddit.com/r/csharp/comments/1mfzmdu/looking_for_feedback_i_made_this_statemachine_lib/)

### Dev Events and Knowledge Sharing

Live events and community standups—on Blazor diagnostics, source generators, and SQL—fuel peer-driven learning and rapid open knowledge exchange across the stack.

- [Special Visual Studio Toolbox Live: Microsoft-Led Sessions on .NET, AI, Azure, and Copilot – Aug 5](https://devblogs.microsoft.com/visualstudio/watch-live-visual-studio-toolbox-at-vs-live-redmond-2025/)
- [ASP.NET Community Standup: Building a Better PerfView Diagnostics Tool with Blazor](/coding/videos/aspnet-community-standup-building-a-better-perfview-diagnostics-tool-with-blazor)
- [.NET Data Community Standup: Exploring jOOQ with Lukas Eder and Inspiration for EF](/coding/videos/net-data-community-standup-exploring-jooq-with-lukas-eder-and-inspiration-for-ef)

### Workflow, IDE Troubleshooting, and Productivity

Threads on VS/Windows update pain, IDE choice, effective MVC UX, and code professionalism highlight the practical side of day-to-day dev work, showing knowledge sharing remains key to consistent, maintainable code delivery.

- [Weird Unhandled Exception Errors in Visual Studio After Windows 11 Update](https://www.reddit.com/r/VisualStudio/comments/1mb6app/weird_unhandled_exception_errors_after_windows_11/)
- [Full Stack: Is It Better to Use Visual Studio or VSCode for Back-End and Front-End Development?](https://www.reddit.com/r/dotnet/comments/1mfuefu/full_stack_visual_studio_or_vscode/)
- [How to Show a Spinner on Form Submit Without Disrupting MVC Behavior](https://www.reddit.com/r/dotnet/comments/1mfpq2m/how_do_i_show_a_spinner_btn_on_form_submit/)
- [What does professional code look like?](https://www.reddit.com/r/csharp/comments/1mfsv2g/what_does_professional_code_look_like/)

### Community Engagement and Protocol Contributions

New contributors can directly impact multi-language protocols like MCP, with clear onboarding and open-source guidelines fostering inclusive, scalable progress.

- [How to Build, Test & Deploy MCP Apps with Real Tools and Workflows](/ai/videos/how-to-build-test-and-deploy-mcp-apps-with-real-tools-and-workflows)
- [How to Contribute to MCP: Tools, Documentation, Code & More](/coding/videos/how-to-contribute-to-mcp-tools-documentation-code-and-more)',
     excerpt      = 'This week brought strong advances to .NET Aspire, TypeScript, and C# tooling, deeper open-source integration, and community-driven productivity patterns. New releases and workflows point to a modern, flexible Microsoft developer stack supporting both rapid prototyping and scalable, production-grade delivery.',
     content_hash = md5('This week brought strong advances to .NET Aspire, TypeScript, and C# tooling, deeper open-source integration, and community-driven productivity patterns. New releases and workflows point to a modern, flexible Microsoft developer stack supporting both rapid prototyping and scalable, production-grade delivery.

<!--excerpt_end-->

## Coding

### .NET Aspire Ecosystem: Distributed App Orchestration

Aspire 9.4 introduced a GA CLI with fast scaffolding, dashboard notifications, and AI model hosting support—simplifying distributed service orchestration and boosting workflow speed. New APIs make adding and monitoring external services easier, while the dashboard and hosting enhancements align Aspire with broader MCP-driven architectures.

- [Aspire 9.4: CLI, Interactive Dashboard, AI Integrations, and New Features](https://devblogs.microsoft.com/dotnet/announcing-aspire-9-4/)
- [.NET Aspire 9.4 New Features: ExternalService Resource, Interaction Service, Aspire CLI, and GitHub Model Integration](/coding/videos/net-aspire-94-new-features-externalservice-resource-interaction-service-aspire-cli-and-github-model-integration)
- [.NET Aspire – Adding Custom Commands to the Dashboard](/coding/videos/net-aspire-adding-custom-commands-to-the-dashboard)

### Full-Stack Development: Modern Patterns and Open Source

Integration tutorials for React with Aspire/ASP.NET Core APIs show how modern front/back workflows, DB migrations, and AI debugging now operate in an all-in-one, frictionless pipeline. Open-source stacks and templates (e.g., Xams) fuel collaborative modernization for .NET teams.

- [Building a Full-Stack App with React and Aspire: Step-by-Step Integration with ASP.NET Core Web API](https://devblogs.microsoft.com/dotnet/new-aspire-app-with-react/)
- [Open-sourced the ASP.NET + React Stack for Internal Business App Development](https://www.reddit.com/r/dotnet/comments/1mgk8cw/opensourced_the_aspnet_react_stack_we_use_to/)
- [Templates for MVC / Razor Pages with a Modern Frontend Build System](https://www.reddit.com/r/dotnet/comments/1mf10vc/templates_for_mvc_razor_pages_with_a_modern/)

### C# Language Evolution: Type Union Advancements

The C# language team adopted type union proposals, marking a move toward safer, more expressive APIs and code. This aligns with .NET’s ongoing shift to ergonomic, maintainable codebases and responds to long-standing community requests for language flexibility.

- [More Type Union Proposals Adopted by the C# Language Design Team](https://www.reddit.com/r/dotnet/comments/1mf2ylu/more_type_union_proposals_adopted_by_the_c/)
- [More type union proposals adopted by the language design team!](https://www.reddit.com/r/csharp/comments/1mf2xll/more_type_union_proposals_adopted_by_the_language/)
- [C# 15 Wishlist: What Features Do Developers Hope For?](https://www.reddit.com/r/csharp/comments/1meqqrk/c_15_wishlist/)

### TypeScript 5.9: JavaScript Tooling Modernization

TS 5.9 delivers streamlined config, ECMAScript import enhancements, improved DOM typings, stronger type inference, and speed boosts. This builds on last week’s RC and signals robust, forward-compatible JavaScript tooling for future upgrades.

- [Announcing TypeScript 5.9: New Features, Improvements, and What’s Next](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9/)

### Expanding .NET and C# Tooling

.NET 10’s `dnx` CLI enables quick-use tools without installs; Aspire Event Hub Live Explorer improves event-driven debugging locally; open-source utilities make Windows service hosting and Spotify command integration more approachable for everyday devs.

- [Running One-Off .NET Tools with dnx: Exploring the .NET 10 Preview](https://andrewlock.net/exploring-dotnet-10-preview-features-5-running-one-off-dotnet-tools-with-dnx/)
- [Introducing .NET Aspire Event Hub Live Explorer](https://www.reddit.com/r/dotnet/comments/1mgm401/introducing_net_aspire_event_hub_live_explorer/)
- [C# Tool to Run Any App as a Windows Service: Managed Alternative to NSSM](https://www.reddit.com/r/dotnet/comments/1mgewsc/just_built_a_tool_that_turns_any_app_into_a/)
- [SpotifyLikeButton: Global Hotkey Utility to Like/Unlike Songs on Spotify](https://www.reddit.com/r/csharp/comments/1mfyig0/spotifylikebutton/)

### Best Practices: Architecture, Validation, and Cross-Platform

Discussions ranged from integrating ASP.NET Core Identity in Clean Architecture, to best practices for FluentValidation centralization. Cross-platform and legacy support using Mono drew caution, while C# inheritance/constructor gotchas resurfaced as valuable reminders for maintainability.

- [How to integrate ASP.NET Core Identity in Clean Architecture (DDD) without breaking domain independence?](https://www.reddit.com/r/dotnet/comments/1meuo7l/how_to_integrate_aspnet_core_identity_in_clean/)
- [Model Validation Best Practices in .NET Using FluentValidation](https://www.reddit.com/r/dotnet/comments/1mg49nf/model_validation_best_practices/)
- [Sanity Check On .NET Framework / Mono / MacOS](https://www.reddit.com/r/dotnet/comments/1mfy7yk/sanity_check_on_net_framework_mono_macos/)
- [C# Inheritance Puzzle](https://www.reddit.com/r/csharp/comments/1mfzryw/c_inheritance_puzzle/)

### UI Modernization and Open Source Community

New C# bindings for Rust’s egui UI, WinUI’s OSS transition, and a wave of community projects (from dashboards to side projects) reflect a robust drive toward desktop UX modernization and continued open source culture in the .NET ecosystem.

- [Egui.NET: unofficial C# bindings for the easy-to-use Rust UI library](https://www.reddit.com/r/csharp/comments/1mgwnvs/eguinet_unofficial_c_bindings_for_the_easytouse/)
- [WinUI OSS Update: Phased Rollout Toward Open Collaboration](https://www.reddit.com/r/dotnet/comments/1mfx9wm/winui_oss_update_phased_rollout_toward_open/)
- [August 2025 Community Project Showcase: C# Side Projects](https://www.reddit.com/r/csharp/comments/1memjhl/come_discuss_your_side_projects_august_2025/)
- [Unlocking Hidden Acrylic/Mica Style UI in Visual Studio 2022](https://www.reddit.com/r/VisualStudio/comments/1mbzcis/visual_studio_2022_has_hidden_acrylicmica_style_ui/)
- [Termix v0.9.0 – Add Rename, Delete, Write File Ops & Fuzzy Search (Preview)](https://www.reddit.com/r/dotnet/comments/1mf1szg/termix_v090_add_rename_delete_write_file_ops/)
- [[Looking for Feedback]: I Made this StateMachine Lib!](https://www.reddit.com/r/csharp/comments/1mfzmdu/looking_for_feedback_i_made_this_statemachine_lib/)

### Dev Events and Knowledge Sharing

Live events and community standups—on Blazor diagnostics, source generators, and SQL—fuel peer-driven learning and rapid open knowledge exchange across the stack.

- [Special Visual Studio Toolbox Live: Microsoft-Led Sessions on .NET, AI, Azure, and Copilot – Aug 5](https://devblogs.microsoft.com/visualstudio/watch-live-visual-studio-toolbox-at-vs-live-redmond-2025/)
- [ASP.NET Community Standup: Building a Better PerfView Diagnostics Tool with Blazor](/coding/videos/aspnet-community-standup-building-a-better-perfview-diagnostics-tool-with-blazor)
- [.NET Data Community Standup: Exploring jOOQ with Lukas Eder and Inspiration for EF](/coding/videos/net-data-community-standup-exploring-jooq-with-lukas-eder-and-inspiration-for-ef)

### Workflow, IDE Troubleshooting, and Productivity

Threads on VS/Windows update pain, IDE choice, effective MVC UX, and code professionalism highlight the practical side of day-to-day dev work, showing knowledge sharing remains key to consistent, maintainable code delivery.

- [Weird Unhandled Exception Errors in Visual Studio After Windows 11 Update](https://www.reddit.com/r/VisualStudio/comments/1mb6app/weird_unhandled_exception_errors_after_windows_11/)
- [Full Stack: Is It Better to Use Visual Studio or VSCode for Back-End and Front-End Development?](https://www.reddit.com/r/dotnet/comments/1mfuefu/full_stack_visual_studio_or_vscode/)
- [How to Show a Spinner on Form Submit Without Disrupting MVC Behavior](https://www.reddit.com/r/dotnet/comments/1mfpq2m/how_do_i_show_a_spinner_btn_on_form_submit/)
- [What does professional code look like?](https://www.reddit.com/r/csharp/comments/1mfsv2g/what_does_professional_code_look_like/)

### Community Engagement and Protocol Contributions

New contributors can directly impact multi-language protocols like MCP, with clear onboarding and open-source guidelines fostering inclusive, scalable progress.

- [How to Build, Test & Deploy MCP Apps with Real Tools and Workflows](/ai/videos/how-to-build-test-and-deploy-mcp-apps-with-real-tools-and-workflows)
- [How to Contribute to MCP: Tools, Documentation, Code & More](/coding/videos/how-to-contribute-to-mcp-tools-documentation-code-and-more)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-08-04';

-- weekly-dotnet-roundup-2025-08-11  (2025-08-11)
UPDATE content_items
SET  content      = 'This week’s .NET ecosystem saw major advances in authentication, language expressiveness, cloud-native tooling, and real-world developer productivity.

<!--excerpt_end-->

## Coding

### Cloud-Native ASP.NET Core and Passwordless Authentication Advance

.NET 10 Preview 6 brings passkey authentication support to ASP.NET Core and Blazor. Aspire—a toolkit for cloud-native app enablement—makes distributed app development more approachable with rich documentation, tooling, and CI/CD support.

- [Exploring Passkey Support in ASP.NET Core Identity with .NET 10 Preview 6](https://andrewlock.net/exploring-dotnet-10-preview-features-6-passkey-support-for-aspnetcore-identity/)
- [ASP.NET Community Standup: Why ASP.NET Core Developers Will Love Aspire](/coding/videos/aspnet-community-standup-why-aspnet-core-developers-will-love-aspire)

### Language Innovations: C# 14 Extension Members and Future Nominal Union Types

C# 14 introduces “Extension Everything”—allowing methods, properties, and operators on existing types. Upcoming nominal union types are poised to simplify modeling alternatives in future C#, driven by deep community interest over several weeks.

- [C# 14 Extension Members: Also Known as Extension Everything - NDepend Blog](https://www.reddit.com/r/dotnet/comments/1mhbukq/c_14_extension_members_also_known_as_extension/)
- [Discussion: Nominal Union Types Demoed at VS Live, Redmond](https://www.reddit.com/r/csharp/comments/1mj96yf/nominal_union_types_were_demoted_at_vs_live_at/)

### Practical Tools and Productivity Boosts Across the Stack

A unified .NET CLI tool is in the works for SDK/runtime management. Community reviews focus on code formatting for old .NET versions and powerful terminal file managers like Termix v1.2.0. The VS Code 1.103 release targets common C#/VS Code “papercuts,” complementing recent .NET tooling expansions.

- [Discussion: New CLI Tool for .NET SDK Management and Updates](https://www.reddit.com/r/dotnet/comments/1mjgvq3/want_to_make_it_easier_to_get_startedstay_up_to/)
- [Termix v1.2.0: .NET Terminal File Manager Adds Cut, Copy, Move, and Stable Fuzzy Search](https://www.reddit.com/r/dotnet/comments/1mhgcod/beautiful_terminal_based_file_manager_now/)

### Testing, Orchestration, and Advanced Workflows

TUnit testing framework now orchestrates parallel dependency injection and resource sharing for integration tests. Detailed guides tackle deterministic cleanup with IAsyncDisposable, config loading post-OS patching, and diagnosing high RAM use in ASP.NET Core apps.

- [TUnit Test Orchestration: Advanced Setup and Parallel Dependency Injection](https://www.reddit.com/r/csharp/comments/1mjgiuq/tunit_test_orchestration/)
- [High RAM Usage Troubleshooting in ASP.NET Core MVC 8 Application](https://www.reddit.com/r/dotnet/comments/1mk4hp1/high_ram_usage_aspnet_core_mvc/)

### Architectures, Patterns, and API Modeling Debates

Debates abound on modular monoliths, repository vs. CQRS, and navigation property best practices. New source generators and reflective case studies surface practical strategies for large codebases, echoing last week’s focus on maintainability and architecture.

- [Reflections on .NET Project Structure and Complexity for Beginners](https://www.reddit.com/r/dotnet/comments/1mkujgo/starting_to_understand_the_differences_of_dotnet/)

### Ecosystem: Updates, Conferences, and the Road Ahead

Microsoft opened the .NET Conf 2025 call for content. Rx.NET modernizes its packaging, and servicing updates for .NET 8/9/Framework ensure stability. These reinforce the open and evolving ecosystem trend observed last week.

- [.NET Conf 2025 Call for Content: Share Your .NET 10 Expertise](https://devblogs.microsoft.com/dotnet/dotnet-conf-2025-announcing-the-call-for-content/)

### Efficiency, Low-Level Performance, and Real-World Coding Tactics

Span-based ZaString delivers zero-allocation string building. JIT vs. AOT benchmarks inform performance optimizations, while community tactics address cross-platform processor detection, deadletter queue processing, and efficient helper methods.

- [ZaString: A Zero-Allocation Span-Based String Builder for .NET](https://www.reddit.com/r/dotnet/comments/1mkqa37/stop_allocating_strings_i_built_a_spanpowered/)

### Learning, Community Growth, and Real-World Case Studies

Upskilling, .NET migration, SCADA scripting, and effective LINQ remain focal points, infusing the community with resources and support for every career stage.

- [Transitioning from .NET Framework 4.8 to Modern .NET (Core/9): Advice & Resources](https://www.reddit.com/r/dotnet/comments/1mk1z6x/studying_net_coming_from_net_framework/)',
     excerpt      = 'This week’s .NET ecosystem saw major advances in authentication, language expressiveness, cloud-native tooling, and real-world developer productivity.',
     content_hash = md5('This week’s .NET ecosystem saw major advances in authentication, language expressiveness, cloud-native tooling, and real-world developer productivity.

<!--excerpt_end-->

## Coding

### Cloud-Native ASP.NET Core and Passwordless Authentication Advance

.NET 10 Preview 6 brings passkey authentication support to ASP.NET Core and Blazor. Aspire—a toolkit for cloud-native app enablement—makes distributed app development more approachable with rich documentation, tooling, and CI/CD support.

- [Exploring Passkey Support in ASP.NET Core Identity with .NET 10 Preview 6](https://andrewlock.net/exploring-dotnet-10-preview-features-6-passkey-support-for-aspnetcore-identity/)
- [ASP.NET Community Standup: Why ASP.NET Core Developers Will Love Aspire](/coding/videos/aspnet-community-standup-why-aspnet-core-developers-will-love-aspire)

### Language Innovations: C# 14 Extension Members and Future Nominal Union Types

C# 14 introduces “Extension Everything”—allowing methods, properties, and operators on existing types. Upcoming nominal union types are poised to simplify modeling alternatives in future C#, driven by deep community interest over several weeks.

- [C# 14 Extension Members: Also Known as Extension Everything - NDepend Blog](https://www.reddit.com/r/dotnet/comments/1mhbukq/c_14_extension_members_also_known_as_extension/)
- [Discussion: Nominal Union Types Demoed at VS Live, Redmond](https://www.reddit.com/r/csharp/comments/1mj96yf/nominal_union_types_were_demoted_at_vs_live_at/)

### Practical Tools and Productivity Boosts Across the Stack

A unified .NET CLI tool is in the works for SDK/runtime management. Community reviews focus on code formatting for old .NET versions and powerful terminal file managers like Termix v1.2.0. The VS Code 1.103 release targets common C#/VS Code “papercuts,” complementing recent .NET tooling expansions.

- [Discussion: New CLI Tool for .NET SDK Management and Updates](https://www.reddit.com/r/dotnet/comments/1mjgvq3/want_to_make_it_easier_to_get_startedstay_up_to/)
- [Termix v1.2.0: .NET Terminal File Manager Adds Cut, Copy, Move, and Stable Fuzzy Search](https://www.reddit.com/r/dotnet/comments/1mhgcod/beautiful_terminal_based_file_manager_now/)

### Testing, Orchestration, and Advanced Workflows

TUnit testing framework now orchestrates parallel dependency injection and resource sharing for integration tests. Detailed guides tackle deterministic cleanup with IAsyncDisposable, config loading post-OS patching, and diagnosing high RAM use in ASP.NET Core apps.

- [TUnit Test Orchestration: Advanced Setup and Parallel Dependency Injection](https://www.reddit.com/r/csharp/comments/1mjgiuq/tunit_test_orchestration/)
- [High RAM Usage Troubleshooting in ASP.NET Core MVC 8 Application](https://www.reddit.com/r/dotnet/comments/1mk4hp1/high_ram_usage_aspnet_core_mvc/)

### Architectures, Patterns, and API Modeling Debates

Debates abound on modular monoliths, repository vs. CQRS, and navigation property best practices. New source generators and reflective case studies surface practical strategies for large codebases, echoing last week’s focus on maintainability and architecture.

- [Reflections on .NET Project Structure and Complexity for Beginners](https://www.reddit.com/r/dotnet/comments/1mkujgo/starting_to_understand_the_differences_of_dotnet/)

### Ecosystem: Updates, Conferences, and the Road Ahead

Microsoft opened the .NET Conf 2025 call for content. Rx.NET modernizes its packaging, and servicing updates for .NET 8/9/Framework ensure stability. These reinforce the open and evolving ecosystem trend observed last week.

- [.NET Conf 2025 Call for Content: Share Your .NET 10 Expertise](https://devblogs.microsoft.com/dotnet/dotnet-conf-2025-announcing-the-call-for-content/)

### Efficiency, Low-Level Performance, and Real-World Coding Tactics

Span-based ZaString delivers zero-allocation string building. JIT vs. AOT benchmarks inform performance optimizations, while community tactics address cross-platform processor detection, deadletter queue processing, and efficient helper methods.

- [ZaString: A Zero-Allocation Span-Based String Builder for .NET](https://www.reddit.com/r/dotnet/comments/1mkqa37/stop_allocating_strings_i_built_a_spanpowered/)

### Learning, Community Growth, and Real-World Case Studies

Upskilling, .NET migration, SCADA scripting, and effective LINQ remain focal points, infusing the community with resources and support for every career stage.

- [Transitioning from .NET Framework 4.8 to Modern .NET (Core/9): Advice & Resources](https://www.reddit.com/r/dotnet/comments/1mk1z6x/studying_net_coming_from_net_framework/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-08-11';

-- weekly-dotnet-roundup-2025-08-18  (2025-08-18)
UPDATE content_items
SET  content      = 'Developers are getting meaningful updates this week, from fresh language features in .NET 10 Preview and smarter Excel Python tools to improved resilience in Spark. New guides cover everything from disk cleanup to building dual-transport servers, reflecting a bigger focus on practical, modern, and cross-platform workflows.

<!--excerpt_end-->

## Coding

### Major Platform Features and Updates: .NET 10 Preview, Excel Python Image Analysis, Spark Resilience

.NET 10 Preview 7 brings new cryptographic features, faster JSON serialization, better diagnostics, easier authentication, and stronger cross-platform support. The improvements boost usability, security, and help streamline cloud-native workflows.

The latest Excel update lets you use Python natively for image analysis tasks like blur detection, brightness checks, and metadata collection—bringing advanced vision analysis tools to everyday spreadsheet users.

Spark’s improved Iteration Panel makes file filtering and API failure handling easier, helping with smoother, more reliable development cycles.

- [.NET 10 Preview 7 Released: Key Updates for Libraries, ASP.NET Core, Blazor, and MAUI](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-7/)
- [Analyzing Images with Python in Excel: Now Natively Supported](https://techcommunity.microsoft.com/t5/microsoft-365-insider-blog/analyze-images-with-python-in-excel/ba-p/4440388)
- [Spark Resilience Improvements Enhance Reliability and Iteration Experience](https://github.blog/changelog/2025-08-13-spark-resilience-improvements)

### Guides for Modern, Cross-Platform Development: .NET Aspire, .NET MAUI, Browser-Based .NET

Step-by-step guides help teams get started with .NET Aspire for distributed systems, including boilerplate code, GitHub Actions integration, and custom metrics for ongoing monitoring. Visual Studio and .NET MAUI tutorials focus on building cross-platform mobile and desktop apps—with advice on UI optimization, file size reduction, and streamlined updates.

A walkthrough from Andrew Lock shows how to run .NET in the browser without Blazor, using WebAssembly templates and JavaScript interop for high-performance client apps.

- [Building Confident Application Systems with .NET Aspire: From Dev to Deployment](/azure/videos/building-confident-application-systems-with-net-aspire-from-dev-to-deployment)
- [Building Mobile and Desktop Apps with Visual Studio and .NET MAUI](/coding/videos/building-mobile-and-desktop-apps-with-visual-studio-and-net-maui)
- [Running .NET in the Browser Without Blazor Using WASM](https://andrewlock.net/running-dotnet-in-the-browser-without-blazor/)

### Language Evolution and the Future of Web Development: C# 14, ASP.NET Core & Blazor in .NET 10

A detailed look at C# 14 covers improved pattern matching, nullability support, and value types—making code safer and cleaner. Current .NET team previews for ASP.NET Core and Blazor in .NET 10 include modern security, diagnostics, WebAuthn support, integrated AI libraries, and faster project ramp-up with Aspire, helping developers stay current with critical web advances.

- [Highlights and Upcoming Features in C#: A Deep Dive into C# 14](/coding/videos/highlights-and-upcoming-features-in-c-a-deep-dive-into-c-14)
- [The Future of Web Development with ASP.NET Core & Blazor in .NET 10](/ai/videos/the-future-of-web-development-with-aspnet-core-and-blazor-in-net-10)

### Other Coding News

VS Code’s “Beast mode” rolls out improvements for batch edits, UI adjustments, and workflow enhancements, with a video guide to help users get started. Additional tutorials show how to simplify .NET mapping with Facet, automate disk cleanup with PowerShell, and build STDIO/HTTP dual-transport MCP servers for flexible cloud and local deployments.

- [VS Code Beast Mode Explained: Features and Usage](/coding/videos/vs-code-beast-mode-explained-features-and-usage)
- [Enhancing .NET Code: Using Facet Instead of Traditional Mapping](/coding/videos/enhancing-net-code-using-facet-instead-of-traditional-mapping)
- [Finding Large Directories and Recovering Lost Disk Space with PowerShell](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
- [Building a Dual-Transport MCP Server with .NET: STDIO and HTTP Support](https://techcommunity.microsoft.com/t5/microsoft-developer-community/one-mcp-server-two-transports-stdio-and-http/ba-p/4443915)',
     excerpt      = 'Developers are getting meaningful updates this week, from fresh language features in .NET 10 Preview and smarter Excel Python tools to improved resilience in Spark. New guides cover everything from disk cleanup to building dual-transport servers, reflecting a bigger focus on practical, modern, and cross-platform workflows.',
     content_hash = md5('Developers are getting meaningful updates this week, from fresh language features in .NET 10 Preview and smarter Excel Python tools to improved resilience in Spark. New guides cover everything from disk cleanup to building dual-transport servers, reflecting a bigger focus on practical, modern, and cross-platform workflows.

<!--excerpt_end-->

## Coding

### Major Platform Features and Updates: .NET 10 Preview, Excel Python Image Analysis, Spark Resilience

.NET 10 Preview 7 brings new cryptographic features, faster JSON serialization, better diagnostics, easier authentication, and stronger cross-platform support. The improvements boost usability, security, and help streamline cloud-native workflows.

The latest Excel update lets you use Python natively for image analysis tasks like blur detection, brightness checks, and metadata collection—bringing advanced vision analysis tools to everyday spreadsheet users.

Spark’s improved Iteration Panel makes file filtering and API failure handling easier, helping with smoother, more reliable development cycles.

- [.NET 10 Preview 7 Released: Key Updates for Libraries, ASP.NET Core, Blazor, and MAUI](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-7/)
- [Analyzing Images with Python in Excel: Now Natively Supported](https://techcommunity.microsoft.com/t5/microsoft-365-insider-blog/analyze-images-with-python-in-excel/ba-p/4440388)
- [Spark Resilience Improvements Enhance Reliability and Iteration Experience](https://github.blog/changelog/2025-08-13-spark-resilience-improvements)

### Guides for Modern, Cross-Platform Development: .NET Aspire, .NET MAUI, Browser-Based .NET

Step-by-step guides help teams get started with .NET Aspire for distributed systems, including boilerplate code, GitHub Actions integration, and custom metrics for ongoing monitoring. Visual Studio and .NET MAUI tutorials focus on building cross-platform mobile and desktop apps—with advice on UI optimization, file size reduction, and streamlined updates.

A walkthrough from Andrew Lock shows how to run .NET in the browser without Blazor, using WebAssembly templates and JavaScript interop for high-performance client apps.

- [Building Confident Application Systems with .NET Aspire: From Dev to Deployment](/azure/videos/building-confident-application-systems-with-net-aspire-from-dev-to-deployment)
- [Building Mobile and Desktop Apps with Visual Studio and .NET MAUI](/coding/videos/building-mobile-and-desktop-apps-with-visual-studio-and-net-maui)
- [Running .NET in the Browser Without Blazor Using WASM](https://andrewlock.net/running-dotnet-in-the-browser-without-blazor/)

### Language Evolution and the Future of Web Development: C# 14, ASP.NET Core & Blazor in .NET 10

A detailed look at C# 14 covers improved pattern matching, nullability support, and value types—making code safer and cleaner. Current .NET team previews for ASP.NET Core and Blazor in .NET 10 include modern security, diagnostics, WebAuthn support, integrated AI libraries, and faster project ramp-up with Aspire, helping developers stay current with critical web advances.

- [Highlights and Upcoming Features in C#: A Deep Dive into C# 14](/coding/videos/highlights-and-upcoming-features-in-c-a-deep-dive-into-c-14)
- [The Future of Web Development with ASP.NET Core & Blazor in .NET 10](/ai/videos/the-future-of-web-development-with-aspnet-core-and-blazor-in-net-10)

### Other Coding News

VS Code’s “Beast mode” rolls out improvements for batch edits, UI adjustments, and workflow enhancements, with a video guide to help users get started. Additional tutorials show how to simplify .NET mapping with Facet, automate disk cleanup with PowerShell, and build STDIO/HTTP dual-transport MCP servers for flexible cloud and local deployments.

- [VS Code Beast Mode Explained: Features and Usage](/coding/videos/vs-code-beast-mode-explained-features-and-usage)
- [Enhancing .NET Code: Using Facet Instead of Traditional Mapping](/coding/videos/enhancing-net-code-using-facet-instead-of-traditional-mapping)
- [Finding Large Directories and Recovering Lost Disk Space with PowerShell](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
- [Building a Dual-Transport MCP Server with .NET: STDIO and HTTP Support](https://techcommunity.microsoft.com/t5/microsoft-developer-community/one-mcp-server-two-transports-stdio-and-http/ba-p/4443915)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-08-18';

-- weekly-dotnet-roundup-2025-08-25  (2025-08-25)
UPDATE content_items
SET  content      = 'Developers benefit this week from updated toolchains and workflow features, helping with modernization and streamlining in both Git and .NET. Git delivers new storage and workflow improvements, while .NET introduces new approaches for testing, migration, and UI challenges.

<!--excerpt_end-->

## Coding

### Git 2.51: Storage, Workflow, and CLI Enhancements

Git 2.51 delivers enhancements to storage and workflow efficiency. The new cruft-free MIDX feature allows for duplication of reachable objects from cruft packs, resulting in smaller, faster repositories and up to 38% storage reduction. This is managed via the `repack.MIDXMustContainCruft` setting. The updated `git repack --path-walk` uses file layout to optimize delta compression and pack size.

A new stash interchange format allows linked ancestor commits, enabling stash export/import across devices. Scripting is improved via more accurate `git cat-file` submodule reporting; commit-graph Bloom filters offer accelerated path filtering in large repositories.

The commands `git switch` and `git restore` are now stable, while the deprecated `git whatchanged` is replaced by `git log --raw`. Looking ahead, Git 3.0 will default to SHA-256 and implement a new reftable backend, so users should start planning for migration. Updates to C99 support and patch submission workflows further modernize the codebase.

These updates maintain Git’s focus on workflow effectiveness and code modernization.

- [Key Updates in Git 2.51: Cruft-Free MIDX, Stash Interchange, and More](https://github.blog/open-source/git/highlights-from-git-2-51/)

### .NET Testing Modernization: CLI, TUnit Migration, and Email Workflow

.NET 10 now runs `dotnet test` using the Microsoft.Testing.Platform (MTP), replacing the previous VSTest engine. The update improves automation, performance, diagnostics, filtering, parallelism, and output. Developers should migrate tests to MTP, update configurations, and remove obsolete settings for simpler and faster solution builds.

Migration resources confirm that moving from xUnit to TUnit is direct, with analyzer and source generator support for parallel, NativeAOT-ready, and .NET Standard 2.0 projects. Guides cover assert conversion, snapshot usage, and CI integration.

A new tutorial explains how to implement reliable email sending within .NET, covering SMTP setup, formatting, debugging, and best practices for maintainable code.

- [Enhance your CLI testing workflow with the new dotnet test](https://devblogs.microsoft.com/dotnet/dotnet-test-with-mtp/)
- [Migrating an xUnit Test Project to TUnit: Experience, Issues, and Practical Steps](https://andrewlock.net/converting-an-xunit-project-to-tunit/)
- [Sending Email Correctly in .NET](/coding/videos/sending-email-correctly-in-net)

### .NET Application Modernization: Migration, WebView2, and Obsolete APIs

Migrating .NET Framework 4.8 applications to .NET 8 is now easier, with tools like Upgrade Assistant, Portability Analyzer, and Roslyn analyzers designed for incremental and batch migration. Documentation and automation help prioritize modernization and minimize risk.

These resources support ongoing themes around cross-platform modernization and multi-targeted project upgrades, including those for Aspire and MAUI. The focus is on easy migration of APIs and structuring large solution upgrades.

WebView2 now supports improved keyboard input mapping, simplifying desktop usability in WPF/WinForms applications. The new `CoreWebView2ControllerOptions.AllowHostInputProcessing` property restores expected keyboard behaviors, supporting better integration between host and browser-based UIs.

Additional tools are available for mapping obsolete APIs to .NET 8. This continues last week’s theme of smoother API and UI modernization.

- [Handling Keyboard Mapping in WebView2 with AllowHostInputProcessing](https://weblog.west-wind.com/posts/2025/Aug/20/Using-the-new-WebView2-AllowHostInputProcessing-Keyboard-Mapping-Feature)
- [Tools and Approaches for Migrating Obsolete .NET Framework APIs to .NET 8](https://techcommunity.microsoft.com/t5/tools/tool-or-approach-to-identify-and-replace-obsolete-net-framework/m-p/4446845#M161)',
     excerpt      = 'Developers benefit this week from updated toolchains and workflow features, helping with modernization and streamlining in both Git and .NET. Git delivers new storage and workflow improvements, while .NET introduces new approaches for testing, migration, and UI challenges.',
     content_hash = md5('Developers benefit this week from updated toolchains and workflow features, helping with modernization and streamlining in both Git and .NET. Git delivers new storage and workflow improvements, while .NET introduces new approaches for testing, migration, and UI challenges.

<!--excerpt_end-->

## Coding

### Git 2.51: Storage, Workflow, and CLI Enhancements

Git 2.51 delivers enhancements to storage and workflow efficiency. The new cruft-free MIDX feature allows for duplication of reachable objects from cruft packs, resulting in smaller, faster repositories and up to 38% storage reduction. This is managed via the `repack.MIDXMustContainCruft` setting. The updated `git repack --path-walk` uses file layout to optimize delta compression and pack size.

A new stash interchange format allows linked ancestor commits, enabling stash export/import across devices. Scripting is improved via more accurate `git cat-file` submodule reporting; commit-graph Bloom filters offer accelerated path filtering in large repositories.

The commands `git switch` and `git restore` are now stable, while the deprecated `git whatchanged` is replaced by `git log --raw`. Looking ahead, Git 3.0 will default to SHA-256 and implement a new reftable backend, so users should start planning for migration. Updates to C99 support and patch submission workflows further modernize the codebase.

These updates maintain Git’s focus on workflow effectiveness and code modernization.

- [Key Updates in Git 2.51: Cruft-Free MIDX, Stash Interchange, and More](https://github.blog/open-source/git/highlights-from-git-2-51/)

### .NET Testing Modernization: CLI, TUnit Migration, and Email Workflow

.NET 10 now runs `dotnet test` using the Microsoft.Testing.Platform (MTP), replacing the previous VSTest engine. The update improves automation, performance, diagnostics, filtering, parallelism, and output. Developers should migrate tests to MTP, update configurations, and remove obsolete settings for simpler and faster solution builds.

Migration resources confirm that moving from xUnit to TUnit is direct, with analyzer and source generator support for parallel, NativeAOT-ready, and .NET Standard 2.0 projects. Guides cover assert conversion, snapshot usage, and CI integration.

A new tutorial explains how to implement reliable email sending within .NET, covering SMTP setup, formatting, debugging, and best practices for maintainable code.

- [Enhance your CLI testing workflow with the new dotnet test](https://devblogs.microsoft.com/dotnet/dotnet-test-with-mtp/)
- [Migrating an xUnit Test Project to TUnit: Experience, Issues, and Practical Steps](https://andrewlock.net/converting-an-xunit-project-to-tunit/)
- [Sending Email Correctly in .NET](/coding/videos/sending-email-correctly-in-net)

### .NET Application Modernization: Migration, WebView2, and Obsolete APIs

Migrating .NET Framework 4.8 applications to .NET 8 is now easier, with tools like Upgrade Assistant, Portability Analyzer, and Roslyn analyzers designed for incremental and batch migration. Documentation and automation help prioritize modernization and minimize risk.

These resources support ongoing themes around cross-platform modernization and multi-targeted project upgrades, including those for Aspire and MAUI. The focus is on easy migration of APIs and structuring large solution upgrades.

WebView2 now supports improved keyboard input mapping, simplifying desktop usability in WPF/WinForms applications. The new `CoreWebView2ControllerOptions.AllowHostInputProcessing` property restores expected keyboard behaviors, supporting better integration between host and browser-based UIs.

Additional tools are available for mapping obsolete APIs to .NET 8. This continues last week’s theme of smoother API and UI modernization.

- [Handling Keyboard Mapping in WebView2 with AllowHostInputProcessing](https://weblog.west-wind.com/posts/2025/Aug/20/Using-the-new-WebView2-AllowHostInputProcessing-Keyboard-Mapping-Feature)
- [Tools and Approaches for Migrating Obsolete .NET Framework APIs to .NET 8](https://techcommunity.microsoft.com/t5/tools/tool-or-approach-to-identify-and-replace-obsolete-net-framework/m-p/4446845#M161)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-08-25';

-- weekly-dotnet-roundup-2025-09-01  (2025-09-01)
UPDATE content_items
SET  content      = 'Updates this week for coding include new features in .NET and C#, diagnostic tools, open sourcing of Windows Subsystem for Linux, and practical workflow guides. Microsoft continues its focus on open source and improving developer experience with new releases and troubleshooting content.

<!--excerpt_end-->

## Coding

### .NET and C# Language and Platform Enhancements

Nick Chapsas previews Discriminated Unions for C# 15/16, demonstrating better type safety and simplified code patterns akin to F#, TypeScript, and Rust. MauiReactor provides MVU architecture options for .NET MAUI UI development. EFCore.Visualizer lets Visual Studio users analyze Entity Framework Core query plans inside the IDE, continuing improvements in .NET tooling.

- [Exploring Discriminated Unions Coming to C# 15 and 16](/coding/videos/exploring-discriminated-unions-coming-to-c-15-and-16)
- [MauiReactor: Introducing the MVU Pattern for .NET MAUI](https://devblogs.microsoft.com/dotnet/mauireactor-mvu-for-dotnet-maui/)
- [EFCore.Visualizer: Analyze Entity Framework Core Query Plans in Visual Studio](https://devblogs.microsoft.com/dotnet/ef-core-visualizer-view-entity-framework-core-query-plan-inside-visual-studio/)

### Open Sourcing Windows Subsystem for Linux (WSL)

Microsoft has published the source code for WSL internals—including VM startup, filesystem mounting, and GPU handling—on GitHub, boosting community involvement and transparency. Developers have direct access for troubleshooting, customize workflow, and can contribute ideas for enhancements or fixes. Official resources welcome community collaboration and learning.

- [You open sourced WSL. What does that mean?](/coding/videos/you-open-sourced-wsl-what-does-that-mean)
- [Explaining the Open Sourcing of Windows Subsystem for Linux (WSL)](/coding/videos/explaining-the-open-sourcing-of-windows-subsystem-for-linux-wsl)

### Developer Experience and Workflow Tools

Aspire CLI for .NET streamlines app configuration, integrates cloud dependencies, and supports easy deployment to Azure, Docker Compose, or Kubernetes. Step-by-step guides for installation and commands aim to make distributed development more approachable. VS Code’s August iteration plan previews new terminal enhancements, agent features, and automation, opening discussion for community feedback.

- [Getting Started with the Aspire CLI](https://devblogs.microsoft.com/dotnet/getting-started-with-the-aspire-cli/)
- [Upcoming Features and Terminal Improvements in Visual Studio Code: August Iteration](/coding/videos/upcoming-features-and-terminal-improvements-in-visual-studio-code-august-iteration)

### Other Coding News

A troubleshooting guide for .NET Core on Alpine Linux explains fixes for native asset loading errors, including environment variable recommendations. A SharePoint branding resource provides practical steps for logo setup, themes, templates, and homepage configuration for consistent organizational branding.

- [Solving Native Library Loading Issues for .NET Core on Alpine Linux](https://andrewlock.net/fixing-an-old-dotnet-core-native-library-loading-issue-on-alpine/)
- [Branding Your SharePoint Site for Your Organization](https://dellenny.com/branding-your-sharepoint-site-for-your-organization/)',
     excerpt      = 'Updates this week for coding include new features in .NET and C#, diagnostic tools, open sourcing of Windows Subsystem for Linux, and practical workflow guides. Microsoft continues its focus on open source and improving developer experience with new releases and troubleshooting content.',
     content_hash = md5('Updates this week for coding include new features in .NET and C#, diagnostic tools, open sourcing of Windows Subsystem for Linux, and practical workflow guides. Microsoft continues its focus on open source and improving developer experience with new releases and troubleshooting content.

<!--excerpt_end-->

## Coding

### .NET and C# Language and Platform Enhancements

Nick Chapsas previews Discriminated Unions for C# 15/16, demonstrating better type safety and simplified code patterns akin to F#, TypeScript, and Rust. MauiReactor provides MVU architecture options for .NET MAUI UI development. EFCore.Visualizer lets Visual Studio users analyze Entity Framework Core query plans inside the IDE, continuing improvements in .NET tooling.

- [Exploring Discriminated Unions Coming to C# 15 and 16](/coding/videos/exploring-discriminated-unions-coming-to-c-15-and-16)
- [MauiReactor: Introducing the MVU Pattern for .NET MAUI](https://devblogs.microsoft.com/dotnet/mauireactor-mvu-for-dotnet-maui/)
- [EFCore.Visualizer: Analyze Entity Framework Core Query Plans in Visual Studio](https://devblogs.microsoft.com/dotnet/ef-core-visualizer-view-entity-framework-core-query-plan-inside-visual-studio/)

### Open Sourcing Windows Subsystem for Linux (WSL)

Microsoft has published the source code for WSL internals—including VM startup, filesystem mounting, and GPU handling—on GitHub, boosting community involvement and transparency. Developers have direct access for troubleshooting, customize workflow, and can contribute ideas for enhancements or fixes. Official resources welcome community collaboration and learning.

- [You open sourced WSL. What does that mean?](/coding/videos/you-open-sourced-wsl-what-does-that-mean)
- [Explaining the Open Sourcing of Windows Subsystem for Linux (WSL)](/coding/videos/explaining-the-open-sourcing-of-windows-subsystem-for-linux-wsl)

### Developer Experience and Workflow Tools

Aspire CLI for .NET streamlines app configuration, integrates cloud dependencies, and supports easy deployment to Azure, Docker Compose, or Kubernetes. Step-by-step guides for installation and commands aim to make distributed development more approachable. VS Code’s August iteration plan previews new terminal enhancements, agent features, and automation, opening discussion for community feedback.

- [Getting Started with the Aspire CLI](https://devblogs.microsoft.com/dotnet/getting-started-with-the-aspire-cli/)
- [Upcoming Features and Terminal Improvements in Visual Studio Code: August Iteration](/coding/videos/upcoming-features-and-terminal-improvements-in-visual-studio-code-august-iteration)

### Other Coding News

A troubleshooting guide for .NET Core on Alpine Linux explains fixes for native asset loading errors, including environment variable recommendations. A SharePoint branding resource provides practical steps for logo setup, themes, templates, and homepage configuration for consistent organizational branding.

- [Solving Native Library Loading Issues for .NET Core on Alpine Linux](https://andrewlock.net/fixing-an-old-dotnet-core-native-library-loading-issue-on-alpine/)
- [Branding Your SharePoint Site for Your Organization](https://dellenny.com/branding-your-sharepoint-site-for-your-organization/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-09-01';

-- weekly-dotnet-roundup-2025-09-08  (2025-09-08)
UPDATE content_items
SET  content      = 'This week features guides and platform enhancements in .NET, Blazor, .NET MAUI, and VS Code. Developers have new resources for building .NET CLI tools, web and mobile apps, and more secure and automated workflows. Further MCP and AI improvements in VS Code support efficient, streamlined daily development.

<!--excerpt_end-->

## Coding

### .NET Ecosystem: Tooling, Web, and Cross-Platform Updates

Continuing last week’s exploration of .NET tooling, Andrew Lock’s article covers how to create robust .NET tools, including tips on global vs. local installations, repeatable manifests, multi-targeting for compatibility, RollForward for stable versioning, and integration with CI/test automation. Developers will learn how to set up tools efficiently, manage versions, and avoid common CI issues.

The Blazor Internship Demo Fest highlights a new media component and improved navigation state, simplifying multimedia usage and app navigation in Blazor. These insights reinforce best practices in UI design and community engagement.

.NET MAUI’s Community Standup introduces Release Candidate 1, improved iPhone support, and new cross-platform features, with practical recommendations for mobile app delivery. Q&A and hands-on content help developers adapt to changing mobile targets—building on prior updates to release cycles and architectural patterns.

- [Using and Authoring .NET Tools: Multi-Targeting, CI, and Best Practices](https://andrewlock.net/using-and-authoring-dotnet-tools/)
- [Blazor Internship Demo Fest: New Components and Enhanced Navigation](/coding/videos/blazor-internship-demo-fest-new-components-and-enhanced-navigation)
- [.NET MAUI Community Standup: Release Candidates, iPhone Support, and Updates](/coding/videos/net-maui-community-standup-release-candidates-iphone-support-and-updates)

### VS Code and MCP: Workflow Automation and AI Integration

Reflecting last week’s work with MCP and Playwright, VS Code now has native MCP server support, allowing for secure authentication and automation of end-to-end tests. Developers are able to run protocol automation directly from their IDE alongside traditional code and CI activities. The integration with Playwright and GitHub MCP further enhances real-world automation.

Kent C. Dodds explains how automation, including authentication to AI coding support, fits into everyday development—demonstrating the shift from initial protocol configuration to stable production usage.

- [Building an MCP Inside VS Code and Exploring AI''s Impact with Kent C. Dodds](/ai/videos/building-an-mcp-inside-vs-code-and-exploring-ais-impact-with-kent-c-dodds)',
     excerpt      = 'This week features guides and platform enhancements in .NET, Blazor, .NET MAUI, and VS Code. Developers have new resources for building .NET CLI tools, web and mobile apps, and more secure and automated workflows. Further MCP and AI improvements in VS Code support efficient, streamlined daily development.',
     content_hash = md5('This week features guides and platform enhancements in .NET, Blazor, .NET MAUI, and VS Code. Developers have new resources for building .NET CLI tools, web and mobile apps, and more secure and automated workflows. Further MCP and AI improvements in VS Code support efficient, streamlined daily development.

<!--excerpt_end-->

## Coding

### .NET Ecosystem: Tooling, Web, and Cross-Platform Updates

Continuing last week’s exploration of .NET tooling, Andrew Lock’s article covers how to create robust .NET tools, including tips on global vs. local installations, repeatable manifests, multi-targeting for compatibility, RollForward for stable versioning, and integration with CI/test automation. Developers will learn how to set up tools efficiently, manage versions, and avoid common CI issues.

The Blazor Internship Demo Fest highlights a new media component and improved navigation state, simplifying multimedia usage and app navigation in Blazor. These insights reinforce best practices in UI design and community engagement.

.NET MAUI’s Community Standup introduces Release Candidate 1, improved iPhone support, and new cross-platform features, with practical recommendations for mobile app delivery. Q&A and hands-on content help developers adapt to changing mobile targets—building on prior updates to release cycles and architectural patterns.

- [Using and Authoring .NET Tools: Multi-Targeting, CI, and Best Practices](https://andrewlock.net/using-and-authoring-dotnet-tools/)
- [Blazor Internship Demo Fest: New Components and Enhanced Navigation](/coding/videos/blazor-internship-demo-fest-new-components-and-enhanced-navigation)
- [.NET MAUI Community Standup: Release Candidates, iPhone Support, and Updates](/coding/videos/net-maui-community-standup-release-candidates-iphone-support-and-updates)

### VS Code and MCP: Workflow Automation and AI Integration

Reflecting last week’s work with MCP and Playwright, VS Code now has native MCP server support, allowing for secure authentication and automation of end-to-end tests. Developers are able to run protocol automation directly from their IDE alongside traditional code and CI activities. The integration with Playwright and GitHub MCP further enhances real-world automation.

Kent C. Dodds explains how automation, including authentication to AI coding support, fits into everyday development—demonstrating the shift from initial protocol configuration to stable production usage.

- [Building an MCP Inside VS Code and Exploring AI''s Impact with Kent C. Dodds](/ai/videos/building-an-mcp-inside-vs-code-and-exploring-ais-impact-with-kent-c-dodds)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-09-08';

-- weekly-dotnet-roundup-2025-09-15  (2025-09-15)
UPDATE content_items
SET  content      = 'This section highlights new milestone releases and tooling updates from Microsoft. With .NET 10 approaching general release, developers are encouraged to start testing, while Visual Studio and VS Code both see new AI and automation features. Guides offer practical advice for modern development and migration.

<!--excerpt_end-->

## Coding

### .NET 10 Release Candidate 1 and Ecosystem Updates

.NET 10 RC1 is available for production testing. Previous updates focused on CLI improvements, multi-platform workflows, and new language features—now, RC1 can be tried with Visual Studio 2026 Insiders and VS Code’s C# Dev Kit. Updates include new quantum-safe cryptography APIs, faster UTF-8 hex-string conversions, and new tensor types for numerical computation. Runtime, SDK, and languages (C#, F#, VB) have refinements for increased stability.

ASP.NET Core features new Blazor component state persistence, expanded identity support, improved Minimal API validation, and better OpenAPI docs. Blazor continues UX and platform improvements, while .NET MAUI offers debugging, new UI events, enhanced refresh controls, and early CoreCLR support on Android. Windows Forms now has dark mode, improved themes, async capabilities, and easier renderer management. Entity Framework Core adds vector search in SQL Server, native JSON types, enhanced Cosmos DB support, and better complex type handling.

Developers are encouraged to validate their applications on RC1, check for breaking changes, join standups, and plan migrations. The .NET Unboxed event shares rollout details and technical Q&A.

- [Announcing .NET 10 Release Candidate 1](https://devblogs.microsoft.com/dotnet/dotnet-10-rc-1/)
- [.NET Unboxed - .NET 10 Release Candidate 1](/coding/videos/net-unboxed-net-10-release-candidate-1)

### Visual Studio 2026 Insiders and VS Code v1.104 Feature Enhancements

Visual Studio 2026 Insiders is a new early-access program for monthly IDE updates and direct community feedback. AI integration improves code completion, automated testing, and review—embeddeding GitHub Copilot Free for all users. Enhanced solution management, Profiler Agent, and faster performance are available for x64/Arm64. UI updates include new Fluent designs, themes, and user onboarding, shaped by user contributions.

VS Code v1.104 features Agent Mode for workspace automation—allowing developers to offload routine tasks and focus on coding. Additional updates include improved APIs for plugins, automated terminal approval, and new TODO management. Git Worktree support simplifies multi-branch reviews and development, enhancing last week’s prompt automation for collaboration.

- [Visual Studio 2026 Insiders Launch: Integrated AI, Performance, and Developer-Centric Upgrades](https://devblogs.microsoft.com/visualstudio/visual-studio-2026-insiders-is-here/)
- [Latest Features in Visual Studio Code: Agent Mode, Git Worktrees, and More](/ai/videos/latest-features-in-visual-studio-code-agent-mode-git-worktrees-and-more)
- [VS Code Live: Exploring Hidden Features in VS Code v1.104](/coding/videos/vs-code-live-exploring-hidden-features-in-vs-code-v1104)

### .NET Tooling, Packaging, and Servicing Releases

A new guide shows how .NET 10 enables expanded packaging for multi-targeted, reusable tools—developers can create NuGet packages for self-contained, trimmed, or ahead-of-time compiled tool distributions. The article includes deployment and configuration samples and recommends thorough testing ahead of general release.

Routine servicing for .NET 8 and .NET 9 in September 2025 brings bug fixes and reliability updates for ASP.NET Core, the SDK, and .NET Framework across all platforms. Developers should consult changelogs and apply fixes to maintain platform stability.

- [Packaging Self-Contained and Native AOT .NET Tools for NuGet: .NET 10 Preview](https://andrewlock.net/exploring-dotnet-10-preview-features-7-packaging-self-contained-and-native-aot-dotnet-tools-for-nuget/)
- [.NET and .NET Framework September 2025 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-september-2025-servicing-updates/)

### Data Access Strategies: Dapper vs Entity Framework Core

The .NET Data Community Standup compared Dapper and Entity Framework Core. Presenters shared concrete lessons—Dapper’s speed and simplicity versus EF Core’s feature set and maintainability. The session covered use cases, performance tips, pitfalls, and factors for teams deciding between direct SQL mapping and a higher-level ORM.

- [.NET Data Community Standup: Practical Dapper vs Entity Framework Core Comparison](/coding/videos/net-data-community-standup-practical-dapper-vs-entity-framework-core-comparison)',
     excerpt      = 'This section highlights new milestone releases and tooling updates from Microsoft. With .NET 10 approaching general release, developers are encouraged to start testing, while Visual Studio and VS Code both see new AI and automation features. Guides offer practical advice for modern development and migration.',
     content_hash = md5('This section highlights new milestone releases and tooling updates from Microsoft. With .NET 10 approaching general release, developers are encouraged to start testing, while Visual Studio and VS Code both see new AI and automation features. Guides offer practical advice for modern development and migration.

<!--excerpt_end-->

## Coding

### .NET 10 Release Candidate 1 and Ecosystem Updates

.NET 10 RC1 is available for production testing. Previous updates focused on CLI improvements, multi-platform workflows, and new language features—now, RC1 can be tried with Visual Studio 2026 Insiders and VS Code’s C# Dev Kit. Updates include new quantum-safe cryptography APIs, faster UTF-8 hex-string conversions, and new tensor types for numerical computation. Runtime, SDK, and languages (C#, F#, VB) have refinements for increased stability.

ASP.NET Core features new Blazor component state persistence, expanded identity support, improved Minimal API validation, and better OpenAPI docs. Blazor continues UX and platform improvements, while .NET MAUI offers debugging, new UI events, enhanced refresh controls, and early CoreCLR support on Android. Windows Forms now has dark mode, improved themes, async capabilities, and easier renderer management. Entity Framework Core adds vector search in SQL Server, native JSON types, enhanced Cosmos DB support, and better complex type handling.

Developers are encouraged to validate their applications on RC1, check for breaking changes, join standups, and plan migrations. The .NET Unboxed event shares rollout details and technical Q&A.

- [Announcing .NET 10 Release Candidate 1](https://devblogs.microsoft.com/dotnet/dotnet-10-rc-1/)
- [.NET Unboxed - .NET 10 Release Candidate 1](/coding/videos/net-unboxed-net-10-release-candidate-1)

### Visual Studio 2026 Insiders and VS Code v1.104 Feature Enhancements

Visual Studio 2026 Insiders is a new early-access program for monthly IDE updates and direct community feedback. AI integration improves code completion, automated testing, and review—embeddeding GitHub Copilot Free for all users. Enhanced solution management, Profiler Agent, and faster performance are available for x64/Arm64. UI updates include new Fluent designs, themes, and user onboarding, shaped by user contributions.

VS Code v1.104 features Agent Mode for workspace automation—allowing developers to offload routine tasks and focus on coding. Additional updates include improved APIs for plugins, automated terminal approval, and new TODO management. Git Worktree support simplifies multi-branch reviews and development, enhancing last week’s prompt automation for collaboration.

- [Visual Studio 2026 Insiders Launch: Integrated AI, Performance, and Developer-Centric Upgrades](https://devblogs.microsoft.com/visualstudio/visual-studio-2026-insiders-is-here/)
- [Latest Features in Visual Studio Code: Agent Mode, Git Worktrees, and More](/ai/videos/latest-features-in-visual-studio-code-agent-mode-git-worktrees-and-more)
- [VS Code Live: Exploring Hidden Features in VS Code v1.104](/coding/videos/vs-code-live-exploring-hidden-features-in-vs-code-v1104)

### .NET Tooling, Packaging, and Servicing Releases

A new guide shows how .NET 10 enables expanded packaging for multi-targeted, reusable tools—developers can create NuGet packages for self-contained, trimmed, or ahead-of-time compiled tool distributions. The article includes deployment and configuration samples and recommends thorough testing ahead of general release.

Routine servicing for .NET 8 and .NET 9 in September 2025 brings bug fixes and reliability updates for ASP.NET Core, the SDK, and .NET Framework across all platforms. Developers should consult changelogs and apply fixes to maintain platform stability.

- [Packaging Self-Contained and Native AOT .NET Tools for NuGet: .NET 10 Preview](https://andrewlock.net/exploring-dotnet-10-preview-features-7-packaging-self-contained-and-native-aot-dotnet-tools-for-nuget/)
- [.NET and .NET Framework September 2025 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-september-2025-servicing-updates/)

### Data Access Strategies: Dapper vs Entity Framework Core

The .NET Data Community Standup compared Dapper and Entity Framework Core. Presenters shared concrete lessons—Dapper’s speed and simplicity versus EF Core’s feature set and maintainability. The session covered use cases, performance tips, pitfalls, and factors for teams deciding between direct SQL mapping and a higher-level ORM.

- [.NET Data Community Standup: Practical Dapper vs Entity Framework Core Comparison](/coding/videos/net-data-community-standup-practical-dapper-vs-entity-framework-core-comparison)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-09-15';

-- weekly-dotnet-roundup-2025-09-22  (2025-09-22)
UPDATE content_items
SET  content      = 'Coding updates this week focus on practical improvements for .NET developers, tool packaging strategies, performance, and changes to support life cycles. As .NET 10 nears release, content covers actionable planning advice for evolving within the .NET ecosystem.

<!--excerpt_end-->

## Coding

### .NET 10 Tool Packaging, WebAssembly, and Support Lifecycle Changes

Andrew Lock’s expanded coverage from last week’s .NET 10 RC1 preview addresses platform-specific tool packaging, new schemas for DotnetToolSettings.xml, and approaches for dual packaging to maintain support for older SDKs, as illustrated by Datadog and NativeAOT examples—continuing migration support started earlier.

The latest ASP.NET Community Standup covers .NET 10’s WebAssembly runtime, outlining performance enhancements and migration tactics that build upon last week’s component feature improvements.

Microsoft will extend .NET Standard Term Support to 24 months beginning with .NET 9, directly addressing prior migration challenges and adopting past recommendations for update planning.

- [Supporting Platform-Specific .NET Tools on Older SDKs: .NET 10 Preview Deep Dive](https://andrewlock.net/exploring-dotnet-10-preview-features-8-supporting-platform-specific-dotnet-tools-on-old-sdks/)
- [ASP.NET Community Standup: .NET 10 WebAssembly Performance Enhancements](/coding/videos/aspnet-community-standup-net-10-webassembly-performance-enhancements)
- [.NET Standard Term Support (STS) Releases Will Be Supported for 24 Months Starting with .NET 9](https://devblogs.microsoft.com/dotnet/dotnet-sts-releases-supported-for-24-months/)',
     excerpt      = 'Coding updates this week focus on practical improvements for .NET developers, tool packaging strategies, performance, and changes to support life cycles. As .NET 10 nears release, content covers actionable planning advice for evolving within the .NET ecosystem.',
     content_hash = md5('Coding updates this week focus on practical improvements for .NET developers, tool packaging strategies, performance, and changes to support life cycles. As .NET 10 nears release, content covers actionable planning advice for evolving within the .NET ecosystem.

<!--excerpt_end-->

## Coding

### .NET 10 Tool Packaging, WebAssembly, and Support Lifecycle Changes

Andrew Lock’s expanded coverage from last week’s .NET 10 RC1 preview addresses platform-specific tool packaging, new schemas for DotnetToolSettings.xml, and approaches for dual packaging to maintain support for older SDKs, as illustrated by Datadog and NativeAOT examples—continuing migration support started earlier.

The latest ASP.NET Community Standup covers .NET 10’s WebAssembly runtime, outlining performance enhancements and migration tactics that build upon last week’s component feature improvements.

Microsoft will extend .NET Standard Term Support to 24 months beginning with .NET 9, directly addressing prior migration challenges and adopting past recommendations for update planning.

- [Supporting Platform-Specific .NET Tools on Older SDKs: .NET 10 Preview Deep Dive](https://andrewlock.net/exploring-dotnet-10-preview-features-8-supporting-platform-specific-dotnet-tools-on-old-sdks/)
- [ASP.NET Community Standup: .NET 10 WebAssembly Performance Enhancements](/coding/videos/aspnet-community-standup-net-10-webassembly-performance-enhancements)
- [.NET Standard Term Support (STS) Releases Will Be Supported for 24 Months Starting with .NET 9](https://devblogs.microsoft.com/dotnet/dotnet-sts-releases-supported-for-24-months/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-09-22';

-- weekly-dotnet-roundup-2025-09-29  (2025-09-29)
UPDATE content_items
SET  content      = 'This week’s coding highlights include updates in .NET development, new container tooling in Visual Studio, and practical advice on platform compliance, distributed workflows, and migration planning.

<!--excerpt_end-->

## Coding

### Visual Studio 2026 and Container Tooling

Visual Studio 2026 Insiders now supports Podman, enabling developers to use this daemonless, rootless container engine instead of Docker for increased security and flexibility. The IDE detects Podman automatically and offers tools for managing images, debugging, and working with containers from the terminal—making secure Linux container development more approachable.

- [Visual Studio 2026 Insiders: Using Podman for Container Development](https://devblogs.microsoft.com/blog/visual-studio-2026-insiders-using-podman-for-container-development)

### .NET Aspire 9.5 and Modern .NET Cloud-Native Development

.NET Aspire 9.5 provides improvements for distributed .NET applications, including a new ''aspire update'' CLI for managing SDK/package upgrades, improved dashboards, a single-file AppHost preview for fast prototyping, and color-coded telemetry. GenAI Visualizer aids model debugging, YARP supports static files, and integration with Azure DevTunnels supports local secure testing. Visual Studio 2026 picks up new Aspire tracing features, and migration guides offer help for upgrades from Aspire 8.x.

- [Announcing Aspire 9.5](https://devblogs.microsoft.com/dotnet/announcing-dotnet-aspire-95/)

### .NET MAUI: App Compliance, Migration, and Community Engagement

.NET MAUI applications must update to MAUI 9 to comply with Google Play''s 16 KB memory page rule for Android 15+. Guidance is available for checking dependencies and updating build tools. The MAUI Community Standup event in Prague continues focus on collaboration and ongoing platform improvements, reflecting recent compliance and migration support topics.

- [Preparing Your .NET MAUI Apps for Google Play’s 16 KB Page Size Requirement](https://devblogs.microsoft.com/dotnet/maui-google-play-16-kb-page-size-support/)
- [.NET MAUI Community Standup - Live in Prague with the .NET MAUI Team](/coding/videos/net-maui-community-standup-live-in-prague-with-the-net-maui-team)

### .NET Platform Strategy and Database Migrations

Microsoft has clarified support timelines for .NET LTS/STS releases. Nick Chapsas provides migration planning guidance, helping developers minimize upgrade risk. Jeremy Miller’s Data Community Standup compares Marten/PostgreSQL and Entity Framework Core, offering real-world migration Q&A for developers planning database changes.

- [Understanding Microsoft''s LTS/STS Changes for .NET Support](/coding/videos/understanding-microsofts-ltssts-changes-for-net-support)
- [.NET Data Community Standup: Jeremy Miller on Marten and Database Migrations](/coding/videos/net-data-community-standup-jeremy-miller-on-marten-and-database-migrations)

### Building Server-Side and CLI Tools with .NET

The latest ASP.NET Community Standup demonstrates a multi-user MCP server, highlighting collaborative code review and refactoring workflows. Andrew Lock’s guide on ''sleep-pc'' covers .NET Native AOT usage, Win32 integration, argument processing, and NuGet packaging for durable server-side and CLI tool creation.

- [ASP.NET Community Standup - Vibe Coding a C# MCP Server](/coding/videos/aspnet-community-standup-vibe-coding-a-c-mcp-server)
- [Building sleep-pc: A .NET Native AOT Tool for Automating Windows Sleep](https://andrewlock.net/sleep-pc-a-dotnet-tool-to-make-windows-sleep-after-a-timeout/)',
     excerpt      = 'This week’s coding highlights include updates in .NET development, new container tooling in Visual Studio, and practical advice on platform compliance, distributed workflows, and migration planning.',
     content_hash = md5('This week’s coding highlights include updates in .NET development, new container tooling in Visual Studio, and practical advice on platform compliance, distributed workflows, and migration planning.

<!--excerpt_end-->

## Coding

### Visual Studio 2026 and Container Tooling

Visual Studio 2026 Insiders now supports Podman, enabling developers to use this daemonless, rootless container engine instead of Docker for increased security and flexibility. The IDE detects Podman automatically and offers tools for managing images, debugging, and working with containers from the terminal—making secure Linux container development more approachable.

- [Visual Studio 2026 Insiders: Using Podman for Container Development](https://devblogs.microsoft.com/blog/visual-studio-2026-insiders-using-podman-for-container-development)

### .NET Aspire 9.5 and Modern .NET Cloud-Native Development

.NET Aspire 9.5 provides improvements for distributed .NET applications, including a new ''aspire update'' CLI for managing SDK/package upgrades, improved dashboards, a single-file AppHost preview for fast prototyping, and color-coded telemetry. GenAI Visualizer aids model debugging, YARP supports static files, and integration with Azure DevTunnels supports local secure testing. Visual Studio 2026 picks up new Aspire tracing features, and migration guides offer help for upgrades from Aspire 8.x.

- [Announcing Aspire 9.5](https://devblogs.microsoft.com/dotnet/announcing-dotnet-aspire-95/)

### .NET MAUI: App Compliance, Migration, and Community Engagement

.NET MAUI applications must update to MAUI 9 to comply with Google Play''s 16 KB memory page rule for Android 15+. Guidance is available for checking dependencies and updating build tools. The MAUI Community Standup event in Prague continues focus on collaboration and ongoing platform improvements, reflecting recent compliance and migration support topics.

- [Preparing Your .NET MAUI Apps for Google Play’s 16 KB Page Size Requirement](https://devblogs.microsoft.com/dotnet/maui-google-play-16-kb-page-size-support/)
- [.NET MAUI Community Standup - Live in Prague with the .NET MAUI Team](/coding/videos/net-maui-community-standup-live-in-prague-with-the-net-maui-team)

### .NET Platform Strategy and Database Migrations

Microsoft has clarified support timelines for .NET LTS/STS releases. Nick Chapsas provides migration planning guidance, helping developers minimize upgrade risk. Jeremy Miller’s Data Community Standup compares Marten/PostgreSQL and Entity Framework Core, offering real-world migration Q&A for developers planning database changes.

- [Understanding Microsoft''s LTS/STS Changes for .NET Support](/coding/videos/understanding-microsofts-ltssts-changes-for-net-support)
- [.NET Data Community Standup: Jeremy Miller on Marten and Database Migrations](/coding/videos/net-data-community-standup-jeremy-miller-on-marten-and-database-migrations)

### Building Server-Side and CLI Tools with .NET

The latest ASP.NET Community Standup demonstrates a multi-user MCP server, highlighting collaborative code review and refactoring workflows. Andrew Lock’s guide on ''sleep-pc'' covers .NET Native AOT usage, Win32 integration, argument processing, and NuGet packaging for durable server-side and CLI tool creation.

- [ASP.NET Community Standup - Vibe Coding a C# MCP Server](/coding/videos/aspnet-community-standup-vibe-coding-a-c-mcp-server)
- [Building sleep-pc: A .NET Native AOT Tool for Automating Windows Sleep](https://andrewlock.net/sleep-pc-a-dotnet-tool-to-make-windows-sleep-after-a-timeout/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-09-29';

-- weekly-dotnet-roundup-2025-10-06  (2025-10-06)
UPDATE content_items
SET  content      = 'Coding updates this week highlight improvements in Visual Studio and .NET, emphasizing better performance, day-to-day workflow enhancements, and clear API design. New frameworks and previews focus on more efficient, maintainable solutions for modern development.

<!--excerpt_end-->

## Coding

### Visual Studio 2026 Insiders: TypeScript 7 Native Preview and Razor Tooling Evolution

Visual Studio 2026 Insiders now offers a native preview for TypeScript 7, following last week’s advances in container tools and Aspire tracing. Native implementation yields faster compile times and reduced memory usage—empirical data from large projects (VS Code, Playwright) illustrates these gains. Microsoft is asking for community feedback to guide further changes.

Razor tooling updates improve Hot Reload and editing within the ASP.NET Community Standup, reflecting ongoing efforts for more responsive development and real-time feedback, with fewer build interruptions—continuing the consistent expansion of Visual Studio features.

- [TypeScript 7 Native Preview Now Available in Visual Studio 2026 Insiders](https://devblogs.microsoft.com/blog/typescript-7-native-preview-in-visual-studio-2026)
- [ASP.NET Community Standup - Razor Reloaded](/coding/videos/aspnet-community-standup-razor-reloaded)

### Coding Agents and Workflow Automation in Visual Studio Code

Coding agent integration in VS Code is further refined to improve speed and accuracy, building on last week’s MCP-based automation and collaborative development advances. Feedback mechanisms allow developers to directly influence future improvements, promoting real-world, robust workflow automation.

- [Latest Updates to Coding Agent Integration in Visual Studio Code](/coding/videos/latest-updates-to-coding-agent-integration-in-visual-studio-code)

### Modern ASP.NET API Architecture with FastEndpoints and the REPR Pattern

Guidance for ASP.NET API design now favors the REPR (Request, Endpoint, Response) pattern with FastEndpoints, stepping away from the older controller structure featured last week. Marcel Medina’s tutorial provides clear, testable examples that support migration and database management, focusing on maintainable, scalable solutions.

- [Simplifying ASP.NET API Design with the REPR Pattern and FastEndpoints](/coding/videos/simplifying-aspnet-api-design-with-the-repr-pattern-and-fastendpoints)

### Other Coding News

A timely reminder steers developers toward maximizing Visual Studio subscription perks, including monthly Azure credits, access to Microsoft dev/test software, and training portals such as Pluralsight and Cloud Academy. These tips support ongoing .NET lifecycle and migration planning for more efficient workflows.

- [Unlocking the Hidden Value of Your Visual Studio Subscription](https://devblogs.microsoft.com/visualstudio/unlock-vss-benefits-myvisualstudio/)',
     excerpt      = 'Coding updates this week highlight improvements in Visual Studio and .NET, emphasizing better performance, day-to-day workflow enhancements, and clear API design. New frameworks and previews focus on more efficient, maintainable solutions for modern development.',
     content_hash = md5('Coding updates this week highlight improvements in Visual Studio and .NET, emphasizing better performance, day-to-day workflow enhancements, and clear API design. New frameworks and previews focus on more efficient, maintainable solutions for modern development.

<!--excerpt_end-->

## Coding

### Visual Studio 2026 Insiders: TypeScript 7 Native Preview and Razor Tooling Evolution

Visual Studio 2026 Insiders now offers a native preview for TypeScript 7, following last week’s advances in container tools and Aspire tracing. Native implementation yields faster compile times and reduced memory usage—empirical data from large projects (VS Code, Playwright) illustrates these gains. Microsoft is asking for community feedback to guide further changes.

Razor tooling updates improve Hot Reload and editing within the ASP.NET Community Standup, reflecting ongoing efforts for more responsive development and real-time feedback, with fewer build interruptions—continuing the consistent expansion of Visual Studio features.

- [TypeScript 7 Native Preview Now Available in Visual Studio 2026 Insiders](https://devblogs.microsoft.com/blog/typescript-7-native-preview-in-visual-studio-2026)
- [ASP.NET Community Standup - Razor Reloaded](/coding/videos/aspnet-community-standup-razor-reloaded)

### Coding Agents and Workflow Automation in Visual Studio Code

Coding agent integration in VS Code is further refined to improve speed and accuracy, building on last week’s MCP-based automation and collaborative development advances. Feedback mechanisms allow developers to directly influence future improvements, promoting real-world, robust workflow automation.

- [Latest Updates to Coding Agent Integration in Visual Studio Code](/coding/videos/latest-updates-to-coding-agent-integration-in-visual-studio-code)

### Modern ASP.NET API Architecture with FastEndpoints and the REPR Pattern

Guidance for ASP.NET API design now favors the REPR (Request, Endpoint, Response) pattern with FastEndpoints, stepping away from the older controller structure featured last week. Marcel Medina’s tutorial provides clear, testable examples that support migration and database management, focusing on maintainable, scalable solutions.

- [Simplifying ASP.NET API Design with the REPR Pattern and FastEndpoints](/coding/videos/simplifying-aspnet-api-design-with-the-repr-pattern-and-fastendpoints)

### Other Coding News

A timely reminder steers developers toward maximizing Visual Studio subscription perks, including monthly Azure credits, access to Microsoft dev/test software, and training portals such as Pluralsight and Cloud Academy. These tips support ongoing .NET lifecycle and migration planning for more efficient workflows.

- [Unlocking the Hidden Value of Your Visual Studio Subscription](https://devblogs.microsoft.com/visualstudio/unlock-vss-benefits-myvisualstudio/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-10-06';

-- weekly-dotnet-roundup-2025-10-13  (2025-10-13)
UPDATE content_items
SET  content      = 'Coding news this week centers on practical updates for developer workflows, including platform releases, debugging, open source compliance, AI-supported code assistance, productivity tools, improved .NET memory management, and guidance for licensing in long-term open source projects.

<!--excerpt_end-->

## Coding

### Visual Studio Code: AI, Copilot, and MCP Advancements

VS Code’s September 2025 update (v1.105) carries forward agentic automation and collaborative MCP workflows from last week. In-editor AI-powered merge conflict resolution uses chat agents, advancing previous source control features.

Chain-of-thought debugging and improved session management for Copilot Chat enhance transparency and explainability, responding to earlier developer feedback. BYOK model previews add workflow flexibility, continuing registry and protocol expansion.

The MCP marketplace preview in Extensions follows registry improvements, making MCP server discovery easier. Customization and agent development become more central as protocol use expands.

Shell and terminal configuration updates, pull request enhancements, and agent-driven validation integrate with core test tooling—in line with past agentic workflow updates. These changes reinforce VS Code’s community-based, developer-driven evolution.

- [Visual Studio Code September 2025 Release (v1.105): AI, MCP, and Developer Enhancements](https://code.visualstudio.com/updates/v1_105)

### Application Performance and Stability: .NET 10 GC and WPF Troubleshooting

Prepping for .NET 10, developers get a deep dive into DATAS GC, which adapts heap sizing for real memory use—targeting containers, Kubernetes, and web apps. Configuration, performance tuning, and diagnostic instructions support safe migration.

For WPF app unresponsiveness with USB pen devices, guidance recommends disabling stylus/touch handlers using an AppContext switch; apps that require these features will need other fixes. Microsoft’s findings point to memory and deadlock issues, with a permanent solution yet to arrive.

- [Preparing for the .NET 10 GC: Understanding and Tuning DATAS](https://devblogs.microsoft.com/dotnet/preparing-for-dotnet-10-gc/)
- [WPF Apps Becoming Unresponsive after USB Pen Device Hotplug](https://techcommunity.microsoft.com/t5/app-development/wpf-application-becomes-unresponsive-after-plugging-unplugging/m-p/4459751#M1274)

### Open Source Licensing Guidance for .NET Foundation Projects

The .NET Foundation clarified its licensing guide, stating only permissive OSI licenses (MIT, Apache 2.0, BSD, ISC) are allowed for main code and dependencies. Copyleft (GPL, AGPL, RPL) is not accepted to avoid issues in commercial applications. The document discusses project governance, corrects myths, and lists monetization options, citing AutoMapper as a past example. Maintainers and teams can use compliance checks and scenario reviews in policy updates for sustainable open source projects.

- [.NET Foundation License Compatibility Guide](https://dotnetfoundation.org/news-events/detail/license-compatibility-guide)',
     excerpt      = 'Coding news this week centers on practical updates for developer workflows, including platform releases, debugging, open source compliance, AI-supported code assistance, productivity tools, improved .NET memory management, and guidance for licensing in long-term open source projects.',
     content_hash = md5('Coding news this week centers on practical updates for developer workflows, including platform releases, debugging, open source compliance, AI-supported code assistance, productivity tools, improved .NET memory management, and guidance for licensing in long-term open source projects.

<!--excerpt_end-->

## Coding

### Visual Studio Code: AI, Copilot, and MCP Advancements

VS Code’s September 2025 update (v1.105) carries forward agentic automation and collaborative MCP workflows from last week. In-editor AI-powered merge conflict resolution uses chat agents, advancing previous source control features.

Chain-of-thought debugging and improved session management for Copilot Chat enhance transparency and explainability, responding to earlier developer feedback. BYOK model previews add workflow flexibility, continuing registry and protocol expansion.

The MCP marketplace preview in Extensions follows registry improvements, making MCP server discovery easier. Customization and agent development become more central as protocol use expands.

Shell and terminal configuration updates, pull request enhancements, and agent-driven validation integrate with core test tooling—in line with past agentic workflow updates. These changes reinforce VS Code’s community-based, developer-driven evolution.

- [Visual Studio Code September 2025 Release (v1.105): AI, MCP, and Developer Enhancements](https://code.visualstudio.com/updates/v1_105)

### Application Performance and Stability: .NET 10 GC and WPF Troubleshooting

Prepping for .NET 10, developers get a deep dive into DATAS GC, which adapts heap sizing for real memory use—targeting containers, Kubernetes, and web apps. Configuration, performance tuning, and diagnostic instructions support safe migration.

For WPF app unresponsiveness with USB pen devices, guidance recommends disabling stylus/touch handlers using an AppContext switch; apps that require these features will need other fixes. Microsoft’s findings point to memory and deadlock issues, with a permanent solution yet to arrive.

- [Preparing for the .NET 10 GC: Understanding and Tuning DATAS](https://devblogs.microsoft.com/dotnet/preparing-for-dotnet-10-gc/)
- [WPF Apps Becoming Unresponsive after USB Pen Device Hotplug](https://techcommunity.microsoft.com/t5/app-development/wpf-application-becomes-unresponsive-after-plugging-unplugging/m-p/4459751#M1274)

### Open Source Licensing Guidance for .NET Foundation Projects

The .NET Foundation clarified its licensing guide, stating only permissive OSI licenses (MIT, Apache 2.0, BSD, ISC) are allowed for main code and dependencies. Copyleft (GPL, AGPL, RPL) is not accepted to avoid issues in commercial applications. The document discusses project governance, corrects myths, and lists monetization options, citing AutoMapper as a past example. Maintainers and teams can use compliance checks and scenario reviews in policy updates for sustainable open source projects.

- [.NET Foundation License Compatibility Guide](https://dotnetfoundation.org/news-events/detail/license-compatibility-guide)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-10-13';

-- weekly-dotnet-roundup-2025-10-20  (2025-10-20)
UPDATE content_items
SET  content      = 'The .NET platform saw the arrival of .NET 10 RC2, new security patches, and updated workflows for automating design and console interfaces, laying the groundwork for a modern and secure application environment.

<!--excerpt_end-->

## Coding

### .NET Platform Updates and Security Maintenance

.NET 10 RC2 is ready for use in production, bringing features like upgraded microphone control, SafeAreaEdges in .NET MAUI, XAML source generation, platform compatibility improvements, extended JSON support in EF Core, and enhanced support for .NET MSBuild tasks. Full details are available in the release notes. Developers are encouraged to validate applications and check documentation before the GA release.

Concurrent with other platform and debugging updates, RC2 continues to improve reliability.

Security updates for October 2025 address issues in .NET 8.0, 9.0, and legacy .NET Framework versions, patching vulnerabilities like information disclosure, feature bypass concerns, denial of service, and remote code execution. The fixes extend to runtime, SDK, ASP.NET Core, and container images; teams should update promptly and use provided support materials.

- [Announcing .NET 10 Release Candidate 2](https://devblogs.microsoft.com/dotnet/dotnet-10-rc-2/)
- [.NET and .NET Framework October 2025 Servicing Updates: Security Fixes and Release Details](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-october-2025-servicing-updates/)

### Developer Workflow Enhancements: Design, Console UIs, and Tutorials

A live VS Code session demonstrates how to connect Figma MCP Server with Code Connect, making it easier to synchronize design tokens and produce up-to-date code from designs in Figma to Visual Studio Code.

This aligns with recent MCP updates in VS Code, providing more custom and integrated workflows.

A new guide highlights how to use RazorConsole to build interactive, visually rich .NET console apps, showing how to integrate RazorConsole, manage layouts, and create modern UIs. This encourages teams to bring new capabilities to CLI tools.

Building on last week’s discussions on debugging and workflow improvement, these tools enhance .NET development experiences and customizability.

- [VS Code Live: Integrating Figma MCP Server with Code Connect](/coding/videos/vs-code-live-integrating-figma-mcp-server-with-code-connect)
- [Building the Coolest Console Apps in .NET](/coding/videos/building-the-coolest-console-apps-in-net)',
     excerpt      = 'The .NET platform saw the arrival of .NET 10 RC2, new security patches, and updated workflows for automating design and console interfaces, laying the groundwork for a modern and secure application environment.',
     content_hash = md5('The .NET platform saw the arrival of .NET 10 RC2, new security patches, and updated workflows for automating design and console interfaces, laying the groundwork for a modern and secure application environment.

<!--excerpt_end-->

## Coding

### .NET Platform Updates and Security Maintenance

.NET 10 RC2 is ready for use in production, bringing features like upgraded microphone control, SafeAreaEdges in .NET MAUI, XAML source generation, platform compatibility improvements, extended JSON support in EF Core, and enhanced support for .NET MSBuild tasks. Full details are available in the release notes. Developers are encouraged to validate applications and check documentation before the GA release.

Concurrent with other platform and debugging updates, RC2 continues to improve reliability.

Security updates for October 2025 address issues in .NET 8.0, 9.0, and legacy .NET Framework versions, patching vulnerabilities like information disclosure, feature bypass concerns, denial of service, and remote code execution. The fixes extend to runtime, SDK, ASP.NET Core, and container images; teams should update promptly and use provided support materials.

- [Announcing .NET 10 Release Candidate 2](https://devblogs.microsoft.com/dotnet/dotnet-10-rc-2/)
- [.NET and .NET Framework October 2025 Servicing Updates: Security Fixes and Release Details](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-october-2025-servicing-updates/)

### Developer Workflow Enhancements: Design, Console UIs, and Tutorials

A live VS Code session demonstrates how to connect Figma MCP Server with Code Connect, making it easier to synchronize design tokens and produce up-to-date code from designs in Figma to Visual Studio Code.

This aligns with recent MCP updates in VS Code, providing more custom and integrated workflows.

A new guide highlights how to use RazorConsole to build interactive, visually rich .NET console apps, showing how to integrate RazorConsole, manage layouts, and create modern UIs. This encourages teams to bring new capabilities to CLI tools.

Building on last week’s discussions on debugging and workflow improvement, these tools enhance .NET development experiences and customizability.

- [VS Code Live: Integrating Figma MCP Server with Code Connect](/coding/videos/vs-code-live-integrating-figma-mcp-server-with-code-connect)
- [Building the Coolest Console Apps in .NET](/coding/videos/building-the-coolest-console-apps-in-net)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-10-20';

-- weekly-dotnet-roundup-2025-10-27  (2025-10-27)
UPDATE content_items
SET  content      = 'Coding resources this week provide guides and new tools for smoother workflows, easier migration, and new hands-on experience. There’s a strong sense of continuity in the ecosystem, with changes to extension support, open source funding options, and game development help for C#, Godot, and distributed .NET caching. Tutorials span beginner-friendly introductions to advanced debugging and prototyping, assisting both new and experienced developers.

<!--excerpt_end-->

## Coding

### Godot Engine with .NET and C#

Picking up from last week''s .NET and workflow tutorials, this week the Godot C# Essentials series wraps up with tips for designing responsive UIs using Control Nodes, containers, and signals—useful for features like crafting or inventory screens. Fresh articles explain event-driven code, Area3D collisions, and how to use signals for input, broadening strategies for prototyping.

Additional material on CSG nodes assists with 3D game development. Beginners have access to thorough onboarding guides for installing Godot with .NET, setting up VS Code and key extensions, and adding GitHub version control—creating a smooth path to professional workflows.

Core guides reinforce onboarding, covering player controls, user interfaces, and C# scripting integration. Tutorials on scripting basics—input mapping, movement, lifecycle methods, inspector variables, and debugging in VS Code—reflect last week''s focus on tool improvement and step-by-step learning.

- [Building Responsive UI in Godot C# with Control Nodes](/coding/videos/building-responsive-ui-in-godot-c-with-control-nodes)
- [Interactions in Godot C#: Handling Player Input with Signals and Collision Detection](/coding/videos/interactions-in-godot-c-handling-player-input-with-signals-and-collision-detection)
- [Prototyping Game Environments in Godot C# with CSG Nodes](/coding/videos/prototyping-game-environments-in-godot-c-with-csg-nodes)
- [Scenes and Nodes - The Core Building Blocks in Godot with C# (Part 4 of 9)](/coding/videos/scenes-and-nodes-the-core-building-blocks-in-godot-with-c-part-4-of-9)
- [Installing Godot with .NET and Setting Up VS Code for C# Game Development](/coding/videos/installing-godot-with-net-and-setting-up-vs-code-for-c-game-development)
- [Introduction to Game Development with C# in Godot: Beginner Essentials](/coding/videos/introduction-to-game-development-with-c-in-godot-beginner-essentials)
- [Scripting Basics in Godot: Writing and Attaching C# Scripts (Part 5 of 9)](/coding/videos/scripting-basics-in-godot-writing-and-attaching-c-scripts-part-5-of-9)
- [Engine Overview: Navigating the Godot Editor with C# (Part 3 of 9)](/coding/videos/engine-overview-navigating-the-godot-editor-with-c-part-3-of-9)
- [Debugging Godot C# Games with Visual Studio Code (Godot Series, Part 6)](/coding/videos/debugging-godot-c-games-with-visual-studio-code-godot-series-part-6)

### .NET Ecosystem: Migration, Sponsorship, and Caching

Coming after last week’s .NET 10 RC2 and security updates, Visual Studio 2026 now introduces an improved compatibility model for easier extension migration. New API versioning helps extension developers reduce maintenance, progressing the workflow upgrade story.

NuGet.org launches a Sponsorship feature, allowing open source maintainers to share direct links to funding platforms—continuing discussion from last week about sustainability in the ecosystem.

Distributed caching in .NET gains a new option: Microsoft.Extensions.Caching.Postgres, which performs well and includes features like unlogged tables, supporting scalability and reliability in cloud applications as highlighted in previous updates.

- [Effortless Extension Migration in Visual Studio 2026: Modern Compatibility Model for Developers](https://devblogs.microsoft.com/visualstudio/modernizing-visual-studio-extension-compatibility-effortless-migration-for-extension-developers-and-users/)
- [Announcing Sponsorship on NuGet.org](https://devblogs.microsoft.com/dotnet/announcing-sponsorship-on-nugetdotorg-for-maintainer-appreciation/)
- [Postgres as a Distributed Cache Unlocks Speed and Simplicity for Modern .NET Workloads](https://techcommunity.microsoft.com/t5/microsoft-developer-community/postgres-as-a-distributed-cache-unlocks-speed-and-simplicity-for/ba-p/4462139)

### ASP.NET Core Endpoint Management

Updates in security and routing include new guides on adding metadata to ASP.NET Core endpoints and managing fallback routes. These tutorials deliver practical examples and match ongoing recommendations for robust and secure platform development.

- [Adding Metadata to Fallback Endpoints in ASP.NET Core](https://andrewlock.net/adding-metadata-to-fallback-endpoints-in-aspnetcore/)

### Other Coding News

Developer stories and DIY toolmaking continue from last week, highlighted by a GitHub Podcast discussing motivations for building custom utilities and Copilot’s role in supporting those efforts. The episode extends coverage on workflow personalization, toolchain improvement, and script development.

- [Building Tools and the Future of DIY Development: GitHub Podcast Episode 3](/ai/videos/building-tools-and-the-future-of-diy-development-github-podcast-episode-3)',
     excerpt      = 'Coding resources this week provide guides and new tools for smoother workflows, easier migration, and new hands-on experience. There’s a strong sense of continuity in the ecosystem, with changes to extension support, open source funding options, and game development help for C#, Godot, and distributed .NET caching. Tutorials span beginner-friendly introductions to advanced debugging and prototyping, assisting both new and experienced developers.',
     content_hash = md5('Coding resources this week provide guides and new tools for smoother workflows, easier migration, and new hands-on experience. There’s a strong sense of continuity in the ecosystem, with changes to extension support, open source funding options, and game development help for C#, Godot, and distributed .NET caching. Tutorials span beginner-friendly introductions to advanced debugging and prototyping, assisting both new and experienced developers.

<!--excerpt_end-->

## Coding

### Godot Engine with .NET and C#

Picking up from last week''s .NET and workflow tutorials, this week the Godot C# Essentials series wraps up with tips for designing responsive UIs using Control Nodes, containers, and signals—useful for features like crafting or inventory screens. Fresh articles explain event-driven code, Area3D collisions, and how to use signals for input, broadening strategies for prototyping.

Additional material on CSG nodes assists with 3D game development. Beginners have access to thorough onboarding guides for installing Godot with .NET, setting up VS Code and key extensions, and adding GitHub version control—creating a smooth path to professional workflows.

Core guides reinforce onboarding, covering player controls, user interfaces, and C# scripting integration. Tutorials on scripting basics—input mapping, movement, lifecycle methods, inspector variables, and debugging in VS Code—reflect last week''s focus on tool improvement and step-by-step learning.

- [Building Responsive UI in Godot C# with Control Nodes](/coding/videos/building-responsive-ui-in-godot-c-with-control-nodes)
- [Interactions in Godot C#: Handling Player Input with Signals and Collision Detection](/coding/videos/interactions-in-godot-c-handling-player-input-with-signals-and-collision-detection)
- [Prototyping Game Environments in Godot C# with CSG Nodes](/coding/videos/prototyping-game-environments-in-godot-c-with-csg-nodes)
- [Scenes and Nodes - The Core Building Blocks in Godot with C# (Part 4 of 9)](/coding/videos/scenes-and-nodes-the-core-building-blocks-in-godot-with-c-part-4-of-9)
- [Installing Godot with .NET and Setting Up VS Code for C# Game Development](/coding/videos/installing-godot-with-net-and-setting-up-vs-code-for-c-game-development)
- [Introduction to Game Development with C# in Godot: Beginner Essentials](/coding/videos/introduction-to-game-development-with-c-in-godot-beginner-essentials)
- [Scripting Basics in Godot: Writing and Attaching C# Scripts (Part 5 of 9)](/coding/videos/scripting-basics-in-godot-writing-and-attaching-c-scripts-part-5-of-9)
- [Engine Overview: Navigating the Godot Editor with C# (Part 3 of 9)](/coding/videos/engine-overview-navigating-the-godot-editor-with-c-part-3-of-9)
- [Debugging Godot C# Games with Visual Studio Code (Godot Series, Part 6)](/coding/videos/debugging-godot-c-games-with-visual-studio-code-godot-series-part-6)

### .NET Ecosystem: Migration, Sponsorship, and Caching

Coming after last week’s .NET 10 RC2 and security updates, Visual Studio 2026 now introduces an improved compatibility model for easier extension migration. New API versioning helps extension developers reduce maintenance, progressing the workflow upgrade story.

NuGet.org launches a Sponsorship feature, allowing open source maintainers to share direct links to funding platforms—continuing discussion from last week about sustainability in the ecosystem.

Distributed caching in .NET gains a new option: Microsoft.Extensions.Caching.Postgres, which performs well and includes features like unlogged tables, supporting scalability and reliability in cloud applications as highlighted in previous updates.

- [Effortless Extension Migration in Visual Studio 2026: Modern Compatibility Model for Developers](https://devblogs.microsoft.com/visualstudio/modernizing-visual-studio-extension-compatibility-effortless-migration-for-extension-developers-and-users/)
- [Announcing Sponsorship on NuGet.org](https://devblogs.microsoft.com/dotnet/announcing-sponsorship-on-nugetdotorg-for-maintainer-appreciation/)
- [Postgres as a Distributed Cache Unlocks Speed and Simplicity for Modern .NET Workloads](https://techcommunity.microsoft.com/t5/microsoft-developer-community/postgres-as-a-distributed-cache-unlocks-speed-and-simplicity-for/ba-p/4462139)

### ASP.NET Core Endpoint Management

Updates in security and routing include new guides on adding metadata to ASP.NET Core endpoints and managing fallback routes. These tutorials deliver practical examples and match ongoing recommendations for robust and secure platform development.

- [Adding Metadata to Fallback Endpoints in ASP.NET Core](https://andrewlock.net/adding-metadata-to-fallback-endpoints-in-aspnetcore/)

### Other Coding News

Developer stories and DIY toolmaking continue from last week, highlighted by a GitHub Podcast discussing motivations for building custom utilities and Copilot’s role in supporting those efforts. The episode extends coverage on workflow personalization, toolchain improvement, and script development.

- [Building Tools and the Future of DIY Development: GitHub Podcast Episode 3](/ai/videos/building-tools-and-the-future-of-diy-development-github-podcast-episode-3)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-10-27';

-- weekly-dotnet-roundup-2025-11-03  (2025-11-03)
UPDATE content_items
SET  content      = 'This week, developers benefited from future-focused events, hands-on tutorials, and ongoing ecosystem discussions. New resources help developers build their skills, contribute to open-source projects, and explore tooling in both Microsoft and open-source environments. The upcoming .NET Conf introduces new technologies, while the GitHub Game Off 2025 inspires creative game design. Tutorials expand tool customization, and funding analysis continues the dialogue around open-source sustainability.

<!--excerpt_end-->

## Coding

### .NET and Visual Studio: Events and Ecosystem Evolution

In anticipation of .NET Conf 2025, developers look forward to previews of Visual Studio 2026, .NET 10, and broader Copilot support. These sessions highlight easier upgrades and expanded cross-platform, AI-powered coding techniques. The sustainability conversation, guided by Nick Chapsas, addresses open-source library contributions, volunteer burnout, and donor options—following the recent rollout of NuGet.org Sponsorships.

- [Join .NET Conf to Explore Visual Studio 2026 and .NET Innovation](https://devblogs.microsoft.com/visualstudio/join-us-at-net-conf-dive-into-the-future-of-development-with-visual-studio-2026/)
- [.NET Libraries: Monetization Models and Open Source Challenges](/coding/videos/net-libraries-monetization-models-and-open-source-challenges)

### PowerToys and Windows 11: Customization and Productivity Automation

Building on previous extension and toolchain updates, Kayla Cinnamon’s PowerToys Command Palette guide introduces end-to-end development, packaging, and testing in Visual Studio. These tutorials are suitable for both beginner and advanced users looking to boost productivity through custom tooling.

Dellenny’s walkthrough on automating Windows 11 virtual desktops with PowerShell and command-line tools extends earlier workflow enhancements, giving users more control over workspace management.

- [Getting Started with PowerToys Command Palette Extension Development](/coding/videos/getting-started-with-powertoys-command-palette-extension-development)
- [Automating Virtual Desktops in Windows 11 with PowerShell and Command-Line Tools](https://dellenny.com/automating-windows-11-virtual-desktop-management-via-scripting-command-line/)

### GitHub Game Off 2025: Programming Meets Creativity

GitHub Game Off 2025 invites participants to explore the theme “WAVES” using any technology stack. The event continues last week’s onboarding and versioning discussions, encouraging collaborative workflows and creative competition—participants are welcome to use Copilot or other tools during the month-long jam.

- [Announcing the GitHub Game Off 2025: Theme—WAVES](https://github.blog/company/github-game-off-2025-theme-announcement/)',
     excerpt      = 'This week, developers benefited from future-focused events, hands-on tutorials, and ongoing ecosystem discussions. New resources help developers build their skills, contribute to open-source projects, and explore tooling in both Microsoft and open-source environments. The upcoming .NET Conf introduces new technologies, while the GitHub Game Off 2025 inspires creative game design. Tutorials expand tool customization, and funding analysis continues the dialogue around open-source sustainability.',
     content_hash = md5('This week, developers benefited from future-focused events, hands-on tutorials, and ongoing ecosystem discussions. New resources help developers build their skills, contribute to open-source projects, and explore tooling in both Microsoft and open-source environments. The upcoming .NET Conf introduces new technologies, while the GitHub Game Off 2025 inspires creative game design. Tutorials expand tool customization, and funding analysis continues the dialogue around open-source sustainability.

<!--excerpt_end-->

## Coding

### .NET and Visual Studio: Events and Ecosystem Evolution

In anticipation of .NET Conf 2025, developers look forward to previews of Visual Studio 2026, .NET 10, and broader Copilot support. These sessions highlight easier upgrades and expanded cross-platform, AI-powered coding techniques. The sustainability conversation, guided by Nick Chapsas, addresses open-source library contributions, volunteer burnout, and donor options—following the recent rollout of NuGet.org Sponsorships.

- [Join .NET Conf to Explore Visual Studio 2026 and .NET Innovation](https://devblogs.microsoft.com/visualstudio/join-us-at-net-conf-dive-into-the-future-of-development-with-visual-studio-2026/)
- [.NET Libraries: Monetization Models and Open Source Challenges](/coding/videos/net-libraries-monetization-models-and-open-source-challenges)

### PowerToys and Windows 11: Customization and Productivity Automation

Building on previous extension and toolchain updates, Kayla Cinnamon’s PowerToys Command Palette guide introduces end-to-end development, packaging, and testing in Visual Studio. These tutorials are suitable for both beginner and advanced users looking to boost productivity through custom tooling.

Dellenny’s walkthrough on automating Windows 11 virtual desktops with PowerShell and command-line tools extends earlier workflow enhancements, giving users more control over workspace management.

- [Getting Started with PowerToys Command Palette Extension Development](/coding/videos/getting-started-with-powertoys-command-palette-extension-development)
- [Automating Virtual Desktops in Windows 11 with PowerShell and Command-Line Tools](https://dellenny.com/automating-windows-11-virtual-desktop-management-via-scripting-command-line/)

### GitHub Game Off 2025: Programming Meets Creativity

GitHub Game Off 2025 invites participants to explore the theme “WAVES” using any technology stack. The event continues last week’s onboarding and versioning discussions, encouraging collaborative workflows and creative competition—participants are welcome to use Copilot or other tools during the month-long jam.

- [Announcing the GitHub Game Off 2025: Theme—WAVES](https://github.blog/company/github-game-off-2025-theme-announcement/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-11-03';

-- weekly-dotnet-roundup-2025-11-10  (2025-11-10)
UPDATE content_items
SET  content      = 'This section highlights new developments in programming languages and frameworks following the recent .NET Conf 2025 preview. Updates for .NET 10, Visual Studio 2026, and supporting tools reinforce cloud-native, AI-integrated, and modular design approaches.

<!--excerpt_end-->

## Coding

### .NET Ecosystem: From .NET 10 and Visual Studio 2026 to EF 10 and MAUI

.NET Conf 2025, running November 11–13, features tracks on security, NuGet, DevOps, and migrations to expedite adoption of .NET 10 and related AI tools. Demos for Model Context Protocol and Agent Framework support best practices for cloud-native, AI-enabled apps. Entity Framework 10’s release is spotlighted in the .NET Data Community Standup, including upgrade steps and new features. The .NET MAUI Standup marks Grial’s ten-year anniversary, tracing its development from Xamarin to .NET 10 controls.

- [.NET Conf 2025: Launching .NET 10, Visual Studio 2026, and the Future of Cloud-Native and AI Development](https://devblogs.microsoft.com/dotnet/get-ready-for-dotnet-conf-2025/)
- [.NET Data Community Standup: EF 10 Release Celebration](/coding/videos/net-data-community-standup-ef-10-release-celebration)
- [.NET MAUI Community Standup: 10 Years of Grial](/coding/videos/net-maui-community-standup-10-years-of-grial)

### Innovations in TypeScript, Language Trends, and AI Integration

TypeScript remains the leading language on GitHub, a trend confirmed by architect Anders Hejlsberg. Its presence in React, Angular, and SvelteKit continues to grow, while migration to Go improves compiler performance and reliability. AI features increasingly shape language tools and design standards, with Model Context Protocol efforts driving further development of AI-ready programming environments.

- [TypeScript’s Dominance in the AI Era: Insights from Lead Architect Anders Hejlsberg](https://github.blog/developer-skills/programming-languages-and-frameworks/typescripts-rise-in-the-ai-era-insights-from-lead-architect-anders-hejlsberg/)
- [Anders Hejlsberg on Octoverse 2025: TypeScript Evolution, Go, and AI in Development](/ai/videos/anders-hejlsberg-on-octoverse-2025-typescript-evolution-go-and-ai-in-development)

### .NET Features: Reflection Improvements and Troubleshooting Runtime Issues

.NET 10 preview includes [UnsafeAccessorType] for easier reflective programming, enabling more direct access to private and internal members in code libraries and tests. It now supports string-based signatures for streamlined cross-assembly interactions. A troubleshooting guide for .NET 8 covers Windows Service start issues caused by version mismatches, advising configuration adjustments and deployment best practices.

- [Easier Reflection with [UnsafeAccessorType] in .NET 10](https://andrewlock.net/exploring-dotnet-10-preview-features-9-easier-reflection-with-unsafeaccessortype/)
- [.NET 8 Windows Service Fails to Start Due to .NETCore.App Version Mismatch](https://techcommunity.microsoft.com/t5/net-runtime/net-runtime-issues-application-not-starting-up/m-p/4466585#M773)

### Rethinking Software Architecture: Concepts, Synchronizations, and Modular Design

MIT research proposes a software architecture built on ''concepts'' and ''synchronizations'', aiming for applications with encapsulated state and clear behaviors to enhance modularity and maintainability. Interactions are explicit with defined error flows—relevant to scalable, complex systems. Guides for web application development emphasize security, user focus, performance, scalability, and cross-platform design to facilitate well-structured, maintainable codebases.

- [MIT Researchers Propose a New Software Architecture with Concepts and Synchronizations](https://devops.com/mit-researchers-propose-a-new-way-to-build-software-that-actually-makes-sense/)
- [5 Pillars of Successful Web App Development](https://devops.com/5-pillars-of-successful-web-app-development/)',
     excerpt      = 'This section highlights new developments in programming languages and frameworks following the recent .NET Conf 2025 preview. Updates for .NET 10, Visual Studio 2026, and supporting tools reinforce cloud-native, AI-integrated, and modular design approaches.',
     content_hash = md5('This section highlights new developments in programming languages and frameworks following the recent .NET Conf 2025 preview. Updates for .NET 10, Visual Studio 2026, and supporting tools reinforce cloud-native, AI-integrated, and modular design approaches.

<!--excerpt_end-->

## Coding

### .NET Ecosystem: From .NET 10 and Visual Studio 2026 to EF 10 and MAUI

.NET Conf 2025, running November 11–13, features tracks on security, NuGet, DevOps, and migrations to expedite adoption of .NET 10 and related AI tools. Demos for Model Context Protocol and Agent Framework support best practices for cloud-native, AI-enabled apps. Entity Framework 10’s release is spotlighted in the .NET Data Community Standup, including upgrade steps and new features. The .NET MAUI Standup marks Grial’s ten-year anniversary, tracing its development from Xamarin to .NET 10 controls.

- [.NET Conf 2025: Launching .NET 10, Visual Studio 2026, and the Future of Cloud-Native and AI Development](https://devblogs.microsoft.com/dotnet/get-ready-for-dotnet-conf-2025/)
- [.NET Data Community Standup: EF 10 Release Celebration](/coding/videos/net-data-community-standup-ef-10-release-celebration)
- [.NET MAUI Community Standup: 10 Years of Grial](/coding/videos/net-maui-community-standup-10-years-of-grial)

### Innovations in TypeScript, Language Trends, and AI Integration

TypeScript remains the leading language on GitHub, a trend confirmed by architect Anders Hejlsberg. Its presence in React, Angular, and SvelteKit continues to grow, while migration to Go improves compiler performance and reliability. AI features increasingly shape language tools and design standards, with Model Context Protocol efforts driving further development of AI-ready programming environments.

- [TypeScript’s Dominance in the AI Era: Insights from Lead Architect Anders Hejlsberg](https://github.blog/developer-skills/programming-languages-and-frameworks/typescripts-rise-in-the-ai-era-insights-from-lead-architect-anders-hejlsberg/)
- [Anders Hejlsberg on Octoverse 2025: TypeScript Evolution, Go, and AI in Development](/ai/videos/anders-hejlsberg-on-octoverse-2025-typescript-evolution-go-and-ai-in-development)

### .NET Features: Reflection Improvements and Troubleshooting Runtime Issues

.NET 10 preview includes [UnsafeAccessorType] for easier reflective programming, enabling more direct access to private and internal members in code libraries and tests. It now supports string-based signatures for streamlined cross-assembly interactions. A troubleshooting guide for .NET 8 covers Windows Service start issues caused by version mismatches, advising configuration adjustments and deployment best practices.

- [Easier Reflection with [UnsafeAccessorType] in .NET 10](https://andrewlock.net/exploring-dotnet-10-preview-features-9-easier-reflection-with-unsafeaccessortype/)
- [.NET 8 Windows Service Fails to Start Due to .NETCore.App Version Mismatch](https://techcommunity.microsoft.com/t5/net-runtime/net-runtime-issues-application-not-starting-up/m-p/4466585#M773)

### Rethinking Software Architecture: Concepts, Synchronizations, and Modular Design

MIT research proposes a software architecture built on ''concepts'' and ''synchronizations'', aiming for applications with encapsulated state and clear behaviors to enhance modularity and maintainability. Interactions are explicit with defined error flows—relevant to scalable, complex systems. Guides for web application development emphasize security, user focus, performance, scalability, and cross-platform design to facilitate well-structured, maintainable codebases.

- [MIT Researchers Propose a New Software Architecture with Concepts and Synchronizations](https://devops.com/mit-researchers-propose-a-new-way-to-build-software-that-actually-makes-sense/)
- [5 Pillars of Successful Web App Development](https://devops.com/5-pillars-of-successful-web-app-development/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-11-10';

-- weekly-dotnet-roundup-2025-11-17  (2025-11-17)
UPDATE content_items
SET  content      = 'This week’s .NET announcements spotlight the general availability of .NET 10, progress in the ecosystem, improved developer tools, advanced security measures, container support, and better workflow orchestration. Community efforts continue to highlight practical migration paths, inclusive development resources, and hands-on learning for modern .NET projects.

<!--excerpt_end-->

## Coding

### .NET 10 Platform Release and Ecosystem Advances

The .NET 10 release marks the new long-term support baseline through 2028, unifying ASP.NET Core, Blazor, MAUI, EF Core 10, C# 14, and F# 10 under one production-ready umbrella. Updates like a new JIT compiler, enhanced NativeAOT, quantum-resistant crypto features, and improved NuGet package security expand on performance and reliability themes. Model Context Protocol (MCP) support and agent frameworks are now central parts of the platform.

Blazor and ASP.NET Core updates—plus OpenAPI 3.1 and improved authentication—continue the push toward modern web capabilities. .NET Aspire strengthens orchestration and Azure deployment options, while Visual Studio 2026 and the enhanced CLI increase productivity and workflow efficiency.

- [Announcing .NET 10: A Major Release for Modern, Productive, and AI-Powered Development](https://devblogs.microsoft.com/dotnet/announcing-dotnet-10/)
- [.NET 10 Migration and New Features for Enterprise Blazor Applications](/coding/videos/net-10-migration-and-new-features-for-enterprise-blazor-applications)
- [Generating Full-Stack .NET Apps with Uno Platform and AI](/ai/videos/generating-full-stack-net-apps-with-uno-platform-and-ai)
- [.NET Framework 4.8 to .NET 9 Step by Step Migration Guide](/coding/videos/net-framework-48-to-net-9-step-by-step-migration-guide)
- [What''s New in NuGet: Security, AI, and Modern Package Management for .NET](/ai/videos/whats-new-in-nuget-security-ai-and-modern-package-management-for-net)
- [Welcome to .NET 10 & Visual Studio 2026!](/coding/videos/welcome-to-net-10-and-visual-studio-2026)
- [Performance Improvements in .NET 10](/coding/videos/performance-improvements-in-net-10)
- [Build Better Web Apps with Blazor in .NET 10](/coding/videos/build-better-web-apps-with-blazor-in-net-10)
- [Modern C# Features to Enhance Your Coding Habits](/coding/videos/modern-c-features-to-enhance-your-coding-habits)
- [What''s New in C# 14](/coding/videos/whats-new-in-c-14)
- [What''s New in ASP.NET Core for .NET 10](/coding/videos/whats-new-in-aspnet-core-for-net-10)
- [What''s New in Containers for .NET 10](/azure/videos/whats-new-in-containers-for-net-10)
- [What''s New in .NET MAUI](/coding/videos/whats-new-in-net-maui)
- [Clean Architecture with ASP.NET Core 10](/coding/videos/clean-architecture-with-aspnet-core-10)
- [Ship Faster with .NET MAUI: Real-World Pitfalls and Solutions](/coding/videos/ship-faster-with-net-maui-real-world-pitfalls-and-solutions)
- [Modern Windows Development with .NET](/coding/videos/modern-windows-development-with-net)
- [New dotnet test Experience with Microsoft.Testing.Platform](/coding/videos/new-dotnet-test-experience-with-microsofttestingplatform)
- [DataMountain: .NET Data Warehousing That Beats C++ Performance](/coding/videos/datamountain-net-data-warehousing-that-beats-c-performance)
- [OpenAPI.NET v2 & v3 Major Release: The Biggest Update Ever](https://devblogs.microsoft.com/openapi/openapi-net-release-announcements/)
- [Simplifying .NET with ''dotnet run file.cs''](/coding/videos/simplifying-net-with-dotnet-run-filecs)
- [.NET and .NET Framework November 2025 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-november-2025-servicing-updates/)

### .NET Aspire: Cloud-Native Orchestration and Distributed Workflows

Aspire remains central for .NET orchestration, with enhanced documentation and cross-language compatibility in hybrid environments. Updated resources detail configuration for Python, Node.js, and non-.NET applications, keeping modular and multi-team architectures in focus. New resources, dashboards, and onboarding solutions support multi-repo and enterprise scenarios.

- [Simplifying Cloud-Native Development with .NET Aspire](/azure/videos/simplifying-cloud-native-development-with-net-aspire)
- [Taking .NET out of .NET Aspire: Working with Non-.NET Applications](/coding/videos/taking-net-out-of-net-aspire-working-with-non-net-applications)
- [Deep Dive: Extending and Customizing Aspire](/azure/videos/deep-dive-extending-and-customizing-aspire)
- [Aspire Unplugged: David Fowler Discusses .NET Aspire’s Vision and Future](/azure/videos/aspire-unplugged-david-fowler-discusses-net-aspires-vision-and-future)
- [Windows 365 Meets .NET Aspire: Boosting Multi-Repo Microservice Productivity](/coding/videos/windows-365-meets-net-aspire-boosting-multi-repo-microservice-productivity)

### Visual Studio 2026 and Developer Productivity Tools

The Visual Studio 2026 preview introduces a new user experience, accessibility improvements, and expanded customization options, reinforcing the focus on productivity. Integrated AI features and support for .NET 10 enhance debugging and profiling tools for practical application.

- [A First Look at the Refreshed UX in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/a-first-look-at-the-all%e2%80%91new-ux-in-visual-studio-2026/)
- [Visual Studio Debugger: Advanced Techniques](/ai/videos/visual-studio-debugger-advanced-techniques)
- [Real-World .NET Profiling with Visual Studio](/coding/videos/real-world-net-profiling-with-visual-studio)

### AI, Automation, and API Integration in .NET

AI workflow guidance and MCP/api integration themes continue this week, with practical tutorials for retrofitting APIs, using MCP via NuGet, and enhancing productivity through scripting. These updates expand prototyping and automation efforts highlighted previously.

- [Enhancing Existing .NET REST APIs with Model Creation Protocol (MCP) and AI](/ai/videos/enhancing-existing-net-rest-apis-with-model-creation-protocol-mcp-and-ai)
- [What''s New in NuGet: Security, AI, and Modern Package Management for .NET](/ai/videos/whats-new-in-nuget-security-ai-and-modern-package-management-for-net)
- [Simplifying .NET with ''dotnet run file.cs''](/coding/videos/simplifying-net-with-dotnet-run-filecs)

### Other Coding News

.NET Community Toolkits and the latest MAUI/Windows releases integrate the new .NET launches. Coverage on decision records, Rx.NET status, and performance frameworks (DataMountain, terrain simulation) stays relevant for modular, high-performance engineering. Sustainability and real-world learning feature with topics like MAUI troubleshooting, carbon-aware computing, F# adoption, and IoT solutions with Raspberry Pi. .NET Foundation status and open source initiatives remain visible.

- [Exploring the .NET Community Toolkits: MAUI, Windows, and More](/coding/videos/exploring-the-net-community-toolkits-maui-windows-and-more)
- [Decision Records: Understanding Why Those Decisions Were Made in .NET](/coding/videos/decision-records-understanding-why-those-decisions-were-made-in-net)
- [Rx.NET Status and Roadmap Overview](/coding/videos/rxnet-status-and-roadmap-overview)
- [DataMountain: .NET Data Warehousing That Beats C++ Performance](/coding/videos/datamountain-net-data-warehousing-that-beats-c-performance)
- [High-Performance Terrain Simulations in .NET](/coding/videos/high-performance-terrain-simulations-in-net)
- [Ship Faster with .NET MAUI: Real-World Pitfalls and Solutions](/coding/videos/ship-faster-with-net-maui-real-world-pitfalls-and-solutions)
- [Carbon Aware Computing with .NET Open Source Libraries for Sustainable Applications](/azure/videos/carbon-aware-computing-with-net-open-source-libraries-for-sustainable-applications)
- [Smatterings of F#: Integrating F# in a C#-Focused World](/coding/videos/smatterings-of-f-integrating-f-in-a-c-focused-world)
- [Understanding Nullable Reference Types in .NET](/coding/videos/understanding-nullable-reference-types-in-net)
- [Building Modern CLI Apps in .NET: Libraries, Patterns, and Packaging](/coding/videos/building-modern-cli-apps-in-net-libraries-patterns-and-packaging)
- [Real-World .NET Profiling with Visual Studio](/coding/videos/real-world-net-profiling-with-visual-studio)
- [If .NET Brewed Beer: IoT Brewing Automation with Raspberry Pi](/coding/videos/if-net-brewed-beer-iot-brewing-automation-with-raspberry-pi)
- [The Future of Python and AI with Guido van Rossum](/ai/videos/the-future-of-python-and-ai-with-guido-van-rossum)
- [State of the .NET Foundation and Advances in .NET Open Source](/coding/videos/state-of-the-net-foundation-and-advances-in-net-open-source)',
     excerpt      = 'This week’s .NET announcements spotlight the general availability of .NET 10, progress in the ecosystem, improved developer tools, advanced security measures, container support, and better workflow orchestration. Community efforts continue to highlight practical migration paths, inclusive development resources, and hands-on learning for modern .NET projects.',
     content_hash = md5('This week’s .NET announcements spotlight the general availability of .NET 10, progress in the ecosystem, improved developer tools, advanced security measures, container support, and better workflow orchestration. Community efforts continue to highlight practical migration paths, inclusive development resources, and hands-on learning for modern .NET projects.

<!--excerpt_end-->

## Coding

### .NET 10 Platform Release and Ecosystem Advances

The .NET 10 release marks the new long-term support baseline through 2028, unifying ASP.NET Core, Blazor, MAUI, EF Core 10, C# 14, and F# 10 under one production-ready umbrella. Updates like a new JIT compiler, enhanced NativeAOT, quantum-resistant crypto features, and improved NuGet package security expand on performance and reliability themes. Model Context Protocol (MCP) support and agent frameworks are now central parts of the platform.

Blazor and ASP.NET Core updates—plus OpenAPI 3.1 and improved authentication—continue the push toward modern web capabilities. .NET Aspire strengthens orchestration and Azure deployment options, while Visual Studio 2026 and the enhanced CLI increase productivity and workflow efficiency.

- [Announcing .NET 10: A Major Release for Modern, Productive, and AI-Powered Development](https://devblogs.microsoft.com/dotnet/announcing-dotnet-10/)
- [.NET 10 Migration and New Features for Enterprise Blazor Applications](/coding/videos/net-10-migration-and-new-features-for-enterprise-blazor-applications)
- [Generating Full-Stack .NET Apps with Uno Platform and AI](/ai/videos/generating-full-stack-net-apps-with-uno-platform-and-ai)
- [.NET Framework 4.8 to .NET 9 Step by Step Migration Guide](/coding/videos/net-framework-48-to-net-9-step-by-step-migration-guide)
- [What''s New in NuGet: Security, AI, and Modern Package Management for .NET](/ai/videos/whats-new-in-nuget-security-ai-and-modern-package-management-for-net)
- [Welcome to .NET 10 & Visual Studio 2026!](/coding/videos/welcome-to-net-10-and-visual-studio-2026)
- [Performance Improvements in .NET 10](/coding/videos/performance-improvements-in-net-10)
- [Build Better Web Apps with Blazor in .NET 10](/coding/videos/build-better-web-apps-with-blazor-in-net-10)
- [Modern C# Features to Enhance Your Coding Habits](/coding/videos/modern-c-features-to-enhance-your-coding-habits)
- [What''s New in C# 14](/coding/videos/whats-new-in-c-14)
- [What''s New in ASP.NET Core for .NET 10](/coding/videos/whats-new-in-aspnet-core-for-net-10)
- [What''s New in Containers for .NET 10](/azure/videos/whats-new-in-containers-for-net-10)
- [What''s New in .NET MAUI](/coding/videos/whats-new-in-net-maui)
- [Clean Architecture with ASP.NET Core 10](/coding/videos/clean-architecture-with-aspnet-core-10)
- [Ship Faster with .NET MAUI: Real-World Pitfalls and Solutions](/coding/videos/ship-faster-with-net-maui-real-world-pitfalls-and-solutions)
- [Modern Windows Development with .NET](/coding/videos/modern-windows-development-with-net)
- [New dotnet test Experience with Microsoft.Testing.Platform](/coding/videos/new-dotnet-test-experience-with-microsofttestingplatform)
- [DataMountain: .NET Data Warehousing That Beats C++ Performance](/coding/videos/datamountain-net-data-warehousing-that-beats-c-performance)
- [OpenAPI.NET v2 & v3 Major Release: The Biggest Update Ever](https://devblogs.microsoft.com/openapi/openapi-net-release-announcements/)
- [Simplifying .NET with ''dotnet run file.cs''](/coding/videos/simplifying-net-with-dotnet-run-filecs)
- [.NET and .NET Framework November 2025 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-november-2025-servicing-updates/)

### .NET Aspire: Cloud-Native Orchestration and Distributed Workflows

Aspire remains central for .NET orchestration, with enhanced documentation and cross-language compatibility in hybrid environments. Updated resources detail configuration for Python, Node.js, and non-.NET applications, keeping modular and multi-team architectures in focus. New resources, dashboards, and onboarding solutions support multi-repo and enterprise scenarios.

- [Simplifying Cloud-Native Development with .NET Aspire](/azure/videos/simplifying-cloud-native-development-with-net-aspire)
- [Taking .NET out of .NET Aspire: Working with Non-.NET Applications](/coding/videos/taking-net-out-of-net-aspire-working-with-non-net-applications)
- [Deep Dive: Extending and Customizing Aspire](/azure/videos/deep-dive-extending-and-customizing-aspire)
- [Aspire Unplugged: David Fowler Discusses .NET Aspire’s Vision and Future](/azure/videos/aspire-unplugged-david-fowler-discusses-net-aspires-vision-and-future)
- [Windows 365 Meets .NET Aspire: Boosting Multi-Repo Microservice Productivity](/coding/videos/windows-365-meets-net-aspire-boosting-multi-repo-microservice-productivity)

### Visual Studio 2026 and Developer Productivity Tools

The Visual Studio 2026 preview introduces a new user experience, accessibility improvements, and expanded customization options, reinforcing the focus on productivity. Integrated AI features and support for .NET 10 enhance debugging and profiling tools for practical application.

- [A First Look at the Refreshed UX in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/a-first-look-at-the-all%e2%80%91new-ux-in-visual-studio-2026/)
- [Visual Studio Debugger: Advanced Techniques](/ai/videos/visual-studio-debugger-advanced-techniques)
- [Real-World .NET Profiling with Visual Studio](/coding/videos/real-world-net-profiling-with-visual-studio)

### AI, Automation, and API Integration in .NET

AI workflow guidance and MCP/api integration themes continue this week, with practical tutorials for retrofitting APIs, using MCP via NuGet, and enhancing productivity through scripting. These updates expand prototyping and automation efforts highlighted previously.

- [Enhancing Existing .NET REST APIs with Model Creation Protocol (MCP) and AI](/ai/videos/enhancing-existing-net-rest-apis-with-model-creation-protocol-mcp-and-ai)
- [What''s New in NuGet: Security, AI, and Modern Package Management for .NET](/ai/videos/whats-new-in-nuget-security-ai-and-modern-package-management-for-net)
- [Simplifying .NET with ''dotnet run file.cs''](/coding/videos/simplifying-net-with-dotnet-run-filecs)

### Other Coding News

.NET Community Toolkits and the latest MAUI/Windows releases integrate the new .NET launches. Coverage on decision records, Rx.NET status, and performance frameworks (DataMountain, terrain simulation) stays relevant for modular, high-performance engineering. Sustainability and real-world learning feature with topics like MAUI troubleshooting, carbon-aware computing, F# adoption, and IoT solutions with Raspberry Pi. .NET Foundation status and open source initiatives remain visible.

- [Exploring the .NET Community Toolkits: MAUI, Windows, and More](/coding/videos/exploring-the-net-community-toolkits-maui-windows-and-more)
- [Decision Records: Understanding Why Those Decisions Were Made in .NET](/coding/videos/decision-records-understanding-why-those-decisions-were-made-in-net)
- [Rx.NET Status and Roadmap Overview](/coding/videos/rxnet-status-and-roadmap-overview)
- [DataMountain: .NET Data Warehousing That Beats C++ Performance](/coding/videos/datamountain-net-data-warehousing-that-beats-c-performance)
- [High-Performance Terrain Simulations in .NET](/coding/videos/high-performance-terrain-simulations-in-net)
- [Ship Faster with .NET MAUI: Real-World Pitfalls and Solutions](/coding/videos/ship-faster-with-net-maui-real-world-pitfalls-and-solutions)
- [Carbon Aware Computing with .NET Open Source Libraries for Sustainable Applications](/azure/videos/carbon-aware-computing-with-net-open-source-libraries-for-sustainable-applications)
- [Smatterings of F#: Integrating F# in a C#-Focused World](/coding/videos/smatterings-of-f-integrating-f-in-a-c-focused-world)
- [Understanding Nullable Reference Types in .NET](/coding/videos/understanding-nullable-reference-types-in-net)
- [Building Modern CLI Apps in .NET: Libraries, Patterns, and Packaging](/coding/videos/building-modern-cli-apps-in-net-libraries-patterns-and-packaging)
- [Real-World .NET Profiling with Visual Studio](/coding/videos/real-world-net-profiling-with-visual-studio)
- [If .NET Brewed Beer: IoT Brewing Automation with Raspberry Pi](/coding/videos/if-net-brewed-beer-iot-brewing-automation-with-raspberry-pi)
- [The Future of Python and AI with Guido van Rossum](/ai/videos/the-future-of-python-and-ai-with-guido-van-rossum)
- [State of the .NET Foundation and Advances in .NET Open Source](/coding/videos/state-of-the-net-foundation-and-advances-in-net-open-source)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-11-17';

-- weekly-dotnet-roundup-2025-11-24  (2025-11-24)
UPDATE content_items
SET  content      = 'Coding news this week includes improvements in programming languages, development tools, and platform interoperability. New releases for C#, F#, and .NET focus on modern features and improved expressiveness. Updates in Visual Studio, VS Code, Git, and Windows target code management, collaboration, and administration. Accessibility, accessible design, and educational content continue to help developers at all levels.

<!--excerpt_end-->

## Coding

### Advancements in .NET Languages: C# 14 and F# 10

After last week’s release of .NET 10, C# 14 and F# 10 introduce updated language features. C# 14 adds extension members, a `field` keyword, unbound generics in `nameof`, and more expressive lambda syntax, supporting safer and more consistent code. Migration resources offer help for adapting to these changes.

F# 10 introduces better warning suppression, enhanced property accessor features, and improved computation expressions and scripting performance. These changes support current tooling and offer hints at the direction for .NET 11’s continued compiler improvements.

- [Introducing C# 14: New Language Features and .NET 10 Integration](https://devblogs.microsoft.com/dotnet/introducing-csharp-14/)
- [What''s New in C# 14 and .NET 10](/coding/videos/whats-new-in-c-14-and-net-10)
- [Introducing F# 10: Language Features, Performance, and Tooling in .NET 10](https://devblogs.microsoft.com/dotnet/introducing-fsharp-10/)

### Visual Studio Family: Modernization, Productivity, and Secure Extension Management

Visual Studio 2026 continues its focus on smooth migration, automation, and productivity. Automated dependency checks, project retargeting, and Copilot support streamline the process of updating legacy apps. Stable update, rollback, and repair options support reliability during upgrades.

Visual Studio Code 1.106 debuts the Private Marketplace, giving organizations better control over which extensions are used while reinforcing secure extension management. Accessibility improvements continue to support every developer’s workflow.

- [Effortless Upgrades and Coding Productivity in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/spend-less-time-upgrading-more-time-coding-in-visual-studio-2026/)
- [Introducing the Visual Studio Code Private Marketplace: Secure Extension Management for Teams](https://code.visualstudio.com/blogs/2025/11/18/PrivateMarketplace)

### Windows Settings and File Explorer: Developer-Centric Enhancements

Windows updates this week address the needs of developers with tools for managing large projects. The Advanced Settings page and long path support resolve issues in handling more complex codebases. Integration with Git directly in File Explorer underlines Windows’ continuing commitment to supporting version control at the OS level.

- [What''s New in Windows Settings for Developers: Advanced Settings, Long Path Support, and Git Integration](/coding/videos/whats-new-in-windows-settings-for-developers-advanced-settings-long-path-support-and-git-integration)
- [What''s New in Windows Settings for Developers: Advanced Settings, Long File Paths, and Git Integration](/coding/videos/whats-new-in-windows-settings-for-developers-advanced-settings-long-file-paths-and-git-integration)

### Git 2.52: Version Control, Performance, and Migration

Git 2.52 brings further improvements for managing large and legacy repositories. Features like ‘git last-modified’ support better traceability, while geometric repacking and updated tools for large codebases fulfill needs highlighted in recent coverage. Plans to move to SHA-256 and more Rust components demonstrate a continued commitment to security and maintainable workflows.

- [What''s New in Git 2.52: Features and Performance Enhancements](https://github.blog/open-source/git/highlights-from-git-2-52/)

### AI-Enhanced, Cross-Platform Development with Uno Platform

Uno Platform continues the trend of AI-driven cross-platform development. Hot Design and Hot Reload for Studio, support for .NET 10, and Figma integration make it easier for designers and developers to work together and move from design to code more efficiently.

- [Building Cross-Platform .NET Apps with Uno Platform and Contextual AI](/ai/videos/building-cross-platform-net-apps-with-uno-platform-and-contextual-ai)

### Other Coding News

VS Code’s accessibility improvements build on earlier work, helping developers with different needs be more productive. GitHub’s open-source Annotation Toolkit for Figma enables better communication in design-to-code workflows, reinforcing shared standards and compliance.

The .NET Conf Student Zone 2025 showcases the ongoing commitment to practical education, supporting upskilling with hands-on content.

- [Accessibility in Visual Studio Code: Insights from Megan Rogge](/coding/videos/accessibility-in-visual-studio-code-insights-from-megan-rogge)
- [Enhance Design-to-Code Collaboration with GitHub''s Annotation Toolkit](https://github.blog/enterprise-software/collaboration/level-up-design-to-code-collaboration-with-githubs-open-source-annotation-toolkit/)
- [.NET Conf Student Zone 2025](/ai/videos/net-conf-student-zone-2025)',
     excerpt      = 'Coding news this week includes improvements in programming languages, development tools, and platform interoperability. New releases for C#, F#, and .NET focus on modern features and improved expressiveness. Updates in Visual Studio, VS Code, Git, and Windows target code management, collaboration, and administration. Accessibility, accessible design, and educational content continue to help developers at all levels.',
     content_hash = md5('Coding news this week includes improvements in programming languages, development tools, and platform interoperability. New releases for C#, F#, and .NET focus on modern features and improved expressiveness. Updates in Visual Studio, VS Code, Git, and Windows target code management, collaboration, and administration. Accessibility, accessible design, and educational content continue to help developers at all levels.

<!--excerpt_end-->

## Coding

### Advancements in .NET Languages: C# 14 and F# 10

After last week’s release of .NET 10, C# 14 and F# 10 introduce updated language features. C# 14 adds extension members, a `field` keyword, unbound generics in `nameof`, and more expressive lambda syntax, supporting safer and more consistent code. Migration resources offer help for adapting to these changes.

F# 10 introduces better warning suppression, enhanced property accessor features, and improved computation expressions and scripting performance. These changes support current tooling and offer hints at the direction for .NET 11’s continued compiler improvements.

- [Introducing C# 14: New Language Features and .NET 10 Integration](https://devblogs.microsoft.com/dotnet/introducing-csharp-14/)
- [What''s New in C# 14 and .NET 10](/coding/videos/whats-new-in-c-14-and-net-10)
- [Introducing F# 10: Language Features, Performance, and Tooling in .NET 10](https://devblogs.microsoft.com/dotnet/introducing-fsharp-10/)

### Visual Studio Family: Modernization, Productivity, and Secure Extension Management

Visual Studio 2026 continues its focus on smooth migration, automation, and productivity. Automated dependency checks, project retargeting, and Copilot support streamline the process of updating legacy apps. Stable update, rollback, and repair options support reliability during upgrades.

Visual Studio Code 1.106 debuts the Private Marketplace, giving organizations better control over which extensions are used while reinforcing secure extension management. Accessibility improvements continue to support every developer’s workflow.

- [Effortless Upgrades and Coding Productivity in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/spend-less-time-upgrading-more-time-coding-in-visual-studio-2026/)
- [Introducing the Visual Studio Code Private Marketplace: Secure Extension Management for Teams](https://code.visualstudio.com/blogs/2025/11/18/PrivateMarketplace)

### Windows Settings and File Explorer: Developer-Centric Enhancements

Windows updates this week address the needs of developers with tools for managing large projects. The Advanced Settings page and long path support resolve issues in handling more complex codebases. Integration with Git directly in File Explorer underlines Windows’ continuing commitment to supporting version control at the OS level.

- [What''s New in Windows Settings for Developers: Advanced Settings, Long Path Support, and Git Integration](/coding/videos/whats-new-in-windows-settings-for-developers-advanced-settings-long-path-support-and-git-integration)
- [What''s New in Windows Settings for Developers: Advanced Settings, Long File Paths, and Git Integration](/coding/videos/whats-new-in-windows-settings-for-developers-advanced-settings-long-file-paths-and-git-integration)

### Git 2.52: Version Control, Performance, and Migration

Git 2.52 brings further improvements for managing large and legacy repositories. Features like ‘git last-modified’ support better traceability, while geometric repacking and updated tools for large codebases fulfill needs highlighted in recent coverage. Plans to move to SHA-256 and more Rust components demonstrate a continued commitment to security and maintainable workflows.

- [What''s New in Git 2.52: Features and Performance Enhancements](https://github.blog/open-source/git/highlights-from-git-2-52/)

### AI-Enhanced, Cross-Platform Development with Uno Platform

Uno Platform continues the trend of AI-driven cross-platform development. Hot Design and Hot Reload for Studio, support for .NET 10, and Figma integration make it easier for designers and developers to work together and move from design to code more efficiently.

- [Building Cross-Platform .NET Apps with Uno Platform and Contextual AI](/ai/videos/building-cross-platform-net-apps-with-uno-platform-and-contextual-ai)

### Other Coding News

VS Code’s accessibility improvements build on earlier work, helping developers with different needs be more productive. GitHub’s open-source Annotation Toolkit for Figma enables better communication in design-to-code workflows, reinforcing shared standards and compliance.

The .NET Conf Student Zone 2025 showcases the ongoing commitment to practical education, supporting upskilling with hands-on content.

- [Accessibility in Visual Studio Code: Insights from Megan Rogge](/coding/videos/accessibility-in-visual-studio-code-insights-from-megan-rogge)
- [Enhance Design-to-Code Collaboration with GitHub''s Annotation Toolkit](https://github.blog/enterprise-software/collaboration/level-up-design-to-code-collaboration-with-githubs-open-source-annotation-toolkit/)
- [.NET Conf Student Zone 2025](/ai/videos/net-conf-student-zone-2025)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-11-24';

-- weekly-dotnet-roundup-2025-12-01  (2025-12-01)
UPDATE content_items
SET  content      = 'This week in Coding covers improved developer tools, new integrations, and best practices for creating sturdy applications. Updates range across IDE enhancements, built-in AI support, and pragmatic architecture advice. With ongoing improvements in Visual Studio 2026 and the Windows 11 developer suite, Microsoft continues to streamline iteration, boost security, and expand AI’s role in everyday coding. In-depth articles on .NET startup routines and sound C# class design reinforce the move toward maintainable, high-quality code in the Microsoft ecosystem.

<!--excerpt_end-->

## Coding

### Modern IDEs and the Windows Developer Experience

Following Visual Studio 2026’s launch last week, more detail is available on how monthly updates let developers manage innovation and stability by choosing between Insiders, Stable, and LTS channels. Major components like .NET and C++ compilers are now modular, supporting independent updates—an advance from prior .NET improvements.

Copilot is integrated further in Visual Studio 2026, including independent Copilot updates for up-to-date AI coding support. This addresses developer needs for fast adaptation to evolving features.

Windows 11’s toolkit also grows: PowerToys and Windows Terminal now add enhanced security and automation, featuring Copilot in Terminal and new command line tools, as recently previewed. The new ''Edit'' terminal utility and MCP Server deployment as AI agent underline Microsoft’s ongoing support for seamless automation and Azure integration.

- [Visual Studio 2026: Modern IDE with Monthly Updates and Flexible Build Tools](https://devblogs.microsoft.com/visualstudio/visual-studio-built-for-the-speed-of-modern-development/)
- [Windows 11 Developer Productivity Tools: WSL, Terminal, PowerToys & Enterprise Security](/coding/videos/windows-11-developer-productivity-tools-wsl-terminal-powertoys-and-enterprise-security)

### .NET Diagnostics and C# Design Discipline

.NET internals and diagnostics remain a focus, with Andrew Lock’s exploration of the .NET boot sequence complementing previous guides on startup and hosting.

Nick Chapsas’ summary on default class sealing practices supports conversations about code maintainability and extensibility, emphasizing the default use of sealed classes and clear extension points—topical as teams seek more robust design approaches.

- [Exploring the .NET Boot Process via Host Tracing](https://andrewlock.net/exploring-the-dotnet-boot-process-via-host-tracing/)
- [Every Class Should Be Sealed in C#](/coding/videos/every-class-should-be-sealed-in-c)',
     excerpt      = 'This week in Coding covers improved developer tools, new integrations, and best practices for creating sturdy applications. Updates range across IDE enhancements, built-in AI support, and pragmatic architecture advice. With ongoing improvements in Visual Studio 2026 and the Windows 11 developer suite, Microsoft continues to streamline iteration, boost security, and expand AI’s role in everyday coding. In-depth articles on .NET startup routines and sound C# class design reinforce the move toward maintainable, high-quality code in the Microsoft ecosystem.',
     content_hash = md5('This week in Coding covers improved developer tools, new integrations, and best practices for creating sturdy applications. Updates range across IDE enhancements, built-in AI support, and pragmatic architecture advice. With ongoing improvements in Visual Studio 2026 and the Windows 11 developer suite, Microsoft continues to streamline iteration, boost security, and expand AI’s role in everyday coding. In-depth articles on .NET startup routines and sound C# class design reinforce the move toward maintainable, high-quality code in the Microsoft ecosystem.

<!--excerpt_end-->

## Coding

### Modern IDEs and the Windows Developer Experience

Following Visual Studio 2026’s launch last week, more detail is available on how monthly updates let developers manage innovation and stability by choosing between Insiders, Stable, and LTS channels. Major components like .NET and C++ compilers are now modular, supporting independent updates—an advance from prior .NET improvements.

Copilot is integrated further in Visual Studio 2026, including independent Copilot updates for up-to-date AI coding support. This addresses developer needs for fast adaptation to evolving features.

Windows 11’s toolkit also grows: PowerToys and Windows Terminal now add enhanced security and automation, featuring Copilot in Terminal and new command line tools, as recently previewed. The new ''Edit'' terminal utility and MCP Server deployment as AI agent underline Microsoft’s ongoing support for seamless automation and Azure integration.

- [Visual Studio 2026: Modern IDE with Monthly Updates and Flexible Build Tools](https://devblogs.microsoft.com/visualstudio/visual-studio-built-for-the-speed-of-modern-development/)
- [Windows 11 Developer Productivity Tools: WSL, Terminal, PowerToys & Enterprise Security](/coding/videos/windows-11-developer-productivity-tools-wsl-terminal-powertoys-and-enterprise-security)

### .NET Diagnostics and C# Design Discipline

.NET internals and diagnostics remain a focus, with Andrew Lock’s exploration of the .NET boot sequence complementing previous guides on startup and hosting.

Nick Chapsas’ summary on default class sealing practices supports conversations about code maintainability and extensibility, emphasizing the default use of sealed classes and clear extension points—topical as teams seek more robust design approaches.

- [Exploring the .NET Boot Process via Host Tracing](https://andrewlock.net/exploring-the-dotnet-boot-process-via-host-tracing/)
- [Every Class Should Be Sealed in C#](/coding/videos/every-class-should-be-sealed-in-c)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-12-01';

-- weekly-dotnet-roundup-2025-12-08  (2025-12-08)
UPDATE content_items
SET  content      = 'Updates include .NET 10, Visual Studio 2026, cross-platform frameworks, TypeScript compiler improvements, and AI-powered development tools.

<!--excerpt_end-->

## Coding

### .NET 10, Visual Studio 2026, and MAUI Ecosystem Developments

.NET Conf 2025 introduced .NET 10 (with support until 2028), following last week’s modular IDE announcements and tighter Copilot/VS integration. Updates focus on performance, post-quantum cryptography, ASP.NET Core, Blazor, .NET MAUI, and C# 14. Aspire 13 CLI adds support for Python and JavaScript. Copilot and Agent Framework continue to grow alongside agent-based development. Progress in code quality and security guidance supports inclusive and streamlined development, and the MAUI Community Standup explored .NET 10’s impact and upgrade strategy.

- [.NET Conf 2025 Recap: What''s New in .NET 10, Visual Studio 2026, AI, and App Modernization](https://devblogs.microsoft.com/dotnet/dotnet-conf-2025-recap/)
- [.NET MAUI Community Standup - .NET 10 Announcements Roundup](/coding/videos/net-maui-community-standup-net-10-announcements-roundup)

### Practical Guidance and Tooling for .NET Developers

Stephen Toub and Scott Hanselman share recommendations for CancellationToken use in asynchronous .NET code, supporting robust and testable patterns. Age verification for .NET MAUI now aligns with current global regulations, providing platform-specific tips for Android, iOS, and Windows. The latest NetEscapades.EnumGenerators release adds support for [EnumMember] and improved analyzers.

- [Deep Dive into Cancellation Tokens in .NET with Stephen Toub](/coding/videos/deep-dive-into-cancellation-tokens-in-net-with-stephen-toub)
- [Cross-Platform Age Verification in .NET MAUI Applications](https://devblogs.microsoft.com/dotnet/cross-platform-age-verification-dotnet-maui/)
- [Recent updates to NetEscapades.EnumGenerators: [EnumMember] support, analyzers, and bug fixes](https://andrewlock.net/recent-updates-to-netescapaades-enumgenerators/)

### Agentic UI, MCP, and Coding Experience Updates in Microsoft’s Stack

Demonstrations show AG-UI and Blazor enabling interactive web components for agent-based apps. Updated tooling for MCP in VS Code improves developer onboarding, and features like in-depth keyboard shortcut analysis support improved workflow familiarity in Visual Studio 2026.

- [Building Agentic UI with AG-UI and Blazor: ASP.NET Community Standup](/ai/videos/building-agentic-ui-with-ag-ui-and-blazor-aspnet-community-standup)
- [Tooling Support for MCP in Visual Studio Code](/coding/videos/tooling-support-for-mcp-in-visual-studio-code)
- [Making Sense of Keyboard Shortcuts in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/why-changing-keyboard-shortcuts-in-visual-studio-isnt-as-simple-as-it-seems/)

### TypeScript Compiler and Language Service Modernization

Project Corsa is taking TypeScript''s compiler and language service to native code in version 7, adding multi-threading and more type safety. Preview features are available in VS Code, and migration guidance is provided.

- [Progress on TypeScript 7: Native Compiler and Language Service Updates](https://devblogs.microsoft.com/typescript/progress-on-typescript-7-december-2025/)

### Other Coding News

The VS Code Insiders Podcast is now available, featuring interviews and technical segments about AI, new VS Code features, accessibility, and open-source engagement.

- [Introducing the VS Code Insiders Podcast](https://code.visualstudio.com/blogs/2025/12/03/introducing-vs-code-insiders-podcast)',
     excerpt      = 'Updates include .NET 10, Visual Studio 2026, cross-platform frameworks, TypeScript compiler improvements, and AI-powered development tools.',
     content_hash = md5('Updates include .NET 10, Visual Studio 2026, cross-platform frameworks, TypeScript compiler improvements, and AI-powered development tools.

<!--excerpt_end-->

## Coding

### .NET 10, Visual Studio 2026, and MAUI Ecosystem Developments

.NET Conf 2025 introduced .NET 10 (with support until 2028), following last week’s modular IDE announcements and tighter Copilot/VS integration. Updates focus on performance, post-quantum cryptography, ASP.NET Core, Blazor, .NET MAUI, and C# 14. Aspire 13 CLI adds support for Python and JavaScript. Copilot and Agent Framework continue to grow alongside agent-based development. Progress in code quality and security guidance supports inclusive and streamlined development, and the MAUI Community Standup explored .NET 10’s impact and upgrade strategy.

- [.NET Conf 2025 Recap: What''s New in .NET 10, Visual Studio 2026, AI, and App Modernization](https://devblogs.microsoft.com/dotnet/dotnet-conf-2025-recap/)
- [.NET MAUI Community Standup - .NET 10 Announcements Roundup](/coding/videos/net-maui-community-standup-net-10-announcements-roundup)

### Practical Guidance and Tooling for .NET Developers

Stephen Toub and Scott Hanselman share recommendations for CancellationToken use in asynchronous .NET code, supporting robust and testable patterns. Age verification for .NET MAUI now aligns with current global regulations, providing platform-specific tips for Android, iOS, and Windows. The latest NetEscapades.EnumGenerators release adds support for [EnumMember] and improved analyzers.

- [Deep Dive into Cancellation Tokens in .NET with Stephen Toub](/coding/videos/deep-dive-into-cancellation-tokens-in-net-with-stephen-toub)
- [Cross-Platform Age Verification in .NET MAUI Applications](https://devblogs.microsoft.com/dotnet/cross-platform-age-verification-dotnet-maui/)
- [Recent updates to NetEscapades.EnumGenerators: [EnumMember] support, analyzers, and bug fixes](https://andrewlock.net/recent-updates-to-netescapaades-enumgenerators/)

### Agentic UI, MCP, and Coding Experience Updates in Microsoft’s Stack

Demonstrations show AG-UI and Blazor enabling interactive web components for agent-based apps. Updated tooling for MCP in VS Code improves developer onboarding, and features like in-depth keyboard shortcut analysis support improved workflow familiarity in Visual Studio 2026.

- [Building Agentic UI with AG-UI and Blazor: ASP.NET Community Standup](/ai/videos/building-agentic-ui-with-ag-ui-and-blazor-aspnet-community-standup)
- [Tooling Support for MCP in Visual Studio Code](/coding/videos/tooling-support-for-mcp-in-visual-studio-code)
- [Making Sense of Keyboard Shortcuts in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/why-changing-keyboard-shortcuts-in-visual-studio-isnt-as-simple-as-it-seems/)

### TypeScript Compiler and Language Service Modernization

Project Corsa is taking TypeScript''s compiler and language service to native code in version 7, adding multi-threading and more type safety. Preview features are available in VS Code, and migration guidance is provided.

- [Progress on TypeScript 7: Native Compiler and Language Service Updates](https://devblogs.microsoft.com/typescript/progress-on-typescript-7-december-2025/)

### Other Coding News

The VS Code Insiders Podcast is now available, featuring interviews and technical segments about AI, new VS Code features, accessibility, and open-source engagement.

- [Introducing the VS Code Insiders Podcast](https://code.visualstudio.com/blogs/2025/12/03/introducing-vs-code-insiders-podcast)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-12-08';

-- weekly-dotnet-roundup-2025-12-15  (2025-12-15)
UPDATE content_items
SET  content      = 'This week''s updates cover .NET networking changes, cross-platform in-app billing, innovations in PC gaming, and new tools in Visual Studio Code for developer error handling and maintainability.

<!--excerpt_end-->

## Coding

### .NET 10 Platform Developments and Servicing Updates

.NET 10 introduced networking features like optional certificate caching and support for more HTTP verbs, continuing last week’s progress in performance and security. TLS 1.3 adoption improves, and .NET 10.0.1 servicing release upholds the recommendation to remain current, with an emphasis on upgrading over older .NET Framework versions.

- [.NET 10 Networking Improvements](https://devblogs.microsoft.com/dotnet/dotnet-10-networking-improvements/)
- [.NET and .NET Framework December 2025 Servicing Updates Recap](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-december-2025-servicing-updates/)

### Cross-Platform Client Development with .NET MAUI

A new sample for .NET MAUI demonstrates unified in-app billing across Android, iOS, Mac, and Windows. It provides abstraction layers for different platforms, migration from third-party plugins, and compliance considerations. Code architecture templates and server validation help lay the groundwork for long-term code maintenance.

- [Cross-Platform In-App Billing in .NET MAUI: New Sample Implementation](https://devblogs.microsoft.com/dotnet/cross-platform-billing-dotnet-maui/)

### Windows PC Gaming Development: Tools and Platform Advancements

PC gaming on Windows receives updates that extend support for handheld devices, ARM-based systems, and new rendering technology. Advanced Shader Delivery and Auto Super Resolution bring both improved player experience and performance, in line with wider cross-device support goals.

- [Windows PC Gaming Innovations in 2025: Handheld Devices, Arm Expansion, and DirectX Upgrades](https://blogs.windows.com/windowsexperience/2025/12/09/windows-pc-gaming-in-2025-handheld-innovation-arm-progress-and-directx-advances/)

### Visual Studio Code: Agent HQ, TypeScript 7, and Copilot Changes

Visual Studio Code introduces the Agent HQ feature for managing multiple development agents, answering increased demand for agentic workflows. TypeScript 7 Preview introduces improvements for modern APIs and parallel services. The deprecation of IntelliCode and Copilot changes for free-tier users adjust the available coding assistance tools.

- [VS Code Update Introduces Agent HQ, TypeScript 7 Preview, and Deprecates IntelliCode](https://devclass.com/2025/12/11/vs-code-update-brings-agent-overload-typescript-7-preview-and-the-end-of-intellicode/)

### Other Coding News

Andrew Lock reviews the new Zed editor, offering .NET and Markdown developers alternatives beyond Visual Studio Code. A troubleshooting guide covers proper exception handling strategies for filesystem issues such as the `\\.\\nul` path, helping developers write more reliable applications.

- [Trying out the Zed editor on Windows for .NET and Markdown](https://andrewlock.net/trying-out-the-zed-editor-on-windows-for-dotnet-and-markdown/)
- [Troubleshooting the `\\.\\nul` Path Error in Directory Files Lookup](https://weblog.west-wind.com/posts/2025/Dec/08/What-the-heck-is-a-nul-path-and-why-is-it-breaking-my-Directory-Files-Lookup)',
     excerpt      = 'This week''s updates cover .NET networking changes, cross-platform in-app billing, innovations in PC gaming, and new tools in Visual Studio Code for developer error handling and maintainability.',
     content_hash = md5('This week''s updates cover .NET networking changes, cross-platform in-app billing, innovations in PC gaming, and new tools in Visual Studio Code for developer error handling and maintainability.

<!--excerpt_end-->

## Coding

### .NET 10 Platform Developments and Servicing Updates

.NET 10 introduced networking features like optional certificate caching and support for more HTTP verbs, continuing last week’s progress in performance and security. TLS 1.3 adoption improves, and .NET 10.0.1 servicing release upholds the recommendation to remain current, with an emphasis on upgrading over older .NET Framework versions.

- [.NET 10 Networking Improvements](https://devblogs.microsoft.com/dotnet/dotnet-10-networking-improvements/)
- [.NET and .NET Framework December 2025 Servicing Updates Recap](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-december-2025-servicing-updates/)

### Cross-Platform Client Development with .NET MAUI

A new sample for .NET MAUI demonstrates unified in-app billing across Android, iOS, Mac, and Windows. It provides abstraction layers for different platforms, migration from third-party plugins, and compliance considerations. Code architecture templates and server validation help lay the groundwork for long-term code maintenance.

- [Cross-Platform In-App Billing in .NET MAUI: New Sample Implementation](https://devblogs.microsoft.com/dotnet/cross-platform-billing-dotnet-maui/)

### Windows PC Gaming Development: Tools and Platform Advancements

PC gaming on Windows receives updates that extend support for handheld devices, ARM-based systems, and new rendering technology. Advanced Shader Delivery and Auto Super Resolution bring both improved player experience and performance, in line with wider cross-device support goals.

- [Windows PC Gaming Innovations in 2025: Handheld Devices, Arm Expansion, and DirectX Upgrades](https://blogs.windows.com/windowsexperience/2025/12/09/windows-pc-gaming-in-2025-handheld-innovation-arm-progress-and-directx-advances/)

### Visual Studio Code: Agent HQ, TypeScript 7, and Copilot Changes

Visual Studio Code introduces the Agent HQ feature for managing multiple development agents, answering increased demand for agentic workflows. TypeScript 7 Preview introduces improvements for modern APIs and parallel services. The deprecation of IntelliCode and Copilot changes for free-tier users adjust the available coding assistance tools.

- [VS Code Update Introduces Agent HQ, TypeScript 7 Preview, and Deprecates IntelliCode](https://devclass.com/2025/12/11/vs-code-update-brings-agent-overload-typescript-7-preview-and-the-end-of-intellicode/)

### Other Coding News

Andrew Lock reviews the new Zed editor, offering .NET and Markdown developers alternatives beyond Visual Studio Code. A troubleshooting guide covers proper exception handling strategies for filesystem issues such as the `\\.\\nul` path, helping developers write more reliable applications.

- [Trying out the Zed editor on Windows for .NET and Markdown](https://andrewlock.net/trying-out-the-zed-editor-on-windows-for-dotnet-and-markdown/)
- [Troubleshooting the `\\.\\nul` Path Error in Directory Files Lookup](https://weblog.west-wind.com/posts/2025/Dec/08/What-the-heck-is-a-nul-path-and-why-is-it-breaking-my-Directory-Files-Lookup)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-12-15';

-- weekly-dotnet-roundup-2025-12-22  (2025-12-22)
UPDATE content_items
SET  content      = 'VS Code 1.107 launches inline chat editing and persistent local agents. ASP.NET Core kicks off .NET 11 planning with community input opportunities.

<!--excerpt_end-->

## Coding

### .NET Development: Cross-Platform Widgets and ASP.NET Core Roadmaps

A .NET MAUI walkthrough demonstrates how to build interactive iOS widgets leveraging shared .NET logic and integrating Swift for platform-specific needs. ASP.NET Core’s .NET 11 planning is underway, with opportunities for community input and transparent discussions—helping teams prepare for migration and long-term design decisions.

- [Building iOS Widgets with .NET MAUI: From Setup to Interactive Features](https://devblogs.microsoft.com/dotnet/how-to-build-ios-widgets-with-dotnet-maui/)
- [ASP.NET Core Planning Kickoff for .NET 11](/coding/videos/aspnet-core-planning-kickoff-for-net-11)
- [ASP.NET Core Server & APIs Roadmap Discussion for .NET 11](/coding/videos/aspnet-core-server-and-apis-roadmap-discussion-for-net-11)

### Editor Experiences: VS Code and Cursor AI Updates

VS Code 1.107 launches inline chat editing, advanced renaming, and persistent local agents for background tasks. Cursor AI Editor 2.2 introduces a visual workflow designer and quick access to LLM options but continues to draw developer concerns over frequent interface changes and complex pricing. These updates feed into broader conversations about balancing speed, usability, and control.

- [VS Code 1.107 Release Highlights](/coding/videos/vs-code-1107-release-highlights)
- [AI-Driven Cursor Editor Adds Visual Designer Amid Developer Frustrations](https://devclass.com/2025/12/16/cursor-ai-editor-gets-visual-designer-but-bugs-and-ever-changing-ui-irk-developers/)

### Practical Profiling and Feedback Workflows

A new guide details profiling the .NET CLR using C# and Silhouette, removing the dependency on C++ for diagnostics and performance monitoring. Another post explains how Visual Studio’s feedback process uses transparent triage and prioritization to connect developer suggestions directly to product improvements.

- [Creating a .NET CLR profiler with C# NativeAOT and Silhouette](https://andrewlock.net/creating-a-dotnet-profiler-using-csharp-with-silhouette/)
- [How Visual Studio Feedback Improves the Developer Experience](https://devblogs.microsoft.com/visualstudio/behind-the-scenes-of-the-visual-studio-feedback-system/)',
     excerpt      = 'VS Code 1.107 launches inline chat editing and persistent local agents. ASP.NET Core kicks off .NET 11 planning with community input opportunities.',
     content_hash = md5('VS Code 1.107 launches inline chat editing and persistent local agents. ASP.NET Core kicks off .NET 11 planning with community input opportunities.

<!--excerpt_end-->

## Coding

### .NET Development: Cross-Platform Widgets and ASP.NET Core Roadmaps

A .NET MAUI walkthrough demonstrates how to build interactive iOS widgets leveraging shared .NET logic and integrating Swift for platform-specific needs. ASP.NET Core’s .NET 11 planning is underway, with opportunities for community input and transparent discussions—helping teams prepare for migration and long-term design decisions.

- [Building iOS Widgets with .NET MAUI: From Setup to Interactive Features](https://devblogs.microsoft.com/dotnet/how-to-build-ios-widgets-with-dotnet-maui/)
- [ASP.NET Core Planning Kickoff for .NET 11](/coding/videos/aspnet-core-planning-kickoff-for-net-11)
- [ASP.NET Core Server & APIs Roadmap Discussion for .NET 11](/coding/videos/aspnet-core-server-and-apis-roadmap-discussion-for-net-11)

### Editor Experiences: VS Code and Cursor AI Updates

VS Code 1.107 launches inline chat editing, advanced renaming, and persistent local agents for background tasks. Cursor AI Editor 2.2 introduces a visual workflow designer and quick access to LLM options but continues to draw developer concerns over frequent interface changes and complex pricing. These updates feed into broader conversations about balancing speed, usability, and control.

- [VS Code 1.107 Release Highlights](/coding/videos/vs-code-1107-release-highlights)
- [AI-Driven Cursor Editor Adds Visual Designer Amid Developer Frustrations](https://devclass.com/2025/12/16/cursor-ai-editor-gets-visual-designer-but-bugs-and-ever-changing-ui-irk-developers/)

### Practical Profiling and Feedback Workflows

A new guide details profiling the .NET CLR using C# and Silhouette, removing the dependency on C++ for diagnostics and performance monitoring. Another post explains how Visual Studio’s feedback process uses transparent triage and prioritization to connect developer suggestions directly to product improvements.

- [Creating a .NET CLR profiler with C# NativeAOT and Silhouette](https://andrewlock.net/creating-a-dotnet-profiler-using-csharp-with-silhouette/)
- [How Visual Studio Feedback Improves the Developer Experience](https://devblogs.microsoft.com/visualstudio/behind-the-scenes-of-the-visual-studio-feedback-system/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-12-22';

-- weekly-dotnet-roundup-2025-12-29  (2025-12-29)
UPDATE content_items
SET  content      = 'This week’s coding news discusses how to prepare for future trends while streamlining everyday workflows. Developers are encouraged to consider strategies and resources for building sustainable, effective skills and projects leading into 2026.

<!--excerpt_end-->

## Coding

### Building Confidence for 2026: Trends, Skills, and Open Source Practices

Cassidy Williams’ curation of GitHub Podcast episodes covers approaches to stay up-to-date with future-oriented coding techniques. Topics include technical advancements like Model Context Protocol (MCP)—which supports improved AI tool interoperability and more resilient software models—with practical references like the GitHub MCP server for hands-on experimentation.

These episodes align with ongoing developer roadmap discussions around technologies like .NET 11, emphasizing the benefits of planning and tool selection for future-ready skills. The guidance is practical, connected to recent posts about career development and open source community health.

DIY and open source contributions remain central. The podcast shares examples of how accessible ecosystems support rapid project building. Episodes also highlight best practices for fostering sustainability in open source, referencing lessons from cases like Log4Shell, and encourage contributors to engage through multiple roles, not just code.

Other segments address trends from Octoverse 2025—such as TypeScript’s growth, the adoption of AI-assisted development, and the continued relevance of existing languages. The Home Assistant community project is given as an example of privacy-focused, community-driven software.

Each episode provides actionable advice, from exploring new tech to contributing for long-term community stability—helping developers align skills and projects with emerging trends.

- [5 Podcast Episodes to Help You Build with Confidence in 2026](https://github.blog/open-source/maintainers/5-podcast-episodes-to-help-you-build-with-confidence-in-2026/)

### Developer Workflow Enhancements in Visual Studio Code

Web developers now have access to the Simple Browser in Visual Studio Code, as explained in Justin Chen’s guide. This feature enables the use of browser tabs for local development servers with integrated inspection tools so developers can debug and resolve HTML issues within the editor.

This follows the recent release of VS Code 1.107, which introduced updated features like inline chat and persistent agents for improved task handling. The current focus on web development features means less switching between tools, streamlining work and saving time.

VS Code’s browser supports integration with AI-powered features and code automation to provide responsive suggestions and speed up front-end workflows. Such tools improve productivity, especially during multi-step debugging and streamlined workspace management.

Planned updates will further integrate development and browser features, ensuring consistent improvements for web developers using VS Code.

- [Unlocking the Power of VS Code''s Simple Browser Feature](/ai/videos/unlocking-the-power-of-vs-codes-simple-browser-feature)',
     excerpt      = 'This week’s coding news discusses how to prepare for future trends while streamlining everyday workflows. Developers are encouraged to consider strategies and resources for building sustainable, effective skills and projects leading into 2026.',
     content_hash = md5('This week’s coding news discusses how to prepare for future trends while streamlining everyday workflows. Developers are encouraged to consider strategies and resources for building sustainable, effective skills and projects leading into 2026.

<!--excerpt_end-->

## Coding

### Building Confidence for 2026: Trends, Skills, and Open Source Practices

Cassidy Williams’ curation of GitHub Podcast episodes covers approaches to stay up-to-date with future-oriented coding techniques. Topics include technical advancements like Model Context Protocol (MCP)—which supports improved AI tool interoperability and more resilient software models—with practical references like the GitHub MCP server for hands-on experimentation.

These episodes align with ongoing developer roadmap discussions around technologies like .NET 11, emphasizing the benefits of planning and tool selection for future-ready skills. The guidance is practical, connected to recent posts about career development and open source community health.

DIY and open source contributions remain central. The podcast shares examples of how accessible ecosystems support rapid project building. Episodes also highlight best practices for fostering sustainability in open source, referencing lessons from cases like Log4Shell, and encourage contributors to engage through multiple roles, not just code.

Other segments address trends from Octoverse 2025—such as TypeScript’s growth, the adoption of AI-assisted development, and the continued relevance of existing languages. The Home Assistant community project is given as an example of privacy-focused, community-driven software.

Each episode provides actionable advice, from exploring new tech to contributing for long-term community stability—helping developers align skills and projects with emerging trends.

- [5 Podcast Episodes to Help You Build with Confidence in 2026](https://github.blog/open-source/maintainers/5-podcast-episodes-to-help-you-build-with-confidence-in-2026/)

### Developer Workflow Enhancements in Visual Studio Code

Web developers now have access to the Simple Browser in Visual Studio Code, as explained in Justin Chen’s guide. This feature enables the use of browser tabs for local development servers with integrated inspection tools so developers can debug and resolve HTML issues within the editor.

This follows the recent release of VS Code 1.107, which introduced updated features like inline chat and persistent agents for improved task handling. The current focus on web development features means less switching between tools, streamlining work and saving time.

VS Code’s browser supports integration with AI-powered features and code automation to provide responsive suggestions and speed up front-end workflows. Such tools improve productivity, especially during multi-step debugging and streamlined workspace management.

Planned updates will further integrate development and browser features, ensuring consistent improvements for web developers using VS Code.

- [Unlocking the Power of VS Code''s Simple Browser Feature](/ai/videos/unlocking-the-power-of-vs-codes-simple-browser-feature)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2025-12-29';

-- weekly-dotnet-roundup-2026-01-05  (2026-01-05)
UPDATE content_items
SET  content      = 'This Coding section features updates for .NET, highlighting advances in .NET 10, opportunities for skill-building, and new resources in AI integration and developer education.

<!--excerpt_end-->

## Coding

### .NET 10 Technical Developments and AI Integration

The latest roundup on the official .NET blog recaps the .NET 10 LTS release and key additions to the runtime, SDK, and C#. Recent features include faster garbage collection, new benchmarking tools, and improved solution management with updated SLNX and CLI capabilities. .NET 10 also supports AI agent development with the Microsoft Agent Framework, upgrades to the MCP server, and instant access to AI via NuGet packages, helping teams use generative AI across both existing and new solutions.

This post continues threads from the past week’s MCP coverage and agent automation, now available through official templates and NuGet for easier use. The Agent Framework and server reduce barriers for customizing enterprise AI agents. Copilot integration, now through enhanced LTS toolchains, connects .NET’s automation approach with that of Visual Studio Code. Articles compare Copilot’s new Ask and Agent Modes, outlining how they fit into .NET development.

Additionally, .NET Aspire 9.3 brings more support for cloud architectures and improved onboarding. The post links to deep dives, community highlights from .NET Conf 2025, security news, and official lifecycle policies, helping developers stay current with migrations and updates.

- [Top .NET Blog Posts of 2025: .NET 10, AI Integrations, Performance, and Tooling](https://devblogs.microsoft.com/dotnet/top-dotnet-blogs-posts-of-2025/)

### .NET Developer Education and Video Resources

Jon Galloway’s annual roundup curates the year’s top videos and live streams for .NET developers. The featured content spans demos of .NET 10 and Visual Studio 2026, Clean Architecture, performance tuning, modular monolith patterns, advanced C# 14 topics, and Blazor with AI integration (including Python and MCP demos). Tutorials cover a range of workflows, including Hands-on with Blazor AI templates, and track .NET’s continued intersections with modern AI.

This complements ongoing themes of open source development, skill advancement, and innovation through community events. Demo sessions, podcasts, and experimental MCP showcases follow up on discussions from the latest podcasts and technical experimentation.

Community Standup events and “Deep .NET” livestreams highlight interactive and transparent development processes. Additional topics include deploying AI locally, using Akka.NET for high-throughput data, and optimizing SQL. All content is freely available on YouTube and Microsoft Learn to help developers build current skills and adopt new .NET concepts.

- [Top .NET Videos and Live Streams of 2025: A Year in Review](https://devblogs.microsoft.com/dotnet/top-dotnet-videos-live-streams-of-2025/)',
     excerpt      = 'This Coding section features updates for .NET, highlighting advances in .NET 10, opportunities for skill-building, and new resources in AI integration and developer education.',
     content_hash = md5('This Coding section features updates for .NET, highlighting advances in .NET 10, opportunities for skill-building, and new resources in AI integration and developer education.

<!--excerpt_end-->

## Coding

### .NET 10 Technical Developments and AI Integration

The latest roundup on the official .NET blog recaps the .NET 10 LTS release and key additions to the runtime, SDK, and C#. Recent features include faster garbage collection, new benchmarking tools, and improved solution management with updated SLNX and CLI capabilities. .NET 10 also supports AI agent development with the Microsoft Agent Framework, upgrades to the MCP server, and instant access to AI via NuGet packages, helping teams use generative AI across both existing and new solutions.

This post continues threads from the past week’s MCP coverage and agent automation, now available through official templates and NuGet for easier use. The Agent Framework and server reduce barriers for customizing enterprise AI agents. Copilot integration, now through enhanced LTS toolchains, connects .NET’s automation approach with that of Visual Studio Code. Articles compare Copilot’s new Ask and Agent Modes, outlining how they fit into .NET development.

Additionally, .NET Aspire 9.3 brings more support for cloud architectures and improved onboarding. The post links to deep dives, community highlights from .NET Conf 2025, security news, and official lifecycle policies, helping developers stay current with migrations and updates.

- [Top .NET Blog Posts of 2025: .NET 10, AI Integrations, Performance, and Tooling](https://devblogs.microsoft.com/dotnet/top-dotnet-blogs-posts-of-2025/)

### .NET Developer Education and Video Resources

Jon Galloway’s annual roundup curates the year’s top videos and live streams for .NET developers. The featured content spans demos of .NET 10 and Visual Studio 2026, Clean Architecture, performance tuning, modular monolith patterns, advanced C# 14 topics, and Blazor with AI integration (including Python and MCP demos). Tutorials cover a range of workflows, including Hands-on with Blazor AI templates, and track .NET’s continued intersections with modern AI.

This complements ongoing themes of open source development, skill advancement, and innovation through community events. Demo sessions, podcasts, and experimental MCP showcases follow up on discussions from the latest podcasts and technical experimentation.

Community Standup events and “Deep .NET” livestreams highlight interactive and transparent development processes. Additional topics include deploying AI locally, using Akka.NET for high-throughput data, and optimizing SQL. All content is freely available on YouTube and Microsoft Learn to help developers build current skills and adopt new .NET concepts.

- [Top .NET Videos and Live Streams of 2025: A Year in Review](https://devblogs.microsoft.com/dotnet/top-dotnet-videos-live-streams-of-2025/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-01-05';

-- weekly-dotnet-roundup-2026-01-12  (2026-01-12)
UPDATE content_items
SET  content      = 'This week’s coding news highlights .NET and C# feature releases, advances in modern UI development, cross-platform tooling, and the evolving use of AI-assisted programming languages.

<!--excerpt_end-->

## Coding

### .NET Ecosystem: New Features, Platforms, and Roadmaps

The .NET ecosystem receives new updates on platform support and development features. MongoDB EF Core adds Queryable Encryption and Vector Search to create secure, privacy-friendly apps with advanced semantic search options.

.NET Community Standups discuss roadmap progress. Blazor Standup previews features coming in .NET 11, with an open invitation to shape web UI planning and migration.

Cross-platform development gains more focus. Avalonia Standup demonstrates Avalonia UI as a Linux backend for .NET MAUI, answering requests for broader deployment options.

Distributed app development is featured in Orleans sessions, diving into scalable architecture patterns for maintainable cloud-native applications.

- [Secure and Intelligent: Queryable Encryption and Vector Search in MongoDB EF Core Provider](https://devblogs.microsoft.com/dotnet/mongodb-efcore-provider-queryable-encryption-vector-search/)
- [Blazor Community Standup: Planning the Future of Blazor in .NET 11](/coding/videos/blazor-community-standup-planning-the-future-of-blazor-in-net-11)
- [.NET MAUI on Linux with Avalonia: Community Standup Recap](/coding/videos/net-maui-on-linux-with-avalonia-community-standup-recap)
- [Orleans Deep Dive: Routing, Placement & Balancing](/coding/videos/orleans-deep-dive-routing-placement-and-balancing)
- [ASP.NET Community Standup: What''s Next for Orleans?](/coding/videos/aspnet-community-standup-whats-next-for-orleans)

### UI Development Tools and Open Source Progress

Microsoft open-sources XAML Studio, inviting contributions to fill gaps in Visual Studio designer functionality highlighted during recent developer discussions. The .NET Foundation release focuses on live, visual design for WinUI and MAUI.

Community conversations continue on sustaining frameworks like WinForms and WPF and the need for unified designer experiences. The project activates open innovation within the .NET UI ecosystem.

- [Microsoft Open Sources XAML Studio and Highlights Visual Studio Designer Challenges](https://devclass.com/2026/01/07/microsoft-open-sources-xaml-studio-amid-developer-discontent-with-visual-studio-designers/)

### Language Trends and AI-Assisted Coding

Analysis of programming language usage reveals Copilot’s effect on adoption of typed languages to reduce AI code issues. This backs up previous trends showing TypeScript’s rise and a movement toward stronger type systems for reliable developer contracts.

Tips on exploring gradually typed languages and testing Copilot CLI match ongoing efforts to improve maintainability as AI tools become integral to coding practices.

- [Why AI Is Pushing Developers Toward Typed Languages](https://github.blog/ai-and-ml/llms/why-ai-is-pushing-developers-toward-typed-languages/)

### Other Coding News

VS Live! 2026 preview features accessible learning in .NET, Visual Studio, and AI development. Labs demonstrate Copilot debugging and modern DevOps, delivering practical community training resources.

- [Immersive Developer Learning with Visual Studio, .NET, Azure, and GitHub Copilot: VS Live! 2026 Preview](https://devblogs.microsoft.com/visualstudio/vs-live-2026-immersive-learning-for-vs2026/)',
     excerpt      = 'This week’s coding news highlights .NET and C# feature releases, advances in modern UI development, cross-platform tooling, and the evolving use of AI-assisted programming languages.',
     content_hash = md5('This week’s coding news highlights .NET and C# feature releases, advances in modern UI development, cross-platform tooling, and the evolving use of AI-assisted programming languages.

<!--excerpt_end-->

## Coding

### .NET Ecosystem: New Features, Platforms, and Roadmaps

The .NET ecosystem receives new updates on platform support and development features. MongoDB EF Core adds Queryable Encryption and Vector Search to create secure, privacy-friendly apps with advanced semantic search options.

.NET Community Standups discuss roadmap progress. Blazor Standup previews features coming in .NET 11, with an open invitation to shape web UI planning and migration.

Cross-platform development gains more focus. Avalonia Standup demonstrates Avalonia UI as a Linux backend for .NET MAUI, answering requests for broader deployment options.

Distributed app development is featured in Orleans sessions, diving into scalable architecture patterns for maintainable cloud-native applications.

- [Secure and Intelligent: Queryable Encryption and Vector Search in MongoDB EF Core Provider](https://devblogs.microsoft.com/dotnet/mongodb-efcore-provider-queryable-encryption-vector-search/)
- [Blazor Community Standup: Planning the Future of Blazor in .NET 11](/coding/videos/blazor-community-standup-planning-the-future-of-blazor-in-net-11)
- [.NET MAUI on Linux with Avalonia: Community Standup Recap](/coding/videos/net-maui-on-linux-with-avalonia-community-standup-recap)
- [Orleans Deep Dive: Routing, Placement & Balancing](/coding/videos/orleans-deep-dive-routing-placement-and-balancing)
- [ASP.NET Community Standup: What''s Next for Orleans?](/coding/videos/aspnet-community-standup-whats-next-for-orleans)

### UI Development Tools and Open Source Progress

Microsoft open-sources XAML Studio, inviting contributions to fill gaps in Visual Studio designer functionality highlighted during recent developer discussions. The .NET Foundation release focuses on live, visual design for WinUI and MAUI.

Community conversations continue on sustaining frameworks like WinForms and WPF and the need for unified designer experiences. The project activates open innovation within the .NET UI ecosystem.

- [Microsoft Open Sources XAML Studio and Highlights Visual Studio Designer Challenges](https://devclass.com/2026/01/07/microsoft-open-sources-xaml-studio-amid-developer-discontent-with-visual-studio-designers/)

### Language Trends and AI-Assisted Coding

Analysis of programming language usage reveals Copilot’s effect on adoption of typed languages to reduce AI code issues. This backs up previous trends showing TypeScript’s rise and a movement toward stronger type systems for reliable developer contracts.

Tips on exploring gradually typed languages and testing Copilot CLI match ongoing efforts to improve maintainability as AI tools become integral to coding practices.

- [Why AI Is Pushing Developers Toward Typed Languages](https://github.blog/ai-and-ml/llms/why-ai-is-pushing-developers-toward-typed-languages/)

### Other Coding News

VS Live! 2026 preview features accessible learning in .NET, Visual Studio, and AI development. Labs demonstrate Copilot debugging and modern DevOps, delivering practical community training resources.

- [Immersive Developer Learning with Visual Studio, .NET, Azure, and GitHub Copilot: VS Live! 2026 Preview](https://devblogs.microsoft.com/visualstudio/vs-live-2026-immersive-learning-for-vs2026/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-01-12';

-- weekly-dotnet-roundup-2026-01-19  (2026-01-19)
UPDATE content_items
SET  content      = 'This week’s section features tutorials and maintenance releases supporting cross-platform development and reliable deployments. The articles focus on Android widget development with .NET MAUI and the latest service updates for .NET, helping developers extend, stabilize, and maintain their applications.

<!--excerpt_end-->

## Coding

### Advanced Android Widget Development with .NET MAUI

You can now create native Android widgets in .NET MAUI using C# (AppWidgetProvider), by following a clear step-by-step guide for project setup, structuring code, and using XML layouts.

The resource outlines how to pass data between your main app and widgets (with Preferences and SharedPreferences), trigger widget actions (like counter buttons) using PendingIntent, and manage configuration steps and Android Context for optimized resource use. Options for automatic or custom widget refreshes—like updatePeriodMillis, AlarmManager, and WorkManager—are explained, with example code for connecting Android platform features to your MAUI app.

This topic extends last week’s coverage of Avalonia UI on Linux for MAUI, as more developers adapt MAUI for broader and more unified use across platforms.

- [How to Build Android Widgets with .NET MAUI](https://devblogs.microsoft.com/dotnet/how-to-build-android-widgets-with-dotnet-maui/)

### .NET Servicing Releases for January 2026

January’s .NET servicing releases are out, delivering non-security patches for .NET 10.0 (10.0.2), 9.0 (9.0.12), and 8.0 (8.0.23), covering the runtime, ASP.NET Core, SDKs, WPF, WinForms, and EF Core. The official changelogs and issue lists are linked for details.

Updates are available as Windows and Linux installers, plus refreshed container images, making it easier to keep environments up to date across hosting scenarios. No changes were released for traditional .NET Framework. Teams should review the changes and update as needed for stability.

These releases connect to last week’s content on the .NET roadmap and developer community status.

- [.NET and .NET Framework January 2026 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-january-2026-servicing-updates/)',
     excerpt      = 'This week’s section features tutorials and maintenance releases supporting cross-platform development and reliable deployments. The articles focus on Android widget development with .NET MAUI and the latest service updates for .NET, helping developers extend, stabilize, and maintain their applications.',
     content_hash = md5('This week’s section features tutorials and maintenance releases supporting cross-platform development and reliable deployments. The articles focus on Android widget development with .NET MAUI and the latest service updates for .NET, helping developers extend, stabilize, and maintain their applications.

<!--excerpt_end-->

## Coding

### Advanced Android Widget Development with .NET MAUI

You can now create native Android widgets in .NET MAUI using C# (AppWidgetProvider), by following a clear step-by-step guide for project setup, structuring code, and using XML layouts.

The resource outlines how to pass data between your main app and widgets (with Preferences and SharedPreferences), trigger widget actions (like counter buttons) using PendingIntent, and manage configuration steps and Android Context for optimized resource use. Options for automatic or custom widget refreshes—like updatePeriodMillis, AlarmManager, and WorkManager—are explained, with example code for connecting Android platform features to your MAUI app.

This topic extends last week’s coverage of Avalonia UI on Linux for MAUI, as more developers adapt MAUI for broader and more unified use across platforms.

- [How to Build Android Widgets with .NET MAUI](https://devblogs.microsoft.com/dotnet/how-to-build-android-widgets-with-dotnet-maui/)

### .NET Servicing Releases for January 2026

January’s .NET servicing releases are out, delivering non-security patches for .NET 10.0 (10.0.2), 9.0 (9.0.12), and 8.0 (8.0.23), covering the runtime, ASP.NET Core, SDKs, WPF, WinForms, and EF Core. The official changelogs and issue lists are linked for details.

Updates are available as Windows and Linux installers, plus refreshed container images, making it easier to keep environments up to date across hosting scenarios. No changes were released for traditional .NET Framework. Teams should review the changes and update as needed for stability.

These releases connect to last week’s content on the .NET roadmap and developer community status.

- [.NET and .NET Framework January 2026 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-january-2026-servicing-updates/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-01-19';

-- weekly-dotnet-roundup-2026-01-26  (2026-01-26)
UPDATE content_items
SET  content      = 'Highlights this week include optimized patterns for .NET and SharePoint, new features for React Native Windows, and practical resources for developer engagement and education.

<!--excerpt_end-->

## Coding

### .NET Performance Optimization and Modern Web Development

This week includes a guide on making `IEnumerable<T>` iteration in .NET allocation-free. Andrew Lock details compiler behavior and testing, explaining how Reflection.Emit/DynamicMethod avoids boxing overhead on older runtimes—useful for SDKs and instrumentation. Resources on modern web teaching (Razor Pages and HTMX) show how to build with less JavaScript, leveraging Razor’s separation of logic for maintainable courses and projects.

- [Making foreach on an IEnumerable Allocation-Free in .NET with Reflection and Dynamic Methods](https://andrewlock.net/making-foreach-on-an-ienumerable-allocation-free-using-reflection-and-dynamic-methods/)
- [Teaching Modern Web Development with .NET, Razor Pages, and HTMX](/coding/videos/teaching-modern-web-development-with-net-razor-pages-and-htmx)

### React Native Windows and Cross-Platform App Development

React Native Windows v0.81 introduces features for desktop debugging and accessibility, including Hermes engine support. Early support for DevTools is coming, helping teams with breakpoints and profiling. Recent architecture work prepares the project for a move to “Fabric.” Discussions remain about whether to prefer React Native or MAUI for .NET-based cross-platform development.

- [Microsoft Updates React Native for Windows: Comparing with MAUI for Cross-Platform Development](https://devclass.com/2026/01/20/microsoft-updates-react-native-for-windows-developers-ask-why-not-use-maui/)

### SharePoint Site Optimization Guides

SharePoint site optimization advice covers scaling with site collections/hubs, filtering lists and libraries, improving metadata, auditing custom code, batching API usage, and regular diagnostics. A checklist provides steps for both urgent and longer-term maintenance across large enterprise sites.

- [Performance Optimization Tips for Large SharePoint Sites](https://dellenny.com/performance-optimization-tips-for-large-sharepoint-sites/)

### Other Coding News

A tutorial shows how to use the "Report Issue" feature in VS Code for feature requests, including tips for improving the chance of getting changes reviewed, referencing feedback cycles as a means to better software.

- [How to Request a VS Code Feature (The RIGHT Way)](/coding/videos/how-to-request-a-vs-code-feature-the-right-way)',
     excerpt      = 'Highlights this week include optimized patterns for .NET and SharePoint, new features for React Native Windows, and practical resources for developer engagement and education.',
     content_hash = md5('Highlights this week include optimized patterns for .NET and SharePoint, new features for React Native Windows, and practical resources for developer engagement and education.

<!--excerpt_end-->

## Coding

### .NET Performance Optimization and Modern Web Development

This week includes a guide on making `IEnumerable<T>` iteration in .NET allocation-free. Andrew Lock details compiler behavior and testing, explaining how Reflection.Emit/DynamicMethod avoids boxing overhead on older runtimes—useful for SDKs and instrumentation. Resources on modern web teaching (Razor Pages and HTMX) show how to build with less JavaScript, leveraging Razor’s separation of logic for maintainable courses and projects.

- [Making foreach on an IEnumerable Allocation-Free in .NET with Reflection and Dynamic Methods](https://andrewlock.net/making-foreach-on-an-ienumerable-allocation-free-using-reflection-and-dynamic-methods/)
- [Teaching Modern Web Development with .NET, Razor Pages, and HTMX](/coding/videos/teaching-modern-web-development-with-net-razor-pages-and-htmx)

### React Native Windows and Cross-Platform App Development

React Native Windows v0.81 introduces features for desktop debugging and accessibility, including Hermes engine support. Early support for DevTools is coming, helping teams with breakpoints and profiling. Recent architecture work prepares the project for a move to “Fabric.” Discussions remain about whether to prefer React Native or MAUI for .NET-based cross-platform development.

- [Microsoft Updates React Native for Windows: Comparing with MAUI for Cross-Platform Development](https://devclass.com/2026/01/20/microsoft-updates-react-native-for-windows-developers-ask-why-not-use-maui/)

### SharePoint Site Optimization Guides

SharePoint site optimization advice covers scaling with site collections/hubs, filtering lists and libraries, improving metadata, auditing custom code, batching API usage, and regular diagnostics. A checklist provides steps for both urgent and longer-term maintenance across large enterprise sites.

- [Performance Optimization Tips for Large SharePoint Sites](https://dellenny.com/performance-optimization-tips-for-large-sharepoint-sites/)

### Other Coding News

A tutorial shows how to use the "Report Issue" feature in VS Code for feature requests, including tips for improving the chance of getting changes reviewed, referencing feedback cycles as a means to better software.

- [How to Request a VS Code Feature (The RIGHT Way)](/coding/videos/how-to-request-a-vs-code-feature-the-right-way)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-01-26';

-- weekly-dotnet-roundup-2026-02-02  (2026-02-02)
UPDATE content_items
SET  content      = 'Recent updates in coding focus on expanded language features, developer tools, and practical advice for .NET, C#, Visual Studio, and programming strategies. The emphasis is on maintainability, performance, and immediate modernization tips.

<!--excerpt_end-->

## Coding

### .NET and C# Development: Practical Instrumentation, Performance, and Language Features

Andrew Lock’s guide to System.Diagnostics.Metrics APIs teaches metrics, counters, and monitoring integration for ASP.NET Core (using dotnet-counters, Datadog, and OpenTelemetry). Steve Gordon’s tutorial connects benchmarking tools and practical performance gains, showing how to find memory allocation bottlenecks and differences in reporting tools. Nick Chapsas'' "100 Essential Tips" round up key best practices for C# devs and introduce Collection Expression Arguments for C# 15/.NET 11, bringing easier parameter handling. He also explores nuanced Boolean behavior. Sentry logging integration now improves observability and connects errors, traces, and logs in .NET and MAUI apps.

- [Creating and Consuming Metrics with System.Diagnostics.Metrics APIs in .NET](https://andrewlock.net/creating-and-consuming-metrics-with-system-diagnostics-metrics-apis/)
- [Application Performance Optimisation in .NET: Practical Techniques and Tools](https://www.stevejgordon.co.uk/talk-application-performance-optimisation-in-practice-60-mins)
- [Solving .NET Memory Allocation Discrepancies: The Case of the Missing 18 Bytes](https://www.stevejgordon.co.uk/the-grand-mystery-of-the-missing-18-bytes)
- [100 Essential Tips for Writing Better C# Code](/coding/videos/100-essential-tips-for-writing-better-c-code)
- [Introducing Collection Expression Arguments in C# 15 and .NET 11](/coding/videos/introducing-collection-expression-arguments-in-c-15-and-net-11)
- [The Boolean Trick No C# Developer Knows About](/coding/videos/the-boolean-trick-no-c-developer-knows-about)
- [Integrating Sentry Logging with .NET Applications](https://dotnetfoundation.org/news-events/detail/sponsor-spotlight-sentryblog1)

### Language Design, Tooling Philosophy, and AI in Programming

Insights from Anders Hejlsberg, the creator of C# and TypeScript, highlight the importance of fast feedback, backward compatibility, and open collaboration for large codebases. Interviews outline the migration of TypeScript to Go and discuss the benefits of Go''s garbage-collected architecture. Hejlsberg also notes AI can boost productivity but does not yet provide the determinism compilers require. TypeScript’s mission of adding opt-in static typing and better tooling to JavaScript is clarified.

- [7 Key Lessons from Anders Hejlsberg: Architect of C# and TypeScript](https://github.blog/developer-skills/programming-languages-and-frameworks/7-learnings-from-anders-hejlsberg-the-architect-behind-c-and-typescript/)
- [TypeScript inventor Anders Hejlsberg: AI’s Role in Language Porting and Development Tools](https://devclass.com/2026/01/28/typescript-inventor-anders-hejlsberg-ai-is-a-big-regurgitator-of-stuff-someone-has-done/)
- [Anders Hejlsberg on the Origins of TypeScript and Fixing JavaScript](/coding/videos/anders-hejlsberg-on-the-origins-of-typescript-and-fixing-javascript)

### WebAssembly and Cross-Platform Coding: .NET, Uno, and Microsoft’s Vision

WebAssembly support is expanding, with improved AOT compilation for .NET 10 and multi-threaded workflows using Uno Platform. New surveys show faster browser loads (Chrome, Firefox, Safari), wider runtime adoption (Node.js, Wasmtime, Deno), and broader language support in Wasm 3.0.

- [WebAssembly Adoption Accelerates with Microsoft .NET 10 and Uno Platform Collaboration](https://devclass.com/2026/01/28/webassembly-gaining-adoption-behind-the-scenes-as-technology-advances/)

### Other Coding News

Visual Studio’s January 2026 update brings editor improvements (scrolling, clipboard, suggestions, Markdown preview), driving faster development. The ''winapp'' CLI preview allows simplified access to modern Windows APIs for non-VS projects, lowering the barrier for Windows API adoption from cross-platform environments.

Microsoft will present at NDC London 2026, covering .NET 10, Copilot coding, and performance, and will offer guidance for effective modernization and productivity.

- [Visual Studio January Update — Enhanced Editor Experience](https://devblogs.microsoft.com/visualstudio/visual-studio-january-update-enhanced-editor-experience/)
- [Microsoft Previews Winapp: Simplifying Access to Modern Windows APIs for Developers](https://devclass.com/2026/01/26/microsoft-previews-command-line-tool-created-because-calling-modern-windows-apis-is-too-difficult/)
- [Join Microsoft at NDC London 2026 – Let’s Build the Future of .NET Together](https://devblogs.microsoft.com/dotnet/join-us-at-ndc-london-2026/)',
     excerpt      = 'Recent updates in coding focus on expanded language features, developer tools, and practical advice for .NET, C#, Visual Studio, and programming strategies. The emphasis is on maintainability, performance, and immediate modernization tips.',
     content_hash = md5('Recent updates in coding focus on expanded language features, developer tools, and practical advice for .NET, C#, Visual Studio, and programming strategies. The emphasis is on maintainability, performance, and immediate modernization tips.

<!--excerpt_end-->

## Coding

### .NET and C# Development: Practical Instrumentation, Performance, and Language Features

Andrew Lock’s guide to System.Diagnostics.Metrics APIs teaches metrics, counters, and monitoring integration for ASP.NET Core (using dotnet-counters, Datadog, and OpenTelemetry). Steve Gordon’s tutorial connects benchmarking tools and practical performance gains, showing how to find memory allocation bottlenecks and differences in reporting tools. Nick Chapsas'' "100 Essential Tips" round up key best practices for C# devs and introduce Collection Expression Arguments for C# 15/.NET 11, bringing easier parameter handling. He also explores nuanced Boolean behavior. Sentry logging integration now improves observability and connects errors, traces, and logs in .NET and MAUI apps.

- [Creating and Consuming Metrics with System.Diagnostics.Metrics APIs in .NET](https://andrewlock.net/creating-and-consuming-metrics-with-system-diagnostics-metrics-apis/)
- [Application Performance Optimisation in .NET: Practical Techniques and Tools](https://www.stevejgordon.co.uk/talk-application-performance-optimisation-in-practice-60-mins)
- [Solving .NET Memory Allocation Discrepancies: The Case of the Missing 18 Bytes](https://www.stevejgordon.co.uk/the-grand-mystery-of-the-missing-18-bytes)
- [100 Essential Tips for Writing Better C# Code](/coding/videos/100-essential-tips-for-writing-better-c-code)
- [Introducing Collection Expression Arguments in C# 15 and .NET 11](/coding/videos/introducing-collection-expression-arguments-in-c-15-and-net-11)
- [The Boolean Trick No C# Developer Knows About](/coding/videos/the-boolean-trick-no-c-developer-knows-about)
- [Integrating Sentry Logging with .NET Applications](https://dotnetfoundation.org/news-events/detail/sponsor-spotlight-sentryblog1)

### Language Design, Tooling Philosophy, and AI in Programming

Insights from Anders Hejlsberg, the creator of C# and TypeScript, highlight the importance of fast feedback, backward compatibility, and open collaboration for large codebases. Interviews outline the migration of TypeScript to Go and discuss the benefits of Go''s garbage-collected architecture. Hejlsberg also notes AI can boost productivity but does not yet provide the determinism compilers require. TypeScript’s mission of adding opt-in static typing and better tooling to JavaScript is clarified.

- [7 Key Lessons from Anders Hejlsberg: Architect of C# and TypeScript](https://github.blog/developer-skills/programming-languages-and-frameworks/7-learnings-from-anders-hejlsberg-the-architect-behind-c-and-typescript/)
- [TypeScript inventor Anders Hejlsberg: AI’s Role in Language Porting and Development Tools](https://devclass.com/2026/01/28/typescript-inventor-anders-hejlsberg-ai-is-a-big-regurgitator-of-stuff-someone-has-done/)
- [Anders Hejlsberg on the Origins of TypeScript and Fixing JavaScript](/coding/videos/anders-hejlsberg-on-the-origins-of-typescript-and-fixing-javascript)

### WebAssembly and Cross-Platform Coding: .NET, Uno, and Microsoft’s Vision

WebAssembly support is expanding, with improved AOT compilation for .NET 10 and multi-threaded workflows using Uno Platform. New surveys show faster browser loads (Chrome, Firefox, Safari), wider runtime adoption (Node.js, Wasmtime, Deno), and broader language support in Wasm 3.0.

- [WebAssembly Adoption Accelerates with Microsoft .NET 10 and Uno Platform Collaboration](https://devclass.com/2026/01/28/webassembly-gaining-adoption-behind-the-scenes-as-technology-advances/)

### Other Coding News

Visual Studio’s January 2026 update brings editor improvements (scrolling, clipboard, suggestions, Markdown preview), driving faster development. The ''winapp'' CLI preview allows simplified access to modern Windows APIs for non-VS projects, lowering the barrier for Windows API adoption from cross-platform environments.

Microsoft will present at NDC London 2026, covering .NET 10, Copilot coding, and performance, and will offer guidance for effective modernization and productivity.

- [Visual Studio January Update — Enhanced Editor Experience](https://devblogs.microsoft.com/visualstudio/visual-studio-january-update-enhanced-editor-experience/)
- [Microsoft Previews Winapp: Simplifying Access to Modern Windows APIs for Developers](https://devclass.com/2026/01/26/microsoft-previews-command-line-tool-created-because-calling-modern-windows-apis-is-too-difficult/)
- [Join Microsoft at NDC London 2026 – Let’s Build the Future of .NET Together](https://devblogs.microsoft.com/dotnet/join-us-at-ndc-london-2026/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-02-02';

-- weekly-dotnet-roundup-2026-02-09  (2026-02-09)
UPDATE content_items
SET  content      = 'Recent coding highlights include updated AI features, platform improvements, and evolving best practices. VS Code and .NET continue to offer better tools for developers, while documentation and community resources ease migration and onboarding.

<!--excerpt_end-->

## Coding

### Visual Studio Code: Workflow, Multi-Agent, Terminal, and Beginner Experience

VS Code is now more agent-focused, continuing last week’s MCP Apps progress with better local, cloud, and background agent handling. The improved Agent Sessions Welcome Page and subagent tasking features help automate and delegate work, while upgraded UIs make agent management easier.

The January 2026 Insiders Update (v1.109) includes notable changes to the terminal (such as kitty protocol support and better input handling), improved formatting, bug fixes, updated command handling, and enhanced API access. Chat features retain context even after archiving, and custom UI support for MCP Apps gets a boost. Official tutorials walk new users through core features—including IntelliSense, issue management, and setup—connecting with the multi-agent and MCP functionality covered last week.

- [Multi-agent Development in VS Code](/multi-agent-development-in-vs-code)
- [Visual Studio Code January 2026 Insiders Update (v1.109): Terminal and Chat Improvements](https://code.visualstudio.com/updates/v1_109)
- [Learn Visual Studio Code in 15 Minutes: Official Beginner Tutorial](/learn-visual-studio-code-in-15-minutes-official-beginner-tutorial)
- [Learn Visual Studio Code in 15 Minutes: 2026 Official Beginner Tutorial](/learn-visual-studio-code-in-15-minutes-2026-official-beginner-tutorial)

### .NET Platform: Community-Driven Libraries, Roadmaps, and Deployment Changes

The .NET Data Community Standup introduces Microsoft.Extensions.DataIngestion for parsing documents and handling structured and vector workflows, supporting AI/ML cases as seen in recent community projects. Included demos and architecture advice shape next steps.

Blazor Community Standup walks through the .NET 11 ASP.NET Core and Blazor roadmap, continuing last week’s modernization efforts. .NET Framework 3.5 now requires a separate installer in new Windows releases (as of Insider Build 27965), with full support ending in 2029, so teams should start updating. The .NET MAUI Standup discusses hybrid development and ongoing plans, keeping focus on flexible deployment options.

- [.NET Data Community Standup: Introduction to Microsoft.Extensions.DataIngestion](/net-data-community-standup-introduction-to-microsoftextensionsdataingestion)
- [Blazor Community Standup: ASP.NET Core & Blazor Roadmap for .NET 11](/blazor-community-standup-aspnet-core-and-blazor-roadmap-for-net-11)
- [.NET Framework 3.5 Now Requires Standalone Deployment on New Windows Versions](https://devblogs.microsoft.com/dotnet/dotnet-framework-3-5-moves-to-standalone-deployment-in-new-versions-of-windows/)
- [.NET MAUI Community Standup: Live from MAUI Day London](/net-maui-community-standup-live-from-maui-day-london)

### Visual Studio and Editor Extensibility

Visual Studio 2026 supports background loading for MEF-based productivity extensions, shortening startup times and improving reliability. The new Microsoft.VisualStudio.SDK.Analyzers package (v17.7.98) helps detect and correct threading issues. Enable the related feature flag and follow new guides and code reviews for easier async use with extensibility code.

- [Performance Improvements for MEF-Based Visual Studio 2026 Extensions](https://devblogs.microsoft.com/visualstudio/performance-improvements-to-mef-based-editor-productivity-extensions/)

### Software Development Trends and Open Source Tooling

GitHub’s Octoverse 2025 shows TypeScript now leads Python and JavaScript in overall usage—driven partly by its better support for AI code suggestions. Project maintainers should add typing details for safer AI-assisted workflows. Python remains dominant for AI, but more projects are updating packaging, build, typing, release, and container practices to transition from prototyping to production.

Interest in tools like astral-sh/uv highlights the community’s desire for fast, repeatable deployments. Strong documentation and onboarding (e.g., VS Code and First Contributions) support sustainable open source growth and easier onboarding for new contributors.

- [What the Fastest-Growing Tools Reveal About Modern Software Development](https://github.blog/news-insights/octoverse/what-the-fastest-growing-tools-reveal-about-how-software-is-being-built/)

### Observability, Diagnostics, and Application Instrumentation in .NET

Andrew Lock reviews source generators for .NET metric instrumentation using Microsoft.Extensions.Telemetry.Abstractions and System.Diagnostics.Metrics. Source generators reduce repetitive code for metrics but sometimes have limits compared to manual instrumentation. The review offers examples to help devs evaluate trade-offs for reliability and monitoring.

- [Evaluating System.Diagnostics.Metrics Source Generators with Microsoft.Extensions.Telemetry.Abstractions](https://andrewlock.net/creating-strongly-typed-metics-with-a-source-generator/)

### Other Coding News

A technical walk-through looks at updating a WPF WebView2 control when data changes, including cache refresh strategies and how to automate reloads or use the DevTools Protocol for consistency.

- [Reliably Refreshing the WebView2 Control in WPF Applications](https://weblog.west-wind.com/posts/2026/Feb/04/Reliably-Refreshing-the-WebView2-Control)',
     excerpt      = 'Recent coding highlights include updated AI features, platform improvements, and evolving best practices. VS Code and .NET continue to offer better tools for developers, while documentation and community resources ease migration and onboarding.',
     content_hash = md5('Recent coding highlights include updated AI features, platform improvements, and evolving best practices. VS Code and .NET continue to offer better tools for developers, while documentation and community resources ease migration and onboarding.

<!--excerpt_end-->

## Coding

### Visual Studio Code: Workflow, Multi-Agent, Terminal, and Beginner Experience

VS Code is now more agent-focused, continuing last week’s MCP Apps progress with better local, cloud, and background agent handling. The improved Agent Sessions Welcome Page and subagent tasking features help automate and delegate work, while upgraded UIs make agent management easier.

The January 2026 Insiders Update (v1.109) includes notable changes to the terminal (such as kitty protocol support and better input handling), improved formatting, bug fixes, updated command handling, and enhanced API access. Chat features retain context even after archiving, and custom UI support for MCP Apps gets a boost. Official tutorials walk new users through core features—including IntelliSense, issue management, and setup—connecting with the multi-agent and MCP functionality covered last week.

- [Multi-agent Development in VS Code](/multi-agent-development-in-vs-code)
- [Visual Studio Code January 2026 Insiders Update (v1.109): Terminal and Chat Improvements](https://code.visualstudio.com/updates/v1_109)
- [Learn Visual Studio Code in 15 Minutes: Official Beginner Tutorial](/learn-visual-studio-code-in-15-minutes-official-beginner-tutorial)
- [Learn Visual Studio Code in 15 Minutes: 2026 Official Beginner Tutorial](/learn-visual-studio-code-in-15-minutes-2026-official-beginner-tutorial)

### .NET Platform: Community-Driven Libraries, Roadmaps, and Deployment Changes

The .NET Data Community Standup introduces Microsoft.Extensions.DataIngestion for parsing documents and handling structured and vector workflows, supporting AI/ML cases as seen in recent community projects. Included demos and architecture advice shape next steps.

Blazor Community Standup walks through the .NET 11 ASP.NET Core and Blazor roadmap, continuing last week’s modernization efforts. .NET Framework 3.5 now requires a separate installer in new Windows releases (as of Insider Build 27965), with full support ending in 2029, so teams should start updating. The .NET MAUI Standup discusses hybrid development and ongoing plans, keeping focus on flexible deployment options.

- [.NET Data Community Standup: Introduction to Microsoft.Extensions.DataIngestion](/net-data-community-standup-introduction-to-microsoftextensionsdataingestion)
- [Blazor Community Standup: ASP.NET Core & Blazor Roadmap for .NET 11](/blazor-community-standup-aspnet-core-and-blazor-roadmap-for-net-11)
- [.NET Framework 3.5 Now Requires Standalone Deployment on New Windows Versions](https://devblogs.microsoft.com/dotnet/dotnet-framework-3-5-moves-to-standalone-deployment-in-new-versions-of-windows/)
- [.NET MAUI Community Standup: Live from MAUI Day London](/net-maui-community-standup-live-from-maui-day-london)

### Visual Studio and Editor Extensibility

Visual Studio 2026 supports background loading for MEF-based productivity extensions, shortening startup times and improving reliability. The new Microsoft.VisualStudio.SDK.Analyzers package (v17.7.98) helps detect and correct threading issues. Enable the related feature flag and follow new guides and code reviews for easier async use with extensibility code.

- [Performance Improvements for MEF-Based Visual Studio 2026 Extensions](https://devblogs.microsoft.com/visualstudio/performance-improvements-to-mef-based-editor-productivity-extensions/)

### Software Development Trends and Open Source Tooling

GitHub’s Octoverse 2025 shows TypeScript now leads Python and JavaScript in overall usage—driven partly by its better support for AI code suggestions. Project maintainers should add typing details for safer AI-assisted workflows. Python remains dominant for AI, but more projects are updating packaging, build, typing, release, and container practices to transition from prototyping to production.

Interest in tools like astral-sh/uv highlights the community’s desire for fast, repeatable deployments. Strong documentation and onboarding (e.g., VS Code and First Contributions) support sustainable open source growth and easier onboarding for new contributors.

- [What the Fastest-Growing Tools Reveal About Modern Software Development](https://github.blog/news-insights/octoverse/what-the-fastest-growing-tools-reveal-about-how-software-is-being-built/)

### Observability, Diagnostics, and Application Instrumentation in .NET

Andrew Lock reviews source generators for .NET metric instrumentation using Microsoft.Extensions.Telemetry.Abstractions and System.Diagnostics.Metrics. Source generators reduce repetitive code for metrics but sometimes have limits compared to manual instrumentation. The review offers examples to help devs evaluate trade-offs for reliability and monitoring.

- [Evaluating System.Diagnostics.Metrics Source Generators with Microsoft.Extensions.Telemetry.Abstractions](https://andrewlock.net/creating-strongly-typed-metics-with-a-source-generator/)

### Other Coding News

A technical walk-through looks at updating a WPF WebView2 control when data changes, including cache refresh strategies and how to automate reloads or use the DevTools Protocol for consistency.

- [Reliably Refreshing the WebView2 Control in WPF Applications](https://weblog.west-wind.com/posts/2026/Feb/04/Reliably-Refreshing-the-WebView2-Control)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-02-09';

-- weekly-dotnet-roundup-2026-02-16  (2026-02-16)
UPDATE content_items
SET  content      = 'Programming languages and tools see several updates this week, with new features in .NET and TypeScript, and productivity improvements for VS Code.

<!--excerpt_end-->

## Coding

### .NET Ecosystem Updates

.NET 11 Preview 1 brings a range of updates: Zstandard compression, BFloat16 data type, better ZipArchiveEntry operations, new hard-link and crypto APIs, and collection improvements. Runtime features include async main, WebAssembly CoreCLR, interpreter/JIT updates, and more hardware support.

SDK tools see improved device selection (`dotnet run`), easier test syntax, better watch/hot reload, static analyzers, and enhanced MSBuild logging. The languages get new collections (C#), parallel F# builds, and speed increases. ASP.NET Core/Blazor gets a new UI, SignalR, and better cert management. .NET MAUI now uses CoreCLR for Android. Entity Framework Core is updated for complex JSON column types. Both VS 2026 and VS Code C# Dev Kit support these changes.

February’s .NET and .NET Framework servicing update targets supported versions and addresses CVE-2026-21218. Guidance is provided for installation, patch verification, and changelog review. These continue last week’s trend toward platform modernization, with regular previews and focus on compatibility.

- [.NET 11 Preview 1: New Features and Improvements Across the Ecosystem](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-1/)
- [.NET and .NET Framework February 2026 Servicing Releases Update](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-february-2026-servicing-updates/)

### TypeScript 6.0 Beta and Language Modernization

TypeScript 6.0 Beta will be the last version built in JavaScript before 7.0 moves to Go. Updates include changes to context-sensitive function declarations, easier Node.js imports, combined resolution/output settings, and deterministic type ordering via `--stableTypeOrdering`.

Several features are now deprecated: strict mode is default, modules align to current standards, legacy settings are dropped, iterables are always available in DOM libraries, and explicit `types` fields are required. Migration is eased with tools like `ts5to6` and configuration tips. Developers are urged to begin testing their codebases and try out 7.0 Go-native builds for feedback and confidence.

These updates continue last week’s focus on rapid TypeScript iteration, improved compatibility with AI code tools, and preparing developer teams for future platform updates.

- [Announcing TypeScript 6.0 Beta: Key Features, Deprecations, and Migration Guide](https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-beta/)

### Visual Studio Code Enhancements: AI Agents and Productivity Tools

VS Code 1.109 adds usability and workflow improvements. You can now "Ask Questions" in the editor, use agent skills for code automation, and run subagents in parallel for advanced tasks. Editor changes include double-click selection of brackets and strings, a browser preview, and upgraded MCP cloud app support.

A video demo shows agent steering in VS Code Insiders, letting you queue and control agent tasks directly for repeatable, controlled workflow automation. These additions build on the theme of expanded automation, agent control, and integration for both new and advanced users.

- [VS Code 1.109 Release Highlights: Editor Improvements & New Features](/vs-code-1109-release-highlights-editor-improvements-and-new-features)
- [Let it Cook: Agent Steering & Queueing in VS Code Insiders](/let-it-cook-agent-steering-and-queueing-in-vs-code-insiders)

### Other Coding News

A recent Rx.NET v7 live session covers new asynchronous APIs and event stream features, adoption recommendations, and future plans, all presented by Rx.NET team engineers.

- [On .NET Live: Rx.NET v7 and Futures](/on-net-live-rxnet-v7-and-futures)',
     excerpt      = 'Programming languages and tools see several updates this week, with new features in .NET and TypeScript, and productivity improvements for VS Code.',
     content_hash = md5('Programming languages and tools see several updates this week, with new features in .NET and TypeScript, and productivity improvements for VS Code.

<!--excerpt_end-->

## Coding

### .NET Ecosystem Updates

.NET 11 Preview 1 brings a range of updates: Zstandard compression, BFloat16 data type, better ZipArchiveEntry operations, new hard-link and crypto APIs, and collection improvements. Runtime features include async main, WebAssembly CoreCLR, interpreter/JIT updates, and more hardware support.

SDK tools see improved device selection (`dotnet run`), easier test syntax, better watch/hot reload, static analyzers, and enhanced MSBuild logging. The languages get new collections (C#), parallel F# builds, and speed increases. ASP.NET Core/Blazor gets a new UI, SignalR, and better cert management. .NET MAUI now uses CoreCLR for Android. Entity Framework Core is updated for complex JSON column types. Both VS 2026 and VS Code C# Dev Kit support these changes.

February’s .NET and .NET Framework servicing update targets supported versions and addresses CVE-2026-21218. Guidance is provided for installation, patch verification, and changelog review. These continue last week’s trend toward platform modernization, with regular previews and focus on compatibility.

- [.NET 11 Preview 1: New Features and Improvements Across the Ecosystem](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-1/)
- [.NET and .NET Framework February 2026 Servicing Releases Update](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-february-2026-servicing-updates/)

### TypeScript 6.0 Beta and Language Modernization

TypeScript 6.0 Beta will be the last version built in JavaScript before 7.0 moves to Go. Updates include changes to context-sensitive function declarations, easier Node.js imports, combined resolution/output settings, and deterministic type ordering via `--stableTypeOrdering`.

Several features are now deprecated: strict mode is default, modules align to current standards, legacy settings are dropped, iterables are always available in DOM libraries, and explicit `types` fields are required. Migration is eased with tools like `ts5to6` and configuration tips. Developers are urged to begin testing their codebases and try out 7.0 Go-native builds for feedback and confidence.

These updates continue last week’s focus on rapid TypeScript iteration, improved compatibility with AI code tools, and preparing developer teams for future platform updates.

- [Announcing TypeScript 6.0 Beta: Key Features, Deprecations, and Migration Guide](https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-beta/)

### Visual Studio Code Enhancements: AI Agents and Productivity Tools

VS Code 1.109 adds usability and workflow improvements. You can now "Ask Questions" in the editor, use agent skills for code automation, and run subagents in parallel for advanced tasks. Editor changes include double-click selection of brackets and strings, a browser preview, and upgraded MCP cloud app support.

A video demo shows agent steering in VS Code Insiders, letting you queue and control agent tasks directly for repeatable, controlled workflow automation. These additions build on the theme of expanded automation, agent control, and integration for both new and advanced users.

- [VS Code 1.109 Release Highlights: Editor Improvements & New Features](/vs-code-1109-release-highlights-editor-improvements-and-new-features)
- [Let it Cook: Agent Steering & Queueing in VS Code Insiders](/let-it-cook-agent-steering-and-queueing-in-vs-code-insiders)

### Other Coding News

A recent Rx.NET v7 live session covers new asynchronous APIs and event stream features, adoption recommendations, and future plans, all presented by Rx.NET team engineers.

- [On .NET Live: Rx.NET v7 and Futures](/on-net-live-rxnet-v7-and-futures)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-02-16';

-- weekly-dotnet-roundup-2026-02-23  (2026-02-23)
UPDATE content_items
SET  content      = 'Coding updates this week highlight new .NET runtime features, expanded agent workflows in VS Code, and community stories in language development and open source. These changes provide more options for reliable code, stronger automation, and better integration in developer tools.

<!--excerpt_end-->

## Coding

### .NET Runtime and Instrumentation Advances

.NET 11 improves Async/Await performance for high-concurrency applications, reducing overhead and boosting scalability. Developers are given practical advice for using these updates in real-world code. A detailed guide explores System.Diagnostics.Metrics APIs in .NET, comparing standard and observable metric instruments, and suggests when to use push or pull reporting and OpenTelemetry integration.

These instrumentation improvements continue .NET’s movement toward greater observability and diagnostics, following last week’s release highlights.

- [Async Await Just Got A Massive Improvement in .NET](/dotnet/videos/async-await-just-got-a-massive-improvement-in-net)
- [Understanding Observable and Standard Instruments with System.Diagnostics.Metrics APIs](https://andrewlock.net/creating-standard-and-observable-instruments/)

### Visual Studio Code Workflow and Browser Integration

VS Code’s integrated browser brings live preview, real-time debugging, Chrome DevTools, and AI chat features directly into the editor. This update lowers the need for context switching and allows for smarter agent extension development. New browser workflows and agent-management features drive a tighter feedback loop and provide more productive coding experiences.

Live coding challenges experiment with agent workflows and share actionable discoveries for continuous improvement.

- [The Browser in Your Editor: Integrated Web Preview in VS Code](/dotnet/videos/the-browser-in-your-editor-integrated-web-preview-in-vs-code)
- [Live Coding Challenge: Exploring Agent Workflows in Visual Studio Code](/dotnet/videos/live-coding-challenge-exploring-agent-workflows-in-visual-studio-code)

### Open Source Ecosystems and Language Histories

A pair of interviews with Anders Hejlsberg discuss why making TypeScript open source drove growth, trust, and quality—along with the impact of migrating to GitHub for increased openness and transparent, sustainable development. These discussions provide perspective on why open-source processes benefit technical communities, as covered in earlier roundups.

- [Why TypeScript Had to Be Open Source](/dotnet/videos/why-typescript-had-to-be-open-source)
- [Why TypeScript Moved to GitHub in 2014](/dotnet/videos/why-typescript-moved-to-github-in-2014)

### Windows MIDI Services: Next-Gen Music Tech on Windows 11

Windows 11 introduces updated MIDI Services, adding support for MIDI 1.0/2.0, high-resolution data, legacy compatibility, and multi-client MIDI ports. New features include metadata editing, loopback endpoints, communication between apps, better timing, updated drivers, and an open SDK. Future plans include USB audio class, BLE MIDI, and expanded routing features.

- [Windows 11 Introduces Advanced MIDI 2.0 Support for Musicians and Developers](https://blogs.windows.com/windowsexperience/2026/02/17/making-music-with-midi-just-got-a-real-boost-in-windows-11/)

### PowerShell, OpenSSH, and DSC: Long-Term Roadmap

For 2026, the PowerShell, OpenSSH, and DSC roadmap details improvements to PowerShell 7.7—covering script path flexibility, Bash-style aliases, AI-powered scripting assistance, predictive IntelliSense in PSReadLine, and module gallery migration. OpenSSH gains better authentication methods and DSC v3.2 includes Linux/Python adapters. Regular updates and open collaboration continue for module development and automation on both Windows and Linux.

AI-driven scripting connects to wider agent-based process advancements in the industry.

- [PowerShell, OpenSSH, and DSC: Planned Team Investments for 2026](https://devblogs.microsoft.com/powershell/powershell-openssh-and-dsc-team-investments-for-2026/)

### Inside Model Context Protocol: Workflow and Open Source Journey

An interview with David Soria Parra explores the evolution of Model Context Protocol, its challenges, the journey to Linux Foundation, the Python/Azure tech stack, the "Skills" approach, and best practices for open-source collaboration and protocol leadership.

The discussion sheds light on the journey from internal protocol to open source, reflecting the process lessons from earlier MCP updates.

- [Inside MCP: Origin, Workflow, and Future with David Soria Parra](/dotnet/videos/inside-mcp-origin-workflow-and-future-with-david-soria-parra)',
     excerpt      = 'Coding updates this week highlight new .NET runtime features, expanded agent workflows in VS Code, and community stories in language development and open source. These changes provide more options for reliable code, stronger automation, and better integration in developer tools.',
     content_hash = md5('Coding updates this week highlight new .NET runtime features, expanded agent workflows in VS Code, and community stories in language development and open source. These changes provide more options for reliable code, stronger automation, and better integration in developer tools.

<!--excerpt_end-->

## Coding

### .NET Runtime and Instrumentation Advances

.NET 11 improves Async/Await performance for high-concurrency applications, reducing overhead and boosting scalability. Developers are given practical advice for using these updates in real-world code. A detailed guide explores System.Diagnostics.Metrics APIs in .NET, comparing standard and observable metric instruments, and suggests when to use push or pull reporting and OpenTelemetry integration.

These instrumentation improvements continue .NET’s movement toward greater observability and diagnostics, following last week’s release highlights.

- [Async Await Just Got A Massive Improvement in .NET](/dotnet/videos/async-await-just-got-a-massive-improvement-in-net)
- [Understanding Observable and Standard Instruments with System.Diagnostics.Metrics APIs](https://andrewlock.net/creating-standard-and-observable-instruments/)

### Visual Studio Code Workflow and Browser Integration

VS Code’s integrated browser brings live preview, real-time debugging, Chrome DevTools, and AI chat features directly into the editor. This update lowers the need for context switching and allows for smarter agent extension development. New browser workflows and agent-management features drive a tighter feedback loop and provide more productive coding experiences.

Live coding challenges experiment with agent workflows and share actionable discoveries for continuous improvement.

- [The Browser in Your Editor: Integrated Web Preview in VS Code](/dotnet/videos/the-browser-in-your-editor-integrated-web-preview-in-vs-code)
- [Live Coding Challenge: Exploring Agent Workflows in Visual Studio Code](/dotnet/videos/live-coding-challenge-exploring-agent-workflows-in-visual-studio-code)

### Open Source Ecosystems and Language Histories

A pair of interviews with Anders Hejlsberg discuss why making TypeScript open source drove growth, trust, and quality—along with the impact of migrating to GitHub for increased openness and transparent, sustainable development. These discussions provide perspective on why open-source processes benefit technical communities, as covered in earlier roundups.

- [Why TypeScript Had to Be Open Source](/dotnet/videos/why-typescript-had-to-be-open-source)
- [Why TypeScript Moved to GitHub in 2014](/dotnet/videos/why-typescript-moved-to-github-in-2014)

### Windows MIDI Services: Next-Gen Music Tech on Windows 11

Windows 11 introduces updated MIDI Services, adding support for MIDI 1.0/2.0, high-resolution data, legacy compatibility, and multi-client MIDI ports. New features include metadata editing, loopback endpoints, communication between apps, better timing, updated drivers, and an open SDK. Future plans include USB audio class, BLE MIDI, and expanded routing features.

- [Windows 11 Introduces Advanced MIDI 2.0 Support for Musicians and Developers](https://blogs.windows.com/windowsexperience/2026/02/17/making-music-with-midi-just-got-a-real-boost-in-windows-11/)

### PowerShell, OpenSSH, and DSC: Long-Term Roadmap

For 2026, the PowerShell, OpenSSH, and DSC roadmap details improvements to PowerShell 7.7—covering script path flexibility, Bash-style aliases, AI-powered scripting assistance, predictive IntelliSense in PSReadLine, and module gallery migration. OpenSSH gains better authentication methods and DSC v3.2 includes Linux/Python adapters. Regular updates and open collaboration continue for module development and automation on both Windows and Linux.

AI-driven scripting connects to wider agent-based process advancements in the industry.

- [PowerShell, OpenSSH, and DSC: Planned Team Investments for 2026](https://devblogs.microsoft.com/powershell/powershell-openssh-and-dsc-team-investments-for-2026/)

### Inside Model Context Protocol: Workflow and Open Source Journey

An interview with David Soria Parra explores the evolution of Model Context Protocol, its challenges, the journey to Linux Foundation, the Python/Azure tech stack, the "Skills" approach, and best practices for open-source collaboration and protocol leadership.

The discussion sheds light on the journey from internal protocol to open source, reflecting the process lessons from earlier MCP updates.

- [Inside MCP: Origin, Workflow, and Future with David Soria Parra](/dotnet/videos/inside-mcp-origin-workflow-and-future-with-david-soria-parra)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-02-23';

-- SKIP 2026-03-02 : no DB record for section 'dotnet'

-- SKIP 2026-03-09 : no DB record for section 'dotnet'

-- weekly-dotnet-roundup-2026-03-16  (2026-03-16)
UPDATE content_items
SET  content      = 'This week’s .NET updates split into new features to try and updates to apply. .NET 11 Preview 2 added runtime, observability, and web/data updates, while .NET 10/9/8 servicing focused on secure, stable builds plus an out-of-band macOS debugger hotfix for VS Code users.

<!--excerpt_end-->

## .NET

### .NET 11 Preview 2: runtime, web, and data stack updates

.NET 11 Preview 2 includes updates that show up in applications and pipelines. Runtime work continues on async improvements (V2) plus JIT/VM changes like cached interface dispatch for interface-heavy hot paths. Libraries also get targeted performance improvements (for example, Matrix4x4.GetDeterminant is ~15% faster) and functional additions like generic GetTypeInfo support in System.Text.Json for source-gen and type-metadata scenarios.

SDK changes reduce day-to-day friction: smaller SDK installers on Linux/macOS (useful for development and smaller CI images), improved analyzers, and more warnings/targets to surface issues earlier. Language changes are light for C# and Visual Basic, while F# adds developer-focused updates like overload resolution caching, new preprocessor directives, and new collection functions.

ASP.NET Core and Blazor leaned into built-in platform features. Native OpenTelemetry tracing in ASP.NET Core reduces custom tracing setup, OpenAPI 3.2.0 support updates API description workflows, and there’s a new .NET Web Worker template. Blazor adds TempData for state that survives redirects/navigation. EF Core adds server-side translation for LINQ MaxBy/MinBy plus SQL Server features like DiskANN vector indexes and VECTOR_SEARCH(), along with full-text catalogs/indexes and JSON_CONTAINS().

UI and deployment edges also moved forward. .NET MAUI includes Map control and TypedBinding performance work, immutability annotations for Color/Font, and API consistency (notably VisualStateManager), while Windows Forms/WPF get reliability updates. Container teams also get smaller images (SDK images up to ~17% smaller), which improves pulls and CI throughput. Preview 2 is available via the .NET 11 Preview 2 SDK, Visual Studio 2026 Insiders on Windows, or VS Code with C# Dev Kit for early validation and tracking release notes.

- [.NET 11 Preview 2: New Features and Improvements Across Runtime, SDK, and Libraries](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-2/)

### Built-in Zstandard compression in .NET 11 and ASP.NET Core

.NET 11 adds built-in Zstandard (zstd) support. System.IO.Compression will include zstd alongside gzip/Deflate and Brotli, which removes the need for third-party wrappers when you want zstd’s compression profile and gives framework components a standard implementation to use.

ASP.NET Core also gains zstd as an out-of-the-box HTTP response compression encoding, so teams can evaluate it like gzip or Brotli when balancing payload size vs CPU. The session covers the new APIs for direct use (streams/files) and indirect use (middleware/framework) and includes benchmarks to help decide when zstd fits versus existing algorithms.

- [ASP.NET Community Standup: Zstandard Compression Comes to .NET 11](https://www.youtube.com/watch?v=JDhs-5wVTnw)

### .NET servicing: March security patches and a macOS VS Code debugger hotfix

March 2026 servicing shipped clear “update now” guidance for teams on multiple .NET versions. Patched releases are .NET 10.0.4, .NET 9.0.14, and .NET 8.0.25, with installers/binaries, MCR container images, Linux package instructions, and known-issues pages. Security includes three “.NET Security Feature Bypass” CVEs: CVE-2026-26130 (.NET 10/9/8), CVE-2026-26127 (.NET 10/9), and CVE-2026-26131 (.NET 10 only). The practical guidance is to update SDKs/runtimes on developer machines, build agents, deployed hosts, and base images, then validate against the known-issues lists. The roundup also links servicing-approved-issue queries for ASP.NET Core, EF Core, runtime, WPF, and more so you can find fixes that may affect your applications.

A second servicing item followed: .NET 10.0.5 out-of-band fixed a macOS-only debugger crash regression introduced in .NET 10.0.4 when debugging from VS Code, especially impacting Apple Silicon (ARM64). If affected, install the .NET 10.0.5 SDK, restart VS Code, and confirm with `dotnet --version`. If you are not on macOS or are not seeing the crash, the guidance says staying on 10.0.4 is fine because 10.0.5 targets this issue narrowly.

- [.NET and .NET Framework March 2026 Servicing Updates: Security and Release Roundup](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-march-2026-servicing-updates/)
- [.NET 10.0.5 Out-of-Band Release: Debugger Crash Fix for macOS with Visual Studio Code](https://devblogs.microsoft.com/dotnet/dotnet-10-0-5-oob-release-macos-debugger-fix/)

### Other .NET News

NetEscapades.EnumGenerators adjusted its package layout to better match how source generators are consumed. The split adds a metapackage for the default path, a Generators-only package for keeping the generator private (for example, `PrivateAssets="All"`), and a RuntimeDependencies package for option/serialization types that generated code may need. The change addresses missing-type issues when teams exclude runtime assets.

- [Splitting the NetEscapades.EnumGenerators Packages: Architecture and Stable Release Path](https://andrewlock.net/splitting-the-netescapades-enumgenerators-packages-the-road-to-a-stable-release/)

A MonoGame walkthrough provided an end-to-end first project on modern .NET (recommended .NET SDK 10), covering setup, templates, the edit/build/run loop, and 2D basics like sprite rendering and input handling. It also reinforces MonoGame’s cross-platform support on Windows and Linux using Visual Studio or VS Code.

- [Make your First Game with .NET and MonoGame](https://www.youtube.com/watch?v=y631qBfWk_I)',
     excerpt      = 'This week’s .NET updates split into new features to try and updates to apply. .NET 11 Preview 2 added runtime, observability, and web/data updates, while .NET 10/9/8 servicing focused on secure, stable builds plus an out-of-band macOS debugger hotfix for VS Code users.',
     content_hash = md5('This week’s .NET updates split into new features to try and updates to apply. .NET 11 Preview 2 added runtime, observability, and web/data updates, while .NET 10/9/8 servicing focused on secure, stable builds plus an out-of-band macOS debugger hotfix for VS Code users.

<!--excerpt_end-->

## .NET

### .NET 11 Preview 2: runtime, web, and data stack updates

.NET 11 Preview 2 includes updates that show up in applications and pipelines. Runtime work continues on async improvements (V2) plus JIT/VM changes like cached interface dispatch for interface-heavy hot paths. Libraries also get targeted performance improvements (for example, Matrix4x4.GetDeterminant is ~15% faster) and functional additions like generic GetTypeInfo support in System.Text.Json for source-gen and type-metadata scenarios.

SDK changes reduce day-to-day friction: smaller SDK installers on Linux/macOS (useful for development and smaller CI images), improved analyzers, and more warnings/targets to surface issues earlier. Language changes are light for C# and Visual Basic, while F# adds developer-focused updates like overload resolution caching, new preprocessor directives, and new collection functions.

ASP.NET Core and Blazor leaned into built-in platform features. Native OpenTelemetry tracing in ASP.NET Core reduces custom tracing setup, OpenAPI 3.2.0 support updates API description workflows, and there’s a new .NET Web Worker template. Blazor adds TempData for state that survives redirects/navigation. EF Core adds server-side translation for LINQ MaxBy/MinBy plus SQL Server features like DiskANN vector indexes and VECTOR_SEARCH(), along with full-text catalogs/indexes and JSON_CONTAINS().

UI and deployment edges also moved forward. .NET MAUI includes Map control and TypedBinding performance work, immutability annotations for Color/Font, and API consistency (notably VisualStateManager), while Windows Forms/WPF get reliability updates. Container teams also get smaller images (SDK images up to ~17% smaller), which improves pulls and CI throughput. Preview 2 is available via the .NET 11 Preview 2 SDK, Visual Studio 2026 Insiders on Windows, or VS Code with C# Dev Kit for early validation and tracking release notes.

- [.NET 11 Preview 2: New Features and Improvements Across Runtime, SDK, and Libraries](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-2/)

### Built-in Zstandard compression in .NET 11 and ASP.NET Core

.NET 11 adds built-in Zstandard (zstd) support. System.IO.Compression will include zstd alongside gzip/Deflate and Brotli, which removes the need for third-party wrappers when you want zstd’s compression profile and gives framework components a standard implementation to use.

ASP.NET Core also gains zstd as an out-of-the-box HTTP response compression encoding, so teams can evaluate it like gzip or Brotli when balancing payload size vs CPU. The session covers the new APIs for direct use (streams/files) and indirect use (middleware/framework) and includes benchmarks to help decide when zstd fits versus existing algorithms.

- [ASP.NET Community Standup: Zstandard Compression Comes to .NET 11](https://www.youtube.com/watch?v=JDhs-5wVTnw)

### .NET servicing: March security patches and a macOS VS Code debugger hotfix

March 2026 servicing shipped clear “update now” guidance for teams on multiple .NET versions. Patched releases are .NET 10.0.4, .NET 9.0.14, and .NET 8.0.25, with installers/binaries, MCR container images, Linux package instructions, and known-issues pages. Security includes three “.NET Security Feature Bypass” CVEs: CVE-2026-26130 (.NET 10/9/8), CVE-2026-26127 (.NET 10/9), and CVE-2026-26131 (.NET 10 only). The practical guidance is to update SDKs/runtimes on developer machines, build agents, deployed hosts, and base images, then validate against the known-issues lists. The roundup also links servicing-approved-issue queries for ASP.NET Core, EF Core, runtime, WPF, and more so you can find fixes that may affect your applications.

A second servicing item followed: .NET 10.0.5 out-of-band fixed a macOS-only debugger crash regression introduced in .NET 10.0.4 when debugging from VS Code, especially impacting Apple Silicon (ARM64). If affected, install the .NET 10.0.5 SDK, restart VS Code, and confirm with `dotnet --version`. If you are not on macOS or are not seeing the crash, the guidance says staying on 10.0.4 is fine because 10.0.5 targets this issue narrowly.

- [.NET and .NET Framework March 2026 Servicing Updates: Security and Release Roundup](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-march-2026-servicing-updates/)
- [.NET 10.0.5 Out-of-Band Release: Debugger Crash Fix for macOS with Visual Studio Code](https://devblogs.microsoft.com/dotnet/dotnet-10-0-5-oob-release-macos-debugger-fix/)

### Other .NET News

NetEscapades.EnumGenerators adjusted its package layout to better match how source generators are consumed. The split adds a metapackage for the default path, a Generators-only package for keeping the generator private (for example, `PrivateAssets="All"`), and a RuntimeDependencies package for option/serialization types that generated code may need. The change addresses missing-type issues when teams exclude runtime assets.

- [Splitting the NetEscapades.EnumGenerators Packages: Architecture and Stable Release Path](https://andrewlock.net/splitting-the-netescapades-enumgenerators-packages-the-road-to-a-stable-release/)

A MonoGame walkthrough provided an end-to-end first project on modern .NET (recommended .NET SDK 10), covering setup, templates, the edit/build/run loop, and 2D basics like sprite rendering and input handling. It also reinforces MonoGame’s cross-platform support on Windows and Linux using Visual Studio or VS Code.

- [Make your First Game with .NET and MonoGame](https://www.youtube.com/watch?v=y631qBfWk_I)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-03-16';

-- weekly-dotnet-roundup-2026-03-23  (2026-03-23)
UPDATE content_items
SET  content      = 'This week''s .NET updates landed where teams feel everyday friction: shells and editors that drive automation and debugging, and UI controls that need to stay responsive under real data. Building on last week''s "apply updates" focus (.NET servicing guidance and the macOS VS Code debugger hotfix) plus "try new features" (.NET 11 Preview 2 wave), this week continues the same two-track story: a clearer production baseline for tooling and a Preview 2 feature that is easy to validate in apps.

<!--excerpt_end-->

## .NET

### PowerShell 7.6 LTS on .NET 10: production automation baseline, with shell and module refinements

PowerShell 7.6 is now GA as the next LTS release, built on .NET 10 (LTS), giving teams a stable target for production scripting and automation where predictable behavior matters. Like last week''s servicing guidance ("keep runtime/tooling patched," including the out-of-band .NET 10.0.5 VS Code macOS debugger fix), 7.6 provides a steadier baseline for build agents, automation runners, and operator scripts, especially for fleets standardizing on .NET 10 LTS.

Alongside engine reliability and cross-platform consistency work, the release updates key in-box modules (PSReadLine, PSResourceGet, ThreadJob), so environments may see behavior changes simply by moving to 7.6 even if modules were not intentionally updated before. This mirrors last week''s "hidden dependency" point: module and runtime layers are often where friction appears first, including on build agents and base images.

Much of the polish is focused on interactive authoring and discoverability. Tab completion has many fixes and expansions (paths across providers, parameter value completions, more contexts where completion works, module completion by shortname), which reduces manual lookup and trial-and-error. Automation-visible updates include `Get-Clipboard -Delimiter`, `Register-ArgumentCompleter -NativeFallback` for native-command completion, `Get-Command -ExcludeModule`, `New-Item` treating `-Target` literally, and `Start-Process -Wait` improving polling efficiency to reduce overhead in scripts waiting on child processes.

On platform conventions, 7.6 adds aliases `PSForEach()` and `PSWhere()` for intrinsic `ForEach()` and `Where()` methods and respects `NO_COLOR` for stderr output in the console host. API and compatibility updates include Unix SystemPolicy public APIs being visible but no-op for `PowerShellStandard.Library`, and certificate DNS name handling using `X509SubjectAlternativeNameExtension.EnumerateDnsNames()`.

Several formerly optional or preview features are now treated as mainstream (`PSFeedbackProvider`, `PSNativeWindowsTildeExpansion`, `PSRedirectToVariable`, `PSSubsystemPluginModel`), which reduces "feature flag semantics" surprises in production. As with any LTS bump, there are breaking changes to validate: `Join-Path` changes `-ChildPath` to `string[]`, `WildcardPattern.Escape()` now escapes lone backticks correctly (changing outputs for scripts relying on prior behavior), and the `GetHelpCommand` trace source name drops a trailing space (affecting trace-name matching). The post links install guidance and What''s New docs and notes ongoing PowerShell 7.7 previews.

- [Announcing PowerShell 7.6 (LTS) GA Release](https://devblogs.microsoft.com/powershell/announcing-powershell-7-6/)

### .NET MAUI 11 Preview 2: built-in map pin clustering for Android and iOS/Mac Catalyst

Following last week''s .NET 11 Preview 2 rundown (including the MAUI Map control landing in Preview 2), this is one of the most visible additions in apps: .NET MAUI 11 Preview 2 adds built-in pin clustering in the `Map` control for Android and iOS/Mac Catalyst. With `IsClusteringEnabled="True"`, overlapping pins collapse into cluster markers with a count when zoomed out, then expand as users zoom in, which improves usability and performance on crowded maps.

The feature supports practical organization via `ClusteringIdentifier` strings per `Pin`: pins with the same identifier cluster together (for example, "coffee"), while different identifiers will not merge even if nearby (keeping "parks" separate). For interaction, `Map` exposes `ClusterClicked`; `ClusterClickedEventArgs` includes the cluster `Pins`, `Location`, and a `Handled` flag to suppress default zoom-to-cluster behavior (the sample shows listing pin labels via `DisplayAlert`).

Under the hood, Android uses a custom grid-based algorithm recalculated on zoom changes and avoids external dependencies, while iOS/Mac Catalyst uses MapKit''s native `MKClusterAnnotation` for platform-native behavior and animations. To try it you need .NET 11 Preview 2 and updated MAUI workloads; the post also calls out Visual Studio 2026 Insiders on Windows or VS Code with C# Dev Kit, continuing last week''s "validate previews in real toolchains" guidance. Samples are updated (the `maui-samples` Maps demo adds a Clustering page) and Microsoft Learn docs are refreshed.

- [Pin Clustering in .NET MAUI Maps](https://devblogs.microsoft.com/dotnet/pin-clustering-in-dotnet-maui-maps/)

### Other .NET News

Last week''s .NET servicing roundup included the out-of-band macOS VS Code debugger fix, and this week''s VS Code Insiders notes continue the "make the daily loop smoother" direction for cross-platform debugging and local workflows.

VS Code Insiders kept refining cross-platform workflows that .NET teams tend to hit quickly. The integrated browser can now bypass certificate errors for localhost HTTPS with self-signed certs, which helps local secure-context loops (OAuth redirects, secure cookies, service workers) without switching browsers or reworking trust stores. Debug config sharing is cleaner with new `launch.json` top-level `"windows"`, `"linux"`, `"osx"` properties so repos can keep platform-specific entries in one file while hiding irrelevant configs on other OSes, reducing noise in the Run and Debug UI. There''s also a WSL fix for "Reveal in File Explorer," plus UX/accessibility improvements like better screen reader labels and image carousel zoom for inspecting assets.

- [''Visual Studio Code 1.113 (Insiders): update notes for March 2026''](https://code.visualstudio.com/updates/v1_113)',
     excerpt      = 'This week''s .NET updates landed where teams feel everyday friction: shells and editors that drive automation and debugging, and UI controls that need to stay responsive under real data. Building on last week''s "apply updates" focus (.NET servicing guidance and the macOS VS Code debugger hotfix) plus "try new features" (.NET 11 Preview 2 wave), this week continues the same two-track story: a clearer production baseline for tooling and a Preview 2 feature that is easy to validate in apps.',
     content_hash = md5('This week''s .NET updates landed where teams feel everyday friction: shells and editors that drive automation and debugging, and UI controls that need to stay responsive under real data. Building on last week''s "apply updates" focus (.NET servicing guidance and the macOS VS Code debugger hotfix) plus "try new features" (.NET 11 Preview 2 wave), this week continues the same two-track story: a clearer production baseline for tooling and a Preview 2 feature that is easy to validate in apps.

<!--excerpt_end-->

## .NET

### PowerShell 7.6 LTS on .NET 10: production automation baseline, with shell and module refinements

PowerShell 7.6 is now GA as the next LTS release, built on .NET 10 (LTS), giving teams a stable target for production scripting and automation where predictable behavior matters. Like last week''s servicing guidance ("keep runtime/tooling patched," including the out-of-band .NET 10.0.5 VS Code macOS debugger fix), 7.6 provides a steadier baseline for build agents, automation runners, and operator scripts, especially for fleets standardizing on .NET 10 LTS.

Alongside engine reliability and cross-platform consistency work, the release updates key in-box modules (PSReadLine, PSResourceGet, ThreadJob), so environments may see behavior changes simply by moving to 7.6 even if modules were not intentionally updated before. This mirrors last week''s "hidden dependency" point: module and runtime layers are often where friction appears first, including on build agents and base images.

Much of the polish is focused on interactive authoring and discoverability. Tab completion has many fixes and expansions (paths across providers, parameter value completions, more contexts where completion works, module completion by shortname), which reduces manual lookup and trial-and-error. Automation-visible updates include `Get-Clipboard -Delimiter`, `Register-ArgumentCompleter -NativeFallback` for native-command completion, `Get-Command -ExcludeModule`, `New-Item` treating `-Target` literally, and `Start-Process -Wait` improving polling efficiency to reduce overhead in scripts waiting on child processes.

On platform conventions, 7.6 adds aliases `PSForEach()` and `PSWhere()` for intrinsic `ForEach()` and `Where()` methods and respects `NO_COLOR` for stderr output in the console host. API and compatibility updates include Unix SystemPolicy public APIs being visible but no-op for `PowerShellStandard.Library`, and certificate DNS name handling using `X509SubjectAlternativeNameExtension.EnumerateDnsNames()`.

Several formerly optional or preview features are now treated as mainstream (`PSFeedbackProvider`, `PSNativeWindowsTildeExpansion`, `PSRedirectToVariable`, `PSSubsystemPluginModel`), which reduces "feature flag semantics" surprises in production. As with any LTS bump, there are breaking changes to validate: `Join-Path` changes `-ChildPath` to `string[]`, `WildcardPattern.Escape()` now escapes lone backticks correctly (changing outputs for scripts relying on prior behavior), and the `GetHelpCommand` trace source name drops a trailing space (affecting trace-name matching). The post links install guidance and What''s New docs and notes ongoing PowerShell 7.7 previews.

- [Announcing PowerShell 7.6 (LTS) GA Release](https://devblogs.microsoft.com/powershell/announcing-powershell-7-6/)

### .NET MAUI 11 Preview 2: built-in map pin clustering for Android and iOS/Mac Catalyst

Following last week''s .NET 11 Preview 2 rundown (including the MAUI Map control landing in Preview 2), this is one of the most visible additions in apps: .NET MAUI 11 Preview 2 adds built-in pin clustering in the `Map` control for Android and iOS/Mac Catalyst. With `IsClusteringEnabled="True"`, overlapping pins collapse into cluster markers with a count when zoomed out, then expand as users zoom in, which improves usability and performance on crowded maps.

The feature supports practical organization via `ClusteringIdentifier` strings per `Pin`: pins with the same identifier cluster together (for example, "coffee"), while different identifiers will not merge even if nearby (keeping "parks" separate). For interaction, `Map` exposes `ClusterClicked`; `ClusterClickedEventArgs` includes the cluster `Pins`, `Location`, and a `Handled` flag to suppress default zoom-to-cluster behavior (the sample shows listing pin labels via `DisplayAlert`).

Under the hood, Android uses a custom grid-based algorithm recalculated on zoom changes and avoids external dependencies, while iOS/Mac Catalyst uses MapKit''s native `MKClusterAnnotation` for platform-native behavior and animations. To try it you need .NET 11 Preview 2 and updated MAUI workloads; the post also calls out Visual Studio 2026 Insiders on Windows or VS Code with C# Dev Kit, continuing last week''s "validate previews in real toolchains" guidance. Samples are updated (the `maui-samples` Maps demo adds a Clustering page) and Microsoft Learn docs are refreshed.

- [Pin Clustering in .NET MAUI Maps](https://devblogs.microsoft.com/dotnet/pin-clustering-in-dotnet-maui-maps/)

### Other .NET News

Last week''s .NET servicing roundup included the out-of-band macOS VS Code debugger fix, and this week''s VS Code Insiders notes continue the "make the daily loop smoother" direction for cross-platform debugging and local workflows.

VS Code Insiders kept refining cross-platform workflows that .NET teams tend to hit quickly. The integrated browser can now bypass certificate errors for localhost HTTPS with self-signed certs, which helps local secure-context loops (OAuth redirects, secure cookies, service workers) without switching browsers or reworking trust stores. Debug config sharing is cleaner with new `launch.json` top-level `"windows"`, `"linux"`, `"osx"` properties so repos can keep platform-specific entries in one file while hiding irrelevant configs on other OSes, reducing noise in the Run and Debug UI. There''s also a WSL fix for "Reveal in File Explorer," plus UX/accessibility improvements like better screen reader labels and image carousel zoom for inspecting assets.

- [''Visual Studio Code 1.113 (Insiders): update notes for March 2026''](https://code.visualstudio.com/updates/v1_113)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-03-23';

-- weekly-dotnet-roundup-2026-03-30  (2026-03-30)
UPDATE content_items
SET  content      = 'This week''s .NET updates focused on meeting developers where they are: keeping code-first workflows while widening where apps can run. It continues last week''s two-track theme: maintain a stable production baseline (PowerShell 7.6 LTS on .NET 10, smoother VS Code Insiders tooling) while trying newer .NET 11 Preview 2 features (like MAUI Maps pin clustering). This week, that split shows up as Aspire getting a supported Azure deployment path and MAUI exploring broader targets via Avalonia''s rendering stack.

<!--excerpt_end-->

## .NET

### .NET Aspire on Azure App Service (GA)

Aspire on Azure App Service reached GA, taking Aspire from local orchestration to a deployable, AppHost-defined topology on App Service without abandoning the Aspire model. Like last week''s focus on maintaining a known-good baseline, this GA gives teams already using AppHost locally a clearer production path with fewer conceptual jumps.

The core is the `Aspire.Hosting.Azure.AppService` NuGet package. You define your App Service environment in AppHost code alongside familiar local constructs: project references, health checks (for example, `WithHttpHealthCheck("/health")`), and endpoint exposure (`WithExternalHttpEndpoints()`). For teams standardizing on code-defined configuration, keeping the app''s "shape" in AppHost reduces drift between local and hosted environments.

A practical GA detail is support for Deployment Slots, mapping to typical staging/production workflows. From AppHost you can declare a slot (for example, `.WithDeploymentSlot("dev")`) with behavior to note: if production does not exist, deployment creates production and the staging slot with identical config; if production exists, deployment targets only the staging slot. Scaling is supported manually (via AppHost code or portal), and rule-based scaling is available through Azure Monitor Autoscale, but fully automatic scaling is not part of Aspire-on-App-Service yet. Overall, this is a bridge from local composition to managed App Service while keeping AppHost as the source of truth for dependencies and topology.

- [Aspire on Azure App Service is now Generally Available](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/aspire-on-azure-app-service-is-now-generally-available/ba-p/4505549)

### .NET MAUI + AvaloniaUI backend (preview) for Linux and WebAssembly

Last week''s .NET 11 Preview 2 notes highlighted MAUI control work (like Maps pin clustering). This week''s MAUI update is about platform reach: a preview AvaloniaUI "backend" for .NET MAUI adds Linux desktop and WebAssembly targets. It''s based on .NET 11 preview and is expected to remain preview until .NET 11 GA (projected around November), matching the "try it, expect churn" posture implied by last week''s preview coverage.

The approach swaps rendering. Instead of only MAUI''s native handler model, MAUI apps can render UI using Avalonia''s custom-drawn control stack, either alongside or replacing MAUI controls. The trade is clear: keep the MAUI app model while gaining Avalonia''s cross-platform reach (Windows, macOS, Linux, iOS, Android, WebAssembly) without rewriting into an Avalonia-first architecture. It also creates a continuation path: the same MAUI app can use new MAUI controls on mobile while experimenting with Linux/browser targets via the backend, though behavior/rendering may differ because the control stack changes.

The preview has constraints teams need to plan for. Microsoft MAUI API coverage is incomplete, especially around storage/media, so workarounds or waiting may be required. Linux relies on X11/XWayland rather than Wayland, which can block modern desktops. On Windows, it does not yet support hosting Avalonia controls inside WinUI (MAUI''s Windows stack), limiting hybrid composition. Engineering from this backend is also feeding into Avalonia 12 controls/APIs and efforts to reduce MAUI/Avalonia control differences. For MAUI teams that need Linux desktop or browser targets, it''s a workable option if you can tolerate preview gaps and cross-platform UI churn.

- [Avalonia adds Linux and WebAssembly targets to .NET MAUI (preview, .NET 11)](https://www.devclass.com/development/2026/03/24/avaloniaui-enhances-net-maui-with-linux-and-webassembly-support/5209515)',
     excerpt      = 'This week''s .NET updates focused on meeting developers where they are: keeping code-first workflows while widening where apps can run. It continues last week''s two-track theme: maintain a stable production baseline (PowerShell 7.6 LTS on .NET 10, smoother VS Code Insiders tooling) while trying newer .NET 11 Preview 2 features (like MAUI Maps pin clustering). This week, that split shows up as Aspire getting a supported Azure deployment path and MAUI exploring broader targets via Avalonia''s rendering stack.',
     content_hash = md5('This week''s .NET updates focused on meeting developers where they are: keeping code-first workflows while widening where apps can run. It continues last week''s two-track theme: maintain a stable production baseline (PowerShell 7.6 LTS on .NET 10, smoother VS Code Insiders tooling) while trying newer .NET 11 Preview 2 features (like MAUI Maps pin clustering). This week, that split shows up as Aspire getting a supported Azure deployment path and MAUI exploring broader targets via Avalonia''s rendering stack.

<!--excerpt_end-->

## .NET

### .NET Aspire on Azure App Service (GA)

Aspire on Azure App Service reached GA, taking Aspire from local orchestration to a deployable, AppHost-defined topology on App Service without abandoning the Aspire model. Like last week''s focus on maintaining a known-good baseline, this GA gives teams already using AppHost locally a clearer production path with fewer conceptual jumps.

The core is the `Aspire.Hosting.Azure.AppService` NuGet package. You define your App Service environment in AppHost code alongside familiar local constructs: project references, health checks (for example, `WithHttpHealthCheck("/health")`), and endpoint exposure (`WithExternalHttpEndpoints()`). For teams standardizing on code-defined configuration, keeping the app''s "shape" in AppHost reduces drift between local and hosted environments.

A practical GA detail is support for Deployment Slots, mapping to typical staging/production workflows. From AppHost you can declare a slot (for example, `.WithDeploymentSlot("dev")`) with behavior to note: if production does not exist, deployment creates production and the staging slot with identical config; if production exists, deployment targets only the staging slot. Scaling is supported manually (via AppHost code or portal), and rule-based scaling is available through Azure Monitor Autoscale, but fully automatic scaling is not part of Aspire-on-App-Service yet. Overall, this is a bridge from local composition to managed App Service while keeping AppHost as the source of truth for dependencies and topology.

- [Aspire on Azure App Service is now Generally Available](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/aspire-on-azure-app-service-is-now-generally-available/ba-p/4505549)

### .NET MAUI + AvaloniaUI backend (preview) for Linux and WebAssembly

Last week''s .NET 11 Preview 2 notes highlighted MAUI control work (like Maps pin clustering). This week''s MAUI update is about platform reach: a preview AvaloniaUI "backend" for .NET MAUI adds Linux desktop and WebAssembly targets. It''s based on .NET 11 preview and is expected to remain preview until .NET 11 GA (projected around November), matching the "try it, expect churn" posture implied by last week''s preview coverage.

The approach swaps rendering. Instead of only MAUI''s native handler model, MAUI apps can render UI using Avalonia''s custom-drawn control stack, either alongside or replacing MAUI controls. The trade is clear: keep the MAUI app model while gaining Avalonia''s cross-platform reach (Windows, macOS, Linux, iOS, Android, WebAssembly) without rewriting into an Avalonia-first architecture. It also creates a continuation path: the same MAUI app can use new MAUI controls on mobile while experimenting with Linux/browser targets via the backend, though behavior/rendering may differ because the control stack changes.

The preview has constraints teams need to plan for. Microsoft MAUI API coverage is incomplete, especially around storage/media, so workarounds or waiting may be required. Linux relies on X11/XWayland rather than Wayland, which can block modern desktops. On Windows, it does not yet support hosting Avalonia controls inside WinUI (MAUI''s Windows stack), limiting hybrid composition. Engineering from this backend is also feeding into Avalonia 12 controls/APIs and efforts to reduce MAUI/Avalonia control differences. For MAUI teams that need Linux desktop or browser targets, it''s a workable option if you can tolerate preview gaps and cross-platform UI churn.

- [Avalonia adds Linux and WebAssembly targets to .NET MAUI (preview, .NET 11)](https://www.devclass.com/development/2026/03/24/avaloniaui-enhances-net-maui-with-linux-and-webassembly-support/5209515)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-03-30';

-- weekly-dotnet-roundup-2026-04-06  (2026-04-06)
UPDATE content_items
SET  content      = 'This week’s .NET items leaned toward "what’s next," with early looks at language features and framework experiments that could change how you model APIs and configure apps. MAUI also clarified how to try new UI ideas without waiting for full releases. This split between stable baselines and previews/experiments continues from last week: alongside GA paths like Aspire on App Service, the .NET 11 Preview 2 thread keeps producing deeper language/runtime experiments, and MAUI is formalizing an "expect churn" lane through an experiments hub.

<!--excerpt_end-->

## .NET

### C# 15 union types in .NET 11 Preview

C# 15 (starting with .NET 11 Preview 2) introduces union types as a first-class way to define a closed set of value shapes without `object`, marker interfaces, or awkward base-class hierarchies. Following last week’s .NET 11 Preview 2 coverage, this is another Preview 2 feature that is likely to evolve as tooling and runtime pieces land. With `union`, you can declare `public union Pet(Cat, Dog, Bird);`, and the compiler treats the cases as complete: it supports implicit conversions from each case type (for example, `Pet pet = new Dog("Rex");`) and enforces exhaustive pattern matching in `switch` expressions. The maintenance benefit is clear: if you add a new case later, existing switches can warn when they are no longer exhaustive.

The preview includes important semantics and caveats. Patterns generally apply to the generated `Value` (auto-unwrapping), except `var` and `_`, which bind/match the union itself. Nullability matters: the default union value has `Value == null`, and the `null` pattern checks whether `Value` is null; if any cases are nullable (for example, `Bird?`), you must handle `null` explicitly for exhaustiveness. Under the hood, `union` is shorthand for a compiler-generated struct with per-case constructors and a `Value` of type `object?`, so value-type cases box by default.

For library authors and performance-sensitive code, "custom union types" are also supported. If you annotate a type with `[System.Runtime.CompilerServices.Union]` and follow the expected shape (public single-parameter constructors plus a public `Value` property), the compiler treats it as a union. Adding `HasValue` / `TryGetValue` can enable union-aware patterns that avoid boxing for value-type cases. To try it now, install the .NET 11 Preview SDK, target `net11.0`, set `<LangVersion>preview</LangVersion>`, and add runtime polyfills for `UnionAttribute` and `IUnion` (not included in .NET 11 Preview 2 yet). IDE support is expected via upcoming Visual Studio Insiders builds and is already in the latest C# Dev Kit Insiders.

- [Explore union types in C# 15](https://devblogs.microsoft.com/dotnet/csharp-15-union-types/)

### Contextual options: per-request configuration via an experimental extensions package

The options pattern got an experimental add-on: `Microsoft.Extensions.Options.Contextual`, a NuGet package that adds a contextual layer on top of `IOptions<T>`. Building on last week’s theme of code-first workflows across more environments, it keeps configuration DI-driven while letting it adapt to request/tenant/user context. Instead of global or named options, you resolve `IContextualOptions<TOptions, TContext>` and call `GetAsync(context)` to compute options for a specific context. The walkthrough uses an ASP.NET Core "weather forecast" app with an `AppContext` (annotated `[OptionsContext]` and `partial`) carrying fields like `UserId` and `Country`, then derives defaults from context at the call site.

Mechanically, there are three parts: a source generator (`ContextualOptionsGenerator`) emits an `IOptionsContext` implementation; you implement an `IOptionsContextReceiver` that consumes key/value pairs via `Receive<T>(string key, T value)`; and you register an additional contextual configure callback `(IOptionsContext ctx, TOptions opts)` to apply derived values. The post calls out a maintainability risk: receivers are coupled to context properties via string keys (property names), so renames can silently change behavior. There is also the cost of adopting an `[Experimental]` API: the package triggers `EXTEXP0018` unless you opt in, and generated code is also experimental, so teams often suppress warnings broadly (for example, `<NoWarn>$(NoWarn);EXTEXP0018</NoWarn>`). The conclusion is to evaluate it mainly if you already rely on `IOptions` and need true per-context config that named options cannot express; otherwise feature flag tooling (`Microsoft.FeatureManagement`, OpenFeature) may fit better.

- [Configuring contextual options with Microsoft.Extensions.Options.Contextual](https://andrewlock.net/configuring-contextual-options-with-microsoft-extensions-options-contextual/)

### .NET MAUI’s new home for experiments: maui-labs

The .NET MAUI Community Standup introduced maui-labs as the official home for experimental and community-driven MAUI work, including prototypes and in-progress ideas not ready for stable MAUI. This fits the pattern we have been tracking: last week’s MAUI + Avalonia backend preview (Linux and WebAssembly) showed active experimentation around rendering and reach, and maui-labs clarifies where that work should live so teams can try it without confusing it with supported MAUI. The practical benefit is clearer boundaries: developers get one place to follow and test early work on expanded platform support, alternate rendering options, and exploratory features, with a clearer path for what might later graduate into the product.

- [''.NET MAUI Community Standup: Introducing maui-labs''](https://www.youtube.com/watch?v=IfCIubKbyqw)',
     excerpt      = 'This week’s .NET items leaned toward "what’s next," with early looks at language features and framework experiments that could change how you model APIs and configure apps. MAUI also clarified how to try new UI ideas without waiting for full releases. This split between stable baselines and previews/experiments continues from last week: alongside GA paths like Aspire on App Service, the .NET 11 Preview 2 thread keeps producing deeper language/runtime experiments, and MAUI is formalizing an "expect churn" lane through an experiments hub.',
     content_hash = md5('This week’s .NET items leaned toward "what’s next," with early looks at language features and framework experiments that could change how you model APIs and configure apps. MAUI also clarified how to try new UI ideas without waiting for full releases. This split between stable baselines and previews/experiments continues from last week: alongside GA paths like Aspire on App Service, the .NET 11 Preview 2 thread keeps producing deeper language/runtime experiments, and MAUI is formalizing an "expect churn" lane through an experiments hub.

<!--excerpt_end-->

## .NET

### C# 15 union types in .NET 11 Preview

C# 15 (starting with .NET 11 Preview 2) introduces union types as a first-class way to define a closed set of value shapes without `object`, marker interfaces, or awkward base-class hierarchies. Following last week’s .NET 11 Preview 2 coverage, this is another Preview 2 feature that is likely to evolve as tooling and runtime pieces land. With `union`, you can declare `public union Pet(Cat, Dog, Bird);`, and the compiler treats the cases as complete: it supports implicit conversions from each case type (for example, `Pet pet = new Dog("Rex");`) and enforces exhaustive pattern matching in `switch` expressions. The maintenance benefit is clear: if you add a new case later, existing switches can warn when they are no longer exhaustive.

The preview includes important semantics and caveats. Patterns generally apply to the generated `Value` (auto-unwrapping), except `var` and `_`, which bind/match the union itself. Nullability matters: the default union value has `Value == null`, and the `null` pattern checks whether `Value` is null; if any cases are nullable (for example, `Bird?`), you must handle `null` explicitly for exhaustiveness. Under the hood, `union` is shorthand for a compiler-generated struct with per-case constructors and a `Value` of type `object?`, so value-type cases box by default.

For library authors and performance-sensitive code, "custom union types" are also supported. If you annotate a type with `[System.Runtime.CompilerServices.Union]` and follow the expected shape (public single-parameter constructors plus a public `Value` property), the compiler treats it as a union. Adding `HasValue` / `TryGetValue` can enable union-aware patterns that avoid boxing for value-type cases. To try it now, install the .NET 11 Preview SDK, target `net11.0`, set `<LangVersion>preview</LangVersion>`, and add runtime polyfills for `UnionAttribute` and `IUnion` (not included in .NET 11 Preview 2 yet). IDE support is expected via upcoming Visual Studio Insiders builds and is already in the latest C# Dev Kit Insiders.

- [Explore union types in C# 15](https://devblogs.microsoft.com/dotnet/csharp-15-union-types/)

### Contextual options: per-request configuration via an experimental extensions package

The options pattern got an experimental add-on: `Microsoft.Extensions.Options.Contextual`, a NuGet package that adds a contextual layer on top of `IOptions<T>`. Building on last week’s theme of code-first workflows across more environments, it keeps configuration DI-driven while letting it adapt to request/tenant/user context. Instead of global or named options, you resolve `IContextualOptions<TOptions, TContext>` and call `GetAsync(context)` to compute options for a specific context. The walkthrough uses an ASP.NET Core "weather forecast" app with an `AppContext` (annotated `[OptionsContext]` and `partial`) carrying fields like `UserId` and `Country`, then derives defaults from context at the call site.

Mechanically, there are three parts: a source generator (`ContextualOptionsGenerator`) emits an `IOptionsContext` implementation; you implement an `IOptionsContextReceiver` that consumes key/value pairs via `Receive<T>(string key, T value)`; and you register an additional contextual configure callback `(IOptionsContext ctx, TOptions opts)` to apply derived values. The post calls out a maintainability risk: receivers are coupled to context properties via string keys (property names), so renames can silently change behavior. There is also the cost of adopting an `[Experimental]` API: the package triggers `EXTEXP0018` unless you opt in, and generated code is also experimental, so teams often suppress warnings broadly (for example, `<NoWarn>$(NoWarn);EXTEXP0018</NoWarn>`). The conclusion is to evaluate it mainly if you already rely on `IOptions` and need true per-context config that named options cannot express; otherwise feature flag tooling (`Microsoft.FeatureManagement`, OpenFeature) may fit better.

- [Configuring contextual options with Microsoft.Extensions.Options.Contextual](https://andrewlock.net/configuring-contextual-options-with-microsoft-extensions-options-contextual/)

### .NET MAUI’s new home for experiments: maui-labs

The .NET MAUI Community Standup introduced maui-labs as the official home for experimental and community-driven MAUI work, including prototypes and in-progress ideas not ready for stable MAUI. This fits the pattern we have been tracking: last week’s MAUI + Avalonia backend preview (Linux and WebAssembly) showed active experimentation around rendering and reach, and maui-labs clarifies where that work should live so teams can try it without confusing it with supported MAUI. The practical benefit is clearer boundaries: developers get one place to follow and test early work on expanded platform support, alternate rendering options, and exploratory features, with a clearer path for what might later graduate into the product.

- [''.NET MAUI Community Standup: Introducing maui-labs''](https://www.youtube.com/watch?v=IfCIubKbyqw)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-04-06';

-- weekly-dotnet-roundup-2026-04-13  (2026-04-13)
UPDATE content_items
SET  content      = 'This week''s .NET updates focused on practical changes: a new way to run Spark SQL from ADO.NET code, early direction on Blazor validation in .NET 11, and a Windows packaging change for PowerShell that will affect machines and build agents. Compared to last week''s "what''s next" previews, this week is more "here is what you can trial now," plus a policy shift that can impact pipelines.

<!--excerpt_end-->

## .NET

### .NET data access to Microsoft Fabric Spark (ADO.NET preview driver)

Microsoft released a preview ADO.NET provider for Microsoft Fabric Data Engineering, letting .NET apps connect to Fabric and run Spark SQL using standard ADO.NET patterns instead of custom HTTP calls to Livy. It uses Fabric''s Livy APIs but exposes familiar abstractions (`DbConnection`, `DbCommand`, `DbDataReader`, `DbParameter`, `DbProviderFactory`) so existing ADO.NET-shaped codebases can adapt with less refactoring. It supports typical command/reader flows and parameterized queries, which makes it easier to integrate Spark SQL execution into existing repository layers or ETL/ELT services without maintaining a separate client stack.

The driver supports Microsoft Entra ID across common auth flows: Azure CLI auth, interactive browser login, client credentials, certificate auth, and direct access-token usage. It also targets efficiency and resilience with connection pooling and Spark session reuse, async prefetch for large result sets, and auto-reconnect to recover sessions after failures. It claims broad Spark SQL type coverage, including `ARRAY`, `MAP`, and `STRUCT`, which matters for lakehouse-shaped data in .NET pipelines. Overall, it gives .NET teams a more standard way to query and manage Fabric Spark from services and tools, aligning with the approach of fitting existing .NET idioms rather than requiring a new programming model.

- [Microsoft ADO.NET Driver for Microsoft Fabric Data Engineering (Preview)](https://blog.fabric.microsoft.com/en-US/blog/microsoft-ado-net-driver-for-microsoft-fabric-data-engineering-preview/)

### Blazor in .NET 11: validation model previews (async rules, localization, render-mode flexibility)

In the Blazor Community Standup, the team previewed .NET 11 validation improvements intended to reduce custom plumbing around `EditForm`. Like last week''s .NET 11 Preview 2 direction-setting items, this is another look at where core workflows are headed so teams can rely less on homegrown patterns.

A key focus is built-in asynchronous validation so forms can validate against async-backed rules (username availability, DB checks, external lookups) without pushing developers into custom "validate on submit" flows outside Blazor''s normal validation model.

They also previewed localized validation messages to reduce manual resource mapping and custom error pipelines in multi-language apps. Finally, they discussed enabling client-side validation without requiring interactive render mode, which helps when teams mix render modes and do not want interactivity only to get validation feedback. Together, the direction is a more flexible validation stack across async rules, localized UX, and modern render-mode composition. It mirrors last week''s theme of "stable defaults with a lane for evolution," applied to everyday app UX.

- [''Blazor Community Standup: Blazor validation in .NET 11''](https://www.youtube.com/watch?v=X-qAr4uqAzc)

### Other .NET News

PowerShell packaging on Windows is changing starting with PowerShell 7.7-preview.1 (April 2026): MSIX becomes the primary installer, and new releases will no longer ship an MSI (MSI remains for existing releases like 7.6). Teams that automate PowerShell rollout across dev machines, CI runners, and managed endpoints will need to shift MSI-based deployment/upgrade pipelines to MSIX tooling for future versions. Microsoft cites a more predictable servicing model (updates, differential updates) and accessibility requirements, while noting gaps still being addressed, especially around remoting and system-level execution (Task Scheduler, services). Practically, it is an early warning to review and update rollout pipelines.

- [PowerShell MSI package deprecation and preview updates](https://devblogs.microsoft.com/powershell/powershell-msi-deprecation/)

Visual Studio shared a workflow tip for multi-monitor users: a setting controls whether floating windows are "owned" by the main IDE window (Tools > Options > Environment > Windows > Floating Windows). Setting ownership to "None" can make floating windows behave more like top-level windows (separate taskbar buttons, fewer always-on-top quirks, and staying visible when the IDE is minimized), which pairs well with PowerToys FancyZones. It also notes other modes ("Tool Windows" and "Documents and Tool Windows") and a shortcut: Ctrl + double-click a tool window title bar to toggle dock/float.

- [Take full control of your floating windows in Visual Studio](https://devblogs.microsoft.com/visualstudio/take-full-control-of-your-floating-windows-in-visual-studio/)',
     excerpt      = 'This week''s .NET updates focused on practical changes: a new way to run Spark SQL from ADO.NET code, early direction on Blazor validation in .NET 11, and a Windows packaging change for PowerShell that will affect machines and build agents. Compared to last week''s "what''s next" previews, this week is more "here is what you can trial now," plus a policy shift that can impact pipelines.',
     content_hash = md5('This week''s .NET updates focused on practical changes: a new way to run Spark SQL from ADO.NET code, early direction on Blazor validation in .NET 11, and a Windows packaging change for PowerShell that will affect machines and build agents. Compared to last week''s "what''s next" previews, this week is more "here is what you can trial now," plus a policy shift that can impact pipelines.

<!--excerpt_end-->

## .NET

### .NET data access to Microsoft Fabric Spark (ADO.NET preview driver)

Microsoft released a preview ADO.NET provider for Microsoft Fabric Data Engineering, letting .NET apps connect to Fabric and run Spark SQL using standard ADO.NET patterns instead of custom HTTP calls to Livy. It uses Fabric''s Livy APIs but exposes familiar abstractions (`DbConnection`, `DbCommand`, `DbDataReader`, `DbParameter`, `DbProviderFactory`) so existing ADO.NET-shaped codebases can adapt with less refactoring. It supports typical command/reader flows and parameterized queries, which makes it easier to integrate Spark SQL execution into existing repository layers or ETL/ELT services without maintaining a separate client stack.

The driver supports Microsoft Entra ID across common auth flows: Azure CLI auth, interactive browser login, client credentials, certificate auth, and direct access-token usage. It also targets efficiency and resilience with connection pooling and Spark session reuse, async prefetch for large result sets, and auto-reconnect to recover sessions after failures. It claims broad Spark SQL type coverage, including `ARRAY`, `MAP`, and `STRUCT`, which matters for lakehouse-shaped data in .NET pipelines. Overall, it gives .NET teams a more standard way to query and manage Fabric Spark from services and tools, aligning with the approach of fitting existing .NET idioms rather than requiring a new programming model.

- [Microsoft ADO.NET Driver for Microsoft Fabric Data Engineering (Preview)](https://blog.fabric.microsoft.com/en-US/blog/microsoft-ado-net-driver-for-microsoft-fabric-data-engineering-preview/)

### Blazor in .NET 11: validation model previews (async rules, localization, render-mode flexibility)

In the Blazor Community Standup, the team previewed .NET 11 validation improvements intended to reduce custom plumbing around `EditForm`. Like last week''s .NET 11 Preview 2 direction-setting items, this is another look at where core workflows are headed so teams can rely less on homegrown patterns.

A key focus is built-in asynchronous validation so forms can validate against async-backed rules (username availability, DB checks, external lookups) without pushing developers into custom "validate on submit" flows outside Blazor''s normal validation model.

They also previewed localized validation messages to reduce manual resource mapping and custom error pipelines in multi-language apps. Finally, they discussed enabling client-side validation without requiring interactive render mode, which helps when teams mix render modes and do not want interactivity only to get validation feedback. Together, the direction is a more flexible validation stack across async rules, localized UX, and modern render-mode composition. It mirrors last week''s theme of "stable defaults with a lane for evolution," applied to everyday app UX.

- [''Blazor Community Standup: Blazor validation in .NET 11''](https://www.youtube.com/watch?v=X-qAr4uqAzc)

### Other .NET News

PowerShell packaging on Windows is changing starting with PowerShell 7.7-preview.1 (April 2026): MSIX becomes the primary installer, and new releases will no longer ship an MSI (MSI remains for existing releases like 7.6). Teams that automate PowerShell rollout across dev machines, CI runners, and managed endpoints will need to shift MSI-based deployment/upgrade pipelines to MSIX tooling for future versions. Microsoft cites a more predictable servicing model (updates, differential updates) and accessibility requirements, while noting gaps still being addressed, especially around remoting and system-level execution (Task Scheduler, services). Practically, it is an early warning to review and update rollout pipelines.

- [PowerShell MSI package deprecation and preview updates](https://devblogs.microsoft.com/powershell/powershell-msi-deprecation/)

Visual Studio shared a workflow tip for multi-monitor users: a setting controls whether floating windows are "owned" by the main IDE window (Tools > Options > Environment > Windows > Floating Windows). Setting ownership to "None" can make floating windows behave more like top-level windows (separate taskbar buttons, fewer always-on-top quirks, and staying visible when the IDE is minimized), which pairs well with PowerToys FancyZones. It also notes other modes ("Tool Windows" and "Documents and Tool Windows") and a shortcut: Ctrl + double-click a tool window title bar to toggle dock/float.

- [Take full control of your floating windows in Visual Studio](https://devblogs.microsoft.com/visualstudio/take-full-control-of-your-floating-windows-in-visual-studio/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-04-13';

-- weekly-dotnet-roundup-2026-04-20  (2026-04-20)
UPDATE content_items
SET  content      = 'This week''s .NET updates split between moving forward and staying current. .NET 11 Preview 3 shipped runtime/SDK/library/framework updates aimed at everyday development, while April 2026 servicing releases delivered security fixes across supported .NET and .NET Framework versions. Building on last week''s .NET 11 direction-setting items (like Blazor validation previews), this is another preview step you can install and test, alongside reminders to keep production stacks patched. Microsoft also set a deadline for an "ASP.NET Core on .NET Framework" escape hatch, pushing teams toward modern .NET for web workloads.

<!--excerpt_end-->

## .NET

### .NET 11 Preview 3: runtime, SDK/CLI, web/WASM, MAUI, EF Core, and containers

.NET 11 Preview 3 updates the runtime, SDK/CLI, BCL, C#, ASP.NET Core, .NET MAUI, EF Core, and official container images, with emphasis on performance and faster inner-loop iteration. Following last week''s focus on evolving app workflows (for example, Blazor validation), Preview 3 spreads incremental improvements across the stack.

Runtime/JIT changes include optimizations for switch statements, bounds checks, and casts to reduce hot-path overhead without code changes. It also makes "runtime async" available without preview-API opt-in, which reduces friction for teams evaluating async runtime capabilities during previews.

BCL updates mix control and safety. System.Text.Json adds controls for naming and ignore-default handling to reduce the need for custom converters. Compression expands: Zstandard (zstd) support moves into System.IO.Compression, and ZIP reading adds CRC32 validation to surface corrupt archives earlier. Lower-level IO improvements expand pipe support via SafeFileHandle and RandomAccess for infra and interop-heavy code. One behavior change is that Regex now recognizes all Unicode newline sequences, which can change matches on inputs that contain non-ASCII newlines (relevant for cross-platform parsing).

SDK/CLI improvements target large repos and fast iteration. You can edit solution filters from the CLI, which helps in monorepos and focused builds. File-based apps can span multiple files, making script-like prototypes easier to organize. `dotnet run` adds `-e` for setting environment variables directly. `dotnet watch` adds Aspire support, crash recovery, and Windows desktop improvements, aiming for more resilient hot reload and watch workflows across cloud-native and desktop apps. This pairs with last week''s pipeline and machine operational notes: teams will notice these changes in local iteration and automation.

For web and browser, ASP.NET Core adds Zstandard response compression and request decompression as an alternative to gzip/brotli. Blazor `Virtualize` can adapt to variable-height items at runtime, reducing jank when item sizes vary and continuing last week''s thread of making common UI patterns require less custom code. HTTP/3 processing starts earlier in the pipeline to reduce latency and overhead. Browser/WASM updates add WebCIL support and debugging improvements, affecting packaging and developer experience for Blazor WebAssembly and other .NET-in-browser workloads.

C# previews experimental union types support for discriminated-union-style modeling. .NET MAUI updates include Maps improvements (clustering, styling, richer APIs), XAML/styling tweaks for startup and iteration, and a built-in `LongPressGestureRecognizer`. .NET for Android adds Android 17 / API 37 preview support to validate upcoming platform changes.

EF Core adds performance and configuration controls. `ChangeTracker.GetEntriesForState()` avoids extra change detection when inspecting tracked state. DbContext config can remove providers and add pooled factories for more flexible DI/provider/pooling combinations. Migrations get more control and clearer feedback, and query generation removes unnecessary joins in some cases. SQL Server provider support adds JSON APIs for teams using JSON-centric schema patterns.

Supply-chain hardening shows up in containers: official .NET container images are now signed, improving provenance for CI/CD policies that require signatures. Read alongside last week''s PowerShell installer transition warning, it is another reminder that build and deploy inputs (base images, installers, agent tooling) matter as much as application code. Preview 3 guidance points to installing the .NET 11 SDK Preview 3 and using Visual Studio 2026 Insiders on Windows, or VS Code with C# Dev Kit.

- [.NET 11 Preview 3 is now available!](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-3/)

### Servicing and support timelines: April 2026 patches and a deadline for ASP.NET Core 2.3 on .NET Framework

Microsoft shipped April 14, 2026 servicing updates for supported .NET and .NET Framework versions, covering security and non-security fixes with links to release notes, installers/binaries, MCR container images, Linux package guidance, and known issues. After last week''s mix of preview features and pipeline-affecting policy shifts, this reinforces the "do not fall behind" track: servicing updates are the immediate production task, and previews are for what comes next.

Patched .NET releases are .NET 10.0.6, .NET 9.0.15, and .NET 8.0.26, with pointers to GitHub release notes and milestone/changelog queries for ASP.NET Core, EF Core, the runtime, and WinForms. For teams running multiple app types, those links help you audit changes in the parts you ship.

Security-wise, the post lists multiple CVEs across .NET and .NET Framework, including denial of service, security feature bypass, and remote code execution (affecting combinations of .NET 10/9/8 and multiple .NET Framework versions). Practically, patching means more than updating dev SDKs: rebuild and redeploy containers on updated MCR base images, update build agents, and pull forward Linux package installs, while checking known issues before broad rollout.

Microsoft also set an end-of-support date for ASP.NET Core 2.3 on .NET Framework: April 7, 2027. After that, it gets no security patches, bug fixes, or support, creating a deadline for orgs that adopted ASP.NET Core but stayed on .NET Framework. The recommended path is modern ASP.NET on .NET 10, aligning with ongoing runtime and framework improvements and enabling cross-platform hosting. Context matters: ASP.NET Core 3.0 dropped .NET Framework support in 2019, and ASP.NET Core 2.3 (early 2025) was a servicing-oriented baseline for Framework users, effectively a re-release of 2.1. In some cases, 2.1 -> 2.3 behaved more like a compatibility tradeoff than a simple bump. With install stats showing these packages still widely used, the EoS date is likely to surface in backlogs soon, especially for stable Windows/IIS apps on long timelines.

- [.NET and .NET Framework April 2026 servicing releases updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-april-2026-servicing-updates/)
- [Microsoft calls time on ASP.NET Core 2.3 on .NET Framework](https://www.devclass.com/devops/2026/04/13/microsoft-calls-time-on-aspnet-core-23-on-net-framework/5216962)',
     excerpt      = 'This week''s .NET updates split between moving forward and staying current. .NET 11 Preview 3 shipped runtime/SDK/library/framework updates aimed at everyday development, while April 2026 servicing releases delivered security fixes across supported .NET and .NET Framework versions. Building on last week''s .NET 11 direction-setting items (like Blazor validation previews), this is another preview step you can install and test, alongside reminders to keep production stacks patched. Microsoft also set a deadline for an "ASP.NET Core on .NET Framework" escape hatch, pushing teams toward modern .NET for web workloads.',
     content_hash = md5('This week''s .NET updates split between moving forward and staying current. .NET 11 Preview 3 shipped runtime/SDK/library/framework updates aimed at everyday development, while April 2026 servicing releases delivered security fixes across supported .NET and .NET Framework versions. Building on last week''s .NET 11 direction-setting items (like Blazor validation previews), this is another preview step you can install and test, alongside reminders to keep production stacks patched. Microsoft also set a deadline for an "ASP.NET Core on .NET Framework" escape hatch, pushing teams toward modern .NET for web workloads.

<!--excerpt_end-->

## .NET

### .NET 11 Preview 3: runtime, SDK/CLI, web/WASM, MAUI, EF Core, and containers

.NET 11 Preview 3 updates the runtime, SDK/CLI, BCL, C#, ASP.NET Core, .NET MAUI, EF Core, and official container images, with emphasis on performance and faster inner-loop iteration. Following last week''s focus on evolving app workflows (for example, Blazor validation), Preview 3 spreads incremental improvements across the stack.

Runtime/JIT changes include optimizations for switch statements, bounds checks, and casts to reduce hot-path overhead without code changes. It also makes "runtime async" available without preview-API opt-in, which reduces friction for teams evaluating async runtime capabilities during previews.

BCL updates mix control and safety. System.Text.Json adds controls for naming and ignore-default handling to reduce the need for custom converters. Compression expands: Zstandard (zstd) support moves into System.IO.Compression, and ZIP reading adds CRC32 validation to surface corrupt archives earlier. Lower-level IO improvements expand pipe support via SafeFileHandle and RandomAccess for infra and interop-heavy code. One behavior change is that Regex now recognizes all Unicode newline sequences, which can change matches on inputs that contain non-ASCII newlines (relevant for cross-platform parsing).

SDK/CLI improvements target large repos and fast iteration. You can edit solution filters from the CLI, which helps in monorepos and focused builds. File-based apps can span multiple files, making script-like prototypes easier to organize. `dotnet run` adds `-e` for setting environment variables directly. `dotnet watch` adds Aspire support, crash recovery, and Windows desktop improvements, aiming for more resilient hot reload and watch workflows across cloud-native and desktop apps. This pairs with last week''s pipeline and machine operational notes: teams will notice these changes in local iteration and automation.

For web and browser, ASP.NET Core adds Zstandard response compression and request decompression as an alternative to gzip/brotli. Blazor `Virtualize` can adapt to variable-height items at runtime, reducing jank when item sizes vary and continuing last week''s thread of making common UI patterns require less custom code. HTTP/3 processing starts earlier in the pipeline to reduce latency and overhead. Browser/WASM updates add WebCIL support and debugging improvements, affecting packaging and developer experience for Blazor WebAssembly and other .NET-in-browser workloads.

C# previews experimental union types support for discriminated-union-style modeling. .NET MAUI updates include Maps improvements (clustering, styling, richer APIs), XAML/styling tweaks for startup and iteration, and a built-in `LongPressGestureRecognizer`. .NET for Android adds Android 17 / API 37 preview support to validate upcoming platform changes.

EF Core adds performance and configuration controls. `ChangeTracker.GetEntriesForState()` avoids extra change detection when inspecting tracked state. DbContext config can remove providers and add pooled factories for more flexible DI/provider/pooling combinations. Migrations get more control and clearer feedback, and query generation removes unnecessary joins in some cases. SQL Server provider support adds JSON APIs for teams using JSON-centric schema patterns.

Supply-chain hardening shows up in containers: official .NET container images are now signed, improving provenance for CI/CD policies that require signatures. Read alongside last week''s PowerShell installer transition warning, it is another reminder that build and deploy inputs (base images, installers, agent tooling) matter as much as application code. Preview 3 guidance points to installing the .NET 11 SDK Preview 3 and using Visual Studio 2026 Insiders on Windows, or VS Code with C# Dev Kit.

- [.NET 11 Preview 3 is now available!](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-3/)

### Servicing and support timelines: April 2026 patches and a deadline for ASP.NET Core 2.3 on .NET Framework

Microsoft shipped April 14, 2026 servicing updates for supported .NET and .NET Framework versions, covering security and non-security fixes with links to release notes, installers/binaries, MCR container images, Linux package guidance, and known issues. After last week''s mix of preview features and pipeline-affecting policy shifts, this reinforces the "do not fall behind" track: servicing updates are the immediate production task, and previews are for what comes next.

Patched .NET releases are .NET 10.0.6, .NET 9.0.15, and .NET 8.0.26, with pointers to GitHub release notes and milestone/changelog queries for ASP.NET Core, EF Core, the runtime, and WinForms. For teams running multiple app types, those links help you audit changes in the parts you ship.

Security-wise, the post lists multiple CVEs across .NET and .NET Framework, including denial of service, security feature bypass, and remote code execution (affecting combinations of .NET 10/9/8 and multiple .NET Framework versions). Practically, patching means more than updating dev SDKs: rebuild and redeploy containers on updated MCR base images, update build agents, and pull forward Linux package installs, while checking known issues before broad rollout.

Microsoft also set an end-of-support date for ASP.NET Core 2.3 on .NET Framework: April 7, 2027. After that, it gets no security patches, bug fixes, or support, creating a deadline for orgs that adopted ASP.NET Core but stayed on .NET Framework. The recommended path is modern ASP.NET on .NET 10, aligning with ongoing runtime and framework improvements and enabling cross-platform hosting. Context matters: ASP.NET Core 3.0 dropped .NET Framework support in 2019, and ASP.NET Core 2.3 (early 2025) was a servicing-oriented baseline for Framework users, effectively a re-release of 2.1. In some cases, 2.1 -> 2.3 behaved more like a compatibility tradeoff than a simple bump. With install stats showing these packages still widely used, the EoS date is likely to surface in backlogs soon, especially for stable Windows/IIS apps on long timelines.

- [.NET and .NET Framework April 2026 servicing releases updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-april-2026-servicing-updates/)
- [Microsoft calls time on ASP.NET Core 2.3 on .NET Framework](https://www.devclass.com/devops/2026/04/13/microsoft-calls-time-on-aspnet-core-23-on-net-framework/5216962)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'dotnet'
  AND slug                 = 'weekly-dotnet-roundup-2026-04-20';

-- SKIP 2026-04-27 : no '.NET/Coding' block found in source (weekly-ai-roundup-2026-04-27)

-- SKIP 2026-05-04 : no '.NET/Coding' block found in source (weekly-ai-roundup-2026-05-04)

-- SKIP 2026-05-11 : no '.NET/Coding' block found in source (weekly-ai-roundup-2026-05-11)

