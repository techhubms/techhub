-- Migration 041: Backfill in-content TOC for per-section roundups
--
-- Migrations 028-034 inserted per-section roundups without an in-content
-- table of contents block after the introduction excerpt marker.
--
-- This migration programmatically rebuilds a markdown TOC from existing
-- ## / ### headings and injects it directly after <!--excerpt_end-->.
--
-- Safe to re-run: skips rows that already include the TOC heading.

WITH targets AS (
    SELECT
        ci.collection_name,
        ci.slug,
        ci.content,
        split_part(ci.content, '<!--excerpt_end-->', 2) AS body_after_intro
    FROM content_items ci
    WHERE ci.collection_name = 'roundups'
      AND ci.primary_section_name <> 'all'
      AND ci.content LIKE '%<!--excerpt_end-->%'
      AND ci.content NOT LIKE '%## This Week''s Overview%'
),
lines AS (
    SELECT
        t.collection_name,
        t.slug,
        l.ordinality,
        l.line
    FROM targets t
    CROSS JOIN LATERAL regexp_split_to_table(t.body_after_intro, E'\n') WITH ORDINALITY AS l(line, ordinality)
),
toc_entries AS (
    SELECT
        l.collection_name,
        l.slug,
        l.ordinality,
        CASE
            WHEN l.line LIKE '## %' THEN
                '- [' || trim(substr(l.line, 4)) || '](#' ||
                lower(regexp_replace(regexp_replace(trim(substr(l.line, 4)), '[^[:alnum:] .-]', '', 'g'), '[^[:alnum:].]', '-', 'g'))
                || ')'
            WHEN l.line LIKE '### %' THEN
                '  - [' || trim(substr(l.line, 5)) || '](#' ||
                lower(regexp_replace(regexp_replace(trim(substr(l.line, 5)), '[^[:alnum:] .-]', '', 'g'), '[^[:alnum:].]', '-', 'g'))
                || ')'
        END AS toc_line
    FROM lines l
    WHERE l.line LIKE '## %' OR l.line LIKE '### %'
),
built_toc AS (
    SELECT
        te.collection_name,
        te.slug,
        string_agg(te.toc_line, E'\n' ORDER BY te.ordinality) AS toc_text
    FROM toc_entries te
    GROUP BY te.collection_name, te.slug
)
UPDATE content_items ci
SET content = replace(
        ci.content,
        '<!--excerpt_end-->',
        E'<!--excerpt_end-->\n\n## This Week''s Overview\n\n' || bt.toc_text || E'\n\n'
    ),
    updated_at = NOW()
FROM built_toc bt
WHERE ci.collection_name = bt.collection_name
  AND ci.slug = bt.slug
  AND bt.toc_text IS NOT NULL
  AND bt.toc_text <> '';
