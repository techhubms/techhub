# Date and Timezone Processing

This document covers the core timezone configuration for the Tech Hub. For implementation details, see the domain-specific AGENTS.md files.

## Critical Timezone Configuration

**CRITICAL**: Jekyll runs in the `Europe/Brussels` timezone and all date calculations must be consistent with this setting.

### Jekyll Configuration

The timezone is configured in `_config.yml`:

```yaml
timezone: Europe/Brussels
```

This affects:

- Jekyll build process date handling
- All Liquid date filters
- Ruby plugin date calculations
- Content sorting and filtering

### Timezone Requirements

- All date filters and calculations must respect `Europe/Brussels` timezone
- Server-side (Liquid) and Ruby plugin date handling must be synchronized
- All dates are normalized to midnight Brussels time for epoch comparisons

## Date Format Standards

### Input Formats

- **Standard Format**: `YYYY-MM-DD` (ISO 8601)
- **Full Datetime**: `YYYY-MM-DD HH:MM:SS +ZONE`
- **Jekyll Front Matter**: Use standard Jekyll date format

### Output Formats

- **Epoch Timestamps**: PRIMARY format for all date storage and calculations
- **Display Formats**: Generated dynamically at runtime
- **URL Parameters**: ISO date format for consistency

## Implementation References

For implementation details, code examples, and patterns:

- **Liquid/Jekyll date filters**: See [jekyll-development.md](jekyll-development.md#date-and-timezone-handling)
- **Ruby plugin date handling**: See [_plugins/AGENTS.md](../_plugins/AGENTS.md#date-handling)
- **JavaScript client-side dates**: See [assets/js/AGENTS.md](../assets/js/AGENTS.md#date-and-timezone-handling)
- **Content limiting with dates**: See [filtering-system.md](filtering-system.md)
