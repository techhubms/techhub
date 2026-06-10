ALTER TABLE newsletter_subscribers
    ADD COLUMN IF NOT EXISTS last_daily_sent_at   TIMESTAMPTZ,
    ADD COLUMN IF NOT EXISTS last_daily_succeeded  BOOLEAN,
    ADD COLUMN IF NOT EXISTS last_weekly_sent_at  TIMESTAMPTZ,
    ADD COLUMN IF NOT EXISTS last_weekly_succeeded BOOLEAN;
