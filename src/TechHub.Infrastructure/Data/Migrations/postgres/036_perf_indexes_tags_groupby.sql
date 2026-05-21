-- Migration 036: Add GroupAggregate-friendly indexes for tag cloud queries.
--
-- The tag cloud query groups by tag_word and uses:
--   HAVING MAX(CASE WHEN is_full_tag THEN 1 ELSE 0 END) = 1
--
-- is_full_tag must stay in HAVING (not WHERE) because COUNT(*) must count all rows
-- (full tags + word-expansions) to match item filter click-through counts. Moving it
-- to WHERE would break that invariant.
--
-- These indexes include is_full_tag AS A COLUMN (not a predicate). This lets PostgreSQL
-- use a GroupAggregate scan on the sorted index to efficiently compute:
--   MAX(CASE WHEN is_full_tag THEN 1 ELSE 0 END) = 1   (HAVING filter)
--   MAX(tag_display)                                    (SELECT expression)
--   COUNT(*)                                            (row count in group)
-- ...without a full table hash-aggregate.
--
-- Bitmask values:
--   AI=1, Azure=2, .NET=4, DevOps=8, GitHub Copilot=16, ML=32, Security=64

-- Full-table GroupAggregate index for all-sections tag cloud queries
-- (tag_word for GROUP BY sort, is_full_tag for HAVING MAX computation, tag_display for SELECT MAX)
CREATE INDEX IF NOT EXISTS idx_tags_groupby ON content_tags_expanded(
    tag_word, is_full_tag, tag_display
);

-- Per-section GroupAggregate partial indexes
-- Each covers only rows matching that section (~14% of rows), enabling very efficient
-- GroupAggregate scans for section-specific tag cloud queries.

CREATE INDEX IF NOT EXISTS idx_tags_groupby_ai ON content_tags_expanded(
    tag_word, is_full_tag, tag_display
) WHERE (sections_bitmask & 1) > 0;

CREATE INDEX IF NOT EXISTS idx_tags_groupby_azure ON content_tags_expanded(
    tag_word, is_full_tag, tag_display
) WHERE (sections_bitmask & 2) > 0;

CREATE INDEX IF NOT EXISTS idx_tags_groupby_dotnet ON content_tags_expanded(
    tag_word, is_full_tag, tag_display
) WHERE (sections_bitmask & 4) > 0;

CREATE INDEX IF NOT EXISTS idx_tags_groupby_devops ON content_tags_expanded(
    tag_word, is_full_tag, tag_display
) WHERE (sections_bitmask & 8) > 0;

CREATE INDEX IF NOT EXISTS idx_tags_groupby_github_copilot ON content_tags_expanded(
    tag_word, is_full_tag, tag_display
) WHERE (sections_bitmask & 16) > 0;

CREATE INDEX IF NOT EXISTS idx_tags_groupby_ml ON content_tags_expanded(
    tag_word, is_full_tag, tag_display
) WHERE (sections_bitmask & 32) > 0;

CREATE INDEX IF NOT EXISTS idx_tags_groupby_security ON content_tags_expanded(
    tag_word, is_full_tag, tag_display
) WHERE (sections_bitmask & 64) > 0;

ANALYZE content_tags_expanded;
