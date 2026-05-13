-- Migration 027: Add billing notice to features page and fix tier inaccuracies
-- Date: 2026-05-13
-- Purpose:
--   1. Add a billingNotice field to the githubcopilot-features custom page data
--      announcing the June 2026 switch to usage-based (token-based) billing.
--
--   2. Fix inaccurate model access claims in subscription tier descriptions:
--      - Student tier: remove "Claude Sonnet 4/4.5" — Claude Sonnet 4.5/4.6 is
--        not available on Student (Pro and above only per official docs).
--      - Pro+ tier: update "Claude Opus 4.5/4.6" → "Claude Opus 4.7" — Pro+ has
--        Claude Opus 4.7, while 4.5/4.6 are in Business/Enterprise only.
--
--   3. Add three billing-related reference links to the token-based-billing video
--      content item, right after the embedded YouTube player.
--
-- Subscription tier array indices (githubcopilot-features JSON):
--   0 = Free, 1 = Student, 2 = Pro, 3 = Business, 4 = Pro+, 5 = Enterprise
--
-- Student features array index 4:
--   "Access to code review, Claude Sonnet 4/4.5, GPT-5, Gemini 2.5 Pro, and more"
--   → "Access to code review, Gemini 2.5 Pro, GPT-5.2, and more"
--
-- Pro+ features array index 1:
--   "Access to all models, including Claude Opus 4.5/4.6 and more"
--   → "Access to all models, including Claude Opus 4.7 and more"

-- ============================================================
-- Step 1: Update githubcopilot-features custom page JSON
-- ============================================================

UPDATE custom_page_data
SET
    json_data  = (
        jsonb_set(
            jsonb_set(
                json_data::jsonb,
                '{subscriptionTiers,1,features,4}',
                '"Access to code review, Gemini 2.5 Pro, GPT-5.2, and more"'
            ),
            '{subscriptionTiers,4,features,1}',
            '"Access to all models, including Claude Opus 4.7 and more"'
        ) || '{
            "billingNotice": {
                "text": "Starting June 1, 2026, GitHub Copilot is switching from request-based to usage-based (token-based) billing.",
                "links": [
                    {
                        "label": "Rob Bos: From premium request units to AI credits and tokens",
                        "url": "/github-copilot/videos/github-copilot-token-based-billing"
                    },
                    {
                        "label": "Jesse Houwing: The why for Usage Based Billing for GitHub Copilot",
                        "url": "https://jessehouwing.net/usage-based-billing-for-github-copilot/"
                    },
                    {
                        "label": "GitHub Docs: Usage-based billing for individuals",
                        "url": "https://docs.github.com/en/copilot/concepts/billing/usage-based-billing-for-individuals"
                    },
                    {
                        "label": "GitHub Docs: Usage-based billing for organizations and enterprises",
                        "url": "https://docs.github.com/en/copilot/concepts/billing/usage-based-billing-for-organizations-and-enterprises"
                    }
                ]
            }
        }'::jsonb
    )::text,
    updated_at = NOW()
WHERE key = 'githubcopilot-features';

-- ============================================================
-- Step 2: Add billing reference links to the video content item
-- ============================================================
-- Inserts three links directly after the {% youtube %} embed,
-- before the "## Full summary based on transcript" section.
-- Note: content_hash is intentionally left stale — this item is
-- manually curated and not re-processed by the RSS pipeline.

UPDATE content_items
SET content = replace(
    content,
    E'{% youtube jpNXTur13fg %}\n\n## Full summary based on transcript',
    E'{% youtube jpNXTur13fg %}\n\n**Learn more about the new billing model:**\n\n- [Rob Bos: From premium request units to AI credits and tokens](/github-copilot/videos/github-copilot-token-based-billing)\n- [Jesse Houwing: The why for Usage Based Billing for GitHub Copilot](https://jessehouwing.net/usage-based-billing-for-github-copilot/)\n- [GitHub Docs: Usage-based billing for individuals](https://docs.github.com/en/copilot/concepts/billing/usage-based-billing-for-individuals)\n- [GitHub Docs: Usage-based billing for organizations and enterprises](https://docs.github.com/en/copilot/concepts/billing/usage-based-billing-for-organizations-and-enterprises)\n\n## Full summary based on transcript'
)
WHERE slug            = 'github-copilot-token-based-billing'
  AND collection_name = 'videos';
