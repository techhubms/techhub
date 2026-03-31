-- Migration 012: Add foreign key from content_tags_expanded to content_items
-- Ensures referential integrity and cascades deletes to prevent orphaned tag rows.

-- Clean up any orphaned tag rows that reference non-existent content items
DELETE FROM content_tags_expanded t
WHERE NOT EXISTS (
    SELECT 1 FROM content_items i
    WHERE i.collection_name = t.collection_name AND i.slug = t.slug
);

ALTER TABLE content_tags_expanded
    ADD CONSTRAINT fk_tags_content_item
    FOREIGN KEY (collection_name, slug)
    REFERENCES content_items (collection_name, slug)
    ON DELETE CASCADE;
