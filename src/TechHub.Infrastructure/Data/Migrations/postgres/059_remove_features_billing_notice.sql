-- ============================================================
-- 059: Remove the billing notice from the GitHub Copilot features page
-- Migration 027 added a billingNotice (the switch to usage-based billing on June 1, 2026) to the custom page data of the
-- features page. The notice is no longer shown and the page no longer reads it, so it is removed from the stored JSON.
-- Running this again changes nothing: the key is already gone.
-- ============================================================

UPDATE custom_page_data
SET json_data = jsonb_pretty(json_data::jsonb - 'billingNotice'),
    updated_at = NOW()
WHERE key = 'githubcopilot-features'
  AND json_data::jsonb ? 'billingNotice';
