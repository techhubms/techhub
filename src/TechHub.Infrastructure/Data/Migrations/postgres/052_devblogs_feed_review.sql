-- ============================================================
-- 052: Feed review
-- - Adds the devblogs.microsoft.com blogs and Tech Community boards that cover Tech Hub topics and were not yet ingested
-- - Disables feeds that no longer exist as a separate source
-- A feed is only added when no row has its url or its name yet: both are unique, and a feed that was added or renamed
-- in the admin (for example the Tech Community AI feed that became the AI Foundry one) must not be added a second time.
-- ============================================================

WITH new_feeds (name, url, output_dir) AS (
    VALUES
        ('Microsoft Aspire Blog',                     'https://devblogs.microsoft.com/aspire/feed/',             '_news'),
        ('Microsoft ASP.NET Blog',                    'https://devblogs.microsoft.com/aspnet/feed/',             '_news'),
        ('Microsoft Azure Cosmos DB Blog',            'https://devblogs.microsoft.com/cosmosdb/feed/',           '_news'),
        ('Microsoft Azure SQL Blog',                  'https://devblogs.microsoft.com/azure-sql/feed/',          '_news'),
        ('Microsoft Azure DocumentDB Blog',           'https://devblogs.microsoft.com/documentdb/feed/',         '_news'),
        ('Microsoft Java Blog',                       'https://devblogs.microsoft.com/java/feed/',               '_news'),
        ('Microsoft Python Blog',                     'https://devblogs.microsoft.com/python/feed/',             '_news'),
        ('Microsoft ISE Developer Blog',              'https://devblogs.microsoft.com/ise/feed/',                '_news'),
        ('Microsoft Entra Identity Platform Blog',    'https://devblogs.microsoft.com/identity/feed/',           '_news'),
        ('Microsoft Foundry on Windows Blog',         'https://devblogs.microsoft.com/foundry-on-windows/feed/', '_news'),
        ('Microsoft Xcode Blog',                      'https://devblogs.microsoft.com/xcode/feed/',              '_news'),
        ('Microsoft Tech Community (AI Foundry)',     'https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=azure-ai-foundry', '_community'),
        ('Microsoft Tech Community (Copilot Studio)', 'https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/board?board.id=copilot-studio',    '_community'),
        ('Microsoft Tech Community (Data)',           'https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=AzureDatabases',  '_community')
)
INSERT INTO rss_feed_configs (name, url, output_dir, enabled)
SELECT n.name, n.url, n.output_dir, TRUE
FROM new_feeds n
WHERE NOT EXISTS (SELECT 1 FROM rss_feed_configs f WHERE f.url = n.url OR f.name = n.name);

-- The Semantic Kernel blog now redirects to the Agent Framework blog and its feed repeats those posts.
-- The Tech Community AI category was deleted: its feed answers "Resource Not Found" with no items.
-- Matched on url, so a row that was already changed to another feed in the admin is left alone.
UPDATE rss_feed_configs
SET enabled    = FALSE,
    updated_at = NOW()
WHERE url IN (
    'https://devblogs.microsoft.com/semantic-kernel/feed/',
    'https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=AI'
)
  AND enabled = TRUE;
