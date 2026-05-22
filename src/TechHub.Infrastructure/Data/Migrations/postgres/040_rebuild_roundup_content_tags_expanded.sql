-- Migration 040: Rebuild content_tags_expanded for per-section roundups
--
-- Migrations 028-034 inserted per-section roundups into content_items but did
-- NOT populate content_tags_expanded. Without expanded tags, the tag cloud
-- shows count=1 (or 0) for all tags when filtering to roundups.
--
-- This migration parses tags_csv from content_items and inserts both
-- full-tag rows and word-expansion rows, matching the logic in
-- ContentItemWriteRepository.BuildTagWords().
--
-- Safe to re-run: uses ON CONFLICT DO NOTHING.

-- Step 1: Insert full-tag rows (is_full_tag = true)
INSERT INTO content_tags_expanded (
    collection_name, slug, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
)
SELECT
    ci.collection_name,
    ci.slug,
    LOWER(TRIM(tag)) AS tag_word,
    TRIM(tag) AS tag_display,
    TRUE AS is_full_tag,
    ci.date_epoch,
    ci.is_ai,
    ci.is_azure,
    ci.is_dotnet,
    ci.is_devops,
    ci.is_github_copilot,
    ci.is_ml,
    ci.is_security,
    ci.sections_bitmask
FROM content_items ci,
     LATERAL regexp_split_to_table(ci.tags_csv, ',') AS tag
WHERE ci.collection_name = 'roundups'
  AND ci.primary_section_name != 'all'
  AND TRIM(tag) != ''
  AND NOT EXISTS (
      SELECT 1 FROM content_tags_expanded cte
      WHERE cte.collection_name = ci.collection_name
        AND cte.slug = ci.slug
        AND cte.tag_word = LOWER(TRIM(tag))
  )
ON CONFLICT DO NOTHING;

-- Step 2: Insert word-expansion rows (is_full_tag = false)
-- Only for multi-word tags (tags containing space, hyphen, or underscore)
INSERT INTO content_tags_expanded (
    collection_name, slug, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
)
SELECT DISTINCT ON (ci.collection_name, ci.slug, LOWER(word))
    ci.collection_name,
    ci.slug,
    LOWER(word) AS tag_word,
    word AS tag_display,
    FALSE AS is_full_tag,
    ci.date_epoch,
    ci.is_ai,
    ci.is_azure,
    ci.is_dotnet,
    ci.is_devops,
    ci.is_github_copilot,
    ci.is_ml,
    ci.is_security,
    ci.sections_bitmask
FROM content_items ci,
     LATERAL regexp_split_to_table(ci.tags_csv, ',') AS tag,
     LATERAL regexp_split_to_table(TRIM(tag), '[ \-_]+') AS word
WHERE ci.collection_name = 'roundups'
  AND ci.primary_section_name != 'all'
  AND TRIM(tag) != ''
  AND word != ''
  -- Only expand multi-word tags
  AND TRIM(tag) ~ '[ \-_]'
  -- Don't insert if same word already exists as full tag for this item
  AND NOT EXISTS (
      SELECT 1 FROM content_tags_expanded cte
      WHERE cte.collection_name = ci.collection_name
        AND cte.slug = ci.slug
        AND cte.tag_word = LOWER(word)
  )
ON CONFLICT DO NOTHING;
