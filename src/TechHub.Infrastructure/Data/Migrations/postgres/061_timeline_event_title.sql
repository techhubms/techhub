-- A short headline per timeline event that says what the event is, so a card on a timeline is understood at a glance.
-- NULL for events stored before titles existed: they are shown as product and feature until the timeline backfill scans their article again.
ALTER TABLE content_timeline_events ADD COLUMN IF NOT EXISTS title TEXT;
