-- Migration 020: Fix GHC feature release dates
-- Date: 2026-05-12
-- Purpose: Correct ghc_features.release_date values that were incorrectly inherited
--          from content_items.date_epoch (video publication date) in migration 016.
--          Many "wrapper" content items had scheduled future dates (2026-08-29 for a
--          batch published on that day, 2026-01-01 as a placeholder batch, and
--          2026-04-28 for token-based billing).
--
--          This migration sets each feature's release_date to the actual GitHub
--          Copilot feature launch date at month granularity, using midnight UTC on
--          the 1st of the month. Dates are best-effort based on the official GitHub
--          Changelog, GitHub Blog announcements, and Build/Universe keynotes.
--
-- Note: EXTRACT(EPOCH FROM TIMESTAMPTZ '...') returns seconds since Unix epoch.

-- ============================================================
-- 1. Features that had a scheduled batch date of 2026-08-29
--    (epoch 1787996371) — these features have been available
--    since 2022–2025, far predating that scheduled date.
-- ============================================================

-- Code referencing / code attribution feature: available at Copilot GA (Nov 2022)
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2022-11-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'code-referencing';

-- Copilot integration in the GitHub.com web UI (PR summaries, chat): June 2023
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2023-06-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'integration-in-web-ui';

-- Copilot explains failed GitHub Actions jobs: GitHub Universe October 2023
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2023-10-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'explain-failed-action-jobs';

-- Copilot Chat slash commands (/explain, /fix, /tests, …): Chat GA December 2023
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2023-12-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'slash-commands';

-- Copilot in-editor code debugging assistance: Copilot Chat GA December 2023
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2023-12-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'code-debugging';

-- Copilot quick-fix / code-fix suggestions: Copilot Chat GA December 2023
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2023-12-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'code-fixing';

-- Bing web search in Copilot Chat (VS Code preview): March 2024
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2024-03-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'web-search-with-bing';

-- Copilot test generation for Playwright: early 2025
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2025-01-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'playwright-test-generation';

-- ============================================================
-- 2. Features that had a placeholder date of 2026-01-01
--    (epoch 1767225600) — all assigned the same batch date.
-- ============================================================

-- Data no longer used for training by default (Copilot Business policy): May 2023
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2023-05-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'data-excluded-from-training-by-default';

-- Copilot user management REST API for organizations: September 2023
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2023-09-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'user-management';

-- Smart actions (AI-powered hover/code actions in VS Code): early 2024
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2024-01-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'smart-actions';

-- Copilot Enterprise: chat with knowledge bases across repos: GA February 2024
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2024-02-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'chat-with-knowledge-bases';

-- Copilot Enterprise: chat with your pull request: GA February 2024
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2024-02-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'chat-with-your-pull-request';

-- Copilot Enterprise: multi-repository knowledge base context: GA February 2024
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2024-02-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'multi-repository-context';

-- Copilot Autofix for code scanning alerts: public beta March 2024
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2024-03-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'code-scanning-ai-autofix';

-- Copilot usage metrics API: April 2024
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2024-04-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'copilot-metrics-api';

-- Copilot-generated security advisory summaries: June 2024
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2024-06-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'security-advisory-summaries';

-- Path-specific custom instructions (per-directory .github/copilot-instructions):
-- GitHub Universe October 2024
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2024-10-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'path-specific-custom-instructions';

-- Copilot fine-tuned models (organization-trained): GitHub Universe October 2024
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2024-10-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'fine-tuned-models';

-- Copilot Extensions marketplace GA: November 2024
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2024-11-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'copilot-extensions-marketplace';

-- Copilot Spaces (persistent context for multi-session work): preview March 2025
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2025-03-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'copilot-spaces';

-- Set coding guidelines for Copilot code review: Code Review GA April 2025
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2025-04-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'set-coding-guidelines-for-code-review';

-- Copilot coding agent assigns issues in pull requests: Build 2025, May 2025
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2025-05-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'copilot-coding-agent-in-pull-requests';

-- GitHub Spark: GitHub Universe October 2025
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2025-10-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'github-spark';

-- Organization-wide custom instructions GA: April 2, 2026
UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2026-04-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'organization-wide-custom-instructions';

-- ============================================================
-- 3. Token-based billing (had wrong date 2026-04-28)
--    Effective June 1, 2026.
-- ============================================================

UPDATE ghc_features
SET release_date = EXTRACT(EPOCH FROM TIMESTAMPTZ '2026-06-01 00:00:00 UTC')::BIGINT,
    updated_at   = NOW()
WHERE slug = 'github-copilot-token-based-billing';
