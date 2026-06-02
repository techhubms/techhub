ALTER TABLE newsletter_send_log
    ADD COLUMN IF NOT EXISTS failed_count INT NOT NULL DEFAULT 0;
