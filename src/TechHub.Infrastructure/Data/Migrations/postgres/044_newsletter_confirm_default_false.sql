-- New subscribers require email confirmation before they receive newsletters.
-- Existing confirmed subscribers are unchanged (is_confirmed = TRUE already).
ALTER TABLE newsletter_subscribers
    ALTER COLUMN is_confirmed SET DEFAULT FALSE;
