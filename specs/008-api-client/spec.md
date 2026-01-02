# Feature Specification: API Client (Blazor ↔ API Communication)

**Feature Number**: 025
**Status**: Placeholder - To Be Detailed
**Priority**: CRITICAL (Required for MVP)

## Overview

Implement typed HttpClient service for Blazor frontend to communicate with TechHub.Api backend, including request/response models, error handling, retry policies, and authentication preparation.

## Why This Matters

Blazor Web (frontend) must call TechHub.Api (backend) to retrieve content, sections, filters, RSS feeds, etc. Without a well-designed API client:
- Developers hardcode URLs and manually serialize JSON
- No type safety between frontend and backend
- Error handling is inconsistent
- Retry logic duplicated everywhere
- Breaking changes cause runtime failures

A typed API client provides compile-time safety and consistent communication patterns.

## Key Requirements (To Be Detailed)

### API Client Architecture
- **IApiClient interface**: Abstraction for testing
- **ApiClient implementation**: HttpClient wrapper with typed methods
- **Shared DTOs**: Request/response models shared between API and Web
- **Base URL configuration**: Environment-specific API endpoints

### Client Methods (Mirror API Endpoints)
- `GetSectionsAsync()` → List of sections
- `GetSectionContentAsync(sectionKey)` → Section with collections
- `GetCollectionItemsAsync(section, collection, filters)` → Paginated items
- `GetItemBySlugAsync(section, collection, slug)` → Single item
- `GetRssFeedAsync(sectionKey)` → RSS feed XML
- `SearchContentAsync(query, filters)` → Search results

### Error Handling
- HTTP status code handling (404, 500, etc.)
- Timeout configuration
- Retry policies via Polly
- Structured error responses
- User-friendly error messages

### Serialization
- System.Text.Json configuration
- Camel case naming policy
- Date/time handling (UTC conversion)
- Null value handling

### Testing Support
- Mock API client for component testing
- Test helpers for creating API responses
- In-memory API for E2E tests

## Technologies

- **HttpClient**: .NET HTTP client
- **IHttpClientFactory**: Typed client registration
- **Polly**: Retry and circuit breaker policies
- **System.Text.Json**: JSON serialization

## Dependencies

- `/specs/008-api-endpoints/spec.md` - API contract definitions
- `/specs/003-resilience-error-handling/spec.md` - Retry policies
- `/specs/002-configuration-management/spec.md` - API URL configuration
- `/specs/014-blazor-components/spec.md` - Components using API client

## Success Criteria

- All API calls type-safe (compile-time checked)
- Consistent error handling across all calls
- Retry policies applied automatically
- API client easily mockable for testing
- Clear separation between API and Web concerns
- Auth-ready (can add authentication headers when needed)

## Next Steps

Use `/speckit.specify` to create full specification with IApiClient interface, typed methods, DTO definitions, HttpClientFactory configuration, Polly policies, error handling patterns, and testing strategies.
