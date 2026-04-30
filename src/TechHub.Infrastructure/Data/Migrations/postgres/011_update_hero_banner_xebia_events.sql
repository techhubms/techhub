-- Migration 011: Update hero banner with Xebia Microsoft events (April/May/June 2026)
-- Date: 2026-04-30
-- Purpose: Replace expired GitHub Copilot Dev Day cards with new Xebia Microsoft events.
--   - AI Is Reshaping Software Delivery (Amsterdam, Antwerp, Zurich) → sections: ["ai"]
--   - Securing the SDLC with GitHub Advanced Security (Webinar)     → sections: ["devops", "security"]
-- The `sections` array (new field) limits which section pages show each card.
-- A null/empty sections array means the card shows on every section with the banner enabled.

UPDATE custom_page_data
SET json_data  = '{
  "label": "Featured Events",
  "findMoreUrl": "",
  "findMoreText": "See all Xebia Microsoft events",
  "cards": [
    {
      "title": "AI Is Reshaping Software Delivery — Amsterdam",
      "description": "Monday, May 18 · 14:00–17:00 CET · GitHub Office, Amsterdam",
      "startDate": "2026-04-30",
      "endDate": "2026-05-18",
      "linkUrl": "https://events.xebia.com/microsoft/ai-reshaping-software-delivery-amsterdam-may-18",
      "linkText": "Register",
      "sections": ["ai"]
    },
    {
      "title": "AI Is Reshaping Software Delivery — Antwerp",
      "description": "Tuesday, May 28 · Xebia HQ, Antwerp",
      "startDate": "2026-04-30",
      "endDate": "2026-05-28",
      "linkUrl": "https://events.xebia.com/microsoft/ai-reshaping-software-delivery-antwerp-may-28",
      "linkText": "Register",
      "sections": ["ai"]
    },
    {
      "title": "AI Is Reshaping Software Delivery — Zurich",
      "description": "Tuesday, June 2 · Microsoft Office, Zurich",
      "startDate": "2026-04-30",
      "endDate": "2026-06-02",
      "linkUrl": "https://events.xebia.com/microsoft/ai-reshaping-software-delivery-zurich-june-2",
      "linkText": "Register",
      "sections": ["ai"]
    },
    {
      "title": "Webinar: Securing the SDLC with GitHub Advanced Security",
      "description": "Thursday, May 7 · 18:00–20:00 CET · Online",
      "startDate": "2026-04-30",
      "endDate": "2026-05-07",
      "linkUrl": "https://events.xebia.com/microsoft/webinar-securing-sdlc-github-advanced-security",
      "linkText": "Register",
      "sections": ["devops", "security"]
    }
  ]
}',
    updated_at = NOW()
WHERE key = 'hero-banner';
