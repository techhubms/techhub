# Feature Specification: Configuration Management

**Priority**: 🔴 CRITICAL - Phase 1 (Implement EARLY, after solution structure)  
**Created**: 2026-01-02  
**Status**: Placeholder  
**Implementation Order**: Should be spec #2-3, not #21

## Overview

Define configuration structure for appsettings.json across dev/staging/production environments, including connection strings, feature flags, external service endpoints, and environment-specific settings.

## Why This Is Critical Early

- **Every service needs configuration** - API endpoints, connection strings, external URLs
- **Environment-specific behavior** - Dev vs staging vs production settings
- **Feature flags** - Toggle features without redeploying
- **Secrets management** - Azure Key Vault integration patterns

## Scope

**Must Define**:
- appsettings.json structure and schema
- Environment-specific configuration (appsettings.Development.json, appsettings.Production.json)
- Configuration validation on startup
- Azure App Configuration integration (optional)
- Azure Key Vault for secrets (connection strings, API keys)
- Feature flags pattern
- Configuration binding to strongly-typed classes

**Key Configuration Areas**:
- Logging configuration (Application Insights, console, file)
- Content repository settings (file paths, cache duration)
- API URLs and external services (YouTube API if needed)
- Performance settings (cache timeouts, batch sizes)
- Monitoring and instrumentation (Application Insights key)

**Caching Configuration** (IMemoryCache):
- **Strategy**: In-memory cache with sliding/absolute expiration
- **RSS Feeds**: 30-minute absolute expiration (balance freshness vs load)
- **API Responses**: 1-hour absolute expiration (sections, content lists)
- **Static Content**: Output caching for rendered pages
- **Invalidation**: Cache cleared automatically on container restart (acceptable for Git-based content updates)
- **Memory Limits**: Configure max cache size to prevent memory exhaustion
- **Cache Keys**: Consistent naming convention for different content types

**Configuration Example**:
```json
{
  "Caching": {
    "RssFeedCacheDuration": "00:30:00",
    "ApiResponseCacheDuration": "01:00:00",
    "MaxCacheSizeInMB": 100,
    "SlidingExpirationEnabled": false
  }
}
```

## Implementation Note

⚠️ **This spec is numbered 021 but should be implemented in Phase 1**, right after 001-solution-structure and before most other specs. The number is sequential, not priority-based.

## Status

📝 **Placeholder** - Needs detailed requirements, configuration examples, validation rules, and Key Vault integration patterns.

## References

- [.NET Configuration Documentation](https://learn.microsoft.com/aspnet/core/fundamentals/configuration)
- [Azure Key Vault Configuration Provider](https://learn.microsoft.com/aspnet/core/security/key-vault-configuration)
- [Azure App Configuration](https://learn.microsoft.com/azure/azure-app-configuration/overview)
