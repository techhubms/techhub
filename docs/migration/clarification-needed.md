# Tech Hub .NET Migration - Clarifications Needed

> **Purpose**: Document questions and decisions that require stakeholder input before implementation

## Project Scope & Priorities

### Question 1: Visual Design Evolution

**Context**: The migration plan states "Must modernize visual design (not replicate Jekyll)" but also "preserve overall visual look."

**Clarification Needed**:
- What level of visual modernization is acceptable?
- Should we maintain exact color schemes and layouts?
- Can we update typography, spacing, animations?
- Should we add dark mode support?
- Are there specific modern UX patterns to incorporate (e.g., infinite scroll, skeleton loaders)?

**Impact**: Affects Phase 4 (Features Implementation) timeline and scope

**Recommendation**: Create visual mockups for approval before implementation

---

### Question 2: URL Structure Changes

**Context**: Migration plan suggests preserving main URLs where practical with 301 redirects acceptable.

**Clarification Needed**:
- Are 301 redirects acceptable for ALL existing URLs?
- Should we maintain backwards compatibility for query parameters?
- Can we simplify URL structure (e.g., remove .html extensions)?
- What's the priority: SEO continuity vs clean modern URLs?

**Current Jekyll URLs**:
- Section index: `/github-copilot/`
- Collection: `/github-copilot/news/` (directory listing)
- Item: `/2025-01-15-item-title.html` (date-prefixed)

**Proposed .NET URLs**:
- Section index: `/github-copilot`
- Collection: `/github-copilot/news.html`
- Item: `/github-copilot/news/item-title.html` (no date prefix)

**Impact**: Affects Phase 2 (Core Architecture) and Phase 6 (Infrastructure)

**Recommendation**: Approve URL structure before API implementation

---

### Question 3: Search Functionality

**Context**: Migration plan mentions "Azure AI Search or Elasticsearch" for full-text search.

**Clarification Needed**:
- Is advanced search required for MVP?
- Can we defer search to post-launch enhancement?
- If required now, prefer managed service (Azure AI Search) or self-hosted (Elasticsearch)?
- What search features are must-haves? (autocomplete, fuzzy matching, faceted search)

**Impact**: Affects Phase 3 (Content System) and Phase 6 (Infrastructure) budget

**Recommendation**: Defer advanced search; implement client-side text filtering for MVP

---

### Question 4: Content Update Frequency

**Context**: Repository pattern uses 1-hour cache duration for content.

**Clarification Needed**:
- How frequently is content published? (Daily? Multiple times per day?)
- Is 1-hour staleness acceptable?
- Do we need real-time content updates?
- Should we implement webhook-based cache invalidation?

**Impact**: Affects caching strategy and infrastructure complexity

**Recommendation**: Start with 1-hour cache; add webhook invalidation if needed

---

### Question 5: Multi-Location Content Access

**Context**: Same content can appear in multiple sections (e.g., `/ai/videos/vs-code-107.html` and `/github-copilot/videos/vs-code-107.html`).

**Clarification Needed**:
- Is this feature required, or can we simplify to single canonical URL per item?
- If required, how should we handle section-specific styling/context?
- Should breadcrumbs reflect current section path?
- How do we prevent duplicate content SEO penalties?

**Current Behavior**: Jekyll allows this via categories array  
**SEO Concern**: Duplicate content without proper canonical tags

**Impact**: Affects domain models, API design, and SEO implementation

**Recommendation**: Keep feature but ensure robust canonical URL implementation

---

## Infrastructure & Deployment

### Question 6: Azure Subscription & Resources

**Clarification Needed**:
- Which Azure subscription should be used?
- What resource naming conventions should we follow?
- What environments are needed? (dev, staging, production)
- Who has access to Azure portal and deployment permissions?
- What are budget constraints?

**Impact**: Affects Phase 6 (Infrastructure) and Phase 7 (CI/CD)

**Recommendation**: Set up Azure resources and access before Phase 6

---

### Question 7: Custom Domain & DNS

**Clarification Needed**:
- Will custom domain remain tech.hub.ms?
- Who manages DNS records?
- What SSL certificate strategy? (Let's Encrypt, Azure managed, custom)
- Are there CDN requirements?

**Impact**: Affects Phase 6 (Infrastructure) and Phase 8 (Migration)

**Recommendation**: Plan DNS cutover strategy before deployment

---

### Question 8: Monitoring & Alerting

**Clarification Needed**:
- Who should receive alert notifications?
- What alert thresholds are acceptable? (error rate, response time, availability)
- Should we integrate with existing monitoring tools?
- What SLA targets should we aim for?

**Impact**: Affects Phase 6 (Monitoring Configuration)

**Recommendation**: Define SLOs before implementing monitoring

---

## Security & Compliance

### Question 9: Authentication Requirements

**Context**: Migration plan includes "Authentication Preparation" for future IdentityServer/Duende integration.

**Clarification Needed**:
- Is authentication planned for specific timeline?
- What scenarios require authentication? (admin content editing, private sections, commenting)
- Should we use Microsoft Entra ID (Azure AD)?
- Any compliance requirements (GDPR, CCPA, etc.)?

**Impact**: Affects architecture decisions now even if implemented later

**Recommendation**: Design API with auth in mind; implement when requirements clear

---

### Question 10: Content Security Policy

**Clarification Needed**:
- What external domains need to be whitelisted? (YouTube, analytics, CDNs)
- Are there iframe embedding requirements?
- Should we implement Subresource Integrity (SRI) for external scripts?

**Impact**: Affects Phase 4 (Security Implementation)

**Recommendation**: Define CSP headers based on current site analysis

---

## Features & Functionality

### Question 11: RSS Feed Compatibility

**Context**: Migration plan states "support RSS feed URLs (content can differ)."

**Clarification Needed**:
- Must RSS feed content be byte-for-byte identical to Jekyll?
- Can we improve feed format (add media enclosures, full content, etc.)?
- Are there RSS feed readers/aggregators we must test against?
- Should we support both RSS 2.0 and Atom 1.0?

**Impact**: Affects Phase 3 (RSS Implementation)

**Recommendation**: Improve feeds while maintaining URL compatibility

---

### Question 12: Google Analytics Migration

**Context**: Jekyll site uses GA. Migration to .NET likely requires GA4.

**Clarification Needed**:
- Current GA property ID?
- Migrate to GA4 or keep Universal Analytics?
- Any custom events or conversions to preserve?
- Should we implement server-side tracking?

**Impact**: Affects Phase 4 (Analytics Implementation)

**Recommendation**: Migrate to GA4 during .NET migration

---

### Question 13: Video Content Handling

**Context**: Videos use YouTube embeds via `youtube_id` frontmatter.

**Clarification Needed**:
- Will all videos remain on YouTube?
- Are there plans to support other platforms (Vimeo, Azure Media Services)?
- Should we implement lazy loading for video embeds?
- Any thumbnail or preview requirements?

**Impact**: Affects markdown processing and component design

**Recommendation**: Support YouTube initially; design extensible for other platforms

---

## Testing & Quality Assurance

### Question 14: Test Data & Environments

**Clarification Needed**:
- Can we use production content for testing, or need sanitized test data?
- Who will perform UAT (User Acceptance Testing)?
- What browsers/devices must we support?
- Are there automated accessibility testing requirements?

**Impact**: Affects Phase 5 (Testing Strategy)

**Recommendation**: Define browser matrix and testing team before Phase 5

---

### Question 15: Performance Benchmarks

**Context**: Migration plan defines performance targets (Lighthouse > 95, LCP < 2.5s, etc.).

**Clarification Needed**:
- Are these targets measured on staging or production?
- What connection speed should we test? (4G, 3G, broadband)
- Should we test from multiple geographic locations?
- What happens if we don't meet targets? (block deployment, accept technical debt)

**Impact**: Affects Phase 5 (Performance Testing)

**Recommendation**: Agree on measurement methodology before testing

---

## Migration & Cutover

### Question 16: Content Freeze Period

**Clarification Needed**:
- Will there be content freeze during migration?
- How long is acceptable downtime?
- Should we implement read-only mode for Jekyll before cutover?
- What's rollback plan if migration fails?

**Impact**: Affects Phase 8 (Migration Steps)

**Recommendation**: Plan maintenance window and communication strategy

---

### Question 17: Post-Migration Support

**Clarification Needed**:
- Who handles bug reports after launch?
- What's the SLA for fixing critical issues?
- Should we maintain Jekyll site as fallback?
- When can we decommission Jekyll infrastructure?

**Impact**: Affects Phase 8 (Post-Migration)

**Recommendation**: Define support plan before go-live

---

## Open Technical Questions

### Question 18: Infinite Scroll Pagination

**Context**: Constitution mandates infinite scroll for modern UX.

**Clarification Needed**:
- Should infinite scroll be on all collection pages?
- What batch size? (20 items suggested)
- Should we provide toggle between infinite scroll and traditional pagination?
- How to handle SEO for paginated content?

**Impact**: Affects filtering system and API design

**Recommendation**: Implement infinite scroll with SEO-friendly static pagination

---

### Question 19: Caching Strategy

**Context**: Multiple caching layers proposed (memory cache, output cache, Redis).

**Clarification Needed**:
- Is Redis budget-approved for distributed caching?
- Can we use in-memory cache initially and add Redis later?
- What's cache invalidation strategy when content updates?
- Should we implement cache warming on deployment?

**Impact**: Affects Phase 2 (Core Architecture) and Phase 6 (Infrastructure)

**Recommendation**: Start with memory cache; add Redis when traffic demands

---

### Question 20: Content Management Workflow

**Context**: Current workflow involves RSS processing and manual content creation.

**Clarification Needed**:
- Will content creation workflow change with .NET migration?
- Should we build admin UI for content management?
- Or maintain file-based workflow with Git?
- Any plans for headless CMS integration?

**Impact**: Could significantly expand scope

**Recommendation**: Maintain file-based workflow for MVP; defer CMS to future phase

---

## Priority Classification

### High Priority (Blocks Implementation)

1. **Question 2**: URL Structure Changes
2. **Question 5**: Multi-Location Content Access
3. **Question 6**: Azure Subscription & Resources
4. **Question 16**: Content Freeze Period

### Medium Priority (Affects Architecture)

1. **Question 3**: Search Functionality
2. **Question 9**: Authentication Requirements
3. **Question 19**: Caching Strategy
4. **Question 20**: Content Management Workflow

### Low Priority (Can Decide During Implementation)

1. **Question 1**: Visual Design Evolution
2. **Question 11**: RSS Feed Compatibility
3. **Question 13**: Video Content Handling
4. **Question 18**: Infinite Scroll Pagination

---

## Next Steps

1. **Review this document** with product owner and stakeholders
2. **Prioritize questions** based on implementation timeline
3. **Schedule decision meetings** for high-priority items
4. **Document decisions** in migration plan or constitution
5. **Update specifications** based on clarifications

## Document Metadata

- **Created**: 2026-01-01
- **Last Updated**: 2026-01-01
- **Owner**: Tech Hub Migration Team
- **Status**: Draft - Awaiting Stakeholder Review

