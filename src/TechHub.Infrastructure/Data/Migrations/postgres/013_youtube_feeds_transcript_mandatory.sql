-- Migration 013: Make transcripts mandatory for all YouTube feeds
-- YouTube feeds are identified by their URL pattern (youtube.com/feeds/videos.xml).
-- Sets transcript_mandatory = TRUE so the content pipeline treats a missing transcript
-- as a hard failure rather than silently skipping it.

UPDATE rss_feed_configs
SET transcript_mandatory = TRUE
WHERE url LIKE '%youtube.com/feeds/videos.xml%';
