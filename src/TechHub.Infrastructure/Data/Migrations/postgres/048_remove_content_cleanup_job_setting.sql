-- Content cleanup is manual-trigger-only and never reads a background job setting.
DELETE FROM background_job_settings
WHERE job_name = 'ContentCleanup';
