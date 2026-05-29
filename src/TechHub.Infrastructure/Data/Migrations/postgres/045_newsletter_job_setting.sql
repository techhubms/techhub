-- ============================================================
-- 045: Add Newsletter background job setting
-- ============================================================

INSERT INTO background_job_settings (job_name, enabled, description) VALUES
    ('Newsletter', FALSE, 'Newsletter sending — sends weekly roundup and daily overview emails to subscribers on a scheduled basis')
ON CONFLICT (job_name) DO NOTHING;
