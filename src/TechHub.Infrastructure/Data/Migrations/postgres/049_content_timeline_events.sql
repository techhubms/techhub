-- Dated lifecycle events (preview, GA, deprecation, ...) that articles state, with the article they come from.
-- Written by the classification step and the timeline backfill job. Read by the weekly roundup "Dates to know" block;
-- the GitHub Copilot features page can be built on it later.
CREATE TABLE IF NOT EXISTS content_timeline_events (
    id              BIGSERIAL PRIMARY KEY,
    collection_name TEXT NOT NULL,
    item_slug       TEXT NOT NULL,
    kind            TEXT NOT NULL,           -- announced, preview, ga, launch, release, deprecation, retirement, end-of-support, breaking-change, pricing-change, event
    product         TEXT NOT NULL,
    feature         TEXT,                    -- NULL when the event is about the whole product
    description     TEXT NOT NULL,
    event_date      DATE NOT NULL,           -- first day of the month/quarter for coarse precision
    date_precision  TEXT NOT NULL,           -- day, month, quarter
    date_source     TEXT NOT NULL,           -- explicit (stated in the article) or publication (article says "now")
    quote           TEXT,                    -- sentence from the article, for spot checks
    extracted_from  TEXT NOT NULL,           -- source (processing time) or stored-content (backfill)
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    FOREIGN KEY (collection_name, item_slug)
        REFERENCES content_items (collection_name, slug) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_timeline_events_item ON content_timeline_events (collection_name, item_slug);
CREATE INDEX IF NOT EXISTS idx_timeline_events_date ON content_timeline_events (event_date, kind);
CREATE INDEX IF NOT EXISTS idx_timeline_events_product ON content_timeline_events (product, feature);

-- Which articles have been scanned for events, so the backfill job is resumable and
-- articles without any event are not scanned again.
CREATE TABLE IF NOT EXISTS content_timeline_extractions (
    collection_name TEXT NOT NULL,
    item_slug       TEXT NOT NULL,
    prompt_version  INTEGER NOT NULL,
    events_found    INTEGER NOT NULL,
    extracted_from  TEXT NOT NULL,
    extracted_at    TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    PRIMARY KEY (collection_name, item_slug),
    FOREIGN KEY (collection_name, item_slug)
        REFERENCES content_items (collection_name, slug) ON DELETE CASCADE
);
