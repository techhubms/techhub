-- Migration 011: Add job_type column to content_processing_jobs
-- Allows the same table to track both content processing and roundup generation jobs,
-- providing a unified admin dashboard overview.

ALTER TABLE content_processing_jobs
    ADD COLUMN IF NOT EXISTS job_type TEXT NOT NULL DEFAULT 'content-processing';
