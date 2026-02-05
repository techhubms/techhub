# Documentation Index

> **Auto-generated file**. Do not edit manually. Run `scripts/Generate-DocumentationIndex.ps1` to update.

This index maps the structure of all documentation files in the project.

## File: [AGENTS.md](../AGENTS.md)

- AI Assistant Workflow
  - What is This File?
  - The 10-Step Workflow
    - Step 1: Core Rules & Boundaries
    - Step 2: Gather Context
    - Step 3: Create a Plan
    - Step 4: Research & Validate
    - Step 5: Verify Current Behavior (Optional)
    - Step 6: Write Tests First (TDD)
    - Step 7: Implement Changes
    - Step 8: Validate & Fix
    - Step 9: Update Documentation
    - Step 10: Report Completion

## File: [README.md](../README.md)

- Tech Hub
  - Quick Start
  - Project Documentation

## File: [docs/AGENTS.md](AGENTS.md)

- Documentation Management Guide
  - Documentation Hierarchy
  - Core Philosophy
  - Critical Rules
    - ✅ Always Do
    - ⚠️ Ask First
    - 🚫 Never Do
  - Content Placement Decision
  - Documentation Standards
  - AGENTS.md Best Practices

## File: [docs/architecture.md](architecture.md)

- Architecture
  - .NET Aspire
    - Capabilities
    - Port Configuration
    - Service Model
    - Shared Configuration
  - Running with Aspire

## File: [docs/caching.md](caching.md)

- Browser Caching
  - Cache Strategy Overview
  - Fingerprinting
    - How Fingerprinting Works
    - Fingerprinted Asset Detection
  - Cache Headers Explained
    - `max-age`
    - `immutable`
    - `must-revalidate`
  - Static Files Middleware
    - Implementation Architecture
    - File Type Detection
  - Performance Benefits
  - Image Multi-Format Support
    - Format Selection
    - Compression Benefits
  - Image Path Convention
  - Implementation Reference

## File: [docs/content-api.md](content-api.md)

- Content API
  - Data Model
    - Section
    - Content Item (ContentItem)
    - Linking Strategy (Internal vs. External)
    - ContentItem Navigation Methods
  - Endpoints
    - Sections
    - Content Items
    - Content Detail
    - Tag Cloud
    - Error Responses
    - 404 Not Found
  - Performance Characteristics
  - Testing

## File: [docs/content-processing.md](content-processing.md)

- Content Processing
  - Content Creation Methods
    - Database Synchronization
    - Manual Content Creation with GitHub Copilot
    - Automated RSS Content Creation
  - Publishing Content
  - Troubleshooting
    - Common Issues
    - Repair Tools
  - RSS Feed Processing
    - Feed Configuration
    - Adding New Feeds
    - Processing Pipeline
    - Azure AI Foundry Integration
    - Branch Strategy
    - AI Content Analysis
    - Content Output
    - Processing Scripts
    - Error Handling
    - Automatic Deployment

## File: [docs/custom-pages.md](custom-pages.md)

- Custom Pages API
  - Endpoints
    - Developer Experience Space
    - GitHub Copilot Handbook
    - Levels of Enlightenment
    - GitHub Copilot Features
    - GenAI Basics
    - GenAI Advanced
    - GenAI Applied
    - SDLC (Software Development Life Cycle)
  - Response Format
  - Implementation
  - Content Sources for Custom Pages
    - GitHub Copilot Features Content
    - Visual Studio Code Updates
  - Custom Page Ordering
    - Configuration
    - Ordering Rules
    - Homepage Badge Display

## File: [docs/database.md](database.md)

- Database Configuration
  - Supported Providers
    - Option 1: FileSystem (No Database)
    - Option 2: SQLite (Recommended for Local Development)
    - Option 3: PostgreSQL (Production + E2E Tests)
  - Content Sync Configuration
  - Database Schema
    - Main Tables
    - Design Decisions
    - Query Patterns
    - Schema Location

## File: [docs/design-system.md](design-system.md)

- Design System
  - Design Tokens - Single Source of Truth
    - Core Principle
    - Adding New Design Tokens
    - Design Token Categories
  - Color Palette
    - Purple Accents (Tech Hub Brand)
    - Background Colors
    - Text Colors
  - Typography
    - Font Stack
    - Font Sizes
  - Spacing
  - Breakpoints
  - CSS Architecture
    - Global CSS (wwwroot/css/)
    - Component-Scoped CSS (.razor.css files)
    - When to Use Global vs Component-Scoped CSS
    - CSS Bundle Configuration
  - Hover Effects
    - Allowed in Hover Effects
    - Forbidden in Hover Effects
  - Image Handling
    - Section Background Images
    - File Structure
    - Adding a New Section
  - Implementation Reference

## File: [docs/filtering.md](filtering.md)

- Content Filtering System
  - Tag Storage and Expansion
    - How Tags Are Stored
    - Why Tag Expansion?
  - Tag Filtering Behavior
    - Single Tag Search
    - Multiple Tag Search (AND Logic)
    - Examples
  - Implementation Details
    - Query Structure
    - Duplicate Prevention
  - Content Filtering API
    - GET /api/sections/{sectionName}/collections/{collectionName}/items
  - Tag Statistics API
    - GET /api/sections/{sectionName}/collections/{collectionName}/tags
    - Tag Size Algorithm (Quantile-Based)
    - Section/Collection Title Exclusion
  - Tag Cloud UI Behavior
    - Visual Active State
    - Toggle Behavior
    - URL State Management
    - Page Integration
  - Testing
  - Future Enhancements (Not Currently Implemented)

## File: [docs/frontmatter.md](frontmatter.md)

- Frontmatter Schema
  - File Structure
  - Required Fields (All Content)
  - Field Definitions
    - Layout
    - Title
    - Author
    - Date
    - Permalink
    - Tags
    - Section Names
  - Content Source Fields
  - Collection-Specific Fields
    - GitHub Copilot Features (`_videos/ghc-features/`)
  - Frontmatter to Domain Model Mapping
  - Example Files
    - News Article
    - Standard Video
    - GitHub Copilot Feature
  - Excerpt Section
    - Definition
    - Purpose
    - Requirements
    - Example
    - Processing
  - Example: Roundup
  - Deprecated Fields (Do Not Use)
  - File Naming Convention
  - Implementation Reference

## File: [docs/health-checks.md](health-checks.md)

- Health Checks
  - Endpoints
    - GET /health
    - GET /alive
  - Implementation Details

## File: [docs/javascript.md](javascript.md)

- JavaScript Architecture
  - When JavaScript Is Required
  - Loading Strategies
  - Fingerprinting (Cache Busting)
    - Static Scripts
    - Dynamic Imports
    - How ImportMap Works
  - Local JavaScript Files
  - Conditional Loading
    - Element Detection
    - Performance Benefits
  - External CDN Libraries
    - Current Libraries
    - Updating CDN Library Versions
  - Navigation Helpers
  - TOC Scroll-Spy
  - Adding New JavaScript Files
  - Implementation Reference

## File: [docs/page-structure.md](page-structure.md)

- Page Structure
  - Semantic HTML Structure
    - Required Structure by Page Type
  - Semantic Element Usage
    - Element Guidelines
    - Choosing Between `<section>` and `<article>`
    - Common Mistakes to Avoid
  - Page Layout Classes
  - Sticky Header Architecture
    - The Problem
    - The Solution
    - How It Works
    - Benefits
  - Sidebar Component Architecture
    - Responsibility Pattern
    - Example Page with Sidebar
    - Available Sidebar Components
    - Sidebar Component Semantic HTML
  - Skeleton Loading States
    - When to Use Skeletons
    - Why Skeletons Matter
    - Skeleton Layout Architecture
  - Mobile Navigation
    - Responsive Behavior
    - Hamburger Menu Pattern
    - Mobile CSS
  - Infinite Scroll Pagination
    - Configuration
    - Pattern
    - JavaScript
  - Implementation Reference

## File: [docs/render-modes.md](render-modes.md)

- Render Mode Strategy
  - Background
  - When to Use Each Mode
    - Static SSR (No Render Mode Attribute)
    - Interactive Server with Prerender
    - Interactive Server without Prerender (Rare)
  - JavaScript Interop Disposal
  - SignalR Message Size Considerations
  - Current Component Configuration
  - Testing Interactive Components
  - Decision Flowchart
  - Best Practices
  - Implementation Reference

## File: [docs/repository-structure.md](repository-structure.md)

- Repository Structure
  - Source Code (`src/`)
  - Content (`collections/`)
  - Tests (`tests/`)
  - Configuration & Documentation

## File: [docs/rss-feeds.md](rss-feeds.md)

- RSS Feed System
  - Available RSS Feeds
    - Everything Feed
    - Roundups Feed
    - Section Feeds
  - Feed Format
    - Feed Metadata
    - Feed Items
    - Content Rendering
  - Feed Behavior
    - Sorting and Limiting
    - Content-Type
  - Integration with Site
    - UI Integration
    - Feed Architecture
  - RSS API Direct Endpoints
    - GET /api/rss/all
    - GET /api/rss/{sectionName}
    - GET /api/rss/{sectionName}/{collectionName}
  - RSS Proxy Endpoints (Blazor Web)
    - GET /all/feed.xml
    - GET /all/roundups/feed.xml
    - GET /{sectionName}/feed.xml
  - Implementation & Testing

## File: [docs/running-and-testing.md](running-and-testing.md)

- Running and Testing
  - Development Environment
    - Option 1: GitHub Codespaces (Browser)
    - Option 2: DevContainer (VS Code)
  - Running the Application
    - Method 1: VS Code F5 (Recommended)
    - Method 2: The `Run` Script
  - Running Tests
    - Basic Usage
    - Advanced Usage
    - Important Notes

## File: [docs/seo.md](seo.md)

- SEO (Search Engine Optimization)
  - Server-Side Rendering (SSR)
  - Schema.org Structured Data
    - Article Schema
    - Implementation Pattern
  - Page Titles
  - Meta Description
  - Canonical URLs
  - Open Graph Tags
  - Twitter Cards
  - RSS Feeds
  - Semantic HTML
  - URL Structure
  - Implementation Reference

## File: [docs/technology-stack.md](technology-stack.md)

- Technology Stack
  - Runtime & Core Frameworks
  - Frontend
  - Infrastructure
  - Testing
  - Scripting

## File: [docs/terminology.md](terminology.md)

- Site Terminology
  - Core Concepts
    - Naming Convention
  - Collection Types
  - Standard Values
    - Section Names
    - Collection Names
- Specialized Collections & Classification
  - Specialized Collections
  - Alt-Collection
    - Use Case
    - Behavior

## File: [docs/testing-strategy.md](testing-strategy.md)

- Testing Strategy
  - Testing Diamond
  - Why Testing Diamond?
  - Test Distribution Philosophy
  - Database Strategy
  - Test Layer Definitions
    - E2E Tests (End-to-End)
    - Integration Tests
    - Unit Tests
    - Component Tests
  - Test Layer Mapping
  - Test Doubles Terminology
  - When to Use Real vs Stub/Mock
    - Use REAL Implementations
    - Stub/Mock (Only These Cases)
    - NEVER Allowed in Unit Tests
  - Testing Singleton Services
  - Implementation Reference

## File: [docs/toc-component.md](toc-component.md)

- Table of Contents Component
  - Component Overview
  - Usage Pattern
    - Basic Usage
    - Generating HTML Content
    - Component Features
  - Scroll Spy Architecture
    - Detection Point
    - Scroll Height Requirement
    - How Scroll Spy Works
  - Content Container Requirements
    - Required Structure
    - HTML Content Generation
  - Expected Behavior
    - User Interactions
    - Accessibility Expectations

## File: [docs/writing-style-guidelines.md](writing-style-guidelines.md)

- Writing Style Guidelines
  - Core Writing Principles
    - Tone and Voice
    - Language Standards
    - Character and Typography Standards
    - Punctuation and Sentence Flow
  - Specific Guidelines for AI Models
    - Common AI Writing Patterns to Avoid
    - Positive Patterns to Embrace
    - Writing Quality Checks for AI
  - What to Avoid
    - Exaggerated Language
    - Vague or Generic Language
    - Technical Jargon Without Context
  - What to Embrace
    - Authentic Language
    - Clear Structure
  - Content-Specific Guidelines
    - Technical Documentation
    - Article Excerpts and Descriptions
    - User Interface Text
  - Quality Checks
    - Before Publishing
    - Content Review Process
  - Examples
    - Good vs. Poor Writing
    - Content Type Examples

## File: [src/AGENTS.md](../src/AGENTS.md)

- Source Code Development Guide
  - Port Configuration
  - Critical Development Rules
    - Starting, Running, and Testing
    - ✅ Always Do
    - ⚠️ Ask First
    - 🚫 Never Do
  - Directory Structure
  - Code Quality Standards
    - Code Analysis Configuration
    - EditorConfig Standards
    - Code Analysis Settings
    - Strategic Warning Suppressions
    - Code Quality Results
    - Usage in Development
  - Shared .NET Patterns
    - Dependency Injection Service Lifetimes
    - Markdown Frontmatter Mapping
  - Documentation Resources
    - Functional Documentation (docs/)
    - Implementation Guides (AGENTS.md)

## File: [src/TechHub.Api/AGENTS.md](../src/TechHub.Api/AGENTS.md)

- TechHub.Api Development Guide
  - Starting & Running
  - Project Structure
  - RESTful Design Principles
  - Minimal API Patterns
    - Endpoint Organization
    - OpenAPI Documentation
    - Endpoint Handler Patterns
    - Response Patterns
    - Error Handling
  - Dependency Injection
  - Configuration
  - Testing
  - Common Patterns
    - Filtering and Querying
    - RSS Feed Generation
    - Functional Documentation (docs/)
    - Implementation Guides (AGENTS.md)

## File: [src/TechHub.Core/AGENTS.md](../src/TechHub.Core/AGENTS.md)

- TechHub.Core Development Guide
  - Project Structure
  - Core Principles
    - No External Dependencies
    - Immutability by Default
  - Domain Model Patterns
    - Entity Design
    - Value Object Pattern
    - Unified Model Pattern (No Separate DTOs)
    - Content Item Model
  - Markdown Frontmatter Mapping
  - Model Patterns
    - Collection Model - Custom Page Ordering
  - Repository Interfaces
  - URL Generation Methods
  - Unix Epoch Timestamp Usage
  - Model Conversion Extensions
  - Validation Patterns
  - Testing
    - Functional Documentation (docs/)
    - Implementation Guides (AGENTS.md)

## File: [src/TechHub.Infrastructure/AGENTS.md](../src/TechHub.Infrastructure/AGENTS.md)

- TechHub.Infrastructure Development Guide
  - Project Structure
  - Database Architecture
    - Provider Configuration
    - Schema Overview
  - Repository Patterns
    - Database Content Repository
  - Content Synchronization
    - ContentSyncService
  - Markdown Processing
    - Frontmatter Parsing
    - Markdown to HTML Conversion
  - Service Registration
  - Tag Services
    - Tag Cloud Service
    - Tag Filtering
  - Error Handling
  - Testing
    - Functional Documentation (docs/)
    - Implementation Guides (AGENTS.md)

## File: [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md)

- Blazor Frontend Development Guide
  - Critical Rules
    - ✅ Always Do
    - ⚠️ Ask First
    - 🚫 Never Do
  - Render Mode Strategy
  - Semantic HTML & Page Structure
  - Design System
    - CSS Architecture
    - JavaScript Architecture
    - Page Structure and Sidebar Components
  - Component Patterns
    - Razor Variable Naming Conflicts
    - Client-Side Navigation Without Re-Renders
    - Skeleton Loading States
    - Article Sidebar Component
    - Basic Component Structure
    - API Client Usage
    - RSS Feed Proxy Endpoints
    - Component with Background Image
    - Date Formatting
  - File Structure
  - Image Conventions
  - Static Files & Browser Caching
  - Testing Components
  - Common Patterns
    - Error Boundary
    - Loading States
    - Responsive Grid
    - Infinite Scroll Pagination
    - Conditional JavaScript Loading
    - JavaScript Utilities
    - Component Catalog Organization
    - Schema.org Structured Data
    - Render Mode Selection
    - Custom Page Patterns
    - Mobile Navigation (Hamburger Menu)
    - Tag Filtering Behavior
    - Functional Documentation (docs/)
    - Implementation Guides (AGENTS.md)

## File: [collections/AGENTS.md](../collections/AGENTS.md)

- Collections Management Guide
  - Critical Content Rules
    - ✅ Always Do
    - ⚠️ Ask First
    - 🚫 Never Do
  - When to Use This Guide
  - Collections Structure
  - Content Organization
  - Frontmatter Schema
    - Collection-Specific Fields
    - Deprecated Fields (Do Not Use)
    - Excerpt Section
    - Content Section
    - Functional Documentation (docs/)
    - Implementation Guides (AGENTS.md)

## File: [scripts/AGENTS.md](../scripts/AGENTS.md)

- PowerShell Development Agent
  - Critical PowerShell Rules
    - ✅ Always Do
    - ⚠️ Ask First
    - 🚫 Never Do
  - When to Use This Guide
  - Directory Structure
  - PowerShell Syntax Rules
    - Correct Examples
    - Wrong Examples (Never Use)
  - Script Standards
    - Parameter Definitions
    - Function Paths Pattern
    - Error Handling
  - Key Scripts
    - Content Processing (in `content-processing/`)
    - Infrastructure
    - Testing
  - PowerShell Testing Standards
    - Test File Location
    - Running Pester Tests
  - Common Functions
    - Get-SourceRoot.ps1
    - Invoke-AiApiCall.ps1
    - Convert-RssToMarkdown.ps1
    - Feed.ps1
  - Best Practices
    - Commands
    - Pipeline Operations
    - Parameter Validation
  - Running Scripts
    - From Repository Root
    - From GitHub Actions
  - Data File Locations
    - Functional Documentation (docs/)
    - Implementation Guides (AGENTS.md)

## File: [tests/AGENTS.md](../tests/AGENTS.md)

- Testing Strategy for Tech Hub .NET
  - Database Strategy
  - Core Testing Rules
  - Test Doubles Terminology
  - Unit Testing Patterns
    - AAA Pattern (Arrange-Act-Assert)
    - Test Naming Convention
    - Theory Tests for Parameterization
    - Testing Singleton Services
    - Test Fixtures (IClassFixture\<T\>)
  - Integration Testing Patterns
    - Test Data File Conventions
    - HTTP Pipeline Testing
  - Directory Structure
  - Testing Strategy
    - Testing Diamond
    - Test Layer Mapping
  - Understanding Test Layers - Detailed Definitions
    - E2E Tests (End-to-End)
    - Integration Tests
    - Unit Tests
    - Component Tests (Blazor)
    - PowerShell Tests
  - Test Project Navigation
  - Cross-References to Source Code
  - Shared Testing Utilities
    - Directory.Build.props
    - Common Test Helpers
    - Functional Documentation (docs/)
    - External Resources

## File: [tests/powershell/AGENTS.md](../tests/powershell/AGENTS.md)

- PowerShell Test Suite
  - What This Directory Contains
  - Where the Implementation Lives
  - Testing Framework
  - Test File Naming Convention
  - Test Structure
  - Testing Best Practices
    - Do's
    - Don'ts
  - Common Test Patterns
    - Testing Function Output
    - Testing Error Handling
    - Mocking External Calls
  - Test Data Management
  - Coverage Requirements
  - Troubleshooting
    - Tests Not Found
    - Function Not Found
    - Import Errors
  - Key Testing Rules

## File: [tests/TechHub.Api.Tests/AGENTS.md](../tests/TechHub.Api.Tests/AGENTS.md)

- API Integration Tests - Tech Hub
  - What This Directory Contains
  - Testing Strategy
  - Test Patterns
    - Using WebApplicationFactory
    - What to Test
  - Best Practices
  - Common Pitfalls

## File: [tests/TechHub.Core.Tests/AGENTS.md](../tests/TechHub.Core.Tests/AGENTS.md)

- Core Unit Tests - Tech Hub
  - What This Directory Contains
  - Testing Strategy
  - Test Patterns
    - What to Test in Domain Models
    - Test Data Factories
  - Best Practices
  - Common Pitfalls

## File: [tests/TechHub.E2E.Tests/AGENTS.md](../tests/TechHub.E2E.Tests/AGENTS.md)

- E2E Tests - Tech Hub
  - Critical Rules
  - Understanding Timeout Failures
  - Interactive Debugging with Playwright MCP
  - Test Architecture
    - Structure
    - Test Organization Strategy
    - API Test Organization
    - Shared Page Pattern
    - Browser Configuration
    - Performance Optimizations
    - Blazor JavaScript Initializers (Ready Detection)
  - Writing New Tests
    - Test Naming Convention
    - Playwright Expect Assertions and Wait Patterns
    - Using BlazorHelpers
    - Assertion Style
    - Writing New Test Classes
  - TOC Scroll Synchronization Patterns
    - The Problem: Race Conditions
    - Solution 1: Playwright Polling (Recommended)
    - Solution 2: Event-Based Waiting (Helper Methods)
    - Pattern for TOC Tests
    - Common TOC Test Scenarios
    - Debugging TOC Issues
    - Performance Notes
    - Related Files
  - Maintenance
    - When to Update Tests
    - Test Stability

## File: [tests/TechHub.Infrastructure.Tests/AGENTS.md](../tests/TechHub.Infrastructure.Tests/AGENTS.md)

- Infrastructure Tests - Tech Hub
  - What This Directory Contains
  - Testing Strategy
  - Test Patterns
    - Testing Singleton Services
    - What to Test
  - Best Practices
  - Common Pitfalls
  - Test Data Location

## File: [tests/TechHub.TestUtilities/AGENTS.md](../tests/TechHub.TestUtilities/AGENTS.md)

- TechHub.TestUtilities
  - Purpose
  - Test Data Strategy
    - Unit Tests (Core/Infrastructure)
    - Integration Tests (API)
    - E2E Tests
  - TechHubApiFactory Classes
    - TechHubIntegrationTestApiFactory
    - TechHubE2ETestApiFactory
  - TestCollectionsSeeder
  - DatabaseFixture
  - Configuration
    - Integration Test Configuration
    - E2E Test Configuration
  - Dependencies
  - Test Builders (`A` Pattern)
    - Functional Documentation (docs/)
    - Implementation Guides (AGENTS.md)

## File: [tests/TechHub.Web.Tests/AGENTS.md](../tests/TechHub.Web.Tests/AGENTS.md)

- Blazor Component Tests - Tech Hub
  - What This Directory Contains
  - Testing Strategy
  - Test Patterns
    - What to Test
    - Key bUnit Patterns
  - Best Practices
  - Common Pitfalls

