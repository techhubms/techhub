CREATE TABLE IF NOT EXISTS newsletter_subscribers (
    id                  BIGSERIAL PRIMARY KEY,
    email               TEXT NOT NULL,
    display_name        TEXT,
    is_confirmed        BOOLEAN NOT NULL DEFAULT FALSE,
    confirmation_token  TEXT,
    subscribed_at       TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    confirmed_at        TIMESTAMPTZ,
    unsubscribed_at     TIMESTAMPTZ,
    preferences         JSONB NOT NULL DEFAULT '{"weeklySections":[],"dailySections":[]}'::jsonb
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_newsletter_subscribers_email_active
    ON newsletter_subscribers (lower(email))
    WHERE unsubscribed_at IS NULL;

CREATE INDEX IF NOT EXISTS idx_newsletter_subscribers_preferences
    ON newsletter_subscribers USING GIN (preferences);

CREATE TABLE IF NOT EXISTS newsletter_send_log (
    id              BIGSERIAL PRIMARY KEY,
    send_kind       TEXT NOT NULL,
    target_key      TEXT NOT NULL,
    sent_at         TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    recipient_count INT NOT NULL,
    status          TEXT NOT NULL DEFAULT 'sent',
    error_message   TEXT
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_newsletter_send_log_kind_key
    ON newsletter_send_log (send_kind, target_key);
