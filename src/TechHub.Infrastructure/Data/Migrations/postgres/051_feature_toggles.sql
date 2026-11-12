-- ============================================================
-- 051: Feature toggles
-- Lets an admin switch a feature off, limit it to admins, or open it to everyone
-- ============================================================

CREATE TABLE IF NOT EXISTS feature_toggles (
    feature_key TEXT        PRIMARY KEY,
    mode        TEXT        NOT NULL DEFAULT 'disabled' CHECK (mode IN ('disabled', 'admins', 'everyone')),
    description TEXT        NOT NULL DEFAULT '',
    updated_at  TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

INSERT INTO feature_toggles (feature_key, mode, description) VALUES
    ('timeline', 'admins', 'Timeline — the dated events (preview, GA, deprecation, ...) of the articles of a section, with its navigation buttons and page')
ON CONFLICT (feature_key) DO NOTHING;
