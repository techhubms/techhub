-- Migration 028: Backfill section roundups and normalize roundup slugs
-- Date: 2026-05-19
-- Purpose:
--   1) Rename section roundup slugs from:
--        weekly-ai-and-tech-news-roundup-{section}-{yyyy-MM-dd}
--      to:
--        weekly-{section}-roundup-{yyyy-MM-dd}
--   2) Backfill legacy /all/roundups records into section-specific roundup records
--   3) Remove legacy /all roundup rows and processed_url entries

-- ============================================================
-- 1. Rename existing section roundup slugs to short format
-- ============================================================

UPDATE content_items ci
SET slug = regexp_replace(
        ci.slug,
        '^weekly-ai-and-tech-news-roundup-([a-z\-]+)-',
        'weekly-\1-roundup-'),
    external_url = '/' || ci.primary_section_name || '/roundups/' || regexp_replace(
        ci.slug,
        '^weekly-ai-and-tech-news-roundup-([a-z\-]+)-',
        'weekly-\1-roundup-'),
    updated_at = NOW()
WHERE ci.collection_name = 'roundups'
  AND ci.primary_section_name <> 'all'
  AND ci.slug ~ '^weekly-ai-and-tech-news-roundup-[a-z\-]+-[0-9]{4}-[0-9]{2}-[0-9]{2}$';

UPDATE content_tags_expanded cte
SET slug = regexp_replace(
        cte.slug,
        '^weekly-ai-and-tech-news-roundup-([a-z\-]+)-',
        'weekly-\1-roundup-')
WHERE cte.collection_name = 'roundups'
  AND cte.slug ~ '^weekly-ai-and-tech-news-roundup-[a-z\-]+-[0-9]{4}-[0-9]{2}-[0-9]{2}$';

UPDATE processed_urls pu
SET slug = regexp_replace(
        pu.slug,
        '^weekly-ai-and-tech-news-roundup-([a-z\-]+)-',
        'weekly-\1-roundup-'),
    external_url = '/' || split_part(ltrim(pu.external_url, '/'), '/', 1) || '/roundups/' || regexp_replace(
        pu.slug,
        '^weekly-ai-and-tech-news-roundup-([a-z\-]+)-',
        'weekly-\1-roundup-'),
    updated_at = NOW()
WHERE pu.collection_name = 'roundups'
  AND pu.slug ~ '^weekly-ai-and-tech-news-roundup-[a-z\-]+-[0-9]{4}-[0-9]{2}-[0-9]{2}$'
  AND pu.external_url ~ '^/[a-z\-]+/roundups/';

-- ============================================================
-- 2. Backfill legacy /all roundup rows into real sections
-- ============================================================

WITH section_map AS (
    SELECT * FROM (VALUES
        ('ai', 1),
        ('azure', 2),
        ('dotnet', 4),
        ('devops', 8),
        ('github-copilot', 16),
        ('ml', 32),
        ('security', 64)
    ) AS s(section_name, section_bit)
),
source_roundups AS (
    SELECT ci.*
    FROM content_items ci
    WHERE ci.collection_name = 'roundups'
      AND (ci.primary_section_name = 'all' OR ci.external_url LIKE '/all/roundups/%')
),
expanded AS (
    SELECT
        sr.slug AS source_slug,
        sr.collection_name,
        sr.title,
        sr.content,
        sr.excerpt,
        sr.date_epoch,
        sm.section_name AS primary_section_name,
        CASE
            WHEN sr.slug ~ '^weekly-ai-and-tech-news-roundup-[0-9]{4}-[0-9]{2}-[0-9]{2}$'
                THEN regexp_replace(sr.slug, '^weekly-ai-and-tech-news-roundup-', 'weekly-' || sm.section_name || '-roundup-')
            ELSE 'weekly-' || sm.section_name || '-roundup-' || to_char(to_timestamp(sr.date_epoch), 'YYYY-MM-DD')
        END AS slug,
        sr.author,
        sr.feed_name,
        sr.tags_csv,
        (sm.section_name = 'ai') AS is_ai,
        (sm.section_name = 'azure') AS is_azure,
        (sm.section_name = 'dotnet') AS is_dotnet,
        (sm.section_name = 'devops') AS is_devops,
        (sm.section_name = 'github-copilot') AS is_github_copilot,
        (sm.section_name = 'ml') AS is_ml,
        (sm.section_name = 'security') AS is_security,
        sm.section_bit AS sections_bitmask,
        sr.content_hash,
        sr.ai_metadata,
        sr.created_at,
        sr.updated_at
    FROM source_roundups sr
    CROSS JOIN section_map sm
)
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt, date_epoch,
    primary_section_name, external_url, author, feed_name,
    tags_csv, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, content_hash, ai_metadata,
    created_at, updated_at
)
SELECT
    e.slug,
    e.collection_name,
    e.title,
    e.content,
    e.excerpt,
    e.date_epoch,
    e.primary_section_name,
    '/' || e.primary_section_name || '/roundups/' || e.slug AS external_url,
    e.author,
    e.feed_name,
    e.tags_csv,
    e.is_ai,
    e.is_azure,
    e.is_dotnet,
    e.is_devops,
    e.is_github_copilot,
    e.is_ml,
    e.is_security,
    e.sections_bitmask,
    e.content_hash,
    e.ai_metadata,
    e.created_at,
    e.updated_at
FROM expanded e
ON CONFLICT (collection_name, slug) DO NOTHING;

WITH section_map AS (
    SELECT * FROM (VALUES
        ('ai', 1),
        ('azure', 2),
        ('dotnet', 4),
        ('devops', 8),
        ('github-copilot', 16),
        ('ml', 32),
        ('security', 64)
    ) AS s(section_name, section_bit)
),
source_roundups AS (
    SELECT ci.slug, ci.date_epoch
    FROM content_items ci
    WHERE ci.collection_name = 'roundups'
      AND (ci.primary_section_name = 'all' OR ci.external_url LIKE '/all/roundups/%')
),
expanded AS (
    SELECT
        sr.slug AS source_slug,
        CASE
            WHEN sr.slug ~ '^weekly-ai-and-tech-news-roundup-[0-9]{4}-[0-9]{2}-[0-9]{2}$'
                THEN regexp_replace(sr.slug, '^weekly-ai-and-tech-news-roundup-', 'weekly-' || sm.section_name || '-roundup-')
            ELSE 'weekly-' || sm.section_name || '-roundup-' || to_char(to_timestamp(sr.date_epoch), 'YYYY-MM-DD')
        END AS new_slug,
        sm.section_name,
        sm.section_bit
    FROM source_roundups sr
    CROSS JOIN section_map sm
)
INSERT INTO content_tags_expanded (
    collection_name, slug, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
)
SELECT
    cte.collection_name,
    e.new_slug,
    cte.tag_word,
    cte.tag_display,
    cte.is_full_tag,
    cte.date_epoch,
    (e.section_name = 'ai'),
    (e.section_name = 'azure'),
    (e.section_name = 'dotnet'),
    (e.section_name = 'devops'),
    (e.section_name = 'github-copilot'),
    (e.section_name = 'ml'),
    (e.section_name = 'security'),
    e.section_bit
FROM content_tags_expanded cte
JOIN expanded e ON e.source_slug = cte.slug
WHERE cte.collection_name = 'roundups'
ON CONFLICT DO NOTHING;

INSERT INTO processed_urls (
    external_url, status, feed_name, collection_name, slug, processed_at, updated_at, reason
)
SELECT
    ci.external_url,
    'succeeded',
    ci.feed_name,
    ci.collection_name,
    ci.slug,
    ci.created_at,
    ci.updated_at,
    'roundup-generated'
FROM content_items ci
WHERE ci.collection_name = 'roundups'
  AND ci.primary_section_name <> 'all'
ON CONFLICT (external_url) DO UPDATE SET
    slug = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    feed_name = EXCLUDED.feed_name,
    status = 'succeeded',
    updated_at = NOW();

-- ============================================================
-- 3. Remove legacy /all roundup rows
-- ============================================================

DELETE FROM content_tags_expanded
WHERE collection_name = 'roundups'
  AND slug IN (
      SELECT slug
      FROM content_items
      WHERE collection_name = 'roundups'
        AND (primary_section_name = 'all' OR external_url LIKE '/all/roundups/%')
  );

DELETE FROM processed_urls
WHERE collection_name = 'roundups'
  AND external_url LIKE '/all/roundups/%';

DELETE FROM content_items
WHERE collection_name = 'roundups'
  AND (primary_section_name = 'all' OR external_url LIKE '/all/roundups/%');
