-- ============================================================
-- 058: Open the timeline to everyone
-- 051 introduced the timeline behind a feature toggle that started as admins only. The timeline is ready, so it is now
-- open to everyone. An admin can still change it on the feature toggles page.
-- Running this again changes nothing visible: the mode is already 'everyone'.
-- ============================================================

UPDATE feature_toggles
SET mode = 'everyone',
    updated_at = NOW()
WHERE feature_key = 'timeline'
  AND mode <> 'everyone';
