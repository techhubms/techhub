-- Migration 009: Custom Page Data
-- Moves custom page JSON from the collections/_custom/ filesystem directory to the database,
-- allowing admin users to edit raw JSON for custom pages from the admin UI.

CREATE TABLE IF NOT EXISTS custom_page_data (
    key         TEXT PRIMARY KEY,
    description TEXT NOT NULL DEFAULT '',
    json_data   TEXT NOT NULL,
    updated_at  TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
