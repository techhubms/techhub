-- Migration 004: Rename roundup section headings
-- Renames "AI" → "Artificial Intelligence" and "ML" → "Machine Learning"
-- in roundup content headings and table-of-contents links.

-- ============================================================
-- 1. Update section headers (## AI → ## Artificial Intelligence, ## ML → ## Machine Learning)
-- ============================================================
-- Uses regexp_replace with multiline mode ('gm') to match exact heading lines only,
-- avoiding partial matches like "## AI Something" or "## Azure AI".

UPDATE content_items
SET content    = regexp_replace(content, E'^## AI$', '## Artificial Intelligence', 'gm'),
    updated_at = NOW()
WHERE collection_name = 'roundups'
  AND content ~ E'^## AI$';

UPDATE content_items
SET content    = regexp_replace(content, E'^## ML$', '## Machine Learning', 'gm'),
    updated_at = NOW()
WHERE collection_name = 'roundups'
  AND content ~ E'^## ML$';

-- ============================================================
-- 2. Update table-of-contents links
-- ============================================================
-- TOC links are generated as "- [AI](#ai)" and "- [ML](#ml)" by RoundupContentBuilder.
-- Replace with full names and updated anchors.

UPDATE content_items
SET content    = REPLACE(content, '[AI](#ai)', '[Artificial Intelligence](#artificial-intelligence)'),
    updated_at = NOW()
WHERE collection_name = 'roundups'
  AND content LIKE '%[AI](#ai)%';

UPDATE content_items
SET content    = REPLACE(content, '[ML](#ml)', '[Machine Learning](#machine-learning)'),
    updated_at = NOW()
WHERE collection_name = 'roundups'
  AND content LIKE '%[ML](#ml)%';
