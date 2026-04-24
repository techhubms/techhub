# Collections

This directory holds JSON data files for custom (structured) pages. Standard content items are stored in the database, not here — only bespoke page data lives in `_custom/`.

## Contents

- `_custom/*.json` — Structured data consumed by custom-page API endpoints (one file per custom page).

## Rules

- **One JSON file per custom page.** The filename (without `.json`) is the page key used by `CustomPageDataRepository` and the `/api/custom-pages/{key}` endpoint.
- **Never hardcode section or collection names** in C#/Razor — the mapping between a custom page and its section lives in configuration (see `src/TechHub.Core/Configuration/`).
- **Schema is page-specific.** Each JSON file shape must match the corresponding `PageData` model in [src/TechHub.Core/Models/PageData/](../src/TechHub.Core/Models/PageData/). When adding or renaming fields, update both the JSON and the model together.
- **Preserve ordering.** If a list in a JSON file is rendered in order, keep the file itself ordered — do not rely on alphabetical sort at render time.
- **Keep files small and diff-friendly.** Use two-space indentation and trailing newline; avoid reformatting unrelated sections when editing.

## Related Documentation

- [Custom Pages](../docs/custom-pages.md) — Endpoint reference and data models.
- [Content Schema](../docs/content-schema.md) — Schema for standard (non-custom) content items.
- [Terminology](../docs/terminology.md) — Collection and section definitions.
