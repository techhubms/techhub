-- Migration 003: Section Roundup Items (SUPERSEDED)
-- This migration originally created the section_roundup_items table.
-- That table has been removed — roundup generation now queries content_items directly
-- using section boolean columns and ai_metadata.
-- The table is dropped here so fresh databases never create it.

DROP TABLE IF EXISTS section_roundup_items;
