-- ai_metadata is for source content articles (news/blogs/videos) only.
-- Roundup items never had a meaningful consumer for this field — it was written
-- during roundup generation but never read back by any rendering or query logic.
-- Clear it from all existing roundup rows and stop writing it going forward.

UPDATE content_items
SET    ai_metadata = NULL,
       updated_at  = NOW()
WHERE  collection_name = 'roundups';
