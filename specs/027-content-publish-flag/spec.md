# Feature Specification: Content Publication Control

**Feature Branch**: `027-content-publish-flag`  
**Created**: 2026-01-03  
**Status**: Draft  
**Input**: User description: "Add published frontmatter flag to control content visibility. Placeholder files with future dates need published: false to hide from regular content. Repository filtering should exclude unpublished items. GitHub Copilot features page should show all items including unpublished."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Hide Unpublished Content from Regular Pages (Priority: P1)

Content creators need to create placeholder files for future content (especially GitHub Copilot feature videos) that should not appear on regular content listings, search results, or RSS feeds until they are ready for publication.

**Why this priority**: This is the core requirement - preventing incomplete/placeholder content from appearing on the public site while allowing it to exist in the repository.

**Independent Test**: Can be fully tested by creating a content item with `published: false`, navigating to any regular content listing page, and verifying the item does not appear. Delivers immediate value by hiding work-in-progress content.

**Acceptance Scenarios**:

1. **Given** a content item has `published: false` in frontmatter, **When** a user views any section page (AI, GitHub Copilot, etc.), **Then** that item does not appear in the content listing
2. **Given** a content item has `published: false` in frontmatter, **When** the RSS feed is generated, **Then** that item is excluded from the feed
3. **Given** a content item has `published: false` in frontmatter, **When** a user performs a search, **Then** that item does not appear in search results
4. **Given** a content item has no `published` field in frontmatter, **When** content is filtered, **Then** it is treated as published (default behavior)

---

### User Story 2 - Show All Content on GitHub Copilot Features Page (Priority: P2)

Content managers need to see all GitHub Copilot feature content (including unpublished placeholders) on the dedicated GitHub Copilot features overview page to track which features have been documented and which are still pending.

**Why this priority**: Enables content planning and management by providing visibility into the complete feature coverage, including future/planned content.

**Independent Test**: Can be tested by creating both published and unpublished items in the `_videos/ghc-features/` collection, navigating to the GitHub Copilot features page, and verifying all items appear. Delivers value as a content management tool.

**Acceptance Scenarios**:

1. **Given** there are items with `published: false` in the `_videos/ghc-features/` collection, **When** a user views the GitHub Copilot features overview page, **Then** all items (both published and unpublished) are displayed
2. **Given** an unpublished item appears on the features overview page, **When** displayed, **Then** it is shown as non-clickable (no link to detail page) since the documentation is not complete
3. **Given** a published item appears on the features overview page, **When** displayed, **Then** it is clickable and links to the full documentation/video page
4. **Given** a user attempts to directly access an unpublished item's URL, **When** navigating to that URL, **Then** they receive a 404 Not Found response

---

### User Story 3 - Bulk Update Existing Placeholder Files (Priority: P3)

Content administrators need to add the `published: false` frontmatter field to all existing placeholder files with future dates in the `_videos/ghc-features/` collection.

**Why this priority**: This is a one-time migration task to bring existing content into compliance with the new publication control system.

**Independent Test**: Can be tested by running a script to update all files with future dates in the target collection, then verifying each file has the `published: false` field. Delivers value by establishing baseline state.

**Acceptance Scenarios**:

1. **Given** placeholder files exist with dates in the future in `_videos/ghc-features/`, **When** the migration script runs, **Then** each file is updated with `published: false` in its frontmatter
2. **Given** a file already has `published: true` or another value, **When** the migration script runs, **Then** the existing value is preserved (no overwriting)
3. **Given** the migration is complete, **When** content is queried, **Then** all future-dated placeholders in `_videos/ghc-features/` are correctly filtered from regular pages

---

### Edge Cases

- What happens when a content item has `published: true` explicitly set? (Should behave same as no field - item is published)
- What happens when the `published` field has an invalid value (not boolean)? (Should default to published to avoid hiding content unintentionally)
- What happens when an unpublished item is directly accessed via its URL? (Should return HTTP 404 Not Found with a helpful message explaining content is not yet available, preventing search engine indexing)
- What happens if the GitHub Copilot features page itself doesn't exist yet? (Published flag filtering should still work on all other pages, features page implementation is separate concern)
- How are dates handled - should future-dated items be automatically unpublished? (No, publication status is explicit via flag, not implicit via date)

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST support a `published` boolean frontmatter field in all content items (collections: news, videos, community, blogs, roundups)
- **FR-002**: System MUST treat content items without a `published` field as published by default (backward compatibility)
- **FR-003**: System MUST exclude items with `published: false` from all regular content listings (section pages, collection pages, search results)
- **FR-004**: System MUST exclude items with `published: false` from all RSS feeds
- **FR-005**: System MUST include items with `published: false` when retrieving content for the GitHub Copilot features overview page
- **FR-005a**: System MUST display unpublished items on the GitHub Copilot features overview page as non-clickable (no hyperlink to detail page)
- **FR-005b**: System MUST display published items on the GitHub Copilot features overview page as clickable (with hyperlink to detail page)
- **FR-006**: System MUST provide a way to filter content by publication status in repository queries (e.g., "get all items" vs "get published items only")
- **FR-007**: System MUST handle invalid `published` values gracefully by treating them as published (fail-safe behavior)
- **FR-008**: Content items with `published: false` MUST return HTTP 404 Not Found status when accessed via direct URL, with a helpful message explaining the content is not yet available
- **FR-009**: Migration script MUST identify all files in `_videos/ghc-features/` with future dates and add `published: false` frontmatter
- **FR-010**: Migration script MUST preserve existing `published` field values if already present

### Key Entities

- **Content Item**: Individual piece of content (article, video, blog post, etc.) with frontmatter metadata including optional `published` boolean field. Relationships: belongs to one collection, has publication status, has publication date
- **Collection**: Group of related content items (e.g., `_videos`, `_blogs`). Relationships: contains multiple content items, has filtering rules
- **GitHub Copilot Features Page**: Special page that displays all content from `_videos/ghc-features/` regardless of publication status. Relationships: queries specific collection, overrides default filtering

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Content administrators can create placeholder content items that remain hidden from public view until explicitly published (0% of unpublished items appear on regular pages)
- **SC-002**: All existing placeholder files in `_videos/ghc-features/` with future dates are correctly marked as unpublished (100% coverage of target files)
- **SC-003**: GitHub Copilot features overview page displays complete feature inventory including unpublished items (100% of items in collection are visible)
- **SC-004**: Regular content pages show only published content without performance degradation (response time remains under 2 seconds)
- **SC-005**: RSS feeds contain only published content and remain valid XML (0% unpublished items in feeds, 100% feed validation success)
- **SC-006**: Content migration completes without data loss or corruption (100% of files retain existing data, only `published` field added)

## Clarifications

### Session 2026-01-03

- Q: When a user directly accesses an unpublished content item via its URL, what HTTP status and behavior should occur? → A: 404 Not Found with helpful message explaining content is not yet available
- Q: Which content directories should the migration script target for adding `published: false` to future-dated files? → A: Only `_videos/ghc-features/` subdirectory (specific to GitHub Copilot features)
- Q: Where should visual indicators appear to distinguish unpublished content? → A: On the GitHub Copilot features overview page, unpublished items are displayed (title/card visible) but not clickable/linked since the documentation page is not complete. Published items are clickable.
