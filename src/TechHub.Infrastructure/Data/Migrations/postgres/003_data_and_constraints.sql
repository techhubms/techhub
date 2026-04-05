-- Migration 003: Data Migrations and Constraints
-- Runs all data fixes, seeds, and backfills needed before FK/unique constraints
-- can be safely added. Consolidates what were previously migrations 002–023.
-- Must run after 002_feature_tables.sql.

-- ============================================================
-- 1. Fix empty author fields
-- ============================================================
-- A batch of early content items was ingested without author information.
-- Fall back to feed_name, matching the C# ingestion service's fallback chain.

UPDATE content_items
SET author     = feed_name,
    updated_at = NOW()
WHERE author IS NULL OR author = '';

-- ============================================================
-- 2. Seed RSS feed configs
-- ============================================================
-- Canonical feed names are used here directly (duplicates from the original
-- seed — "The GitHub Blog" (two URLs) and "Microsoft Tech Community" (three URLs)
-- — are disambiguated with suffixes from the start).

INSERT INTO rss_feed_configs (name, url, output_dir, enabled, transcript_mandatory) VALUES
    ('The GitHub Blog',                          'https://github.blog/feed/',                                                                          '_news',      TRUE,  FALSE),
    ('The GitHub Blog (Changelog)',              'https://github.blog/changelog/feed/',                                                                '_news',      TRUE,  FALSE),
    ('Microsoft AI Foundry Blog',                'https://devblogs.microsoft.com/foundry/feed/',                                                       '_news',      TRUE,  FALSE),
    ('Microsoft Semantic Kernel Blog',           'https://devblogs.microsoft.com/semantic-kernel/feed/',                                               '_news',      TRUE,  FALSE),
    ('Microsoft AutoGen Blog',                   'https://devblogs.microsoft.com/autogen/feed/',                                                       '_news',      TRUE,  FALSE),
    ('Microsoft News',                           'https://news.microsoft.com/source/feed/',                                                            '_news',      TRUE,  FALSE),
    ('Rob Bos'' Blog',                           'https://devopsjournal.io/blog/atom.xml',                                                             '_blogs',     TRUE,  FALSE),
    ('Hidde de Smet''s Blog',                    'https://hiddedesmet.com/feed.xml',                                                                   '_blogs',     TRUE,  FALSE),
    ('Jesse Houwing''s Blog',                    'https://jessehouwing.net/rss/',                                                                      '_blogs',     TRUE,  FALSE),
    ('Jesse Swart''s Blog',                      'https://blog.jesseswart.nl/index.xml',                                                              '_blogs',     TRUE,  FALSE),
    ('John Savill''s Technical Training',        'https://www.youtube.com/feeds/videos.xml?channel_id=UCpIn7ox7j7bH_OFj7tYouOQ',                     '_videos',    TRUE,  FALSE),
    ('DotNet YouTube',                           'https://www.youtube.com/feeds/videos.xml?channel_id=UCvtT19MZW8dq5Wwfu6B0oxw',                     '_videos',    TRUE,  FALSE),
    ('GitHub YouTube',                           'https://www.youtube.com/feeds/videos.xml?channel_id=UC7c3Kb6jYCRj4JOHHZTxKsQ',                     '_videos',    TRUE,  FALSE),
    ('Learn Microsoft AI Youtube',               'https://www.youtube.com/feeds/videos.xml?channel_id=UCQf_yRJpsfyEiWWpt1MZ6vA',                     '_videos',    TRUE,  FALSE),
    ('Harald Binkle''s blog',                    'https://harrybin.de/rss.xml',                                                                        '_blogs',     TRUE,  FALSE),
    ('Reinier van Maanen''s blog',               'https://r-vm.com/feed.xml',                                                                          '_blogs',     TRUE,  FALSE),
    ('Zure Data & AI Blog',                      'https://zure.com/blog/rss.xml',                                                                      '_blogs',     TRUE,  FALSE),
    ('Arrested DevOps',                          'https://www.arresteddevops.com/episode/index.xml',                                                   '_blogs',     TRUE,  FALSE),
    ('Microsoft DevOps Blog',                    'https://devblogs.microsoft.com/devops/feed/',                                                        '_news',      TRUE,  FALSE),
    ('Microsoft Tech Community',                 'https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure',                '_community', TRUE,  FALSE),
    ('Microsoft Tech Community (.NET)',          'https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=dotnet',               '_community', TRUE,  FALSE),
    ('Microsoft Tech Community (AI)',            'https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=AI',                   '_community', TRUE,  FALSE),
    ('Randy Pagels''s Blog',                     'https://www.cooknwithcopilot.com/rss.xml',                                                           '_blogs',     TRUE,  FALSE),
    ('The Azure Blog',                           'https://azure.microsoft.com/en-us/blog/feed/',                                                       '_news',      TRUE,  FALSE),
    ('Microsoft Blog',                           'https://devblogs.microsoft.com/feed',                                                                '_news',      TRUE,  FALSE),
    ('Microsoft Engineering Blog',               'https://devblogs.microsoft.com/engineering-at-microsoft/feed/',                                      '_news',      TRUE,  FALSE),
    ('Microsoft Azure Notification Hubs Blog',   'https://devblogs.microsoft.com/azure-notification-hubs/feed/',                                       '_news',      TRUE,  FALSE),
    ('Microsoft All Things Azure Blog',          'https://devblogs.microsoft.com/all-things-azure/feed/',                                              '_news',      TRUE,  FALSE),
    ('Microsoft OpenAPI Blog',                   'https://devblogs.microsoft.com/openapi/feed/',                                                       '_news',      TRUE,  FALSE),
    ('Microsoft Azure SDK Blog',                 'https://devblogs.microsoft.com/azure-sdk/feed/',                                                     '_news',      TRUE,  FALSE),
    ('Microsoft Cloud YouTube',                  'https://www.youtube.com/feeds/videos.xml?channel_id=UCSgzRJMqIiCNtoM6Q7Q9Lqw',                     '_videos',    TRUE,  FALSE),
    ('Scott Hanselman''s Blog',                  'https://www.hanselman.com/blog/SyndicationService.asmx/GetRss',                                     '_blogs',     TRUE,  FALSE),
    ('Andrew Lock''s Blog',                      'https://andrewlock.net/rss.xml',                                                                     '_blogs',     TRUE,  FALSE),
    ('Nick Chapsas YouTube',                     'https://www.youtube.com/feeds/videos.xml?channel_id=UCrkPsvLGln62OMZRO6K-llg',                     '_videos',    TRUE,  FALSE),
    ('David Fowler''s Blog',                     'https://medium.com/feed/@davidfowl',                                                                '_blogs',     TRUE,  FALSE),
    ('Steve Gordon''s Blog',                     'https://www.stevejgordon.co.uk/feed',                                                               '_blogs',     TRUE,  FALSE),
    ('Rick Strahl''s Blog',                      'https://feeds.feedburner.com/rickstrahl',                                                           '_blogs',     TRUE,  FALSE),
    ('Khalid Abuhakmeh''s Blog',                 'https://khalidabuhakmeh.com/feed.xml',                                                              '_blogs',     TRUE,  FALSE),
    ('Code Maze Blog',                           'https://code-maze.com/feed/',                                                                        '_blogs',     TRUE,  FALSE),
    ('Microsoft PowerShell Blog',                'https://devblogs.microsoft.com/powershell/feed/',                                                    '_news',      TRUE,  FALSE),
    ('Microsoft TypeScript Blog',                'https://devblogs.microsoft.com/typescript/feed/',                                                    '_news',      TRUE,  FALSE),
    ('Microsoft .NET Blog',                      'https://devblogs.microsoft.com/dotnet/feed/',                                                        '_news',      TRUE,  FALSE),
    ('GitHub Engineering Blog',                  'https://github.blog/engineering/feed/',                                                              '_news',      TRUE,  FALSE),
    ('Thomas Maurer''s Blog',                    'https://www.thomasmaurer.ch/feed/',                                                                  '_blogs',     TRUE,  FALSE),
    ('Microsoft Security Blog',                  'https://www.microsoft.com/en-us/security/blog/feed/',                                                '_news',      TRUE,  FALSE),
    ('Microsoft Fabric Blog',                    'https://blog.fabric.microsoft.com/en-us/blog/feed/',                                                 '_news',      TRUE,  FALSE),
    ('Visual Studio Code YouTube',               'https://www.youtube.com/feeds/videos.xml?channel_id=UCs5Y5_7XK8HLDX0SLNwkd3w',                     '_videos',    TRUE,  FALSE),
    ('Microsoft Developer YouTube',              'https://www.youtube.com/feeds/videos.xml?channel_id=UCsMica-v34Irf9KVTh6xx-g',                     '_videos',    TRUE,  FALSE),
    ('Microsoft Events YouTube',                 'https://www.youtube.com/feeds/videos.xml?channel_id=UCrhJmfAGQ5K81XQ8_od1iTg',                     '_videos',    TRUE,  FALSE),
    ('.NET Foundation''s Blog',                  'https://dotnetfoundation.org/feeds/blog',                                                            '_blogs',     TRUE,  FALSE),
    ('René van Osnabrugge''s Blog',              'https://roadtoalm.com/feed/',                                                                        '_blogs',     TRUE,  FALSE),
    ('Michiel van Oudheusden''s Blog',           'https://mindbyte.nl/feed.xml',                                                                       '_blogs',     TRUE,  FALSE),
    ('Dellenny''s Blog',                         'https://dellenny.com/feed/',                                                                         '_blogs',     TRUE,  FALSE),
    ('DevClass',                                 'https://devclass.com/feed/',                                                                         '_blogs',     TRUE,  FALSE),
    ('Spindev''s Blog',                          'https://spindev.net/index.xml',                                                                      '_blogs',     TRUE,  FALSE),
    ('Visual Studio Code Releases',              'https://code.visualstudio.com/feed.xml',                                                             '_news',      TRUE,  FALSE),
    ('Microsoft VisualStudio Blog',              'https://devblogs.microsoft.com/visualstudio/feed/',                                                  '_news',      TRUE,  FALSE),
    ('Alireza Chegini''s YouTube',               'https://www.youtube.com/feeds/videos.xml?channel_id=UCZSAqzABRmDxDHuPS6YuXZA',                     '_videos',    TRUE,  FALSE),
    ('Emanuele Bartolesi''s Blog',               'https://dev.to/feed/kasuken',                                                                        '_blogs',     TRUE,  FALSE),
    ('Bruno Van Thournout''s Blog',              'https://brunovt.be/rss.xml',                                                                         '_blogs',     TRUE,  FALSE),
    ('Authorised Territory''s YouTube',          'https://www.youtube.com/feeds/videos.xml?channel_id=UCR2VG1Rq7abZ33S1T3XSWNg',                     '_videos',    TRUE,  FALSE),
    ('Fokko at Work YouTube',                    'https://www.youtube.com/feeds/videos.xml?channel_id=UCemYJar_AE5cF_dkCoag02A',                     '_videos',    TRUE,  TRUE),
    -- Disabled legacy feeds — produced historical content, no longer active
    ('Microsoft DevBlog',                        'https://devblogs.microsoft.com/feed/',                                                               '_news',      FALSE, FALSE),
    ('Microsoft Build 2025 YouTube',             'https://www.youtube.com/feeds/videos.xml?playlist_id=PLlrxD0HtieGidlsr4FzmSFpjMU1gOGGAf',         '_videos',    FALSE, FALSE),
    ('TechHub',                                  'urn:techhub:internal',                                                                               '_videos',    FALSE, FALSE)
ON CONFLICT (url) DO NOTHING;

-- ============================================================
-- 3. Fix feed_name mismatches in content_items
-- ============================================================
-- Content was ingested before some feeds were renamed in rss_feed_configs.
-- Update to match the canonical names now in rss_feed_configs.

UPDATE content_items SET feed_name = 'Microsoft DevOps Blog'   WHERE feed_name = 'DevOps Blog';
UPDATE content_items SET feed_name = 'DotNet YouTube'          WHERE feed_name = 'Dotnet''s Youtube channel';
UPDATE content_items SET feed_name = 'Fokko at Work YouTube'   WHERE feed_name = 'Fokko at Work';
UPDATE content_items SET feed_name = 'GitHub YouTube'          WHERE feed_name = 'GitHub''s Youtube channel';
UPDATE content_items SET feed_name = 'Microsoft .NET Blog'     WHERE feed_name = 'dotnet';

-- ============================================================
-- 4. Delete content from removed feeds
-- ============================================================
-- Reddit (all variants), The New Stack, and Manual Test feeds are gone and won't return.
-- Tags for these items must be deleted first (no FK cascade yet at this point).

DELETE FROM content_tags_expanded
WHERE (collection_name, slug) IN (
    SELECT collection_name, slug FROM content_items
    WHERE feed_name LIKE 'Reddit %'
       OR feed_name = 'The New Stack'
       OR feed_name = 'Manual Test'
);

DELETE FROM content_items
WHERE feed_name LIKE 'Reddit %'
   OR feed_name = 'The New Stack'
   OR feed_name = 'Manual Test';

-- ============================================================
-- 5. Fix duplicate external_urls in content_items
-- ============================================================
-- Three early roundup items shared the same external_url template.
-- Make them unique by appending the slug.

UPDATE content_items
SET external_url = '/all/roundups/' || slug
WHERE external_url = '/github-copilot/roundups/Weekly-AI-and-Tech-News-Roundup'
  AND collection_name = 'roundups';

-- ============================================================
-- 6. Lowercase roundup slugs
-- ============================================================
-- RoundupContentBuilder.BuildSlug was generating mixed-case slugs (e.g.
-- "Weekly-AI-and-Tech-News-Roundup-2026-03-30"). Fixed to produce lowercase.
-- Backfill existing rows so URLs resolve correctly after the fix.

UPDATE content_items
SET slug         = LOWER(slug),
    external_url = LOWER(external_url)
WHERE collection_name = 'roundups'
  AND slug <> LOWER(slug);

UPDATE content_tags_expanded
SET slug = LOWER(slug)
WHERE collection_name = 'roundups'
  AND slug <> LOWER(slug);

-- ============================================================
-- 7. Backfill processed_urls from content_items
-- ============================================================
-- Ensure every content_item has a corresponding processed_urls entry so the
-- admin "Processed URLs" page doubles as a universal content browser.

INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, processed_at, updated_at)
SELECT ci.external_url, 'succeeded', ci.feed_name, ci.collection_name, ci.slug, ci.created_at, ci.updated_at
FROM content_items ci
ON CONFLICT (external_url) DO NOTHING;

-- ============================================================
-- 8. Tag YouTube items with has_transcript = FALSE
-- ============================================================
-- Mark all existing YouTube entries so the transcript column is not NULL.
-- New items will have this set by the ingestion pipeline.

UPDATE processed_urls
SET has_transcript = FALSE
WHERE has_transcript IS NULL
  AND (external_url LIKE '%youtube.com%' OR external_url LIKE '%youtu.be%');

-- ============================================================
-- 9. Seed background_job_settings
-- ============================================================

INSERT INTO background_job_settings (job_name, enabled, description) VALUES
    ('ContentProcessor', FALSE, 'RSS content processing pipeline — fetches and processes RSS feed items on a periodic schedule'),
    ('RoundupGenerator',  FALSE, 'Weekly roundup generation — generates weekly content roundups using AI categorization'),
    ('ContentCleanup',    FALSE, 'Content cleanup — normalizes tags, authors, markdown formatting and backfills AI metadata')
ON CONFLICT (job_name) DO NOTHING;

-- ============================================================
-- 10. Add constraints
-- ============================================================
-- All data is correct at this point; constraints can now be enforced safely.

-- Clean up any orphaned tag rows before adding the FK
DELETE FROM content_tags_expanded t
WHERE NOT EXISTS (
    SELECT 1 FROM content_items i
    WHERE i.collection_name = t.collection_name AND i.slug = t.slug
);

-- content_tags_expanded → content_items
ALTER TABLE content_tags_expanded
    ADD CONSTRAINT fk_tags_content_item
    FOREIGN KEY (collection_name, slug)
    REFERENCES content_items (collection_name, slug)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- content_reviews → content_items (already added inline in 002, included here for documentation)

-- UNIQUE name on rss_feed_configs (names are now canonical and deduplicated)
ALTER TABLE rss_feed_configs
    ADD CONSTRAINT uq_rss_feed_configs_name UNIQUE (name);

-- UNIQUE external_url on content_items (duplicates fixed in step 5)
ALTER TABLE content_items
    ADD CONSTRAINT uq_content_items_external_url UNIQUE (external_url);

-- content_items.feed_name → rss_feed_configs.name
-- ON UPDATE CASCADE: feed renames in admin UI cascade to all content
-- ON DELETE RESTRICT: prevent deleting a feed that still has content
ALTER TABLE content_items
    ADD CONSTRAINT fk_content_items_feed
    FOREIGN KEY (feed_name)
    REFERENCES rss_feed_configs (name)
    ON UPDATE CASCADE ON DELETE RESTRICT;

-- processed_urls.feed_name → rss_feed_configs.name (nullable)
-- ON DELETE RESTRICT: feed deletion is blocked while any processed_url still references it.
-- This includes skipped/failed items that have no content_item. Those must be cleaned up
-- explicitly before the feed can be removed.
ALTER TABLE processed_urls
    ADD CONSTRAINT fk_processed_urls_feed
    FOREIGN KEY (feed_name)
    REFERENCES rss_feed_configs (name)
    ON UPDATE CASCADE ON DELETE RESTRICT;

-- processed_urls.job_id → content_processing_jobs.id
ALTER TABLE processed_urls
    ADD CONSTRAINT fk_processed_urls_job
    FOREIGN KEY (job_id)
    REFERENCES content_processing_jobs (id)
    ON DELETE SET NULL;

-- processed_urls → content_items (via collection_name + slug)
-- ON UPDATE CASCADE: slug renames in content_items propagate automatically
ALTER TABLE processed_urls
    ADD CONSTRAINT fk_processed_urls_content_item
    FOREIGN KEY (collection_name, slug)
    REFERENCES content_items (collection_name, slug)
    ON DELETE CASCADE ON UPDATE CASCADE;
