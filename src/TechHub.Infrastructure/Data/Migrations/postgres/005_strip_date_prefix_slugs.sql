-- Migration 005: Strip date prefix from slugs
-- Removes YYYY-MM-DD- prefix from all slug columns across relevant tables.
-- UrlNormalizationMiddleware handles 301 redirects from old date-prefixed URLs.
-- Pre-validated: zero slug collisions exist after stripping (verified locally and in prod).

BEGIN;

-- 1. content_items (primary key: collection_name + slug)
UPDATE content_items
SET    slug       = regexp_replace(slug, '^[0-9]{4}-[0-9]{2}-[0-9]{2}-', ''),
       updated_at = NOW()
WHERE  slug ~ '^[0-9]{4}-[0-9]{2}-[0-9]{2}-';

-- 2. content_tags_expanded (foreign key references collection_name + slug)
UPDATE content_tags_expanded
SET    slug = regexp_replace(slug, '^[0-9]{4}-[0-9]{2}-[0-9]{2}-', '')
WHERE  slug ~ '^[0-9]{4}-[0-9]{2}-[0-9]{2}-';

-- 3. content_reviews
UPDATE content_reviews
SET    slug = regexp_replace(slug, '^[0-9]{4}-[0-9]{2}-[0-9]{2}-', '')
WHERE  slug ~ '^[0-9]{4}-[0-9]{2}-[0-9]{2}-';

-- 4. processed_urls
UPDATE processed_urls
SET    slug = regexp_replace(slug, '^[0-9]{4}-[0-9]{2}-[0-9]{2}-', '')
WHERE  slug IS NOT NULL
  AND  slug ~ '^[0-9]{4}-[0-9]{2}-[0-9]{2}-';

COMMIT;
